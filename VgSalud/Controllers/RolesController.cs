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
    public class RolesController : Controller
    {

        public List<E_Roles> listaroles()
        {
            List<E_Roles> Lista = new List<E_Roles>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Roles", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Roles roles = new E_Roles();

                            roles.CodRoles = dr.GetString(0);
                            roles.DescRoles = dr.GetString(1);
                            roles.ObsRoles = dr.GetString(2);
                            roles.EstRoles = dr.GetBoolean(3);

                            Lista.Add(roles);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult ListarRoles() {

            return View(listaroles());
        }

        public ActionResult RegistrarRoles() {
            return View(); 
        }

        [HttpPost]
        public ActionResult RegistrarRoles(E_Roles rol)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Roles", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;


                        da.Parameters.AddWithValue("@CodRoles", "");
                        da.Parameters.AddWithValue("@DescRoles",rol.DescRoles.ToUpper());
                        da.Parameters.AddWithValue("@ObsRoles",rol.ObsRoles.ToUpper());
                        da.Parameters.AddWithValue("@EstRoles", rol.EstRoles);
                        da.Parameters.AddWithValue("@Tipo", "1");


                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "Error: " + e.Message;

                        return View(rol);
                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListarRoles");

        }

        public ActionResult ModificarRoles(string id)
        {
            var listaRoles = (from x in listaroles() where x.CodRoles == id select x).FirstOrDefault(); 
            return View(listaRoles); 
        }

        [HttpPost]
        public ActionResult ModificarRoles(E_Roles rol)
        {
         using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Roles", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@CodRoles",rol.CodRoles);
                        da.Parameters.AddWithValue("@DescRoles", rol.DescRoles.ToUpper());
                        da.Parameters.AddWithValue("@ObsRoles", rol.ObsRoles.ToUpper());
                        da.Parameters.AddWithValue("@EstRoles", "");
                        da.Parameters.AddWithValue("@Tipo", "2");

                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {

                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListarRoles");
        }


        public ActionResult Eliminar(string id) {


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Roles", con))
                {
                    try
                    {
                       
                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@CodRoles", id);
                        da.Parameters.AddWithValue("@DescRoles","");
                        da.Parameters.AddWithValue("@ObsRoles", "");
                        da.Parameters.AddWithValue("@EstRoles", "");
                        da.Parameters.AddWithValue("@Tipo", "3");



                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {

                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListarRoles");
        }

        public ActionResult Activar(string id)
        {


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Roles", con))
                {
                    try
                    {

                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@CodRoles", id);
                        da.Parameters.AddWithValue("@DescRoles", "");
                        da.Parameters.AddWithValue("@ObsRoles", "");
                        da.Parameters.AddWithValue("@EstRoles", "");
                        da.Parameters.AddWithValue("@Tipo", "4");
                        

                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {

                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListarRoles");
        }






    }
}