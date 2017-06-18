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
namespace VgSalud.Controllers
{
    public class AccesoController : Controller
    {

        public List<E_Acceso> ListaModulos(string CodUsu)
        {
            List<E_Acceso> Lista = new List<E_Acceso>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaModulos", con))
                {
                    cmd.Parameters.AddWithValue("@CodUsu", CodUsu);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Acceso acc = new E_Acceso();
                            acc.idModulo = dr.GetInt32(0);
                            acc.NombreModulo = dr.GetString(1);
                            acc.icono = dr.GetString(2);


                            Lista.Add(acc);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
      
        public List<E_Acceso> ListaModulosTotal()
        {
            List<E_Acceso> Lista = new List<E_Acceso>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_modulosVista", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Acceso acc = new E_Acceso();

                            acc.idModulo = dr.GetInt32(0);
                            acc.NombreModulo = dr.GetString(1);
                            acc.icono = dr.GetString(2);


                            Lista.Add(acc);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Acceso> ListaCategorias(int id, string CodUsu)
        {
            List<E_Acceso> Lista = new List<E_Acceso>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Categorias", con))
                {
                    cmd.Parameters.AddWithValue("@idMod", id);
                    cmd.Parameters.AddWithValue("@CodUsu", CodUsu);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Acceso acc = new E_Acceso();

                            acc.idCat = dr.GetInt32(0);
                            acc.NomCat = dr.GetString(2);

                            Lista.Add(acc);

                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Acceso> ListaFormularioMenu(int id, string CodUsu)
        {
            List<E_Acceso> Lista = new List<E_Acceso>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_FormularioMenu", con))
                {
                    cmd.Parameters.AddWithValue("@idCat", id);
                    cmd.Parameters.AddWithValue("@CodUsu", CodUsu);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Acceso acc = new E_Acceso();

                            acc.idForm = dr.GetInt32(0);
                            acc.nomForm = dr.GetString(1);
                            acc.AliasForm = dr.GetString(2);

                            Lista.Add(acc);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Acceso> ListaFormulariGeneral(string CodUsu)
        {
            List<E_Acceso> Lista = new List<E_Acceso>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_FormularioGeneral", con))
                {
                    cmd.Parameters.AddWithValue("@CodUsu", CodUsu);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Acceso acc = new E_Acceso();
                            acc.idForm = dr.GetInt32(0);
                            acc.nomForm = dr.GetString(1);
                            acc.AliasForm = dr.GetString(2);
                            Lista.Add(acc);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Acceso> ListaAcceso(string CodSede)
        {
            List<E_Acceso> Lista = new List<E_Acceso>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Listar_Acceso_General", con))
                {
                    cmd.Parameters.AddWithValue("CodSede", CodSede);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Acceso acc = new E_Acceso();

                            acc.CodAcc = dr.GetInt32(0);
                            acc.CodPerf = dr.GetString(1);
                            acc.CodUsu = dr.GetString(2);
                            acc.EstAcc = dr.GetString(3);


                            Lista.Add(acc);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

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
                            per.descPerf = dr.GetString(1).ToUpper();
                            per.EstperF = dr.GetBoolean(2);


                            Lista.Add(per);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

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
                            usu.DniUsu = dr.GetString(3);
                            usu.ApePaterUsu = dr.GetString(4);
                            usu.ApeMaterUsu = dr.GetString(5);
                            usu.NomUsu = dr.GetString(6).ToUpper();
                            usu.FecNacUsu = dr.GetDateTime(7);
                            usu.EstUsu = dr.GetBoolean(8);

                            Lista.Add(usu);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult ListarAcceso()
        {
            var listaperfil = (List<E_Perfil>)listaPerfiles();
            var listausuario = (List<E_Usuario>)listaUsuarios();
            string sede = Session["codSede"].ToString();
            var lista = (List<E_Acceso>)ListaAcceso(sede);
            List<E_Acceso> listado = new List<E_Acceso>();
            foreach (var item1 in lista)
            {
                foreach (var item2 in listausuario)
                {
                    foreach (var item3 in listaperfil)
                    {
                        if (item1.CodUsu == item2.codUsu)
                        {
                            if (item1.CodPerf == item3.codperf)
                            {
                                E_Acceso acceso = new E_Acceso();
                                acceso.CodAcc = item1.CodAcc;
                                acceso.CodPerf = item3.codperf.ToUpper();
                                acceso.NombrePerfil = item3.descPerf;
                                acceso.NombreUsuario = item2.aliasUsu.ToUpper();
                                if (item1.EstAcc.Equals("1")) { acceso.Estado = true; } else { acceso.Estado = false; }
                                listado.Add(acceso);
                            }
                        }
                    }
                }
            }
            return View(listado);
        }

        public ActionResult RegistrarAcceso()
        {

            ViewBag.ListadoPerfiles = new SelectList(listaPerfiles().Where(x => x.EstperF == true).ToList(), "codperf", "descPerf");
            ViewBag.ListadoUsuarios = new SelectList(listaUsuarios().Where(x => x.EstUsu == true).ToList(), "CodUsu", "AliasUsu");
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarAcceso(E_Acceso acc)
        {
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
            ViewBag.ListadoPerfiles = new SelectList(listaPerfiles().Where(x => x.EstperF == true).ToList(), "codperf", "descPerf");
            ViewBag.ListadoUsuarios = new SelectList(listaUsuarios().Where(x => x.EstUsu == true).ToList(), "CodUsu", "AliasUsu");
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Acesso", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;

                        da.Parameters.AddWithValue("@CodAcc", "");
                        da.Parameters.AddWithValue("@CodPerf", acc.CodPerf);
                        da.Parameters.AddWithValue("@CodUsu", acc.CodUsu);
                        if (acc.Estado == true)
                        {
                            da.Parameters.AddWithValue("@EstAcc", "1");
                        }
                        else
                        {
                            da.Parameters.AddWithValue("@EstAcc", "0");
                        }

                        da.Parameters.AddWithValue("@Crea", crea);
                        da.Parameters.AddWithValue("@Modifica", "");
                        da.Parameters.AddWithValue("@Elimina", "");
                        da.Parameters.AddWithValue("@Evento", "1");
                        var index = da.ExecuteNonQuery();
                        if (index == -1)
                        {
                            ViewBag.mensaje = "Error el Usuario ya se encuentra asignado al mismo perfil";
                            return View();
                        }

                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "Error:Datos No Validos";

                        return View(acc);
                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListarAcceso");
        }


        public ActionResult ModificarAcceso(int id)
        {
            ViewBag.ListadoPerfiles = new SelectList(listaPerfiles(), "codperf", "descPerf");
            ViewBag.ListadoUsuarios = new SelectList(listaUsuarios(), "CodUsu", "AliasUsu");
            string sede = Session["codSede"].ToString();
            var lista = (from x in ListaAcceso(sede) where x.CodAcc == id select x).FirstOrDefault();
            return View(lista);
        }

        [HttpPost]
        public ActionResult ModificarAcceso(E_Acceso acc)
        {
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string modifica = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Acesso", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@CodAcc", "");
                        da.Parameters.AddWithValue("@CodPerf", acc.CodPerf);
                        da.Parameters.AddWithValue("@CodUsu", acc.CodUsu);
                        da.Parameters.AddWithValue("@EstAcc", acc.EstAcc);
                        da.Parameters.AddWithValue("@Crea", "");
                        da.Parameters.AddWithValue("@Modifica", modifica);
                        da.Parameters.AddWithValue("@Elimina", "");
                        da.Parameters.AddWithValue("@Evento", "2");

                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {

                    }
                    finally { con.Close(); }
                }
            }
            return RedirectToAction("ListarAcceso");
        }


        public ActionResult Eliminar(int id)
        {
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string elimina = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Acesso", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@CodAcc", id);
                        da.Parameters.AddWithValue("@CodPerf", "");
                        da.Parameters.AddWithValue("@CodUsu", "");
                        da.Parameters.AddWithValue("@EstAcc", "");
                        da.Parameters.AddWithValue("@Crea", "");
                        da.Parameters.AddWithValue("@Modifica", "");
                        da.Parameters.AddWithValue("@Elimina", elimina);
                        da.Parameters.AddWithValue("@Evento", "3");

                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {

                    }
                    finally { con.Close(); }
                }
            }
            return RedirectToAction("ListarAcceso");

        }


        public ActionResult Activar(int id)
        {
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string modifica = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Acesso", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@CodAcc", id);
                        da.Parameters.AddWithValue("@CodPerf", "");
                        da.Parameters.AddWithValue("@CodUsu", "");
                        da.Parameters.AddWithValue("@EstAcc", "");
                        da.Parameters.AddWithValue("@Crea", "");
                        da.Parameters.AddWithValue("@Modifica", modifica);
                        da.Parameters.AddWithValue("@Elimina", "");
                        da.Parameters.AddWithValue("@Evento", "4");

                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "Error Datos no validos";
                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListarAcceso");
        }


        public ActionResult AsignarRoles(int? CodAcc = null, int? CodForm = null)
        {
            MasterController ma = new MasterController();
            ViewBag.perfiles = new SelectList(listaPerfiles().ToList(), "CODMEDIOS", "DESCRIPCION");
            ViewBag.formularios = new SelectList(ma.ListaFormularios().ToList(), "CODMEDIOS", "DESCRIPCION");

            return View();
        }






    }
}