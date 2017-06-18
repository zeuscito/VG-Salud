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
    public class HonorariosCabeceraDetalleController : Controller
    {


        public ActionResult RegistrarEnBD(E_Honorario_Cabecera_Detalle CD)
        {
            string[] array1;
            int nroDoc = 0;
            int nroAte = 0;
            string CodMed = "";
            string CodServ = "";
            decimal totalFact = 0;
            foreach (var item in CD.arreglo)
            {
                array1 = item.Split('|');
                nroDoc = Convert.ToInt32(array1[2].Count());
                nroAte += Convert.ToInt32(array1[12]);
                CodMed = array1[15];
                CodServ = array1[16];
                totalFact += Decimal.Parse(array1[7]);
            }

            String Tip = ""; string TipC = ""; string tipL = ""; string tipR = "";
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
            ServiciosController ser = new ServiciosController();
            MedicosController me = new MedicosController();
            var medico = (E_Medico)me.ListadoMedico().Where(x => x.CodMed == CodMed).FirstOrDefault();
            var codesp = (E_Servicios)ser.ListadoServicios().Where(x => x.CodServ == CodServ).FirstOrDefault();
            string sede = Session["codSede"].ToString();
            string IDC = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Honorario_Cabecera", con, tr))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;

                        da.Parameters.AddWithValue("@IdC", "");
                        da.Parameters.AddWithValue("@CodServ", CodServ);
                        da.Parameters.AddWithValue("@CodEspec", codesp.CodEspec);
                        if (CD.TipoLiquidacion == 1) da.Parameters.AddWithValue("@CodMed", "--"); else if (CD.TipoLiquidacion == 2) da.Parameters.AddWithValue("@CodMed", CodMed);
                        da.Parameters.AddWithValue("@FechaLiquidacion", CD.FechaLiquidacion);
                        da.Parameters.AddWithValue("@Fechainicio", CD.FechaInicio);
                        da.Parameters.AddWithValue("@Fechafinal", CD.FechaFinal);
                        da.Parameters.AddWithValue("@Estado", "1");
                        if (CD.TipoDoc == "T") Tip = "CUENTA/FACTURAS";
                        else if (CD.TipoDoc == "G") Tip = "CUENTAS";
                        else if (CD.TipoDoc == "F") Tip = "FACTURADAS";
                        da.Parameters.AddWithValue("@TipoDoc", Tip);
                        if (CD.TipoPago == "T") TipC = "CONTADO/CREDITO";
                        else if (CD.TipoPago == "CONTADO") TipC = "CONTADO";
                        else if (CD.TipoPago == "CREDITO") TipC = "CREDITO";
                        da.Parameters.AddWithValue("@TipoPago", TipC);

                        da.Parameters.AddWithValue("@NroDocumentos", nroDoc);
                        da.Parameters.AddWithValue("@NroAtenciones", nroAte);
                        da.Parameters.AddWithValue("@TotalFac", totalFact);
                        da.Parameters.AddWithValue("@Total", CD.Total);
                        da.Parameters.AddWithValue("@FechaAnulado", "");
                        da.Parameters.AddWithValue("@UsuarioAnulado", "");
                        da.Parameters.AddWithValue("@CodSede", sede);
                        if (CD.TipoLiquidacion == 1) tipL = "SERVICIO"; else if (CD.TipoLiquidacion == 2) tipL = "MEDICOS";
                        da.Parameters.AddWithValue("@TipoLiquidacion", tipL);
                        if (CD.TipoRango == "1") tipR = "FECHA ATENCION"; else tipR = "FECHA PAGO";
                        da.Parameters.AddWithValue("@TipoRango", tipR);
                        da.Parameters.AddWithValue("@PagoTurno", CD.PagaTurno);
                        da.Parameters.AddWithValue("@CantTurno", CD.Cantidad);
                        da.Parameters.AddWithValue("@PagoTotalTurno", CD.PagoTotalTurno);
                        da.Parameters.AddWithValue("@FormaLiq", CD.comoPagar);
                        da.Parameters.AddWithValue("@Crea", crea);
                        da.Parameters.AddWithValue("@Modifica", "");
                        da.Parameters.AddWithValue("@Eliminar", "");
                        da.Parameters.AddWithValue("@Evento", "1");

                        IDC = da.ExecuteScalar().ToString();
                        if (!IDC.Equals(null))
                        {
                            int item = 1;
                            List<E_Honorario_Cabecera_Detalle> listaEmision = new List<E_Honorario_Cabecera_Detalle>();


                            foreach (var array in CD.arreglo)
                            {

                                string[] splite = array.Split('|');

                                using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_Honorario_Detalle", con, tr))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@Item", item);
                                    cmd.Parameters.AddWithValue("@IdC", IDC);
                                    cmd.Parameters.AddWithValue("@CodSede", splite[0]);
                                    cmd.Parameters.AddWithValue("@Historia", splite[1]);
                                    cmd.Parameters.AddWithValue("@CodCue", int.Parse(splite[2]));
                                    cmd.Parameters.AddWithValue("@ItemCta", splite[3]);
                                    cmd.Parameters.AddWithValue("@TipoDoc", splite[4]);
                                    cmd.Parameters.AddWithValue("@SerieDocumento", splite[5]);
                                    cmd.Parameters.AddWithValue("@NroDocumento", splite[6]);
                                    cmd.Parameters.AddWithValue("@Tot_Tarifa", decimal.Parse(splite[7]));
                                    cmd.Parameters.AddWithValue("@PorcentajeTarifa", decimal.Parse(splite[8]));
                                    cmd.Parameters.AddWithValue("@APagar", decimal.Parse(splite[9]));
                                    cmd.Parameters.AddWithValue("@SecFactCaja", int.Parse(splite[3]));
                                    cmd.Parameters.AddWithValue("@Estado", "L");
                                    if (splite[18] == "")
                                    {
                                        cmd.Parameters.AddWithValue("@FecEmision", "");
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@FecEmision", DateTime.Parse(splite[18]));
                                    }
                                    cmd.Parameters.AddWithValue("@FecAtencion", splite[10]);
                                    cmd.Parameters.AddWithValue("@Tarifa", splite[11]);
                                    cmd.Parameters.AddWithValue("@Cant", int.Parse(splite[12]));
                                    cmd.Parameters.AddWithValue("@Turno", splite[13]);
                                    cmd.Parameters.AddWithValue("@Crea", crea);
                                    cmd.Parameters.AddWithValue("@Modifica", "");
                                    cmd.Parameters.AddWithValue("@Eliminar", "");
                                    cmd.Parameters.AddWithValue("@Evento", "1");
                                    item++;

                                    cmd.ExecuteNonQuery();

                                }

                            }
                        }
                        tr.Commit();
                    }
                    catch (Exception e)
                    {
                        tr.Rollback();
                        ViewBag.mensaje = "Error: Datos No Validos ";

                        return View(da);
                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("RegistrarHonorarios");
        }
        public ActionResult RegistrarHonorarios(string servicio = null, string fechaI = null, string fechaf = null, string TipoPago = null, string TipoCuenta = null, int? TipoRango = null, string fechaLiqui = null, int? TipoLiqui = null)
        {

            UtilitarioController u = new UtilitarioController();
            TarifarioController ta = new TarifarioController();
            MedicosController medi = new MedicosController();
            ViewBag.tarifario = (List<E_Tarifario>)ta.ListadoTarifa().ToList();

            E_Master reg = u.ListadoHoraServidor().FirstOrDefault(); ViewBag.tipoRango = TipoRango;
            ViewBag.tipoRango = TipoRango;
            ViewBag.servicio = servicio;
            ViewBag.fechaI = fechaI;
            ViewBag.fechaF = fechaf;
            ViewBag.tipoPago = TipoPago;
            ViewBag.tipoCuenta = TipoCuenta;
            ViewBag.TipoLiqui = TipoLiqui;
            if (fechaLiqui == null)
            {
                ViewBag.fechaLiqui = reg.HoraServidor.ToShortDateString();
            }
            else
            {
                ViewBag.fechaLiqui = fechaLiqui;
            }


            if (servicio != null && fechaI != null && fechaf != null && TipoPago != null && TipoCuenta != null && TipoRango != null && fechaLiqui != null && TipoLiqui != null)
            {
                ViewBag.tipoRango = TipoRango;
                ViewBag.servicio = servicio;
                ViewBag.fechaI = fechaI;
                ViewBag.fechaF = fechaf;
                ViewBag.tipoPago = TipoPago;
                ViewBag.tipoCuenta = TipoCuenta;
                ViewBag.TipoLiqui = TipoLiqui;
                ViewBag.fechaLiqui = fechaLiqui;

                if (TipoCuenta == "F")
                {

                    DateTime fechaIParse = DateTime.Parse(fechaI);
                    DateTime fechaFParse = DateTime.Parse(fechaf);
                    fechaI = fechaIParse.ToString("yyyy-MM-dd");
                    fechaf = fechaFParse.ToString("yyyy-MM-dd");

                }
                else
                {

                    DateTime fechaIParse = DateTime.Parse(fechaI);
                    DateTime fechaFParse = DateTime.Parse(fechaf);
                    fechaI = fechaIParse.ToString("dd-MM-yyyy");
                    fechaf = fechaFParse.ToString("dd-MM-yyyy");

                }

                if (TipoRango == 1)
                {

                    DateTime fechaIParse = DateTime.Parse(fechaI);
                    DateTime fechaFParse = DateTime.Parse(fechaf);
                    fechaI = fechaIParse.ToString("dd/MM/yyyy");
                    fechaf = fechaFParse.ToString("dd/MM/yyyy");

                }

                if (TipoRango == 1)
                {
                    ViewBag.lista = (List<E_Honorario_Cabecera_Detalle>)ListaHonorariosxServicio(fechaI, fechaf, TipoPago, TipoCuenta, servicio).Where(x => x.EstadoD == "G").ToList();
                }
                else if (TipoRango == 2)
                {
                    ViewBag.Lista = (List<E_Honorario_Cabecera_Detalle>)ListaHonorariosxServicio_FechaEmision(fechaI, fechaf, TipoPago, TipoCuenta, servicio).Where(x => x.EstadoD == "G").ToList();
                }

            }
            else
            {
                ViewBag.mensaje = "Llene todos los campos del formulario";
            }

            return View();
        }
        public ActionResult RegistrarHonorariosMedicoMixto(string servicio = null, string medico = null, string fechaI = null, string fechaf = null, string TipoPago = null, string TipoCuenta = null, int? TipoRango = null, string fechaLiqui = null, int? TipoLiqui = null)
        {
            UtilitarioController u = new UtilitarioController();
            TarifarioController ta = new TarifarioController();
            MedicosController medi = new MedicosController();
            ViewBag.tarifario = (List<E_Tarifario>)ta.ListadoTarifa().ToList();

            E_Master reg = u.ListadoHoraServidor().FirstOrDefault(); ViewBag.tipoRango = TipoRango;
            ViewBag.tipoRango = TipoRango;
            ViewBag.servicio = servicio;
            ViewBag.medico = medico;
            ViewBag.fechaI = fechaI;
            ViewBag.fechaF = fechaf;
            ViewBag.tipoPago = TipoPago;
            ViewBag.tipoCuenta = TipoCuenta;
            ViewBag.TipoLiqui = TipoLiqui;
            if (fechaLiqui == null)
            {
                ViewBag.fechaLiqui = reg.HoraServidor.ToShortDateString();
            }
            else
            {
                ViewBag.fechaLiqui = fechaLiqui;
            }


            if (servicio != null && medico != null && fechaI != null && fechaf != null && TipoPago != null && TipoCuenta != null && TipoRango != null && fechaLiqui != null && TipoLiqui != null)
            {
                E_Medico med = medi.ListadoMedico().Find(x => x.CodMed == medico);
                ViewBag.PagoTurno = med.PagoTurno;
                ViewBag.tipoRango = TipoRango;
                ViewBag.servicio = servicio;
                ViewBag.medico = medico;
                ViewBag.fechaI = fechaI;
                ViewBag.fechaF = fechaf;
                ViewBag.tipoPago = TipoPago;
                ViewBag.tipoCuenta = TipoCuenta;
                ViewBag.TipoLiqui = TipoLiqui;
                ViewBag.fechaLiqui = fechaLiqui;


                DateTime fechaIParse = DateTime.Parse(fechaI);
                DateTime fechaFParse = DateTime.Parse(fechaf);
                fechaI = fechaIParse.ToString("dd/MM/yyyy");
                fechaf = fechaFParse.ToString("dd/M/yyyy");


                if (TipoRango == 1)
                {
                    ViewBag.Lista = (List<E_Honorario_Cabecera_Detalle>)ListaHonorariosxMedico(fechaI, fechaf, TipoPago, TipoCuenta, medico, servicio).Where(x => x.EstadoD == "G").ToList();
                }
                else if (TipoRango == 2)
                {
                    ViewBag.Lista = (List<E_Honorario_Cabecera_Detalle>)ListaHonorariosxMedico_FechaEmision(fechaI, fechaf, TipoPago, TipoCuenta, medico, servicio).Where(x => x.EstadoD == "G").ToList();
                }

            }
            else
            {
                ViewBag.mensaje = "Llene todos los campos del formulario";
            }


            return View();
        }


        public List<E_Honorario_Cabecera_Detalle> ListaHonorariosxMedico(string fechaInicio, string fechaFin, string TipoPago, string TipoCuenta, string Medico, string Servicio)
        {

            List<E_Honorario_Cabecera_Detalle> Lista = new List<E_Honorario_Cabecera_Detalle>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ListaHonorariosxMedico", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
                    cmd.Parameters.AddWithValue("@TipoPago", TipoPago);
                    cmd.Parameters.AddWithValue("@TipoCuenta", TipoCuenta);
                    cmd.Parameters.AddWithValue("@Medico", Medico);
                    cmd.Parameters.AddWithValue("@Servicio", Servicio);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Honorario_Cabecera_Detalle HCD = new E_Honorario_Cabecera_Detalle();

                            HCD.CodSede = dr.GetString(0);
                            HCD.Historia = dr.GetInt32(1);
                            HCD.CodCuenta = dr.GetInt32(2);
                            HCD.ItemCita = dr.GetInt32(3);
                            HCD.CodDocSerie = (dr["CodDocSerie"] is DBNull) ? string.Empty : dr["CodDocSerie"].ToString();
                            HCD.SerieDocumento = (dr["Serie"] is DBNull) ? string.Empty : dr["Serie"].ToString();
                            HCD.NumDoc = (dr["NumDoc"] is DBNull) ? string.Empty : dr["NumDoc"].ToString();
                            HCD.CTprecio = dr.GetDecimal(7);
                            HCD.PorcentajeTarifa = dr.GetDecimal(8);
                            HCD.Total = dr.GetDecimal(9);
                            HCD.EstadoD = dr.GetString(10);
                            HCD.fechaCrea = dr.GetDateTime(11);
                            HCD.FechaAtencion = dr.GetString(12);
                            HCD.FechaEmision = (dr["FechaEmision"] is DBNull) ? string.Empty : dr["FechaEmision"].ToString();
                            HCD.Tarifa = dr.GetString(14);
                            HCD.Cantidad = dr.GetInt32(15);
                            HCD.Turno = (dr["Turno"] is DBNull) ? string.Empty : dr["Turno"].ToString();
                            HCD.CodMed = (dr["CodMed"] is DBNull) ? string.Empty : dr["CodMed"].ToString();
                            HCD.CodServ = dr.GetString(18);
                            HCD.SecFactCaja = (dr["SetFact"] is DBNull) ? 0 : (int)dr["SetFact"];
                            HCD.primario = (dr["primario"] is DBNull) ? 0 : (int)dr["primario"];
                            Lista.Add(HCD);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }

        }


        public List<E_Honorario_Cabecera_Detalle> Usp_Lista_HonorarioCabeceraXId(int? id = null)
        {
            List<E_Honorario_Cabecera_Detalle> Lista = new List<E_Honorario_Cabecera_Detalle>();
            MedicosController med = new MedicosController();
            ServiciosController ser = new ServiciosController();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_HonorarioCabeceraXId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDc", id);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            E_Honorario_Cabecera_Detalle HCD = new E_Honorario_Cabecera_Detalle();
                            var servicio = ser.ListadoServicios().Where(x => x.CodServ == dr.GetString(0)).FirstOrDefault();
                            var medico = med.ListadoMedico().Where(x => x.CodMed == dr.GetString(1)).FirstOrDefault();
                            HCD.CodServ = servicio.NomServ;
                            if (medico == null)
                            {
                                HCD.CodMed = "--";
                            }
                            else
                            {
                                HCD.CodMed = medico.NomMed;
                            }

                            HCD.TipoDoc = dr.GetString(2);
                            HCD.TipoPago = dr.GetString(3);
                            HCD.FechaLiquidacion = dr.GetDateTime(4);
                            HCD.FechaInicio = dr.GetDateTime(5);
                            HCD.FechaFinal = dr.GetDateTime(6);
                            HCD.TotalFac = dr.GetDecimal(7);
                            HCD.CantTurno = dr.GetInt32(8);
                            HCD.PagoTotalTurno = dr.GetDecimal(9);
                            HCD.Total = dr.GetDecimal(10);

                            Lista.Add(HCD);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }


        }

        public List<E_Honorario_Cabecera_Detalle> Usp_Lista_HonorarioDetalleXId(int? id = null)
        {
            List<E_Honorario_Cabecera_Detalle> Lista = new List<E_Honorario_Cabecera_Detalle>();
            TarifarioController tar = new TarifarioController();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_HonorarioDetalleXId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDc", id);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            E_Honorario_Cabecera_Detalle HCD = new E_Honorario_Cabecera_Detalle();

                            HCD.CodCuenta = dr.GetInt32(0);
                            HCD.ItemCita = dr.GetInt32(1);
                            HCD.NroDocumentos = dr.GetInt32(2);
                            HCD.Tot_Tarifa = dr.GetDecimal(3);
                            HCD.PorcentajeTarifa = Convert.ToInt32(dr.GetDecimal(4));
                            HCD.APagar = dr.GetDecimal(5);
                            HCD.FechaAtencion = dr["FecAtencion"] is DBNull ? string.Empty : dr["FecAtencion"].ToString();
                            HCD.FechaEmision = dr.GetDateTime(7).ToShortDateString();
                            var tarifa = tar.ListadoTarifa().Where(x => x.CodTar == dr.GetString(8)).FirstOrDefault();
                            HCD.Tarifa = tarifa.DescTar;
                            HCD.Cantidad = dr.GetInt32(9);
                            HCD.Turno = dr.GetString(10);
                            Lista.Add(HCD);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }


        }



        public List<E_Honorario_Cabecera_Detalle> Usp_TotalTurnos(string cadena, string tipo)
        {
            List<E_Honorario_Cabecera_Detalle> Lista = new List<E_Honorario_Cabecera_Detalle>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_LiquidaxTurno", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cadena", cadena);
                    cmd.Parameters.AddWithValue("@tipo", tipo);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            //CodCue , item , NroDocumento , Tot_Tarifa , PorcentajeTarifa , APagar , FecAtencion , FecEmision , Tarifa , Cant , Turno
                            E_Honorario_Cabecera_Detalle HCD = new E_Honorario_Cabecera_Detalle();

                            HCD.TotalTurnos = dr.GetInt32(0);
                            HCD.fechaTur = dr.GetString(1);
                            HCD.Turno = dr.GetString(2);
                            Lista.Add(HCD);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }


        }



        public List<E_Honorario_Cabecera_Detalle> ListaHonorariosxServicio(string fechaInicio, string fechaFin, string TipoPago, string TipoCuenta, string servicio)
        {
            List<E_Honorario_Cabecera_Detalle> Lista = new List<E_Honorario_Cabecera_Detalle>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ListaHonorariosxServicio", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
                    cmd.Parameters.AddWithValue("@TipoPago", TipoPago);
                    cmd.Parameters.AddWithValue("@TipoCuenta", TipoCuenta);
                    cmd.Parameters.AddWithValue("@Servicio", servicio);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Honorario_Cabecera_Detalle HCD = new E_Honorario_Cabecera_Detalle();

                            HCD.CodSede = dr.GetString(0);
                            HCD.Historia = dr.GetInt32(1);
                            HCD.CodCuenta = dr.GetInt32(2);
                            HCD.ItemCita = dr.GetInt32(3);
                            HCD.CodDocSerie = (dr["CodDocSerie"] is DBNull) ? string.Empty : dr["CodDocSerie"].ToString();
                            HCD.SerieDocumento = (dr["Serie"] is DBNull) ? string.Empty : dr["Serie"].ToString();
                            HCD.NumDoc = (dr["NumDoc"] is DBNull) ? string.Empty : dr["NumDoc"].ToString();
                            HCD.CTprecio = dr.GetDecimal(7);
                            HCD.PorcentajeTarifa = dr.GetDecimal(8);
                            HCD.Total = dr.GetDecimal(9);
                            HCD.EstadoD = dr.GetString(10);
                            HCD.fechaCrea = dr.GetDateTime(11);
                            HCD.FechaAtencion = dr.GetString(12);
                            HCD.FechaEmision = (dr["FechaEmision"] is DBNull) ? string.Empty : dr["FechaEmision"].ToString();
                            HCD.Tarifa = dr.GetString(14);
                            HCD.Cantidad = dr.GetInt32(15);
                            HCD.Turno = (dr["Turno"] is DBNull) ? string.Empty : dr["Turno"].ToString();
                            HCD.CodMed = (dr["CodMed"] is DBNull) ? string.Empty : dr["CodMed"].ToString();
                            HCD.CodServ = dr.GetString(18);
                            HCD.SecFactCaja = (dr["SetFact"] is DBNull) ? 0 : (int)dr["SetFact"];
                            Lista.Add(HCD);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public List<E_Honorario_Cabecera_Detalle> ListaHonorariosxMedico_FechaEmision(string fechaInicio, string fechaFin, string TipoPago, string TipoCuenta, string medico, string Servicio)
        {
            List<E_Honorario_Cabecera_Detalle> Lista = new List<E_Honorario_Cabecera_Detalle>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ListaHonorariosxMedico_FechaEmision", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
                    cmd.Parameters.AddWithValue("@TipoPago", TipoPago);
                    cmd.Parameters.AddWithValue("@TipoCuenta", TipoCuenta);
                    cmd.Parameters.AddWithValue("@Medico", medico);
                    cmd.Parameters.AddWithValue("@Servicio", Servicio);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Honorario_Cabecera_Detalle HCD = new E_Honorario_Cabecera_Detalle();


                            HCD.CodSede = dr.GetString(0);
                            HCD.Historia = dr.GetInt32(1);
                            HCD.CodCuenta = dr.GetInt32(2);
                            HCD.ItemCita = dr.GetInt32(3);
                            HCD.CodDocSerie = (dr["CodDocSerie"] is DBNull) ? string.Empty : dr["CodDocSerie"].ToString();
                            HCD.SerieDocumento = (dr["Serie"] is DBNull) ? string.Empty : dr["Serie"].ToString();
                            HCD.NumDoc = (dr["NumDoc"] is DBNull) ? string.Empty : dr["NumDoc"].ToString();
                            HCD.CTprecio = dr.GetDecimal(7);
                            HCD.PorcentajeTarifa = dr.GetDecimal(8);
                            HCD.Total = dr.GetDecimal(9);
                            HCD.EstadoD = dr.GetString(10);
                            HCD.fechaCrea = dr.GetDateTime(11);
                            HCD.FechaAtencion = dr.GetString(12);
                            HCD.FechaEmision = (dr["FechaEmision"] is DBNull) ? string.Empty : dr["FechaEmision"].ToString();
                            HCD.Tarifa = dr.GetString(14);
                            HCD.Cantidad = dr.GetInt32(15);
                            HCD.Turno = (dr["Turno"] is DBNull) ? string.Empty : dr["Turno"].ToString();
                            HCD.CodMed = (dr["CodMed"] is DBNull) ? string.Empty : dr["CodMed"].ToString();
                            HCD.CodServ = dr.GetString(18);
                            HCD.SecFactCaja = (dr["SetFact"] is DBNull) ? 0 : (int)dr["SetFact"];
                            HCD.primario = (dr["primario"] is DBNull) ? 0 : (int)dr["primario"];
                            Lista.Add(HCD);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }

        }


        public List<E_Honorario_Cabecera_Detalle> ListaHonorariosxServicio_FechaEmision(string fechaInicio, string fechaFin, string TipoPago, string TipoCuenta, string servicio)
        {
            List<E_Honorario_Cabecera_Detalle> Lista = new List<E_Honorario_Cabecera_Detalle>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ListaHonorariosxServicio_FechaEmision", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
                    cmd.Parameters.AddWithValue("@TipoPago", TipoPago);
                    cmd.Parameters.AddWithValue("@TipoCuenta", TipoCuenta);
                    cmd.Parameters.AddWithValue("@Servicio", servicio);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Honorario_Cabecera_Detalle HCD = new E_Honorario_Cabecera_Detalle();

                            HCD.CodSede = dr.GetString(0);
                            HCD.Historia = dr.GetInt32(1);
                            HCD.CodCuenta = dr.GetInt32(2);
                            HCD.ItemCita = dr.GetInt32(3);
                            HCD.CodDocSerie = (dr["CodDocSerie"] is DBNull) ? string.Empty : dr["CodDocSerie"].ToString();
                            HCD.SerieDocumento = (dr["Serie"] is DBNull) ? string.Empty : dr["Serie"].ToString();
                            HCD.NumDoc = (dr["NumDoc"] is DBNull) ? string.Empty : dr["NumDoc"].ToString();
                            HCD.CTprecio = dr.GetDecimal(7);
                            HCD.PorcentajeTarifa = dr.GetDecimal(8);
                            HCD.Total = dr.GetDecimal(9);
                            HCD.EstadoD = dr.GetString(10);
                            HCD.fechaCrea = dr.GetDateTime(11);
                            HCD.FechaAtencion = dr.GetString(12);
                            HCD.FechaEmision = (dr["FechaEmision"] is DBNull) ? string.Empty : dr["FechaEmision"].ToString();
                            HCD.Tarifa = dr.GetString(14);
                            HCD.Cantidad = dr.GetInt32(15);
                            HCD.Turno = (dr["Turno"] is DBNull) ? string.Empty : dr["Turno"].ToString();
                            HCD.CodMed = (dr["CodMed"] is DBNull) ? string.Empty : dr["CodMed"].ToString();
                            HCD.CodServ = dr.GetString(18);
                            HCD.SecFactCaja = (dr["SetFact"] is DBNull) ? 0 : (int)dr["SetFact"];
                            Lista.Add(HCD);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }


        }


        public List<E_Honorario_Cabecera_Detalle> ListaDeTurnosxFEmision(string CodMed, string IdC)
        {
            List<E_Honorario_Cabecera_Detalle> Lista = new List<E_Honorario_Cabecera_Detalle>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("busca_NroTurno", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodMed", CodMed);
                    cmd.Parameters.AddWithValue("@IdC", IdC);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Honorario_Cabecera_Detalle HCD = new E_Honorario_Cabecera_Detalle();

                            HCD.FechaGroup = dr.GetDateTime(0);
                            HCD.Turno = dr.GetString(1);
                            HCD.PagaTurno = dr.GetDecimal(2);
                            Lista.Add(HCD);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }


        }

        public JsonResult GroupVFinal(string cadena, string tipo)
        {
            var cuenta = Usp_TotalTurnos(cadena, tipo).ToList();
            E_Honorario_Cabecera_Detalle ho = new E_Honorario_Cabecera_Detalle();
            ho.TotalTurnos = cuenta.Count();

            List<E_Honorario_Cabecera_Detalle> Lista = new List<E_Honorario_Cabecera_Detalle>();
            Lista.Add(ho);

            if (Lista.Count() != 0)
            {
                return Json(Lista, JsonRequestBehavior.AllowGet);
            }

            return null;

        }


        int cantidad = 0;
        public JsonResult ObtenerGroupBy(string fechaEmision, string fechaAtencion, string turno, int id, string tipoRango, int inicio, int final, string codMed)
        {
            if (inicio == 0)
            {
                if (Session["temporal"] == null)
                {
                    Session["temporal"] = new List<E_Honorario_Cabecera_Detalle>();
                }
                else
                {
                    Session.Remove("temporal");
                    Session["temporal"] = new List<E_Honorario_Cabecera_Detalle>();
                }
            }


            if (final != 123456789)
            {

                var lista = (List<E_Honorario_Cabecera_Detalle>)Session["temporal"];

                var controla = lista.Find(x => x.idTemporal == id);

                if (controla == null)
                {
                    E_Honorario_Cabecera_Detalle item = new E_Honorario_Cabecera_Detalle();
                    item.idTemporal = id;
                    item.FechaEmision = fechaEmision;
                    item.FechaAtencion = fechaAtencion;
                    item.Turno = turno;
                    lista.Add(item);
                    Session["temporal"] = lista;
                }
                else
                {
                    var registro = lista.Where(x => x.idTemporal.Equals(id)).FirstOrDefault();
                    lista.Remove(registro);
                    Session["atenciones"] = lista;
                }



                if (tipoRango == "1")
                {

                    var demo = lista.GroupBy(x => x.FechaAtencion, x => x.Turno);
                    cantidad = demo.Count();

                }
                else
                {

                    var demo = lista.GroupBy(x => x.FechaEmision, x => x.Turno);
                    cantidad = demo.Count();

                }
                MedicosController medi = new MedicosController();
                E_Medico med = medi.ListadoMedico().Find(x => x.CodMed == codMed);
                List<E_Honorario_Cabecera_Detalle> Lista = new List<E_Honorario_Cabecera_Detalle>();
                E_Honorario_Cabecera_Detalle listita = new E_Honorario_Cabecera_Detalle();
                listita.Cantidad = cantidad;
                listita.Precio = med.PagoTurno;
                Lista.Add(listita);

                return Json(Lista, JsonRequestBehavior.AllowGet);

            }

            else
            {

                List<E_Honorario_Cabecera_Detalle> Lista = new List<E_Honorario_Cabecera_Detalle>();
                E_Honorario_Cabecera_Detalle listita = new E_Honorario_Cabecera_Detalle();
                listita.Cantidad = 0;
                listita.Precio = 0;
                Lista.Add(listita);

                return Json(Lista, JsonRequestBehavior.AllowGet);

            }

        }

        public JsonResult ListarMedicos(string codserv)
        {

            MedicosController med = new MedicosController();
            List<E_Medico> Lista = med.ListadoMedico().Where(x => x.CodServ == codserv && x.EstMed == true && x.EnLista == true).ToList();
            if (Lista.Count() != 0)
            {
                return Json(Lista, JsonRequestBehavior.AllowGet);
            }

            return null;

        }
        public JsonResult Mirame(int Total)
        {

            MedicosController med = new MedicosController();
            int demo = Total;

            return null;

        }
        public JsonResult ListaServicios()
        {
            ServiciosController ser = new ServiciosController();
            List<E_Servicios> Lista = ser.ListadoServicios().Where(x => x.EstServ == true).ToList();
            if (Lista.Count() != 0)
            {

                return Json(Lista, JsonRequestBehavior.AllowGet);

            }
            return null;
        }

        public ActionResult ListadeLiquidacion(string CodMed = null, string CodServ = null, string fechai = null, string fechaf = null)
        {
            ServiciosController ser = new ServiciosController();
            MedicosController med = new MedicosController();
            if (CodMed == "") { CodMed = null; }
            if (CodServ == "") { CodServ = null; }
            ViewBag.fechai = fechai; ViewBag.fechaf = fechaf;
            ViewBag.CodServ = ""; ViewBag.CodMed = "";
            ViewBag.ListaServicio = new SelectList(ser.ListadoServicios().Where(x => x.EstServ == true).ToList(), "CodServ", "NomServ");
            ViewBag.ListaMedico = new SelectList(med.ListadoMedico().Where(x => x.EstMed == true).ToList(), "CodMed", "NomMed");
            if (!string.IsNullOrWhiteSpace(CodMed))
            {
                ViewBag.CodMed = CodMed;


                ViewBag.medico = ListaLiquidacionXMedico(CodMed, fechai, fechaf);

            }
            else if (!string.IsNullOrWhiteSpace(CodServ))
            {
                ViewBag.CodServ = CodServ;
                ViewBag.servicio = ListaLiquidacionXServicio(CodServ, fechai, fechaf);

            }
            else
            {
                ViewBag.medico = null;
                ViewBag.servicio = null;

            }


            return View();

        }

        public ActionResult VerDetalleHonorarios(int? id = null)
        {

            var cabecera = Usp_Lista_HonorarioCabeceraXId(id).FirstOrDefault();
            ViewBag.detalle = Usp_Lista_HonorarioDetalleXId(id).ToList();

            return View(cabecera);
        }


        public List<E_Honorario_Cabecera_Detalle> ListaLiquidacionXMedico(string Medico, string fechai, string fechaf)
        {
            List<E_Honorario_Cabecera_Detalle> Lista = new List<E_Honorario_Cabecera_Detalle>();
            MedicosController med = new MedicosController();
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_LiquidacionXMedicoORServicio", cnn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodMed", Medico);
                    cmd.Parameters.AddWithValue("@CodSer", "");
                    cmd.Parameters.AddWithValue("@fechai", fechai);
                    cmd.Parameters.AddWithValue("@fechaf", fechaf);
                    cmd.Parameters.AddWithValue("@Evento", 2);
                    cnn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_Honorario_Cabecera_Detalle cd = new E_Honorario_Cabecera_Detalle();
                        cd.IdC = dr.GetInt32(0).ToString();
                        var medico = med.ListadoMedico().Where(x => x.CodMed == dr.GetString(1)).FirstOrDefault();
                        cd.CodMed = medico.CodMed + "-" + medico.NomMed;
                        cd.FechaLiquidacion = dr.GetDateTime(2);
                        cd.FechaInicio = dr.GetDateTime(3);
                        cd.FechaFinal = dr.GetDateTime(4);
                        cd.TipoDoc = dr.GetString(5);
                        cd.TotalFac = dr.GetDecimal(6);
                        cd.Total = dr.GetDecimal(7);
                        Lista.Add(cd);
                    }
                }
            }
            return Lista;
        }
        public List<E_Honorario_Cabecera_Detalle> ListaLiquidacionXServicio(string Servicio, string fechai, string fechaf)
        {
            List<E_Honorario_Cabecera_Detalle> Lista = new List<E_Honorario_Cabecera_Detalle>();
            ServiciosController serv = new ServiciosController();

            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_LiquidacionXMedicoORServicio", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodMed", "");
                    cmd.Parameters.AddWithValue("@CodSer", Servicio);
                    cmd.Parameters.AddWithValue("@fechai", fechai);
                    cmd.Parameters.AddWithValue("@fechaf", fechaf);
                    cmd.Parameters.AddWithValue("@Evento", 1);
                    cnn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        E_Honorario_Cabecera_Detalle cd = new E_Honorario_Cabecera_Detalle();
                        cd.IdC = dr.GetInt32(0).ToString();
                        var servicio = serv.ListadoServicios().Where(x => x.CodServ == dr.GetString(1)).FirstOrDefault();
                        cd.CodServ = servicio.CodServ + "-" + servicio.NomServ;
                        cd.FechaLiquidacion = dr.GetDateTime(2);
                        cd.FechaInicio = dr.GetDateTime(3);
                        cd.FechaFinal = dr.GetDateTime(4);
                        cd.TipoDoc = dr.GetString(5);
                        cd.TotalFac = dr.GetDecimal(6);
                        cd.Total = dr.GetDecimal(7);
                        Lista.Add(cd);
                    }
                }
            }

            return Lista;

        }


    }
}