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
    public class UsuarioDocumentoSerieController : Controller
    {

        public List<E_UsuarioDocumentoSerie> ListaUsuarioDocumentoSerie(string CodSede)
        {
            List<E_UsuarioDocumentoSerie> Lista = new List<E_UsuarioDocumentoSerie>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ListaUsuarioDocumentoSerie", con))
                {
                    cmd.Parameters.AddWithValue("@CodSede", CodSede);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_UsuarioDocumentoSerie Ser = new E_UsuarioDocumentoSerie();


                            Ser.CodUsu = dr.GetString(0);
                            Ser.CodDocSerie = dr.GetString(1).ToUpper();
                            Ser.Prioridad = dr.GetBoolean(2);
                            Ser.EstUDs = dr.GetBoolean(3);
                            Ser.AliasUsu = dr.GetString(4).ToUpper();
                            Ser.Serie = dr.GetString(5);


                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult ListaUsuarioDocSerie()
        {
            string sede = Session["codSede"].ToString();
            return View(ListaUsuarioDocumentoSerie(sede));
        }

        public ActionResult RegistrarUsuarioDocSerie()
        {
            string sede = Session["codSede"].ToString();
            UsuarioController usu = new UsuarioController();
            DocumentoSerieController s = new DocumentoSerieController();

            ViewBag.usuario = new SelectList(usu.listaUsuarios().Where(x=>x.EstUsu == true).ToList(), "codUsu", "Concatena");
            ViewBag.serie = new SelectList(s.ListarDocumentoSerie().Where(x => x.CodSede == sede && x.EstDocSerie == true).ToList(), "CodDocSerie", "SerieDocumento");

            return View();
        }

        [HttpPost]
        public ActionResult RegistrarUsuarioDocSerie(E_UsuarioDocumentoSerie e)
        {
            string sede = Session["codSede"].ToString();
            UsuarioController usu = new UsuarioController();
            DocumentoSerieController s = new DocumentoSerieController();
            int result = 0;
            ViewBag.usuario = new SelectList(usu.listaUsuarios().Where(x => x.EstUsu == true).ToList(), "codUsu", "Concatena", e.CodUsu);
            ViewBag.serie = new SelectList(s.ListarDocumentoSerie().Where(x => x.CodSede== sede && x.EstDocSerie == true).ToList(), "CodDocSerie", "SerieDocumento", e.CodDocSerie);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_UsuarioDocumentoSerie", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodUsuario", e.CodUsu);
                        cmd.Parameters.AddWithValue("@DocSerie", e.CodDocSerie);
                        cmd.Parameters.AddWithValue("@Prioridad", e.Prioridad);
                        cmd.Parameters.AddWithValue("@EstDC", e.EstUDs);
                        cmd.Parameters.AddWithValue("@Tipo", 1);
                        cmd.CommandType = CommandType.StoredProcedure;
                        result =  Convert.ToInt32(cmd.ExecuteScalar().ToString());
                        if (result > 0)
                        {
                            return RedirectToAction("ListaUsuarioDocSerie");
                        }
                        else {
                            ViewBag.mensaje = "Error El Usuario ya esta Asignado a un Documento Serie";
                            return View(e);
                        }
                     
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Error Datos no Validos";
                        return View(e);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
              
            }
            return View();
        }


        public ActionResult ModificarUsuarioDocSerie(string CodUsu, string CodSerie)
        {
            string sede = Session["codSede"].ToString();
            UsuarioController usu = new UsuarioController();
            DocumentoSerieController s = new DocumentoSerieController();

            ViewBag.usuario = new SelectList(usu.listaUsuarios().Where(x=>x.EstUsu==true).ToList(), "codUsu", "Concatena");
            ViewBag.serie = new SelectList(s.ListarDocumentoSerie().Where(x => x.CodSede == sede && x.EstDocSerie == true).ToList(), "CodDocSerie", "SerieDocumento");
            var lista = (from x in ListaUsuarioDocumentoSerie(sede) where x.CodUsu == CodUsu && x.CodDocSerie==CodSerie select x).FirstOrDefault();
            return View(lista);
        }

        [HttpPost]
        public ActionResult ModificarUsuarioDocSerie(E_UsuarioDocumentoSerie e)
        {
            UsuarioController usu = new UsuarioController();
            DocumentoSerieController s = new DocumentoSerieController();
            string sede = Session["codSede"].ToString();
            int result = 0;
            ViewBag.usuario = new SelectList(usu.listaUsuarios().Where(x=>x.EstUsu == true).ToList(), "codUsu", "Concatena", e.CodUsu);
            ViewBag.serie = new SelectList(s.ListarDocumentoSerie().Where(x => x.CodSede == sede && x.EstDocSerie == true).ToList(), "CodDocSerie", "SerieDocumento", e.CodDocSerie);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_UsuarioDocumentoSerie", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodUsuario", e.CodUsu);
                        cmd.Parameters.AddWithValue("@DocSerie", e.CodDocSerie);
                        cmd.Parameters.AddWithValue("@Prioridad", e.Prioridad);
                        cmd.Parameters.AddWithValue("@EstDC", e.EstUDs);
                        cmd.Parameters.AddWithValue("@Tipo", 2);
                        cmd.CommandType = CommandType.StoredProcedure;
                        result = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                        if (result > 0)
                        {
                            return RedirectToAction("ListaUsuarioDocSerie");
                        }
                        else
                        {
                            ViewBag.mensaje = "Error El Usuario ya esta Asignado a un Documento Serie";
                            return View(e);
                        }
                     
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                        return View(e);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return RedirectToAction("ListaUsuarioDocSerie");
            }
        }


        public ActionResult EliminarUsuarioDocSerie(string CodUsu, string CodSerie)
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_UsuarioDocumentoSerie", con))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@CodUsuario", CodUsu);
                        cmd.Parameters.AddWithValue("@DocSerie", CodSerie);
                        cmd.Parameters.AddWithValue("@Prioridad", "");
                        cmd.Parameters.AddWithValue("@EstDC", "");
                        cmd.Parameters.AddWithValue("@Tipo", 3);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                        ViewBag.mensaje = "Bien";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                        return RedirectToAction("ListaUsuarioDocSerie");
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return RedirectToAction("ListaUsuarioDocSerie");
            }
        }


        public ActionResult ActivarUsuarioDocSerie(string CodUsu, string CodSerie)
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_UsuarioDocumentoSerie", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodUsuario", CodUsu);
                        cmd.Parameters.AddWithValue("@DocSerie", CodSerie);
                        cmd.Parameters.AddWithValue("@Prioridad", "");
                        cmd.Parameters.AddWithValue("@EstDC", "");
                        cmd.Parameters.AddWithValue("@Tipo", 4);
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.ExecuteNonQuery();
                        ViewBag.mensaje = "Bien";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                        return RedirectToAction("ListaUsuarioDocSerie");
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return RedirectToAction("ListaUsuarioDocSerie");
            }
        }




    }
}