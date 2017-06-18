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
    public class TipoTarifaController : Controller
    {
        // GET: TipoTarifa
        public ActionResult RegistrarTipoTarifa()
        {
            ViewBag.modulo = new SelectList(ListadoModulos(), "Modulo", "DescMod");
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarTipoTarifa(E_Tipo_Tarifa ETipTar)
        {
        
            string Crea = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;
            ViewBag.modulo = new SelectList(ListadoModulos(), "Modulo", "DescMod");
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoTipo_Tarifa", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodTipTar", "");
                        cmd.Parameters.AddWithValue("@DescTipTar", ETipTar.DescTipTar.ToUpper());
                        cmd.Parameters.AddWithValue("@EstTipTar", true);
                        cmd.Parameters.AddWithValue("@Crea", Crea);
                        cmd.Parameters.AddWithValue("@Modifica", "");
                        cmd.Parameters.AddWithValue("@Elimina", ""); 
                        cmd.Parameters.AddWithValue("@AtencionRapida",ETipTar.AtencionRapida);
                        cmd.Parameters.AddWithValue("@Modulo", ETipTar.Modulo);
                        cmd.Parameters.AddWithValue("@Evento", 1);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se registro Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                        return View(ETipTar);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListaTipoTarifa");
            }
        }


        public ActionResult ModificarTipoTarifa(string Id)
        {
            var lista = (from x in ListadoTipoTarifa() where x.CodTipTar == Id select x).FirstOrDefault();
            ViewBag.codigo = lista.Modulo;
            ViewBag.modulo = new SelectList(ListadoModulos(), "Modulo", "DescMod");
            return View(lista);
            
        }

        [HttpPost]
        public ActionResult ModificarTipoTarifa(E_Tipo_Tarifa ETipTar)
        {
            string Modificar = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;
            ViewBag.modulo = new SelectList(ListadoModulos(), "Modulo", "DescMod",ETipTar.Modulo);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoTipo_Tarifa", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodTipTar", ETipTar.CodTipTar);
                        cmd.Parameters.AddWithValue("@DescTipTar", ETipTar.DescTipTar.ToUpper());
                        cmd.Parameters.AddWithValue("@EstTipTar", ETipTar.EstTipTar);
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", Modificar);
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@AtencionRapida", ETipTar.AtencionRapida);
                        cmd.Parameters.AddWithValue("@Modulo", ETipTar.Modulo);
                        cmd.Parameters.AddWithValue("@Evento", 2);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se modifico Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Error Datos No Validos";
                        return View(ETipTar);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListaTipoTarifa");
            }
        }


        public ActionResult ListaTipoTarifa()
        {
            
                return View(ListadoTipoTarifa());
           
        }

        public List<E_Tipo_Tarifa> ListadoTipoTarifa()
        {
            List<E_Tipo_Tarifa> Lista = new List<E_Tipo_Tarifa>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_TipoTarifa", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Tipo_Tarifa Etip = new E_Tipo_Tarifa();

                            Etip.CodTipTar = dr.GetString(0);
                            Etip.DescTipTar = dr.GetString(1);
                            Etip.EstTipTar = dr.GetBoolean(2);
                            Etip.AtencionRapida = dr["AtencionRapida"] is DBNull ? false : dr.GetBoolean(6) ;
                            Etip.Modulo = dr["Modulo"] is DBNull ? 0 : dr.GetInt32(7);
                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Tipo_Tarifa> ListadoModulos()
        {
            List<E_Tipo_Tarifa> Lista = new List<E_Tipo_Tarifa>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Modulos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Tipo_Tarifa Etip = new E_Tipo_Tarifa();

                            Etip.Modulo = dr.GetInt32(0);
                            Etip.DescMod = dr.GetString(1);
                            Etip.url = dr.GetString(2);
                            Etip.urlImprime = dr.GetString(3);
                            Etip.urlImprimeReceta = dr.GetString(4);

                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

    }
}