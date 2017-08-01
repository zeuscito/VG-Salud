using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using VgSalud.Models;

using System.IO;

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
        public List<E_Carnet_Sanitario> Get_Odontologia_LLamado()
        {
            List<E_Carnet_Sanitario> listatipo = new List<E_Carnet_Sanitario>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_get_Buscar_Odontologia", db))
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
        public List<E_Carnet_Sanitario> Usp_Get_Laboratorio_EnEspera()
        {
            List<E_Carnet_Sanitario> listatipo = new List<E_Carnet_Sanitario>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_Get_Laboratorio_EnEspera", db))
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
        public List<E_Carnet_Sanitario> Usp_get_Odontologia_EnEspera()
        {
            List<E_Carnet_Sanitario> listatipo = new List<E_Carnet_Sanitario>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_get_Odontologia_EnEspera", db))
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
        public ActionResult RegistroCarnet(E_Carnet_Sanitario car,HttpPostedFileWrapper FileFotoCarnetSanidad)
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

                var evalua = tar.ListadoCategoriaPacienteTarifa(car.CodTar).Find(x => x.CodCatPac == pacientes.CodCatPac);
                var evalua1 = tar.ListadoTarifa().Find(x => x.CodTar == car.CodTar);
                DatosGeneralesController da1 = new DatosGeneralesController();
                E_Datos_Generales dat = da1.listadatogenerales().FirstOrDefault();

                precio = decimal.Round(evalua.Precio / (dat.igv + 1), 2);
                var result = ((evalua.Precio / decimal.Parse("1.18")) * dat.igv);
                igv = decimal.Round(result, 2);
                total = decimal.Round(precio + igv, 2);

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
                                da.Parameters.AddWithValue("@STotCue", precio);
                                da.Parameters.AddWithValue("@IgvCue", igv);
                                da.Parameters.AddWithValue("@TotCue", total);
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



                                //tari.Precio = decimal.Round(evalua.Precio / (dat.igv + 1), 2);
                                //var result = ((evalua.Precio / decimal.Parse("1.18")) * dat.igv);
                                //tari.igv = decimal.Round(result, 2);
                                //tari.total = decimal.Round(tari.Precio + tari.igv, 2);

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
                                dd.Parameters.AddWithValue("@precioUni", total);
                                dd.Parameters.AddWithValue("@precio", precio);
                                dd.Parameters.AddWithValue("@igv", igv);
                                dd.Parameters.AddWithValue("@total", total);
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

                        string path = Server.MapPath("~/Imagen/FotoCarnetSanidad/");

                        if (FileFotoCarnetSanidad != null)
                        {
                            //string extension = Path.GetExtension(FileFotoCarnetSanidad.FileName);
                            //byte[] data;
                            //using (Stream inputStream = FileFotoCarnetSanidad.InputStream)
                            //{
                            //    MemoryStream memoryStream = inputStream as MemoryStream;
                            //    if (memoryStream == null)
                            //    {
                            //        memoryStream = new MemoryStream();
                            //        inputStream.CopyTo(memoryStream);
                            //    }
                            //    data = memoryStream.ToArray();
                            //    string Base64 = Convert.ToBase64String(data);
                            //}

                            //------
                            //if (extension==".jpg")
                            //{
                               

                            //------
                            SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                                using (SqlCommand cmd = new SqlCommand("Usp_insert_carnet_sanitario", con, tr))
                                {

                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@Manipulador", car.Manipulador);
                                    cmd.Parameters.AddWithValue("@TipoCarnet", car.TipoCarnet);
                                    cmd.Parameters.AddWithValue("@Campana", car.Campana);
                                    cmd.Parameters.AddWithValue("@FecRecojo", "");
                                    cmd.Parameters.AddWithValue("@EstadoCarnet", car.EstadoCarnet);
                                    if (car.Empresa != null)
                                    {
                                        cmd.Parameters.AddWithValue("@Empresa", car.Empresa);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@Empresa", "");
                                    }
                                    
                                    cmd.Parameters.AddWithValue("@QR", "");
                                    cmd.Parameters.AddWithValue("@CodCue", Resu);
                                    cmd.Parameters.AddWithValue("@Estado", true);
                                    cmd.Parameters.AddWithValue("@Crea", Crea);
                                    cmd.Parameters.AddWithValue("@Modifica", "");
                                    cmd.Parameters.AddWithValue("@Elimina", "");
                                    cmd.Parameters.AddWithValue("@CodSede", sede);
                                    cmd.Parameters.AddWithValue("@historia", car.Historia);
                                    cmd.Parameters.AddWithValue("@Evento", 1);
                                    SqlParameter DatoNombre = cmd.Parameters.Add("@NombreFotoCarnet", SqlDbType.VarChar, 200);
                                    DatoNombre.Direction = ParameterDirection.Output;
                                    codCarnet = Convert.ToInt32(cmd.ExecuteScalar());
                                    string Nombre = Convert.ToString(cmd.Parameters["@NombreFotoCarnet"].Value);


                                    string Name = Path.GetFileName(FileFotoCarnetSanidad.FileName);

                                    FileFotoCarnetSanidad.SaveAs(path + Name);

                                    if (System.IO.File.Exists(path + Name))
                                    {

                                        System.IO.File.Copy(path + Name, path + Nombre);
                                        System.IO.File.Delete(path + Name);
                                    }

                            }

                            

                            //------

                            // --- Para Laboratorio
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
                                //------
                                //------
                                //Para odontologia
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
                                //------

                                //------
                                // Para Medicina
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

                                //------

                                //Listamos el valor de Nombre Foto Carnet

                                
                                //

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
                            }//------
                        //}//------ TERMINA EL IF FILE
                        else
                        {
                            //Response.Write("<script language=javascript>alert('Formato de Imagen no permitido ..');</script>");
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


        public ActionResult ModificarCarnetSanitario(int Historia, int IdCarnet, E_Carnet_Sanitario CS)
        {

            var EditDatos = ListarCarnetPorIdCarnet(IdCarnet).FirstOrDefault();
            ViewBag.TipoCarnet = EditDatos.TipoCarnet;
            ViewBag.Manipulador = EditDatos.Manipulador;
            ViewBag.Campana = EditDatos.Campana;
            ViewBag.FotoCarnet = EditDatos.FotoCarnet;
            ViewBag.IdCarnetSanidad = EditDatos.IdCarnetSanidad;
            
            return View(EditDatos);
        }


        public List<E_Carnet_Sanitario> ListarCarnetPorIdCarnet(int? IdCarnet)
        {
            string sede = Session["CodSede"].ToString();

            List<E_Carnet_Sanitario> Lista = new List<E_Carnet_Sanitario>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("UspBuscarCarnetPorIdCarnet", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdCarnet", IdCarnet);
                    cmd.Parameters.AddWithValue("@CodSed", sede);
                    using (
                        SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Carnet_Sanitario CS = new E_Carnet_Sanitario();

                            //CS.DescTipoTar
                            CS.DescTar = dr.GetString(0);
                            CS.TipoCarnet = dr.GetInt32(1);
                            CS.Manipulador = dr.GetString(2);
                            CS.Campana = dr.GetString(3);
                            CS.Empresa = dr.GetString(4);
                            CS.FotoCarnet = "/Imagen/FotoCarnetSanidad/"+ dr.GetString(5);
                            CS.IdCarnetSanidad = dr.GetInt32(6);

                            Lista.Add(CS);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        [HttpPost]
        public ActionResult ModificarCarnetSanitario(int IdCarnetSanidad, E_Carnet_Sanitario CS, HttpPostedFileWrapper FileFotoCarnetSanidad)
        {
            string CodSede = Session["CodSede"].ToString();
            
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                con.Open();

                string path = Server.MapPath("~/Imagen/FotoCarnetSanidad/");

                    if (FileFotoCarnetSanidad != null)
                    {
                        //string extension = Path.GetExtension(FileFotoCarnetSanidad.FileName);

                        //if (extension==".jpg")
                        //{
                        
                        using (SqlCommand cmd = new SqlCommand("Usp_ModificarCarnetSanidad_PorIdCarnetSanidad", con))
                        {

                            try
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@IdCarnetSanidad", IdCarnetSanidad);
                                cmd.Parameters.AddWithValue("@TipoCarnet", CS.TipoCarnet);
                                cmd.Parameters.AddWithValue("@Manipulador", CS.Manipulador);
                                cmd.Parameters.AddWithValue("@Campana", CS.Campana);
                                if(CS.Empresa!=null)
                                {
                                    cmd.Parameters.AddWithValue("@NomEmpresa", CS.Empresa);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@NomEmpresa", "");
                                }
                                cmd.Parameters.AddWithValue("@CodSede", CodSede);
                                SqlParameter DatoNombre = cmd.Parameters.Add("@NombreFotoCarnet", SqlDbType.VarChar, 200);
                                DatoNombre.Direction = ParameterDirection.Output;
                                int resultado = cmd.ExecuteNonQuery();
                                string Nombre = Convert.ToString(cmd.Parameters["@NombreFotoCarnet"].Value);

                                string Name = Path.GetFileName(FileFotoCarnetSanidad.FileName);
                                FileFotoCarnetSanidad.SaveAs(path + Name);

                                if (System.IO.File.Exists(path + Name))
                                {
                                    System.IO.File.Delete(path + Nombre);
                                    System.IO.File.Copy(path + Name, path + Nombre);
                                    System.IO.File.Delete(path + Name);
                                }


                                if (resultado > 0)
                                {
                                    ViewBag.mensaje = "2";
                                }
                                else
                                {
                                    ViewBag.mensaje = "3";
                                }
                                return RedirectToAction("ListarCarnetRegistradosDia");
                            }
                            catch (Exception e)
                            {
                                
                            }

                            
                        }

                        //}
                    
                    }
                    else
                    {
                        using (SqlCommand cmd = new SqlCommand("Usp_ModificarCarnetSanidad_PorIdCarnetSanidad", con))
                        {

                            try
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@IdCarnetSanidad", IdCarnetSanidad);
                                cmd.Parameters.AddWithValue("@TipoCarnet", CS.TipoCarnet);
                                cmd.Parameters.AddWithValue("@Manipulador", CS.Manipulador);
                                cmd.Parameters.AddWithValue("@Campana", CS.Campana);
                                if(CS.Empresa!=null)
                                {
                                    cmd.Parameters.AddWithValue("@NomEmpresa", CS.Empresa);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@NomEmpresa", "");
                                }
                                
                                cmd.Parameters.AddWithValue("@CodSede", CodSede);
                                SqlParameter DatoNombre = cmd.Parameters.Add("@NombreFotoCarnet", SqlDbType.VarChar, 200);
                                DatoNombre.Direction = ParameterDirection.Output;
                                int resultado = cmd.ExecuteNonQuery();
                                string Nombre = Convert.ToString(cmd.Parameters["@NombreFotoCarnet"].Value);


                                if (resultado > 0)
                                {
                                    ViewBag.mensaje = "2";
                                }
                                else
                                {
                                    ViewBag.mensaje = "3";
                                }
                                return RedirectToAction("ListarCarnetRegistradosDia");
                            }
                            catch (Exception e)
                            {

                            }


                        }
                    }
                    
               
                }
            }
            catch (Exception e)
            {

                return RedirectPermanent("../Master");

            }

            return RedirectToAction("ListarCarnetRegistradosDia");

        }
            





        public ActionResult Vista_Detalle_Lab()
        {
            ViewBag.lista_Detalle = Usp_Get_Laboratorio_EnEspera();
            return View();
        }

        public List<E_Carnet_Sanitario> Usp_get_Buscar_Medicina()
        {
            List<E_Carnet_Sanitario> listatipo = new List<E_Carnet_Sanitario>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_get_Buscar_Medicina", db))
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

        public List<E_Carnet_Sanitario> Usp_get_Medicina_EnEspera()
        {
            List<E_Carnet_Sanitario> listatipo = new List<E_Carnet_Sanitario>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_get_Medicina_EnEspera", db))
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




        public ActionResult Vista_Cabecera_Odo()
        {
            ViewBag.listaOdo = Get_Odontologia_LLamado();
            return View();
        }
        public ActionResult Vista_Detalle_Odo()
        {
            ViewBag.listaDetOdo = Usp_get_Odontologia_EnEspera();
            return View();
        }

        public ActionResult Vista_2()
        {
            return View();
        }

        public ActionResult Vista_Cabecera_Med()
        {
            ViewBag.listaCabMed = Usp_get_Buscar_Medicina();
            return View();
        }
        public ActionResult Vista_Detalle_Med()
        {
            ViewBag.listaDetMed = Usp_get_Medicina_EnEspera();
            return View();
        }

        public JsonResult GetFiltroDniFecha(string dni = "", string fecha = "")
        {
            var lista = Usp_Entrega_Carnet_Dni_Buscar(dni, fecha);
            string resultadoHTML = "";
            if (lista != null)
            {
                foreach (var item in lista)
                {
                    resultadoHTML += $"<li>" +
                                  "<a href ='#' style='border-bottom-style:dotted;border-bottom-color:black;'>" +
                                   "<h3>" +
                                   item.NombrePaciente +
                                   " - " + item.NroDoc +
                                   "</h3>" +
                                   "<button type='button' style='margin-left:35px;' class='llamar btn btn-success " +item.Historia+"' historia=" + item.Historia + " carnet=" + item.NroCarnet + ">LLamar</button>" +
                                    "<button type='button' style='margin-left:35px;' class='" + item.NroCarnet + " Si btn btn-success" + "' carnet=" + item.NroCarnet + " historia=" + item.Historia +">Si</button>" +
                                   "<button type='button' style='margin-left:35px;' class='" + item.NroCarnet + " No btn btn-danger" + "' carnet=" + item.NroCarnet + ">No</button>" +
                                   "<button type='button' class='imprimir btn btn-primary' style='margin-left:35px;' >Imprimir</button>" +
                               
                                   "</a></li>";
                }

            }
            resultadoHTML += "<script src=\"/Scripts/Util.js\"></scritp>";
            return Json(resultadoHTML, JsonRequestBehavior.AllowGet);

        }



        public ActionResult VerHistorialCarnet(string Nombre = null, string Dni = null)
        {
            ViewBag.Nombre = Nombre;
            ViewBag.Dni = Dni;
            return View(HistorialFiltro(Nombre, Dni));
        }


        public List<E_Historia_Carnet> HistorialFiltro(string Nombre = null, string Dni = null)
        {
            List<E_Historia_Carnet> lista = new List<E_Historia_Carnet>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Historial_Carnet_Sanitario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (Nombre == null || Nombre == "")
                    {
                        cmd.Parameters.AddWithValue("@Nombre", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Nombre", Nombre);
                    }
                    if (Dni == null || Dni == "")
                    {
                        cmd.Parameters.AddWithValue("@Dni", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Dni", Dni);
                    }

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_Historia_Carnet h = new E_Historia_Carnet();
                        h.Carnet = dr.GetString(0);
                        h.ApePat = dr.GetString(1);
                        h.ApeMat = dr.GetString(2);
                        h.NomPac = dr.GetString(3);
                        h.EstadoCarnet = dr.GetString(4);
                        h.FecRecojo = dr.GetDateTime(5);
                        h.FecVencimiento = dr.GetDateTime(6);
                        h.FechaRegistro = dr.GetDateTime(7);
                        h.NumDoc = dr.GetString(8);
                        lista.Add(h);

                    }
                    con.Close();

                }
                return lista;
            }
        }

        public int Usp_Actualizar_CarnetSanidad_Entregado1(int carnet, int historia)
        {
            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_Actualizar_CarnetSanidad_Entregado", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NroCarnet", carnet);
                    cmd.Parameters.AddWithValue("@Historia", historia);
                    try
                    {
                        db.Open();
                        return cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return 0;
                    }
                }
            }

        }
        public JsonResult Usp_Actualizar_CarnetSanidad_Entregado(int historia = 0, string dni = "", string fecha = "", int carnet = 0)
        {
            string resultadoHTML = "";
            if (carnet != 0 && historia != 0)
            {
                int resultado = Usp_Actualizar_CarnetSanidad_Entregado1(carnet, historia);
            }


            if (!string.IsNullOrWhiteSpace(fecha) || !string.IsNullOrWhiteSpace(dni))
            {
                var lista = Usp_Entrega_Carnet_Dni_Buscar(dni, fecha);

                if (lista != null)
                {
                    foreach (var item in lista)
                    {

                        resultadoHTML += $"<li>" +
                                                     "<a href ='#' style='border-bottom-style:dotted;border-bottom-color:black;text-align: center;'>" +
                                                      "<h3>" +
                                                      item.NombrePaciente +
                                                      " - " + item.NroDoc +
                                                      "</h3>" +
                                                      "<button type='button' style='margin-left:35px;' class='" + item.Historia + " llamar btn btn-success' historia=" + item.Historia + " carnet=" + item.NroCarnet + ">LLamar</button>" +
                                                       "<button type='button' style='margin-left:35px;' class='" + item.NroCarnet + " Si btn btn-success" + "' carnet=" + item.NroCarnet + " historia=" + item.Historia + ">Si</button>" +
                                                      "<button type='button' style='margin-left:35px;' class='" + item.NroCarnet + " No btn btn-danger" + "' carnet=" + item.NroCarnet + ">No</button>" +
                                                      "<button type='button' class='imprimir btn btn-primary' style='margin-left:35px;' >Imprimir</button>" +

                                                      "</a></li>";


                    }
                }

            }
            else
            {
                var lista = usp_get_Entrega_Carnet();
                if (lista != null)
                {
                    foreach (var item in lista)
                    {
                        resultadoHTML += $"<li>" +
                                                    "<a href ='#' style='border-bottom-style:dotted;border-bottom-color:black;text-align: center;'>" +
                                                     "<h3>" +
                                                     item.NombrePaciente +
                                                     " - " + item.NroDoc +
                                                     "</h3>" +
                                                     "<button type='button' style='margin-left:35px;' class='" + item.Historia + " llamar btn btn-success' historia=" + item.Historia + " carnet=" + item.NroCarnet + ">LLamar</button>" +
                                                      "<button type='button' style='margin-left:35px;' class='" + item.NroCarnet + " Si btn btn-success" + "' carnet=" + item.NroCarnet + " historia=" + item.Historia + ">Si</button>" +
                                                     "<button type='button' style='margin-left:35px;' class='" + item.NroCarnet + " No btn btn-danger" + "' carnet=" + item.NroCarnet + ">No</button>" +
                                                     "<button type='button' class='imprimir btn btn-primary' style='margin-left:35px;' >Imprimir</button>" +

                                                     "</a></li>";
                    }


                }
            }

            resultadoHTML += "<script src=\"/Scripts/Util.js\"></scritp>";
            return Json(resultadoHTML, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_Actualizar_Entrega_Carnet_Sanidad_index(int historia = 0, int carnet = 0)
        {
            if (carnet != 0 && historia != 0)
            {
                int resultado = Usp_Actualizar_CarnetSanidad_Entregado1(carnet, historia);
            }
            var lista = Usp_En_Espera_GetEntregar();
            string resultadoHTML = "";
            if (lista != null)
            {
                foreach (var item in lista)
                {
                    resultadoHTML += $"<li>" +
                                  "<a href ='#' style='border-bottom-style:dotted;border-bottom-color:black;text-align: center;'>" +
                                   "<h3>" +
                                   item.NombrePaciente +
                                   " - " + item.NroDoc +
                                   "</h3>" +
                                   "<button type='button' style='margin-left:35px;' class='" + item.Historia + " llamar1 btn btn-success' historia=" + item.Historia + " carnet=" + item.NroCarnet + ">LLamar</button>" +
                                    "<button type='button' style='margin-left:35px;' class='" + item.NroCarnet + " Si1 btn btn-success" + "' carnet=" + item.NroCarnet + " historia=" + item.Historia + ">Si</button>" +
                                   "<button type='button' style='margin-left:35px;' class='" + item.NroCarnet + " No1 btn btn-danger" + "' carnet=" + item.NroCarnet + ">No</button>" +
                                   "<button type='button' class='imprimir btn btn-primary' style='margin-left:35px;' >Imprimir</button>" +

                                   "</a></li>";
                }

            }
            resultadoHTML += "<script src=\"/Scripts/Util.js\"></scritp>";
            return Json(resultadoHTML, JsonRequestBehavior.AllowGet);

        }


        public List<E_Carnet_Sanitario> Usp_Entrega_Carnet_Dni_Buscar(string dni, string fecha)
        {
            List<E_Carnet_Sanitario> listatipo = new List<E_Carnet_Sanitario>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_Entrega_Carnet_Dni_Buscar", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    cmd.Parameters.AddWithValue("@NumDoc", dni);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        //p.Historia , p.NomPac , p.NumDoc
                        E_Carnet_Sanitario car = new E_Carnet_Sanitario();
                        car.Historia = int.Parse(dr["Historia"].ToString());
                        car.NombrePaciente = dr["NomPac"].ToString();
                        car.NroDoc = dr["NumDoc"].ToString();
                        car.NroCarnet = Convert.ToInt32(dr["NroCarnet"].ToString());
                        car.EstadoCarnet = dr["EstadoCarnet"].ToString();
                        listatipo.Add(car);

                    }

                }
            }
            return listatipo;
        }

        public List<E_Carnet_Sanitario> usp_get_Entrega_Carnet()
        {
            List<E_Carnet_Sanitario> listatipo = new List<E_Carnet_Sanitario>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_Entrega_Carnet", db))
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
                        car.NroCarnet = Convert.ToInt32(dr["NroCarnet"].ToString());
                        car.EstadoCarnet = dr["EstadoCarnet"].ToString();
                        listatipo.Add(car);

                    }

                }
            }
            return listatipo;
        }

        public List<E_Carnet_Sanitario> Usp_get_Entregar_CarnetSanidad()
        {
            List<E_Carnet_Sanitario> listatipo = new List<E_Carnet_Sanitario>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_get_Entregar_CarnetSanidad", db))
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
        public List<E_Carnet_Sanitario> Usp_get_Entregar_CarnetSanidad_Top()
        {
            List<E_Carnet_Sanitario> listatipo = new List<E_Carnet_Sanitario>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_get_Entregar_CarnetSanidad_top", db))
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

        public List<E_Carnet_Sanitario> Usp_En_Espera_GetEntregar()
        {
            List<E_Carnet_Sanitario> listatipo = new List<E_Carnet_Sanitario>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_En_Espera_GetEntregar", db))
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
        public JsonResult Get_Entregar_Carnet_Inicio()
        {

            var lista = usp_get_Entrega_Carnet();
            string resultadoHTML = "";
            if (lista != null)
            {
                foreach (var item in lista)
                {
                    resultadoHTML += $"<li>" +
                                  "<a href ='#' style='border-bottom-style:dotted;border-bottom-color:black;text-align: center;'>" +
                                   "<h3>" +
                                   item.NombrePaciente +
                                   " - " + item.NroDoc +
                                   "</h3>" +
                                   "<button type='button' style='margin-left:35px;' class='" + item.Historia + " llamar1 btn btn-success' historia=" + item.Historia + " carnet=" + item.NroCarnet + ">LLamar</button>" +
                                    "<button type='button' style='margin-left:35px;' class='" + item.NroCarnet + " Si btn btn-success" + "' carnet=" + item.NroCarnet + " historia=" + item.Historia + ">Si</button>" +
                                   "<button type='button' style='margin-left:35px;' class='" + item.NroCarnet + " No btn btn-danger" + "' carnet=" + item.NroCarnet + ">No</button>" +
                                   "<button type='button' class='imprimir btn btn-primary' style='margin-left:35px;' >Imprimir</button>" +

                                   "</a></li>";
                }

            }
            resultadoHTML += "<script src=\"/Scripts/Util.js\"></scritp>";
            return Json(resultadoHTML, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Vista_Cabecera_EntregaCarnet()
        {
            ViewBag.listaCabEnt = Usp_get_Entregar_CarnetSanidad_Top();
            return View();
        }
        public ActionResult Vista_detalle_EntregaCarne()
        {
            ViewBag.listaDetEnt = Usp_get_Entregar_CarnetSanidad();
            return View();
        }

        // Mantenimiento Independiente
        public ActionResult RegistrarCarnetSanidad(E_Carnet_Sanitario car)
        {
            string sede = Session["CodSede"].ToString();
            ViewBag.listadoTari = new SelectList(cit.ListadoTarifaAtencion().Where(x => x.CodEspec == "ES005" && x.CodSede == sede), "CodTar", "DescTar");

            if(car.CodCue != 0)
            {
                int CodCue = car.CodCue;
                var ListaPaciente = ListaPacientesPorCodCue(CodCue).FirstOrDefault();
                ViewBag.Historia = ListaPaciente.Historia;
                ViewBag.NombrePaciente = ListaPaciente.NombrePaciente;
                ViewBag.NroDocumento = ListaPaciente.NroDoc;
                ViewBag.Edad = ListaPaciente.Edad;
            }
            else
            {
                ViewBag.Historia = 0;
                ViewBag.NombrePaciente = "";
                ViewBag.NroDocumento = "";
                ViewBag.Edad = "";
            }
            


            return View(car);
        }

        public List<E_Carnet_Sanitario> ListaPacientesPorCodCue(int CodCue)
        {
            
            string sede = Session["CodSede"].ToString();
            List<E_Carnet_Sanitario> listaPaciente = new List<E_Carnet_Sanitario>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_ListaPacientePorIdCuenta", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodCue", CodCue);
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    try
                    {
                        while (dr.Read())
                        {
                            E_Carnet_Sanitario car = new E_Carnet_Sanitario();
                            car.Historia = Convert.ToInt32(dr["Historia"].ToString());
                            car.NombrePaciente = dr["NombrePaciente"].ToString();
                            car.NroDoc = dr["NumDoc"].ToString();
                            car.Edad = dr["Edad"].ToString();
                            listaPaciente.Add(car);
                        }
                    }
                    catch (Exception e)
                    {
                        
                    }
                    


                }
            }
            return listaPaciente;
        }

        [HttpPost]
        public ActionResult RegistrarCarnetSanidad(E_Carnet_Sanitario car, HttpPostedFileWrapper FileFotoCarnetSanidad)
        {
            string sede = Session["CodSede"].ToString();

            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string Crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
            string usuario = Session["UserID"].ToString();

            int Resu = 0;
           

                try
                {
                    int codCarnet = 0;
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();

                        string path = Server.MapPath("~/Imagen/FotoCarnetSanidad/");

                        if (FileFotoCarnetSanidad != null)
                        {
                            //string extension = Path.GetExtension(FileFotoCarnetSanidad.FileName);
                            //byte[] data;
                            //using (Stream inputStream = FileFotoCarnetSanidad.InputStream)
                            //{
                            //    MemoryStream memoryStream = inputStream as MemoryStream;
                            //    if (memoryStream == null)
                            //    {
                            //        memoryStream = new MemoryStream();
                            //        inputStream.CopyTo(memoryStream);
                            //    }
                            //    data = memoryStream.ToArray();
                            //    string Base64 = Convert.ToBase64String(data);
                            //}

                            //------
                            //if (extension==".jpg")
                            //{


                            //------
                            SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                            using (SqlCommand cmd = new SqlCommand("Usp_insert_carnet_sanitario", con, tr))
                            {

                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@Manipulador", car.Manipulador);
                                cmd.Parameters.AddWithValue("@TipoCarnet", car.TipoCarnet);
                                cmd.Parameters.AddWithValue("@Campana", car.Campana);
                                cmd.Parameters.AddWithValue("@FecRecojo", "");
                                cmd.Parameters.AddWithValue("@EstadoCarnet", car.EstadoCarnet);
                                if (car.Empresa != null)
                                {
                                    cmd.Parameters.AddWithValue("@Empresa", car.Empresa);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@Empresa", "");
                                }

                                cmd.Parameters.AddWithValue("@QR", "");
                                cmd.Parameters.AddWithValue("@CodCue", Resu);
                                cmd.Parameters.AddWithValue("@Estado", true);
                                cmd.Parameters.AddWithValue("@Crea", Crea);
                                cmd.Parameters.AddWithValue("@Modifica", "");
                                cmd.Parameters.AddWithValue("@Elimina", "");
                                cmd.Parameters.AddWithValue("@CodSede", sede);
                                cmd.Parameters.AddWithValue("@historia", car.Historia);
                                cmd.Parameters.AddWithValue("@Evento", 1);
                                SqlParameter DatoNombre = cmd.Parameters.Add("@NombreFotoCarnet", SqlDbType.VarChar, 200);
                                DatoNombre.Direction = ParameterDirection.Output;
                                codCarnet = Convert.ToInt32(cmd.ExecuteScalar());
                                string Nombre = Convert.ToString(cmd.Parameters["@NombreFotoCarnet"].Value);


                                string Name = Path.GetFileName(FileFotoCarnetSanidad.FileName);

                                FileFotoCarnetSanidad.SaveAs(path + Name);

                                if (System.IO.File.Exists(path + Name))
                                {

                                    System.IO.File.Copy(path + Name, path + Nombre);
                                    System.IO.File.Delete(path + Name);
                                }

                            }



                            //------

                            // --- Para Laboratorio
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
                            //------
                            //------
                            //Para odontologia
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
                            //------

                            //------
                            // Para Medicina
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

                            //------

                            //Listamos el valor de Nombre Foto Carnet


                            //

                           
                        }//------
                        //}//------ TERMINA EL IF FILE
                        else
                        {
                            //Response.Write("<script language=javascript>alert('Formato de Imagen no permitido ..');</script>");
                        }
                    }
                }
                catch (Exception e)
                {

                    return RedirectPermanent("../Master");

                }

            return View(car);
        }




    }
}