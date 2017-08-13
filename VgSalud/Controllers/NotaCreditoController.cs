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
using System.Net;

namespace VgSalud.Controllers
{
    public class NotaCreditoController : Controller
    {

        public ActionResult BuscaCaja(int? CodCaja = null)
        {
            string usuario = Session["UserID"].ToString();
            CajaController c = new CajaController();
            UtilitarioController ut = new UtilitarioController();
            E_Master hora = ut.ListadoHoraServidor().FirstOrDefault();

            ViewBag.hoy = hora.HoraServidor.ToShortDateString();


            var listaSerie = (List<E_UsuarioSerie>)c.ListadoUsuarioSerie(usuario).Where(x => x.CodDocCont == 7).ToList();
            ViewBag.listaSerie = listaSerie;

            if (listaSerie.Count() == 0)
            {
                ViewBag.mensaje = "El usuario no esta asignado a ninguna nota de credito";
                return View();
            }

            return View();
        }


        [HttpPost]
        public ActionResult BuscaCaja(E_Caja c)
        {

            string usuario = Session["UserID"].ToString();

            UtilitarioController ut = new UtilitarioController();
            DocumentoSerieController ds = new DocumentoSerieController();
            E_Master hora = ut.ListadoHoraServidor().FirstOrDefault();
            CajaController ca = new CajaController();

            ViewBag.hoy = hora.HoraServidor.ToShortDateString();

            ViewBag.listaCabeceraa = (List<E_Caja>)ca.ListadoCajaCabecera().Where(x => x.CodCaja == c.CodCaja).ToList();
            ViewBag.listaDetalle = (List<E_CajaDetalle>)ca.ListadoCajaDetalle().Where(x => x.CodCaja == c.CodCaja).ToList();
            
            ViewBag.CodCaja = "" + c.CodCaja;
            
            var listaSerie = (List<E_UsuarioSerie>)ca.ListadoUsuarioSerie(usuario).Where(x => x.CodDocCont == 7).ToList();
            ViewBag.listaSerie = listaSerie;

            string strHostName = string.Empty;

            string Crea = Session["usuario"] + " " + hora.HoraServidor.ToString() + " " + Dns.GetHostName();

            if (c.evento == 1)
            {
                if (listaSerie.Count() == 0)
                {
                    ViewBag.mensaje = "El usuario no esta asignado a ninguna nota de credito";
                    return View();
                }

                E_Caja caja = ca.ListadoCajaCabecera().Find(x => x.CodCaja == c.CodCaja);

                if (caja != null)
                {
                    if (caja.Estado == true)
                    {

                        ViewBag.listaCabecera = (List<E_Caja>)ca.ListadoCajaCabecera().Where(x => x.CodCaja == c.CodCaja).ToList();
                        ViewBag.listaDetalle = (List<E_CajaDetalle>)ca.ListadoCajaDetalle().Where(x => x.CodCaja == c.CodCaja).ToList();
                        ViewBag.CodCaja = "" + c.CodCaja;

                    }
                    else
                    {
                        ViewBag.listaCabecera = null;
                        ViewBag.listaDetalle = null;
                    }
                }
                else
                {
                    ViewBag.listaCabecera = null;
                    ViewBag.listaDetalle = null;
                }

                return View();

            }

            if (c.evento == 2)
            {
                E_Caja caja = ca.ListadoCajaCabecera().Find(x => x.CodCaja == c.CodCaja);
               
                E_DocumentoSerie docS = ds.ListarDocumentoSerie().Where(x => x.CodDocSerie == c.Serie).FirstOrDefault();
                E_DocumentoSerie docSCaja = ds.ListarDocumentoSerie().Where(x => x.CodDocSerie == caja.CodDocSerie).FirstOrDefault();
                DocumentoContableController dc = new DocumentoContableController();
                E_DocumentoContable docCon = dc.ListaDocumentoContable().Find(x => x.CodDocCont==docS.CodDocCont);
                E_DocumentoContable docConCaja = dc.ListaDocumentoContable().Find(x => x.CodDocCont == docSCaja.CodDocCont);
                


                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                    con.Open();
                    try
                    {
                        SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                        using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_NotaCredito", con, tr))
                        {
                            E_DocumentoSerie correlativo = ca.ListadoCorrelativo(c.Serie, con, tr).FirstOrDefault();
                            da.CommandType = CommandType.StoredProcedure;
                            da.Parameters.AddWithValue("@CodNotaCre", "");
                            da.Parameters.AddWithValue("@CodCaja", c.CodCaja);
                            da.Parameters.AddWithValue("@CodDocSerie", c.Serie);
                            da.Parameters.AddWithValue("@Serie", docS.Serie);
                            da.Parameters.AddWithValue("@NumDoc", correlativo.Serie);
                            da.Parameters.AddWithValue("@TipoDoc", docCon.AliasCodDoc);
                            da.Parameters.AddWithValue("@FechaEmision", c.FechaEmision);
                            da.Parameters.AddWithValue("@Total", c.Total);
                            da.Parameters.AddWithValue("@Usuario", usuario);
                            da.Parameters.AddWithValue("@Estado", "1");                            
                            da.Parameters.AddWithValue("@CodDocSerieC", caja.CodDocSerie);
                            da.Parameters.AddWithValue("@SerieC", caja.Serie);
                            da.Parameters.AddWithValue("@NumDocC", caja.NumDoc);
                            da.Parameters.AddWithValue("@TipoDocC", docConCaja.AliasCodDoc);
                            da.Parameters.AddWithValue("@TipoPago", caja.TipoPago);
                            da.Parameters.AddWithValue("@Paciente", caja.RazonSocial);
                            da.Parameters.AddWithValue("@CodCatePac", caja.CodCatPac);
                            da.Parameters.AddWithValue("@CatePac", c.DescCatPac);
                            da.Parameters.AddWithValue("@UsuCaja", caja.UsuCrea);
                            da.Parameters.AddWithValue("@Ruc", caja.Ruc);
                            da.Parameters.AddWithValue("@SubTotal", caja.SubTotal);
                            da.Parameters.AddWithValue("@Igv", caja.Igv);
                            da.Parameters.AddWithValue("@TotalC", caja.Total);
                            da.Parameters.AddWithValue("@Tasa", caja.TazaIgv);
                            da.Parameters.AddWithValue("@Crea", Crea);
                            da.Parameters.AddWithValue("@Modifica", "");
                            da.Parameters.AddWithValue("@Elimina", "");
                            da.Parameters.AddWithValue("@Tipo", "1");

                            da.ExecuteNonQuery();

                        }
                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = e.Message;
                        return View();
                    }
                }

                ViewBag.mensaje = "1";
            }

            return View();

        }





    }
}