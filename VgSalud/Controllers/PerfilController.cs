using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VgSalud.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
namespace VgSalud.Controllers
{
    public class PerfilController : Controller
    {
        // GET: Perfil


        public List<E_Perfil> listaPerfiles()
        {
            List<E_Perfil> Lista = new List<E_Perfil>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Lista_General_Perfil", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Perfil per = new E_Perfil();

                            per.codperf = dr.GetString(0);
                            per.descPerf = dr.GetString(1);
                            per.EstperF = dr.GetBoolean(2);


                            Lista.Add(per);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult ListaPerfil()
        {
            var listaperfiles = (List<E_Perfil>)listaPerfiles().ToList();

            return View(listaperfiles);
        }



        public ActionResult RegistraPerfil()
        {

            return View();
        }

        [HttpPost]
        public ActionResult RegistraPerfil(E_Perfil per)
        {

            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Perfil", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;


                        da.Parameters.AddWithValue("@codperf", "");
                        da.Parameters.AddWithValue("@descPerf", per.descPerf.ToUpper());
                        da.Parameters.AddWithValue("@EstPerf", per.EstperF);
                        da.Parameters.AddWithValue("@crea", crea);
                        da.Parameters.AddWithValue("@Modifica", "");
                        da.Parameters.AddWithValue("@Elimina", "");
                        da.Parameters.AddWithValue("@Tipo", "1");

                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "Error: " + e.Message;

                        return View(per);
                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListaPerfil");
        }



        public ActionResult ModificarPerfil(string id)
        {

            var lista = (from x in listaPerfiles() where x.codperf.Equals(id) select x).FirstOrDefault();

            return View(lista);
        }


        [HttpPost]
        public ActionResult ModificarPerfil(E_Perfil per)
        {
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string modifica = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Perfil", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure; 
                        da.Parameters.AddWithValue("@codperf", per.codperf);
                        da.Parameters.AddWithValue("@descPerf", per.descPerf.ToUpper());
                        da.Parameters.AddWithValue("@EstPerf", per.EstperF);
                        da.Parameters.AddWithValue("@crea", "");
                        da.Parameters.AddWithValue("@Modifica", modifica);
                        da.Parameters.AddWithValue("@Elimina", "");
                        da.Parameters.AddWithValue("@Tipo", "2");

                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {

                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListaPerfil");
        }

        public ActionResult Eliminar(string id)
        {
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string elimina = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Perfil", con))
                {
                    try
                    {
                        
                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@codperf", id);
                        da.Parameters.AddWithValue("@descPerf", "");
                        da.Parameters.AddWithValue("@EstPerf", "");
                        da.Parameters.AddWithValue("@crea", "");
                        da.Parameters.AddWithValue("@Modifica", "");
                        da.Parameters.AddWithValue("@Elimina", elimina) ;
                        da.Parameters.AddWithValue("@Tipo", "3");

                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {

                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListaPerfil");
        }

        public ActionResult Activar(string id)
        {
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string modifica = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Perfil", con))
                {
                    try
                    {

                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@codperf", id);
                        da.Parameters.AddWithValue("@descPerf", "");
                        da.Parameters.AddWithValue("@EstPerf", "");
                        da.Parameters.AddWithValue("@crea", "");
                        da.Parameters.AddWithValue("@Modifica", modifica);
                        da.Parameters.AddWithValue("@Elimina", "");
                        da.Parameters.AddWithValue("@Tipo", "4");

                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {

                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListaPerfil");
        }





    }
}