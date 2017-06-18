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
using Microsoft.Reporting.WebForms;
using System.IO;

namespace VgSalud.Controllers
{
    public class ReportesController : Controller
    {

        public ActionResult ReporteGastos(string FechaI = null, string FechaF = null, string Idcc = null, string accion = null)
        {
            CentroCostoController costo = new CentroCostoController();
            ViewBag.centro = new SelectList(costo.ListaCentroCosto().Where(x => x.Estado == true), "Idcc", "Descripcion");
            ViewBag.fechaI = FechaI; ViewBag.fechaF = FechaF; ViewBag.idd = Idcc;
            if (!string.IsNullOrWhiteSpace(FechaI) && !string.IsNullOrWhiteSpace(FechaF) && !string.IsNullOrWhiteSpace(Idcc))
            {
                if (accion == "1")
                {
                    ViewBag.listado = Reporte_Lista_Gastos_XFechaI_XFechaF_XCentroCosto(Convert.ToDateTime(FechaI), Convert.ToDateTime(FechaF), Idcc);
                }
                else
                {
                    ViewBag.listado = null;
                }
            }
            return View();
        }

        public ActionResult GeneraReporteGastos(string id, string FechaI, string FechaF, string Idcc)
        {

            string sede = Session["codSede"].ToString();

            DateTime FechaIni = DateTime.Parse(FechaI);
            DateTime FechaFin = DateTime.Parse(FechaF);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReporteDeGastos.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ReporteGastos");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Gastos> cm = Reporte_Lista_Gastos_XFechaI_XFechaF_XCentroCosto(FechaIni, FechaFin, Idcc);
            ReportDataSource rd = new ReportDataSource("DataSet1", cm);
            lr.DataSources.Add(rd);
            UtilitarioController u = new UtilitarioController();
            SedesController sed = new SedesController();
            ServiciosController ser = new ServiciosController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();
            E_Sede us = sed.ListadoSedes().Find(x => x.CodSede == sede);
            string nombreCompleto = us.NomSede;


            ReportParameter[] rptParameters = new ReportParameter[] {
                new ReportParameter("fechaHoy", ma.HoraServidor.ToString("dd/MM/yyyy hh:mm tt")),
                new ReportParameter("FechaInicio", FechaIni.ToString("dd/MM/yyyy")),
                new ReportParameter("FechaFinal", FechaFin.ToString("dd/MM/yyyy")),
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("NomSede", nombreCompleto)
            };

            lr.SetParameters(rptParameters);

            string reportType = id;
            string mimeType;
            string encoding;
            string FileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "   <OutputFormat>" + id + "</OutputFormat>" +
                "   <PageWidth>8in</PageWidth>" +
                "   <PageHeight>10in</PageHeight>" +
                "   <MarginTop>0.3in</MarginTop>" +
                "   <MarginLeft>0in</MarginLeft>" +
                "   <MarginRight>0in</MarginRight>" +
                "   <MarginBottom>0.3in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] rendereBytes;

            rendereBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out FileNameExtension,
                out streams,
                out warnings);

            return File(rendereBytes, mimeType);
        }

