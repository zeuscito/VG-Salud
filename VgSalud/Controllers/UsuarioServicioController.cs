using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using VgSalud.Models; 
namespace VgSalud.Controllers
{
    public class UsuarioServicioController : Controller
    {


        public List<E_Usuario_Servicio> ListaUsuarioServicio()
        {
            List<E_Usuario_Servicio> Lista = new List<E_Usuario_Servicio>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_UsuarioServicio_General", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Usuario_Servicio ususervicio = new E_Usuario_Servicio();

                            ususervicio.CodUsu = dr.GetString(0);
                            ususervicio.NombreUSuario = dr.GetString(1).ToUpper();
                            ususervicio.CodServ = dr.GetString(2);
                            ususervicio.NomServ = dr.GetString(3).ToUpper();
                            ususervicio.CodEsp = dr.GetString(4);
                            ususervicio.General = dr.GetBoolean(5);

                            Lista.Add(ususervicio);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        ServiciosController ser = new ServiciosController();
        public ActionResult ListarUsuarioServicio() { 
            
            return View(ListaUsuarioServicio()); 
        }
        public ActionResult RegistrarUsuarioServicio()
        {
            UsuarioController usu = new UsuarioController();

            string sede = Session["codSede"].ToString();
            ViewBag.listaServicio = new SelectList(ser.ListadoServicios().Where(x => x.CodSede== sede && x.EstServ == true).ToList(),"CodServ","NomServ");
                 
            ViewBag.listaUsuario = new SelectList(usu.listaUsuarios().Where(x=>x.EstUsu == true).ToList(), "CodUsu", "AliasUsu");
        

            return View();
        }

        [HttpPost]
        public ActionResult RegistrarUsuarioServicio(E_Usuario_Servicio ususer)
        {
            UsuarioController usu = new UsuarioController();
            string sede = Session["codSede"].ToString();
            ViewBag.listaServicio = new SelectList(ser.ListadoServicios().Where(x => x.CodSede == sede && x.EstServ == true).ToList(), "CodServ", "NomServ");

            ViewBag.listaUsuario = new SelectList(usu.listaUsuarios().Where(x => x.EstUsu == true).ToList(), "CodUsu", "AliasUsu");
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Usuario_Servicio", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@codUsu", ususer.CodUsu);
                        da.Parameters.AddWithValue("@codSer", ususer.CodServ);
                        da.Parameters.AddWithValue("@Tipo", "1");
                       var result = (int) da.ExecuteScalar();
                        if (result == 0)
                        {
                            ViewBag.mensaje = "Error el Usuario ya se encuentra Asignado";
                            return View(ususer);
                        }
                        else {
                            ViewBag.success = "Se Registro con exito";
                        }
                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "3";

                        return View(ususer);
                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListarUsuarioServicio");

        }


        public ActionResult Eliminar(string id, string id1)
        {
            
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Usuario_Servicio", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@codUsu", id);
                        da.Parameters.AddWithValue("@codSer", id1);
                        da.Parameters.AddWithValue("@Tipo", "2");
                                               
                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "Error: " + e.Message;

                    }
                    finally { con.Close(); }
                }

            }

            return RedirectToAction("ListarUsuarioServicio");
        }


    }
}