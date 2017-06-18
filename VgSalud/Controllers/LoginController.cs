using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VgSalud.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Security;


namespace VgSalud.Controllers
{
    public class LoginController : Controller
    {

        public List<E_Sede> BuscarSede(string cod)
        {
            List<E_Sede> Lista = new List<E_Sede>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_UsuarioSede", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AliasUsu", cod);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Sede r = new E_Sede();
                            r.CodSede = dr.GetString(0);
                            r.NomSede = dr.GetString(1);

                            Lista.Add(r);

                        }
                        con.Close();
                    }
                }
            }
            return Lista;
        }

        

        public bool IsValid(string _username, string _password, string sede, E_Login c)
        {
            List<E_Login> lista = new List<E_Login>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ToString()))
            {
                cn.Open();
                string _sql = "Usp_Login";
                try
                {
                    MasterController cm = new MasterController();
                    string encripta = cm.EncryptKey(_password);
                    var cmd = new SqlCommand(_sql, cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario", _username);
                    cmd.Parameters.AddWithValue("@clave", encripta);
                    cmd.Parameters.AddWithValue("@CodSede", sede);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            c.CodigoUsuario = dr.GetString(0);
                            c.Nombres = dr.GetString(1);
                            c.Apellidos = dr.GetString(2);
                            c.AliasUsu = dr.GetString(3);
                            c.NomSede = dr.GetString(4);
                           
                            lista.Add(c);
                        }

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

        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            { 
                return View();
            }
            else
            {
                return RedirectToAction("../Master/Index");
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(E_Login user)
        {
            string restante = string.Empty; 
            string[] splite = user.Pass.Split('+');
            if (splite.Length >1)
            {
                restante = splite[0].ToString();
                user.Pass = splite[1].ToString(); 
            }
            var util = new UsuarioController();
            var userID = util.listaUsuarios().Where(x => x.aliasUsu.ToLower() == user.AliasUsu.Trim().ToLower()).FirstOrDefault();
            var licencia = new DatosGeneralesController();
            var fecha_licencia = licencia.Getdatogenerales();
            var utilitarios = new UtilitarioController().ListadoHoraServidor().FirstOrDefault();
            DateTime fecha = utilitarios.HoraServidor;
            if (Convert.ToDateTime(fecha_licencia.fecha) >= fecha)
            {

                if (userID != null)
                {
                    if (userID.codUsu == "00001")
                    {
                        if (restante == "42130999")
                        {

                            if (IsValid(user.AliasUsu, user.Pass, user.CodSede, user))
                            {
                                Session["UserID"] = user.CodigoUsuario;
                                Session["usuario"] = user.AliasUsu.ToString();
                                Session["nombre"] = user.Nombres.ToString();
                                Session["apellido"] = user.Apellidos.ToString();
                                Session["nomSede"] = user.NomSede.ToString();
                                Session["codSede"] = user.CodSede.ToString();
                                return RedirectToAction("../Master/index");

                            }
                            else
                            {
                               
                                ModelState.AddModelError("", "Datos invalidos!");
                            }
                            ViewBag.validation = "Error Datos Invalidos!!!"; 
                            return View(user);


                        }
                        else
                        {
                            ModelState.AddModelError("", "Datos invalidos!");
                            ViewBag.validation = "Error Autenticacion Invalida!!!";
                            return View(user);
                        }
                    }
                    else
                    {
                        if (IsValid(user.AliasUsu, user.Pass, user.CodSede, user))
                        {
                            Session["UserID"] = user.CodigoUsuario;
                            Session["usuario"] = user.AliasUsu.ToString();
                            Session["nombre"] = user.Nombres.ToString();
                            Session["apellido"] = user.Apellidos.ToString();
                            Session["nomSede"] = user.NomSede.ToString();
                            Session["codSede"] = user.CodSede.ToString();
                            return RedirectToAction("../Master/index");

                        }
                        else
                        {
                            ModelState.AddModelError("", "Datos invalidos!");
                        }
                        return View(user);

                    }
                }
                else
                {
                    ViewBag.validation = "Error Datos Invalidos!!!";
                    ModelState.AddModelError("", "Datos invalidos!");
                    return View(user);
                }

            }
            else
            {
                ModelState.AddModelError("", "La Fecha de Licencia ah Expirado");
                ViewBag.validation = "La Fecha de Licencia ah Expirado!!!";
                return View(user);

            }
          
        }


        public ActionResult CerrarSesion()
        {
            Session.Remove("UserID");
            Session.Remove("usuario");
            Session.Remove("nombre");
            Session.Remove("apePat");
            Session.Remove("rol");
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult VerSedes(string alias)
        {
            var evalua = (List<E_Sede>)BuscarSede(alias);
            
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
        }



       


    }
}