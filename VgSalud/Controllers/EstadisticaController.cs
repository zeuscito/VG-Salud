using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using VgSalud.Models;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Globalization;

namespace VgSalud.Controllers
{
    public class EstadisticaController : Controller
    {

        public ActionResult EstadisticaFacturacionXMes()
        {
            UtilitarioController uti = new UtilitarioController();
            var hora = uti.ListadoHoraServidor().FirstOrDefault();
            var NroMes = hora.HoraServidor.Month;
            var Anio = hora.HoraServidor.Year;

            ViewBag.Mes = NroMes;
            ViewBag.anio = Anio;

            var mes = NroMes;
            var anio = Anio;

            ViewBag.ListatipTar = ListaTipoTarifaFacturacion(mes.ToString(), anio);
            ViewBag.ListaDiasTar = ListaDiasTipoTarifaFacturacion(mes.ToString(), anio);
            ViewBag.totalxmes = ListaDeFacturacionXMes(mes.ToString(), anio);
            ViewBag.totpacientesxmes = ListaPacientesAtendidos(mes.ToString(), anio);
            ViewBag.FacturacionXTipoTarifa = FacturacionXTipoTarifa(mes.ToString(), anio);
            ViewBag.FacturacionXPacienteXSexo = FacturacionXPacienteXSexo(mes.ToString(), anio);
            ViewBag.ListaDiasSexo = ListaDiasSexo(mes.ToString(), anio);
            ViewBag.turno = ListaTurno();
            ViewBag.FacturacionXDiasXturno = FacturacionXDiasXturno(mes.ToString(), anio);
            ViewBag.FacturacionXTotalXturnoPacientes = FacturacionXTotalXturnoPacientes(mes.ToString(), anio);
            ViewBag.ListaSexos = ListadoSexo();
            ViewBag.ListaVentasTipoTarifa = ListaVentasTipoTarifa(mes.ToString(), anio);

            return View();
        }

        [HttpPost]
        public ActionResult EstadisticaFacturacionXMes(string mes, int anio)
        {

            ViewBag.Mes = mes;
            ViewBag.anio = anio;
            ViewBag.ListatipTar = ListaTipoTarifaFacturacion(mes.ToString(), anio);
            ViewBag.ListaDiasTar = ListaDiasTipoTarifaFacturacion(mes.ToString(), anio);
            ViewBag.totalxmes = ListaDeFacturacionXMes(mes.ToString(), anio);
            ViewBag.totpacientesxmes = ListaPacientesAtendidos(mes.ToString(), anio);
            ViewBag.FacturacionXTipoTarifa = FacturacionXTipoTarifa(mes.ToString(), anio);
            ViewBag.FacturacionXPacienteXSexo = FacturacionXPacienteXSexo(mes.ToString(), anio);
            ViewBag.ListaDiasSexo = ListaDiasSexo(mes.ToString(), anio);
            ViewBag.turno = ListaTurno();
            ViewBag.FacturacionXDiasXturno = FacturacionXDiasXturno(mes.ToString(), anio);
            ViewBag.FacturacionXTotalXturnoPacientes = FacturacionXTotalXturnoPacientes(mes.ToString(), anio);
            ViewBag.ListaSexos = ListadoSexo();
            ViewBag.ListaVentasTipoTarifa = ListaVentasTipoTarifa(mes.ToString(), anio);

            return View();
        }

