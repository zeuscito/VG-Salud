using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using VgSalud.Models;
using System.Globalization;
using System.Configuration; 
namespace VgSalud.Controllers
{
    public class UsuarioSedeController : Controller
    {



        public List<E_Usuario_Sede> listaUsuarioSede()
        {
            List<E_Usuario_Sede> Lista = new List<E_Usuario_Sede>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_UsuarioSede_General", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Usuario_Sede ususede = new E_Usuario_Sede();

                            ususede.CodUsu = dr.GetString(0);
                            ususede.NombreUsuario = dr.GetString(1).ToUpper();
                            ususede.CodSede = dr.GetString(2);
                            ususede.NombreSede = dr.GetString(3).ToUpper();

                            Lista.Add(ususede);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult ListarUsuarioSede() {
          return View(listaUsuarioSede()); 
        }


        public ActionResult RegistrarUsuarioSede() {
            UsuarioController usu = new UsuarioController();
            SedesController sedes = new SedesController(); 
            ViewBag.listaUsuario = new SelectList(usu.listaUsuarios().Where(x=>x.EstUsu == true).ToList(),"CodUsu","AliasUsu");
            ViewBag.listaSedes = new  SelectList(sedes.ListadoSedes().Where(x=>x.EstSede == true).ToList(),"CodSede","NomSede"); 
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarUsuarioSede(E_Usuario_Sede usu) {

            UsuarioController usua = new UsuarioController();
            SedesController sedes = new SedesController();
            ViewBag.listaUsuario = new SelectList(usua.listaUsuarios().Where(x => x.EstUsu == true).ToList(), "CodUsu", "AliasUsu");
            ViewBag.listaSedes = new SelectList(sedes.ListadoSedes().Where(x => x.EstSede == true).ToList(), "CodSede", "NomSede");

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_UsuarioSede", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@CodUsu", usu.CodUsu);
                        da.Parameters.AddWithValue("@CodSede", usu.CodSede);
                        da.Parameters.AddWithValue("@Tipo", "1");
                      

                        var result = (int)da.ExecuteScalar();
                        if (result == 0)
                        {
                            ViewBag.mensaje = "Error: Ya se encuentra asignado el Usuario";
                            return View(usu);
                        }
                        else {
                            ViewBag.success = "Se Registro Correctamente"; 
                        }

                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "3";

                        return View(usu);
                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListarUsuarioSede");

        }


        public ActionResult Eliminar(string id, string id1) {


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_UsuarioSede", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;


                        da.Parameters.AddWithValue("@CodUsu",id);
                        da.Parameters.AddWithValue("@CodSede",id1);
                        da.Parameters.AddWithValue("@Tipo", "2");



                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {

                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListarUsuarioSede");
        }



    }
}