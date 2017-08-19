using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using VgSalud.Models;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace VgSalud.Controllers
{
    public class CajaController : Controller
    {
        public string Generador_De_Codigo()
        {
            string codigo = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Generar_NroOperacion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NroOperacion", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    codigo = cmd.Parameters["@NroOperacion"].Value.ToString();
                }
                catch (Exception ex)
                {
                    ViewBag.mensaje = "error" + ex.Message;
                }


            }

            return codigo;
        }
        public List<E_Medios_Pago> ListadoMedioPago()
        {
            List<E_Medios_Pago> Lista = new List<E_Medios_Pago>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Medios_pago", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Medios_Pago usu = new E_Medios_Pago();

                            usu.CODMEDIOS = dr.GetString(0);
                            usu.DESCRIPCION = dr.GetString(1);
                            usu.ESTADO = dr.GetBoolean(2);

                            Lista.Add(usu);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public List<E_Caja> ListaCajaPago()
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Caja_Pago", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja usu = new E_Caja();

                            usu.CodCaja = dr.GetInt32(0);
                            usu.item = dr.GetInt32(1);
                            usu.CODMEDIOS = dr.GetString(2);
                            usu.Importe = dr.GetDecimal(3);
                            usu.ImporteSoles = dr.GetDecimal(4);
                            usu.CodTipMon = dr.GetString(5);
                            usu.TipoCambio = dr.GetDecimal(6);
                            usu.Estado = dr.GetBoolean(7);

                            Lista.Add(usu);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Caja> ListaCajaPecioTotal(int CodCaja)
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaCajaPrecioTotal", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodCaja", CodCaja);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja usu = new E_Caja();

                            usu.CodCaja = dr.GetInt32(0);
                            usu.Total = dr.GetDecimal(1);

                            Lista.Add(usu);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Caja> ListaCajaPagoTotal(int CodCaja)
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Precio_Caja_PagoTotal", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodCaja", CodCaja);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja usu = new E_Caja();

                            usu.CodCaja = dr.GetInt32(0);
                            usu.Total = dr.GetDecimal(1);

                            Lista.Add(usu);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Caja> CajaPreResumen(string CodUsu, string fechaE, string fechaS)
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaCajaPreResumen", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodUsu", CodUsu);
                    cmd.Parameters.AddWithValue("@FechaE", fechaE);
                    cmd.Parameters.AddWithValue("@FechaS", fechaS);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja usu = new E_Caja();

                            usu.Total = dr.GetDecimal(0);
                            usu.FechaEmision = dr.GetDateTime(1);
                            usu.TipoPago = dr.GetString(2);
                            usu.CodSede = dr.GetString(3);
                            usu.CodUsu = dr.GetString(4);
                            usu.AliasCodDoc = dr.GetString(5);

                            Lista.Add(usu);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Caja> ListaMediosPagoEncabezado(string CodUsu, string fechaE, string fechaS)
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaMediosPagoResumen", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodUsu", CodUsu);
                    cmd.Parameters.AddWithValue("@FechaE", fechaE);
                    cmd.Parameters.AddWithValue("@FechaS", fechaS);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja usu = new E_Caja();

                            usu.CODMEDIOS = dr.GetString(0);
                            usu.CodTipMon = dr.GetString(1);
                            usu.CodSede = dr.GetString(2);

                            Lista.Add(usu);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Caja> CajaPreResumenDetalle(string CodUsu, string fechaE, string fechaS)
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaCajaPreResumenDetalle", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodUsu", CodUsu);
                    cmd.Parameters.AddWithValue("@FechaE", fechaE);
                    cmd.Parameters.AddWithValue("@FechaS", fechaS);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja usu = new E_Caja();

                            usu.Total = dr.GetDecimal(0);
                            usu.FechaEmision = dr.GetDateTime(1);
                            usu.CODMEDIOS = dr.GetString(2);
                            usu.DescTipMon = dr.GetString(3);
                            usu.CodSede = dr.GetString(4);

                            Lista.Add(usu);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Caja> ListadoCajaResumen()
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_CajaResumen", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja ca = new E_Caja();

                            ca.CodCajRes = dr.GetInt32(0);
                            ca.CodUsu = dr.GetString(1);
                            ca.FechaDeposito = dr.GetDateTime(2);
                            ca.FechaCaja = dr.GetDateTime(3);
                            ca.TipoPago = dr.GetString(4);
                            ca.TipoCambio = dr.GetDecimal(5);
                            ca.TotalDolares = dr.GetDecimal(6);
                            ca.TotalUsuario = dr.GetDecimal(7);
                            ca.TotalSistema = dr.GetDecimal(8);
                            ca.Diferencia = dr.GetDecimal(9);
                            ca.Estado = dr.GetBoolean(10);

                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Caja> ListadoUsuario()
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaUsuarios", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja usu = new E_Caja();

                            usu.CodUsu = dr.GetString(0);
                            usu.AliasCodDoc = dr.GetString(1);
                            usu.CodSede = dr.GetString(2);

                            Lista.Add(usu);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Caja> ListadoUsuarioxServicio(string CodServ)
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaUsuariosxServicio", con))
                {
                    cmd.Parameters.AddWithValue("@CodServ", CodServ);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja usu = new E_Caja();

                            usu.CodUsu = dr.GetString(0);
                            usu.AliasCodDoc = dr.GetString(1);
                            usu.CodSede = dr.GetString(2);

                            Lista.Add(usu);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Caja> ListadoCajaCabecera()
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaCaja", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja ca = new E_Caja();

                            ca.CodCaja = dr.GetInt32(0);
                            ca.CodSede = dr.GetString(1);
                            ca.NomSede = dr.GetString(2);
                            ca.CodCue = dr.GetInt32(3);
                            ca.DescCodDoc = dr.GetString(4);
                            ca.Serie = dr.GetString(5);
                            ca.NumDoc = dr.GetString(6);
                            ca.FechaEmision = dr.GetDateTime(7);
                            ca.HoraEmision = dr.GetTimeSpan(8);
                            ca.Historia = dr.GetInt32(9);
                            ca.NomPac = dr.GetString(10);
                            ca.RazonSocial = dr.GetString(11);
                            ca.DirRazSoc = dr.GetString(12);
                            ca.DescCatPac = dr.GetString(13);
                            ca.Estado = dr.GetBoolean(14);
                            ca.SubTotal = dr.GetDecimal(15);
                            ca.Igv = dr.GetDecimal(16);
                            ca.Total = dr.GetDecimal(17);
                            ca.UsuCrea = dr.GetString(18);
                            ca.UsuAnula = dr.GetString(19);
                            ca.FechaAnula = dr.GetString(20);
                            ca.TipoPago = dr.GetString(21);
                            ca.CodRazSoc = dr.GetString(22);
                            ca.DescTipMon = dr.GetString(23);
                            ca.TipoCambio = dr.GetDecimal(24);
                            ca.Obser = dr.GetString(25);
                            ca.TazaIgv = dr.GetDecimal(26);
                            ca.AutorizaAnu = dr.GetString(27);
                            ca.PorAnular = dr.GetBoolean(28);
                            ca.AliasCodDoc = dr.GetString(4);
                            ca.CodDocSerie = dr.GetString(32);
                            ca.Ruc = dr.GetString(33);
                            ca.CodCatPac = dr.GetString(34);

                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public List<E_Caja> ListadoCajaGeneral(string CodUsu, string fechaE, string fechaS)
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaCajaGeneral", con))
                {
                    cmd.Parameters.AddWithValue("@CodUsu", CodUsu);
                    cmd.Parameters.AddWithValue("@FechaE", fechaE);
                    cmd.Parameters.AddWithValue("@FechaS", fechaS);
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
                            ca.PorAnular = dr.GetBoolean(18);

                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Caja> ListadoMediosPago(string CodUsu, string fechaE, string fechaS)
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaMediosPagosCaja", con))
                {
                    cmd.Parameters.AddWithValue("@CodUsu", CodUsu);
                    cmd.Parameters.AddWithValue("@FechaE", fechaE);
                    cmd.Parameters.AddWithValue("@FechaS", fechaS);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja ca = new E_Caja();

                            ca.DescCodDoc = dr.GetString(0);
                            ca.Total = dr.GetDecimal(1);
                            ca.CodSede = dr.GetString(2);

                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Caja> BuscaCajaCabecera(string CodServ, int CodCaja)
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_BuscaCaja", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodServ", CodServ);
                    cmd.Parameters.AddWithValue("@CodCaja", CodCaja);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja ca = new E_Caja();

                            ca.CodCaja = dr.GetInt32(0);
                            ca.CodSede = dr.GetString(1);
                            ca.NomSede = dr.GetString(2);
                            ca.CodCue = dr.GetInt32(3);
                            ca.DescCodDoc = dr.GetString(4);
                            ca.Serie = dr.GetString(5);
                            ca.NumDoc = dr.GetString(6);
                            ca.FechaEmision = dr.GetDateTime(7);
                            ca.HoraEmision = dr.GetTimeSpan(8);
                            ca.Historia = dr.GetInt32(9);
                            ca.NomPac = dr.GetString(10);
                            ca.RazonSocial = dr.GetString(11);
                            ca.DirRazSoc = dr.GetString(12);
                            ca.DescCatPac = dr.GetString(13);
                            ca.Estado = dr.GetBoolean(14);
                            ca.SubTotal = dr.GetDecimal(15);
                            ca.Igv = dr.GetDecimal(16);
                            ca.Total = dr.GetDecimal(17);
                            ca.UsuCrea = dr.GetString(18);
                            ca.UsuAnula = dr.GetString(19);
                            ca.FechaAnula = dr.GetString(20);
                            ca.TipoPago = dr.GetString(21);
                            ca.CodRazSoc = dr.GetString(22);
                            ca.DescTipMon = dr.GetString(23);
                            ca.TipoCambio = dr.GetDecimal(24);
                            ca.Obser = dr.GetString(25);
                            ca.TazaIgv = dr.GetDecimal(26);
                            ca.AutorizaAnu = dr.GetString(27);
                            ca.PorAnular = dr.GetBoolean(28);
                            ca.AliasCodDoc = dr.GetString(32);

                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_CajaDetalle> ListadoCajaDetalle()
        {
            List<E_CajaDetalle> Lista = new List<E_CajaDetalle>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaCajaDetalle", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_CajaDetalle ca = new E_CajaDetalle();

                            ca.CodSede = dr.GetString(0);
                            ca.CodCaja = dr.GetInt32(1);
                            ca.CodCue = dr.GetInt32(2);
                            ca.item = dr.GetInt32(3);
                            ca.CodTar = dr.GetString(4);
                            ca.NomTar = dr.GetString(5);
                            ca.Cantidad = dr.GetInt32(6);
                            ca.PUnit = dr.GetDecimal(7);
                            ca.SubTotal = dr.GetDecimal(8);
                            ca.Igv = dr.GetDecimal(9);
                            ca.Total = dr.GetDecimal(10);
                            ca.TazaIgv = dr.GetDecimal(11);

                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Caja> ImprimeCajaCabecera(int CodCaja)
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ImprimeFacturaCaja", con))
                {
                    cmd.Parameters.AddWithValue("@CodCaja", CodCaja);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja ca = new E_Caja();

                            ca.CodCaja = dr.GetInt32(0);
                            ca.CodSede = dr.GetString(1);
                            ca.NomSede = dr.GetString(2);
                            ca.CodCue = dr.GetInt32(3);
                            ca.DescCodDoc = dr.GetString(4);
                            ca.Serie = dr.GetString(5);
                            ca.NumDoc = dr.GetString(6);
                            ca.FechaEmision = dr.GetDateTime(7);
                            //ca.HoraEmision = dr.GetTimeSpan(8);
                            ca.Hora = dr.GetString(8);
                            ca.Historia = dr.GetInt32(9);
                            ca.NomPac = dr.GetString(10);
                            ca.RazonSocial = dr.GetString(11);
                            ca.DirRazSoc = dr.GetString(12);
                            ca.DescCatPac = dr.GetString(13);
                            ca.Estado = dr.GetBoolean(14);
                            ca.SubTotal = dr.GetDecimal(15);
                            ca.Igv = dr.GetDecimal(16);
                            ca.Total = dr.GetDecimal(17);
                            ca.UsuCrea = dr.GetString(18);
                            ca.UsuAnula = dr.GetString(19);
                            ca.FechaAnula = dr.GetString(20);
                            ca.TipoPago = dr.GetString(21);
                            ca.CodRazSoc = dr.GetString(22);
                            ca.DescTipMon = dr.GetString(23);
                            ca.TipoCambio = dr.GetDecimal(24);
                            ca.Obser = dr.GetString(25);
                            ca.TazaIgv = dr.GetDecimal(26);
                            ca.AutorizaAnu = dr.GetString(27);
                            ca.PorAnular = dr.GetBoolean(28);
                            ca.PrecioLetra = dr.GetString(29);
                            ca.Edad = dr.GetInt32(30);
                            ca.FecNac = dr.GetDateTime(31);
                            ca.Ruc =  dr["Ruc"] is DBNull ? string.Empty : dr["Ruc"].ToString();
                            ca.DirecSede = dr.GetString(33);

                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_CajaDetalle> ImprimeCajaDetalle(int CodCaja)
        {
            List<E_CajaDetalle> Lista = new List<E_CajaDetalle>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ImprimeFacturaCajaDetalle", con))
                {
                    cmd.Parameters.AddWithValue("@CodCaja", CodCaja);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_CajaDetalle ca = new E_CajaDetalle();

                            ca.CodSede = dr.GetString(0);
                            ca.CodCaja = dr.GetInt32(1);
                            ca.CodCue = dr.GetInt32(2);
                            ca.item = dr.GetInt32(3);
                            ca.CodTar = dr.GetString(4);
                            ca.NomTar = dr.GetString(5);
                            ca.Cantidad = dr.GetInt32(6);
                            ca.PUnit = dr.GetDecimal(7);
                            ca.SubTotal = dr.GetDecimal(8);
                            ca.Igv = dr.GetDecimal(9);
                            ca.Total = dr.GetDecimal(10);
                            ca.TazaIgv = dr.GetDecimal(11);

                            Lista.Add(ca);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_DocumentoSerie> ListadoCorrelativo(string CodSerie,SqlConnection cnn, SqlTransaction tr)
        {
            List<E_DocumentoSerie> Lista = new List<E_DocumentoSerie>();
           
                using (SqlCommand cmd = new SqlCommand("usp_CorrelativoCaja", cnn, tr))
                {
                    cmd.Parameters.AddWithValue("@CodSerie", CodSerie);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_DocumentoSerie usu = new E_DocumentoSerie();

                            usu.Serie = dr.GetString(0);
                            usu.CodDocSerie = dr.GetString(1);

                            Lista.Add(usu);
                        }
                     
                    }

                }
                return Lista;
            
        }

        public List<E_DocumentoSerie> ListadoBuscarCorrelativo(string CodSerie)
        {
            List<E_DocumentoSerie> Lista = new List<E_DocumentoSerie>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_CorrelativoCaja", con))
                {
                    cmd.Parameters.AddWithValue("@CodSerie", CodSerie);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_DocumentoSerie usu = new E_DocumentoSerie();

                            usu.Serie = dr.GetString(0);
                            usu.CodDocSerie = dr.GetString(1);

                            Lista.Add(usu);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_DocumentoSerie> ListadoCorrelativoMuestra(string CodSerie)
        {
            List<E_DocumentoSerie> Lista = new List<E_DocumentoSerie>();
            if (!string.IsNullOrWhiteSpace(CodSerie))
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_CorrelativoCajaMuestra", con))
                    {
                        cmd.Parameters.AddWithValue("@CodSerie", CodSerie);
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                E_DocumentoSerie usu = new E_DocumentoSerie();

                                usu.Serie = dr.GetString(0);
                                usu.CodDocSerie = dr.GetString(1);

                                Lista.Add(usu);
                            }
                            con.Close();
                        }

                    }

                }
            }
            return Lista;

        }

        public List<E_PersonaJuridica> ListadoPersonaJuridica()
        {
            List<E_PersonaJuridica> Lista = new List<E_PersonaJuridica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaPersonaJuridica", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_PersonaJuridica usu = new E_PersonaJuridica();

                            usu.CodPerJur = dr.GetInt32(0);
                            usu.RUC = dr.GetString(1);
                            usu.RazonSocial = dr.GetString(2);
                            usu.Direccion = dr.GetString(3);
                            usu.Estado = dr.GetBoolean(4);
                            usu.Concatenado = dr.GetString(1) + " - " + dr.GetString(2);

                            Lista.Add(usu);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_UsuarioSerie> ListadoUsuarioSerie(string CodUsu)
        {
            List<E_UsuarioSerie> Lista = new List<E_UsuarioSerie>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ListadoUsuarioSede", con))
                {
                    cmd.Parameters.AddWithValue("@CodUsu", CodUsu);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_UsuarioSerie usu = new E_UsuarioSerie();

                            usu.CodUsu = dr.GetString(0);
                            usu.CodDocSerie = dr.GetString(1);
                            usu.Prioridad = dr.GetBoolean(2);
                            usu.EstUDs = dr.GetBoolean(3);
                            usu.Etiqueta = dr.GetString(4);
                            usu.CodDocCont = dr.GetInt32(5);
                            usu.codigo = dr.GetInt32(6);
                            Lista.Add(usu);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public List<E_UsuarioSerie> usp_Ticket_TipoDoc(int CodCaja)
        {
            List<E_UsuarioSerie> Lista = new List<E_UsuarioSerie>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ticket_TipoDoc", con))
                {
                    cmd.Parameters.AddWithValue("@CodCaja", CodCaja);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_UsuarioSerie usu = new E_UsuarioSerie();
                            usu.Etiqueta = dr.GetString(0);
                            usu.CodDocCont = dr.GetInt32(1);
                            usu.CodUsu = $"{dr["AliasUsu"].ToString()}";
                            //usu.CodUsu = $"{dr["Nombre"].ToString()}";
                            Lista.Add(usu);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult RegistrarCaja(int id, string ruc = null, string cadena = null)
        {
            UtilitarioController u = new UtilitarioController();
            TipoMonedaController mo = new TipoMonedaController();
            E_Master hor = u.ListadoHoraServidor().FirstOrDefault();
            var evalua = mo.ListadoTipoMoneda1().Where(x => x.fechaParse == hor.HoraServidor.ToShortDateString() && x.CodTipMon == "TM002");
            TarifarioController tar = new TarifarioController();
            ViewBag.tarifa = tar.ListadoTarifa();

            if (evalua.Count() == 0)
            {
                ViewBag.mensaje = "Error: no esta asignado el tipo cambio dolar para hoy";
                ViewBag.bloquea = 1;
            }


            if (cadena != null)
            {
                string[] fija = cadena.Split(',');
                int i = 1;
                foreach (string item in fija)
                {
                    if (i == 1)
                    {
                        ViewBag.serie = item;
                    }
                    else if (i == 2)
                    {
                        ViewBag.rucC = item;
                    }
                    else if (i == 3)
                    {
                        ViewBag.tipPago = item;
                    }
                    else if (i == 4)
                    {
                        ViewBag.tipMoneda = item;
                    }
                    else if (i == 5)
                    {
                        ViewBag.modal = item;
                    }
                    else if (i == 6)
                    {
                        ViewBag.tc = item;
                    }

                    i++;

                }

            }
            else
            {

                ViewBag.serie = "";
                ViewBag.rucC = "";
                ViewBag.tipPago = "";
                ViewBag.tipMoneda = "";
                ViewBag.modal = "";
                ViewBag.tc = "";
            }


            string sede = Session["codSede"].ToString();
            string usuario = Session["UserID"].ToString();

            CuentasController c = new CuentasController();
            DatosGeneralesController da = new DatosGeneralesController();
            E_Datos_Generales reg1 = da.listadatogenerales().FirstOrDefault();
            E_Cuentas reg2 = c.ListadoCuenta(sede).Where(x => x.CodCue == id).FirstOrDefault();


            if (Session["pago"] == null)
            {
                Session["pago"] = new List<E_CajaPago>();

                var pago = (List<E_CajaPago>)Session["pago"];

                int cuenta = pago.Count() + 1;

                E_CajaPago item = new E_CajaPago();

                item.item = cuenta;
                item.CODMEDIOS = "ME001";
                item.NomMedios = "EFECTIVO";
                item.Importe = reg2.TotCue;
                item.ImporteSoles = reg2.TotCue;
                item.CodTipMon = "TM001";
                item.NomMoneda = "NUEVOS SOLES";
                item.TipoCambio = 0;
                item.Estado = true;

                pago.Add(item);
                Session["pago"] = pago;

                ViewBag.igv = (int)(reg1.igv * 100);
                ViewBag.tipoCambio = reg1.Tipo_Cambio;
            }

            else
            {
                Session.Remove("pago");
                Session["pago"] = new List<E_CajaPago>();

                var pago = (List<E_CajaPago>)Session["pago"];

                int cuenta = pago.Count() + 1;

                E_CajaPago item = new E_CajaPago();

                item.item = cuenta;
                item.CODMEDIOS = "ME001";
                item.NomMedios = "EFECTIVO";
                item.Importe = reg2.TotCue;
                item.ImporteSoles = reg2.TotCue;
                item.CodTipMon = "TM001";
                item.NomMoneda = "NUEVOS SOLES";
                item.TipoCambio = 0;
                item.Estado = true;

                pago.Add(item);
                Session["pago"] = pago;

                ViewBag.igv = (int)(reg1.igv * 100);
                ViewBag.tipoCambio = reg1.Tipo_Cambio;
            }


            ViewBag.CodCue = id;

            if (ruc != null)
            {
                ViewBag.ruc = ruc;
            }
            else
            {
                ViewBag.ruc = "";
            }

            TipoMonedaController tm = new TipoMonedaController();



            ViewBag.tipoMoneda = new SelectList(tm.ListadoTipoMoneda().ToList(), "CodTipMon", "DescTipMon");
            ViewBag.personaJuridica = new SelectList(ListadoPersonaJuridica().ToList(), "CodPerJur", "Concatenado");
            ViewBag.mediosPago = new SelectList(ListadoMedioPago().ToList(), "CODMEDIOS", "DESCRIPCION");
            ViewBag.listaCuenta = (List<E_Cuentas>)c.ListadoCuenta(sede).Where(x => x.CodCue == id).ToList();
            ViewBag.listaDetalle = (List<E_CuentaDetalle>)c.ListadoCuentaDetalle(id).Where(x => x.EstDet != "C").ToList();

            ViewBag.listaSerie = (List<E_UsuarioSerie>)ListadoUsuarioSerie(usuario).ToList();

            ViewBag.pago = (List<E_CajaPago>)Session["pago"];


            return View();
        }

        [HttpPost]
        public ActionResult RegistrarCaja(E_Caja c)
        {
            string sede = Session["codSede"].ToString();
            string usuario = Session["UserID"].ToString();
            AlertasController a = new AlertasController();
            DatosGeneralesController dat = new DatosGeneralesController();
            E_Datos_Generales reg1 = dat.listadatogenerales().FirstOrDefault();
            UtilitarioController ut = new UtilitarioController();
            E_Master hora = ut.ListadoHoraServidor().FirstOrDefault();
            TimeSpan HoraInicialCorte;
            
            var dataCorte = Usp_CorteCajaDiario(usuario, hora.HoraServidor, sede).FirstOrDefault();
            if (dataCorte != null)
            {
                HoraInicialCorte = dataCorte.horaInicio;
            }
            else {
                HoraInicialCorte = TimeSpan.Parse("00:00:00");
            }

            


            int Resu = 0; 
            
            UtilitarioController u = new UtilitarioController();
            TipoMonedaController mo = new TipoMonedaController();
            E_Master hor = u.ListadoHoraServidor().FirstOrDefault();
            E_TipoMoneda evalua = mo.ListadoTipoMoneda1().Find(x => x.fechaParse == hor.HoraServidor.ToShortDateString() && x.CodTipMon == "TM002");


            DocumentoSerieController ds = new DocumentoSerieController();
            PacientesController pa = new PacientesController();
            CuentasController cu = new CuentasController();
            TarifarioController ta = new TarifarioController();
            int CodCaja = 0;

            DateTime horaActual = hora.HoraServidor;
            string horaF = horaActual.ToString("H:mm:ss");

            E_DocumentoSerie docS = ds.ListarDocumentoSerie().Where(x => x.CodDocSerie == c.Serie).FirstOrDefault();
         
            E_Pacientes paciente = pa.ListadoPacientes().Where(x => x.Historia == c.Historia).FirstOrDefault();

            ViewBag.tipoMoneda = new SelectList(mo.ListadoTipoMoneda().ToList(), "CodTipMon", "DescTipMon", c.CodTipMon);
            ViewBag.personaJuridica = new SelectList(ListadoPersonaJuridica().ToList(), "CodPerJur", "Concatenado", c.Ruc);
            ViewBag.mediosPago = new SelectList(ListadoMedioPago().ToList(), "CODMEDIOS", "DESCRIPCION", c.CODMEDIOS);
            ViewBag.listaCuenta = (List<E_Cuentas>)cu.ListadoCuenta(sede).Where(x => x.CodCue == c.CodCue).ToList();
            ViewBag.listaDetalle = (List<E_CuentaDetalle>)cu.ListadoCuentaDetalle(c.CodCue).Where(x => x.EstDet != "C").ToList();

            ViewBag.listaSerie = (List<E_UsuarioSerie>)ListadoUsuarioSerie(usuario).ToList();

            ViewBag.pago = (List<E_CajaPago>)Session["pago"];

            ViewBag.serie = c.Serie;
            ViewBag.rucC = c.Ruc;
            ViewBag.tipPago = c.TipoPago;
            ViewBag.tipMoneda = c.tipMoneda;
            ViewBag.tc = "" + c.montoCambio;

            TarifarioController tarif = new TarifarioController();
            ViewBag.tarifa = tarif.ListadoTarifa();


            string modifica = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            string Crea = Session["usuario"] + " " + hora.HoraServidor.ToString() + " " + Environment.MachineName;

            if (c.evento == 1)
            {

                var listaDetalle = (List<E_CuentaDetalle>)cu.ListadoCuentaDetalle(c.CodCue).Where(x => x.EstDet != "C").ToList();

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                    con.Open();
                    SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                    if (docS.CodDocCont.Equals(1))
                    {

                        try
                        {
                            using (SqlCommand da = new SqlCommand("Usp_MtoPersonaJuridica", con, tr))
                            {

                                da.CommandType = CommandType.StoredProcedure;

                                da.Parameters.AddWithValue("@CodPerJur", c.Ruc);
                                da.Parameters.AddWithValue("@RUC", c.NrRUC);
                                da.Parameters.AddWithValue("@RazonSocial", c.RazonSocial.ToUpper());
                                da.Parameters.AddWithValue("@Direccion", c.DirRazSoc.ToUpper());
                                da.Parameters.AddWithValue("@Estado", "1");
                                da.Parameters.AddWithValue("@Evento", "2");


                                da.ExecuteNonQuery();

                                using (SqlCommand ca = new SqlCommand("Usp_MtoCaja", con, tr))
                                {
                                  
                                    ca.CommandType = CommandType.StoredProcedure;
                                    E_DocumentoSerie correlativo = ListadoCorrelativo(c.Serie, con, tr).FirstOrDefault();
                                    ca.Parameters.AddWithValue("@CodCaja", "");
                                    ca.Parameters.AddWithValue("@CodSede", sede);
                                    ca.Parameters.AddWithValue("@CodCue", c.CodCue);
                                    ca.Parameters.AddWithValue("@CodDocSerie", docS.CodDocSerie);
                                    ca.Parameters.AddWithValue("@Serie", docS.Serie);
                                    ca.Parameters.AddWithValue("@NumDoc", correlativo.Serie);
                                    ca.Parameters.AddWithValue("@FechaEmision", "");
                                    ca.Parameters.AddWithValue("@HoraEmision", horaF);
                                    ca.Parameters.AddWithValue("@Historia", c.Historia);
                                    ca.Parameters.AddWithValue("@NomPac", c.NomPac.ToUpper());
                                    ca.Parameters.AddWithValue("@DirecPac", paciente.Direcc.ToUpper());
                                    ca.Parameters.AddWithValue("@Ruc", c.NrRUC);
                                    ca.Parameters.AddWithValue("@RazonSocial", c.RazonSocial.ToUpper());
                                    ca.Parameters.AddWithValue("@DirRazSoc", c.DirRazSoc.ToUpper());
                                    ca.Parameters.AddWithValue("@CodCatPac", paciente.CodCatPac);
                                    ca.Parameters.AddWithValue("@Estado", "1");
                                    ca.Parameters.AddWithValue("@SubTotal", c.SubTotal);
                                    ca.Parameters.AddWithValue("@Igv", c.Igv);
                                    ca.Parameters.AddWithValue("@Total", c.Total);
                                    ca.Parameters.AddWithValue("@UsuCrea", usuario);
                                    ca.Parameters.AddWithValue("@UsuAnula", "");
                                    ca.Parameters.AddWithValue("@FechaAnula", "");
                                    ca.Parameters.AddWithValue("@TipoPago", c.TipoPago);
                                    ca.Parameters.AddWithValue("@CodRazSoc", c.Ruc);
                                    ca.Parameters.AddWithValue("@CodTipMon", c.CodTipMon);
                                    ca.Parameters.AddWithValue("@TipoCambio", evalua.TipoCambio);
                                    if (c.Obser == null)
                                    {
                                        ca.Parameters.AddWithValue("@Obser", "");
                                    }
                                    else
                                    {
                                        ca.Parameters.AddWithValue("@Obser", c.Obser);
                                    }
                                    ca.Parameters.AddWithValue("@TazaIgv", c.TazaIgv);
                                    ca.Parameters.AddWithValue("@AutorizaAnu", "");
                                    ca.Parameters.AddWithValue("@PorAnular", "");
                                    ca.Parameters.AddWithValue("@SecVenta", Generador_De_Codigo());
                                    ca.Parameters.AddWithValue("@Crea", Crea);
                                    ca.Parameters.AddWithValue("@Modifica", "");
                                    ca.Parameters.AddWithValue("@Elimina", "");
                                    ca.Parameters.AddWithValue("@Evento", "1");

                                    Resu = (int)ca.ExecuteScalar();
                                    CodCaja = Resu;

                                    using (SqlCommand cue = new SqlCommand("Usp_MtoCuentas", con, tr))
                                    {

                                        cue.CommandType = CommandType.StoredProcedure;
                                        cue.Parameters.AddWithValue("@CodCue", c.CodCue);
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
                                        cue.Parameters.AddWithValue("@SecFact", Resu);
                                        cue.Parameters.AddWithValue("@Usuario", "");
                                        cue.Parameters.AddWithValue("@UsuarioAnula", "");
                                        cue.Parameters.AddWithValue("@Crea", "");
                                        cue.Parameters.AddWithValue("@Modifica", Crea);
                                        cue.Parameters.AddWithValue("@Elimina", "");
                                        cue.Parameters.AddWithValue("@Evento", "4");
                                        cue.ExecuteNonQuery();

                                        int item = 1;

                                        foreach (var it in listaDetalle)
                                        {

                                            E_Tarifario tar = ta.ListadoTarifa().Where(x => x.CodTar == it.Tarifa).FirstOrDefault();

                                            SqlCommand cmd = new SqlCommand("Usp_MtoCaja_Detalle", con, tr);
                                            cmd.CommandType = CommandType.StoredProcedure;


                                            cmd.Parameters.AddWithValue("@CodSede", sede);
                                            cmd.Parameters.AddWithValue("@CodCaja", Resu);
                                            cmd.Parameters.AddWithValue("@CodCue", c.CodCue);
                                            cmd.Parameters.AddWithValue("@item", item);
                                            cmd.Parameters.AddWithValue("@CodTar", it.Tarifa);
                                            cmd.Parameters.AddWithValue("@NomTar", tar.DescTar.ToUpper());
                                            cmd.Parameters.AddWithValue("@Cantidad", it.Cantidad);
                                            cmd.Parameters.AddWithValue("@PUnit", it.precioUni);
                                            cmd.Parameters.AddWithValue("@SubTotal", it.precio);
                                            cmd.Parameters.AddWithValue("@Igv", it.igv);
                                            cmd.Parameters.AddWithValue("@Total", it.total);
                                            cmd.Parameters.AddWithValue("@TazaIgv", c.TazaIgv);
                                            cmd.Parameters.AddWithValue("@Crea", Crea);
                                            cmd.Parameters.AddWithValue("@Modifica", "");
                                            cmd.Parameters.AddWithValue("@Elimina", "");
                                            cmd.Parameters.AddWithValue("@Evento", "1");
                                            item++;
                                            cmd.ExecuteNonQuery();



                                        }

                                        decimal sumaPrecio = 0;
                                        foreach (E_CajaPago fi in (List<E_CajaPago>)Session["pago"])
                                        {


                                            SqlCommand cp = new SqlCommand("Usp_MtoCaja_Pago", con, tr);
                                            cp.CommandType = CommandType.StoredProcedure;

                                            cp.Parameters.AddWithValue("@CodCaja", Resu);
                                            cp.Parameters.AddWithValue("@item", fi.item);
                                            cp.Parameters.AddWithValue("@CODMEDIOS", fi.CODMEDIOS);
                                            cp.Parameters.AddWithValue("@Importe", fi.Importe);
                                            cp.Parameters.AddWithValue("@ImporteSoles", fi.ImporteSoles);
                                            cp.Parameters.AddWithValue("@CodTipMon", c.CodTipMon);
                                            cp.Parameters.AddWithValue("@TipoCambio", c.TipoCambio);
                                            cp.Parameters.AddWithValue("@Estado", "1");
                                            cp.Parameters.AddWithValue("@Crea", Crea);
                                            cp.Parameters.AddWithValue("@Modifica", "");
                                            cp.Parameters.AddWithValue("@Elimina", "");
                                            cp.Parameters.AddWithValue("@Evento", "1");
                                            sumaPrecio = sumaPrecio + fi.ImporteSoles;
                                            cp.ExecuteNonQuery();

                                        }

                                        if (sumaPrecio == c.Total)
                                        {

                                            tr.Commit();
                                        }
                                        else
                                        {
                                            string cadena = c.Serie + ',' + c.Ruc + ',' + c.TipoPago + ',' + c.CodTipMon + ",," + c.TipoCambio;
                                            Response.Write("<script language=javascript> function pregunta() " +
                    " { if (confirm('Error, el monto a pagar no coencide con a distribución de precios.')) " +
                    "    document.location.href = '" + "RegistrarCaja/?id=" + c.CodCue + "&cadena=" + cadena + "'; " +
                    " else document.location.href = '" + "RegistrarCaja/?id=" + c.CodCue + "&cadena=" + cadena + "'; } " +
                    " document.writeln(pregunta()); </script>");
                                            return null;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            tr.Rollback();
                            return View();
                        }
                        finally
                        {
                            con.Close();
                            Session.Remove("catePacienteM");
                        }
                    }

                    else
                    {

                        try
                        {

                            using (SqlCommand ca = new SqlCommand("Usp_MtoCaja", con, tr))
                            {

                                ca.CommandType = CommandType.StoredProcedure;
                                E_DocumentoSerie correlativo = ListadoCorrelativo(c.Serie, con,  tr).FirstOrDefault();
                                ca.Parameters.AddWithValue("@CodCaja", "");
                                ca.Parameters.AddWithValue("@CodSede", sede);
                                ca.Parameters.AddWithValue("@CodCue", c.CodCue);
                                ca.Parameters.AddWithValue("@CodDocSerie", docS.CodDocSerie);
                                ca.Parameters.AddWithValue("@Serie", docS.Serie);
                                ca.Parameters.AddWithValue("@NumDoc", correlativo.Serie);
                                ca.Parameters.AddWithValue("@FechaEmision", "");
                                ca.Parameters.AddWithValue("@HoraEmision", horaF);
                                ca.Parameters.AddWithValue("@Historia", c.Historia);
                                ca.Parameters.AddWithValue("@NomPac", c.NomPac.ToUpper());
                                ca.Parameters.AddWithValue("@DirecPac", paciente.Direcc.ToUpper());
                                ca.Parameters.AddWithValue("@Ruc", "");
                                ca.Parameters.AddWithValue("@RazonSocial", c.NomPac.ToUpper());
                                ca.Parameters.AddWithValue("@DirRazSoc", paciente.Direcc.ToUpper());
                                ca.Parameters.AddWithValue("@CodCatPac", paciente.CodCatPac);
                                ca.Parameters.AddWithValue("@Estado", "1");
                                ca.Parameters.AddWithValue("@SubTotal", c.SubTotal);
                                ca.Parameters.AddWithValue("@Igv", c.Igv);
                                ca.Parameters.AddWithValue("@Total", c.Total);
                                ca.Parameters.AddWithValue("@UsuCrea", usuario);
                                ca.Parameters.AddWithValue("@UsuAnula", "");
                                ca.Parameters.AddWithValue("@FechaAnula", "");
                                ca.Parameters.AddWithValue("@TipoPago", c.TipoPago);
                                ca.Parameters.AddWithValue("@CodRazSoc", "");
                                ca.Parameters.AddWithValue("@CodTipMon", c.CodTipMon);
                                ca.Parameters.AddWithValue("@TipoCambio", evalua.TipoCambio);
                                if (c.Obser == null)
                                {
                                    ca.Parameters.AddWithValue("@Obser", "");
                                }
                                else
                                {
                                    ca.Parameters.AddWithValue("@Obser", c.Obser);
                                }
                                ca.Parameters.AddWithValue("@TazaIgv", c.TazaIgv);
                                ca.Parameters.AddWithValue("@AutorizaAnu", "");
                                ca.Parameters.AddWithValue("@PorAnular", "");
                                ca.Parameters.AddWithValue("@SecVenta", Generador_De_Codigo());
                                ca.Parameters.AddWithValue("@Crea", Crea);
                                ca.Parameters.AddWithValue("@Modifica", "");
                                ca.Parameters.AddWithValue("@Elimina", "");
                                ca.Parameters.AddWithValue("@Evento", "1");

                                Resu = (int)ca.ExecuteScalar();
                                CodCaja = Resu;

                                using (SqlCommand cue = new SqlCommand("Usp_MtoCuentas", con, tr))
                                {

                                    cue.CommandType = CommandType.StoredProcedure;
                                    cue.Parameters.AddWithValue("@CodCue", c.CodCue);
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
                                    cue.Parameters.AddWithValue("@SecFact", Resu);
                                    cue.Parameters.AddWithValue("@Usuario", "");
                                    cue.Parameters.AddWithValue("@UsuarioAnula", "");
                                    cue.Parameters.AddWithValue("@Crea", "");
                                    cue.Parameters.AddWithValue("@Modifica", Crea);
                                    cue.Parameters.AddWithValue("@Elimina", "");
                                    cue.Parameters.AddWithValue("@Evento", "4");
                                    cue.ExecuteNonQuery();

                                    int item = 1;

                                    foreach (var it in listaDetalle)
                                    {

                                        E_Tarifario tar = ta.ListadoTarifa().Where(x => x.CodTar == it.Tarifa).FirstOrDefault();

                                        SqlCommand cmd = new SqlCommand("Usp_MtoCaja_Detalle", con, tr);
                                        cmd.CommandType = CommandType.StoredProcedure;


                                        cmd.Parameters.AddWithValue("@CodSede", sede);
                                        cmd.Parameters.AddWithValue("@CodCaja", Resu);
                                        cmd.Parameters.AddWithValue("@CodCue", c.CodCue);
                                        cmd.Parameters.AddWithValue("@item", item);
                                        cmd.Parameters.AddWithValue("@CodTar", it.Tarifa);
                                        cmd.Parameters.AddWithValue("@NomTar", tar.DescTar.ToUpper());
                                        cmd.Parameters.AddWithValue("@Cantidad", it.Cantidad);
                                        cmd.Parameters.AddWithValue("@PUnit", it.precioUni);
                                        cmd.Parameters.AddWithValue("@SubTotal", it.precio);
                                        cmd.Parameters.AddWithValue("@Igv", it.igv);
                                        cmd.Parameters.AddWithValue("@Total", it.total);
                                        cmd.Parameters.AddWithValue("@TazaIgv", c.TazaIgv);
                                        cmd.Parameters.AddWithValue("@Crea", Crea);
                                        cmd.Parameters.AddWithValue("@Modifica", "");
                                        cmd.Parameters.AddWithValue("@Elimina", "");
                                        cmd.Parameters.AddWithValue("@Evento", "1");
                                        item++;
                                        cmd.ExecuteNonQuery();



                                    }
                                    decimal sumaPrecio = 0;
                                    foreach (E_CajaPago fi in (List<E_CajaPago>)Session["pago"])
                                    {


                                        SqlCommand cp = new SqlCommand("Usp_MtoCaja_Pago", con, tr);
                                        cp.CommandType = CommandType.StoredProcedure;

                                        cp.Parameters.AddWithValue("@CodCaja", Resu);
                                        cp.Parameters.AddWithValue("@item", fi.item);
                                        cp.Parameters.AddWithValue("@CODMEDIOS", fi.CODMEDIOS);
                                        cp.Parameters.AddWithValue("@Importe", fi.Importe);
                                        cp.Parameters.AddWithValue("@ImporteSoles", fi.ImporteSoles);
                                        cp.Parameters.AddWithValue("@CodTipMon", c.CodTipMon);
                                        cp.Parameters.AddWithValue("@TipoCambio", c.TipoCambio);
                                        cp.Parameters.AddWithValue("@Estado", "1");
                                        cp.Parameters.AddWithValue("@Crea", Crea);
                                        cp.Parameters.AddWithValue("@Modifica", "");
                                        cp.Parameters.AddWithValue("@Elimina", "");
                                        cp.Parameters.AddWithValue("@Evento", "1");
                                        sumaPrecio = sumaPrecio + fi.ImporteSoles;
                                        cp.ExecuteNonQuery();
                                    }

                                    if (sumaPrecio == c.Total)
                                    {
                                        
                                        tr.Commit();
                                        E_Master horaFin = ut.ListadoHoraServidor().FirstOrDefault();
                                        var valorVendido = Usp_DataCorteCaja(hora.HoraServidor, usuario, HoraInicialCorte, horaFin.HoraServidor.TimeOfDay, sede).FirstOrDefault();
                                        if (reg1.montoCierre <= valorVendido.Total)
                                        {
                                            string asunto = "El usuario " + usuario + " ya supero el monto maximo en caja";
                                            a.registroAlertas(asunto, "", usuario, usuario);
                                        }
                                    }
                                    else
                                    {
                                        string cadena = c.Serie + ',' + c.Ruc + ',' + c.TipoPago + ',' + c.CodTipMon + ",," + c.TipoCambio;
                                        Response.Write("<script language=javascript> function pregunta() " +
                " { if (confirm('Error, el monto a pagar no coencide con a distribución de precios.')) " +
                "    document.location.href = '" + "RegistrarCaja/?id=" + c.CodCue + "&cadena=" + cadena + "'; " +
                " else document.location.href = '" + "RegistrarCaja/?id=" + c.CodCue + "&cadena=" + cadena + "'; } " +
                " document.writeln(pregunta()); </script>");
                                        return null;
                                    }

                                }

                            }
                        }

                        catch (Exception e)
                        {
                            tr.Rollback();
                            return View();
                        }
                        finally
                        {
                            con.Close();
                        }
                    }

                    Session.Remove("pago");
                    ViewBag.modal = "";
                    ViewBag.CodigoCajita = CodCaja;
                    ViewBag.activaAlerta = 1;

                    return RedirectPermanent("~/Caja/ImprimirTicket?CodCaja=" + Resu);

                }

            }

            else if (c.evento == 2)
            {

                TipoMonedaController tm = new TipoMonedaController();
                E_TipoMoneda tmo = tm.ListadoTipoMoneda().Where(x => x.CodTipMon == c.tipMoneda).FirstOrDefault();

                var pago = (List<E_CajaPago>)Session["pago"];

                int cuenta = pago.Count() + 1;



                string cadena = c.seriePago + ',' + c.rucPago + ',' + c.tipPago + ',' + c.tipMoneda + ',' + 1 + ',' + c.montoCambio;

                decimal evaluaImporte = c.Monto;

                foreach (var i in pago)
                {
                    if (c.montoCambio == 0)
                    {
                        evaluaImporte = evaluaImporte + i.ImporteSoles;
                    }
                    else
                    {
                        evaluaImporte = (evaluaImporte * c.montoCambio) + i.ImporteSoles;
                    }
                }

                if (pago.Count == 0)
                {
                    if (c.montoCambio == 0)
                    {
                        evaluaImporte = c.Monto;
                    }
                    else
                    {
                        evaluaImporte = c.Monto * c.montoCambio;
                    }
                }

                E_Cuentas reg2 = cu.ListadoCuenta(sede).Where(x => x.CodCue == c.CodCue).FirstOrDefault();

                if (evaluaImporte <= reg2.TotCue)
                {
                    E_CajaPago item = new E_CajaPago();

                    E_Medios_Pago med = ListadoMedioPago().Where(x => x.CODMEDIOS == c.CODMEDIOS).FirstOrDefault();


                    item.item = cuenta;
                    item.CODMEDIOS = c.CODMEDIOS;
                    item.NomMedios = med.DESCRIPCION;
                    item.Importe = c.Monto;
                    item.CodTipMon = c.tipMoneda;
                    item.NomMoneda = tmo.DescTipMon;
                    if (c.montoCambio == 0)
                    {
                        item.ImporteSoles = c.Monto;
                    }
                    else
                    {
                        item.ImporteSoles = c.montoCambio * c.Monto;
                    }
                    item.TipoCambio = c.montoCambio;
                    item.Estado = true;

                    pago.Add(item);
                    Session["pago"] = pago;
                    ViewBag.modal = "1";


                    return View(c);
                }
                else
                {

                    ViewBag.modal = "1";
                    decimal tot = evaluaImporte - reg2.TotCue;
                    ViewBag.excede = tot;
                    ViewBag.activaAlerta = 3;
                    return View(c);
                }
            }
            else if (c.evento == 3)
            {
                var formularios = (List<E_CajaPago>)Session["pago"];
                var registro = formularios.Where(x => x.item.Equals(c.item)).FirstOrDefault();
                formularios.Remove(registro);

                Session["pago"] = formularios;
            }

            else if (c.evento == 4)
            {
                var pago = (List<E_CajaPago>)Session["pago"];
                decimal evaluaImporte = c.Monto;
                foreach (var i in pago)
                {
                    if (c.montoCambio == 0)
                    {
                        evaluaImporte = evaluaImporte + i.ImporteSoles;
                    }
                    else
                    {
                        evaluaImporte = evaluaImporte + i.ImporteSoles;
                    }
                }

                if (pago.Count == 0)
                {
                    if (c.montoCambio == 0)
                    {
                        evaluaImporte = c.Monto;
                    }
                    else
                    {
                        evaluaImporte = c.Monto * c.montoCambio;
                    }
                }

                E_Cuentas reg2 = cu.ListadoCuenta(sede).Where(x => x.CodCue == c.CodCue).FirstOrDefault();

                if (evaluaImporte == reg2.TotCue)
                {
                    ViewBag.modal = "";
                    return View(c);
                }
                else
                {
                    ViewBag.modal = "1";

                    decimal tot = reg2.TotCue - evaluaImporte;
                    ViewBag.excede = tot;
                    ViewBag.activaAlerta = 4;
                    return View(c);
                }

            }
            else if (c.evento == 5)
            {
                int codigo = 0;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Usp_MtoPersonaJuridica", con))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CodPerJur", "");
                            cmd.Parameters.AddWithValue("@RUC", c.RucA);
                            cmd.Parameters.AddWithValue("@RazonSocial", c.RazonSocialA.ToUpper());
                            cmd.Parameters.AddWithValue("@Direccion", c.DirRazSocA.ToUpper());
                            cmd.Parameters.AddWithValue("@Estado", "");
                            cmd.Parameters.AddWithValue("@Evento", "1");
                            Resu = (int)cmd.ExecuteScalar();
                            codigo = Resu;
                            ViewBag.mensaje = "1";


                        }
                        catch (Exception ex)
                        {
                            ViewBag.Mensaje = "Error Datos no Validos";
                            return View();
                        }
                        finally
                        {
                            con.Close();

                        }
                    }
                    ViewBag.personaJuridica = new SelectList(ListadoPersonaJuridica().ToList(), "CodPerJur", "Concatenado");
                    ViewBag.ruc = codigo;
                    ViewBag.rucC = "" + codigo;
                    ViewBag.modal = "";
                    return View(c);
                }
            }

            return View(c);

        }

        public ActionResult Añadir(E_CajaPago c)
        {
            TipoMonedaController tm = new TipoMonedaController();
            E_TipoMoneda tmo = tm.ListadoTipoMoneda().Where(x => x.CodTipMon == c.tipMoneda).FirstOrDefault();
            string sede = Session["codSede"].ToString();

            var pago = (List<E_CajaPago>)Session["pago"];

            int cuenta = pago.Count() + 1;



            string cadena = c.seriePago + ',' + c.rucPago + ',' + c.tipPago + ',' + c.tipMoneda + ',' + 1 + ',' + c.montoCambio;

            decimal evaluaImporte = c.Monto;

            foreach (var i in pago)
            {
                if (c.montoCambio == 0)
                {
                    evaluaImporte = evaluaImporte + i.ImporteSoles;
                }
                else
                {
                    evaluaImporte = (evaluaImporte * c.montoCambio) + i.ImporteSoles;
                }
            }

            if (pago.Count == 0)
            {
                if (c.montoCambio == 0)
                {
                    evaluaImporte = c.Monto;
                }
                else
                {
                    evaluaImporte = c.Monto * c.montoCambio;
                }
            }

            CuentasController cu = new CuentasController();
            E_Cuentas reg2 = cu.ListadoCuenta(sede).Where(x => x.CodCue == c.CodCue).FirstOrDefault();

            if (evaluaImporte <= reg2.TotCue)
            {
                E_CajaPago item = new E_CajaPago();

                E_Medios_Pago med = ListadoMedioPago().Where(x => x.CODMEDIOS == c.CODMEDIOS).FirstOrDefault();


                item.item = cuenta;
                item.CODMEDIOS = c.CODMEDIOS;
                item.NomMedios = med.DESCRIPCION;
                item.Importe = c.Monto;
                item.CodTipMon = c.tipMoneda;
                item.NomMoneda = tmo.DescTipMon;
                if (c.montoCambio == 0)
                {
                    item.ImporteSoles = c.Monto;
                }
                else
                {
                    item.ImporteSoles = c.montoCambio * c.Monto;
                }
                item.TipoCambio = c.montoCambio;
                item.Estado = true;

                pago.Add(item);
                Session["pago"] = pago;

                return RedirectPermanent("RegistrarCaja/?id=" + c.CodCue + "&cadena=" + cadena);
            }
            else
            {
                decimal tot = evaluaImporte - reg2.TotCue;
                Response.Write("<script language=javascript> function pregunta() " +
                " { if (confirm('Error, el monto excede al precio total en  S/." + tot + "')) " +
                "    document.location.href = '" + "RegistrarCaja/?id=" + c.CodCue + "&cadena=" + cadena + "'; " +
                " else document.location.href = '" + "RegistrarCaja/?id=" + c.CodCue + "&cadena=" + cadena + "'; } " +
                " document.writeln(pregunta()); </script>");
                return null;
            }

        }

        public ActionResult Delete(E_CajaPago c)
        {

            var formularios = (List<E_CajaPago>)Session["pago"];
            var registro = formularios.Where(x => x.item.Equals(c.item)).FirstOrDefault();
            formularios.Remove(registro);

            Session["pago"] = formularios;

            string cadena = c.seriePago + ',' + c.rucPago + ',' + c.tipPago + ',' + c.tipMoneda + ',' + 1 + ',' + c.montoCambio;


            return RedirectPermanent("RegistrarCaja/?id=" + c.CodCue + "&cadena=" + cadena);
        }

        public ActionResult RegistrarPersonaJuridica(E_PersonaJuridica e)
        {
            int codigo = 0;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoPersonaJuridica", con))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodPerJur", "");
                        cmd.Parameters.AddWithValue("@RUC", e.RUC);
                        cmd.Parameters.AddWithValue("@RazonSocial", e.RazonSocial.ToUpper());
                        cmd.Parameters.AddWithValue("@Direccion", e.Direccion.ToUpper());
                        cmd.Parameters.AddWithValue("@Estado", "");
                        cmd.Parameters.AddWithValue("@Evento", "1");
                        int Resu = (int)cmd.ExecuteScalar();
                        codigo = Resu;
                        ViewBag.mensaje = "Se registro correctamente";


                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                        return View();
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectPermanent("RegistrarCaja/?id=" + e.CodCue + "&RUC=" + codigo);
            }
        }

        public ActionResult ObtenerDatos(int RUC)
        {
            var evalua = (List<E_PersonaJuridica>)ListadoPersonaJuridica().Where(x => x.CodPerJur == RUC).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult ObtenerMoneda(string TipMo)
        {
            UtilitarioController u = new UtilitarioController();
            E_Master m = u.ListadoHoraServidor().FirstOrDefault();
            TipoMonedaController mo = new TipoMonedaController();
            var evalua = (List<E_TipoMoneda>)mo.ListadoTipoMoneda1().Where(x => x.CodTipMon == TipMo && x.fechaParse == m.HoraServidor.ToShortDateString()).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            else
            {
                E_TipoMoneda tip = new E_TipoMoneda();
                tip.TipoCambio = 0;
                evalua.Add(tip);
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ObtenerSerie(string CodSerie)
        {
            var evalua = (List<E_DocumentoSerie>)ListadoBuscarCorrelativo(CodSerie).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult ObtenerSerieMuestra(string CodSerie)
        {
            var evalua = (List<E_DocumentoSerie>)ListadoCorrelativoMuestra(CodSerie).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult AutorizarAnulacion(int? CodCaja = null)
        {

            if (CodCaja != null)
            {
                ViewBag.listaCabecera = (List<E_Caja>)ListadoCajaCabecera().Where(x => x.CodCaja == CodCaja).ToList();
                ViewBag.listaDetalle = (List<E_CajaDetalle>)ListadoCajaDetalle().Where(x => x.CodCaja == CodCaja).ToList();
                ViewBag.CodCaja = CodCaja;
            }
            else
            {
                ViewBag.listaCabecera = null;
                ViewBag.listaDetalle = null;
                ViewBag.CodCaja = "";
            }

            return View();
        }
        
        public ActionResult DetalleCaja(int id)
        {

            ViewBag.listaCabecera = (List<E_Caja>)ListadoCajaCabecera().Where(x => x.CodCaja == id).ToList();
            ViewBag.listaDetalle = (List<E_CajaDetalle>)ListadoCajaDetalle().Where(x => x.CodCaja == id).ToList();
            ViewBag.CodCaja = id;

            return View();
        }

        public ActionResult ActivarEliminar(E_Caja c)
        {
            string modifica = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();

                try
                {

                    using (SqlCommand ca = new SqlCommand("Usp_MtoCaja", con))
                    {

                        ca.CommandType = CommandType.StoredProcedure;
                        ca.Parameters.AddWithValue("@CodCaja", c.CodCaja);
                        ca.Parameters.AddWithValue("@CodSede", "");
                        ca.Parameters.AddWithValue("@CodCue", "");
                        ca.Parameters.AddWithValue("@CodDocSerie", "");
                        ca.Parameters.AddWithValue("@Serie", "");
                        ca.Parameters.AddWithValue("@NumDoc", "");
                        ca.Parameters.AddWithValue("@FechaEmision", "");
                        ca.Parameters.AddWithValue("@HoraEmision", "");
                        ca.Parameters.AddWithValue("@Historia", "");
                        ca.Parameters.AddWithValue("@NomPac", "");
                        ca.Parameters.AddWithValue("@DirecPac", "");
                        ca.Parameters.AddWithValue("@Ruc", "");
                        ca.Parameters.AddWithValue("@RazonSocial", "");
                        ca.Parameters.AddWithValue("@DirRazSoc", "");
                        ca.Parameters.AddWithValue("@CodCatPac", "");
                        ca.Parameters.AddWithValue("@Estado", "");
                        ca.Parameters.AddWithValue("@SubTotal", 0);
                        ca.Parameters.AddWithValue("@Igv", 0);
                        ca.Parameters.AddWithValue("@Total", 0);
                        ca.Parameters.AddWithValue("@UsuCrea", "");
                        ca.Parameters.AddWithValue("@UsuAnula", "");
                        ca.Parameters.AddWithValue("@FechaAnula", "");
                        ca.Parameters.AddWithValue("@TipoPago", "");
                        ca.Parameters.AddWithValue("@CodRazSoc", "");
                        ca.Parameters.AddWithValue("@CodTipMon", "");
                        ca.Parameters.AddWithValue("@TipoCambio", 0);
                        ca.Parameters.AddWithValue("@Obser", "");
                        ca.Parameters.AddWithValue("@TazaIgv", 0);
                        ca.Parameters.AddWithValue("@AutorizaAnu", modifica);
                        ca.Parameters.AddWithValue("@PorAnular", "");
                        ca.Parameters.AddWithValue("@SecVenta", "");
                        ca.Parameters.AddWithValue("@Crea", "");
                        ca.Parameters.AddWithValue("@Modifica", "");
                        ca.Parameters.AddWithValue("@Elimina", "");
                        if (c.evento == 1)
                        {
                            ca.Parameters.AddWithValue("@Evento", "3");
                        }
                        else
                        {
                            ca.Parameters.AddWithValue("@Evento", "4");
                        }

                        ca.ExecuteNonQuery();

                    }
                }
                catch (Exception e)
                {
                    ViewBag.mensaje = e.Message;
                }
                finally
                {
                    con.Close();
                }

                Response.Write("<script language=javascript> alert('Se autorizo correctamente') </script>");
            }

            return RedirectPermanent("AutorizarAnulacion?CodCaja=" + c.CodCaja);

        }

        public ActionResult ListadoCaja(string fechaE = null, string fechaS = null, int? CodCaja = null, int? Elimina = null, int? Evento = null, int? Detalle = null, string moneda = null, decimal? monto = null, decimal[] array = null, string FecDeposito = null)
        {
            ViewBag.modal = "";
            UtilitarioController ma = new UtilitarioController();
            E_Master hor = ma.ListadoHoraServidor().FirstOrDefault();
            var validaFecha = hor.HoraServidor.AddDays(-1);
            ViewBag.mediosPago = new SelectList(ListadoMedioPago().ToList(), "CODMEDIOS", "DESCRIPCION");
            ViewBag.pago = null;

            string sede = Session["codSede"].ToString();
            string usuario = Session["UserID"].ToString();
            ViewBag.Hoy = hor.HoraServidor.ToString("dd/MM/yyyy");
            ViewBag.usuario = usuario;

            ViewBag.modal = "";

            string Crea = Session["usuario"] + " " + hor.HoraServidor.ToString() + " " + Environment.MachineName;

            if (Detalle != null)
            {
                if (Detalle != 0)
                {
                    ViewBag.modal = "1";
                    ViewBag.Detalle = Detalle;
                    ViewBag.pago = (List<E_Caja>)ListaCajaPago().Where(x => x.CodCaja == Detalle && x.Estado == true).ToList();
                }
            }


            if (Evento == 3)
            {
                ViewBag.modal = "1";
                ViewBag.Detalle = Detalle;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cp = new SqlCommand("Usp_MtoCaja_Pago", con))
                    {
                        try
                        {
                            cp.CommandType = CommandType.StoredProcedure;

                            cp.Parameters.AddWithValue("@CodCaja", Detalle);
                            cp.Parameters.AddWithValue("@item", Elimina);
                            cp.Parameters.AddWithValue("@CODMEDIOS", "");
                            cp.Parameters.AddWithValue("@Importe", 0);
                            cp.Parameters.AddWithValue("@ImporteSoles", 0);
                            cp.Parameters.AddWithValue("@CodTipMon", "");
                            cp.Parameters.AddWithValue("@TipoCambio", 0);
                            cp.Parameters.AddWithValue("@Estado", "");
                            cp.Parameters.AddWithValue("@Crea", "");
                            cp.Parameters.AddWithValue("@Modifica", "");
                            cp.Parameters.AddWithValue("@Elimina", Crea);
                            cp.Parameters.AddWithValue("@Evento", "2");
                            cp.ExecuteNonQuery();

                            ViewBag.mensaje = "Se registro correctamente";


                        }
                        catch (Exception ex)
                        {
                            ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                            return View();
                        }
                        finally
                        {
                            con.Close();

                        }
                    }
                    ViewBag.fechaE = fechaE;
                    ViewBag.fechaS = fechaS;
                    DateTime fec1 = DateTime.Parse(fechaE);
                    DateTime fec2 = DateTime.Parse(fechaS);
                    ViewBag.CajaPreResumen = (List<E_Caja>)CajaPreResumen(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede && x.FechaCaja >= validaFecha).ToList();
                    ViewBag.CajaPreResumenDetalle = (List<E_Caja>)CajaPreResumenDetalle(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                    ViewBag.listaMediosPago = (List<E_Caja>)ListadoMediosPago(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                    ViewBag.listaMediosEncabezado = (List<E_Caja>)ListaMediosPagoEncabezado(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                    ViewBag.pago = (List<E_Caja>)ListaCajaPago().Where(x => x.CodCaja == Detalle && x.Estado == true).ToList();
                    return View(ListadoCajaGeneral(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList());
                }
            }

            if (Evento == 4)
            {
                ViewBag.modal = "1";
                ViewBag.Detalle = Detalle;
                int it = 1;
                E_Caja cajj = ListaCajaPago().Where(x => x.CodCaja == Detalle && x.Estado == true).LastOrDefault();
                if (cajj == null)
                {
                    it = 1;
                }
                else
                {
                    it = cajj.item + 1;
                }
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    E_Caja eva = ListaCajaPecioTotal((int)Detalle).FirstOrDefault();
                    E_Caja evaPago = ListaCajaPagoTotal((int)Detalle).FirstOrDefault();
                    decimal tot = 0;
                    if (evaPago == null)
                    {
                        tot = 0;
                    }
                    else
                    {
                        tot = evaPago.Total;
                    }
                    decimal total = tot + (decimal)monto;
                    if (eva.Total < total)
                    {
                        ViewBag.mensaje = "El monto asignado excede al total de la caja";
                    }
                    else
                    {
                        con.Open();
                        using (SqlCommand cp = new SqlCommand("Usp_MtoCaja_Pago", con))
                        {
                            try
                            {
                                cp.CommandType = CommandType.StoredProcedure;

                                cp.Parameters.AddWithValue("@CodCaja", Detalle);
                                cp.Parameters.AddWithValue("@item", it);
                                cp.Parameters.AddWithValue("@CODMEDIOS", moneda);
                                cp.Parameters.AddWithValue("@Importe", monto);
                                cp.Parameters.AddWithValue("@ImporteSoles", monto);
                                cp.Parameters.AddWithValue("@CodTipMon", "TM001");
                                cp.Parameters.AddWithValue("@TipoCambio", 0);
                                cp.Parameters.AddWithValue("@Estado", "1");
                                cp.Parameters.AddWithValue("@Crea", Crea);
                                cp.Parameters.AddWithValue("@Modifica", "");
                                cp.Parameters.AddWithValue("@Elimina", "");
                                cp.Parameters.AddWithValue("@Evento", "1");
                                cp.ExecuteNonQuery();

                                ViewBag.mensaje = "Se registro correctamente";


                            }
                            catch (Exception ex)
                            {
                                ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                                return View();
                            }
                            finally
                            {
                                con.Close();

                            }
                        }
                    }
                    ViewBag.fechaE = fechaE;
                    ViewBag.fechaS = fechaS;
                    DateTime fec1 = DateTime.Parse(fechaE);
                    DateTime fec2 = DateTime.Parse(fechaS);
                    ViewBag.CajaPreResumen = (List<E_Caja>)CajaPreResumen(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede && x.FechaCaja >= validaFecha).ToList();
                    ViewBag.CajaPreResumenDetalle = (List<E_Caja>)CajaPreResumenDetalle(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                    ViewBag.listaMediosPago = (List<E_Caja>)ListadoMediosPago(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                    ViewBag.listaMediosEncabezado = (List<E_Caja>)ListaMediosPagoEncabezado(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                    ViewBag.pago = (List<E_Caja>)ListaCajaPago().Where(x => x.CodCaja == Detalle && x.Estado == true).ToList();
                    return View(ListadoCajaGeneral(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList());
                }
            }

            if (Evento == 5)
            {
                ViewBag.modal = "1";
                ViewBag.Detalle = Detalle;

                E_Caja eva = ListaCajaPecioTotal((int)Detalle).FirstOrDefault();
                E_Caja evaPago = ListaCajaPagoTotal((int)Detalle).FirstOrDefault();
                decimal tot = 0;
                if (evaPago == null)
                {
                    tot = 0;
                }
                else
                {
                    tot = evaPago.Total;
                }
                if (eva.Total != tot)
                {
                    ViewBag.mensaje = "Los montos no equivalen al total de caja";
                }
                else
                {
                    ViewBag.modal = "";
                    ViewBag.Detalle = 0;
                }
                ViewBag.fechaE = fechaE;
                ViewBag.fechaS = fechaS;
                DateTime fec1 = DateTime.Parse(fechaE);
                DateTime fec2 = DateTime.Parse(fechaS);
                ViewBag.CajaPreResumen = (List<E_Caja>)CajaPreResumen(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede && x.FechaEmision >= validaFecha).ToList();
                ViewBag.CajaPreResumenDetalle = (List<E_Caja>)CajaPreResumenDetalle(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.listaMediosPago = (List<E_Caja>)ListadoMediosPago(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.listaMediosEncabezado = (List<E_Caja>)ListaMediosPagoEncabezado(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.pago = (List<E_Caja>)ListaCajaPago().Where(x => x.CodCaja == Detalle && x.Estado == true).ToList();
                return View(ListadoCajaGeneral(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList());

            }
            if (Evento == 6)
            {

                ViewBag.fechaE = fechaE;
                ViewBag.fechaS = fechaS;
                DateTime fec1 = DateTime.Parse(fechaE);
                DateTime fec2 = DateTime.Parse(fechaS);

                var listaCajaPreResumen = (List<E_Caja>)CajaPreResumen(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede && x.FechaEmision >= validaFecha).ToList();
                var listaCajaPreResumenDetalle = (List<E_Caja>)CajaPreResumenDetalle(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();

                int cuenta = 0;
                decimal diferencia = 0;

                UtilitarioController ut = new UtilitarioController();
                E_Usuario usu = ut.ListadoUsuarioEspecialidadGeneral(usuario).FirstOrDefault();
                E_Master hora = ut.ListadoHoraServidor().FirstOrDefault();

                DateTime horaActual = hora.HoraServidor;
                string horaF = horaActual.ToString("dd/MM/yyyy H:mm:ss");
                string modifica = Session["usuario"] + " " + horaF + " " + Environment.MachineName;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                    con.Open();
                    SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        var evalua = ListadoCajaResumen().ToList();
                        foreach (var lista in listaCajaPreResumen)
                        {
                            var consulta = evalua.Find(x => x.FechaCaja == lista.FechaEmision && x.CodUsu == usuario);

                            if (consulta != null)
                            {
                                if (consulta.Estado == true)
                                {
                                    break;
                                }
                            }

                            if (lista.Total >= array[cuenta])
                            {
                                diferencia = lista.Total - array[cuenta];
                            }
                            else
                            {
                                diferencia = array[cuenta] - lista.Total;
                            }

                            using (SqlCommand ca = new SqlCommand("Usp_MtoCajaResumen", con, tr))
                            {

                                ca.CommandType = CommandType.StoredProcedure;

                                ca.Parameters.AddWithValue("@CodCajRes", "");
                                ca.Parameters.AddWithValue("@Usuario", usuario);
                                ca.Parameters.AddWithValue("@FechaDeposito", FecDeposito);
                                ca.Parameters.AddWithValue("@FechaCaja", lista.FechaEmision);
                                ca.Parameters.AddWithValue("@TipoPago", lista.TipoPago);
                                ca.Parameters.AddWithValue("@TipoCambio", 0);
                                ca.Parameters.AddWithValue("@TotalDolares", 0);
                                ca.Parameters.AddWithValue("@TotalUsuario", array[cuenta]);
                                ca.Parameters.AddWithValue("@TotalSistema", lista.Total);
                                ca.Parameters.AddWithValue("@Diferencia", diferencia);

                                if (usu != null)
                                {
                                    ca.Parameters.AddWithValue("@Estado", "1");
                                }
                                else
                                {
                                    ca.Parameters.AddWithValue("@Estado", "0");
                                }
                                ca.Parameters.AddWithValue("@Crea", modifica);
                                ca.Parameters.AddWithValue("@Modifica", "");
                                ca.Parameters.AddWithValue("@Elimina", "");
                                int Resu = 0;
                                if (consulta == null)
                                {
                                    ca.Parameters.AddWithValue("@Evento", "1");
                                    Resu = (int)ca.ExecuteScalar();
                                }
                                else
                                {
                                    ca.Parameters.AddWithValue("@Evento", "2");
                                    ca.ExecuteNonQuery();

                                    using (SqlCommand caj = new SqlCommand("Usp_MtoCajaResumenDetalle", con, tr))
                                    {

                                        caj.CommandType = CommandType.StoredProcedure;
                                        caj.Parameters.AddWithValue("@CodCajRes", consulta.CodCajRes);
                                        caj.Parameters.AddWithValue("@CodMedios", "");
                                        caj.Parameters.AddWithValue("@DescriMedio", "");
                                        caj.Parameters.AddWithValue("@Total", 0);
                                        caj.Parameters.AddWithValue("@Crea", "");
                                        caj.Parameters.AddWithValue("@Modifica", "");
                                        caj.Parameters.AddWithValue("@Elimina", "");
                                        caj.Parameters.AddWithValue("@Evento", "2");

                                        caj.ExecuteNonQuery();

                                    }
                                }

                                var listaDetalle = listaCajaPreResumenDetalle.Where(x => x.FechaEmision.ToShortDateString() == lista.FechaEmision.ToShortDateString()).ToList();
                                foreach (var li in listaDetalle)
                                {

                                    using (SqlCommand caj = new SqlCommand("Usp_MtoCajaResumenDetalle", con, tr))
                                    {

                                        caj.CommandType = CommandType.StoredProcedure;
                                        if (consulta == null)
                                        {
                                            caj.Parameters.AddWithValue("@CodCajRes", Resu);
                                        }
                                        else
                                        {
                                            caj.Parameters.AddWithValue("@CodCajRes", consulta.CodCajRes);
                                        }
                                        caj.Parameters.AddWithValue("@CodMedios", li.CODMEDIOS);
                                        caj.Parameters.AddWithValue("@DescriMedio", li.DescTipMon);
                                        caj.Parameters.AddWithValue("@Total", li.Total);
                                        caj.Parameters.AddWithValue("@Crea", modifica);
                                        caj.Parameters.AddWithValue("@Modifica", "");
                                        caj.Parameters.AddWithValue("@Elimina", "");
                                        caj.Parameters.AddWithValue("@Evento", "1");

                                        caj.ExecuteNonQuery();

                                    }
                                }
                            }
                            cuenta++;
                        }

                        tr.Commit();
                    }
                    catch (Exception e)
                    {
                        tr.Rollback();
                        ViewBag.mensaje = e.Message;
                    }
                    finally
                    {
                        con.Close();
                    }

                }

                ViewBag.mensaje = "Registro exitoso";

                ViewBag.CajaPreResumen = (List<E_Caja>)CajaPreResumen(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede && x.FechaEmision >= validaFecha).ToList();
                ViewBag.CajaPreResumenDetalle = (List<E_Caja>)CajaPreResumenDetalle(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.listaMediosPago = (List<E_Caja>)ListadoMediosPago(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.listaMediosEncabezado = (List<E_Caja>)ListaMediosPagoEncabezado(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.pago = (List<E_Caja>)ListaCajaPago().Where(x => x.CodCaja == Detalle && x.Estado == true).ToList();
                return View(ListadoCajaGeneral(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList());

            }


            if (CodCaja != null)
            {
                string fechaF = hor.HoraServidor.ToString("dd/MM/yyyy");
                ViewBag.fechaE = fechaF;
                ViewBag.fechaS = fechaF;
                string fechaF1 = hor.HoraServidor.ToString("yyyy-MM-dd");
                DateTime manda = DateTime.Parse(fechaF1);
                ViewBag.fecha = fechaF;
                UsuarioServicioController us = new UsuarioServicioController();
                E_Usuario_Servicio u = us.ListaUsuarioServicio().Find(x => x.CodUsu == usuario);
                int ParseCaja = (int)CodCaja;
                ViewBag.codigo = ParseCaja;
                ViewBag.listaMediosPago = null;
                return View(BuscaCajaCabecera(u.CodServ, ParseCaja).ToList());
            }
            if (fechaE != null && fechaS != null)
            {
                ViewBag.fechaE = fechaE;
                ViewBag.fechaS = fechaS;
                DateTime fec1 = DateTime.Parse(fechaE);
                DateTime fec2 = DateTime.Parse(fechaS);
                ViewBag.CajaPreResumen = (List<E_Caja>)CajaPreResumen(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede && x.FechaEmision >= validaFecha).ToList();
                ViewBag.CajaPreResumenDetalle = (List<E_Caja>)CajaPreResumenDetalle(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.listaMediosPago = (List<E_Caja>)ListadoMediosPago(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.listaMediosEncabezado = (List<E_Caja>)ListaMediosPagoEncabezado(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                return View(ListadoCajaGeneral(usuario, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList());
            }
            else
            {
                string fechaF = hor.HoraServidor.ToString("dd/MM/yyyy");
                ViewBag.fechaE = fechaF;
                ViewBag.fechaS = fechaF;
                string fechaF1 = hor.HoraServidor.ToString("yyyy-MM-dd");
                DateTime manda = DateTime.Parse(fechaF1);
                ViewBag.fecha = fechaF;

                ViewBag.listaMediosPago = (List<E_Caja>)ListadoMediosPago(usuario, manda.ToShortDateString(), manda.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.CajaPreResumen = (List<E_Caja>)CajaPreResumen(usuario, manda.ToShortDateString(), manda.ToShortDateString()).Where(x => x.CodSede == sede && x.FechaEmision >= validaFecha).ToList();
                ViewBag.CajaPreResumenDetalle = (List<E_Caja>)CajaPreResumenDetalle(usuario, manda.ToShortDateString(), manda.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.listaMediosEncabezado = (List<E_Caja>)CajaPreResumenDetalle(usuario, manda.ToShortDateString(), manda.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                return View(ListadoCajaGeneral(usuario, manda.ToShortDateString(), manda.ToShortDateString()).Where(x => x.CodSede == sede).ToList());
            }

        }

        public ActionResult ListadoCajaAdmin(string fechaE = null, string fechaS = null, string CodUsu = null, int? Elimina = null, int? Evento = null, int? Detalle = null, string moneda = null, decimal? monto = null, decimal[] array = null, string FecDeposito = null)
        {

            string sede = Session["codSede"].ToString();
            string usuario = Session["UserID"].ToString();
            UsuarioServicioController us = new UsuarioServicioController();
            UtilitarioController uti = new UtilitarioController();

            E_Usuario usu = uti.ListadoUsuarioEspecialidadGeneral(usuario).FirstOrDefault();

            E_Usuario_Servicio use = us.ListaUsuarioServicio().Find(x => x.CodUsu == usuario);

            if (usu != null)
            {
                ViewBag.Listausuario = new SelectList(ListadoUsuario().Where(x => x.CodSede == sede), "CodUsu", "AliasCodDoc");
            }
            else
            {
                ViewBag.Listausuario = new SelectList(ListadoUsuarioxServicio(use.CodServ).Where(x => x.CodSede == sede), "CodUsu", "AliasCodDoc");
            }


            ViewBag.modal = "";
            UtilitarioController ma = new UtilitarioController();
            E_Master hor = ma.ListadoHoraServidor().FirstOrDefault();
            var validaFecha = hor.HoraServidor.AddDays(-1);
            ViewBag.mediosPago = new SelectList(ListadoMedioPago().ToList(), "CODMEDIOS", "DESCRIPCION");
            ViewBag.pago = null;

            ViewBag.Hoy = hor.HoraServidor.ToString("dd/MM/yyyy");
            ViewBag.usuario = usuario;

            ViewBag.modal = "";

            string Crea = Session["usuario"] + " " + hor.HoraServidor.ToString() + " " + Environment.MachineName;

            if (Detalle != null)
            {
                if (Detalle != 0)
                {
                    ViewBag.modal = "1";
                    ViewBag.Detalle = Detalle;
                    ViewBag.pago = (List<E_Caja>)ListaCajaPago().Where(x => x.CodCaja == Detalle && x.Estado == true).ToList();
                }
            }


            if (Evento == 3)
            {
                ViewBag.modal = "1";
                ViewBag.Detalle = Detalle;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cp = new SqlCommand("Usp_MtoCaja_Pago", con))
                    {
                        try
                        {
                            cp.CommandType = CommandType.StoredProcedure;

                            cp.Parameters.AddWithValue("@CodCaja", Detalle);
                            cp.Parameters.AddWithValue("@item", Elimina);
                            cp.Parameters.AddWithValue("@CODMEDIOS", "");
                            cp.Parameters.AddWithValue("@Importe", 0);
                            cp.Parameters.AddWithValue("@ImporteSoles", 0);
                            cp.Parameters.AddWithValue("@CodTipMon", "");
                            cp.Parameters.AddWithValue("@TipoCambio", 0);
                            cp.Parameters.AddWithValue("@Estado", "");
                            cp.Parameters.AddWithValue("@Crea", "");
                            cp.Parameters.AddWithValue("@Modifica", "");
                            cp.Parameters.AddWithValue("@Elimina", Crea);
                            cp.Parameters.AddWithValue("@Evento", "2");
                            cp.ExecuteNonQuery();

                            ViewBag.mensaje = "Se registro correctamente";


                        }
                        catch (Exception ex)
                        {
                            ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                            return View();
                        }
                        finally
                        {
                            con.Close();

                        }
                    }
                    ViewBag.fechaE = fechaE;
                    ViewBag.fechaS = fechaS;
                    DateTime fec1 = DateTime.Parse(fechaE);
                    DateTime fec2 = DateTime.Parse(fechaS);
                    ViewBag.CajaPreResumen = (List<E_Caja>)CajaPreResumen(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                    ViewBag.CajaPreResumenDetalle = (List<E_Caja>)CajaPreResumenDetalle(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                    ViewBag.listaMediosPago = (List<E_Caja>)ListadoMediosPago(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                    ViewBag.listaMediosEncabezado = (List<E_Caja>)ListaMediosPagoEncabezado(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                    ViewBag.pago = (List<E_Caja>)ListaCajaPago().Where(x => x.CodCaja == Detalle && x.Estado == true).ToList();
                    return View(ListadoCajaGeneral(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList());
                }
            }

            if (Evento == 4)
            {
                ViewBag.modal = "1";
                ViewBag.Detalle = Detalle;
                int it = 1;
                E_Caja cajj = ListaCajaPago().Where(x => x.CodCaja == Detalle && x.Estado == true).LastOrDefault();
                if (cajj == null)
                {
                    it = 1;
                }
                else
                {
                    it = cajj.item + 1;
                }
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    E_Caja eva = ListaCajaPecioTotal((int)Detalle).FirstOrDefault();
                    E_Caja evaPago = ListaCajaPagoTotal((int)Detalle).FirstOrDefault();
                    decimal tot = 0;
                    if (evaPago == null)
                    {
                        tot = 0;
                    }
                    else
                    {
                        tot = evaPago.Total;
                    }
                    decimal total = tot + (decimal)monto;
                    if (eva.Total < total)
                    {
                        ViewBag.mensaje = "El monto asignado excede al total de la caja";
                    }
                    else
                    {
                        con.Open();
                        using (SqlCommand cp = new SqlCommand("Usp_MtoCaja_Pago", con))
                        {
                            try
                            {
                                cp.CommandType = CommandType.StoredProcedure;

                                cp.Parameters.AddWithValue("@CodCaja", Detalle);
                                cp.Parameters.AddWithValue("@item", it);
                                cp.Parameters.AddWithValue("@CODMEDIOS", moneda);
                                cp.Parameters.AddWithValue("@Importe", monto);
                                cp.Parameters.AddWithValue("@ImporteSoles", monto);
                                cp.Parameters.AddWithValue("@CodTipMon", "TM001");
                                cp.Parameters.AddWithValue("@TipoCambio", 0);
                                cp.Parameters.AddWithValue("@Estado", "1");
                                cp.Parameters.AddWithValue("@Crea", Crea);
                                cp.Parameters.AddWithValue("@Modifica", "");
                                cp.Parameters.AddWithValue("@Elimina", "");
                                cp.Parameters.AddWithValue("@Evento", "1");
                                cp.ExecuteNonQuery();

                                ViewBag.mensaje = "Se registro correctamente";


                            }
                            catch (Exception ex)
                            {
                                ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                                return View();
                            }
                            finally
                            {
                                con.Close();

                            }
                        }
                    }
                    ViewBag.fechaE = fechaE;
                    ViewBag.fechaS = fechaS;
                    DateTime fec1 = DateTime.Parse(fechaE);
                    DateTime fec2 = DateTime.Parse(fechaS);
                    ViewBag.CajaPreResumen = (List<E_Caja>)CajaPreResumen(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                    ViewBag.CajaPreResumenDetalle = (List<E_Caja>)CajaPreResumenDetalle(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                    ViewBag.listaMediosPago = (List<E_Caja>)ListadoMediosPago(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                    ViewBag.listaMediosEncabezado = (List<E_Caja>)ListaMediosPagoEncabezado(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                    ViewBag.pago = (List<E_Caja>)ListaCajaPago().Where(x => x.CodCaja == Detalle && x.Estado == true).ToList();
                    return View(ListadoCajaGeneral(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList());
                }
            }

            if (Evento == 5)
            {
                ViewBag.modal = "1";
                ViewBag.Detalle = Detalle;

                E_Caja eva = ListaCajaPecioTotal((int)Detalle).FirstOrDefault();
                E_Caja evaPago = ListaCajaPagoTotal((int)Detalle).FirstOrDefault();
                decimal tot = 0;
                if (evaPago == null)
                {
                    tot = 0;
                }
                else
                {
                    tot = evaPago.Total;
                }
                if (eva.Total != tot)
                {
                    ViewBag.mensaje = "Los montos no equivalen al total de caja";
                }
                else
                {
                    ViewBag.modal = "";
                    ViewBag.Detalle = 0;
                }
                ViewBag.fechaE = fechaE;
                ViewBag.fechaS = fechaS;
                DateTime fec1 = DateTime.Parse(fechaE);
                DateTime fec2 = DateTime.Parse(fechaS);
                ViewBag.CajaPreResumen = (List<E_Caja>)CajaPreResumen(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.CajaPreResumenDetalle = (List<E_Caja>)CajaPreResumenDetalle(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.listaMediosPago = (List<E_Caja>)ListadoMediosPago(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.listaMediosEncabezado = (List<E_Caja>)ListaMediosPagoEncabezado(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.pago = (List<E_Caja>)ListaCajaPago().Where(x => x.CodCaja == Detalle && x.Estado == true).ToList();
                return View(ListadoCajaGeneral(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList());

            }
            if (Evento == 6)
            {

                ViewBag.fechaE = fechaE;
                ViewBag.fechaS = fechaS;
                DateTime fec1 = DateTime.Parse(fechaE);
                DateTime fec2 = DateTime.Parse(fechaS);

                var listaCajaPreResumen = (List<E_Caja>)CajaPreResumen(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                var listaCajaPreResumenDetalle = (List<E_Caja>)CajaPreResumenDetalle(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();

                int cuenta = 0;
                decimal diferencia = 0;

                UtilitarioController ut = new UtilitarioController();
                E_Master hora = ut.ListadoHoraServidor().FirstOrDefault();

                DateTime horaActual = hora.HoraServidor;
                string horaF = horaActual.ToString("dd/MM/yyyy H:mm:ss");
                string modifica = Session["usuario"] + " " + horaF + " " + Environment.MachineName;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                    con.Open();
                    SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        var evalua = ListadoCajaResumen().ToList();
                        foreach (var lista in listaCajaPreResumen)
                        {
                            var consulta = evalua.Find(x => x.FechaCaja == lista.FechaEmision && x.CodUsu == CodUsu);

                            if (consulta != null)
                            {
                                if (consulta.Estado == true)
                                {
                                    break;
                                }
                            }

                            if (lista.Total >= array[cuenta])
                            {
                                diferencia = lista.Total - array[cuenta];
                            }
                            else
                            {
                                diferencia = array[cuenta] - lista.Total;
                            }

                            using (SqlCommand ca = new SqlCommand("Usp_MtoCajaResumen", con, tr))
                            {

                                ca.CommandType = CommandType.StoredProcedure;

                                ca.Parameters.AddWithValue("@CodCajRes", "");
                                ca.Parameters.AddWithValue("@Usuario", CodUsu);
                                ca.Parameters.AddWithValue("@FechaDeposito", FecDeposito);
                                ca.Parameters.AddWithValue("@FechaCaja", lista.FechaEmision);
                                ca.Parameters.AddWithValue("@TipoPago", lista.TipoPago);
                                ca.Parameters.AddWithValue("@TipoCambio", 0);
                                ca.Parameters.AddWithValue("@TotalDolares", 0);
                                ca.Parameters.AddWithValue("@TotalUsuario", array[cuenta]);
                                ca.Parameters.AddWithValue("@TotalSistema", lista.Total);
                                ca.Parameters.AddWithValue("@Diferencia", diferencia);

                                if (usu != null)
                                {
                                    ca.Parameters.AddWithValue("@Estado", "1");
                                }
                                else
                                {
                                    ca.Parameters.AddWithValue("@Estado", "0");
                                }
                                ca.Parameters.AddWithValue("@Crea", modifica);
                                ca.Parameters.AddWithValue("@Modifica", "");
                                ca.Parameters.AddWithValue("@Elimina", "");
                                int Resu = 0;
                                if (consulta == null)
                                {
                                    ca.Parameters.AddWithValue("@Evento", "1");
                                    Resu = (int)ca.ExecuteScalar();
                                }
                                else
                                {
                                    ca.Parameters.AddWithValue("@Evento", "2");
                                    ca.ExecuteNonQuery();

                                    using (SqlCommand caj = new SqlCommand("Usp_MtoCajaResumenDetalle", con, tr))
                                    {

                                        caj.CommandType = CommandType.StoredProcedure;
                                        caj.Parameters.AddWithValue("@CodCajRes", consulta.CodCajRes);
                                        caj.Parameters.AddWithValue("@CodMedios", "");
                                        caj.Parameters.AddWithValue("@DescriMedio", "");
                                        caj.Parameters.AddWithValue("@Total", 0);
                                        caj.Parameters.AddWithValue("@Crea", "");
                                        caj.Parameters.AddWithValue("@Modifica", "");
                                        caj.Parameters.AddWithValue("@Elimina", "");
                                        caj.Parameters.AddWithValue("@Evento", "2");

                                        caj.ExecuteNonQuery();

                                    }
                                }

                                var listaDetalle = listaCajaPreResumenDetalle.Where(x => x.FechaEmision.ToShortDateString() == lista.FechaEmision.ToShortDateString()).ToList();
                                foreach (var li in listaDetalle)
                                {

                                    using (SqlCommand caj = new SqlCommand("Usp_MtoCajaResumenDetalle", con, tr))
                                    {

                                        caj.CommandType = CommandType.StoredProcedure;
                                        if (consulta == null)
                                        {
                                            caj.Parameters.AddWithValue("@CodCajRes", Resu);
                                        }
                                        else
                                        {
                                            caj.Parameters.AddWithValue("@CodCajRes", consulta.CodCajRes);
                                        }
                                        caj.Parameters.AddWithValue("@CodMedios", li.CODMEDIOS);
                                        caj.Parameters.AddWithValue("@DescriMedio", li.DescTipMon);
                                        caj.Parameters.AddWithValue("@Total", li.Total);
                                        caj.Parameters.AddWithValue("@Crea", modifica);
                                        caj.Parameters.AddWithValue("@Modifica", "");
                                        caj.Parameters.AddWithValue("@Elimina", "");
                                        caj.Parameters.AddWithValue("@Evento", "1");

                                        caj.ExecuteNonQuery();

                                    }
                                }
                            }
                            cuenta++;
                        }

                        tr.Commit();
                    }
                    catch (Exception e)
                    {
                        tr.Rollback();
                        ViewBag.mensaje = e.Message;
                    }
                    finally
                    {
                        con.Close();
                    }

                }

                ViewBag.mensaje = "Registro exitoso";

                ViewBag.CajaPreResumen = (List<E_Caja>)CajaPreResumen(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.CajaPreResumenDetalle = (List<E_Caja>)CajaPreResumenDetalle(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.listaMediosPago = (List<E_Caja>)ListadoMediosPago(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.listaMediosEncabezado = (List<E_Caja>)ListaMediosPagoEncabezado(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.pago = (List<E_Caja>)ListaCajaPago().Where(x => x.CodCaja == Detalle && x.Estado == true).ToList();
                return View(ListadoCajaGeneral(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList());

            }



            ViewBag.Hoy = hor.HoraServidor.ToString("dd/MM/yyyy");


            if (fechaS != null && fechaE != null && CodUsu != null)
            {
                ViewBag.fechaE = fechaE;
                ViewBag.fechaS = fechaS;
                ViewBag.usuario = CodUsu;
                DateTime fec1 = DateTime.Parse(fechaE);
                DateTime fec2 = DateTime.Parse(fechaS);
                ViewBag.listaMediosPago = (List<E_Caja>)ListadoMediosPago(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();

                ViewBag.CajaPreResumen = (List<E_Caja>)CajaPreResumen(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.CajaPreResumenDetalle = (List<E_Caja>)CajaPreResumenDetalle(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();
                ViewBag.listaMediosEncabezado = (List<E_Caja>)ListaMediosPagoEncabezado(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList();

                return View(ListadoCajaGeneral(CodUsu, fec1.ToShortDateString(), fec2.ToShortDateString()).Where(x => x.CodSede == sede).ToList());
            }
            else
            {
                ViewBag.fechaE = hor.HoraServidor.ToString("dd/MM/yyyy");
                ViewBag.fechaS = hor.HoraServidor.ToString("dd/MM/yyyy");
                ViewBag.CajaPreResumen = null;
                ViewBag.CajaPreResumenDetalle = null;
                ViewBag.listaMediosEncabezado = null;
                ViewBag.listaMediosPago = null;
                return View();
            }

        }

        public ActionResult RegistrarCajaResumen(E_Caja c)
        {
            string sede = Session["codSede"].ToString();
            var listaCajaPreResumen = (List<E_Caja>)CajaPreResumen(c.CodUsu, c.fechaI, c.fechaF).Where(x => x.CodSede == sede).ToList();
            var listaCajaPreResumenDetalle = (List<E_Caja>)CajaPreResumenDetalle(c.CodUsu, c.fechaI, c.fechaF).Where(x => x.CodSede == sede).ToList();

            string usuario = Session["UserID"].ToString();
            int cuenta = 0;
            decimal diferencia = 0;

            UtilitarioController ut = new UtilitarioController();
            E_Usuario usu = ut.ListadoUsuarioEspecialidadGeneral(usuario).FirstOrDefault();
            E_Master hora = ut.ListadoHoraServidor().FirstOrDefault();

            DateTime horaActual = hora.HoraServidor;
            string horaF = horaActual.ToString("dd/MM/yyyy H:mm:ss");
            string modifica = Session["usuario"] + " " + horaF + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    var evalua = ListadoCajaResumen().ToList();
                    foreach (var lista in listaCajaPreResumen)
                    {
                        var consulta = evalua.Find(x => x.FechaCaja == lista.FechaEmision && x.CodUsu == c.CodUsu);

                        if (consulta != null)
                        {
                            if (consulta.Estado == true)
                            {
                                break;
                            }
                        }

                        if (lista.Total >= c.array[cuenta])
                        {
                            diferencia = lista.Total - c.array[cuenta];
                        }
                        else
                        {
                            diferencia = c.array[cuenta] - lista.Total;
                        }

                        using (SqlCommand ca = new SqlCommand("Usp_MtoCajaResumen", con, tr))
                        {

                            ca.CommandType = CommandType.StoredProcedure;

                            ca.Parameters.AddWithValue("@CodCajRes", "");
                            ca.Parameters.AddWithValue("@Usuario", c.CodUsu);
                            ca.Parameters.AddWithValue("@FechaDeposito", c.FecNac);
                            ca.Parameters.AddWithValue("@FechaCaja", lista.FechaEmision);
                            ca.Parameters.AddWithValue("@TipoPago", lista.TipoPago);
                            ca.Parameters.AddWithValue("@TipoCambio", 0);
                            ca.Parameters.AddWithValue("@TotalDolares", 0);
                            ca.Parameters.AddWithValue("@TotalUsuario", c.array[cuenta]);
                            ca.Parameters.AddWithValue("@TotalSistema", lista.Total);
                            ca.Parameters.AddWithValue("@Diferencia", diferencia);

                            if (usu != null)
                            {
                                ca.Parameters.AddWithValue("@Estado", "1");
                            }
                            else
                            {
                                ca.Parameters.AddWithValue("@Estado", "0");
                            }
                            ca.Parameters.AddWithValue("@Crea", modifica);
                            ca.Parameters.AddWithValue("@Modifica", "");
                            ca.Parameters.AddWithValue("@Elimina", "");
                            int Resu = 0;
                            if (consulta == null)
                            {
                                ca.Parameters.AddWithValue("@Evento", "1");
                                Resu = (int)ca.ExecuteScalar();
                            }
                            else
                            {
                                ca.Parameters.AddWithValue("@Evento", "2");
                                ca.ExecuteNonQuery();

                                using (SqlCommand caj = new SqlCommand("Usp_MtoCajaResumenDetalle", con, tr))
                                {

                                    caj.CommandType = CommandType.StoredProcedure;
                                    caj.Parameters.AddWithValue("@CodCajRes", consulta.CodCajRes);
                                    caj.Parameters.AddWithValue("@CodMedios", "");
                                    caj.Parameters.AddWithValue("@DescriMedio", "");
                                    caj.Parameters.AddWithValue("@Total", 0);
                                    caj.Parameters.AddWithValue("@Crea", "");
                                    caj.Parameters.AddWithValue("@Modifica", "");
                                    caj.Parameters.AddWithValue("@Elimina", "");
                                    caj.Parameters.AddWithValue("@Evento", "2");

                                    caj.ExecuteNonQuery();

                                }
                            }

                            var listaDetalle = listaCajaPreResumenDetalle.Where(x => x.FechaEmision.ToShortDateString() == lista.FechaEmision.ToShortDateString()).ToList();
                            foreach (var li in listaDetalle)
                            {

                                using (SqlCommand caj = new SqlCommand("Usp_MtoCajaResumenDetalle", con, tr))
                                {

                                    caj.CommandType = CommandType.StoredProcedure;
                                    if (consulta == null)
                                    {
                                        caj.Parameters.AddWithValue("@CodCajRes", Resu);
                                    }
                                    else
                                    {
                                        caj.Parameters.AddWithValue("@CodCajRes", consulta.CodCajRes);
                                    }
                                    caj.Parameters.AddWithValue("@CodMedios", li.CODMEDIOS);
                                    caj.Parameters.AddWithValue("@DescriMedio", li.DescTipMon);
                                    caj.Parameters.AddWithValue("@Total", li.Total);
                                    caj.Parameters.AddWithValue("@Crea", modifica);
                                    caj.Parameters.AddWithValue("@Modifica", "");
                                    caj.Parameters.AddWithValue("@Elimina", "");
                                    caj.Parameters.AddWithValue("@Evento", "1");

                                    caj.ExecuteNonQuery();

                                }
                            }
                        }
                        cuenta++;
                    }

                    tr.Commit();
                }
                catch (Exception e)
                {
                    tr.Rollback();
                    ViewBag.mensaje = e.Message;
                }
                finally
                {
                    con.Close();
                }

            }

            Response.Write("<script language=javascript> window.history.go(-1) </script>");
            Response.Write("<script language=javascript> alert('Registro exitoso'); </script>");

            return null;

        }

        public ActionResult EliminarCaja(int id)
        {
            UtilitarioController ut = new UtilitarioController();
            E_Master hora = ut.ListadoHoraServidor().FirstOrDefault();

            DateTime horaActual = hora.HoraServidor;
            string horaF = horaActual.ToString("dd/MM/yyyy H:mm:ss");
            string FechaHoy = horaActual.ToString("dd/MM/yyyy");

            string modifica = Session["usuario"] + " " + horaF + Environment.MachineName;
            string usuario = Session["UserID"].ToString();

            var listaDetalle = (List<E_CajaDetalle>)ListadoCajaDetalle().Where(x => x.CodCaja != id).ToList();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();

                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    using (SqlCommand ca = new SqlCommand("Usp_MtoCaja", con, tr))
                    {

                        ca.CommandType = CommandType.StoredProcedure;
                        ca.Parameters.AddWithValue("@CodCaja", id);
                        ca.Parameters.AddWithValue("@CodSede", "");
                        ca.Parameters.AddWithValue("@CodCue", "");
                        ca.Parameters.AddWithValue("@CodDocSerie", "");
                        ca.Parameters.AddWithValue("@Serie", "");
                        ca.Parameters.AddWithValue("@NumDoc", "");
                        ca.Parameters.AddWithValue("@FechaEmision", "");
                        ca.Parameters.AddWithValue("@HoraEmision", "");
                        ca.Parameters.AddWithValue("@Historia", "");
                        ca.Parameters.AddWithValue("@NomPac", "");
                        ca.Parameters.AddWithValue("@DirecPac", "");
                        ca.Parameters.AddWithValue("@Ruc", "");
                        ca.Parameters.AddWithValue("@RazonSocial", "");
                        ca.Parameters.AddWithValue("@DirRazSoc", "");
                        ca.Parameters.AddWithValue("@CodCatPac", "");
                        ca.Parameters.AddWithValue("@Estado", "");
                        ca.Parameters.AddWithValue("@SubTotal", 0);
                        ca.Parameters.AddWithValue("@Igv", 0);
                        ca.Parameters.AddWithValue("@Total", 0);
                        ca.Parameters.AddWithValue("@UsuCrea", "");
                        ca.Parameters.AddWithValue("@UsuAnula", usuario);
                        ca.Parameters.AddWithValue("@FechaAnula", horaF);
                        ca.Parameters.AddWithValue("@TipoPago", "");
                        ca.Parameters.AddWithValue("@CodRazSoc", "");
                        ca.Parameters.AddWithValue("@CodTipMon", "");
                        ca.Parameters.AddWithValue("@TipoCambio", 0);
                        ca.Parameters.AddWithValue("@Obser", "");
                        ca.Parameters.AddWithValue("@TazaIgv", 0);
                        ca.Parameters.AddWithValue("@AutorizaAnu", "");
                        ca.Parameters.AddWithValue("@PorAnular", "");
                        ca.Parameters.AddWithValue("@SecVenta", "");
                        ca.Parameters.AddWithValue("@Crea", "");
                        ca.Parameters.AddWithValue("@Modifica", "");
                        ca.Parameters.AddWithValue("@Elimina", modifica);
                        ca.Parameters.AddWithValue("@Evento", "2");

                        ca.ExecuteNonQuery();

                        using (SqlCommand da = new SqlCommand("Usp_MtoCuentas", con, tr))
                        {

                            da.CommandType = CommandType.StoredProcedure;
                            da.Parameters.AddWithValue("@CodCue", "");
                            da.Parameters.AddWithValue("@CodSede", "");
                            da.Parameters.AddWithValue("@Historia", 0);
                            da.Parameters.AddWithValue("@CodcatPac", "");
                            da.Parameters.AddWithValue("@STotCue", 0);
                            da.Parameters.AddWithValue("@IgvCue", 0);
                            da.Parameters.AddWithValue("@TotCue", 0);
                            da.Parameters.AddWithValue("@FecCrea", "");
                            da.Parameters.AddWithValue("@FecAnul", "");
                            da.Parameters.AddWithValue("@EstCue", "1");
                            da.Parameters.AddWithValue("@EstGene", "");
                            da.Parameters.AddWithValue("@SecFact", id);
                            da.Parameters.AddWithValue("@Usuario", "");
                            da.Parameters.AddWithValue("@UsuarioAnula", "");
                            da.Parameters.AddWithValue("@Crea", "");
                            da.Parameters.AddWithValue("@Modifica", "");
                            da.Parameters.AddWithValue("@Elimina", "");
                            da.Parameters.AddWithValue("@Evento", "5");

                            da.ExecuteNonQuery();

                        }
                    }
                    tr.Commit();
                }
                catch (Exception e)
                {
                    tr.Rollback();
                    ViewBag.mensaje = e.Message;
                }
                finally
                {
                    con.Close();
                }

            }

            return RedirectToAction("ListadoCaja");

        }

        public List<E_Caja> Usp_CajaPaciente(int CodCaja)
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Caja_InfoPaciente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodCaja", CodCaja);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja usu = new E_Caja();
                            usu.Historia = dr.GetInt32(0);
                            usu.NomPac = dr.GetString(1);
                            usu.Edad = dr.GetInt32(2);
                            usu.NumDoc = dr.GetString(3);
                            usu.FecNac = dr.GetDateTime(4);
                            Lista.Add(usu);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public List<E_Servicios> Usp_DetalleServicioTicket(int CodCaja)
        {
            List<E_Servicios> Lista = new List<E_Servicios>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ImprimeTicketDetalle", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodCaja", CodCaja);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Servicios ser = new E_Servicios();
                            ser.CodServ = dr.GetString(0);
                            ser.NomServ = dr.GetString(1);
                            Lista.Add(ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult ImprimirTicket(int CodCaja)
        {
            var cm = ImprimeCajaCabecera(CodCaja);
            var cd = ImprimeCajaDetalle(CodCaja);
            var pac = Usp_CajaPaciente(CodCaja);
            var ser = Usp_DetalleServicioTicket(CodCaja).FirstOrDefault();
            var Doc = usp_Ticket_TipoDoc(CodCaja);
            //    Response.Write("<script langu age='JavaScript' type ='text/javascript'>" +
            //" window.open('../../Cuentas/VerificaCuenta/" + c.CodCue + "', '_blank'); </script>"); 
            var DatosGenerales = new DatosGeneralesController().Getdatogenerales();
            ViewBag.DatosGenerales = DatosGenerales; 
            ViewBag.TipoDoc = Doc; 
            ViewBag.paciente = pac;
            ViewBag.cabecera = cm;
            ViewBag.detalle = cd;
            ViewBag.servicio = ser.NomServ;
            return View();
        }

        public ActionResult ImprimirCaja(string id, int CodCaja)
        {

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Impresion"), "ImpresionFactura.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("RegistrarCaja");
            }

            List<E_Caja> cm = new List<E_Caja>();
            List<E_CajaDetalle> cd = new List<E_CajaDetalle>();

            cm = ImprimeCajaCabecera(CodCaja);
            cd = ImprimeCajaDetalle(CodCaja);

            ReportDataSource rd = new ReportDataSource("DataSet1", cm);
            ReportDataSource cdr = new ReportDataSource("DataSet2", cd);

            lr.DataSources.Add(rd);
            lr.DataSources.Add(cdr);

            string reportType = id;
            string mimeType;
            string encoding;
            string FileNameExtension;


            //AQUI SE CAMBIA EL TAMAÑO DE LA HORA--> PARA MAS INFO (AVERIGUA EN GOOGLE)
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

        public List<E_Caja> Usp_CorteCajaDiario(string usuario, DateTime fecha, string sede)
        {
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_CorteCajaDiario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    cmd.Parameters.AddWithValue("@CodUsuario", usuario);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja ser = new E_Caja();
                            ser.id = dr.GetInt32(0);
                            ser.FechaCaja = dr.GetDateTime(1);
                            ser.horaInicio = dr.GetTimeSpan(2);
                            ser.horaFin = dr.GetTimeSpan(3);
                            ser.corte = dr.GetInt32(4);
                            ser.nroTickets = dr.GetInt32(5);
                            ser.anuladas = dr.GetInt32(6);
                            ser.TotalSistema = dr.GetDecimal(7);
                            ser.TotalUsuario = dr.GetDecimal(8);
                            ser.Diferencia = dr.GetDecimal(9);
                            ser.UsuCrea = dr.GetString(10);
                            ser.CodSede = dr.GetString(11);
                            ser.Estado = dr.GetBoolean(12);
                            Lista.Add(ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Caja> Usp_DataCorteCaja(DateTime fecha, string CodUsuario, TimeSpan HoraInicio, TimeSpan HoraFin, string sede)
        {
            
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_DataCorteCaja", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    cmd.Parameters.AddWithValue("@CodUsu", CodUsuario);
                    cmd.Parameters.AddWithValue("@HoraInicio", HoraInicio);
                    cmd.Parameters.AddWithValue("@HoraFin", HoraFin);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {                            
                            E_Caja ser = new E_Caja();
                            ser.Total = dr["total"] is DBNull ? 0 : dr.GetDecimal(0); 
                            ser.nroTickets = dr["nroTickets"] is DBNull ? 0 : dr.GetInt32(1);
                            ser.anuladas = dr["canceladas"] is DBNull ? 0 : dr.GetInt32(2); 
                            Lista.Add(ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult CorteCaja()
        {
            string sede = Session["codSede"].ToString();
            UtilitarioController ut = new UtilitarioController();
            E_Master hora = ut.ListadoHoraServidor().FirstOrDefault();
            string usuario = Session["UserID"].ToString();
            var evalua = Usp_CorteCajaDiario(usuario, hora.HoraServidor, sede).FirstOrDefault();
            if (evalua == null)
            {
                ViewBag.horaInicio = TimeSpan.Parse("00:00:00");
                ViewBag.corte = 1;
            }
            else {
                ViewBag.horaInicio = evalua.horaFin;
                ViewBag.corte = evalua.corte + 1;
            }
            return View();
        }

        public ActionResult ListadoCorteCaja(DateTime? fecha = null, string CodUsu = null)
        {
            UtilitarioController ut = new UtilitarioController();
            E_Master hora = ut.ListadoHoraServidor().FirstOrDefault();
            UsuarioController usu = new UsuarioController();
            ViewBag.ListaUsuario = new SelectList(usu.listaUsuarios().Where(x => x.EstUsu == true).ToList(), "codUsu", "aliasUsu");

            string sede = Session["codSede"].ToString();

            if (fecha != null && CodUsu != null)
            {
                ViewBag.fecha = DateTime.Parse(fecha.ToString()).ToShortDateString();
                ViewBag.data = Usp_CorteCajaDiario(CodUsu, DateTime.Parse(fecha.ToString()),sede);
            }
            else
            {                
                ViewBag.fecha = hora.HoraServidor.ToShortDateString();
                ViewBag.data = null;
            }

            return View();
        }

        [HttpPost]
        public JsonResult CorteCaja(decimal totalUsuario, TimeSpan horaInicio, int corte)
        {
            string usuario = Session["UserID"].ToString();
            string sede = Session["codSede"].ToString();
            UtilitarioController ut = new UtilitarioController();
            E_Master hora = ut.ListadoHoraServidor().FirstOrDefault();
            DateTime fecha = hora.HoraServidor.Date;
            TimeSpan horaFinal = hora.HoraServidor.TimeOfDay;
            var data = Usp_DataCorteCaja(fecha, usuario, horaInicio, horaFinal, sede).FirstOrDefault();
            decimal diferencia = totalUsuario - data.Total;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cp = new SqlCommand("usp_registraCorte", con))
                {
                    try
                    {
                        cp.CommandType = CommandType.StoredProcedure;

                        cp.Parameters.AddWithValue("@fecha", fecha);
                        cp.Parameters.AddWithValue("@horaInicio", horaInicio);
                        cp.Parameters.AddWithValue("@horaFin", horaFinal);
                        cp.Parameters.AddWithValue("@corte", corte);
                        cp.Parameters.AddWithValue("@nroTickets", data.nroTickets);
                        cp.Parameters.AddWithValue("@anulados", data.anuladas);
                        cp.Parameters.AddWithValue("@totalSistema", data.Total);
                        cp.Parameters.AddWithValue("@totalUsuario", totalUsuario);
                        cp.Parameters.AddWithValue("diferencia", diferencia);
                        cp.Parameters.AddWithValue("@usuario", usuario);
                        cp.Parameters.AddWithValue("@sede", sede);
                        cp.ExecuteNonQuery();
                        
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
            }                
        }





        public ActionResult ImprimirCierreCajaPorCajera()
        {
            ViewBag.ListaCierreCaja = ListaImprimeCierreCajaPorUsuario();
            
            return View();
        }



        public List<E_Caja> ListaImprimeCierreCajaPorUsuario()
        {

            string Usuario = Session["UserID"].ToString();
            List<E_Caja> Lista = new List<E_Caja>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ConsolidadoCajeroxDoc", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Usu", Usuario);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Caja ser = new E_Caja();
                            
                            ser.NomSede = dr["Sede"].ToString();
                            ser.Usuario = dr["Usuario"].ToString();
                            ser.FechaDia = dr["Fecha"].ToString();
                            ser.Documento = dr["Doc"].ToString();
                            ser.Serie = dr["Serie"].ToString();
                            ser.DocIni = dr["DocIni"].ToString();
                            ser.DocFin = dr["DocFin"].ToString();
                            ser.SubTotal = Convert.ToDecimal(dr["SubTot"].ToString());
                            ser.Igv = Convert.ToDecimal(dr["Igv"].ToString());
                            ser.Total = Convert.ToDecimal(dr["TOTAL"].ToString());
                            Lista.Add(ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

    }
}