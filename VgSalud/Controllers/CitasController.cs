using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using VgSalud.Models;
using System.Configuration;
using System.Globalization;

namespace VgSalud.Controllers
{
    public class CitasController : Controller
    {
        public List<E_Tipo_Tarifa> ListadoTipoTarifaCitas()
        {
            List<E_Tipo_Tarifa> Lista = new List<E_Tipo_Tarifa>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_TipoTarifa_Citas", con))
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
        public List<E_Citas> BuscaMedico(string codigo, string dia)
        {
            List<E_Citas> Lista = new List<E_Citas>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_buscaMedico_CodEsp", con))
                {
                    cmd.Parameters.AddWithValue("@CodServ", codigo);
                    cmd.Parameters.AddWithValue("@dia", dia);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Citas Ser = new E_Citas();

                            Ser.CodMed = dr.GetString(0);
                            Ser.NomMed = dr.GetString(1);
                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Citas> BuscaTurno(string codigo, string dia)
        {
            List<E_Citas> Lista = new List<E_Citas>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_buscaTurno", con))
                {
                    cmd.Parameters.AddWithValue("@CodMed", codigo);
                    cmd.Parameters.AddWithValue("@dia", dia);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Citas Ser = new E_Citas();

                            Ser.Turno = dr.GetString(0);
                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Citas> ConsultaCitas(string CodEspec, string CodServ, string CodMed, string dia, string turno)
        {
            List<E_Citas> Lista = new List<E_Citas>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ConsultaCitas", con))
                {
                    cmd.Parameters.AddWithValue("@CodEspec", CodEspec);
                    cmd.Parameters.AddWithValue("@CodServ", CodServ);
                    cmd.Parameters.AddWithValue("@CodMed", CodMed);
                    cmd.Parameters.AddWithValue("@dia", dia);
                    cmd.Parameters.AddWithValue("@Turno", turno.ToUpper());
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Citas Ser = new E_Citas();

                            Ser.CodMed = dr.GetString(0);
                            Ser.CodServ = dr.GetString(1);
                            Ser.CodEspec = dr.GetString(2);
                            Ser.NomMed = dr.GetString(3);
                            Ser.horaI = dr.GetTimeSpan(4);
                            Ser.horaF = dr.GetTimeSpan(5);
                            Ser.intMin = dr.GetInt32(6);
                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Citas> ListadoCitas()
        {
            List<E_Citas> Lista = new List<E_Citas>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaCitas", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Citas Ser = new E_Citas();
                            Ser.CodCita = dr.GetInt32(0);
                            Ser.Historia = dr.GetInt32(1);
                            Ser.CodEspec = dr.GetString(2);
                            Ser.CodServ = dr.GetString(3);
                            Ser.CodMed = dr.GetString(4);
                            Ser.TipoPac = dr.GetString(5);
                            Ser.Obser = dr.GetString(6);
                            Ser.CodCatPac = dr.GetString(7);
                            Ser.precio = dr.GetDecimal(8);
                            Ser.igv = dr.GetDecimal(9);
                            Ser.total = dr.GetDecimal(10);
                            Ser.CodTar = dr.GetString(11);
                            Ser.CodTipTar = dr.GetString(12);
                            Ser.horaI = dr.GetTimeSpan(13);
                            Ser.horaF = dr.GetTimeSpan(14);
                            Ser.FechaRegistro = dr.GetDateTime(15);
                            Ser.fechaCitas = dr.GetDateTime(16);
                            Ser.Turno = dr.GetString(17);
                            Ser.CodCue = dr.GetInt32(18);
                            Ser.Estado = dr.GetString(19);
                            Ser.Usuario = dr.GetString(20);
                            Ser.TipoRegistro = dr.GetString(21);

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Citas> buscaCitas(string id)
        {
            List<E_Citas> Lista = new List<E_Citas>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_buscaCita", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodCita", id);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Citas Ser = new E_Citas();
                            Ser.CodCita = dr.GetInt32(0);
                            Ser.NombrePaciente = dr.GetString(1);
                            Ser.NomEspec = dr.GetString(2);
                            Ser.NomServ = dr.GetString(3);
                            Ser.TipoPac = dr.GetString(4);
                            Ser.NombreCatePaciente = dr.GetString(5);
                            Ser.DescTar = dr.GetString(6);
                            Ser.DescTipoTar = dr.GetString(7);
                            Ser.NomMed = dr.GetString(8);
                            Ser.horaI = dr.GetTimeSpan(9);
                            Ser.horaF = dr.GetTimeSpan(10);
                            Ser.fechaCitas = dr.GetDateTime(11);
                            Ser.Turno = dr.GetString(12);
                            Ser.Consultorio = dr.GetString(13);
                            Ser.precio = dr.GetDecimal(14);
                            Ser.igv = dr.GetDecimal(15);
                            Ser.total = dr.GetDecimal(16);
                            Ser.TipoConsu = dr.GetString(17);
                            Ser.CodCue = dr.GetInt32(18);
                            Ser.Historia = dr.GetInt32(19);

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Citas> ListaGeneralCitas(string especialidad, string servicio, DateTime fecha, string turno)
        {
            List<E_Citas> Lista = new List<E_Citas>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ListaCitasGeneral", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodEspec", especialidad);
                    cmd.Parameters.AddWithValue("@CodServ", servicio);
                    cmd.Parameters.AddWithValue("@Fecha", fecha);
                    cmd.Parameters.AddWithValue("@turno", turno.ToUpper());
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Citas Ser = new E_Citas();
                            Ser.CodCita = dr.GetInt32(0);
                            Ser.NombrePaciente = dr.GetString(1);
                            Ser.NomEspec = dr.GetString(2);
                            Ser.NomServ = dr.GetString(3);
                            Ser.TipoPac = dr.GetString(4);
                            Ser.NomMed = dr.GetString(5);
                            Ser.horaI = dr.GetTimeSpan(6);
                            Ser.horaF = dr.GetTimeSpan(7);
                            Ser.fechaCitas = dr.GetDateTime(8);
                            Ser.Turno = dr.GetString(9);
                            Ser.Consultorio = dr.GetString(10);
                            Ser.precio = dr.GetDecimal(11);
                            Ser.igv = dr.GetDecimal(12);
                            Ser.total = dr.GetDecimal(13);
                            Ser.TipoConsu = dr.GetString(14);
                            Ser.CodCue = dr.GetInt32(15);
                            Ser.Historia = dr.GetInt32(16);
                            Ser.Estado = dr.GetString(17);

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_Citas> ConsultaCitasEspecial(string CodMed, string turno, string FechaCita)
        {
            List<E_Citas> Lista = new List<E_Citas>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaEspecialCitas", con))
                {
                    cmd.Parameters.AddWithValue("@CodMed", CodMed);
                    cmd.Parameters.AddWithValue("@Turno", turno);
                    cmd.Parameters.AddWithValue("@FechaCita", FechaCita);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Citas Ser = new E_Citas();

                            Ser.CodCita = dr.GetInt32(0);
                            Ser.horaI = dr.GetTimeSpan(1);
                            Ser.horaF = dr.GetTimeSpan(2);
                            Ser.NombrePaciente = dr.GetString(3);
                            Ser.Estado = dr.GetString(4);
                            Ser.DescTar = dr.GetString(5);

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Citas> ConsultaCitasEspecialConflictos(string CodMed, string turno, string FechaCita, string horaI, string horaF)
        {
            List<E_Citas> Lista = new List<E_Citas>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ConsultaCitasConflictos", con))
                {
                    cmd.Parameters.AddWithValue("@CodMed", CodMed);
                    cmd.Parameters.AddWithValue("@Turno", turno);
                    cmd.Parameters.AddWithValue("@FechaCita", FechaCita);
                    cmd.Parameters.AddWithValue("@horaI", horaI);
                    cmd.Parameters.AddWithValue("@horaF", horaF);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Citas Ser = new E_Citas();

                            Ser.CodCita = dr.GetInt32(0);
                            Ser.horaI = dr.GetTimeSpan(1);
                            Ser.horaF = dr.GetTimeSpan(2);
                            Ser.NombrePaciente = dr.GetString(3);
                            Ser.Estado = dr.GetString(4);

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Tarifario> ListadoTarifaAtencion()
        {
            List<E_Tarifario> Lista = new List<E_Tarifario>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaTipoTarifaCitas", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Tarifario Etip = new E_Tarifario();

                            Etip.CodTar = dr.GetString(0);
                            Etip.DescTar = dr.GetString(1);
                            Etip.Precio = dr.GetDecimal(2);
                            Etip.AfecIgcv = dr.GetBoolean(3);
                            Etip.ModPrecTar = dr.GetBoolean(4);
                            Etip.CodEspec = dr.GetString(5);
                            Etip.CodTipTar = dr.GetString(6);
                            Etip.CodSTipTar = dr.GetString(7);
                            Etip.CodTipMon = dr.GetString(8);
                            Etip.CodSede = dr.GetString(9);
                            Etip.ModPrecio = dr.GetBoolean(10);
                            Etip.EstTar = dr.GetBoolean(11);
                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        

        EspecialidadController es = new EspecialidadController();
        ServiciosController ser = new ServiciosController();
        TarifarioController tar = new TarifarioController();
        PacientesController pa = new PacientesController();
        CategoriaPacienteController caP = new CategoriaPacienteController();
        TipoTarifaController tipT = new TipoTarifaController();
        public ActionResult AsignarTipoTarifa()
        {
            string sede = Session["codSede"].ToString();
           
            ViewBag.listadoTipoTarifa = new SelectList(tipT.ListadoTipoTarifa().Where(d => d.EstTipTar == true), "CodTipTar", "DescTipTar");
            ViewBag.listaTarifa = (List<E_Tipo_Tarifa>)ListadoTipoTarifaCitas().Where(x => x.CodSede == sede).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult AsignarTipoTarifa(E_Tipo_Tarifa tip)
        {

            try
            {
                string sede = Session["codSede"].ToString();
                ViewBag.listaTarifa = (List<E_Tipo_Tarifa>)ListadoTipoTarifaCitas().Where(x => x.CodSede == sede).ToList();
                ViewBag.listadoTipoTarifa = new SelectList(tipT.ListadoTipoTarifa().Where(d => d.EstTipTar == true), "CodTipTar", "DescTipTar", tip.CodTipTar);
                E_Tipo_Tarifa reg = tipT.ListadoTipoTarifa().Find(x => x.CodTipTar.Equals(tip.CodTipTar));
                tip.DescTipTar = reg.DescTipTar;
                var evalua = (List<E_Tipo_Tarifa>)ListadoTipoTarifaCitas().Where(x => x.CodTipTar == tip.CodTipTar).ToList();

                if (evalua.Count == 0)
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        using (SqlCommand da = new SqlCommand("Usp_MtoTipoTarifa_Citas", con))
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
                                ViewBag.mensaje = "Error: Datos No Validos!!!";
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
            } catch (Exception ex)
            {
                ViewBag.mensaje = "Error: Dato Requerido";
                return View(tip);
            }
          
        }
        public ActionResult EliminarTipoTarifa(string id)
        {

            string sede = Session["codSede"].ToString();



            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_MtoTipoTarifa_Citas", con))
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





        public ActionResult CancelarCita(string id, string cadena)
        {
            ViewBag.cadena = cadena;
            string sede = Session["codSede"].ToString();
            var lista = (from x in buscaCitas(id) select x).FirstOrDefault();
            return View(lista);

        }

        [HttpPost]
        public ActionResult CancelarCita(E_Citas c)
        {
            string sede = Session["codSede"].ToString();
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();

            CuentasController cu = new CuentasController();

            E_Cuentas reg = cu.ListadoCuenta(sede).Find(x => x.CodCue == c.CodCue);

            decimal precio = 0, igv = 0, total = 0;

            precio = reg.STotCue - c.precio;
            igv = reg.IgvCue - c.igv;
            total = reg.TotCue - c.total;

            E_CuentaDetalle cue = cu.ListadoCuentaDetalle(c.CodCue).Find(x => x.CodProce == c.CodCita);

            string elimina = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                using (SqlCommand cmd = new SqlCommand("Usp_MtoCitas", con, tr))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.AddWithValue("@CodCita", c.CodCita);
                        cmd.Parameters.AddWithValue("@Historia", "");
                        cmd.Parameters.AddWithValue("@CodEspec", "");
                        cmd.Parameters.AddWithValue("@CodServ", "");
                        cmd.Parameters.AddWithValue("@CodMed", "");
                        cmd.Parameters.AddWithValue("@TipoPac", "");
                        if (c.Obser == null) { cmd.Parameters.AddWithValue("@Obser", " "); } else { cmd.Parameters.AddWithValue("@Obser", c.Obser); }
                        cmd.Parameters.AddWithValue("@CodCatPac", "");
                        cmd.Parameters.AddWithValue("@precio", 0);
                        cmd.Parameters.AddWithValue("@igv", 0);
                        cmd.Parameters.AddWithValue("@total", 0);
                        cmd.Parameters.AddWithValue("@CodTar", "");
                        cmd.Parameters.AddWithValue("@CodTipTar", "");
                        cmd.Parameters.AddWithValue("@HoraInicio", "");
                        cmd.Parameters.AddWithValue("@HoraFin", "");
                        cmd.Parameters.AddWithValue("@FechaRegistro", "");
                        cmd.Parameters.AddWithValue("@FechaCita", "");
                        cmd.Parameters.AddWithValue("@Turno", "");
                        cmd.Parameters.AddWithValue("@CodCue", "");
                        cmd.Parameters.AddWithValue("@Usuario", "");
                        cmd.Parameters.AddWithValue("@TipoRegistro", "");
                        cmd.Parameters.AddWithValue("@CodSede", "");
                        cmd.Parameters.AddWithValue("@TipoConsu", "");
                        cmd.Parameters.AddWithValue("@MedInter", "");
                        cmd.Parameters.AddWithValue("@Consultorio", "");
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", "");
                        cmd.Parameters.AddWithValue("@Elimina", elimina);
                        cmd.Parameters.AddWithValue("@Evento", "3");

                        cmd.ExecuteNonQuery();


                        using (SqlCommand dd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr))
                        {
                            dd.CommandType = CommandType.StoredProcedure;
                            dd.Parameters.AddWithValue("@Procedencia", "");
                            dd.Parameters.AddWithValue("@CodCue", c.CodCue);
                            dd.Parameters.AddWithValue("@Item", cue.Item);
                            dd.Parameters.AddWithValue("@Tarifa", "");
                            dd.Parameters.AddWithValue("@CodProce", c.CodCita);
                            dd.Parameters.AddWithValue("@CodDetalleP", "");
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
                            dd.Parameters.AddWithValue("@MedicoEnvia", 0);
                            dd.Parameters.AddWithValue("@Crea", "");
                            dd.Parameters.AddWithValue("@Modifica", "");
                            dd.Parameters.AddWithValue("@Elimina", elimina);
                            dd.Parameters.AddWithValue("@Evento", "2");

                            dd.ExecuteNonQuery();

                            using (SqlCommand da = new SqlCommand("Usp_MtoCuentas", con, tr))
                            {
                                da.CommandType = CommandType.StoredProcedure;
                                da.Parameters.AddWithValue("@CodCue", c.CodCue);
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
                                da.Parameters.AddWithValue("@Modifica", elimina);
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", "2");
                                da.ExecuteNonQuery();

                                tr.Commit();
                                ViewBag.mensaje = "Pedido registrado";
                            }
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
            return RedirectPermanent("~/Citas/ConsultaCita?id=" + c.Historia);
            //return RedirectPermanent("../ConsultaCita/?id=" + c.Historia + "&cadena=" + c.cadena);
        }

        public ActionResult ConsultaCita(string CodEspec = null, string CodServ = null, string fechaCita = null, string CodMed = null, string Turno = null, int? id = null, string cadena = null, string CodCue = null, string cuenta = null, int? dimension = null)
        {
            int x1 = 0;
            ViewBag.id = id;
            if (dimension != null)
            {
                x1 = (int)dimension;
            }

            string sede = Session["codSede"].ToString();
            if (CodCue != null)
            {
                ViewBag.cuenta = CodCue;
            }
            else
            {
                ViewBag.cuenta = "";
            }


            MedicosController h = new MedicosController();
            ConsultorioController co = new ConsultorioController();

            try
            {

                if (CodEspec != null && CodServ != null && fechaCita != null && CodMed != null && Turno != null && id != null && CodCue != null)
                {
                    ViewBag.especialidad = CodEspec;
                    ViewBag.servicio = CodServ;
                    ViewBag.idCliente = id;
                    ViewBag.turno = Turno;
                    ViewBag.fechaSeleccionada = fechaCita;
                    ViewBag.medico = CodMed;
                    ViewBag.dimension = x1;
                    DateTime fechaCambia = DateTime.Parse(fechaCita);
                    CultureInfo culture = new CultureInfo("es-PE");
                    string demo, demo1, demo2;
                    demo2 = fechaCambia.ToString("yyyy-MM-dd", culture);
                    DateTime nuevo = DateTime.Parse(demo2);
                    E_HorarioMedico reg = h.ListadoHorarioMedico().Find(x => x.Turno == Turno && x.CodMed == CodMed && x.dia == nuevo);
                    E_Consultorio reg1 = co.ListadoConsultorio().Find(x => x.IdConsul.Equals(reg.Consultorio));

                    ViewBag.consultorio = reg1.DescConsul;

                    demo = fechaCambia.ToString("dddd", culture);
                    demo1 = fechaCambia.ToString("dd/MM/yyyy", culture);
                    demo = demo.TrimEnd('.');



                    ViewBag.listaConsulta = (List<E_Citas>)ConsultaCitas(CodEspec, CodServ, CodMed, fechaCita, Turno);
                    ViewBag.listaCitasHoy = (List<E_Citas>)ConsultaCitasEspecial(CodMed, Turno, demo1);

                }
                else
                {
                    ViewBag.especialidad = "";
                    ViewBag.servicio = "";
                    ViewBag.idCliente = "";
                    ViewBag.turno = "";
                    ViewBag.fechaSeleccionada = "";
                    ViewBag.medico = "";
                    ViewBag.dimension = x1;
                }



                ViewBag.listadoEspecialidad = new SelectList(es.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true).ToList(), "CodEspec", "NomEspec");


            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Ingrese datos correctos";

            }


            return View();

        }

        public ActionResult ConsultaCitaGenerada(int id, string cadena = null, string cuenta = null)
        {
            string sede = Session["codSede"].ToString();

            if (cuenta != null)
            {
                ViewBag.cuenta = cuenta;
            }
            else
            {
                ViewBag.cuenta = "";
            }
            

            string especialidad = "";
            string servicio = "";
            ViewBag.cadena = cadena;
            ViewBag.dimension = "";
            string turno = "";
            string medico = "";
            string fecha = "";
            string dimension = "";


            string[] fija = cadena.Split(',');
            int i = 1;
            foreach (string item in fija)
            {
                if (i == 1)
                {
                    especialidad = item;
                }
                else if (i == 2)
                {
                    servicio = item;
                }
                else if (i == 3)
                {
                    fecha = item;
                }
                else if (i == 4)
                {
                    medico = item;
                }
                else if (i == 5)
                {
                    turno = item;
                }
                else if (i == 6)
                {
                    dimension = item;
                }

                i++;

            }

            MedicosController h = new MedicosController();
            ConsultorioController co = new ConsultorioController();

            try
            {

                ViewBag.especialidad = especialidad;
                ViewBag.servicio = servicio;
                ViewBag.idCliente = id;
                ViewBag.turno = turno;
                ViewBag.fechaSeleccionada = fecha;
                ViewBag.medico = medico;
                ViewBag.dimension = dimension;
                DateTime fechaCambia = DateTime.Parse(fecha);
                CultureInfo culture = new CultureInfo("es-PE");
                string demo, demo1, demo2;
                demo2 = fechaCambia.ToString("yyyy-MM-dd", culture);
                DateTime nuevo = DateTime.Parse(demo2);
                E_HorarioMedico reg = h.ListadoHorarioMedico().Find(x => x.Turno == turno && x.CodMed == medico && x.dia == nuevo);
                E_Consultorio reg1 = co.ListadoConsultorio().Find(x => x.IdConsul.Equals(reg.Consultorio));

                ViewBag.consultorio = reg1.DescConsul;

                demo = fechaCambia.ToString("dddd", culture);
                demo1 = fechaCambia.ToString("dd/MM/yyyy", culture);
                demo = demo.TrimEnd('.');

                ViewBag.listadoEspecialidad = new SelectList(es.ListadoEspecialidades().Where(x => x.CodSed == sede).ToList(), "CodEspec", "NomEspec", especialidad);
                ViewBag.listaConsulta = (List<E_Citas>)ConsultaCitas(especialidad, servicio, medico, fecha, turno);
                ViewBag.listaCitasHoy = (List<E_Citas>)ConsultaCitasEspecial(medico, turno, demo1);

            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Ingrese datos correctos";

            }
            return View();


        }


        public ActionResult ObtenerTiempo(int tiempo, string horaActual, string CodMed, string turno, string FechaCita)
        {
            DateTime t = DateTime.Parse(horaActual);
            t = t.AddMinutes(tiempo);
            string hvIFF = t.ToString("hh:mm tt");
            string hvIFFF = t.ToString("hh:mm tt");
            hvIFF = hvIFF.Replace(". ", "");
            hvIFF = hvIFF.Replace(".", "");
            horaActual = horaActual.Replace(". ", "");
            horaActual = horaActual.Replace(".", "");
            var eva = ConsultaCitasEspecialConflictos(CodMed, turno, FechaCita, horaActual, hvIFF);
            E_Citas c = new E_Citas();
            if (eva.Count() >= 1)
            {
                c.HoraFin = "-1";                
            }
            else {
                c.HoraFin = hvIFFF;
            }
            
            List<E_Citas> lista = new List<E_Citas>();
            lista.Add(c);
          
            if (lista.Count() != 0)
            {
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            return null;
        }


        public ActionResult ObtenerEspecialidad()
        {
            string sede = Session["codSede"].ToString();
            var evalua = (List<E_Especialidades>)es.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true).ToList();
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
            var evaluaEspe = (List<E_Servicios>)ser.ListadoServicios().Where(x => x.CodEspec == CodEspec && x.EstServ == true).ToList();

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

        public ActionResult ObtenerServiciosGeneral(string CodEspec)
        {

            ServiciosController ser = new ServiciosController();

            var evaluaEspe = (List<E_Servicios>)ser.ListadoServicios().Where(x => x.CodEspec == CodEspec && x.EstServ == true).ToList();

            if (evaluaEspe.Count() != 0)
            {
                return Json(evaluaEspe, JsonRequestBehavior.AllowGet);
            }

            return null;

        }


        public ActionResult ObtenerTarifa(string CodEspec)
        {
            string sede = Session["codSede"].ToString();

            var evalua = (List<E_Tarifario>)ListadoTarifaAtencion().Where(x => x.CodEspec == CodEspec && x.CodSede == sede && x.EstTar == true).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult ObtenerMedico(string CodServ, string Fecha)
        {
            try
            {

                var evalua = (List<E_Citas>)BuscaMedico(CodServ, Fecha);
                if (evalua.Count() != 0)
                {
                    return Json(evalua, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public ActionResult ObtenerTurno(string CodMed, string Fecha)
        {

            var evalua = (List<E_Citas>)BuscaTurno(CodMed, Fecha);
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult ObtenerDetalleTarifa(string CodTar, string CodCat)
        {
            var evalua = tar.ListadoCategoriaPacienteTarifa(CodTar).Find(x => x.CodCatPac==CodCat);
            var evalua1 = tar.ListadoTarifa().Find(x => x.CodTar == CodTar);

            E_Tarifario tari = new E_Tarifario();
            if (tari != null)
            {
                if (evalua1.AfecIgcv == true)
                {
                    DatosGeneralesController da = new DatosGeneralesController();
                    E_Datos_Generales dat = da.listadatogenerales().FirstOrDefault();
                    tari.Precio = decimal.Round(evalua.Precio / (dat.igv + 1), 2);
                    var result = ((evalua.Precio / decimal.Parse("1.18")) * dat.igv);
                    tari.igv =  decimal.Round(result,2);
                    tari.total = decimal.Round(tari.Precio + tari.igv,2);

                }
                else {
                    tari.Precio = evalua.Precio;
                    tari.igv = 0;
                    tari.total = evalua.Precio;
                }                
            }
            else {
                tari.Precio = 0;
                tari.igv = 0;
                tari.total = 0;
            }

            tari.CodTipTar = evalua1.CodTipTar;
            tari.TiempoApox = evalua1.TiempoApox;

            List<E_Tarifario> lista = new List<E_Tarifario>();
            lista.Add(tari);
            
            if (lista.Count() != 0)
            {
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public int Usp_Validar_Cita(string codmed,string horainicio,string horafin , string fechacita )
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Validar_Cita", con))
                {
                    string replace = horainicio.Substring(6);
                    string replace1 = horafin.Substring(6);
                    TimeSpan time1 = TimeSpan.Parse(horainicio.Replace(replace, ""));
                    TimeSpan time2 = TimeSpan.Parse(horafin.Replace(replace1, ""));
                    DateTime fecha = DateTime.Parse(fechacita);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codmed",codmed);
                    cmd.Parameters.AddWithValue("@HoraInicio", time1);
                    cmd.Parameters.AddWithValue("@HoraFin", time2);
                    cmd.Parameters.AddWithValue("@fechaCita", fecha);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {

                        while (dr.Read())
                        {
                            result = dr.GetInt32(0);
                        }
                        con.Close();
                    }

                }
                return result;
            }
        }

        public ActionResult RegistraCita(E_Citas c,string condicion=null)
        {

             string sede = Session["codSede"].ToString();
            ViewBag.condicion = condicion;
            ViewBag.listadoTari = new SelectList(ListadoTarifaAtencion().Where(x => x.CodEspec == c.CodEspec && x.CodSede == sede), "CodTar", "DescTar");
            var util = new DatosGeneralesController().listadatogenerales().FirstOrDefault();
            if (c.evento == 1) {

                if (Usp_Validar_Cita(c.CodMed, c.HoraInicio, c.HoraFin, c.fechaCita) == 0)
                {
                    int codigoCuenta = 0;

                    CuentasController cu = new CuentasController();
                    E_Cuentas cue = cu.ListadoCuenta(sede).Find(x => x.CodCue == c.CodCue);
                    E_CuentaDetalle cueD = cu.ListadoCuentaDetalle(c.CodCue).LastOrDefault();
                    ViewBag.listadoTari = new SelectList(ListadoTarifaAtencion().Where(x => x.CodEspec == c.CodEspec && x.CodSede == sede), "CodTar", "DescTar", c.CodTar);
                    decimal precio = 0, igv = 0, total = 0;


                    UtilitarioController ut = new UtilitarioController();
                    var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();

                    c.HoraInicio = c.HoraInicio.Replace(".", "");
                    c.HoraInicio = c.HoraInicio.Replace("p m", "pm");
                    c.HoraInicio = c.HoraInicio.Replace("a m", "am");
                    c.HoraFin = c.HoraFin.Replace(".", "");
                    c.HoraFin = c.HoraFin.Replace("p m", "pm");
                    c.HoraFin = c.HoraFin.Replace("a m", "am");


                    E_Servicios reg2 = ser.ListadoServicios().Find(x => x.CodServ == c.CodServ);

                    string Crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
                    string usuario = Session["UserID"].ToString();

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                        using (SqlCommand da = new SqlCommand("Usp_MtoCuentas", con, tr))
                        {
                            try
                            {
                                da.CommandType = CommandType.StoredProcedure;

                                if (c.CodCue == 0)
                                {

                                    da.Parameters.AddWithValue("@CodCue", "");
                                    da.Parameters.AddWithValue("@CodSede", reg2.CodSede);
                                    da.Parameters.AddWithValue("@Historia", c.Historia);
                                    da.Parameters.AddWithValue("@CodcatPac", c.CodCatPac);
                                    da.Parameters.AddWithValue("@STotCue", c.precio);
                                    da.Parameters.AddWithValue("@IgvCue", c.igv);
                                    da.Parameters.AddWithValue("@TotCue", c.total);
                                    da.Parameters.AddWithValue("@FecCrea", "");
                                    da.Parameters.AddWithValue("@FecAnul", "");
                                    da.Parameters.AddWithValue("@EstCue", "1");
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


                                    precio = cue.STotCue + c.precio;
                                    igv = cue.IgvCue + c.igv;
                                    total = cue.TotCue + c.total;

                                    da.Parameters.AddWithValue("@CodCue", c.CodCue);
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

                                int Resu = (int)da.ExecuteScalar();


                                codigoCuenta = Resu;

                                using (SqlCommand cmd = new SqlCommand("Usp_MtoCitas", con, tr))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.AddWithValue("@CodCita", "");
                                    cmd.Parameters.AddWithValue("@Historia", c.Historia);
                                    cmd.Parameters.AddWithValue("@CodEspec", c.CodEspec);
                                    cmd.Parameters.AddWithValue("@CodServ", c.CodServ);
                                    cmd.Parameters.AddWithValue("@CodMed", c.CodMed);
                                    cmd.Parameters.AddWithValue("@TipoPac", c.TipoPac);
                                    if (c.Obser == null) { cmd.Parameters.AddWithValue("@Obser", " "); } else { cmd.Parameters.AddWithValue("@Obser", c.Obser); }
                                    cmd.Parameters.AddWithValue("@CodCatPac", c.CodCatPac);
                                    cmd.Parameters.AddWithValue("@precio", c.precio);
                                    cmd.Parameters.AddWithValue("@igv", c.igv);
                                    cmd.Parameters.AddWithValue("@total", c.total);
                                    cmd.Parameters.AddWithValue("@CodTar", c.CodTar);
                                    cmd.Parameters.AddWithValue("@CodTipTar", c.CodTipTar);
                                    cmd.Parameters.AddWithValue("@HoraInicio", c.HoraInicio);
                                    cmd.Parameters.AddWithValue("@HoraFin", c.HoraFin);
                                    cmd.Parameters.AddWithValue("@FechaRegistro", "");
                                    cmd.Parameters.AddWithValue("@FechaCita", c.fechaCita);
                                    cmd.Parameters.AddWithValue("@Turno", c.Turno);
                                    cmd.Parameters.AddWithValue("@CodCue", Resu);
                                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                                    cmd.Parameters.AddWithValue("@TipoRegistro", "Local");
                                    cmd.Parameters.AddWithValue("@CodSede", reg2.CodSede);
                                    cmd.Parameters.AddWithValue("@TipoConsu", c.TipoConsu);
                                    cmd.Parameters.AddWithValue("@MedInter", c.MedInter);
                                    cmd.Parameters.AddWithValue("@Consultorio", c.Consultorio);
                                    cmd.Parameters.AddWithValue("@Crea", Crea);
                                    cmd.Parameters.AddWithValue("@Modifica", "");
                                    cmd.Parameters.AddWithValue("@Elimina", "");
                                    cmd.Parameters.AddWithValue("@Evento", "1");


                                    int codCita = (int)cmd.ExecuteScalar();


                                    using (SqlCommand dd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr))
                                    {
                                        dd.CommandType = CommandType.StoredProcedure;
                                        dd.Parameters.AddWithValue("@Procedencia", 1);
                                        dd.Parameters.AddWithValue("@CodCue", Resu);
                                        if (c.CodCue == 0)
                                        {
                                            dd.Parameters.AddWithValue("@Item", 1);
                                        }
                                        else
                                        {
                                            dd.Parameters.AddWithValue("@Item", cueD.Item + 1);
                                        }
                                        dd.Parameters.AddWithValue("@Tarifa", c.CodTar);
                                        dd.Parameters.AddWithValue("@CodProce", codCita);
                                        dd.Parameters.AddWithValue("@CodDetalleP", codCita);
                                        dd.Parameters.AddWithValue("@CodSede", reg2.CodSede);
                                        dd.Parameters.AddWithValue("@Cantidad", 1);
                                        dd.Parameters.AddWithValue("@precioUni", c.total);
                                        dd.Parameters.AddWithValue("@precio", c.precio);
                                        dd.Parameters.AddWithValue("@igv", c.igv);
                                        dd.Parameters.AddWithValue("@total", c.total);
                                        dd.Parameters.AddWithValue("@EstDet", "1");
                                        dd.Parameters.AddWithValue("@FechaAten", "");
                                        dd.Parameters.AddWithValue("@TurnoAten", "");
                                        dd.Parameters.AddWithValue("@RegMedico", c.CodMed);
                                        dd.Parameters.AddWithValue("@MedicoEnvia", c.MedInter);
                                        dd.Parameters.AddWithValue("@Crea", Crea);
                                        dd.Parameters.AddWithValue("@Modifica", "");
                                        dd.Parameters.AddWithValue("@Elimina", "");
                                        dd.Parameters.AddWithValue("@Evento", "1");

                                        dd.ExecuteNonQuery();

                                        tr.Commit();
                                        if (util.GENERARCUENTAAUTO)
                                        {
                                            return RedirectPermanent("~/Caja/RegistrarCaja?id=" + Resu);
                                        }
                                        else {
                                            if (util.PREGXATENCIONPROGRAMADAS)
                                            {
                                                ViewBag.activaAlerta = 1;
                                            }
                                            else
                                            {
                                                return RedirectPermanent("~/Cuentas/VerificaCuenta/" + Resu);
                                            }
                                        }

                                  
                                        ViewBag.historia = c.Historia;
                                        ViewBag.cuenta = Resu;
                                    }
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
                }
                else {
                    if (condicion.Equals("informe"))
                    {
                        ViewBag.condicion = "2/Pacientes/Informes";
                    }
                    else if (condicion.Equals("cita")) {
                
                        ViewBag.condicion = "1/Citas/ConsultaCita?id=" + c.Historia;
                    }
                    ViewBag.mensaje = "Error: la Cita ya fue Reservada";
                
                }
            }

            MedicosController me = new MedicosController();

            ViewBag.listaMedicos = (List<E_Medico>)me.ListadoMedico().ToList();
            ViewBag.CodCue = c.CodCue;
            E_Especialidades espec = es.ListadoEspecialidades().Find(x => x.CodEspec == c.CodEspec);
            ViewBag.TarifaDefault = espec.CodTar;
            try
            {
                E_Pacientes reg = pa.ListadoPacientes().Find(x => x.Historia == c.Historia);
                E_Especialidades reg1 = es.ListadoEspecialidades().Find(x => x.CodEspec == c.CodEspec);
                E_Servicios reg2 = ser.ListadoServicios().Find(x => x.CodServ == c.CodServ);
                E_Categoria_Paciente reg3 = caP.listadoCategoriaCliente().Find(x => x.CodCatPac == reg.CodCatPac);

                var evaluaTipo = (List<E_Citas>)ListadoCitas().Where(x => x.CodServ == c.CodServ && x.Historia == c.Historia).ToList();

                if (evaluaTipo.Count() != 0)
                {
                    ViewBag.TipoPaciente = "Continuo";
                }
                else
                {
                    ViewBag.TipoPaciente = "Nuevo";
                }

                ViewBag.CodCue = c.CodCue;
                ViewBag.Historia = c.Historia;
                ViewBag.CodCatPac = reg.CodCatPac;
                ViewBag.CodEspec = c.CodEspec;
                ViewBag.CodServ = c.CodServ;
                ViewBag.CodTar = c.CodTar;
                ViewBag.CodMed = c.CodMed;
                ViewBag.nombre = reg.NomPac + " " + reg.ApePat + " " + reg.ApeMat;
                ViewBag.nombreEspecialidad = reg1.NomEspec;
                ViewBag.nombreServicio = reg2.NomServ;
                ViewBag.nombreCategoria = reg3.DescCatPac;

            }
            catch (Exception e)
            {
                ViewBag.mensaje = "Error Datos no Validos!!!";
            }
            
            return View(c);           

        }


        public ActionResult RegistraCitaCuenta(E_Citas c)
        {
            int codigoCuenta = 0;
            string sede = Session["codSede"].ToString();

            var util = new DatosGeneralesController().listadatogenerales().FirstOrDefault();
            CuentasController cu = new CuentasController();
            E_Cuentas cue = cu.ListadoCuenta(sede).Find(x => x.CodCue == c.CodCue);
            E_CuentaDetalle cueD = cu.ListadoCuentaDetalle(c.CodCue).LastOrDefault();
            decimal precio = 0, igv = 0, total = 0;


            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();

            c.HoraInicio = c.HoraInicio.Replace(".", "");
            c.HoraInicio = c.HoraInicio.Replace("p m", "pm");
            c.HoraInicio = c.HoraInicio.Replace("a m", "am");
            c.HoraFin = c.HoraFin.Replace(".", "");
            c.HoraFin = c.HoraFin.Replace("p m", "pm");
            c.HoraFin = c.HoraFin.Replace("a m", "am");


            E_Servicios reg2 = ser.ListadoServicios().Find(x => x.CodServ == c.CodServ);

            string Crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
            string usuario = Session["UserID"].ToString();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                using (SqlCommand da = new SqlCommand("Usp_MtoCuentas", con, tr))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;

                        if (c.CodCue == 0)
                        {
                            da.Parameters.AddWithValue("@CodCue", "");
                            da.Parameters.AddWithValue("@CodSede", reg2.CodSede);
                            da.Parameters.AddWithValue("@Historia", c.Historia);
                            da.Parameters.AddWithValue("@CodcatPac", c.CodCatPac);
                            da.Parameters.AddWithValue("@STotCue", c.precio);
                            da.Parameters.AddWithValue("@IgvCue", c.igv);
                            da.Parameters.AddWithValue("@TotCue", c.total);
                            da.Parameters.AddWithValue("@FecCrea", "");
                            da.Parameters.AddWithValue("@FecAnul", "");
                            da.Parameters.AddWithValue("@EstCue", "1");
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


                            precio = cue.STotCue + c.precio;
                            igv = cue.IgvCue + c.igv;
                            total = cue.TotCue + c.total;

                            da.Parameters.AddWithValue("@CodCue", c.CodCue);
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

                        int Resu = (int)da.ExecuteScalar();


                        codigoCuenta = Resu;

                        using (SqlCommand cmd = new SqlCommand("Usp_MtoCitas", con, tr))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@CodCita", "");
                            cmd.Parameters.AddWithValue("@Historia", c.Historia);
                            cmd.Parameters.AddWithValue("@CodEspec", c.CodEspec);
                            cmd.Parameters.AddWithValue("@CodServ", c.CodServ);
                            cmd.Parameters.AddWithValue("@CodMed", c.CodMed);
                            cmd.Parameters.AddWithValue("@TipoPac", c.TipoPac);
                            if (c.Obser == null) { cmd.Parameters.AddWithValue("@Obser", " "); } else { cmd.Parameters.AddWithValue("@Obser", c.Obser); }
                            cmd.Parameters.AddWithValue("@CodCatPac", c.CodCatPac);
                            cmd.Parameters.AddWithValue("@precio", c.precio);
                            cmd.Parameters.AddWithValue("@igv", c.igv);
                            cmd.Parameters.AddWithValue("@total", c.total);
                            cmd.Parameters.AddWithValue("@CodTar", c.CodTar);
                            cmd.Parameters.AddWithValue("@CodTipTar", c.CodTipTar);
                            cmd.Parameters.AddWithValue("@HoraInicio", c.HoraInicio);
                            cmd.Parameters.AddWithValue("@HoraFin", c.HoraFin);
                            cmd.Parameters.AddWithValue("@FechaRegistro", "");
                            cmd.Parameters.AddWithValue("@FechaCita", c.fechaCita);
                            cmd.Parameters.AddWithValue("@Turno", c.Turno);
                            cmd.Parameters.AddWithValue("@CodCue", Resu);
                            cmd.Parameters.AddWithValue("@Usuario", usuario);
                            cmd.Parameters.AddWithValue("@TipoRegistro", "Local");
                            cmd.Parameters.AddWithValue("@CodSede", reg2.CodSede);
                            cmd.Parameters.AddWithValue("@TipoConsu", c.TipoConsu);
                            cmd.Parameters.AddWithValue("@MedInter", c.MedInter);
                            cmd.Parameters.AddWithValue("@Consultorio", c.Consultorio);
                            cmd.Parameters.AddWithValue("@Crea", Crea);
                            cmd.Parameters.AddWithValue("@Modifica", "");
                            cmd.Parameters.AddWithValue("@Elimina", "");
                            cmd.Parameters.AddWithValue("@Evento", "1");


                            int codCita = (int)cmd.ExecuteScalar();


                            using (SqlCommand dd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr))
                            {
                                dd.CommandType = CommandType.StoredProcedure;
                                dd.Parameters.AddWithValue("@Procedencia", 1);
                                dd.Parameters.AddWithValue("@CodCue", Resu);
                                if (c.CodCue == 0)
                                {
                                    dd.Parameters.AddWithValue("@Item", 1);
                                }
                                else
                                {
                                    dd.Parameters.AddWithValue("@Item", cueD.Item + 1);
                                }
                                dd.Parameters.AddWithValue("@Tarifa", c.CodTar);
                                dd.Parameters.AddWithValue("@CodProce", codCita);
                                dd.Parameters.AddWithValue("@CodDetalleP", codCita);
                                dd.Parameters.AddWithValue("@CodSede", reg2.CodSede);
                                dd.Parameters.AddWithValue("@Cantidad", 1);
                                dd.Parameters.AddWithValue("@precioUni", c.total);
                                dd.Parameters.AddWithValue("@precio", c.precio);
                                dd.Parameters.AddWithValue("@igv", c.igv);
                                dd.Parameters.AddWithValue("@total", c.total);
                                dd.Parameters.AddWithValue("@EstDet", "1");
                                dd.Parameters.AddWithValue("@FechaAten", "");
                                dd.Parameters.AddWithValue("@TurnoAten", "");
                                dd.Parameters.AddWithValue("@RegMedico", c.CodMed);
                                dd.Parameters.AddWithValue("@MedicoEnvia", c.MedInter);
                                dd.Parameters.AddWithValue("@Crea", Crea);
                                dd.Parameters.AddWithValue("@Modifica", "");
                                dd.Parameters.AddWithValue("@Elimina", "");
                                dd.Parameters.AddWithValue("@Evento", "1");

                                dd.ExecuteNonQuery();

                                tr.Commit();
                                if (util.PREGXATENCIONPROGRAMADAS)
                                {
                                    ViewBag.activaAlerta = 1;
                                }
                                else {
                                    ViewBag.activaAlerta = 0;
                                }
                             
                                ViewBag.historia = c.Historia;
                                ViewBag.cuenta = Resu;
                                ViewBag.mensaje = "Pedido registrado";
                            }
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
                        
            return View(c);

        }

        public ActionResult ListadoDeCitas(string servicio = null, string especialidad = null, string fecha = null, string turno = null)
        {
            string sede = Session["codSede"].ToString();

            if (servicio != null && especialidad != null && fecha != null)
            {
                try
                {
                    DateTime fechaF = DateTime.Parse(fecha);
                    ViewBag.evaluaTipo = (List<E_Citas>)ListaGeneralCitas(especialidad, servicio, fechaF, turno).ToList();
                    ViewBag.servicio = servicio;
                    ViewBag.especialidad = especialidad;
                    ViewBag.fecha = fecha;
                    ViewBag.turno = turno;
                }
                catch (Exception e)
                {
                    ViewBag.mensaje = "Ingrese todos los campos para realizar la busqueda";
                }
            }
            else
            {
                ViewBag.evaluaTipo = null;
                ViewBag.servicio = "";
                ViewBag.especialidad = "";
                ViewBag.fecha = "";
                ViewBag.turno = "";
            }

            return View();
        }

        public ActionResult CancelarUnaCita(E_Citas c)
        {
            string sede = Session["codSede"].ToString();
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();

            CuentasController cu = new CuentasController();

            E_Cuentas reg = cu.ListadoCuenta(sede).Find(x => x.CodCue == c.CodCue);

            decimal precio = 0, igv = 0, total = 0;

            precio = reg.STotCue - c.precio;
            igv = reg.IgvCue - c.igv;
            total = reg.TotCue - c.total;

            E_CuentaDetalle cue = cu.ListadoCuentaDetalle(c.CodCue).Find(x => x.CodProce == c.CodCita);

            string elimina = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                using (SqlCommand cmd = new SqlCommand("Usp_MtoCitas", con, tr))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.AddWithValue("@CodCita", c.CodCita);
                        cmd.Parameters.AddWithValue("@Historia", "");
                        cmd.Parameters.AddWithValue("@CodEspec", "");
                        cmd.Parameters.AddWithValue("@CodServ", "");
                        cmd.Parameters.AddWithValue("@CodMed", "");
                        cmd.Parameters.AddWithValue("@TipoPac", "");
                        if (c.Obser == null) { cmd.Parameters.AddWithValue("@Obser", " "); } else { cmd.Parameters.AddWithValue("@Obser", c.Obser.ToUpper()); }
                        cmd.Parameters.AddWithValue("@CodCatPac", "");
                        cmd.Parameters.AddWithValue("@precio", 0);
                        cmd.Parameters.AddWithValue("@igv", 0);
                        cmd.Parameters.AddWithValue("@total", 0);
                        cmd.Parameters.AddWithValue("@CodTar", "");
                        cmd.Parameters.AddWithValue("@CodTipTar", "");
                        cmd.Parameters.AddWithValue("@HoraInicio", "");
                        cmd.Parameters.AddWithValue("@HoraFin", "");
                        cmd.Parameters.AddWithValue("@FechaRegistro", "");
                        cmd.Parameters.AddWithValue("@FechaCita", "");
                        cmd.Parameters.AddWithValue("@Turno", "");
                        cmd.Parameters.AddWithValue("@CodCue", "");
                        cmd.Parameters.AddWithValue("@Usuario", "");
                        cmd.Parameters.AddWithValue("@TipoRegistro", "");
                        cmd.Parameters.AddWithValue("@CodSede", "");
                        cmd.Parameters.AddWithValue("@TipoConsu", "");
                        cmd.Parameters.AddWithValue("@MedInter", "");
                        cmd.Parameters.AddWithValue("@Consultorio", "");
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", "");
                        cmd.Parameters.AddWithValue("@Elimina", elimina);
                        cmd.Parameters.AddWithValue("@Evento", "3");

                        cmd.ExecuteNonQuery();


                        using (SqlCommand dd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr))
                        {
                            dd.CommandType = CommandType.StoredProcedure;
                            dd.Parameters.AddWithValue("@Procedencia", "");
                            dd.Parameters.AddWithValue("@CodCue", c.CodCue);
                            dd.Parameters.AddWithValue("@Item", cue.Item);
                            dd.Parameters.AddWithValue("@Tarifa", "");
                            dd.Parameters.AddWithValue("@CodProce", c.CodCita);
                            dd.Parameters.AddWithValue("@CodDetalleP", "");
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
                            dd.Parameters.AddWithValue("@MedicoEnvia", 0);
                            dd.Parameters.AddWithValue("@Crea", "");
                            dd.Parameters.AddWithValue("@Modifica", "");
                            dd.Parameters.AddWithValue("@Elimina", elimina);
                            dd.Parameters.AddWithValue("@Evento", "2");

                            dd.ExecuteNonQuery();

                            using (SqlCommand da = new SqlCommand("Usp_MtoCuentas", con, tr))
                            {
                                da.CommandType = CommandType.StoredProcedure;
                                da.Parameters.AddWithValue("@CodCue", c.CodCue);
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
                                da.Parameters.AddWithValue("@Modifica", elimina);
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", "2");
                                da.ExecuteNonQuery();

                                tr.Commit();
                                ViewBag.mensaje = "Pedido registrado";
                            }
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
            return RedirectPermanent("ListadoDeCitas/?servicio=" + c.CodServ + "&especialidad=" + c.CodEspec + "&fecha=" + c.fechaCita + "&turno=" + c.Turno);
        }




    }
}