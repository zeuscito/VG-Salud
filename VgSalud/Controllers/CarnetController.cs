using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using VgSalud.Models;

namespace VgSalud.Controllers
{
    public class CarnetController : Controller
    {
        // GET: Carnet
        public ActionResult Index()
        {
            return View();
        }

        string cadena = ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString;
        SqlConnection db;
        SqlCommand cmd;


        /*
         llamando a otros controladores
       
        */
        TipoTarifaController tt = new TipoTarifaController();
        EspecialidadController es = new EspecialidadController();
        ServiciosController ser = new ServiciosController();
        MedicosController med = new MedicosController();
        TarifarioController tar = new TarifarioController();
        AtencionVariasController aten = new AtencionVariasController();
        CitasController cit = new CitasController();
        /*
        
        lista de los tipos de carnet
        
        */

        public ActionResult Vista_Cabecera_Lab()
        {
            ViewBag.lista = Get_Laboratorio_LLamado();
            return View();
        }

        public List<E_Tipo_Carnet> listaTipoCarnet()
        {
            List<E_Tipo_Carnet> listatipo = new List<E_Tipo_Carnet>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_lista_tipo_carnet", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_Tipo_Carnet tipo = new E_Tipo_Carnet();
                        tipo.TipoCarnet = int.Parse(dr["TipoCarnet"].ToString());
                        tipo.DescCarnet = dr["DescCarnet"].ToString();
                        listatipo.Add(tipo);

                    }


                }
            }
            return listatipo;
        }

        public List<E_Carnet_Sanitario> Get_Laboratorio_LLamado()
        {
            List<E_Carnet_Sanitario> listatipo = new List<E_Carnet_Sanitario>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_Get_Buscar_Laboratorio", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        //p.Historia , p.NomPac , p.NumDoc
                        E_Carnet_Sanitario car = new E_Carnet_Sanitario();
                        car.Historia = int.Parse(dr["Historia"].ToString());
                        car.NombrePaciente = dr["NomPac"].ToString();
                        car.NroDoc = dr["NumDoc"].ToString();
                        listatipo.Add(car);

                    }

                }
            }
            return listatipo;
        }

        //        Usp_En_Espera 
        //as
        //select c.CodCue , p.Historia , t.DescCarnet , p.NomPac , p.NumDoc
        public List<E_Carnet_Sanitario> Lista_En_Espera()
        {
            List<E_Carnet_Sanitario> listatipo = new List<E_Carnet_Sanitario>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_En_Espera", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_Carnet_Sanitario car = new E_Carnet_Sanitario();
                        car.CodCue = int.Parse(dr["CodCue"].ToString());
                        car.Historia = int.Parse(dr["Historia"].ToString());
                        car.DesCarnet = dr["DescCarnet"].ToString();
                        car.NombrePaciente = dr["NomPac"].ToString();
                        car.NroDoc = dr["NumDoc"].ToString();
                        listatipo.Add(car);

                    }


                }
            }
            return listatipo;
        }

        public ActionResult RegistroCarnet(int historia, E_Carnet_Sanitario car)
        {
            car.Historia = historia;

            string sede = Session["CodSede"].ToString();

            ViewBag.listadoTari = new SelectList(cit.ListadoTarifaAtencion().Where(x => x.CodEspec == "ES005" && x.CodSede == sede), "CodTar", "DescTar");

            //ViewBag.listadoTipCarnet = new SelectList(listaTipoCarnet().ToList(), "TipoCarnet", "DescCarnet");


            return View(car);
        }


        [HttpPost]
        public ActionResult RegistroCarnet(E_Carnet_Sanitario car)
        {

            string sede = Session["CodSede"].ToString();

            ViewBag.listadoTari = new SelectList(cit.ListadoTarifaAtencion().Where(x => x.CodEspec == "ES005" && x.CodSede == sede), "CodTar", "DescTar");

            var util = new DatosGeneralesController().listadatogenerales().FirstOrDefault();
            var codusu = Session["UserID"];
            var medico = new MedicosController().ListadoMedico().Where(x => x.CodUsu == codusu.ToString()).FirstOrDefault();

            int codigoCuenta = 0;
            decimal precio = 0, igv = 0, total = 0;

            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();

            var pacientes = new PacientesController().ListadoPacientes().Where(x => x.Historia == car.Historia).FirstOrDefault();

            string Crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
            string usuario = Session["UserID"].ToString();

            int Resu = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                    con.Open();
                    SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                    using (SqlCommand da = new SqlCommand("Usp_MtoCuentas", con, tr))
                    {
                        try
                        {
                            da.CommandType = CommandType.StoredProcedure;

                            if (car.CodCue == 0)
                            {

                                da.Parameters.AddWithValue("@CodCue", "");
                                da.Parameters.AddWithValue("@CodSede", sede);
                                da.Parameters.AddWithValue("@Historia", car.Historia);
                                da.Parameters.AddWithValue("@CodcatPac", pacientes.CodCatPac);
                                da.Parameters.AddWithValue("@STotCue", car.precio);
                                da.Parameters.AddWithValue("@IgvCue", car.igv);
                                da.Parameters.AddWithValue("@TotCue", car.total);
                                da.Parameters.AddWithValue("@FecCrea", "");
                                da.Parameters.AddWithValue("@FecAnul", "");
                                da.Parameters.AddWithValue("@EstCue", 1);
                                da.Parameters.AddWithValue("@EstGene", "");
                                da.Parameters.AddWithValue("@SecFact", "");
                                da.Parameters.AddWithValue("@Usuario", usuario);
                                da.Parameters.AddWithValue("@UsuarioAnula", "");
                                da.Parameters.AddWithValue("@Crea", Crea);
                                da.Parameters.AddWithValue("@Modifica", "");
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", "1");
                            }
                            else
                            {


                                precio = 0;
                                igv = 0;
                                total = 0;

                                da.Parameters.AddWithValue("@CodCue", car.CodCue);
                                da.Parameters.AddWithValue("@CodSede", "");
                                da.Parameters.AddWithValue("@Historia", "");
                                da.Parameters.AddWithValue("@CodcatPac", "");
                                da.Parameters.AddWithValue("@STotCue", precio);
                                da.Parameters.AddWithValue("@IgvCue", igv);
                                da.Parameters.AddWithValue("@TotCue", total);
                                da.Parameters.AddWithValue("@FecCrea", "");
                                da.Parameters.AddWithValue("@FecAnul", "");
                                da.Parameters.AddWithValue("@EstCue", "1");
                                da.Parameters.AddWithValue("@EstGene", "");
                                da.Parameters.AddWithValue("@SecFact", "");
                                da.Parameters.AddWithValue("@Usuario", "");
                                da.Parameters.AddWithValue("@UsuarioAnula", "");
                                da.Parameters.AddWithValue("@Crea", "");
                                da.Parameters.AddWithValue("@Modifica", Crea);
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", "2");

                            }

                            Resu = (int)da.ExecuteScalar();


                            using (SqlCommand dd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr))
                            {
                                dd.CommandType = CommandType.StoredProcedure;
                                dd.Parameters.AddWithValue("@Procedencia", 1);
                                dd.Parameters.AddWithValue("@CodCue", Resu);
                                if (car.CodCue == 0)
                                {
                                    dd.Parameters.AddWithValue("@Item", 1);
                                }
                                else
                                {
                                    dd.Parameters.AddWithValue("@Item", 1);
                                }
                                dd.Parameters.AddWithValue("@Tarifa", car.CodTar);
                                dd.Parameters.AddWithValue("@CodProce", 3);
                                dd.Parameters.AddWithValue("@CodDetalleP", 3);
                                dd.Parameters.AddWithValue("@CodSede", sede);
                                dd.Parameters.AddWithValue("@Cantidad", 1);
                                dd.Parameters.AddWithValue("@precioUni", car.total);
                                dd.Parameters.AddWithValue("@precio", car.precio);
                                dd.Parameters.AddWithValue("@igv", car.igv);
                                dd.Parameters.AddWithValue("@total", car.total);
                                dd.Parameters.AddWithValue("@EstDet", "1");
                                dd.Parameters.AddWithValue("@FechaAten", "");
                                dd.Parameters.AddWithValue("@TurnoAten", "");
                                dd.Parameters.AddWithValue("@RegMedico", medico.CodMed);
                                dd.Parameters.AddWithValue("@MedicoEnvia", medico.CodMed);
                                dd.Parameters.AddWithValue("@Crea", Crea);
                                dd.Parameters.AddWithValue("@Modifica", "");
                                dd.Parameters.AddWithValue("@Elimina", "");
                                dd.Parameters.AddWithValue("@Evento", "1");

                                dd.ExecuteNonQuery();

                                tr.Commit();
                                ViewBag.historia = car.Historia;
                                ViewBag.cuenta = Resu;
                            }

                        }
                        catch (Exception e)
                        {
                            tr.Rollback();

                        }
                    }
                }


                try
                {
                    int codCarnet = 0;
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                        using (SqlCommand cmd = new SqlCommand("Usp_insert_carnet_sanitario", con, tr))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Manipulador", car.Manipulador);
                            cmd.Parameters.AddWithValue("@TipoCarnet", car.TipoCarnet);
                            cmd.Parameters.AddWithValue("@Campana", car.Campana);
                            cmd.Parameters.AddWithValue("@FecRecojo", "");
                            cmd.Parameters.AddWithValue("@EstadoCarnet", car.EstadoCarnet);
                            if(car.Empresa!=null)
                            {
                                cmd.Parameters.AddWithValue("@Empresa", car.Empresa);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Empresa", "");
                            }
                            
                            cmd.Parameters.AddWithValue("@FotoCarnet", car.FotoCarnet);
                            cmd.Parameters.AddWithValue("@QR", "");
                            cmd.Parameters.AddWithValue("@FecVencimiento", "");
                            cmd.Parameters.AddWithValue("@CodCue", Resu);
                            cmd.Parameters.AddWithValue("@Estado", true);
                            cmd.Parameters.AddWithValue("@Crea", Crea);
                            cmd.Parameters.AddWithValue("@Modifica", "");
                            cmd.Parameters.AddWithValue("@Elimina", "");
                            cmd.Parameters.AddWithValue("@CodSede", sede);
                            cmd.Parameters.AddWithValue("@historia", car.Historia);
                            cmd.Parameters.AddWithValue("@Evento", 1);
                            codCarnet = Convert.ToInt32(cmd.ExecuteScalar());

                        }
                        using (SqlCommand cmd = new SqlCommand("usp_Insertar_CSLaboratorio", con, tr))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Id", 0);
                            cmd.Parameters.AddWithValue("@IdProcencia", codCarnet);
                            cmd.Parameters.AddWithValue("@Manipulacion", car.Manipulador);
                            cmd.Parameters.AddWithValue("@Procedencia", "1");
                            cmd.Parameters.AddWithValue("@HIV", "");
                            cmd.Parameters.AddWithValue("@GrupoFactor", "");
                            cmd.Parameters.AddWithValue("@Parasitologico", "");
                            cmd.Parameters.AddWithValue("@RPR", "");
                            cmd.Parameters.AddWithValue("@Apto", "NE");
                            cmd.Parameters.AddWithValue("@Reevaluado", "");
                            cmd.Parameters.AddWithValue("@Observacion", "");
                            cmd.Parameters.AddWithValue("@Prioridad", 2);
                            cmd.Parameters.AddWithValue("@MuestraSangre", false);
                            cmd.Parameters.AddWithValue("@MuestraHeces", false);
                            cmd.Parameters.AddWithValue("@fechaReg", ut.ListadoHoraServidor().FirstOrDefault().HoraServidor);
                            cmd.Parameters.AddWithValue("@fechaAten", ut.ListadoHoraServidor().FirstOrDefault().HoraServidor);
                            cmd.Parameters.AddWithValue("@IdEstado", 5);
                            cmd.Parameters.AddWithValue("@Crea", Crea);
                            cmd.Parameters.AddWithValue("@Modifica", "");
                            cmd.Parameters.AddWithValue("@Elimina", "");
                            cmd.Parameters.AddWithValue("@CodSede", sede);
                            cmd.Parameters.AddWithValue("@Evento", 1);
                            int codlab = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                        }

                        using (SqlCommand cmd = new SqlCommand("usp_Insertar_CSOdontologia", con, tr))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Id", 0);
                            cmd.Parameters.AddWithValue("@IdProcencia", codCarnet);
                            cmd.Parameters.AddWithValue("@Manipulacion", car.Manipulador);
                            cmd.Parameters.AddWithValue("@Procedencia", "1");
                            cmd.Parameters.AddWithValue("@CodOdontograma", 0);
                            cmd.Parameters.AddWithValue("@Apto", "NE");
                            cmd.Parameters.AddWithValue("@Reevaluado", "");
                            cmd.Parameters.AddWithValue("@Observacion", "");
                            cmd.Parameters.AddWithValue("@Prioridad", 2);
                            cmd.Parameters.AddWithValue("@FechaReg", "");
                            cmd.Parameters.AddWithValue("@fechaAten", "");
                            cmd.Parameters.AddWithValue("@IdEstado", 5);
                            cmd.Parameters.AddWithValue("@Crea", Crea);
                            cmd.Parameters.AddWithValue("@Modifica", "");
                            cmd.Parameters.AddWithValue("@Elimina", "");
                            cmd.Parameters.AddWithValue("@CodSede", sede);
                            cmd.Parameters.AddWithValue("@Evento", 1);
                            int CodOdont = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                        }


                        using (SqlCommand cmd = new SqlCommand("usp_Insertar_CSMedicina", con, tr))
                        {
                            try
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@Id", 0);
                                cmd.Parameters.AddWithValue("@IdProcencia", codCarnet);
                                cmd.Parameters.AddWithValue("@Manipulacion", car.Manipulador);
                                cmd.Parameters.AddWithValue("@Procedencia", "1");
                                cmd.Parameters.AddWithValue("@Conclucion", "");
                                cmd.Parameters.AddWithValue("@Apto", "NE");
                                cmd.Parameters.AddWithValue("@Reevaluado", "");
                                cmd.Parameters.AddWithValue("@Observacion", "");
                                cmd.Parameters.AddWithValue("@Prioridad", 2);
                                cmd.Parameters.AddWithValue("@FechaReg", "");
                                cmd.Parameters.AddWithValue("@fechaAten", "");
                                cmd.Parameters.AddWithValue("@IdEstado", 8);
                                cmd.Parameters.AddWithValue("@Crea", Crea);
                                cmd.Parameters.AddWithValue("@Modifica", "");
                                cmd.Parameters.AddWithValue("@Elimina", "");
                                cmd.Parameters.AddWithValue("@CodSede", sede);
                                cmd.Parameters.AddWithValue("@Evento", 1);
                                int CodMed = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                            }
                            catch (Exception e)
                            {
                                con.Close();
                            }
                        }

                        tr.Commit();
                        if (util.GENERARCUENTAAUTO)
                        {
                            return RedirectPermanent("~/Caja/RegistrarCaja?id=" + Resu);
                        }
                        else
                        {
                            if (util.PREGXATENCIONPROGRAMADAS)
                            {
                                ViewBag.activaAlerta = 1;
                            }
                            else
                            {
                                return RedirectPermanent("~/Cuentas/VerificaCuenta/" + Resu);
                            }
                        }

                    }
                }
                catch (Exception e)
                {

                    return RedirectPermanent("../Master");

                }

            }
            catch (Exception e)
            {
                return RedirectPermanent("../Master");
            }



            return View(car);
        }

        public ActionResult VerDetalles(int id)
        {
            return View();

        }

        public ActionResult Vista_1()
        {
            return View();

        }




        public ActionResult ListarCarnetRegistradosDia()
        {

            return View(ListadoCarnet_RegistradosDia());
        }



        //Listamos los carnet por entregar del dia
        public List<E_Carnet_Sanitario> ListadoCarnet_RegistradosDia()
        {
            List<E_Carnet_Sanitario> listacarnet = new List<E_Carnet_Sanitario>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("usp_ListadoCarnet_RegistradosDia", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_Carnet_Sanitario carnet = new E_Carnet_Sanitario();

                        carnet.NroCarnet = int.Parse(dr["IdCarnet"].ToString());
                        carnet.Manipulador = dr["Manipulador"].ToString();
                        carnet.DesCarnet = dr["DescCarnet"].ToString();
                        carnet.Campana = dr["Campana"].ToString();
                        carnet.Carnet = dr["NroCarnet"].ToString();
                        carnet.CodCue = int.Parse(dr["CodCue"].ToString());
                        carnet.Estado = bool.Parse(dr["Estado"].ToString());
                        carnet.NombrePaciente = dr["NombrePaciente"].ToString();
                        carnet.CodPaciente = Convert.ToInt32(dr["Historia"].ToString());
                        listacarnet.Add(carnet);

                    }


                }
            }
            return listacarnet;
        }


        //VISTA PARA -->EN ESPERA
        public ActionResult ListaCarnetEnEspera()
        {
            ViewBag.lista = Lista_En_Espera();
            return View();
        }

        //VISTA PARA --> POR ATENDER
        public ActionResult ListaCarnetPorAtender()
        {

            return View();
        }







    }
}