        public List<E_Gastos> Reporte_Lista_Gastos_XFechaI_XFechaF_XCentroCosto(DateTime FechaI, DateTime FechaF, string Idcc)
        {
            List<E_Gastos> Lista = new List<E_Gastos>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Reporte_Lista_Gastos_XFechaI_XFechaF_XCentroCosto", con))
                {
                    cmd.Parameters.AddWithValue("@FechaI", FechaI);
                    cmd.Parameters.AddWithValue("@FechaF", FechaF);
                    cmd.Parameters.AddWithValue("@CentroC", Idcc);

                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Gastos ca = new E_Gastos();

                            //gc.IdgastosC , NroOperacion , RazonS , Ruc , FechaEmision , gd.importe
                            ca.IdgastosC = dr.GetInt32(0);
                            ca.NroOperacion = dr.GetString(1);
                            ca.RazonS = dr.GetString(2);
                            ca.Ruc = dr.GetString(3);
                            ca.FechaEmision = dr.GetDateTime(4);
                            ca.TotalBruto = dr.GetDecimal(5);


                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_Reporte> usp_ListaTarifaMedicoEnvia()
        {
            List<E_Reporte> Lista = new List<E_Reporte>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ListaTarifaMedicoEnvia", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Reporte ca = new E_Reporte();

                            ca.CodTar = dr.GetString(0);
                            ca.DescTar = dr.GetString(1);
                            ca.CodServ = dr.GetString(2);
                            ca.CodSede = dr.GetString(3);

                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Caja> ListaFacturaxUsuario(string CodUsu, DateTime FechaI, DateTime FechaF, TimeSpan HoraI, TimeSpan HoraF)
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaFacturaxUsuario", con))
                {
                    cmd.Parameters.AddWithValue("@fechaInicio", FechaI);
                    cmd.Parameters.AddWithValue("@fechaFin", FechaF);
                    cmd.Parameters.AddWithValue("@horaInicio", HoraI);
                    cmd.Parameters.AddWithValue("@horaFin", HoraF);
                    cmd.Parameters.AddWithValue("@CodUsu", CodUsu);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja ca = new E_Caja();


                            ca.CodCaja = dr.GetInt32(1);
                            ca.CodSede = dr.GetString(2);
                            ca.CodCue = dr.GetInt32(3);
                            ca.AliasCodDoc = dr.GetString(4);
                            ca.Serie = dr.GetString(5);
                            ca.NumDoc = dr.GetString(6);
                            ca.FechaEmision = dr.GetDateTime(7);
                            ca.HoraEmision = dr.GetTimeSpan(8);
                            ca.RazonSocial = dr.GetString(9);
                            ca.DescCatPac = dr.GetString(10);
                            ca.Estado = dr.GetBoolean(11);
                            ca.SubTotal = dr.GetDecimal(12);
                            ca.Igv = dr.GetDecimal(13);
                            ca.Total = dr.GetDecimal(0);
                            ca.UsuCrea = dr.GetString(15);
                            ca.UsuAnula = dr.GetString(16);
                            ca.FechaAnula = dr.GetString(17);

                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Reporte> ListaDeCantidadxVentas()
        {
            List<E_Reporte> Lista = new List<E_Reporte>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ReporteChartCantidadVentas", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Reporte Ser = new E_Reporte();
                            Ser.fechaEmision = dr.GetDateTime(0);
                            Ser.cantidad = dr.GetInt32(1);


                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_Caja> ListaFacturaxSede(string CodSede, int CodDoc, string serie, DateTime FechaI, DateTime FechaF, TimeSpan HoraI, TimeSpan HoraF)
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaFacturaxSede", con))
                {
                    cmd.Parameters.AddWithValue("@fechaInicio", FechaI);
                    cmd.Parameters.AddWithValue("@fechaFin", FechaF);
                    cmd.Parameters.AddWithValue("@horaInicio", HoraI);
                    cmd.Parameters.AddWithValue("@horaFin", HoraF);
                    cmd.Parameters.AddWithValue("@CodSede", CodSede);
                    cmd.Parameters.AddWithValue("@CodDocSerie", CodDoc);
                    cmd.Parameters.AddWithValue("@Serie", serie);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja ca = new E_Caja();

                            ca.CodCaja = dr.GetInt32(1);
                            ca.CodSede = dr.GetString(2);
                            ca.CodCue = dr.GetInt32(3);
                            ca.AliasCodDoc = dr.GetString(4);
                            ca.Serie = dr.GetString(5);
                            ca.NumDoc = dr.GetString(6);
                            ca.FechaEmision = dr.GetDateTime(7);
                            ca.HoraEmision = dr.GetTimeSpan(8);
                            ca.RazonSocial = dr.GetString(9);
                            ca.DescCatPac = dr.GetString(10);
                            ca.Estado = dr.GetBoolean(11);
                            ca.SubTotal = dr.GetDecimal(12);
                            ca.Igv = dr.GetDecimal(13);
                            ca.Total = dr.GetDecimal(0);
                            ca.UsuCrea = dr.GetString(15);
                            ca.UsuAnula = dr.GetString(16);
                            ca.FechaAnula = dr.GetString(17);

                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Honorario_Cabecera_Detalle> ListaLiquidacion(int Tipo, string comodin, DateTime fechaInicio, DateTime fechaFin, string CodSede)
        {
            List<E_Honorario_Cabecera_Detalle> Lista = new List<E_Honorario_Cabecera_Detalle>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaLiquidacion", con))
                {
                    cmd.Parameters.AddWithValue("@Tipo", Tipo);
                    cmd.Parameters.AddWithValue("@comodin", comodin);
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaFin", fechaFin);
                    cmd.Parameters.AddWithValue("@CodSede", CodSede);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Honorario_Cabecera_Detalle Ser = new E_Honorario_Cabecera_Detalle();


                            Ser.nombre = dr.GetString(0);
                            Ser.FechaLiquidacion = dr.GetDateTime(1);
                            Ser.NroDocumentos = dr.GetInt32(2);
                            Ser.NroAtenciones = dr.GetInt32(3);
                            Ser.Tot_Tarifa = dr.GetDecimal(4);
                            Ser.APagar = dr.GetDecimal(5);

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }



        public List<E_Reporte> ReporteTarifaMasVendidadxServicio(DateTime fechaI, DateTime fechaF, string cadena)
        {
            List<E_Reporte> Lista = new List<E_Reporte>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ReporteTarifaMasVendidadxServicio", con))
                {
                    cmd.Parameters.AddWithValue("@fechaI", fechaI);
                    cmd.Parameters.AddWithValue("@fechaF", fechaF);
                    cmd.Parameters.AddWithValue("@Str", cadena);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Reporte Ser = new E_Reporte();
                            Ser.NomServ = dr.GetString(0);
                            Ser.fechaEmision = dr.GetDateTime(1);
                            Ser.DescTar = dr.GetString(2);
                            Ser.cantidad = dr.GetInt32(3);
                            Ser.total = dr.GetDecimal(4);

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Reporte> ReporteTarifaMasVendidadxEspecialidad(DateTime fechaI, DateTime fechaF, string cadena)
        {
            List<E_Reporte> Lista = new List<E_Reporte>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ReporteTarifaMasVendidadxEspecialidad", con))
                {
                    cmd.Parameters.AddWithValue("@fechaI", fechaI);
                    cmd.Parameters.AddWithValue("@fechaF", fechaF);
                    cmd.Parameters.AddWithValue("@Str", cadena);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Reporte Ser = new E_Reporte();
                            Ser.NomEspec = dr.GetString(0);
                            Ser.fechaEmision = dr.GetDateTime(1);
                            Ser.DescTar = dr.GetString(2);
                            Ser.cantidad = dr.GetInt32(3);
                            Ser.total = dr.GetDecimal(4);

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public ActionResult ReporteTarifaMasVendidadxServicios(DateTime? fechaI = null, DateTime? fechaF = null, string[] CodServ = null)
        {
            string sede = Session["codSede"].ToString();
            ServiciosController s = new ServiciosController();
            ViewBag.listadoServicio = new SelectList(s.ListadoServicios().Where(x => x.CodSede == sede && x.EstServ == true), "CodServ", "NomServ", CodServ);

            if (fechaI != null && fechaF != null && CodServ != null)
            {
                ViewBag.FechaInicio = DateTime.Parse(fechaI.ToString()).ToShortDateString();
                ViewBag.FechaFinal = DateTime.Parse(fechaF.ToString()).ToShortDateString();
                string Servicio = "";
                int cuenta = 1;
                foreach (string item in CodServ)
                {
                    if (item == "")
                    {
                        Servicio = "T";
                        break;
                    }
                    if (cuenta == 1)
                    {
                        Servicio = item;
                    }
                    else
                    {
                        Servicio = Servicio + "," + item;
                    }

                    cuenta++;
                }
                ViewBag.servicio = Servicio;
                ViewBag.listado = (List<E_Reporte>)ReporteTarifaMasVendidadxServicio((DateTime)fechaI, (DateTime)fechaF, Servicio).ToList();
            }
            else
            {
                ViewBag.listado = null;
            }

            return View();
        }

        public ActionResult GeneraReporteTarifaMasVendidadxServicios(string id, string Serv, string fechaI, string fechaF)
        {

            string sede = Session["codSede"].ToString();

            DateTime FechaIni = DateTime.Parse(fechaI);
            DateTime FechaFin = DateTime.Parse(fechaF);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReporteTarifaVendidadxServicio.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ReporteTarifaMasVendidadxServicios");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Reporte> cm = new List<E_Reporte>();

            cm = ReporteTarifaMasVendidadxServicio(FechaIni, FechaFin, Serv);


            ReportDataSource rd = new ReportDataSource("DataSet1", cm);

            lr.DataSources.Add(rd);

            UtilitarioController u = new UtilitarioController();
            SedesController sed = new SedesController();
            ServiciosController ser = new ServiciosController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();
            E_Sede us = sed.ListadoSedes().Find(x => x.CodSede == sede);
            string nombreCompleto = us.NomSede;

            string Servicio = "";
            int cuenta = 1;
            string[] fija = Serv.Split(',');
            foreach (string item in fija)
            {
                if (item == "T")
                {
                    Servicio = "TODOS";
                    break;
                }
                if (cuenta == 1)
                {
                    E_Servicios s = ser.ListadoServicios().Find(x => x.CodServ == item);
                    Servicio = s.NomServ;
                }
                else
                {
                    E_Servicios s = ser.ListadoServicios().Find(x => x.CodServ == item);
                    Servicio = Servicio + ", " + s.NomServ;
                }

                cuenta++;

            }



            ReportParameter[] rptParameters = new ReportParameter[] {
                new ReportParameter("fechaHoy", ma.HoraServidor.ToString("dd/MM/yyyy hh:mm tt")),
                new ReportParameter("FechaInicio", FechaIni.ToString("dd/MM/yyyy")),
                new ReportParameter("FechaFinal", FechaFin.ToString("dd/MM/yyyy")),
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("Servicios", Servicio)
            };

            lr.SetParameters(rptParameters);

            string reportType = id;
            string mimeType;
            string encoding;
            string FileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "   <OutputFormat>" + id + "</OutputFormat>" +
                "   <PageWidth>8in</PageWidth>" +
                "   <PageHeight>10in</PageHeight>" +
                "   <MarginTop>0.3in</MarginTop>" +
                "   <MarginLeft>0in</MarginLeft>" +
                "   <MarginRight>0in</MarginRight>" +
                "   <MarginBottom>0.3in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] rendereBytes;

            rendereBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out FileNameExtension,
                out streams,
                out warnings);

            return File(rendereBytes, mimeType);
        }


        public ActionResult ReporteTarifaMasVendidadxEspecialidades(DateTime? fechaI = null, DateTime? fechaF = null, string[] CodEspec = null)
        {
            string sede = Session["codSede"].ToString();
            EspecialidadController s = new EspecialidadController();
            ViewBag.listadoEspecialidad = new SelectList(s.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true), "CodEspec", "NomEspec", CodEspec);

            if (fechaI != null && fechaF != null && CodEspec != null)
            {
                ViewBag.FechaInicio = DateTime.Parse(fechaI.ToString()).ToShortDateString();
                ViewBag.FechaFinal = DateTime.Parse(fechaF.ToString()).ToShortDateString();
                string Especialidad = "";
                int cuenta = 1;
                foreach (string item in CodEspec)
                {
                    if (item == "")
                    {
                        Especialidad = "T";
                        break;
                    }
                    if (cuenta == 1)
                    {
                        Especialidad = item;
                    }
                    else
                    {
                        Especialidad = Especialidad + "," + item;
                    }

                    cuenta++;
                }
                ViewBag.espcialidad = Especialidad;
                ViewBag.listado = (List<E_Reporte>)ReporteTarifaMasVendidadxEspecialidad((DateTime)fechaI, (DateTime)fechaF, Especialidad).ToList();
            }
            else
            {
                ViewBag.listado = null;
            }

            return View();
        }

        public ActionResult GeneraReporteTarifaMasVendidadxEspecialidad(string id, string Espc, string fechaI, string fechaF)
        {

            string sede = Session["codSede"].ToString();

            DateTime FechaIni = DateTime.Parse(fechaI);
            DateTime FechaFin = DateTime.Parse(fechaF);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReporteTarifaVendidadxEspecialidad.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ReporteTarifaMasVendidadxEspecialidad");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Reporte> cm = new List<E_Reporte>();

            cm = ReporteTarifaMasVendidadxEspecialidad(FechaIni, FechaFin, Espc);


            ReportDataSource rd = new ReportDataSource("DataSet1", cm);

            lr.DataSources.Add(rd);

            UtilitarioController u = new UtilitarioController();
            SedesController sed = new SedesController();
            EspecialidadController ser = new EspecialidadController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();
            E_Sede us = sed.ListadoSedes().Find(x => x.CodSede == sede);
            string nombreCompleto = us.NomSede;

            string Servicio = "";
            int cuenta = 1;
            string[] fija = Espc.Split(',');
            foreach (string item in fija)
            {
                if (item == "T")
                {
                    Servicio = "TODOS";
                    break;
                }
                if (cuenta == 1)
                {
                    E_Especialidades s = ser.ListadoEspecialidades().Find(x => x.CodEspec == item);
                    Servicio = s.NomEspec;
                }
                else
                {
                    E_Especialidades s = ser.ListadoEspecialidades().Find(x => x.CodEspec == item);
                    Servicio = Servicio + ", " + s.NomEspec;
                }

                cuenta++;

            }



            ReportParameter[] rptParameters = new ReportParameter[] {
                new ReportParameter("fechaHoy", ma.HoraServidor.ToString("dd/MM/yyyy hh:mm tt")),
                new ReportParameter("FechaInicio", FechaIni.ToString("dd/MM/yyyy")),
                new ReportParameter("FechaFinal", FechaFin.ToString("dd/MM/yyyy")),
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("Especialidad", Servicio)
            };

            lr.SetParameters(rptParameters);

            string reportType = id;
            string mimeType;
            string encoding;
            string FileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "   <OutputFormat>" + id + "</OutputFormat>" +
                "   <PageWidth>8in</PageWidth>" +
                "   <PageHeight>10in</PageHeight>" +
                "   <MarginTop>0.3in</MarginTop>" +
                "   <MarginLeft>0in</MarginLeft>" +
                "   <MarginRight>0in</MarginRight>" +
                "   <MarginBottom>0.3in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] rendereBytes;

            rendereBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out FileNameExtension,
                out streams,
                out warnings);

            return File(rendereBytes, mimeType);
        }


        public ActionResult Liquidacion(int? Tipo = null, string CodMed = null, string CodServ = null, string FechaI = null, string FechaF = null)
        {
            string sede = Session["codSede"].ToString();

            ServiciosController u = new ServiciosController();
            MedicosController m = new MedicosController();
            ViewBag.ListaMedico = new SelectList(m.ListadoMedico().Where(x => x.EstMed == true).ToList(), "CodMed", "NomMed");
            ViewBag.ListaServicio = new SelectList(u.ListadoServicios().Where(x => x.EstServ == true).ToList(), "CodServ", "NomServ");

            if (Tipo != null && FechaI != null && FechaF != null)
            {
                int tipoF = (int)Tipo;
                DateTime FechaInicio = DateTime.Parse(FechaI);
                DateTime FechaFinal = DateTime.Parse(FechaF);
                if (Tipo == 1)
                {
                    ViewBag.listado = (List<E_Honorario_Cabecera_Detalle>)ListaLiquidacion(tipoF, CodMed, FechaInicio, FechaFinal, sede);
                }
                else
                {
                    ViewBag.listado = (List<E_Honorario_Cabecera_Detalle>)ListaLiquidacion(tipoF, CodServ, FechaInicio, FechaFinal, sede);
                }
                ViewBag.Tipo = Tipo;
                ViewBag.FechaInicio = FechaI;
                ViewBag.FechaFinal = FechaF;
                ViewBag.CodMed = CodMed;
                ViewBag.CodServ = CodServ;
            }
            else
            {
                ViewBag.listado = null;
                ViewBag.Tipo = 0;
                ViewBag.FechaInicio = "";
                ViewBag.FechaFinal = "";
                ViewBag.CodMed = "";
                ViewBag.CodServ = "";
            }
            return View();
        }


        public ActionResult GeneraLiquidacion(string id, int? Tipo = null, string CodMed = null, string CodServ = null, string FechaI = null, string FechaF = null)
        {

            string sede = Session["codSede"].ToString();
            string comodin = "";
            int tipoF = (int)Tipo;
            ServiciosController se = new ServiciosController();
            MedicosController m = new MedicosController();
            ViewBag.ListaMedico = new SelectList(m.ListadoMedico(), "CodMed", "NomMed", CodMed);
            ViewBag.ListaServicio = new SelectList(se.ListadoServicios(), "CodServ", "NomServ", CodServ);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "Liquidacion.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Liquidacion");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Honorario_Cabecera_Detalle> cm = new List<E_Honorario_Cabecera_Detalle>();
            CuentasController e = new CuentasController();
            DateTime FechaInicio = DateTime.Parse(FechaI);
            DateTime FechaFinal = DateTime.Parse(FechaF);
            if (Tipo == 1)
            {
                cm = ListaLiquidacion(tipoF, CodMed, FechaInicio, FechaFinal, sede);
                E_Medico med = m.ListadoMedico().Find(x => x.CodMed == CodMed);
                comodin = med.NomMed;
            }
            else
            {
                cm = ListaLiquidacion(tipoF, CodServ, FechaInicio, FechaFinal, sede);
                E_Servicios serv = se.ListadoServicios().Find(x => x.CodServ == CodServ);
                comodin = serv.NomServ;
            }

            ReportDataSource rd = new ReportDataSource("DataSet1", cm);

            lr.DataSources.Add(rd);

            UtilitarioController u = new UtilitarioController();
            SedesController sed = new SedesController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();
            E_Sede us = sed.ListadoSedes().Find(x => x.CodSede == sede);
            string nombreCompleto = us.NomSede;


            ReportParameter[] rptParameters = new ReportParameter[] {
                new ReportParameter("fechaHoy", ma.HoraServidor.ToString("dd/MM/yyyy hh:mm tt")),
                new ReportParameter("comodin", comodin),
                new ReportParameter("FechaInicio", FechaInicio.ToString("dd/MM/yyyy")),
                new ReportParameter("FechaFinal", FechaFinal.ToString("dd/MM/yyyy")),
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("Tipo", tipoF.ToString())
            };

            lr.SetParameters(rptParameters);

            string reportType = id;
            string mimeType;
            string encoding;
            string FileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "   <OutputFormat>" + id + "</OutputFormat>" +
                "   <PageWidth>8in</PageWidth>" +
                "   <PageHeight>10in</PageHeight>" +
                "   <MarginTop>0.3in</MarginTop>" +
                "   <MarginLeft>0in</MarginLeft>" +
                "   <MarginRight>0in</MarginRight>" +
                "   <MarginBottom>0.3in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] rendereBytes;

            rendereBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out FileNameExtension,
                out streams,
                out warnings);

            return File(rendereBytes, mimeType);
        }


        public ActionResult FacturacionXUsuario(string CodUsu = null, string FechaI = null, string FechaF = null, string HoraI = null, string HoraF = null)
        {
            ServiciosController u = new ServiciosController();
            MedicosController m = new MedicosController();
            UsuarioController usu = new UsuarioController();
            ViewBag.ListaMedico = new SelectList(m.ListadoMedico().Where(x => x.EstMed == true).ToList(), "CodMed", "NomMed");
            ViewBag.ListaServicio = new SelectList(u.ListadoServicios().Where(x => x.EstServ == true).ToList(), "CodServ", "NomServ");
            ViewBag.ListaUsuario = new SelectList(usu.listaUsuarios().Where(x => x.EstUsu == true).ToList(), "codUsu", "aliasUsu");

            if (CodUsu != null && FechaI != null && FechaF != null)
            {
                DateTime FechaInicio = DateTime.Parse(FechaI);
                DateTime FechaFinal = DateTime.Parse(FechaF);
                DateTime horai = DateTime.Parse(HoraI.ToString());
                string formatI = horai.ToString("H:mm:ss");
                DateTime horaF = DateTime.Parse(HoraF.ToString());
                string formatF = horaF.ToString("H:mm:ss");
                TimeSpan HoraInicio = TimeSpan.Parse(formatI);
                TimeSpan HoraFinal = TimeSpan.Parse(formatF);
                ViewBag.listado = (List<E_Caja>)ListaFacturaxUsuario(CodUsu, FechaInicio, FechaFinal, HoraInicio, HoraFinal);
                ViewBag.CodUsu = CodUsu;
                ViewBag.FechaInicio = FechaI;
                ViewBag.FechaFinal = FechaF;
                ViewBag.HoraInicio = HoraI;
                ViewBag.HoraFinal = HoraF;
            }
            else
            {
                ViewBag.listado = null;
                ViewBag.CodUsu = "";
                ViewBag.FechaInicio = "";
                ViewBag.FechaFinal = "";
                ViewBag.HoraInicio = "";
                ViewBag.HoraFinal = "";
            }
            return View();
        }

        public ActionResult GeneraFacturacionXUsuario(string id, string CodUsu = null, string FechaI = null, string FechaF = null, string HoraI = null, string HoraF = null)
        {

            UsuarioController usu = new UsuarioController();
            ViewBag.ListaUsuario = new SelectList(usu.listaUsuarios().Where(x => x.EstUsu == true).ToList(), "codUsu", "Concatena", CodUsu);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "FacturacionXUsuario.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("FacturacionXUsuario");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Caja> cm = new List<E_Caja>();
            CuentasController e = new CuentasController();
            DateTime FechaInicio = DateTime.Parse(FechaI);
            DateTime FechaFinal = DateTime.Parse(FechaF);
            DateTime horai = DateTime.Parse(HoraI.ToString());
            string formatI = horai.ToString("H:mm:ss");
            DateTime horaF = DateTime.Parse(HoraF.ToString());
            string formatF = horaF.ToString("H:mm:ss");
            TimeSpan HoraInicio = TimeSpan.Parse(formatI);
            TimeSpan HoraFinal = TimeSpan.Parse(formatF);
            cm = ListaFacturaxUsuario(CodUsu, FechaInicio, FechaFinal, HoraInicio, HoraFinal);

            ReportDataSource rd = new ReportDataSource("DataSet1", cm);

            lr.DataSources.Add(rd);

            UtilitarioController u = new UtilitarioController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();
            E_Usuario us = usu.listaUsuarios().Find(x => x.codUsu == CodUsu);
            string nombreCompleto = us.NomUsu + " " + us.ApePaterUsu + " " + us.ApeMaterUsu;

            ReportParameter[] rptParameters = new ReportParameter[] {
                new ReportParameter("fechaHoy", ma.HoraServidor.ToString("dd/MM/yyyy hh:mm tt")),
                new ReportParameter("CodUsuario", CodUsu.ToString()),
                new ReportParameter("FechaInicio", FechaInicio.ToString("dd/MM/yyyy")),
                new ReportParameter("FechaFinal", FechaFinal.ToString("dd/MM/yyyy")),
                new ReportParameter("NomUsu", nombreCompleto),
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("HoraI", HoraI.ToString()),
                new ReportParameter("HoraF", HoraF.ToString())
            };

            lr.SetParameters(rptParameters);

            string reportType = id;
            string mimeType;
            string encoding;
            string FileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "   <OutputFormat>" + id + "</OutputFormat>" +
                "   <PageWidth>8in</PageWidth>" +
                "   <PageHeight>10in</PageHeight>" +
                "   <MarginTop>0.3in</MarginTop>" +
                "   <MarginLeft>0in</MarginLeft>" +
                "   <MarginRight>0in</MarginRight>" +
                "   <MarginBottom>0.3in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] rendereBytes;

            rendereBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out FileNameExtension,
                out streams,
                out warnings);

            return File(rendereBytes, mimeType);
        }


        public ActionResult FacturacionXSede(string CodSede = null, int? CodDoc = null, string serie = null, string FechaI = null, string FechaF = null, string HoraI = null, string HoraF = null)
        {
            SedesController se = new SedesController();
            DocumentoContableController dc = new DocumentoContableController();

            ViewBag.ListaSede = new SelectList(se.ListadoSedes().Where(x => x.EstSede == true).ToList(), "CodSede", "NomSede");
            ViewBag.ListaDoc = new SelectList(dc.ListaDocumentoContable().Where(x => x.EstCodDoc == true).ToList(), "CodDocCont", "DescCodDoc");
            string sede = Session["codSede"].ToString();
            if (CodSede != null && FechaI != null && FechaF != null)
            {
                DateTime FechaInicio = DateTime.Parse(FechaI);
                DateTime FechaFinal = DateTime.Parse(FechaF);
                DateTime horai = DateTime.Parse(HoraI.ToString());
                string formatI = horai.ToString("H:mm:ss");
                DateTime horaF = DateTime.Parse(HoraF.ToString());
                string formatF = horaF.ToString("H:mm:ss");
                TimeSpan HoraInicio = TimeSpan.Parse(formatI);
                TimeSpan HoraFinal = TimeSpan.Parse(formatF);
                int CodigoDoc = (int)CodDoc;
                ViewBag.listado = (List<E_Caja>)ListaFacturaxSede(CodSede, CodigoDoc, serie, FechaInicio, FechaFinal, HoraInicio, HoraFinal);
                ViewBag.CodSede = sede;
                ViewBag.FechaInicio = FechaI;
                ViewBag.FechaFinal = FechaF;
                ViewBag.HoraInicio = HoraI;
                ViewBag.HoraFinal = HoraF;
                ViewBag.CodDoc = CodDoc;
                ViewBag.Serie = serie;
            }
            else
            {
                ViewBag.listado = null;
                ViewBag.CodSede = sede;
                ViewBag.FechaInicio = "";
                ViewBag.FechaFinal = "";
                ViewBag.HoraInicio = "";
                ViewBag.HoraFinal = "";
                ViewBag.CodDoc = 0;
                ViewBag.Serie = "";
            }
            return View();
        }

        public ActionResult ObtenerSerie(int CodDoc)
        {
            DocumentoSerieController ds = new DocumentoSerieController();
            var evalua = (List<E_DocumentoSerie>)ds.ListarDocumentoSerie().Where(x => x.CodDocCont == CodDoc).ToList();


            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult GeneraFacturacionXSede(string id, string CodSede = null, int? CodDoc = null, string serie = null, string FechaI = null, string FechaF = null, string HoraI = null, string HoraF = null)
        {
            SedesController se = new SedesController();
            ViewBag.ListaSede = new SelectList(se.ListadoSedes().Where(x => x.EstSede == true).ToList(), "CodSede", "NomSede", CodSede);

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "FacturacionXSede.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("FacturacionXUsuario");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Caja> cm = new List<E_Caja>();
            CuentasController e = new CuentasController();
            DateTime FechaInicio = DateTime.Parse(FechaI);
            DateTime FechaFinal = DateTime.Parse(FechaF);
            DateTime horai = DateTime.Parse(HoraI.ToString());
            string formatI = horai.ToString("H:mm:ss");
            DateTime horaF = DateTime.Parse(HoraF.ToString());
            string formatF = horaF.ToString("H:mm:ss");
            TimeSpan HoraInicio = TimeSpan.Parse(formatI);
            TimeSpan HoraFinal = TimeSpan.Parse(formatF);

            int CodigoDoc = (int)CodDoc;
            cm = ListaFacturaxSede(CodSede, CodigoDoc, serie, FechaInicio, FechaFinal, HoraInicio, HoraFinal);

            ReportDataSource rd = new ReportDataSource("DataSet1", cm);

            lr.DataSources.Add(rd);

            UtilitarioController u = new UtilitarioController();
            DocumentoContableController dc = new DocumentoContableController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();
            E_Sede us = se.ListadoSedes().Find(x => x.CodSede == CodSede);
            E_DocumentoContable doc = dc.ListaDocumentoContable().Find(x => x.CodDocCont == CodDoc);
            string nombreCompleto = us.NomSede;

            ReportParameter[] rptParameters = new ReportParameter[] {
                new ReportParameter("fechaHoy", ma.HoraServidor.ToString("dd/MM/yyyy hh:mm tt")),
                new ReportParameter("CodSede", CodSede.ToString()),
                new ReportParameter("FechaInicio", FechaInicio.ToString("dd/MM/yyyy")),
                new ReportParameter("FechaFinal", FechaFinal.ToString("dd/MM/yyyy")),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("HoraI", HoraI.ToString()),
                new ReportParameter("HoraF", HoraF.ToString()),
                new ReportParameter("DocCont", doc.DescCodDoc),
                new ReportParameter("Serie", serie)
            };

            lr.SetParameters(rptParameters);

            string reportType = id;
            string mimeType;
            string encoding;
            string FileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "   <OutputFormat>" + id + "</OutputFormat>" +
                "   <PageWidth>8in</PageWidth>" +
                "   <PageHeight>10in</PageHeight>" +
                "   <MarginTop>0.3in</MarginTop>" +
                "   <MarginLeft>0in</MarginLeft>" +
                "   <MarginRight>0in</MarginRight>" +
                "   <MarginBottom>0.3in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] rendereBytes;

            rendereBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out FileNameExtension,
                out streams,
                out warnings);

            return File(rendereBytes, mimeType);
        }



        public ActionResult ReporteCatidadxVenta()
        {
            ViewBag.lista = (List<E_Reporte>)ListaDeCantidadxVentas().ToList();
            return View();
        }

        public ActionResult ReporteRecurrenteAtendidos(string FechaI = null, string FechaF = null, string CIe10 = null, string CodServ = null)
        {
            var sede = Session["codSede"].ToString();

            FichaElectronicaController ficha = new FichaElectronicaController();
            ServiciosController servicio = new ServiciosController();
            ViewBag.FechaI = FechaI; ViewBag.fechaF = FechaF; ViewBag.cie10 = CIe10; ViewBag.codser = CodServ;
            ViewBag.diagnostico = new SelectList(ficha.listCie10(), "CIe10", "Descripcion");
            ViewBag.servicio = new SelectList(servicio.ListadoServicios().Where(x => x.CodSede == sede).ToList(), "CodServ", "NomServ");
            var servicio1 = servicio.ListadoServicios().Where(x => x.CodSede == sede).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(FechaI) && !string.IsNullOrWhiteSpace(FechaF) && !string.IsNullOrWhiteSpace(CIe10) && !string.IsNullOrWhiteSpace(CodServ))
            {
                ViewBag.fechaI = FechaI; ViewBag.fechaF = FechaF;
                ViewBag.vista = Usp_Reporte_Recurrentes_Atendidos(FechaI, FechaF, CIe10, servicio1.CodSede, CodServ).ToList();
            }
            else { ViewBag.vista = null; }
            return View();
        }

        public List<E_Ficha_Electronica> Usp_Reporte_Recurrentes_Atendidos(string FechaI = null, string FechaF = null, string CIe10 = null, string sede = null, string CodServ = null)
        {

            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Reporte_Recurrentes_Atendidos", con))
                {
                    cmd.Parameters.AddWithValue("@fechaI", FechaI);
                    cmd.Parameters.AddWithValue("@fechaF", FechaF);
                    cmd.Parameters.AddWithValue("@codser", CodServ);
                    cmd.Parameters.AddWithValue("@sede", sede);
                    cmd.Parameters.AddWithValue("@cie", CIe10);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Ficha_Electronica ca = new E_Ficha_Electronica();


                            ca.cant = dr.GetInt32(0);
                            ca.CIe10 = dr.GetString(1);
                            ca.NomServ = dr.GetString(2);

                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public ActionResult GeneraRecurrentesAtendidos(string id, string FechaI = null, string FechaF = null, string CIe10 = null, string CodServ = null)
        {
            ServiciosController servicio = new ServiciosController();
            var sede = Session["codSede"].ToString();

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReporteRecurrenteAtendidos.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ReporteRecurrenteAtendidos");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Ficha_Electronica> cm = new List<E_Ficha_Electronica>();

            DateTime FechaInicio = DateTime.Parse(FechaI);
            DateTime FechaFinal = DateTime.Parse(FechaF);

            var servicio1 = servicio.ListadoServicios().Where(x => x.CodSede == sede).FirstOrDefault();
            cm = Usp_Reporte_Recurrentes_Atendidos(FechaI, FechaF, CIe10, servicio1.CodSede, CodServ);

            ReportDataSource rd = new ReportDataSource("DataSet1", cm);

            lr.DataSources.Add(rd);
            SedesController se = new SedesController();
            UtilitarioController u = new UtilitarioController();
            DocumentoContableController dc = new DocumentoContableController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();
            E_Sede us = se.ListadoSedes().Find(x => x.CodSede == sede);

            string nombreCompleto = us.NomSede;

            ReportParameter[] rptParameters = new ReportParameter[] {
                new ReportParameter("fechaHoy", ma.HoraServidor.ToString("dd/MM/yyyy hh:mm tt")),
                new ReportParameter("CodSede", sede.ToString()),
                new ReportParameter("FechaInicio", FechaInicio.ToString("dd/MM/yyyy").Substring(0,10)),
                new ReportParameter("FechaFinal",FechaFinal.ToString("dd/MM/yyyy").Substring(0, 10)),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("UsuGene", usuarioGenera)

            };

            lr.SetParameters(rptParameters);

            string reportType = id;
            string mimeType;
            string encoding;
            string FileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "   <OutputFormat>" + id + "</OutputFormat>" +
                "   <PageWidth>8in</PageWidth>" +
                "   <PageHeight>10in</PageHeight>" +
                "   <MarginTop>0.3in</MarginTop>" +
                "   <MarginLeft>0in</MarginLeft>" +
                "   <MarginRight>0in</MarginRight>" +
                "   <MarginBottom>0.3in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] rendereBytes;

            rendereBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out FileNameExtension,
                out streams,
                out warnings);

            return File(rendereBytes, mimeType);
        }
        public List<E_Reporte> Usp_Reporte_Nuevos_Continuadores(string FechaI = null, string FechaF = null)
        {

            List<E_Reporte> Lista = new List<E_Reporte>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Reporte_Nuevos_Continuadores", con))
                {
                    cmd.Parameters.AddWithValue("@fechaI", FechaI);
                    cmd.Parameters.AddWithValue("@fechaF", FechaF);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Reporte ca = new E_Reporte();
                            ca.fecha = dr.GetDateTime(0).ToString("dd/MM/yyyy");
                            ca.PacAtendidos = dr.GetInt32(1);
                            ca.TotAtendido = dr.GetInt32(2);
                            ca.totalpac = dr.GetInt32(3);
                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Reporte> usp_ProMasVendido(string FechaI, string FechaF, string servicio, string tipoCon)
        {

            List<E_Reporte> Lista = new List<E_Reporte>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ProMasVendido", con))
                {
                    cmd.Parameters.AddWithValue("@fechaI", FechaI);
                    cmd.Parameters.AddWithValue("@fechaF", FechaF);
                    cmd.Parameters.AddWithValue("@servicio", servicio);
                    cmd.Parameters.AddWithValue("@tipoCon", tipoCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Reporte ca = new E_Reporte();
                            ca.DescTar = dr.GetString(0);
                            ca.NomServ = dr.GetString(1);
                            ca.cantidad = dr.GetInt32(2);
                            ca.total = dr.GetDecimal(3);

                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Reporte> uspFacturaMedEnvia(string FechaI, string FechaF, string servicio, string medico, string producto, string sede)

        {

            List<E_Reporte> Lista = new List<E_Reporte>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("uspFacturaMedEnvia", con))
                {
                    cmd.Parameters.AddWithValue("@fechaI", FechaI);
                    cmd.Parameters.AddWithValue("@fechaF", FechaF);
                    cmd.Parameters.AddWithValue("@servicio", servicio);
                    cmd.Parameters.AddWithValue("@medico", medico);
                    cmd.Parameters.AddWithValue("@producto", producto);
                    cmd.Parameters.AddWithValue("@sede", sede);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Reporte ca = new E_Reporte();
                            ca.NomServ = dr.GetString(0);
                            ca.NomMed = dr.GetString(1);
                            ca.DescTar = dr.GetString(2);
                            ca.cantidad = dr.GetInt32(3);
                            ca.total = dr.GetDecimal(4);

                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Reporte> uspFactServicioTipo(string FechaI, string FechaF, string servicio, string sede)
        {

            List<E_Reporte> Lista = new List<E_Reporte>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("uspFactServicioTipo", con))
                {
                    cmd.Parameters.AddWithValue("@fechaI", FechaI);
                    cmd.Parameters.AddWithValue("@fechaF", FechaF);
                    cmd.Parameters.AddWithValue("@servicio", servicio);
                    cmd.Parameters.AddWithValue("@sede", sede);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Reporte ca = new E_Reporte();
                            ca.NomServ = dr.GetString(0);
                            ca.fecha = dr.GetString(1);
                            ca.tipoTar = dr.GetString(2);
                            ca.turno = dr.GetString(3);
                            ca.cantidad = dr.GetInt32(4);
                            ca.total = dr.GetDecimal(5);

                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Reporte> usp_MedicoProduccion(string FechaI, string FechaF, string servicio, string medico, string sede)
        {

            List<E_Reporte> Lista = new List<E_Reporte>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_MedicoProduccion", con))
                {
                    cmd.Parameters.AddWithValue("@fechaI", FechaI);
                    cmd.Parameters.AddWithValue("@fechaF", FechaF);
                    cmd.Parameters.AddWithValue("@servicio", servicio);
                    cmd.Parameters.AddWithValue("@medico", medico);
                    cmd.Parameters.AddWithValue("@sede", sede);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Reporte ca = new E_Reporte();
                            ca.NomServ = dr.GetString(0);
                            ca.NomMed = dr.GetString(1);
                            ca.DescTipTar = dr.GetString(2);
                            ca.cantidad = dr.GetInt32(3);
                            ca.total = dr.GetDecimal(4);

                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public ActionResult ReporteProductoMasVendido(string FechaI = null, string FechaF = null, string[] servicio = null, string tipoCon = null)
        {

            UtilitarioController u = new UtilitarioController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();

            string sede = Session["codSede"].ToString();
            ServiciosController s = new ServiciosController();
            ViewBag.ListaServicio = new SelectList(s.ListadoServicios().Where(x => x.EstServ == true && x.CodSede == sede).ToList(), "CodServ", "NomServ");
            string servicioV = "";
            if (FechaI != null && FechaF != null && servicio != null && tipoCon != null)
            {
                string Servicio = "";
                int cuenta = 1;
                foreach (string item in servicio)
                {
                    if (item == "")
                    {
                        Servicio = "T";

                    }
                    else {
                        Servicio = $"{item},";
                    }
                    if (cuenta == 1)
                    {
                       
                        if (item == "") { servicioV += "[TODOS]"; } else { var ser = (new ServiciosController()).ListadoServicios().Where(x => x.CodServ == item.ToString()).FirstOrDefault(); servicioV += $" - {ser.NomServ.ToUpper()}"; }
                    }
                    else
                    {
                        Servicio = Servicio + "," + item;
                        if (item == "T") { servicioV += "[TODOS]"; } else { var ser = (new ServiciosController()).ListadoServicios().Where(x => x.CodServ == item.ToString()).FirstOrDefault(); servicioV += $" - {ser.NomServ.ToUpper()}"; }
                    }

                    cuenta++;
                }
                ViewBag.servicioV = servicioV;
                ViewBag.FechaI = FechaI;
                ViewBag.FechaF = FechaF;
                ViewBag.servicio = Servicio;
                ViewBag.tipoCon = tipoCon;
                return View(usp_ProMasVendido(FechaI, FechaF, Servicio, tipoCon).ToList());
            }
            else
            {
                ViewBag.FechaI = ma.HoraServidor.ToShortDateString();
                ViewBag.FechaF = ma.HoraServidor.ToShortDateString();
                return View();
            }
        }

        public ActionResult GeneraProductoMasVendido(string id, string FechaI, string FechaF, string servicio, string tipoCon)
        {

            string sede = Session["codSede"].ToString();

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReporteProductoMasVendido.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ReporteServicioTipoTar");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Reporte> cm = new List<E_Reporte>();

            cm = usp_ProMasVendido(FechaI, FechaF, servicio, tipoCon);


            ReportDataSource rd = new ReportDataSource("DataSet1", cm);

            lr.DataSources.Add(rd);

            UtilitarioController u = new UtilitarioController();
            SedesController sed = new SedesController();
            ServiciosController ser = new ServiciosController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();
            E_Sede us = sed.ListadoSedes().Find(x => x.CodSede == sede);
            string nombreCompleto = us.NomSede;

            string Servicio = "";
            int cuenta = 1;
            string[] fija = servicio.Split(',');
            foreach (string item in fija)
            {
                if (item == "T")
                {
                    Servicio = "TODOS";
                    break;
                }
                if (cuenta == 1)
                {
                    E_Servicios s = ser.ListadoServicios().Find(x => x.CodServ == item);
                    Servicio = s.NomServ;
                }
                else
                {
                    E_Servicios s = ser.ListadoServicios().Find(x => x.CodServ == item);
                    Servicio = Servicio + ", " + s.NomServ;
                }

                cuenta++;

            }



            ReportParameter[] rptParameters = new ReportParameter[] {
                new ReportParameter("fechaHoy", ma.HoraServidor.ToString("dd/MM/yyyy hh:mm tt")),
                new ReportParameter("FechaInicio", FechaI),
                new ReportParameter("FechaFinal", FechaF),
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("Servicio", Servicio)
            };

            lr.SetParameters(rptParameters);

            string reportType = id;
            string mimeType;
            string encoding;
            string FileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "   <OutputFormat>" + id + "</OutputFormat>" +
                "   <PageWidth>8in</PageWidth>" +
                "   <PageHeight>10in</PageHeight>" +
                "   <MarginTop>0.3in</MarginTop>" +
                "   <MarginLeft>0in</MarginLeft>" +
                "   <MarginRight>0in</MarginRight>" +
                "   <MarginBottom>0.3in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] rendereBytes;

            rendereBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out FileNameExtension,
                out streams,
                out warnings);

            return File(rendereBytes, mimeType);
        }



        public ActionResult ReporteServicioTipoTar(string FechaI = null, string FechaF = null, string[] servicio = null)
        {

            UtilitarioController u = new UtilitarioController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();

            string sede = Session["codSede"].ToString();
            ServiciosController s = new ServiciosController();
            ViewBag.ListaServicio = new SelectList(s.ListadoServicios().Where(x => x.EstServ == true && x.CodSede == sede).ToList(), "CodServ", "NomServ");

            if (FechaI != null && FechaF != null && servicio != null)
            {
                string Servicio = "";
                int cuenta = 1;
                string servicioV = ""; 
                foreach (string item in servicio)
                {
                    if (item == "")
                    {
                        Servicio = "T";

                    }
                    else {
                        Servicio += $"{item},";
                    }
                    if (cuenta == 1)
                    {
                        
                        if (item == "") { servicioV += "[TODOS]"; } else { var ser = (new ServiciosController()).ListadoServicios().Where(x => x.CodServ == item.ToString()).FirstOrDefault(); servicioV += $" - {ser.NomServ.ToUpper()}"; }
                    }
                    else
                    {
                        Servicio = Servicio + "," + item;
                        if (item == "T") { servicioV += "[TODOS]"; } else { var ser = (new ServiciosController()).ListadoServicios().Where(x => x.CodServ == item.ToString()).FirstOrDefault(); servicioV += $" - {ser.NomServ.ToUpper()}"; }
                    }

                    cuenta++;
                }

                ViewBag.FechaI = FechaI;
                ViewBag.FechaF = FechaF;
                ViewBag.servicioV = servicioV;
                ViewBag.servicio = Servicio;
                return View(uspFactServicioTipo(FechaI, FechaF, Servicio, sede).ToList());
            }
            else
            {
                ViewBag.FechaI = ma.HoraServidor.ToShortDateString();
                ViewBag.FechaF = ma.HoraServidor.ToShortDateString();
                return View();
            }
        }


        public ActionResult GeneraServicioTipoTar(string id, string FechaI, string FechaF, string servicio)
        {

            string sede = Session["codSede"].ToString();

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReporteServicioTipoTar.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ReporteServicioTipoTar");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Reporte> cm = new List<E_Reporte>();

            cm = uspFactServicioTipo(FechaI, FechaF, servicio, sede);


            ReportDataSource rd = new ReportDataSource("DataSet1", cm);

            lr.DataSources.Add(rd);

            UtilitarioController u = new UtilitarioController();
            SedesController sed = new SedesController();
            ServiciosController ser = new ServiciosController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();
            E_Sede us = sed.ListadoSedes().Find(x => x.CodSede == sede);
            string nombreCompleto = us.NomSede;

            string Servicio = "";
            int cuenta = 1;
            string[] fija = servicio.Split(',');
            foreach (string item in fija)
            {
                if (item == "T")
                {
                    Servicio = "TODOS";
                    break;
                }
                if (cuenta == 1)
                {
                    E_Servicios s = ser.ListadoServicios().Find(x => x.CodServ == item);
                    Servicio = s.NomServ;
                }
                else
                {
                    E_Servicios s = ser.ListadoServicios().Find(x => x.CodServ == item);
                    Servicio = Servicio + ", " + s.NomServ;
                }

                cuenta++;

            }



            ReportParameter[] rptParameters = new ReportParameter[] {
                new ReportParameter("fechaHoy", ma.HoraServidor.ToString("dd/MM/yyyy hh:mm tt")),
                new ReportParameter("FechaInicio", FechaI),
                new ReportParameter("FechaFinal", FechaF),
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("Servicio", Servicio)
            };

            lr.SetParameters(rptParameters);

            string reportType = id;
            string mimeType;
            string encoding;
            string FileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "   <OutputFormat>" + id + "</OutputFormat>" +
                "   <PageWidth>8in</PageWidth>" +
                "   <PageHeight>10in</PageHeight>" +
                "   <MarginTop>0.3in</MarginTop>" +
                "   <MarginLeft>0in</MarginLeft>" +
                "   <MarginRight>0in</MarginRight>" +
                "   <MarginBottom>0.3in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] rendereBytes;

            rendereBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out FileNameExtension,
                out streams,
                out warnings);

            return File(rendereBytes, mimeType);
        }



        public ActionResult ReporteMedicosEnvia(string FechaI = null, string FechaF = null, string[] servicio = null, string[] medico = null, string[] producto = null)
        {

            UtilitarioController u = new UtilitarioController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();

            string sede = Session["codSede"].ToString();
            ServiciosController s = new ServiciosController();
            ViewBag.ListaServicio = new SelectList(s.ListadoServicios().Where(x => x.EstServ == true && x.CodSede == sede).ToList(), "CodServ", "NomServ");

            if (FechaI != null && FechaF != null && servicio != null && medico != null && producto != null)
            {
                string Servicio = "";
                int cuenta = 1; string servicioV = ""; string medicoV = ""; string productoV = "";
                foreach (string item in servicio)
                {


                    if (item == "")
                    {
                        Servicio = "T";

                    }
                    else {
                        Servicio += $"{item},"; 
                    }
                    if (cuenta == 1)
                    {
                   
                        if (item == "") { servicioV += "[TODOS]"; } else { var ser = s.ListadoServicios().Where(x => x.CodServ == item.ToString()).FirstOrDefault(); servicioV += ser.NomServ; }
                    }
                    else
                    {
                        Servicio = Servicio + "," + item;
                        if (item == "T") { servicioV += "[TODOS]"; } else { var ser = s.ListadoServicios().Where(x => x.CodServ == item.ToString()).FirstOrDefault(); servicioV += $" - {ser.NomServ}"; }
                    }

                    cuenta++;
                }

                string Medico = "";
                cuenta = 1;
                foreach (string item in medico)
                {

                    if (item == "")
                    {
                        Medico = "T";
                        break;
                    }

                    if (cuenta == 1)
                    {
                        Medico = item;
                        if (item == "T") { medicoV += "[TODOS]"; } else { var med = (new MedicosController()).ListadoMedico().Where(x => x.CodMed == item.ToString()).FirstOrDefault(); medicoV += med.NomMed; }
                    }
                    else
                    {
                        Medico = Medico + "," + item;
                        if (item == "T") { medicoV += "[TODOS]"; } else { var med = (new MedicosController()).ListadoMedico().Where(x => x.CodMed == item.ToString()).FirstOrDefault(); medicoV += $" - {med.NomMed}"; }
                    }

                    cuenta++;
                }

                string Producto = "";
                cuenta = 1;
                foreach (string item in producto)
                {

                    if (item == "")
                    {
                        Producto = "T";
                        break;
                    }
                    if (cuenta == 1)
                    {
                        Producto = item;
                        if (item == "T") { productoV += "[TODOS]"; } else { var prod = (new TarifarioController()).ListadoTarifa().Where(x => x.CodTar == item.ToString()).FirstOrDefault(); productoV += prod.DescTar; }
                    }
                    else
                    {
                        Producto = Producto + "," + item;
                        if (item == "T") { productoV += "[TODOS]"; } else { var prod = (new TarifarioController()).ListadoTarifa().Where(x => x.CodTar == item.ToString()).FirstOrDefault(); productoV += $" - {prod.DescTar}"; }
                    }

                    cuenta++;
                }


                ViewBag.servicioV = servicioV;
                ViewBag.medicoV = medicoV;
                ViewBag.productoV = productoV;
                ViewBag.FechaI = FechaI;
                ViewBag.FechaF = FechaF;
                ViewBag.servicio = Servicio;
                ViewBag.medico = Medico;
                ViewBag.producto = Producto;
                return View(uspFacturaMedEnvia(FechaI, FechaF, Servicio, Medico, Producto, sede).ToList());
            }
            else
            {
                ViewBag.FechaI = ma.HoraServidor.ToShortDateString();
                ViewBag.FechaF = ma.HoraServidor.ToShortDateString();
                return View();
            }
        }


        public ActionResult GeneraMedicosEnvia(string id, string FechaI, string FechaF, string servicio, string medico, string producto)
        {

            string sede = Session["codSede"].ToString();

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReporteMedicosEnvia.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ReporteMedicosEnvia");
            }



            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Reporte> cm = new List<E_Reporte>();

            cm = uspFacturaMedEnvia(FechaI, FechaF, servicio, medico, producto, sede);


            ReportDataSource rd = new ReportDataSource("DataSet1", cm);

            lr.DataSources.Add(rd);

            UtilitarioController u = new UtilitarioController();
            SedesController sed = new SedesController();
            ServiciosController ser = new ServiciosController();
            MedicosController me = new MedicosController();
            TarifarioController ta = new TarifarioController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();
            E_Sede us = sed.ListadoSedes().Find(x => x.CodSede == sede);
            string nombreCompleto = us.NomSede;

            string Servicio = "";
            int cuenta = 1;
            string[] fija = servicio.Split(',');
            foreach (string item in fija)
            {
                if (item == "T")
                {
                    Servicio = "TODOS";
                    break;
                }
                if (cuenta == 1)
                {
                    E_Servicios s = ser.ListadoServicios().Find(x => x.CodServ == item);
                    Servicio = s.NomServ;
                }
                else
                {
                    E_Servicios s = ser.ListadoServicios().Find(x => x.CodServ == item);
                    Servicio = Servicio + ", " + s.NomServ;
                }

                cuenta++;

            }

            string Medico = "";
            cuenta = 1;
            fija = medico.Split(',');
            foreach (string item in fija)
            {
                if (item == "T")
                {
                    Medico = "TODOS";
                    break;
                }
                if (cuenta == 1)
                {
                    E_Medico s = me.ListadoMedico().Find(x => x.CodMed == item);
                    Medico = s.NomMed;
                }
                else
                {
                    E_Medico s = me.ListadoMedico().Find(x => x.CodMed == item);
                    Medico = Medico + ", " + s.NomMed;
                }

                cuenta++;
            }

            string Tarifa = "";
            cuenta = 1;
            fija = producto.Split(',');
            foreach (string item in fija)
            {
                if (item == "T")
                {
                    Tarifa = "TODOS";
                    break;
                }
                if (cuenta == 1)
                {
                    E_Tarifario s = ta.ListadoTarifa().Find(x => x.CodTar == item);
                    Tarifa = s.DescTar;
                }
                else
                {
                    E_Tarifario s = ta.ListadoTarifa().Find(x => x.CodTar == item);
                    Tarifa = Tarifa + ", " + s.DescTar;
                }

                cuenta++;
            }



            ReportParameter[] rptParameters = new ReportParameter[] {
                new ReportParameter("fechaHoy", ma.HoraServidor.ToString("dd/MM/yyyy hh:mm tt")),
                new ReportParameter("FechaInicio", FechaI),
                new ReportParameter("FechaFinal", FechaF),
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("Servicio", Servicio),
                new ReportParameter("Medico", Medico),
                new ReportParameter("Tarifa", Tarifa)
            };

            lr.SetParameters(rptParameters);

            string reportType = id;
            string mimeType;
            string encoding;
            string FileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "   <OutputFormat>" + id + "</OutputFormat>" +
                "   <PageWidth>8in</PageWidth>" +
                "   <PageHeight>10in</PageHeight>" +
                "   <MarginTop>0.3in</MarginTop>" +
                "   <MarginLeft>0in</MarginLeft>" +
                "   <MarginRight>0in</MarginRight>" +
                "   <MarginBottom>0.3in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] rendereBytes;

            rendereBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out FileNameExtension,
                out streams,
                out warnings);

            return File(rendereBytes, mimeType);
        }


        public ActionResult ReporteProduccionMedico(string FechaI = null, string FechaF = null, string[] servicio = null, string[] medico = null)
        {

            UtilitarioController u = new UtilitarioController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();

            string sede = Session["codSede"].ToString();
            ServiciosController s = new ServiciosController();
            ViewBag.ListaServicio = new SelectList(s.ListadoServicios().Where(x => x.EstServ == true && x.CodSede == sede).ToList(), "CodServ", "NomServ");

            if (FechaI != null && FechaF != null && servicio != null && medico != null)
            {
                string Servicio = "";
                int cuenta = 1; string servicioV = ""; string medicoV = "";
                foreach (string item in servicio)
                {
                    if (item == "")
                    {
                        Servicio = "T";

                    }
                    else {
                        Servicio += $"{item},";
                    }
                    if (cuenta == 1)
                    {
                       
                        if (item == "") { servicioV += "[TODOS]"; } else { var ser = (new ServiciosController()).ListadoServicios().Where(x => x.CodServ == item.ToString()).FirstOrDefault(); servicioV += ser.NomServ.ToString(); }
                    }
                    else
                    {
                        Servicio = Servicio + "," + item;
                        if (item == "T") { servicioV += "[TODOS]"; } else { var ser = (new ServiciosController()).ListadoServicios().Where(x => x.CodServ == item.ToString()).FirstOrDefault(); servicioV += $" - {ser.NomServ.ToString()}"; }
                    }

                    cuenta++;
                }

                string Medico = "";
                cuenta = 1;
                foreach (string item in medico)
                {
                    if (item == "")
                    {
                        Medico = "T";
                        servicioV += "[TODOS]";
                        break;
                    }
                    else {
                        Medico += $"{item},";
                    }
                    if (cuenta == 1)
                    {
                      
                        if (item == "T") { medicoV += "[TODOS]"; } else { var med = (new MedicosController()).ListadoMedico().Where(x => x.CodMed == item.ToString()).FirstOrDefault(); medicoV += med.NomMed.ToString(); }
                    }
                    else
                    {
                        Medico = Medico + "," + item;
                        if (item == "T") { medicoV += "[TODOS]"; } else { var med = (new MedicosController()).ListadoMedico().Where(x => x.CodMed == item.ToString()).FirstOrDefault(); medicoV += $" - {med.NomMed.ToString()}"; }
                    }

                    cuenta++;
                }

                ViewBag.medicoV = medicoV;
                ViewBag.servicioV = servicioV;
                ViewBag.FechaI = FechaI;
                ViewBag.FechaF = FechaF;
                ViewBag.servicio = Servicio;
                ViewBag.medico = Medico;
                return View(usp_MedicoProduccion(FechaI, FechaF, Servicio, Medico, sede).ToList());
            }
            else
            {
                ViewBag.FechaI = ma.HoraServidor.ToShortDateString();
                ViewBag.FechaF = ma.HoraServidor.ToShortDateString();
                return View();
            }
        }

        public ActionResult GenerarProduccionMedico(string id, string FechaI, string FechaF, string servicio, string medico)
        {

            string sede = Session["codSede"].ToString();

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReporteProduccionMedico.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ReporteProduccionMedico");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Reporte> cm = new List<E_Reporte>();

            cm = usp_MedicoProduccion(FechaI, FechaF, servicio, medico, sede);


            ReportDataSource rd = new ReportDataSource("DataSet1", cm);

            lr.DataSources.Add(rd);

            UtilitarioController u = new UtilitarioController();
            SedesController sed = new SedesController();
            ServiciosController ser = new ServiciosController();
            MedicosController me = new MedicosController();
            TarifarioController ta = new TarifarioController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();
            E_Sede us = sed.ListadoSedes().Find(x => x.CodSede == sede);
            string nombreCompleto = us.NomSede;

            string Servicio = "";
            int cuenta = 1;
            string[] fija = servicio.Split(',');
            foreach (string item in fija)
            {
                if (item == "T")
                {
                    Servicio = "TODOS";
                    break;
                }
                if (cuenta == 1)
                {
                    E_Servicios s = ser.ListadoServicios().Find(x => x.CodServ == item);
                    Servicio = s.NomServ;
                }
                else
                {
                    E_Servicios s = ser.ListadoServicios().Find(x => x.CodServ == item);
                    Servicio = Servicio + ", " + s.NomServ;
                }

                cuenta++;

            }

            string Medico = "";
            cuenta = 1;
            fija = medico.Split(',');
            foreach (string item in fija)
            {
                if (item == "T")
                {
                    Medico = "TODOS";
                    break;
                }
                if (cuenta == 1)
                {
                    E_Medico s = me.ListadoMedico().Find(x => x.CodMed == item);
                    Medico = s.NomMed;
                }
                else
                {
                    E_Medico s = me.ListadoMedico().Find(x => x.CodMed == item);
                    Medico = Medico + ", " + s.NomMed;
                }

                cuenta++;
            }




            ReportParameter[] rptParameters = new ReportParameter[] {
                new ReportParameter("fechaHoy", ma.HoraServidor.ToString("dd/MM/yyyy hh:mm tt")),
                new ReportParameter("FechaInicio", FechaI),
                new ReportParameter("FechaFinal", FechaF),
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("Servicio", Servicio),
                new ReportParameter("Medico", Medico)
            };

            lr.SetParameters(rptParameters);

            string reportType = id;
            string mimeType;
            string encoding;
            string FileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "   <OutputFormat>" + id + "</OutputFormat>" +
                "   <PageWidth>8in</PageWidth>" +
                "   <PageHeight>10in</PageHeight>" +
                "   <MarginTop>0.3in</MarginTop>" +
                "   <MarginLeft>0in</MarginLeft>" +
                "   <MarginRight>0in</MarginRight>" +
                "   <MarginBottom>0.3in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] rendereBytes;

            rendereBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out FileNameExtension,
                out streams,
                out warnings);

            return File(rendereBytes, mimeType);
        }


        public ActionResult ObtenerMedico(string CodServ)
        {
            string sede = Session["codSede"].ToString();
            MedicosController m = new MedicosController();
            if (CodServ == "")
            {
                var result = (List<E_Medico>)m.ListadoMedico().Where(x => x.CodSede == sede && x.EnLista == true).ToList();
                if (result.Count() != 0)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                string[] fija = CodServ.Split(',');

                string[] inq = new string[fija.Count()];
                for (int runs = 0; runs < fija.Count(); runs++)
                {
                    inq[runs] = fija[runs];
                }

                var result = (from Q in m.ListadoMedico()
                              where inq.Contains(Q.CodServ) && Q.EnLista == true
                              select Q).ToList();

                if (result.Count() != 0)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }


        public ActionResult ObtenerTarifa(string CodServ)
        {
            string sede = Session["codSede"].ToString();
            if (CodServ == "")
            {
                var result = (List<E_Reporte>)usp_ListaTarifaMedicoEnvia().Where(x => x.CodSede == sede).ToList();
                if (result.Count() != 0)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                string[] fija = CodServ.Split(',');

                string[] inq = new string[fija.Count()];
                for (int runs = 0; runs < fija.Count(); runs++)
                {
                    inq[runs] = fija[runs];
                }

                var result = (from Q in usp_ListaTarifaMedicoEnvia()
                              where inq.Contains(Q.CodServ)
                              select Q).ToList();

                if (result.Count() != 0)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }

        public ActionResult ReporteNuevosContinuadores(string FechaI = null, string FechaF = null)
        {

            if (!string.IsNullOrWhiteSpace(FechaI) && !string.IsNullOrWhiteSpace(FechaF))
            {
                ViewBag.fechaI = FechaI;
                ViewBag.fechaF = FechaF;
                ViewBag.lista = Usp_Reporte_Nuevos_Continuadores(FechaI, FechaF);
            }

            return View();
        }

        public ActionResult GeneraReporteNuevosContinuadores(string id, string FechaI = null, string FechaF = null)
        {
            ServiciosController servicio = new ServiciosController();
            var sede = Session["codSede"].ToString();

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReporteNuevoContinuadores.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ReporteNuevosContinuadores");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Reporte> cm = new List<E_Reporte>();

            DateTime FechaInicio = DateTime.Parse(FechaI);
            DateTime FechaFinal = DateTime.Parse(FechaF);

            var servicio1 = servicio.ListadoServicios().Where(x => x.CodSede == sede).FirstOrDefault();
            cm = Usp_Reporte_Nuevos_Continuadores(FechaI, FechaF);

            ReportDataSource rd = new ReportDataSource("DataSet1", cm);

            lr.DataSources.Add(rd);
            SedesController se = new SedesController();
            UtilitarioController u = new UtilitarioController();
            DocumentoContableController dc = new DocumentoContableController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();
            E_Sede us = se.ListadoSedes().Find(x => x.CodSede == sede);

            string nombreCompleto = us.NomSede;

            ReportParameter[] rptParameters = new ReportParameter[] {
                new ReportParameter("fechaHoy", ma.HoraServidor.ToString("dd/MM/yyyy hh:mm tt")),
                new ReportParameter("CodSede", sede.ToString()),
                new ReportParameter("FechaInicio", FechaInicio.ToString("dd/MM/yyyy").Substring(0,10)),
                new ReportParameter("FechaFinal",FechaFinal.ToString("dd/MM/yyyy").Substring(0, 10)),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("UsuGene", usuarioGenera)

            };

            lr.SetParameters(rptParameters);

            string reportType = id;
            string mimeType;
            string encoding;
            string FileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "   <OutputFormat>" + id + "</OutputFormat>" +
                "   <PageWidth>8in</PageWidth>" +
                "   <PageHeight>10in</PageHeight>" +
                "   <MarginTop>0.3in</MarginTop>" +
                "   <MarginLeft>0in</MarginLeft>" +
                "   <MarginRight>0in</MarginRight>" +
                "   <MarginBottom>0.3in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] rendereBytes;

            rendereBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out FileNameExtension,
                out streams,
                out warnings);

            return File(rendereBytes, mimeType);
        }

        public List<E_Receta> Usp_Reporte_Receta(int procedencia, int fe)
        {
            List<E_Receta> Lista = new List<E_Receta>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Reporte_Receta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@procedencia", procedencia);
                    cmd.Parameters.AddWithValue("@Fe", fe);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            E_Receta ca = new E_Receta();
                            ca.Item = dr.GetInt32(0);
                            ca.Descripcion = dr.GetString(1);
                            ca.Concentra = dr.GetString(2);
                            ca.FormaFarmec = dr.GetString(3);
                            ca.Dosis = dr.GetString(4);
                            ca.ViaAdmin = dr.GetString(5);
                            ca.Duracion = dr.GetString(6);
                            ca.Frecuencia = dr.GetString(7);
                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Reporte> Usp_Reporte_Atenciones(int Historia)
        {
            List<E_Reporte> Lista = new List<E_Reporte>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Reporte_Atenciones", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codpac", Historia);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Reporte ca = new E_Reporte();
                            ca.NombrePac = dr.GetString(0);
                            ca.Especialidad = dr.GetString(1).ToUpper();
                            ca.Servicio = dr.GetString(2).ToString();
                            ca.Medico = dr.GetString(3);
                            ca.Tarifa = dr.GetString(4);
                            ca.cantidad = dr.GetInt32(5);
                            ca.total = dr.GetDecimal(6);
                            ca.FechaAtenReg = dr.GetString(7);
                            ca.CodCue = dr.GetInt32(8);
                            ca.Item = dr.GetInt32(9);
                            ca.ItemCue = dr.GetString(10);
                            ca.FechaPago = dr.GetDateTime(11).ToShortDateString();
                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult GeneraReporte_Atenciones(string id, int Historia)
        {
            ServiciosController servicio = new ServiciosController();
            var sede = Session["codSede"].ToString();

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReporteAtenciones.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("../FichaElectronica/BandejaDeAtenciones");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Reporte> cm = Usp_Reporte_Atenciones(Historia);

            ReportDataSource rd = new ReportDataSource("DataSet1", cm);

            lr.DataSources.Add(rd);
            SedesController se = new SedesController();
            UtilitarioController u = new UtilitarioController();
            DocumentoContableController dc = new DocumentoContableController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();
            E_Sede us = se.ListadoSedes().Find(x => x.CodSede == sede);

            string nombreCompleto = us.NomSede;
            var paciente = new PacientesController().ListadoPacientes().Where(x => x.Historia == Historia).FirstOrDefault();
            var DocumentoIdentidad = new UtilitarioController().ListadoDocumentoIdentidad().Where(x => x.CodDocIdent == paciente.CodDocIdent).FirstOrDefault();
            var categoriapac = new CategoriaPacienteController().listadoCategoriaCliente().Where(x => x.CodCatPac == paciente.CodCatPac).FirstOrDefault();
            ReportParameter[] rptParameters = new ReportParameter[] {
                new ReportParameter("fechaHoy", ma.HoraServidor.ToString("dd/MM/yyyy hh:mm tt")),
                new ReportParameter("CodSede", sede.ToString()),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("Documento", DocumentoIdentidad.NomDocIdent.ToUpper()),
                new ReportParameter("NroDoc",paciente.NumDoc),
                new ReportParameter("NombrePac",paciente.ApePat + " " + paciente.ApeMat + " " +paciente.NomPac),
                new ReportParameter("CategoriaPac",categoriapac.DescCatPac),
                new ReportParameter("FechaNac",paciente.FecNac.ToShortDateString())

            };

            lr.SetParameters(rptParameters);

            string reportType = id;
            string mimeType;
            string encoding;
            string FileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "   <OutputFormat>" + id + "</OutputFormat>" +
                "   <PageWidth>8in</PageWidth>" +
                "   <PageHeight>10in</PageHeight>" +
                "   <MarginTop>0.3in</MarginTop>" +
                "   <MarginLeft>0in</MarginLeft>" +
                "   <MarginRight>0in</MarginRight>" +
                "   <MarginBottom>0.3in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] rendereBytes;

            rendereBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out FileNameExtension,
                out streams,
                out warnings);

            return File(rendereBytes, mimeType);
        }



        public ActionResult GeneraReporteReceta(string id, int procedencia, int fe, string pac, string med)
        {
            ServiciosController servicio = new ServiciosController();
            var sede = Session["codSede"].ToString();

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReporteReceta.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("../FichaElectronica/BandejaDeAtenciones");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Receta> cm = Usp_Reporte_Receta(procedencia, fe);

            ReportDataSource rd = new ReportDataSource("DataSet1", cm);

            lr.DataSources.Add(rd);
            SedesController se = new SedesController();
            UtilitarioController u = new UtilitarioController();
            DocumentoContableController dc = new DocumentoContableController();
            E_Master ma = u.ListadoHoraServidor().FirstOrDefault();
            E_Sede us = se.ListadoSedes().Find(x => x.CodSede == sede);

            string nombreCompleto = us.NomSede;

            ReportParameter[] rptParameters = new ReportParameter[] {
                new ReportParameter("fechaHoy", ma.HoraServidor.ToString("dd/MM/yyyy hh:mm tt")),
                new ReportParameter("CodSede", sede.ToString()),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("UsuGene", usuarioGenera),
                 new ReportParameter("paciente", pac),
                  new ReportParameter("medico",med)

            };

            lr.SetParameters(rptParameters);

            string reportType = id;
            string mimeType;
            string encoding;
            string FileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                "   <OutputFormat>" + id + "</OutputFormat>" +
                "   <PageWidth>8in</PageWidth>" +
                "   <PageHeight>10in</PageHeight>" +
                "   <MarginTop>0.3in</MarginTop>" +
                "   <MarginLeft>0in</MarginLeft>" +
                "   <MarginRight>0in</MarginRight>" +
                "   <MarginBottom>0.3in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] rendereBytes;

            rendereBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out FileNameExtension,
                out streams,
                out warnings);

            return File(rendereBytes, mimeType);
        }



    }
}