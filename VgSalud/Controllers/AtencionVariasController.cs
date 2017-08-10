using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using VgSalud.Models;
using System.Text;

namespace VgSalud.Controllers
{
    public class AtencionVariasController : Controller
    {
        TipoTarifaController tt = new TipoTarifaController();
        EspecialidadController es = new EspecialidadController();
        ServiciosController ser = new ServiciosController();
        MedicosController med = new MedicosController();
        TarifarioController tar = new TarifarioController();

        public List<E_AtencionVarias> ListadoAtencionVarias(int historia)
        {
            List<E_AtencionVarias> Lista = new List<E_AtencionVarias>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_AtencionVarias", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Historia", historia);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_AtencionVarias ate = new E_AtencionVarias();

                            ate.CodAten = dr.GetInt32(0);
                            ate.CodSede = dr.GetString(1);
                            ate.CodCue = dr.GetInt32(2);
                            ate.Historia = dr.GetInt32(3);
                            ate.NomPac = dr.GetString(4);
                            ate.CodCatPac = dr.GetString(5);
                            ate.SubTotal = dr.GetDecimal(6);
                            ate.Igv = dr.GetDecimal(7);
                            ate.Total = dr.GetDecimal(8);
                            ate.Fecha = dr.GetDateTime(9);
                            ate.CodUsu = dr.GetString(10);
                            ate.EstConsul = dr.GetString(11);

