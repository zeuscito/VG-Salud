using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using VgSalud.Models;
using System.Configuration;

namespace VgSalud.Controllers
{
    public class DocumentoSerieController : Controller
    {
        // GET: DocumentoSerie

        public ActionResult ListaDocumentoSerie()
        {
            return View(ListarDocumentoSerie());
        }

        public List<E_DocumentoSerie> ListarCorrelativo()
        {
            List<E_DocumentoSerie> Lista = new List<E_DocumentoSerie>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_CorrelativoCaja", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_DocumentoSerie Edoc = new E_DocumentoSerie();
                            Edoc.CodDocSerie = dr.GetString(0);
                            Edoc.CodDocCont = dr.GetInt32(1);
                            Edoc.Serie = dr.GetString(2);
                            Edoc.NumDoc = dr.GetString(3);
                            Edoc.CodSede = dr.GetString(4);
                            Edoc.EstDocSerie = dr.GetBoolean(5);
                            Edoc.DescCodDoc = dr.GetString(6).ToUpper();
                            Lista.Add(Edoc);
                        }
                        con.Close();
                    }
                }
            }
            return Lista;
        }

        public List<E_DocumentoSerie> ListarDocumentoSerie()
        {
            List<E_DocumentoSerie> Lista = new List<E_DocumentoSerie>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_DocumentoSeries", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_DocumentoSerie Edoc = new E_DocumentoSerie();
                            Edoc.CodDocSerie = dr.GetString(0);
                            Edoc.CodDocCont = dr.GetInt32(1);
                            Edoc.Serie = dr.GetString(2);
                            Edoc.NumDoc = dr.GetString(3);
                            Edoc.CodSede = dr.GetString(4);
                            Edoc.EstDocSerie = dr.GetBoolean(5);
                            Edoc.DescCodDoc = dr.GetString(6).ToUpper();
                            Edoc.SerieDocumento = dr.GetString(6).ToUpper() + " - " + dr.GetString(2);
                            Lista.Add(Edoc);
                        }
                        con.Close();
                    }
                }
            }
            return Lista;
        }

        public ActionResult RegistrarDocumentoSerie()
        {

            DocumentoContableController d = new DocumentoContableController();
            ViewBag.ListaDocumentoContable = new SelectList(d.ListaDocumentoContable().Where(x => x.EstCodDoc == true).ToList(), "CodDocCont", "DescCodDoc");

            SedesController Sede = new SedesController();
            ViewBag.ListaSedes = new SelectList(Sede.ListadoSedes().Where(x => x.EstSede == true), "CodSede", "NomSede");


            return View();
        }

        [HttpPost]
        public ActionResult RegistrarDocumentoSerie(E_DocumentoSerie EdSe)
        {
            string Crea = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;
            SedesController Sede = new SedesController();
            ViewBag.ListaSedes = new SelectList(Sede.ListadoSedes(), "CodSede", "NomSede", EdSe.CodSede);

            DocumentoContableController d = new DocumentoContableController();
            ViewBag.ListaDocumentoContable = new SelectList(d.ListaDocumentoContable(), "CodDocCont", "DescCodDoc", EdSe.CodDocCont);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_DocumentoSeries", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodDocSerie", "");
                        cmd.Parameters.AddWithValue("@CodDocCont", EdSe.CodDocCont);
                        cmd.Parameters.AddWithValue("@Serie", EdSe.Serie);
                        cmd.Parameters.AddWithValue("@NumDoc", EdSe.NumDoc);
                        cmd.Parameters.AddWithValue("@CodSede", EdSe.CodSede);
                        cmd.Parameters.AddWithValue("@EstDocSerie", EdSe.EstDocSerie);
                        cmd.Parameters.AddWithValue("@Crea", Crea);
                        cmd.Parameters.AddWithValue("@Modifica", "");
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Tipo", 1);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                        ViewBag.mensaje = "Bien";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                        return View(EdSe);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return RedirectToAction("ListaDocumentoSerie");
            }
        }


        public ActionResult ModificarDocumentoSerie(string Id)
        {

            SedesController Sede = new SedesController();
            ViewBag.ListaSedes = new SelectList(Sede.ListadoSedes().Where(x => x.EstSede == true), "CodSede", "NomSede");

            DocumentoContableController d = new DocumentoContableController();
            ViewBag.ListaDocumentoContable = new SelectList(d.ListaDocumentoContable(), "CodDocCont", "DescCodDoc");

            var fila = (from x in ListarDocumentoSerie() where x.CodDocSerie == Id select x).FirstOrDefault();
            return View(fila);
        }

        [HttpPost]
        public ActionResult ModificarDocumentoSerie(E_DocumentoSerie EdSe)
        {
            string Modifica = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            DocumentoContableController d = new DocumentoContableController();
            ViewBag.ListaDocumentoContable = new SelectList(d.ListaDocumentoContable(), "CodDocCont", "DescCodDoc", EdSe.CodDocCont);
            SedesController Sede = new SedesController();
            ViewBag.ListaSedes = new SelectList(Sede.ListadoSedes(), "CodSede", "NomSede", EdSe.CodSede);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_DocumentoSeries", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodDocSerie", EdSe.CodDocSerie);
                        cmd.Parameters.AddWithValue("@CodDocCont", EdSe.CodDocCont);
                        cmd.Parameters.AddWithValue("@Serie", EdSe.Serie);
                        cmd.Parameters.AddWithValue("@NumDoc", EdSe.NumDoc);
                        cmd.Parameters.AddWithValue("@CodSede", EdSe.CodSede);
                        cmd.Parameters.AddWithValue("@EstDocSerie", EdSe.EstDocSerie);
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", Modifica);
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Tipo", 2);

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        ViewBag.mensaje = "Bien";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al modificar : " + ex.Message.ToString();
                        return View(EdSe);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListaDocumentoSerie");
            }
        }







    }
}