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
    public class TipoMonedaController : Controller
    {
        // GET: TipoMoneda
        public ActionResult RegistrarTipoMoneda()
        {

            return View();
        }

        [HttpPost]
        public ActionResult RegistrarTipoMoneda(E_TipoMoneda ETipM)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                using (SqlCommand cmd = new SqlCommand("Usp_MtoTipoMoneda", con,tr))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@CodTipMon", "");
                        cmd.Parameters.AddWithValue("@DescTipMon", ETipM.DescTipMon.ToUpper());
                        cmd.Parameters.AddWithValue("@EstTipMon", ETipM.EstTipMon);
                        cmd.Parameters.AddWithValue("@TipoCambio", ETipM.TipoCambio);
                        cmd.Parameters.AddWithValue("@Evento", 1);
                        cmd.CommandType = CommandType.StoredProcedure;

                        string codigo = cmd.ExecuteScalar().ToString();

                        SqlCommand cmd1 = new SqlCommand("Usp_Mantenimiento_TipoCambio", con, tr); 
                       
                        cmd1.Parameters.AddWithValue("@IdTipoC", "");
                        cmd1.Parameters.AddWithValue("@CodTipMon",codigo);
                        cmd1.Parameters.AddWithValue("@TipoCambio", ETipM.TipoCambio);
                        cmd1.Parameters.AddWithValue("@Fecha", ETipM.fecha);
                        cmd1.Parameters.AddWithValue("@Evento", 1);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.ExecuteNonQuery();
                       

                        tr.Commit();
                        ViewBag.Mensaje = "1";
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        ViewBag.Mensaje = "3";
                        return View(ETipM);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListaTipoMoneda");
            }
        }

        public List<E_TipoMoneda> Consultar_TipoCambio(DateTime date, string CodTipMoney)
        {
            List<E_TipoMoneda> Lista = new List<E_TipoMoneda>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Consultar_TipoCambio", con))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@fecha", date);
                        cmd.Parameters.AddWithValue("@CodTipMon", CodTipMoney);

                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlDataReader dr = cmd.ExecuteReader();


                        while (dr.Read())
                        {
                            if (dr.GetInt32(0) == 0)
                            {
                                Lista = null;
                                return Lista;
                            }
                            else
                            {
                                E_TipoMoneda Money = new E_TipoMoneda();
                                Money.IdTipoCambio = dr.GetInt32(0);
                                Money.CodTipMon = dr.GetString(1);
                                Money.TipoCambio = dr.GetDecimal(2);
                                Money.fecha = dr.GetDateTime(3);
                                Lista.Add(Money);
                            }
                        }


                        cmd.Dispose();

                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();

                    }
                    finally
                    {
                        con.Close();

                    }
                }

            }
            return Lista;
        }


        public ActionResult ModificarTipoMoneda(string Id)
        {

            var lista = (from x in ListadoTipoMoneda1() where x.CodTipMon == Id select x).FirstOrDefault();
            return View(lista);
        }

        [HttpPost]
        public ActionResult ModificarTipoMoneda(E_TipoMoneda ETipM)
        {

            if (ETipM.Evento == "1")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_TipoCambio", con))
                    {
                        try
                        {
                            decimal example = ETipM.TipoCambio;
                            decimal output = Math.Round(example, 3);
                            cmd.Parameters.AddWithValue("@IdTipoC", "");
                            cmd.Parameters.AddWithValue("@CodTipMon", ETipM.CodTipMon);
                            cmd.Parameters.AddWithValue("@TipoCambio", output);
                            cmd.Parameters.AddWithValue("@Fecha", ETipM.fecha);
                            cmd.Parameters.AddWithValue("@Evento", 1);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            ViewBag.Mensaje = "Se registro Satisfactoriamente";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                            return View(ETipM);
                        }
                        finally
                        {
                            con.Close();

                        }
                    }
                    return RedirectToAction("ListaTipoMoneda");
                }

            }
            else if (ETipM.Evento == "2")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                  
                    using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_TipoCambio", con))
                    {
                        try
                        {
                            decimal example = ETipM.TipoCambio;
                            decimal output = Math.Round(example, 3);
                            cmd.Parameters.AddWithValue("@IdTipoC", ETipM.IdTipoCambio);
                            cmd.Parameters.AddWithValue("@CodTipMon", ETipM.CodTipMon);
                            cmd.Parameters.AddWithValue("@TipoCambio", output);
                            cmd.Parameters.AddWithValue("@Fecha", ETipM.fecha);
                            cmd.Parameters.AddWithValue("@Evento", 2);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            ViewBag.Mensaje = "Se Modifico Satisfactoriamente";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Mensaje = "Ocurrio algun error al modificar : " + ex.Message.ToString();
                            return View(ETipM);
                        }
                        finally
                        {
                            con.Close();

                        }
                    }
                    return RedirectToAction("ListaTipoMoneda");
                }

            }
            return RedirectToAction("ListaTipoMoneda");


        }


        public ActionResult ListaTipoMoneda()
        {
            //if (Session["UserID"] != null)
            //{
            return View(ListadoTipoMoneda1());
            //}
            //else
            //{
            //    return RedirectToAction("../Login/Index");
            //}
        }

        public List<E_TipoMoneda> ListadoTipoMoneda1()
        {
            List<E_TipoMoneda> Lista = new List<E_TipoMoneda>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_General_Tipo_Cambio", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_TipoMoneda ETipM = new E_TipoMoneda();

                            ETipM.CodTipMon = dr.GetString(0);
                            ETipM.DescTipMon = dr.GetString(1);
                            ETipM.EstTipMon = dr.GetBoolean(2);
                            ETipM.TipoCambio = (dr["TipoCambio"] is DBNull) ? 0 : (decimal)dr["TipoCambio"];
                            ETipM.fecha = (dr["Fecha"] == DBNull.Value) ? (DateTime?)null : ((DateTime)dr["Fecha"]);
                            ETipM.fechaParse = (dr["Fecha"] == DBNull.Value) ? "" : (dr.GetDateTime(4).ToShortDateString());
                            Lista.Add(ETipM);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_TipoMoneda> ListadoTipoMoneda()
        {
            List<E_TipoMoneda> Lista = new List<E_TipoMoneda>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Tipo_Moneda", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_TipoMoneda ETipM = new E_TipoMoneda();

                            ETipM.CodTipMon = dr.GetString(0);
                            ETipM.DescTipMon = dr.GetString(1);
                            ETipM.EstTipMon = dr.GetBoolean(2);
                            ETipM.TipoCambio = dr.GetDecimal(3);

                            Lista.Add(ETipM);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public JsonResult ObtenerTipoCambio(string fecha, string CodTipoMoneda)
        {
            if (fecha != null && CodTipoMoneda != null)
            {
                var Result = Consultar_TipoCambio(Convert.ToDateTime(fecha), CodTipoMoneda);
                if (Result != null)
                {
                    return Json(Result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var valor = false;
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            return null;


        }

    }
}