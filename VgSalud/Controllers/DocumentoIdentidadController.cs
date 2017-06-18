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
    public class DocumentoIdentidadController : Controller
    {
        // GET: DocumentoIdentidad
        public ActionResult RegistrarDocumentoIdentidad()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }else
            {
                return RedirectToAction("../Login/Index");
            }
            
        }

        [HttpPost]
        public ActionResult RegistrarDocumentoIdentidad(E_Documento_Identidad EDoc)
        {
            string Crea = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoDocumento_Identidad", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodDocIdent", "");
                        cmd.Parameters.AddWithValue("@NomDocIdent", EDoc.NomDocIdent.ToUpper());
                        cmd.Parameters.AddWithValue("@Estado", EDoc.Estado);
                        cmd.Parameters.AddWithValue("@Evento", 1);
                      
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se registro Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "3";
                        return View(EDoc);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListaDocumentoIdentidad");
            }

        }



        public ActionResult ModificarDocumentoIdentidad(string Id)
        {
            if (Session["UserID"] != null) { 
            var lista = (from x in ListadoDocumentoIdentidad() where x.CodDocIdent == Id select x).FirstOrDefault();
            return View(lista);
            }
            else
            {
                return RedirectToAction("ListaDocumentoIdentidad");

            }
        }

        [HttpPost]
        public ActionResult ModificarDocumentoIdentidad(E_Documento_Identidad EDoc)
        {
            string Modificar = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoDocumento_Identidad", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodDocIdent", EDoc.CodDocIdent);
                        cmd.Parameters.AddWithValue("@NomDocIdent", EDoc.NomDocIdent.ToUpper());
                        cmd.Parameters.AddWithValue("@Estado", EDoc.Estado);
                        cmd.Parameters.AddWithValue("@Evento", 2);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se registro Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al modificar : " + ex.Message.ToString();
                        return View(EDoc);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListaDocumentoIdentidad");
            }

        }

        public ActionResult Eliminar(string id)
        {
            string Modificar = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoDocumento_Identidad", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodDocIdent", id);
                        cmd.Parameters.AddWithValue("@NomDocIdent","");
                        cmd.Parameters.AddWithValue("@Estado","");
                        cmd.Parameters.AddWithValue("@Evento", 3);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se Elimino Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al modificar : " + ex.Message.ToString();
                        return View(id);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListaDocumentoIdentidad");
            }
        }


        public ActionResult Activar(string id)
        {
            string Modificar = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoDocumento_Identidad", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodDocIdent", id);
                        cmd.Parameters.AddWithValue("@NomDocIdent", "");
                        cmd.Parameters.AddWithValue("@Estado", "");
                        cmd.Parameters.AddWithValue("@Evento", 4);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se Elimino Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al modificar : " + ex.Message.ToString();
                        return View(id);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListaDocumentoIdentidad");
            }
        }


        public ActionResult ListaDocumentoIdentidad()
        {
            return View(ListadoDocumentoIdentidad());
        }

        public List<E_Documento_Identidad> ListadoDocumentoIdentidad()
        {
            List<E_Documento_Identidad> Lista = new List<E_Documento_Identidad>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Documento_Identidad", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Documento_Identidad EDoc = new E_Documento_Identidad();

                            EDoc.CodDocIdent = dr.GetString(0);
                            EDoc.NomDocIdent = dr.GetString(1).ToUpper();
                            EDoc.Estado = dr.GetBoolean(2);
                            Lista.Add(EDoc);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }



    }
}