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
namespace VgSalud.Controllers
{
    public class UsuarioController : Controller
    {

        public List<E_Usuario> listaUsuarios()
        {
            List<E_Usuario> Lista = new List<E_Usuario>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Usuario_General", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Usuario usu = new E_Usuario();

                            usu.codUsu = dr.GetString(0);
                            usu.aliasUsu = dr.GetString(1).ToUpper();
                            usu.pwdUsu = dr.GetString(2);
                            usu.DniUsu = dr.GetString(3);
                            usu.ApePaterUsu = dr.GetString(4).ToUpper();
                            usu.ApeMaterUsu = dr.GetString(5).ToUpper();
                            usu.NomUsu = dr.GetString(6).ToUpper();
                            usu.FecNacUsu = dr.GetDateTime(7);
                            usu.EstUsu = dr.GetBoolean(8);
                            usu.Concatena = dr.GetString(6) + " " + dr.GetString(4) + " " + dr.GetString(5) + " | " + dr.GetString(1);


                            Lista.Add(usu);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public ActionResult listaUsuario()
        {

            string usuario = Session["UserID"].ToString();
            string codsede = Session["codSede"].ToString();
            var acceso = new AccesoController().ListaAcceso(codsede).Where(x => x.CodUsu == usuario).FirstOrDefault();
            if (acceso.CodPerf == "P0002")
            {
                var listUsuario = listaUsuarios().Where(x => x.codUsu != "00001").ToList();
                return View("ListaUsuarioPersonales", listUsuario);
            }
            else if (acceso.CodPerf == "P0001") {
                var listUsuario = listaUsuarios().ToList();
                return View("ListaUsuarioPersonales", listUsuario);
            }
            else
            {
                var listUsuario = listaUsuarios().Where(x => x.codUsu == usuario).ToList();
                return View(listUsuario);
            }
 
        }
        
        public ActionResult RegistraUsuario()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegistraUsuario(E_Usuario usu)
        {
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
            MasterController ma = new MasterController();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Usuario", con))
                {
                    try
                    {
                        string encripta = ma.EncryptKey(usu.pwdUsu);
                        da.CommandType = CommandType.StoredProcedure;


                        da.Parameters.AddWithValue("@CodUsu", "");
                        da.Parameters.AddWithValue("@AliasUsu", usu.aliasUsu.ToUpper());
                        da.Parameters.AddWithValue("@PwdUsu", encripta);
                        da.Parameters.AddWithValue("@DniUsu", usu.DniUsu);
                        da.Parameters.AddWithValue("@ApePaterUsu", usu.ApePaterUsu.ToUpper());
                        da.Parameters.AddWithValue("ApeMaterUsu", usu.ApeMaterUsu.ToUpper());
                        da.Parameters.AddWithValue("@NomUsu", usu.NomUsu.ToUpper());
                        da.Parameters.AddWithValue("@FecNacUsu", usu.FecNacUsu);
                        da.Parameters.AddWithValue("@EstUsu", usu.EstUsu);
                        da.Parameters.AddWithValue("@Crea", crea);
                        da.Parameters.AddWithValue("@Modifica", "");
                        da.Parameters.AddWithValue("@Elimina", "");
                        da.Parameters.AddWithValue("@Tipo", "1");

                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "3";

                        return View(usu);
                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("listaUsuario");
        }

        public ActionResult ModificarUsuario(string id)
        {

            var lista = (from x in listaUsuarios() where x.codUsu.Equals(id) select x).FirstOrDefault();
            return View(lista);
        }
        [HttpPost]
        public ActionResult ModificarUsuario(E_Usuario usu)
        {
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string modifica = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Usuario", con))
                {
                    try
                    {

                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@CodUsu", usu.codUsu);
                        da.Parameters.AddWithValue("@AliasUsu", usu.aliasUsu);
                        da.Parameters.AddWithValue("@PwdUsu","");
                        da.Parameters.AddWithValue("@DniUsu", usu.DniUsu);
                        da.Parameters.AddWithValue("@ApePaterUsu", usu.ApePaterUsu.ToUpper());
                        da.Parameters.AddWithValue("ApeMaterUsu", usu.ApeMaterUsu.ToUpper());
                        da.Parameters.AddWithValue("@NomUsu", usu.NomUsu.ToUpper());
                        da.Parameters.AddWithValue("@FecNacUsu", usu.FecNacUsu);
                        da.Parameters.AddWithValue("@EstUsu", usu.EstUsu);
                        da.Parameters.AddWithValue("@Crea", "");
                        da.Parameters.AddWithValue("@Modifica", modifica);
                        da.Parameters.AddWithValue("@Elimina", "");
                        da.Parameters.AddWithValue("@Tipo", "2");

                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "Error Datos no Validos";
                        return View(usu); 
                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("listaUsuario");
        }

        public ActionResult Eliminar(string id)
        {
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string elimina = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Usuario", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@CodUsu", id);
                        da.Parameters.AddWithValue("@AliasUsu", "");
                        da.Parameters.AddWithValue("@PwdUsu", "");
                        da.Parameters.AddWithValue("@DniUsu", "");
                        da.Parameters.AddWithValue("@ApePaterUsu", "");
                        da.Parameters.AddWithValue("ApeMaterUsu", "");
                        da.Parameters.AddWithValue("@NomUsu", "");
                        da.Parameters.AddWithValue("@FecNacUsu", "");
                        da.Parameters.AddWithValue("@EstUsu", "");
                        da.Parameters.AddWithValue("@Crea", "");
                        da.Parameters.AddWithValue("@Modifica", "");
                        da.Parameters.AddWithValue("@Elimina", elimina);
                        da.Parameters.AddWithValue("@Tipo", "3");

                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {

                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("listaUsuario");
        }


        public ActionResult activar(string id)
        {
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string modifica = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Usuario", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@CodUsu", id);
                        da.Parameters.AddWithValue("@AliasUsu", "");
                        da.Parameters.AddWithValue("@PwdUsu", "");
                        da.Parameters.AddWithValue("@DniUsu", "");
                        da.Parameters.AddWithValue("@ApePaterUsu", "");
                        da.Parameters.AddWithValue("ApeMaterUsu", "");
                        da.Parameters.AddWithValue("@NomUsu", "");
                        da.Parameters.AddWithValue("@FecNacUsu", "");
                        da.Parameters.AddWithValue("@EstUsu", "");
                        da.Parameters.AddWithValue("@Crea", "");
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
            return RedirectToAction("listaUsuario");
        }
               


        public ActionResult CambiarContraseña(string id)
        {
            E_Usuario usuario = listaUsuarios().Where(x => x.codUsu == id).FirstOrDefault(); 
            return View(usuario);
        }


        [HttpPost]
        public ActionResult CambiarContraseña(E_Usuario usu)
        {
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string modifica = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
       
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Usuario", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;
                        MasterController cm = new MasterController();
                        string encripta1 = cm.EncryptKey(usu.passwordRepit);

                        if (usu.passwordNew == usu.passwordRepit)
                        {
                            da.Parameters.AddWithValue("@CodUsu", usu.codUsu);
                            da.Parameters.AddWithValue("@AliasUsu", "");
                            da.Parameters.AddWithValue("@PwdUsu", encripta1);
                            da.Parameters.AddWithValue("@DniUsu", "");
                            da.Parameters.AddWithValue("@ApePaterUsu", "");
                            da.Parameters.AddWithValue("ApeMaterUsu", "");
                            da.Parameters.AddWithValue("@NomUsu", "");
                            da.Parameters.AddWithValue("@FecNacUsu", "");
                            da.Parameters.AddWithValue("@EstUsu", "");
                            da.Parameters.AddWithValue("@Crea", "");
                            da.Parameters.AddWithValue("@Modifica","");
                            da.Parameters.AddWithValue("@Elimina", "");
                            da.Parameters.AddWithValue("@Tipo", "5");


                            da.ExecuteNonQuery();
                        }
                        else {
                            ViewBag.alerta = "No ingreso correctamente los datos";
                        }

                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "Error: " + e.Message;

                        return View(usu);
                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("listaUsuario");
        }



        public bool IsValid(string alias, string passwordLast)
        {

            
            var sede = Session["codSede"];

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ToString()))
            {

                try
                {
                    MasterController cm = new MasterController();
                    string encripta = cm.EncryptKey(passwordLast);
                    var cmd = new SqlCommand("Usp_Login", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario",alias);
                    cmd.Parameters.AddWithValue("@clave", encripta);
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {

                        if (dr.HasRows)
                        {

                            dr.Dispose();
                            cmd.Dispose();
                            return true;
                        }
                        else
                        {
                            dr.Dispose();
                            cmd.Dispose();
                            return false;
                        }

                    }
                }

                catch (Exception e)
                {
                    ViewBag.mensaje = "Error no se pudo ingresar";
                    return false;
                }
            }

        }

        public JsonResult IsValidadoPassword( string alias = null, string Pass=null) {
            
            if (alias != null && Pass != null ) { 
                bool valor = IsValid(alias, Pass);
                return Json(valor, JsonRequestBehavior.AllowGet);              
              
            }
            return null;

        } 


       



    }
}