                            Lista.Add(ate);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_AtencionVarias> ListadoAtencionVariasxID(int CodAten)
        {
            List<E_AtencionVarias> Lista = new List<E_AtencionVarias>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_AtencionVariasxId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodAten", CodAten);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_AtencionVarias ate = new E_AtencionVarias();

                            ate.CodAten = dr.GetInt32(0);
                            ate.CodSede = dr.GetString(1);
                            ate.CodCue = dr.GetInt32(2);
                            ate.Historia = dr.GetInt32(3);
                            ate.NomPac = dr.GetString(4);
                            ate.CodCatPac = dr.GetString(5);
                            ate.SubTotal = dr.GetDecimal(6);
                            ate.Igv = dr.GetDecimal(7);
                            ate.Total = dr.GetDecimal(8);
                            ate.Fecha = dr.GetDateTime(9);
                            ate.CodUsu = dr.GetString(10);
                            ate.EstConsul = dr.GetString(11);

                            Lista.Add(ate);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_Tipo_Tarifa> ListadoTipoTarifaAtencionV()
        {
            List<E_Tipo_Tarifa> Lista = new List<E_Tipo_Tarifa>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_TipoTarifa_AtencionVarias", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Tipo_Tarifa Etip = new E_Tipo_Tarifa();

                            Etip.CodTipTar = dr.GetString(0);
                            Etip.DescTipTar = dr.GetString(1);
                            Etip.CodSede = dr.GetString(2);

                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }



        public List<E_AtencionVarias_Detalle> ListadoAtencionDetalleNew(int Resu)
        {
            List<E_AtencionVarias_Detalle> Lista = new List<E_AtencionVarias_Detalle>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_AtencionVariasDetalle_xCodCuenta", con))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoCuenta", Resu);
                        using (SqlDataReader dr = cmd.ExecuteReader())   //
                        {
                            while (dr.Read())
                            {
                                E_AtencionVarias_Detalle a = new E_AtencionVarias_Detalle();

                                a.CodAtenDet = dr.GetInt32(0);
                                a.CodAten = dr.GetInt32(1);
                                a.item = dr.GetInt32(2);
                                a.CodSede = dr.GetString(3);
                                a.CodEspec = dr.GetString(4);
                                a.CodServ = dr.GetString(5);
                                a.CodMed = dr.GetString(6);
                                a.CodTar = dr.GetString(7);
                                a.CodTipTar = dr.GetString(8);
                                a.CodSTipTar = dr.GetString(9);
                                a.Cantida = dr.GetInt32(10);
                                a.SubTotal = dr.GetDecimal(11);
                                a.Igv = dr.GetDecimal(12);
                                a.Total = dr.GetDecimal(13);
                                a.MedicoEnvia = dr.GetString(14);
                                a.EspeciEnvia = dr.GetString(15);
                                a.Turno = dr.GetString(16);
                                a.Estado = dr.GetString(17);
                                a.CodUsu = dr.GetString(18);
                                a.CodCue = dr.GetInt32(22);


                                Lista.Add(a);
                            }
                        }
                    }
                    catch (Exception e)
                    {

                    }
                    finally
                    {
                        con.Close();
                    }


                }
                return Lista;
            }
        }




        public List<E_AtencionVarias_Detalle> ListadoAtencionDetalle()
        {
            List<E_AtencionVarias_Detalle> Lista = new List<E_AtencionVarias_Detalle>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_AtencionVariasDetalle", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader()) //IDataReader
                        {
                            while (dr.Read())
                            {
                                E_AtencionVarias_Detalle a = new E_AtencionVarias_Detalle();

                                a.CodAtenDet = dr.GetInt32(0);
                                a.CodAten = dr.GetInt32(1);
                                a.item = dr.GetInt32(2);
                                a.CodSede = dr.GetString(3);
                                a.CodEspec = dr.GetString(4);
                                a.CodServ = dr.GetString(5);
                                a.CodMed = dr.GetString(6);
                                a.CodTar = dr.GetString(7);
                                a.CodTipTar = dr.GetString(8);
                                a.CodSTipTar = dr.GetString(9);
                                a.Cantida = dr.GetInt32(10);
                                a.SubTotal = dr.GetDecimal(11);
                                a.Igv = dr.GetDecimal(12);
                                a.Total = dr.GetDecimal(13);
                                a.MedicoEnvia = dr.GetString(14);
                                a.EspeciEnvia = dr.GetString(15);
                                a.Turno = dr.GetString(16);
                                a.Estado = dr.GetString(17);
                                a.CodUsu = dr.GetString(18);
                                a.CodCue = dr.GetInt32(22);


                                Lista.Add(a);
                            }
                            con.Close();
                        }
                    }
                    catch (Exception e)
                    {

                    }


                }
                return Lista;
            }
        }

        public List<E_Promo> ListadoPromociones()
        {
            List<E_Promo> Lista = new List<E_Promo>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaPromocionesCabecera", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader()) //IDataReader
                    {
                        while (dr.Read())
                        {
                            E_Promo a = new E_Promo();
                            a.IdPC = dr.GetInt32(0);
                            a.Descripcion = dr.GetString(1);
                            a.CodEspec = dr.GetString(2);
                            a.CodSede = dr.GetString(3);
                            a.Precio = dr.GetDecimal(4);
                            a.estado = dr.GetBoolean(8);

                            Lista.Add(a);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Promo> ListadoPromocionesDetalle(int id)
        {
            List<E_Promo> Lista = new List<E_Promo>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaPromocionesDetalle", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Idpc", id);
                    using (SqlDataReader dr = cmd.ExecuteReader()) //IDataReader
                    {
                        while (dr.Read())
                        {
                            E_Promo a = new E_Promo();
                            a.IdPC = dr.GetInt32(0);
                            a.item = dr.GetInt32(1);
                            a.CodTar = dr.GetString(2);
                            a.CodTipTar = dr.GetString(3);
                            a.Precio = dr.GetDecimal(4);
                            a.cantidad = dr.GetInt32(5);
                            a.SubtotalD = dr.GetDecimal(6);
                            a.estado = dr.GetBoolean(10);

                            Lista.Add(a);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Cuentas> ListadoCuentas()
        {
            List<E_Cuentas> Lista = new List<E_Cuentas>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_comboCuentas", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Cuentas Etip = new E_Cuentas();

                            Etip.CodCue = dr.GetInt32(0);
                            Etip.CodSede = dr.GetString(1);
                            Etip.Historia = dr.GetInt32(2);
                            Etip.EstCue = dr.GetBoolean(3);
                            Etip.EstGene = dr.GetString(4);
                            Etip.NomPac = dr.GetString(5);
                            Etip.ApePat = dr.GetString(6);
                            Etip.ApeMat = dr.GetString(7);
                            Etip.nomCompleto = Etip.CodCue + " - " + Etip.NomPac + " " + Etip.ApePat + " " + Etip.ApeMat;

                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Tarifario> ListadoTarifaAtencion()
        {
            string sede = Session["codSede"].ToString();
            List<E_Tarifario> Lista = new List<E_Tarifario>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaTipoTarifaAtencion", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Tarifario Etip = new E_Tarifario();

                            Etip.CodTar = dr.GetString(0);
                            Etip.DescTar = dr.GetString(1);
                            Etip.EstTar = dr.GetBoolean(2);
                            Etip.CodEspec = dr.GetString(3);
                            Etip.procedencia = dr.GetInt32(4);
                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Tarifario> ListadoTarifaAtencionVarias(string codEspec)
        {
            string sede = Session["codSede"].ToString();
            List<E_Tarifario> Lista = new List<E_Tarifario>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaTipoTarifaAtencionVarias", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    cmd.Parameters.AddWithValue("@CodEspec", codEspec);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Tarifario Etip = new E_Tarifario();

                            Etip.CodTar = dr.GetString(0);
                            Etip.DescTar = dr.GetString(1);
                            Etip.EstTar = dr.GetBoolean(2);
                            Etip.CodEspec = dr.GetString(3);
                            Etip.procedencia = dr.GetInt32(4);
                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Tarifario> ListadoTarifVentaRapida(string CodServ, string sede, int historia, string descripcion)
        {
            List<E_Tarifario> Lista = new List<E_Tarifario>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaTipoTarifaVentaRapida", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    cmd.Parameters.AddWithValue("@CodServ", CodServ);
                    cmd.Parameters.AddWithValue("@historia", historia);
                    cmd.Parameters.AddWithValue("@descripcion", descripcion);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Tarifario Etip = new E_Tarifario();

                            Etip.CodTar = dr.GetString(0);
                            Etip.DescTar = dr.GetString(1);
                            Etip.EstTar = dr.GetBoolean(2);
                            Etip.CodEspec = dr.GetString(3);
                            Etip.procedencia = dr.GetInt32(4);
                            Etip.ModPrecio = dr.GetBoolean(5);
                            Etip.Precio = dr.GetDecimal(6);
                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult AsignarTipoTarifa()
        {
            string sede = Session["codSede"].ToString();

            ViewBag.listadoTipoTarifa = new SelectList(tt.ListadoTipoTarifa().Where(d => d.EstTipTar == true), "CodTipTar", "DescTipTar");
            ViewBag.listaTarifa = (List<E_Tipo_Tarifa>)ListadoTipoTarifaAtencionV().Where(x => x.CodSede == sede).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult AsignarTipoTarifa(E_Tipo_Tarifa tip)
        {
            try
            {
                string sede = Session["codSede"].ToString();

                ViewBag.listaTarifa = (List<E_Tipo_Tarifa>)ListadoTipoTarifaAtencionV().Where(x => x.CodSede == sede).ToList();
                ViewBag.listadoTipoTarifa = new SelectList(tt.ListadoTipoTarifa().Where(d => d.EstTipTar == true), "CodTipTar", "DescTipTar", tip.CodTipTar);

                E_Tipo_Tarifa reg = tt.ListadoTipoTarifa().Find(x => x.CodTipTar.Equals(tip.CodTipTar));
                tip.DescTipTar = reg.DescTipTar;


                var evalua = (List<E_Tipo_Tarifa>)ListadoTipoTarifaAtencionV().Where(x => x.CodTipTar == tip.CodTipTar).ToList();

                if (evalua.Count == 0)
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        using (SqlCommand da = new SqlCommand("Usp_MtoTipoTarifa_AtencionVarias", con))
                        {
                            try
                            {
                                da.CommandType = CommandType.StoredProcedure;

                                da.Parameters.AddWithValue("@CodTipTar", tip.CodTipTar);
                                da.Parameters.AddWithValue("@descripcion", tip.DescTipTar.ToUpper());
                                da.Parameters.AddWithValue("@CodSede", sede);
                                da.Parameters.AddWithValue("@Evento", "1");

                                da.ExecuteNonQuery();

                                ViewBag.mensaje = "Pedido registrado";

                            }
                            catch (Exception e)
                            {
                                ViewBag.mensaje = "Error: " + e.Message;
                            }
                            finally { con.Close(); }
                        }

                        return RedirectToAction("AsignarTipoTarifa");
                    }

                }
                else
                {
                    ViewBag.mensaje = "Error: El tipo de tarifa ya esta agregado";
                    return View(tip);
                }
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Error: Datos Requerido";
                return View(tip);
            }




        }


        public ActionResult EliminarTipoTarifa(string id)
        {

            string sede = Session["codSede"].ToString();



            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_MtoTipoTarifa_AtencionVarias", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;

                        da.Parameters.AddWithValue("@CodTipTar", id);
                        da.Parameters.AddWithValue("@descripcion", "");
                        da.Parameters.AddWithValue("@CodSede", "");
                        da.Parameters.AddWithValue("@Evento", "2");

                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "Error: " + e.Message;
                    }
                    finally { con.Close(); }
                }

                return RedirectToAction("AsignarTipoTarifa");
            }

        }


        public ActionResult RegistroAtenciones(int historia, string cuenta = null, string cadena = null, int? flag = null)
        {

            string sede = Session["codSede"].ToString();

            ViewBag.esp = es.ListadoEspecialidades();
            ViewBag.ser = ser.ListadoServicios();
            ViewBag.tar = tar.ListadoTarifa();
            ViewBag.med = med.ListadoMedico();

            ViewBag.tiptar = (new TipoTarifaController()).ListadoTipoTarifa();


            PacientesController pa = new PacientesController();
            CategoriaPacienteController cate = new CategoriaPacienteController();
            E_Pacientes reg = pa.ListadoPacientes().Find(x => x.Historia == historia);
            E_Categoria_Paciente cat = cate.listadoCategoriaCliente().Where(x => x.CodCatPac == reg.CodCatPac).FirstOrDefault();
            ViewBag.nombrePaciente = reg.NomPac + " " + reg.ApePat + " " + reg.ApeMat;
            ViewBag.historia = historia;
            ViewBag.categoriapaciente = cat.DescCatPac;
            if (cuenta != null)
            {
                ViewBag.cuenta = cuenta;
            }
            else
            {
                ViewBag.cuenta = null;
            }


            if (cadena != null)
            {

                string[] fija = cadena.Split(',');
                int i = 1;
                foreach (string item in fija)
                {
                    if (i == 1)
                    {
                        ViewBag.especialidad = item;
                    }
                    else if (i == 2)
                    {
                        ViewBag.servicio = item;
                    }
                    else if (i == 3)
                    {
                        ViewBag.medico = item;
                    }
                    else if (i == 4)
                    {
                        ViewBag.tarifa = item;
                    }
                    else if (i == 5)
                    {
                        ViewBag.cantidad = item;
                    }
                    else if (i == 6)
                    {
                        ViewBag.precio = item;
                    }
                    else if (i == 7)
                    {
                        ViewBag.total = item;
                    }
                    else if (i == 8)
                    {
                        ViewBag.medicoEnvia = item;
                    }
                    else if (i == 9)
                    {
                        ViewBag.codCuenta = item;
                    }

                    i++;

                }

            }
            else
            {
                if (flag != 1 || flag == null)
                {
                    if (Session["atenciones"] != null)
                    {
                        Session.Remove("atenciones");
                    }
                }
                ViewBag.especialidad = "";
                ViewBag.servicio = "";
                ViewBag.medico = "";
                ViewBag.tarifa = "";
                ViewBag.cantidad = "";
                ViewBag.precio = "";
                ViewBag.total = "";
                ViewBag.medicoEnvia = "";


            }


            if (Session["atenciones"] == null)
            {
                Session["atenciones"] = new List<E_AtencionVarias_Detalle>();
            }

            ViewBag.listadoEspecialidad = new SelectList(es.ListadoEspecialidades().Where(x => x.EstEspec == true && x.CodSed == sede), "CodEspec", "NomEspec");
            ViewBag.listadoServicios = new SelectList(ser.ListadoServicios().Where(x => x.EstServ == true && x.CodSede == sede), "CodServ", "NomServ");
            ViewBag.listadoMedico = new SelectList(med.ListadoMedico().Where(x => x.EstMed == true && x.CodSede == sede), "CodMed", "NomMed");
            ViewBag.listadoTarifa = new SelectList(tar.ListadoTarifa().Where(x => x.EstTar == true && x.CodSede == sede), "CodTar", "DescTar");
            ViewBag.listadoCuenta = new SelectList(ListadoCuentas().Where(x => x.EstCue == true && x.CodSede == sede && x.EstGene == "G" && x.Historia == historia), "CodCue", "nomCompleto");
            ViewBag.listadoDetalleAtencion = (List<E_AtencionVarias_Detalle>)Session["atenciones"];

            return View();

        }


        [HttpPost]
        public ActionResult RegistroAtenciones(E_AtencionVarias ate)
        {
            int codigoCuenta = 0;

            CuentasController cu = new CuentasController();
            PacientesController pa = new PacientesController();
            UtilitarioController ut = new UtilitarioController();

            string sede = Session["codSede"].ToString();

            var util = new DatosGeneralesController().listadatogenerales().FirstOrDefault();

            ViewBag.esp = es.ListadoEspecialidades();
            ViewBag.ser = ser.ListadoServicios();
            ViewBag.tar = tar.ListadoTarifa();
            ViewBag.med = med.ListadoMedico();
            ViewBag.tiptar = (new TipoTarifaController()).ListadoTipoTarifa();


            ViewBag.listadoEspecialidad = new SelectList(es.ListadoEspecialidades().Where(x => x.EstEspec == true && x.CodSed == sede), "CodEspec", "NomEspec", ate.CodEspec);
            ViewBag.listadoServicios = new SelectList(ser.ListadoServicios().Where(x => x.EstServ == true && x.CodSede == sede), "CodServ", "NomServ", ate.CodServ);
            ViewBag.listadoMedico = new SelectList(med.ListadoMedico().Where(x => x.EstMed == true && x.CodSede == sede), "CodMed", "NomMed", ate.CodMed);
            ViewBag.listadoTarifa = new SelectList(tar.ListadoTarifa().Where(x => x.EstTar == true && x.CodSede == sede), "CodTar", "DescTar", ate.CodTar);
            ViewBag.listadoCuenta = new SelectList(ListadoCuentas().Where(x => x.EstCue == true && x.CodSede == sede && x.EstGene == "G" && x.Historia == ate.Historia), "CodCue", "nomCompleto", ate.CodCue);

            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            int hora = horaSis.HoraServidor.Hour;
            string turno = "";
            if (hora < 13)
            {
                turno = "MAÑANA";
            }
            else
            {
                turno = "TARDE";
            }

            string Crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
            string usuario = Session["UserID"].ToString();
            E_Pacientes reg = pa.ListadoPacientes().Find(x => x.Historia == ate.Historia);

            string nomPac = reg.NomPac + " " + reg.ApePat + " " + reg.ApeMat;
            E_CuentaDetalle cueD = cu.ListadoCuentaDetalle(ate.CodCue).LastOrDefault();

            decimal nuevoPrecio = 0;
            decimal nuevoIgv = 0;
            decimal nuevoTotal = 0;

            if (ate.CodCue != 0)
            {
                E_Cuentas cuentas = cu.BuscaCuenta(ate.CodCue).FirstOrDefault();
                nuevoPrecio = ate.SubTotal + cuentas.STotCue;
                nuevoIgv = ate.Igv + cuentas.IgvCue;
                nuevoTotal = ate.Total + cuentas.TotCue;
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);

                using (SqlCommand da = new SqlCommand("Usp_MtoCuentas", con, tr))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;

                        if (ate.CodCue == 0)
                        {
                            da.Parameters.AddWithValue("@CodCue", "");

                        }
                        else
                        {
                            da.Parameters.AddWithValue("@CodCue", ate.CodCue);
                        }
                        da.Parameters.AddWithValue("@CodSede", sede);
                        da.Parameters.AddWithValue("@Historia", ate.Historia);
                        da.Parameters.AddWithValue("@CodcatPac", reg.CodCatPac);
                        if (ate.CodCue == 0)
                        {
                            da.Parameters.AddWithValue("@STotCue", ate.SubTotal);
                            da.Parameters.AddWithValue("@IgvCue", ate.Igv);
                            da.Parameters.AddWithValue("@TotCue", ate.Total);
                        }
                        else
                        {
                            da.Parameters.AddWithValue("@STotCue", nuevoPrecio);
                            da.Parameters.AddWithValue("@IgvCue", nuevoIgv);
                            da.Parameters.AddWithValue("@TotCue", nuevoTotal);
                        }
                        da.Parameters.AddWithValue("@FecCrea", "");
                        da.Parameters.AddWithValue("@FecAnul", "");
                        da.Parameters.AddWithValue("@EstCue", "1");
                        da.Parameters.AddWithValue("@EstGene", "");
                        da.Parameters.AddWithValue("@SecFact", "");
                        da.Parameters.AddWithValue("@Usuario", usuario);
                        da.Parameters.AddWithValue("@UsuarioAnula", "");
                        da.Parameters.AddWithValue("@Crea", Crea);
                        da.Parameters.AddWithValue("@Modifica", Crea);
                        da.Parameters.AddWithValue("@Elimina", "");
                        if (ate.CodCue == 0)
                        {
                            da.Parameters.AddWithValue("@Evento", "1");
                        }
                        else
                        {
                            da.Parameters.AddWithValue("@Evento", "2");
                        }
                        int Resu = (int)da.ExecuteScalar();

                        var aaa = ListadoAtencionDetalleNew(Resu).FirstOrDefault();

                        codigoCuenta = Resu;

                        using (SqlCommand cmd = new SqlCommand("Usp_MtoAtencionVarias", con, tr))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@CodAten", "");
                            cmd.Parameters.AddWithValue("@CodSede", sede);
                            cmd.Parameters.AddWithValue("@CodCue", Resu);
                            cmd.Parameters.AddWithValue("@Historia", ate.Historia);
                            cmd.Parameters.AddWithValue("@NomPac", nomPac.ToString());
                            cmd.Parameters.AddWithValue("@CodCatPac", reg.CodCatPac);
                            cmd.Parameters.AddWithValue("@SubTotal", ate.SubTotal);
                            cmd.Parameters.AddWithValue("@Igv", ate.Igv);
                            cmd.Parameters.AddWithValue("@Total", ate.Total);
                            cmd.Parameters.AddWithValue("@Fecha", "");
                            cmd.Parameters.AddWithValue("@CodUsu", usuario);
                            cmd.Parameters.AddWithValue("@EstConsul", "");
                            cmd.Parameters.AddWithValue("@Crea", Crea);
                            cmd.Parameters.AddWithValue("@Modifica", "");
                            cmd.Parameters.AddWithValue("@Elimina", "");
                            cmd.Parameters.AddWithValue("@Evento", "1");


                            int codAtencion = (int)cmd.ExecuteScalar();

                            int CodigoCuentaResult = Resu;

                            //E_AtencionVarias_Detalle aaa = ListadoAtencionDetalle().Where(x => x.CodCue == Resu).LastOrDefault();





                            int item = 0;
                            int item2 = 0;

                            foreach (E_AtencionVarias_Detalle it in (List<E_AtencionVarias_Detalle>)Session["atenciones"])
                            {


                                if (aaa == null)
                                {
                                    if (item == 0)
                                    {
                                        item = 0 + 1;
                                    }
                                    else
                                    {
                                        item = item + 1;
                                    }
                                }
                                else
                                {
                                    item = aaa.item + 1;
                                }

                                using (SqlCommand av = new SqlCommand("Usp_MtoAtencionVarias_Detalle ", con, tr))
                                {


                                    av.CommandType = CommandType.StoredProcedure;
                                    av.Parameters.AddWithValue("@CodAtenDet", "");
                                    av.Parameters.AddWithValue("@CodAten", codAtencion);
                                    av.Parameters.AddWithValue("@item", item);
                                    av.Parameters.AddWithValue("@CodSede", sede);
                                    av.Parameters.AddWithValue("@CodEspec", it.CodEspec);
                                    av.Parameters.AddWithValue("@CodServ", it.CodServ);
                                    av.Parameters.AddWithValue("@CodMed", it.CodMed);
                                    av.Parameters.AddWithValue("@CodTar", it.CodTar);
                                    av.Parameters.AddWithValue("@CodTipTar", it.CodTipTar);
                                    av.Parameters.AddWithValue("@CodSTipTar", it.CodSTipTar);
                                    av.Parameters.AddWithValue("@Cantida", it.Cantida);
                                    av.Parameters.AddWithValue("@SubTotal", it.SubTotal);
                                    av.Parameters.AddWithValue("@Igv", it.Igv);
                                    av.Parameters.AddWithValue("@Total", it.Total);
                                    av.Parameters.AddWithValue("@MedicoEnvia", it.MedicoEnvia);
                                    av.Parameters.AddWithValue("@EspeciEnvia", it.EspeciEnvia);
                                    av.Parameters.AddWithValue("@Turno", turno);
                                    av.Parameters.AddWithValue("@Estado", it.Estado);
                                    av.Parameters.AddWithValue("@CodUsu", it.CodUsu);
                                    av.Parameters.AddWithValue("@Crea", Crea);
                                    av.Parameters.AddWithValue("@Modifica", "");
                                    av.Parameters.AddWithValue("@Elimina", "");
                                    av.Parameters.AddWithValue("@Evento", "1");

                                    int codAtencionDetalle = (int)av.ExecuteScalar();




                                    if (cueD == null)
                                    {
                                        if (item2 == 0)
                                        {
                                            item2 = 0 + 1;
                                        }
                                        else
                                        {
                                            item2 = item2 + 1;
                                        }
                                    }
                                    else
                                    {
                                        item2 = cueD.Item + 1;
                                    }

                                    using (SqlCommand dd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr))
                                    {

                                        E_Tarifario tari = tar.ListadoTarifa().Find(x => x.CodTar == it.CodTar);

                                        dd.CommandType = CommandType.StoredProcedure;
                                        dd.Parameters.AddWithValue("@Procedencia", 2);
                                        dd.Parameters.AddWithValue("@CodCue", Resu);
                                        dd.Parameters.AddWithValue("@Item", item2);
                                        dd.Parameters.AddWithValue("@Tarifa", tari.CodTar);
                                        dd.Parameters.AddWithValue("@CodProce", codAtencion);
                                        dd.Parameters.AddWithValue("@CodDetalleP", codAtencionDetalle);
                                        dd.Parameters.AddWithValue("@CodSede", sede);
                                        dd.Parameters.AddWithValue("@Cantidad", it.Cantida);
                                        dd.Parameters.AddWithValue("@precioUni", it.Precio / it.Cantida);
                                        dd.Parameters.AddWithValue("@precio", it.SubTotal);
                                        dd.Parameters.AddWithValue("@igv", it.Igv);
                                        dd.Parameters.AddWithValue("@total", it.Total);
                                        dd.Parameters.AddWithValue("@EstDet", "1");
                                        dd.Parameters.AddWithValue("@FechaAten", "");
                                        dd.Parameters.AddWithValue("@TurnoAten", "");
                                        dd.Parameters.AddWithValue("@RegMedico", it.CodMed);
                                        dd.Parameters.AddWithValue("@MedicoEnvia", it.MedicoEnvia);
                                        dd.Parameters.AddWithValue("@Crea", Crea);
                                        dd.Parameters.AddWithValue("@Modifica", "");
                                        dd.Parameters.AddWithValue("@Elimina", "");
                                        dd.Parameters.AddWithValue("@Evento", "1");
                                        if (cueD != null)
                                        {
                                            cueD.Item++;
                                        }
                                        dd.ExecuteNonQuery();



                                    }
                                }
                            }
                            tr.Commit();
                            if (util.GENERARCUENTAAUTO)
                            {
                                return RedirectPermanent("~/Caja/RegistrarCaja?id=" + Resu);
                            }
                            else
                            {
                                if (util.PREGXATENCIONNOPROGRAMADAS)
                                {
                                    ViewBag.activaAlerta = 1;
                                }
                                else
                                {
                                    return RedirectPermanent("~/Cuentas/VerificaCuenta/" + Resu);
                                }
                            }

                            ViewBag.historia = ate.Historia;
                            ViewBag.cuenta = Resu;
                        }
                    }
                    catch (Exception e)
                    {
                        tr.Rollback();
                        return RedirectPermanent("../Master");
                    }
                    finally
                    {
                        con.Close();
                    }

                }
            }
            Session.Remove("atenciones");


            return View(ate);
        }






        [HttpPost]
        public ActionResult AsignarAtencion(E_AtencionVarias_Detalle ad)
        {


            string[] fija = ad.CodTar.Split(',');
            string total = "";
            string sede = Session["codSede"].ToString();

            ViewBag.esp = es.ListadoEspecialidades();
            ViewBag.ser = ser.ListadoServicios();
            ViewBag.tar = tar.ListadoTarifa();
            ViewBag.med = med.ListadoMedico();
            ViewBag.tiptar = (new TipoTarifaController()).ListadoTipoTarifa();

            ViewBag.listadoEspecialidad = new SelectList(es.ListadoEspecialidades().Where(x => x.EstEspec == true && x.CodSed == sede), "CodEspec", "NomEspec", ad.CodEspec);
            ViewBag.listadoEspecialidad = new SelectList(es.ListadoEspecialidades().Where(x => x.EstEspec == true && x.CodSed == sede), "CodEspec", "NomEspec", ad.EspeciEnvia);
            ViewBag.listadoServicios = new SelectList(ser.ListadoServicios().Where(x => x.EstServ == true && x.CodSede == sede), "CodServ", "NomServ", ad.CodServ);
            ViewBag.listadoMedico = new SelectList(med.ListadoMedico().Where(x => x.EstMed == true && x.CodSede == sede), "CodMed", "NomMed", ad.CodMed);
            ViewBag.listadoMedico = new SelectList(med.ListadoMedico().Where(x => x.EstMed == true && x.CodSede == sede), "CodMed", "NomMed", ad.MedicoEnvia);
            ViewBag.listadoTarifa = new SelectList(tar.ListadoTarifa().Where(x => x.EstTar == true && x.CodSede == sede), "CodTar", "DescTar", fija[0]);


            try
            {
                var Evalua = (List<E_AtencionVarias_Detalle>)Session["atenciones"];
                var registro = Evalua.Where(x => x.CodTar.Equals(fija[0])).FirstOrDefault();
                total = ad.CodEspec + "," + ad.CodServ + "," + ad.CodMed + "," + fija[0] + "," + ad.Cantida + "," + ad.Precio + "," + ad.Total + "," + ad.MedicoEnvia + "," + ad.CodCue;
                if (registro == null)
                {
                    if (fija[1] == "1")
                    {

                        E_Tarifario reg = tar.ListadoTarifa().Find(x => x.CodTar == fija[0]);

                        var formularios = (List<E_AtencionVarias_Detalle>)Session["atenciones"];

                        DatosGeneralesController da = new DatosGeneralesController();

                        E_Datos_Generales reg1 = da.listadatogenerales().FirstOrDefault();

                        decimal igvDemo = reg1.igv;
                        decimal igvv;
                        decimal precio;
                        decimal subtotal = 0;
                        decimal totalG = 0;
                        if (reg.AfecIgcv == true)
                        {
                            precio = (ad.Total / (1 + igvDemo));
                            //precio = decimal.Round(precio, 2);
                            igvv = precio * igvDemo;
                            //igvv = decimal.Round(igvv, 2);

                            subtotal = precio + igvv;

                            totalG = Decimal.Round(subtotal, 2);


                        }
                        else
                        {
                            igvv = 0;
                            precio = ad.Total;
                        }
                        int aleat0 = DateTime.Now.Millisecond;

                        E_AtencionVarias_Detalle item = new E_AtencionVarias_Detalle();
                        item.id = aleat0;
                        item.CodSede = sede;
                        item.CodEspec = ad.CodEspec;
                        item.CodServ = ad.CodServ;
                        if (ad.CodMed == "nulo")
                        {
                            item.CodMed = "[NO SELECCIONADO]";
                        }
                        else
                        {
                            item.CodMed = ad.CodMed;
                        }
                        item.CodTar = fija[0];
                        item.CodTipTar = reg.CodTipTar;
                        item.CodSTipTar = reg.CodSTipTar;
                        item.Cantida = ad.Cantida;
                        item.Precio = decimal.Round(ad.Total, 2);
                        item.SubTotal = decimal.Round(precio, 2);
                        item.Igv = decimal.Round(igvv, 2);
                        item.Total = totalG;
                        if (ad.MedicoEnvia == null)
                        {
                            item.MedicoEnvia = "[NO SELECCIONADO]";
                            item.EspeciEnvia = "[NO SELECCIONADO]";
                        }
                        else
                        {
                            E_Medico reg2 = med.ListadoMedico().Find(x => x.CodMed == ad.MedicoEnvia);
                            item.MedicoEnvia = ad.MedicoEnvia;
                            item.EspeciEnvia = reg2.CodEspec;
                        }
                        item.Estado = "G";
                        item.CodUsu = Session["UserID"].ToString();

                        formularios.Add(item);
                        Session["atenciones"] = formularios;

                        ViewBag.mensaje = "registro Agregado";

                    }
                    else
                    {

                        foreach (E_Promo it in (List<E_Promo>)ListadoPromocionesDetalle(int.Parse(fija[0])))
                        {

                            E_Tarifario reg = tar.ListadoTarifa().Find(x => x.CodTar == it.CodTar);

                            var formularios = (List<E_AtencionVarias_Detalle>)Session["atenciones"];

                            DatosGeneralesController da = new DatosGeneralesController();

                            E_Datos_Generales reg1 = da.listadatogenerales().FirstOrDefault();

                            decimal igvDemo = reg1.igv;
                            decimal igvv;
                            decimal precio;


                            if (reg.AfecIgcv == true)
                            {
                                precio = it.SubtotalD / (1 + igvDemo);
                                precio = decimal.Round(precio, 2);
                                igvv = precio * igvDemo;
                                igvv = decimal.Round(igvv, 2);
                            }
                            else
                            {
                                igvv = 0;
                                precio = it.SubtotalD;
                            }
                            int aleat0 = DateTime.Now.Millisecond;

                            E_AtencionVarias_Detalle item = new E_AtencionVarias_Detalle();
                            item.id = aleat0;
                            item.CodSede = sede;
                            item.CodEspec = ad.CodEspec;
                            item.CodServ = ad.CodServ;
                            if (ad.CodMed == "nulo")
                            {
                                item.CodMed = "[NO SELECCIONADO]";
                            }
                            else
                            {
                                item.CodMed = ad.CodMed;
                            }
                            item.CodTar = it.CodTar;
                            item.CodTipTar = it.CodTipTar;
                            item.CodSTipTar = "";
                            item.Cantida = it.cantidad;
                            item.Precio = it.SubtotalD;
                            item.SubTotal = precio;
                            item.Igv = igvv;
                            item.Total = precio + igvv;
                            if (ad.MedicoEnvia == null)
                            {
                                item.MedicoEnvia = "[NO SELECCIONADO]";
                                item.EspeciEnvia = "[NO SELECCIONADO]";
                            }
                            else
                            {
                                E_Medico reg2 = med.ListadoMedico().Find(x => x.CodMed == ad.MedicoEnvia);
                                item.MedicoEnvia = ad.MedicoEnvia;
                                item.EspeciEnvia = reg2.CodEspec;
                            }
                            item.Estado = "G";
                            item.CodUsu = Session["UserID"].ToString();

                            formularios.Add(item);
                            Session["atenciones"] = formularios;
                        }

                        ViewBag.mensaje = "registro Agregado";

                    }

                }

                if (ad.CodCueY != 0)
                {

                    return RedirectPermanent("RegistroAtenciones/?historia=" + ad.historia + "&cuenta=" + ad.CodCueY + "&cadena=" + total);

                }
                else
                {

                    return RedirectPermanent("RegistroAtenciones/?historia=" + ad.historia + "&cadena=" + total);

                }
            }
            catch (Exception)
            {
                if (ad.CodCueY != 0)
                {
                    return RedirectPermanent("RegistroAtenciones/?historia=" + ad.historia + "&cuenta=" + ad.CodCueY + "&cadena=" + total);
                }
                else
                {

                    return RedirectPermanent("RegistroAtenciones/?historia=" + ad.historia + "&cadena=" + total);

                }

            }


        }

        public ActionResult Delete(int id, int historia)
        {

            var formularios = (List<E_AtencionVarias_Detalle>)Session["atenciones"];
            var registro = formularios.Where(x => x.id.Equals(id)).FirstOrDefault();
            formularios.Remove(registro);

            Session["atenciones"] = formularios;

            return RedirectPermanent("../RegistroAtenciones/?historia=" + historia + "&flag=1");
        }


        public ActionResult ObtenerEspecialidad()
        {
            string sede = Session["codSede"].ToString();
            var evalua = (List<E_Especialidades>)es.ListadoEspecialidades().Where(x => x.CodSed == sede).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult ObtenerServicios(string CodEspec)
        {
            string sede = Session["codSede"].ToString();
            string usuario = Session["UserID"].ToString();
            UsuarioServicioController US = new UsuarioServicioController();
            ServiciosController ser = new ServiciosController();
            EspecialidadController es = new EspecialidadController();

            E_Usuario_Servicio verificaEspecialidad = US.ListaUsuarioServicio().Find(x => x.CodUsu == usuario && x.General == true);

            var evalua = (List<E_Usuario_Servicio>)US.ListaUsuarioServicio().Where(x => x.CodUsu == usuario && x.CodEsp == CodEspec).ToList();
            var evaluaEspe = (List<E_Servicios>)ser.ListadoServicios().Where(x => x.CodEspec == CodEspec).ToList();

            evalua.Find(x => x.General == true);

            if (verificaEspecialidad != null)
            {
                if (verificaEspecialidad.General == true)
                {

                    if (evaluaEspe.Count() != 0)
                    {
                        return Json(evaluaEspe, JsonRequestBehavior.AllowGet);
                    }
                    return null;
                }
            }
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
        }


        public ActionResult ObtenerMedico(string CodServ)
        {
            try
            {
                CitasController c = new CitasController();
                UtilitarioController u = new UtilitarioController();

                E_Master reg = u.ListadoHoraServidor().FirstOrDefault();

                string Fecha = reg.HoraServidor.ToString("dd/MM/yyyy");

                var evalua = (List<E_Citas>)c.BuscaMedico(CodServ, Fecha);
                if (evalua.Count() != 0)
                {
                    E_Citas cita = new E_Citas();
                    cita.CodMed = "nulo";
                    cita.NomMed = "[NO SELECCIONADO]";

                    evalua.Add(cita);

                    return Json(evalua, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    E_Citas cita = new E_Citas();
                    cita.CodMed = "nulo";
                    cita.NomMed = "[NO SELECCIONADO]";

                    evalua.Add(cita);

                    return Json(evalua, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public ActionResult ObtenerTarifa(string CodEspec)
        {
            string sede = Session["codSede"].ToString();

            var evalua = (List<E_Tarifario>)ListadoTarifaAtencionVarias(CodEspec).Where(x => x.EstTar == true).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult ObtenerPrecio(string CodTar, int Historia)
        {
            string[] fija = CodTar.Split(',');

            if (fija[0] != "")
            {
                PacientesController p = new PacientesController();
                E_Pacientes reg = p.ListadoPacientes().Find(x => x.Historia == Historia);
                var evalua = (List<E_Tarifa_CategoriaPaciente>)tar.ListadoCategoriaPacienteTarifa().Where(x => x.CodTar == fija[0] && x.CodCatPac == reg.CodCatPac).ToList();
                if (evalua.Count() != 0)
                {
                    return Json(evalua, JsonRequestBehavior.AllowGet);
                }
            }
            else if (fija[1] != "")
            {
                var evalua = (List<E_Promo>)ListadoPromociones().Where(x => x.IdPC == int.Parse(fija[1])).ToList();
                if (evalua.Count() != 0)
                {
                    return Json(evalua, JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }

        public ActionResult ObtenerPrecioHabil(string CodTar)
        {
            string sede = Session["codSede"].ToString();

            var evalua = (List<E_Tarifario>)ListadoTarifaAtencion().Where(x => x.CodTar == CodTar).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
        }


        public ActionResult ListadoAtencionesVarias(int? historia = null)
        {
            PacientesController pa = new PacientesController();
            ViewBag.paciente = new SelectList(pa.ListadoPacientes().Where(x => x.EstPac == true).ToList(), "Historia", "nombCompleto");
            string sede = Session["codSede"].ToString();

            if (historia != null)
            {
                try
                {
                    int historiaF = (int)historia;
                    ViewBag.evaluaTipo = (List<E_AtencionVarias>)ListadoAtencionVarias(historiaF).Where(x => x.CodSede == sede).ToList();
                    ViewBag.historia = historia;
                }
                catch (Exception e)
                {
                    ViewBag.mensaje = "Ingrese todos los campos para realizar la busqueda";
                }
            }
            else
            {
                ViewBag.historia = "";
            }

            return View();
        }


        public ActionResult EliminarAtencionVaria(int id)
        {
            int codigoCuenta = 0;
            CuentasController cu = new CuentasController();
            PacientesController pa = new PacientesController();
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string sede = Session["codSede"].ToString();

            string Modifica = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
            string usuario = Session["UserID"].ToString();
            E_AtencionVarias reg = ListadoAtencionVariasxID(id).FirstOrDefault();
            E_Cuentas cuentas = cu.BuscaCuenta(reg.CodCue).FirstOrDefault();
            var listadoAtencionDetalle = ListadoAtencionDetalle().Where(x => x.CodAten == id).ToList();

            decimal nuevoPrecio = 0;
            decimal nuevoIgv = 0;
            decimal nuevoTotal = 0;

            nuevoPrecio = cuentas.STotCue - reg.SubTotal;
            nuevoIgv = cuentas.IgvCue - reg.Igv;
            nuevoTotal = cuentas.TotCue - reg.Total;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                using (SqlCommand da = new SqlCommand("Usp_MtoCuentas", con, tr))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;

                        da.Parameters.AddWithValue("@CodCue", reg.CodCue);
                        da.Parameters.AddWithValue("@CodSede", "");
                        da.Parameters.AddWithValue("@Historia", "");
                        da.Parameters.AddWithValue("@CodcatPac", "");
                        da.Parameters.AddWithValue("@STotCue", nuevoPrecio);
                        da.Parameters.AddWithValue("@IgvCue", nuevoIgv);
                        da.Parameters.AddWithValue("@TotCue", nuevoTotal);
                        da.Parameters.AddWithValue("@FecCrea", "");
                        da.Parameters.AddWithValue("@FecAnul", "");
                        da.Parameters.AddWithValue("@EstCue", "1");
                        da.Parameters.AddWithValue("@EstGene", "");
                        da.Parameters.AddWithValue("@SecFact", "");
                        da.Parameters.AddWithValue("@Usuario", "");
                        da.Parameters.AddWithValue("@UsuarioAnula", "");
                        da.Parameters.AddWithValue("@Crea", "");
                        da.Parameters.AddWithValue("@Modifica", Modifica);
                        da.Parameters.AddWithValue("@Elimina", "");
                        da.Parameters.AddWithValue("@Evento", "2");

                        int Resu = (int)da.ExecuteScalar();

                        codigoCuenta = Resu;

                        using (SqlCommand cmd = new SqlCommand("Usp_MtoAtencionVarias", con, tr))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@CodAten", id);
                            cmd.Parameters.AddWithValue("@CodSede", "");
                            cmd.Parameters.AddWithValue("@CodCue", "");
                            cmd.Parameters.AddWithValue("@Historia", "");
                            cmd.Parameters.AddWithValue("@NomPac", "");
                            cmd.Parameters.AddWithValue("@CodCatPac", "");
                            cmd.Parameters.AddWithValue("@SubTotal", 0);
                            cmd.Parameters.AddWithValue("@Igv", 0);
                            cmd.Parameters.AddWithValue("@Total", 0);
                            cmd.Parameters.AddWithValue("@Fecha", "");
                            cmd.Parameters.AddWithValue("@CodUsu", "");
                            cmd.Parameters.AddWithValue("@EstConsul", "");
                            cmd.Parameters.AddWithValue("@Crea", "");
                            cmd.Parameters.AddWithValue("@Modifica", Modifica);
                            cmd.Parameters.AddWithValue("@Elimina", "");
                            cmd.Parameters.AddWithValue("@Evento", "3");

                            cmd.ExecuteNonQuery();


                            foreach (var it in listadoAtencionDetalle)
                            {

                                using (SqlCommand av = new SqlCommand("Usp_MtoAtencionVarias_Detalle ", con, tr))
                                {

                                    av.CommandType = CommandType.StoredProcedure;
                                    av.Parameters.AddWithValue("@CodAtenDet", it.CodAtenDet);
                                    av.Parameters.AddWithValue("@CodAten", "");
                                    av.Parameters.AddWithValue("@item", "");
                                    av.Parameters.AddWithValue("@CodSede", "");
                                    av.Parameters.AddWithValue("@CodEspec", "");
                                    av.Parameters.AddWithValue("@CodServ", "");
                                    av.Parameters.AddWithValue("@CodMed", "");
                                    av.Parameters.AddWithValue("@CodTar", "");
                                    av.Parameters.AddWithValue("@CodTipTar", "");
                                    av.Parameters.AddWithValue("@CodSTipTar", "");
                                    av.Parameters.AddWithValue("@Cantida", 0);
                                    av.Parameters.AddWithValue("@SubTotal", 0);
                                    av.Parameters.AddWithValue("@Igv", 0);
                                    av.Parameters.AddWithValue("@Total", 0);
                                    av.Parameters.AddWithValue("@MedicoEnvia", "");
                                    av.Parameters.AddWithValue("@EspeciEnvia", "");
                                    av.Parameters.AddWithValue("@Turno", "");
                                    av.Parameters.AddWithValue("@Estado", "");
                                    av.Parameters.AddWithValue("@CodUsu", "");
                                    av.Parameters.AddWithValue("@Crea", "");
                                    av.Parameters.AddWithValue("@Modifica", "");
                                    av.Parameters.AddWithValue("@Elimina", Modifica);
                                    av.Parameters.AddWithValue("@Evento", "3");

                                    av.ExecuteNonQuery();

                                    using (SqlCommand dd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr))
                                    {

                                        dd.CommandType = CommandType.StoredProcedure;
                                        dd.Parameters.AddWithValue("@Procedencia", 2);
                                        dd.Parameters.AddWithValue("@CodCue", reg.CodCue);
                                        dd.Parameters.AddWithValue("@Item", "");
                                        dd.Parameters.AddWithValue("@Tarifa", "");
                                        dd.Parameters.AddWithValue("@CodProce", id);
                                        dd.Parameters.AddWithValue("@CodDetalleP", it.CodAtenDet);
                                        dd.Parameters.AddWithValue("@CodSede", "");
                                        dd.Parameters.AddWithValue("@Cantidad", 0);
                                        dd.Parameters.AddWithValue("@precioUni", 0);
                                        dd.Parameters.AddWithValue("@precio", 0);
                                        dd.Parameters.AddWithValue("@igv", 0);
                                        dd.Parameters.AddWithValue("@total", 0);
                                        dd.Parameters.AddWithValue("@EstDet", "");
                                        dd.Parameters.AddWithValue("@FechaAten", "");
                                        dd.Parameters.AddWithValue("@TurnoAten", "");
                                        dd.Parameters.AddWithValue("@RegMedico", "");
                                        dd.Parameters.AddWithValue("@MedicoEnvia", "");
                                        dd.Parameters.AddWithValue("@Crea", "");
                                        dd.Parameters.AddWithValue("@Modifica", "");
                                        dd.Parameters.AddWithValue("@Elimina", Modifica);
                                        dd.Parameters.AddWithValue("@Evento", "4");

                                        dd.ExecuteNonQuery();


                                    }
                                }
                            }
                            tr.Commit();
                            ViewBag.mensaje = "Pedido registrado";
                        }
                    }
                    catch (Exception e)
                    {
                        tr.Rollback();
                        return RedirectPermanent("../Master");
                    }
                    finally
                    {
                        con.Close();
                    }
                }

            }
            return RedirectPermanent("../ListadoAtencionesVarias/?historia=" + reg.Historia);
        }


        public ActionResult ListadoAtencionesVariasDetalle(int id)
        {
            return View(ListadoAtencionDetalle().Where(x => x.CodAten == id));
        }


        public ActionResult EliminarAtencionVariaDetalle(int id)
        {
            int codigoCuenta = 0;
            CuentasController cu = new CuentasController();
            PacientesController pa = new PacientesController();
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string sede = Session["codSede"].ToString();

            string Modifica = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
            string usuario = Session["UserID"].ToString();
            E_AtencionVarias_Detalle reg1 = ListadoAtencionDetalle().Find(x => x.CodAtenDet == id);
            E_AtencionVarias reg = ListadoAtencionVariasxID(reg1.CodAten).FirstOrDefault();
            E_Cuentas cuentas = cu.BuscaCuenta(reg.CodCue).FirstOrDefault();

            decimal nuevoPrecioC = 0;
            decimal nuevoIgvC = 0;
            decimal nuevoTotalC = 0;

            nuevoPrecioC = reg.SubTotal - reg1.SubTotal;
            nuevoIgvC = reg.Igv - reg1.Igv;
            nuevoTotalC = reg.Total - reg1.Total;


            decimal nuevoPrecio = 0;
            decimal nuevoIgv = 0;
            decimal nuevoTotal = 0;

            nuevoPrecio = cuentas.STotCue - reg1.SubTotal;
            nuevoIgv = cuentas.IgvCue - reg1.Igv;
            nuevoTotal = cuentas.TotCue - reg1.Total;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                using (SqlCommand da = new SqlCommand("Usp_MtoCuentas", con, tr))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;

                        da.Parameters.AddWithValue("@CodCue", reg.CodCue);
                        da.Parameters.AddWithValue("@CodSede", "");
                        da.Parameters.AddWithValue("@Historia", "");
                        da.Parameters.AddWithValue("@CodcatPac", "");
                        da.Parameters.AddWithValue("@STotCue", nuevoPrecio);
                        da.Parameters.AddWithValue("@IgvCue", nuevoIgv);
                        da.Parameters.AddWithValue("@TotCue", nuevoTotal);
                        da.Parameters.AddWithValue("@FecCrea", "");
                        da.Parameters.AddWithValue("@FecAnul", "");
                        da.Parameters.AddWithValue("@EstCue", "1");
                        da.Parameters.AddWithValue("@EstGene", "");
                        da.Parameters.AddWithValue("@SecFact", "");
                        da.Parameters.AddWithValue("@Usuario", "");
                        da.Parameters.AddWithValue("@UsuarioAnula", "");
                        da.Parameters.AddWithValue("@Crea", "");
                        da.Parameters.AddWithValue("@Modifica", Modifica);
                        da.Parameters.AddWithValue("@Elimina", "");
                        da.Parameters.AddWithValue("@Evento", "2");

                        int Resu = (int)da.ExecuteScalar();

                        codigoCuenta = Resu;

                        using (SqlCommand cmd = new SqlCommand("Usp_MtoAtencionVarias", con, tr))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@CodAten", reg1.CodAten);
                            cmd.Parameters.AddWithValue("@CodSede", "");
                            cmd.Parameters.AddWithValue("@CodCue", "");
                            cmd.Parameters.AddWithValue("@Historia", "");
                            cmd.Parameters.AddWithValue("@NomPac", "");
                            cmd.Parameters.AddWithValue("@CodCatPac", "");
                            cmd.Parameters.AddWithValue("@SubTotal", nuevoPrecioC);
                            cmd.Parameters.AddWithValue("@Igv", nuevoIgvC);
                            cmd.Parameters.AddWithValue("@Total", nuevoTotalC);
                            cmd.Parameters.AddWithValue("@Fecha", "");
                            cmd.Parameters.AddWithValue("@CodUsu", "");
                            cmd.Parameters.AddWithValue("@EstConsul", "");
                            cmd.Parameters.AddWithValue("@Crea", "");
                            cmd.Parameters.AddWithValue("@Modifica", Modifica);
                            cmd.Parameters.AddWithValue("@Elimina", "");
                            cmd.Parameters.AddWithValue("@Evento", "2");

                            cmd.ExecuteNonQuery();


                            using (SqlCommand av = new SqlCommand("Usp_MtoAtencionVarias_Detalle ", con, tr))
                            {

                                av.CommandType = CommandType.StoredProcedure;
                                av.Parameters.AddWithValue("@CodAtenDet", id);
                                av.Parameters.AddWithValue("@CodAten", "");
                                av.Parameters.AddWithValue("@item", "");
                                av.Parameters.AddWithValue("@CodSede", "");
                                av.Parameters.AddWithValue("@CodEspec", "");
                                av.Parameters.AddWithValue("@CodServ", "");
                                av.Parameters.AddWithValue("@CodMed", "");
                                av.Parameters.AddWithValue("@CodTar", "");
                                av.Parameters.AddWithValue("@CodTipTar", "");
                                av.Parameters.AddWithValue("@CodSTipTar", "");
                                av.Parameters.AddWithValue("@Cantida", 0);
                                av.Parameters.AddWithValue("@SubTotal", 0);
                                av.Parameters.AddWithValue("@Igv", 0);
                                av.Parameters.AddWithValue("@Total", 0);
                                av.Parameters.AddWithValue("@MedicoEnvia", "");
                                av.Parameters.AddWithValue("@EspeciEnvia", "");
                                av.Parameters.AddWithValue("@Turno", "");
                                av.Parameters.AddWithValue("@Estado", "");
                                av.Parameters.AddWithValue("@CodUsu", "");
                                av.Parameters.AddWithValue("@Crea", "");
                                av.Parameters.AddWithValue("@Modifica", "");
                                av.Parameters.AddWithValue("@Elimina", Modifica);
                                av.Parameters.AddWithValue("@Evento", "3");

                                av.ExecuteNonQuery();

                                using (SqlCommand dd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr))
                                {

                                    dd.CommandType = CommandType.StoredProcedure;
                                    dd.Parameters.AddWithValue("@Procedencia", 2);
                                    dd.Parameters.AddWithValue("@CodCue", reg.CodCue);
                                    dd.Parameters.AddWithValue("@Item", "");
                                    dd.Parameters.AddWithValue("@Tarifa", "");
                                    dd.Parameters.AddWithValue("@CodProce", reg1.CodAten);
                                    dd.Parameters.AddWithValue("@CodDetalleP", id);
                                    dd.Parameters.AddWithValue("@CodSede", "");
                                    dd.Parameters.AddWithValue("@Cantidad", 0);
                                    dd.Parameters.AddWithValue("@precioUni", 0);
                                    dd.Parameters.AddWithValue("@precio", 0);
                                    dd.Parameters.AddWithValue("@igv", 0);
                                    dd.Parameters.AddWithValue("@total", 0);
                                    dd.Parameters.AddWithValue("@EstDet", "");
                                    dd.Parameters.AddWithValue("@FechaAten", "");
                                    dd.Parameters.AddWithValue("@TurnoAten", "");
                                    dd.Parameters.AddWithValue("@RegMedico", "");
                                    dd.Parameters.AddWithValue("@MedicoEnvia", "");
                                    dd.Parameters.AddWithValue("@Crea", "");
                                    dd.Parameters.AddWithValue("@Modifica", "");
                                    dd.Parameters.AddWithValue("@Elimina", Modifica);
                                    dd.Parameters.AddWithValue("@Evento", "4");

                                    dd.ExecuteNonQuery();


                                }
                            }
                        }
                        tr.Commit();
                        ViewBag.mensaje = "Pedido registrado";

                    }
                    catch (Exception e)
                    {
                        tr.Rollback();
                        return RedirectPermanent("../Master");
                    }
                    finally
                    {
                        con.Close();
                    }
                }

            }
            return RedirectPermanent("../ListadoAtencionesVariasDetalle/" + reg1.CodAten);
        }


        public ActionResult ImprimirHistoriaClinica(int Historia, string CodEspec)
        {

            var Lista = ListarDatosHistoriaClinica(Historia).FirstOrDefault();

            ViewBag.ListaEspec = ListarEspecialidadImprimirHistoria(CodEspec);

            return View(Lista);
        }


        public List<E_Pacientes> ListarDatosHistoriaClinica(int Historia)
        {
            List<E_Pacientes> Lista = new List<E_Pacientes>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("UspListarDatosHistoriaClinica", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Historia", Historia);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            E_Pacientes p = new E_Pacientes();
                            p.Historia = Convert.ToInt32(dr["Historia"]);
                            p.nombCompleto = dr["NombrePaciente"].ToString();
                            p.NonSex = dr["NomSexo"].ToString();
                            p.NomEstCivil = dr["NomEstCivil"].ToString();
                            p.LugarNac = dr["LugarNac"].ToString();
                            p.FecNac = Convert.ToDateTime(dr["FecNac"].ToString());
                            p.Edad = dr["Edad"].ToString();
                            p.NomDist = dr["NomDist"].ToString();
                            p.Direcc = dr["Direcc"].ToString();
                            p.TelfFijo = dr["TelfFijo"].ToString();
                            p.TelfCel = dr["TelfCel"].ToString();
                            p.Email = dr["Email"].ToString();
                            p.FecAfil = Convert.ToDateTime(dr["FecAfil"].ToString());
                            p.Crea = dr["Crea"].ToString();

                            //p.DescTar = dr["DescTar"] is DBNull ? string.Empty : dr["DescTar"].ToString();

                            Lista.Add(p);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        //Listamos la especialidad
        public List<E_Pacientes> ListarEspecialidadImprimirHistoria(string CodEspec)
        {
            List<E_Pacientes> Lista = new List<E_Pacientes>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("UspListarEspecialidadImprimirHistoria", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodEspec", CodEspec);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            E_Pacientes p = new E_Pacientes();
                            p.NomEspec = dr["NomEspec"].ToString();

                            //p.DescTar = dr["DescTar"] is DBNull ? string.Empty : dr["DescTar"].ToString();

                            Lista.Add(p);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        [HttpPost]
        public JsonResult CargaServicios(int historia)
        {
            string sede = Session["codSede"].ToString();
            var listado = ser.ListadoServiciosVentaRapida(sede, historia).ToList();
            StringBuilder sb = new StringBuilder();

            foreach (var a in listado)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine("<td>" + a.NomServ + "</td>");
                sb.AppendLine("<td>" +
                    "<input type='radio' onclick=generaTarifa('" + a.CodServ + "') name='ckServicio' id='ckServicio' value='" + a.CodServ + "," + a.CodEspec + "," + a.CodTar + "," + a.NomServ + "," + a.precio + "' />" +
                    "</td>");
                sb.AppendLine("</tr>");
            }
            string result = "";
            result = sb.ToString();

            return Json(new { result, success = true }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult RegistroVentaRapida(string[] cabecera, string[] detalle)
        {
            var valorCabecera = cabecera[0].Split(',');
            UtilitarioController ut = new UtilitarioController();
            PacientesController pa = new PacientesController();
            CajaController caja = new CajaController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            int hora = horaSis.HoraServidor.Hour;
            string turno = "";
            if (hora < 13)
            {
                turno = "MAÑANA";
            }
            else
            {
                turno = "TARDE";
            }
            string Crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
            string usuario = Session["UserID"].ToString();
            var series = caja.ListadoUsuarioSerie(usuario).Find(x => x.Prioridad == true);
            var serieDoc = series.Etiqueta.Split('-');
            string sede = Session["codSede"].ToString();
            E_Pacientes reg = pa.ListadoPacientes().Find(x => x.Historia == int.Parse(valorCabecera[0].ToString()));
            string nomPac = reg.NomPac + " " + reg.ApePat + " " + reg.ApeMat;
            decimal igvv;
            decimal precio;
            decimal subtotal = 0;
            decimal totalG = 0;
            DatosGeneralesController dat = new DatosGeneralesController();
            TipoMonedaController mo = new TipoMonedaController();
            E_Datos_Generales reg1 = dat.listadatogenerales().FirstOrDefault();
            decimal igvDemo = reg1.igv;

            precio = (decimal.Parse(valorCabecera[1].ToString()) / (1 + igvDemo));
            igvv = precio * igvDemo;
            subtotal = precio + igvv;

            totalG = Decimal.Round(subtotal, 2);
            E_DocumentoSerie correlativo = caja.ListadoCorrelativo(series.CodDocSerie).FirstOrDefault();
            E_TipoMoneda evalua = mo.ListadoTipoMoneda1().Find(x => x.fechaParse == horaSis.HoraServidor.ToShortDateString() && x.CodTipMon == "TM002");

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);

                using (SqlCommand da = new SqlCommand("Usp_MtoCuentas", con, tr))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;

                        da.Parameters.AddWithValue("@CodCue", "");
                        da.Parameters.AddWithValue("@CodSede", sede);
                        da.Parameters.AddWithValue("@Historia", int.Parse(valorCabecera[0]));
                        da.Parameters.AddWithValue("@CodcatPac", reg.CodCatPac);
                        da.Parameters.AddWithValue("@STotCue", subtotal);
                        da.Parameters.AddWithValue("@IgvCue", igvv);
                        da.Parameters.AddWithValue("@TotCue", totalG);
                        da.Parameters.AddWithValue("@FecCrea", "");
                        da.Parameters.AddWithValue("@FecAnul", "");
                        da.Parameters.AddWithValue("@EstCue", "1");
                        da.Parameters.AddWithValue("@EstGene", "");
                        da.Parameters.AddWithValue("@SecFact", "");
                        da.Parameters.AddWithValue("@Usuario", usuario);
                        da.Parameters.AddWithValue("@UsuarioAnula", "");
                        da.Parameters.AddWithValue("@Crea", Crea);
                        da.Parameters.AddWithValue("@Modifica", Crea);
                        da.Parameters.AddWithValue("@Elimina", "");
                        da.Parameters.AddWithValue("@Evento", "1");

                        int Resu = (int)da.ExecuteScalar();
                        
                        using (SqlCommand cmd = new SqlCommand("Usp_MtoAtencionVarias", con, tr))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@CodAten", "");
                            cmd.Parameters.AddWithValue("@CodSede", sede);
                            cmd.Parameters.AddWithValue("@CodCue", Resu);
                            cmd.Parameters.AddWithValue("@Historia", int.Parse(valorCabecera[0]));
                            cmd.Parameters.AddWithValue("@NomPac", nomPac.ToString());
                            cmd.Parameters.AddWithValue("@CodCatPac", reg.CodCatPac);
                            cmd.Parameters.AddWithValue("@SubTotal", subtotal);
                            cmd.Parameters.AddWithValue("@Igv", igvv);
                            cmd.Parameters.AddWithValue("@Total", totalG);
                            cmd.Parameters.AddWithValue("@Fecha", "");
                            cmd.Parameters.AddWithValue("@CodUsu", usuario);
                            cmd.Parameters.AddWithValue("@EstConsul", "");
                            cmd.Parameters.AddWithValue("@Crea", Crea);
                            cmd.Parameters.AddWithValue("@Modifica", "");
                            cmd.Parameters.AddWithValue("@Elimina", "");
                            cmd.Parameters.AddWithValue("@Evento", "1");
                            int codAtencion = (int)cmd.ExecuteScalar();
                            int CodigoCuentaResult = Resu;
                            
                            using (SqlCommand ca = new SqlCommand("Usp_MtoCaja", con, tr))
                            {
                                ca.CommandType = CommandType.StoredProcedure;
                                ca.Parameters.AddWithValue("@CodCaja", "");
                                ca.Parameters.AddWithValue("@CodSede", sede);
                                ca.Parameters.AddWithValue("@CodCue", Resu);
                                ca.Parameters.AddWithValue("@CodDocSerie", series.CodDocSerie);
                                ca.Parameters.AddWithValue("@Serie", serieDoc[1].ToString());
                                ca.Parameters.AddWithValue("@NumDoc", correlativo.Serie);
                                ca.Parameters.AddWithValue("@FechaEmision", "");
                                ca.Parameters.AddWithValue("@HoraEmision", horaSis.HoraServidor.ToShortTimeString());
                                ca.Parameters.AddWithValue("@Historia", int.Parse(valorCabecera[0]));
                                ca.Parameters.AddWithValue("@NomPac", nomPac);
                                ca.Parameters.AddWithValue("@DirecPac", reg.Direcc.ToUpper());
                                ca.Parameters.AddWithValue("@Ruc", reg.NumDoc);
                                ca.Parameters.AddWithValue("@RazonSocial", nomPac);
                                ca.Parameters.AddWithValue("@DirRazSoc", reg.Direcc.ToUpper());
                                ca.Parameters.AddWithValue("@CodCatPac", reg.CodCatPac);
                                ca.Parameters.AddWithValue("@Estado", "1");
                                ca.Parameters.AddWithValue("@SubTotal", subtotal);
                                ca.Parameters.AddWithValue("@Igv", igvv);
                                ca.Parameters.AddWithValue("@Total", totalG);
                                ca.Parameters.AddWithValue("@UsuCrea", usuario);
                                ca.Parameters.AddWithValue("@UsuAnula", "");
                                ca.Parameters.AddWithValue("@FechaAnula", "");
                                ca.Parameters.AddWithValue("@TipoPago", "Contado");
                                ca.Parameters.AddWithValue("@CodRazSoc", reg.NumDoc);
                                ca.Parameters.AddWithValue("@CodTipMon", "TM001");
                                ca.Parameters.AddWithValue("@TipoCambio", evalua.TipoCambio);
                                ca.Parameters.AddWithValue("@Obser", "");
                                ca.Parameters.AddWithValue("@TazaIgv", igvDemo);
                                ca.Parameters.AddWithValue("@AutorizaAnu", "");
                                ca.Parameters.AddWithValue("@PorAnular", "");
                                ca.Parameters.AddWithValue("@SecVenta", caja.Generador_De_Codigo());
                                ca.Parameters.AddWithValue("@Crea", Crea);
                                ca.Parameters.AddWithValue("@Modifica", "");
                                ca.Parameters.AddWithValue("@Elimina", "");
                                ca.Parameters.AddWithValue("@Evento", "1");

                                int CodCaja = (int)ca.ExecuteScalar();

                                using (SqlCommand cue = new SqlCommand("Usp_MtoCuentas", con, tr))
                                {

                                    cue.CommandType = CommandType.StoredProcedure;
                                    cue.Parameters.AddWithValue("@CodCue", Resu);
                                    cue.Parameters.AddWithValue("@CodSede", "");
                                    cue.Parameters.AddWithValue("@Historia", "");
                                    cue.Parameters.AddWithValue("@CodcatPac", "");
                                    cue.Parameters.AddWithValue("@STotCue", 0);
                                    cue.Parameters.AddWithValue("@IgvCue", 0);
                                    cue.Parameters.AddWithValue("@TotCue", 0);
                                    cue.Parameters.AddWithValue("@FecCrea", "");
                                    cue.Parameters.AddWithValue("@FecAnul", "");
                                    cue.Parameters.AddWithValue("@EstCue", "1");
                                    cue.Parameters.AddWithValue("@EstGene", "");
                                    cue.Parameters.AddWithValue("@SecFact", CodCaja);
                                    cue.Parameters.AddWithValue("@Usuario", "");
                                    cue.Parameters.AddWithValue("@UsuarioAnula", "");
                                    cue.Parameters.AddWithValue("@Crea", "");
                                    cue.Parameters.AddWithValue("@Modifica", Crea);
                                    cue.Parameters.AddWithValue("@Elimina", "");
                                    cue.Parameters.AddWithValue("@Evento", "4");
                                    cue.ExecuteNonQuery();


                                    int contador = 1;
                                    foreach (var a in detalle)
                                    {
                                        var valorDetalle = a.Split(',');
                                        E_Tarifario regi = tar.ListadoTarifa().Find(x => x.CodTar == valorDetalle[0].ToString());

                                        if (regi.AfecIgcv == true)
                                        {
                                            precio = (decimal.Parse(valorDetalle[1].ToString()) * int.Parse(valorDetalle[2]) / (1 + igvDemo));
                                            igvv = precio * igvDemo;
                                            subtotal = precio + igvv;
                                            totalG = Decimal.Round(subtotal, 2);
                                        }
                                        else
                                        {
                                            precio = (int.Parse(valorDetalle[1].ToString()) * decimal.Parse(valorDetalle[2]));
                                            igvv = 0;
                                            subtotal = precio + igvv;
                                            totalG = Decimal.Round(subtotal, 2);
                                        }
                                        
                                        using (SqlCommand av = new SqlCommand("Usp_MtoAtencionVarias_Detalle ", con, tr))
                                        {


                                            av.CommandType = CommandType.StoredProcedure;
                                            av.Parameters.AddWithValue("@CodAtenDet", "");
                                            av.Parameters.AddWithValue("@CodAten", codAtencion);
                                            av.Parameters.AddWithValue("@item", contador);
                                            av.Parameters.AddWithValue("@CodSede", sede);
                                            av.Parameters.AddWithValue("@CodEspec", valorDetalle[4].ToString());
                                            av.Parameters.AddWithValue("@CodServ", valorDetalle[3].ToString());
                                            av.Parameters.AddWithValue("@CodMed", "");
                                            av.Parameters.AddWithValue("@CodTar", valorDetalle[0].ToString());
                                            av.Parameters.AddWithValue("@CodTipTar", regi.CodTipTar);
                                            av.Parameters.AddWithValue("@CodSTipTar", regi.CodSTipTar);
                                            av.Parameters.AddWithValue("@Cantida", int.Parse(valorDetalle[1].ToString()));
                                            av.Parameters.AddWithValue("@SubTotal", subtotal);
                                            av.Parameters.AddWithValue("@Igv", igvv);
                                            av.Parameters.AddWithValue("@Total", totalG);
                                            av.Parameters.AddWithValue("@MedicoEnvia", "");
                                            av.Parameters.AddWithValue("@EspeciEnvia", "");
                                            av.Parameters.AddWithValue("@Turno", turno);
                                            av.Parameters.AddWithValue("@Estado", 'G');
                                            av.Parameters.AddWithValue("@CodUsu", usuario);
                                            av.Parameters.AddWithValue("@Crea", Crea);
                                            av.Parameters.AddWithValue("@Modifica", "");
                                            av.Parameters.AddWithValue("@Elimina", "");
                                            av.Parameters.AddWithValue("@Evento", "1");

                                            int codAtencionDetalle = (int)av.ExecuteScalar();

                                            using (SqlCommand dd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr))
                                            {

                                                dd.CommandType = CommandType.StoredProcedure;
                                                dd.Parameters.AddWithValue("@Procedencia", 2);
                                                dd.Parameters.AddWithValue("@CodCue", Resu);
                                                dd.Parameters.AddWithValue("@Item", contador);
                                                dd.Parameters.AddWithValue("@Tarifa", valorDetalle[0].ToString());
                                                dd.Parameters.AddWithValue("@CodProce", codAtencion);
                                                dd.Parameters.AddWithValue("@CodDetalleP", codAtencionDetalle);
                                                dd.Parameters.AddWithValue("@CodSede", sede);
                                                dd.Parameters.AddWithValue("@Cantidad", int.Parse(valorDetalle[1].ToString()));
                                                dd.Parameters.AddWithValue("@precioUni", decimal.Parse(valorDetalle[2].ToString()));
                                                dd.Parameters.AddWithValue("@precio", subtotal);
                                                dd.Parameters.AddWithValue("@igv", igvv);
                                                dd.Parameters.AddWithValue("@total", totalG);
                                                dd.Parameters.AddWithValue("@EstDet", "1");
                                                dd.Parameters.AddWithValue("@FechaAten", "");
                                                dd.Parameters.AddWithValue("@TurnoAten", "");
                                                dd.Parameters.AddWithValue("@RegMedico", "");
                                                dd.Parameters.AddWithValue("@MedicoEnvia", "");
                                                dd.Parameters.AddWithValue("@Crea", Crea);
                                                dd.Parameters.AddWithValue("@Modifica", "");
                                                dd.Parameters.AddWithValue("@Elimina", "");
                                                dd.Parameters.AddWithValue("@Evento", "1");
                                                
                                                dd.ExecuteNonQuery();

                                                using (SqlCommand cajDet = new SqlCommand("Usp_MtoCaja_Detalle", con, tr))
                                                {

                                                    cajDet.CommandType = CommandType.StoredProcedure;


                                                    cajDet.Parameters.AddWithValue("@CodSede", sede);
                                                    cajDet.Parameters.AddWithValue("@CodCaja", Resu);
                                                    cajDet.Parameters.AddWithValue("@CodCue", Resu);
                                                    cajDet.Parameters.AddWithValue("@item", contador);
                                                    cajDet.Parameters.AddWithValue("@CodTar", valorDetalle[0].ToString());
                                                    cajDet.Parameters.AddWithValue("@NomTar", regi.DescTar.ToUpper());
                                                    cajDet.Parameters.AddWithValue("@Cantidad", int.Parse(valorDetalle[1].ToString()));
                                                    cajDet.Parameters.AddWithValue("@PUnit", decimal.Parse(valorDetalle[2].ToString()));
                                                    cajDet.Parameters.AddWithValue("@SubTotal", subtotal);
                                                    cajDet.Parameters.AddWithValue("@Igv", igvv);
                                                    cajDet.Parameters.AddWithValue("@Total", totalG);
                                                    cajDet.Parameters.AddWithValue("@TazaIgv", igvDemo);
                                                    cajDet.Parameters.AddWithValue("@Crea", Crea);
                                                    cajDet.Parameters.AddWithValue("@Modifica", "");
                                                    cajDet.Parameters.AddWithValue("@Elimina", "");
                                                    cajDet.Parameters.AddWithValue("@Evento", "1");
                                                    cajDet.ExecuteNonQuery();

                                                    using (SqlCommand cpago = new SqlCommand("Usp_MtoCaja_Pago", con, tr))
                                                    {

                                                        cpago.CommandType = CommandType.StoredProcedure;

                                                        cpago.Parameters.AddWithValue("@CodCaja", CodCaja);
                                                        cpago.Parameters.AddWithValue("@item", contador);
                                                        cpago.Parameters.AddWithValue("@CODMEDIOS", "ME001");
                                                        cpago.Parameters.AddWithValue("@Importe", decimal.Parse(valorDetalle[2].ToString()));
                                                        cpago.Parameters.AddWithValue("@ImporteSoles", decimal.Parse(valorDetalle[2].ToString()));
                                                        cpago.Parameters.AddWithValue("@CodTipMon", "TM001");
                                                        cpago.Parameters.AddWithValue("@TipoCambio", evalua.TipoCambio);
                                                        cpago.Parameters.AddWithValue("@Estado", "1");
                                                        cpago.Parameters.AddWithValue("@Crea", Crea);
                                                        cpago.Parameters.AddWithValue("@Modifica", "");
                                                        cpago.Parameters.AddWithValue("@Elimina", "");
                                                        cpago.Parameters.AddWithValue("@Evento", "1");
                                                        cpago.ExecuteNonQuery();
                                                    }

                                                }
                                            }
                                        }

                                    }
                                    
                                }
                            }
                            tr.Commit();
                            
                        }
                    }
                    catch (Exception e)
                    {
                        tr.Rollback();
                    }
                    finally
                    {
                        con.Close();
                    }

                }
            }





            string result = "";
            return Json(new { result, success = true }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult CargaTarifa(string CodServ, int historia, string descripcion)
        {
            int clave = 1;
            string sede = Session["codSede"].ToString();
            var listado = ListadoTarifVentaRapida(CodServ, sede, historia, descripcion).ToList();
            StringBuilder sb = new StringBuilder();

            foreach (var a in listado)
            {
                sb.AppendLine("<tr data-id='" + a.procedencia + "'>");
                sb.AppendLine("<td>" + a.DescTar + "</td>");
                sb.AppendLine("<td>" +
                    "<input type='text' name='cantidad' id='cantidad-" + clave + "' value='1' />" +
                    "</td>");
                if (a.ModPrecio == true)
                {
                    sb.AppendLine("<td>" +
                    "<input type='text' name='precio' id='precio-" + clave + "' value='" + a.Precio + "' />" +
                    "</td>");
                }
                else
                {
                    sb.AppendLine("<td>" +
                    "<input type='text' name='precio' id='precio-" + clave + "' value='" + a.Precio + "' disabled />" +
                    "</td>");
                }

                sb.AppendLine("<td>" +
                    "<input type='hidden' name='tarifa' id='tarifa-" + clave + "' value='" + a.CodTar + "' />" +
                    "<a onclick=registraItem('" + clave + "') href='#'><i class='fa fa-check'></i></a>" + "</td>");

                sb.AppendLine("</tr>");

                clave++;
            }
            string result = "";
            result = sb.ToString();

            return Json(new { result, success = true }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult VentaRapida(int historia)
        {
            ViewBag.historia = historia;
            return View();
        }


    }
}