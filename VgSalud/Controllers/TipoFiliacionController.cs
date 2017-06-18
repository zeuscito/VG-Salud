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
    public class TipoFiliacionController : Controller
    {
     

        // GET: TipoFiliacion
        public ActionResult RegistrarTipoFiliacion()
        {
                return View();
        }
        [HttpPost]

        public ActionResult RegistrarTipoFiliacion(E_Tipo_Filiacion Tfil)
        {
            string Crea = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoTipo_Filiacion", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodTipFil", "");
                        cmd.Parameters.AddWithValue("@DescTipFil", Tfil.DescTipFil.ToUpper());
                        cmd.Parameters.AddWithValue("@Estado", true);
                        cmd.Parameters.AddWithValue("@Evento", 1);

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se registro Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "3";
                        return View(Tfil);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListaTipoFiliacion");
            }
        }
        public ActionResult ModificarTipoFiliacion(string Id)
        {
            //if (Session["UserID"] != null)
            //{
                var lista = (from x in ListadoTipoFiliacion() where x.CodTipFil == Id select x).FirstOrDefault();
                return View(lista);
            //}else
            //{
            //    return RedirectToAction("../Login/Index");
            //}
        }
        [HttpPost]

        public ActionResult ModificarTipoFiliacion(E_Tipo_Filiacion Tfil)
        {
            string Crea = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoTipo_Filiacion", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodTipFil", Tfil.CodTipFil);
                        cmd.Parameters.AddWithValue("@DescTipFil", Tfil.DescTipFil.ToUpper());
                        cmd.Parameters.AddWithValue("@Estado", Tfil.Estado);
                        cmd.Parameters.AddWithValue("@Evento", 2);

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se modifico Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al modificar : " + ex.Message.ToString();
                        return View(Tfil);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListaTipoFiliacion");
            }
        }



        public ActionResult ListaTipoFiliacion()
        {
            //if (Session["UserID"] != null)
            //{
                return View(ListadoTipoFiliacion());
            //}else
            //{
            //    return RedirectToAction("../Login/Index");
            //}
        }

        public List<E_Tipo_Filiacion> ListadoTipoFiliacion()
        {
            List<E_Tipo_Filiacion> Lista = new List<E_Tipo_Filiacion>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Tipo_Filiacion", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Tipo_Filiacion ETfil = new E_Tipo_Filiacion();

                            ETfil.CodTipFil = dr.GetString(0);
                            ETfil.DescTipFil = dr.GetString(1).ToUpper();
                            ETfil.Estado = dr.GetBoolean(2);
                            Lista.Add(ETfil);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }









    }
}