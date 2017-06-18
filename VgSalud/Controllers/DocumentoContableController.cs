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
    public class DocumentoContableController : Controller
    {
        // GET: DocumentoContable
        public ActionResult RegistrarDocumentoContable()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegistrarDocumentoContable(E_DocumentoContable DocI)
        {
            string crear = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString())){
                con.Open();
                using (SqlCommand da = new SqlCommand("usp_MtoTipoDocumento", con))
                {
                    try
                    {
                        
                    da.Parameters.AddWithValue("@codigo", "");
                    da.Parameters.AddWithValue("@descripcion", DocI.DescCodDoc.ToUpper());
                    da.Parameters.AddWithValue("@AliasCodDoc", DocI.AliasCodDoc.ToUpper());
                    da.Parameters.AddWithValue("@IncluRegVen", DocI.IncluRegVen);
                    da.Parameters.AddWithValue("@Estado", DocI.EstCodDoc);
                    da.Parameters.AddWithValue("@Crea", crear);
                    da.Parameters.AddWithValue("@Modifica", "");
                    da.Parameters.AddWithValue("@Elimina", "");
                    da.Parameters.AddWithValue("@Evento", "1");
                    da.CommandType = CommandType.StoredProcedure;

                    
                        da.ExecuteNonQuery();
                        ViewBag.mensaje = "Se registro correctamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                        return View(DocI);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListarDocumentoContable");
            }

        }



        public ActionResult ListarDocumentoContable()
        {
            return View(ListaDocumentoContable());
        }

        public List<E_DocumentoContable> ListaDocumentoContable()
        {
            List<E_DocumentoContable> Lista = new List<E_DocumentoContable>();
            using(SqlConnection con=new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using(SqlCommand da=new SqlCommand("Usp_Lista_General_TipoDocumentoTotal", con))
                {
                    da.CommandType = CommandType.StoredProcedure;
                    using(SqlDataReader dr = da.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_DocumentoContable Ed = new E_DocumentoContable();
                            Ed.CodDocCont = dr.GetInt32(0);
                            Ed.DescCodDoc = dr.GetString(1).ToUpper();
                            Ed.AliasCodDoc = dr.GetString(2).ToUpper().Trim();
                            Ed.IncluRegVen = dr.GetBoolean(3);
                            Ed.EstCodDoc = dr.GetBoolean(4);
                            Ed.Crea = (dr["Crea"] is DBNull) ? string.Empty : dr["Crea"].ToString(); ;
                            Ed.Modifica = (dr["Modifica"] is DBNull) ? string.Empty : dr["Modifica"].ToString();
                            Ed.Elimina = (dr["Elimina"] is DBNull) ? string.Empty : dr["Elimina"].ToString();

                            Lista.Add(Ed);
                            
                        }
                        con.Close();
                    }
                }
                
            }
            return Lista;
        }


        public ActionResult ModificarDocumentoContable(int Id)
        {
            var Lista = (from x in ListaDocumentoContable() where x.CodDocCont == Id select x).FirstOrDefault();
            return View(Lista);
        }
        [HttpPost]
        public ActionResult ModificarDocumentoContable(E_DocumentoContable DocI)
        {
             string modificar = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;
            using (SqlConnection con=new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using(SqlCommand da=new SqlCommand("usp_MtoTipoDocumento", con))
                {
                    try
                    {

                        da.Parameters.AddWithValue("@codigo", DocI.CodDocCont);
                        da.Parameters.AddWithValue("@descripcion", DocI.DescCodDoc.ToUpper());
                        da.Parameters.AddWithValue("@AliasCodDoc", DocI.AliasCodDoc.ToUpper().Trim());
                        da.Parameters.AddWithValue("@IncluRegVen", DocI.IncluRegVen);
                        da.Parameters.AddWithValue("@Estado", DocI.EstCodDoc);
                        da.Parameters.AddWithValue("@Crea", "");
                        da.Parameters.AddWithValue("@Modifica", modificar);
                        da.Parameters.AddWithValue("@Elimina", "");
                        da.Parameters.AddWithValue("@Evento", "2");
                        da.CommandType = CommandType.StoredProcedure;
                        
                    
                        da.ExecuteNonQuery();
                        ViewBag.mensaje = "Se registro correctamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al modificar : " + ex.Message.ToString();
                        return View(DocI);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListarDocumentoContable");
            }

        }

    }
}