        public List<E_Estadistica> ListaPacientesAtendidos(string mes, int anio)
        {
            var codSede = Session["codSede"].ToString();
            List<E_Estadistica> Lista = new List<E_Estadistica>();
            Random r = new Random();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("FacturacionXPacientesAtendidos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@CodSede", codSede);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Estadistica Ser = new E_Estadistica();
                            Ser.TotPaciente = dr.GetInt32(0);
                            Ser.dia = dr.GetInt32(1);
                            int numeroP = 0;
                            string cadena = "";
                            string[] PaletaDeColor = { "215,255,0", "255,0,0", "255,128,0", "255,255,0", "0,255,0", "255,0,255", "255,204,153", "0,0,255", "143,31,255, 153,255,255" };
                            int index = r.Next(0, PaletaDeColor.Count() - 1);
                            string ii = index.ToString();
                            string[] fija = cadena.Split(',');
                            bool b = true;

                            foreach (string item in fija)
                            {
                                if (item == "")
                                {
                                    numeroP = 0;
                                }
                                else
                                {
                                    numeroP = 1;
                                }
                                b = cadena.Contains(ii);
                                if (b == true)
                                {
                                    index = r.Next(0, PaletaDeColor.Count() - 1);
                                    ii = index.ToString();
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (numeroP == 0)
                            {
                                cadena = "" + index;
                            }
                            else
                            {
                                cadena = cadena + " , " + index;
                            }

                            Ser.NameDia = dr.GetString(2).Replace("á", "a");
                            Ser.color1 = PaletaDeColor[index].ToString();
                            Ser.color2 = r.Next(200, 255);
                            Ser.color3 = r.Next(200, 255);
                            Lista.Add(Ser);


                        }

                    }
                    return Lista;
                }
            }
        }

        public List<E_Estadistica> FacturacionXTipoTarifa(string mes, int anio)
        {
            var codSede = Session["codSede"].ToString();
            List<E_Estadistica> Lista = new List<E_Estadistica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("FacturacionXTipoTarifa", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@CodSede", codSede);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Estadistica Ser = new E_Estadistica();
                            Ser.NomTipTar = dr.GetString(0);
                            Ser.dia = dr.GetInt32(1);
                            Ser.Total = dr.GetDecimal(2);
                            Ser.CodTipTar = dr.GetString(3);

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Estadistica> ListaTipoTarifaFacturacion(string mes, int anio)
        {
            Random r = new Random();
            List<E_Estadistica> Lista = new List<E_Estadistica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("ListaTipoTarFactura", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@anio", anio);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        int numeroP = 0;
                        string cadena = "";
                        string[] PaletaDeColor = { "215,255,0", "255,0,0", "255,128,0", "255,255,0", "0,255,0", "255,0,255", "255,204,153", "0,0,255", "143,31,255, 153,255,255" };
                        while (dr.Read())
                        {
                            int index = r.Next(0, PaletaDeColor.Count() - 1);
                            string ii = index.ToString();
                            string[] fija = cadena.Split(',');
                            bool b = true;

                            foreach (string item in fija)
                            {
                                if (item == "")
                                {
                                    numeroP = 0;
                                }
                                else
                                {
                                    numeroP = 1;
                                }
                                b = cadena.Contains(ii);
                                if (b == true)
                                {
                                    index = r.Next(0, PaletaDeColor.Count() - 1);
                                    ii = index.ToString();
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (numeroP == 0)
                            {
                                cadena = "" + index;
                            }
                            else
                            {
                                cadena = cadena + " , " + index;
                            }

                            E_Estadistica Ser = new E_Estadistica();
                            Ser.NomTipTar = dr.GetString(0);
                            Ser.CodTipTar = dr.GetString(1);
                            Ser.color1 = PaletaDeColor[index].ToString();
                            Ser.color2 = r.Next(200, 255);
                            Ser.color3 = r.Next(200, 255);
                            Lista.Add(Ser);
                            r.Next(0);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Estadistica> ListaDiasTipoTarifaFacturacion(string mes, int anio)
        {
            var codSede = Session["codSede"].ToString();
            List<E_Estadistica> Lista = new List<E_Estadistica>();
            string[] PaletaDeColor = { "215,255,0", "255,0,0", "255,128,0", "255,255,0", "0,255,0", "255,0,255", "255,204,153", "0,0,255", "143,31,255, 153,255,255" };
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("FacturacionDiasXTipoTarifa", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@CodSede", codSede);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Estadistica Ser = new E_Estadistica();
                            Ser.dia = dr.GetInt32(0);

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        //4  
        public List<E_Estadistica> ListadoSexo()
        {
            Random r = new Random();
            List<E_Estadistica> Lista = new List<E_Estadistica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Sexo", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        int numeroP = 0;
                        string cadena = "";
                        string[] PaletaDeColor = { "215,255,0", "255,0,0", "255,128,0", "255,255,0", "0,255,0", "255,0,255", "255,204,153", "0,0,255", "143,31,255, 153,255,255" };
                        while (dr.Read())
                        {
                            int index = r.Next(0, PaletaDeColor.Count() - 1);
                            string ii = index.ToString();
                            string[] fija = cadena.Split(',');
                            bool b = true;

                            foreach (string item in fija)
                            {
                                if (item == "")
                                {
                                    numeroP = 0;
                                }
                                else
                                {
                                    numeroP = 1;
                                }
                                b = cadena.Contains(ii);
                                if (b == true)
                                {
                                    index = r.Next(0, PaletaDeColor.Count() - 1);
                                    ii = index.ToString();
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (numeroP == 0)
                            {
                                cadena = "" + index;
                            }
                            else
                            {
                                cadena = cadena + " , " + index;
                            }
                            E_Estadistica Ser = new E_Estadistica();

                            Ser.nomSexo = dr.GetString(1);
                            Ser.color1 = PaletaDeColor[index].ToString();
                            Ser.color2 = r.Next(200, 255);
                            Ser.color3 = r.Next(200, 255);
                            Lista.Add(Ser);
                        }
                        con.Close();
                    }
                }
                return Lista;
            }

        }
        //4  
        public List<E_Estadistica> FacturacionXPacienteXSexo(string mes, int anio)
        {
            var codSede = Session["codSede"].ToString();
            List<E_Estadistica> Lista = new List<E_Estadistica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("FacturacionXPacienteXSexo", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@CodSede", codSede);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Estadistica Ser = new E_Estadistica();
                            Ser.dia = dr.GetInt32(0);
                            Ser.nomSexo = dr.GetString(1);
                            Ser.TotPaciente = dr.GetInt32(2);
                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        //4  
        public List<E_Estadistica> ListaDiasSexo(string mes, int anio)
        {
            List<E_Estadistica> Lista = new List<E_Estadistica>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("ListaDiasSexo", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@anio", anio);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Estadistica Ser = new E_Estadistica();
                            Ser.dia = dr.GetInt32(0);

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Estadistica> ListaDeFacturacionXMes(string mes, int anio)
        {
            var codSede = Session["codSede"].ToString();
            List<E_Estadistica> Lista = new List<E_Estadistica>();
            Random r = new Random();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("FacturacionXPorMes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@CodSede", codSede);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        int numeroP = 0;
                        string cadena = "";
                        string[] PaletaDeColor = { "0,0,255" };
                        while (dr.Read())
                        {
                            int index = r.Next(0, PaletaDeColor.Count() - 1);
                            string ii = index.ToString();
                            string[] fija = cadena.Split(',');
                            bool b = true;

                            foreach (string item in fija)
                            {
                                if (item == "")
                                {
                                    numeroP = 0;
                                }
                                else
                                {
                                    numeroP = 1;
                                }
                                b = cadena.Contains(ii);
                                if (b == true)
                                {
                                    index = r.Next(0, PaletaDeColor.Count() - 1);
                                    ii = index.ToString();
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (numeroP == 0)
                            {
                                cadena = "" + index;
                            }
                            else
                            {
                                cadena = cadena + " , " + index;
                            }
                            E_Estadistica Ser = new E_Estadistica();
                            Ser.Total = dr.GetDecimal(0);
                            Ser.dia = dr.GetInt32(1);
                            Ser.NameDia = dr.GetString(2).Replace("á", "a");
                            Ser.color1 = PaletaDeColor[index].ToString();
                            Ser.color2 = r.Next(200, 255);
                            Ser.color3 = r.Next(200, 255);
                            Lista.Add(Ser);
                        }
                        con.Close();



                        return Lista;
                    }
                }
            }
        }
        //5 
        public List<E_Estadistica> ListaTurno()
        {
            Random r = new Random();
            List<E_Estadistica> Lista = new List<E_Estadistica>();

            string[] PaletaDeColor = { "0,128,255", "255,128,0" };


            E_Estadistica Ser = new E_Estadistica();
            Ser.Turno = "MAÑANA";
            Ser.color1 = PaletaDeColor[0].ToString();
            Ser.color2 = r.Next(200, 255);
            Ser.color3 = r.Next(200, 255);
            Lista.Add(Ser);
            E_Estadistica Ser1 = new E_Estadistica();
            Ser1.Turno = "TARDE";
            Ser1.color1 = PaletaDeColor[1].ToString();
            Ser1.color2 = r.Next(200, 255);
            Ser1.color3 = r.Next(200, 255);
            Lista.Add(Ser1);

            return Lista;

        }



        public List<E_Estadistica> FacturacionXDiasXturno(string mes, int anio)
        {
            var codSede = Session["codSede"].ToString();
            List<E_Estadistica> Lista = new List<E_Estadistica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("FacturacionXDiasXturno", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@CodSede", codSede);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Estadistica Ser = new E_Estadistica();
                            Ser.dia = dr.GetInt32(0);
                            Ser.Turno = dr.GetString(1);
                            Ser.Total = dr.GetDecimal(2);
                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Estadistica> FacturacionXTotalXturnoPacientes(string mes, int anio)
        {
            var codSede = Session["codSede"].ToString();
            List<E_Estadistica> Lista = new List<E_Estadistica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("FacturacionXTotalXturnoPacientes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@CodSede", codSede);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Estadistica Ser = new E_Estadistica();
                            Ser.dia = dr.GetInt32(0);
                            Ser.Turno = dr.GetString(1);
                            Ser.TotPaciente = dr.GetInt32(2);
                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }






        //Obed


        public List<E_Estadistica> ListaVentasTipoTarifa(string mes, int anio)
        {
            var codSede = Session["codSede"].ToString();
            Random r = new Random();
            List<E_Estadistica> Lista = new List<E_Estadistica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("TotalVentasTipoTarifa", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@CodSede", codSede);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        int numeroP = 0;
                        string cadena = "";
                        string[] PaletaDeColor = { "215,255,0", "255,0,0", "255,128,0", "255,255,0", "0,255,0", "255,0,255", "255,204,153", "0,0,255", "143,31,255, 153,255,255" };
                        while (dr.Read())
                        {
                            int index = r.Next(0, PaletaDeColor.Count() - 1);
                            string ii = index.ToString();
                            string[] fija = cadena.Split(',');
                            bool b = true;

                            foreach (string item in fija)
                            {
                                if (item == "")
                                {
                                    numeroP = 0;
                                }
                                else
                                {
                                    numeroP = 1;
                                }
                                b = cadena.Contains(ii);
                                if (b == true)
                                {
                                    index = r.Next(0, PaletaDeColor.Count() - 1);
                                    ii = index.ToString();
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (numeroP == 0)
                            {
                                cadena = "" + index;
                            }
                            else
                            {
                                cadena = cadena + " , " + index;
                            }

                            E_Estadistica Ser = new E_Estadistica();
                            Ser.NomTipTar = dr.GetString(0);
                            Ser.Total = dr.GetDecimal(1);
                            Ser.color1 = PaletaDeColor[index].ToString();

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult GeneraFacturacionXPorMes(string id, string Mes, int anio)
        {
            string sede = Session["codSede"].ToString();
            System.Globalization.DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
            string month = formatoFecha.GetMonthName(Convert.ToInt32(Mes));

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReporteFacturacionXMes.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("EstadisticaFacturacionXMes");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Estadistica> cm = ListaDeFacturacionXMes(Mes, anio);
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
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("CodSede", sede),
                    new ReportParameter("Mes", month),
                     new ReportParameter("Año", anio.ToString())
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

        public ActionResult GeneraPacienteAtendidoXDia(string id, string Mes, int anio)
        {
            string sede = Session["codSede"].ToString();
            System.Globalization.DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
            string month = formatoFecha.GetMonthName(Convert.ToInt32(Mes));

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReportePacientesAtendido.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("EstadisticaFacturacionXMes");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Estadistica> cm = ListaPacientesAtendidos(Mes, anio);
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
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("CodSede", sede),
                new ReportParameter("Mes", month),
                new ReportParameter("Año", anio.ToString())
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

        public ActionResult GeneraTipoTarifaPorDia(string id, string Mes, int anio)
        {
            string sede = Session["codSede"].ToString();
            System.Globalization.DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
            string month = formatoFecha.GetMonthName(Convert.ToInt32(Mes));

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReporteTipoTarifa.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("EstadisticaFacturacionXMes");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Estadistica> cm = FacturacionXTipoTarifa(Mes, anio);
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
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("CodSede", sede),
                new ReportParameter("Mes", month),
                new ReportParameter("Año", anio.ToString())
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

        public ActionResult GeneraTotalPacientesXSexo(string id, string Mes, int anio)
        {
            string sede = Session["codSede"].ToString();
            System.Globalization.DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
            string month = formatoFecha.GetMonthName(Convert.ToInt32(Mes));

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReportePacientesXSexo.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("EstadisticaFacturacionXMes");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Estadistica> cm = FacturacionXPacienteXSexo(Mes, anio);
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
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("CodSede", sede),
                new ReportParameter("Mes", month),
                new ReportParameter("Año", anio.ToString())
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

        public ActionResult GeneraFacturacionXTurno(string id, string Mes, int anio)
        {
            string sede = Session["codSede"].ToString();
            System.Globalization.DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
            string month = formatoFecha.GetMonthName(Convert.ToInt32(Mes));

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReporteFacturacionXTurno.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("EstadisticaFacturacionXMes");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Estadistica> cm = FacturacionXDiasXturno(Mes, anio);
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
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("CodSede", sede),
                new ReportParameter("Mes", month),
                new ReportParameter("Año", anio.ToString())
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


        public ActionResult GeneraPacienteXTurno(string id, string Mes, int anio)
        {
            string sede = Session["codSede"].ToString();
            System.Globalization.DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
            string month = formatoFecha.GetMonthName(Convert.ToInt32(Mes));

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReportePacienteXTurno.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("EstadisticaFacturacionXMes");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Estadistica> cm = FacturacionXTotalXturnoPacientes(Mes, anio);
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
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("CodSede", sede),
                new ReportParameter("Mes", month),
                new ReportParameter("Año", anio.ToString())
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


        public ActionResult GeneraTotalVentarXProducto(string id, string Mes, int anio)
        {
            string sede = Session["codSede"].ToString();
            System.Globalization.DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
            string month = formatoFecha.GetMonthName(Convert.ToInt32(Mes));

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reportes"), "ReportePacienteXTurno.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("EstadisticaFacturacionXMes");
            }

            string usuarioGenera = Session["UserID"].ToString() + " - " + Session["usuario"].ToString();

            List<E_Estadistica> cm = ListaVentasTipoTarifa(Mes, anio);
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
                new ReportParameter("UsuGene", usuarioGenera),
                new ReportParameter("NomSede", nombreCompleto),
                new ReportParameter("CodSede", sede),
                new ReportParameter("Mes", month),
                new ReportParameter("Año", anio.ToString())
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