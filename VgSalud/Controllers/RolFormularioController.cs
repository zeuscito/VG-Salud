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
    public class RolFormularioController : Controller
    {

        public List<E_RolFormulario> ListaRoles(string CodPerf)
        {
            List<E_RolFormulario> Lista = new List<E_RolFormulario>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_General_RolFormulario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodPerf", CodPerf);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_RolFormulario Rol = new E_RolFormulario();

                            Rol.CodPerf = dr.GetString(0);
                            Rol.IdForm = dr.GetInt32(1);
                            Rol.AliasForm = dr.GetString(2);
                            Rol.DescPerf = dr.GetString(3);

                            Lista.Add(Rol);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_RolFormulario> ListaFormulariosxPerfil(string CodPerf)
        {
            List<E_RolFormulario> Lista = new List<E_RolFormulario>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaFormularioxPerfil", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodPerf", CodPerf);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_RolFormulario Rol = new E_RolFormulario();

                            Rol.IdForm = dr.GetInt32(0);
                            Rol.AliasForm = dr.GetString(2);
                            Rol.idCat = dr.GetInt32(3);
                            Rol.idModulo = dr.GetInt32(5);
                            Rol.Estado = dr.GetInt32(6);

                            Lista.Add(Rol);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_RolFormulario> ListaFormulario()
        {
            List<E_RolFormulario> Lista = new List<E_RolFormulario>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_General_Formulario ", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_RolFormulario Rol = new E_RolFormulario();

                            Rol.IdForm = dr.GetInt32(0);
                            Rol.AliasForm = dr.GetString(2);

                            Lista.Add(Rol);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult BuscarRolFormulario(string CodPerf = null)
        {
            PerfilController perfil = new PerfilController();

            ViewBag.perfil = new SelectList(perfil.listaPerfiles().Where(x => x.EstperF == true).ToList(), "CodPerf", "DescPerf");
            if (CodPerf != null)
            {
                ViewBag.lista = ListaRoles(CodPerf);
                ViewBag.cod = CodPerf;
            }
            else
            {
                ViewBag.lista = null;
            }

            return View();

        }


        public ActionResult RegistrarRolFormulario(string CodPerf = null)
        {
            AccesoController ac = new AccesoController();

            if (CodPerf != null)
            {
                ViewBag.lista = ListaFormulariosxPerfil(CodPerf);
                ViewBag.modulos = ac.ListaModulosTotal();
                ViewBag.Codperfil = (string)CodPerf;
            }
            else {
                ViewBag.lista = null;
                ViewBag.modulos = null;
                ViewBag.Codperfil = "";
            }

            PerfilController perfil = new PerfilController();
            ViewBag.perfil = new SelectList(perfil.listaPerfiles().Where(x => x.EstperF == true).ToList(), "CodPerf", "DescPerf", CodPerf);
            
            return View();
        }


        public ActionResult agregar(E_RolFormulario rol)
        {
            PerfilController perfil = new PerfilController();
            List<E_RolFormulario> Lista = new List<E_RolFormulario>();

            ViewBag.perfil = new SelectList(perfil.listaPerfiles().Where(x => x.EstperF == true).ToList(), "CodPerf", "DescPerf");
            ViewBag.formulario = new SelectList(ListaFormulario(), "IdForm", "AliasForm");
            var rolesForm = (List<E_RolFormulario>)Session["agregar"];

            try
            {
                if (rolesForm == null)
                {
                    E_Perfil e = perfil.listaPerfiles().Where(x => x.codperf == rol.CodPerf).FirstOrDefault();
                    E_RolFormulario r = (E_RolFormulario)ListaFormulario().Where(x => x.IdForm == rol.IdForm).FirstOrDefault();
                    E_RolFormulario roles = new E_RolFormulario();
                    roles.CodPerf = rol.CodPerf;
                    roles.IdForm = rol.IdForm;
                    roles.DescPerf = e.descPerf;
                    roles.AliasForm = r.AliasForm;

                    Lista.Add(roles);
                    Session["agregar"] = Lista;

                }

                else
                {

                    E_Perfil e = perfil.listaPerfiles().Where(x => x.codperf == rol.CodPerf).FirstOrDefault();
                    E_RolFormulario r = (E_RolFormulario)ListaFormulario().Where(x => x.IdForm == rol.IdForm).FirstOrDefault();
                    E_RolFormulario roles = new E_RolFormulario();
                    roles.CodPerf = rol.CodPerf;
                    roles.IdForm = rol.IdForm;
                    roles.DescPerf = e.descPerf;
                    roles.AliasForm = r.AliasForm;
                    rolesForm.Add(roles);
                    Session["agregar"] = rolesForm;

                }

            }
            catch (Exception ex)
            {

                ViewBag.mensaje = $"Error: {ex.Message}";
                return RedirectPermanent("RegistrarRolFormulario?CodPerf=" + rol.CodPerf + "&IdForm=" + rol.IdForm);

            }
            return RedirectPermanent("RegistrarRolFormulario?CodPerf=" + rol.CodPerf + "&IdForm=" + rol.IdForm);

        }

        public ActionResult Delete(string id, int id2)
        {

            var formulario = (List<E_RolFormulario>)Session["agregar"];
            var registro = formulario.Where(x => x.CodPerf == id && x.IdForm == id2).FirstOrDefault();
            formulario.Remove(registro);
            Session["agregar"] = formulario;
            //Response.Write("<script language=javascript> history.back(1); </script>");

            return RedirectToAction("RegistrarRolFormulario");
        }
        
        public ActionResult RegistrarRolFormulario123(E_RolFormulario rol)
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    
                    using (SqlCommand da = new SqlCommand("usp_eliminaPerfilFormulario", con, tr))
                    {
                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@CodPerf", rol.CodPerf);

                        da.ExecuteNonQuery();
                    }

                    foreach (string ia in rol.array)
                    {
                        int av = Int32.Parse(ia);
                        using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_RolFormulario", con, tr))
                        {
                            da.CommandType = CommandType.StoredProcedure;
                            da.Parameters.AddWithValue("@CodPerf", rol.CodPerf);
                            da.Parameters.AddWithValue("@IdForm", av);
                            da.Parameters.AddWithValue("@Evento", "1");
                        
                            da.ExecuteNonQuery();
                        }
                    }
                    tr.Commit();
                }
                catch (Exception e)
                {
                    ViewBag.mensaje = "Error";
                    tr.Rollback();
                    return View(rol);

                }
                finally { con.Close(); }
                
            }

            return RedirectToAction("BuscarRolFormulario");

        }


        public ActionResult Eliminar(string id, int id2, string id3)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();

                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_RolFormulario", con))
                {
                    try
                    {

                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@CodPerf", id);
                        da.Parameters.AddWithValue("@IdForm", id2);
                        da.Parameters.AddWithValue("@Evento", "2");
                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "Error: " + e.Message;

                    }
                    finally { con.Close(); }
                }

            }
            return RedirectPermanent("../BuscarRolFormulario?CodPerf=" + id3);

        }





    }
}