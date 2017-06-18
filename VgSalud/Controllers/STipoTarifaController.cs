using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using VgSalud.Models;


namespace VgSalud.Controllers
{
    public class STipoTarifaController : Controller
    {
        // GET: STipoTarifa

        public ActionResult RegistrarSTipoTarifa()
        {
            TipoTarifaController TT = new TipoTarifaController();
            ViewBag.ListadoTipoTarifa = new SelectList(TT.ListadoTipoTarifa().Where(x=>x.EstTipTar == true).ToList(), "CodTipTar", "DescTipTar");
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarSTipoTarifa(E_Sub_Tipo_Tarifa ESTipTar)
        {
            TipoTarifaController TT = new TipoTarifaController();
            ViewBag.ListadoTipoTarifa = new SelectList(TT.ListadoTipoTarifa().Where(x=>x.EstTipTar == true).ToList(), "CodTipTar", "DescTipTar",ESTipTar.CodTipTar);
            string Crea = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoSubTipoTarifa", con))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@CodSTipTar", "");
                        cmd.Parameters.AddWithValue("@DescSTipTar", ESTipTar.DescSTipTar.ToUpper());
                        cmd.Parameters.AddWithValue("@CodTipTar", ESTipTar.CodTipTar);
                        cmd.Parameters.AddWithValue("@EstTipTar", true);
                        cmd.Parameters.AddWithValue("@Crea", Crea);
                        cmd.Parameters.AddWithValue("@Modifica", "");
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Evento", 1);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.mensaje = "1";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "3";
                        return View(ESTipTar);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListaSTipoTarifa");
            }
        }


        public ActionResult ModificarSTipoTarifa(string Id)
        {
            TipoTarifaController TT = new TipoTarifaController();
            ViewBag.ListadoTipoTarifa = new SelectList(TT.ListadoTipoTarifa().Where(x => x.EstTipTar == true).ToList(), "CodTipTar", "DescTipTar");

            var lista = (from x in ListadoSTipoTarifa() where x.CodSTipTar == Id select x).FirstOrDefault();
            return View(lista);

        }

        [HttpPost]
        public ActionResult ModificarSTipoTarifa(E_Sub_Tipo_Tarifa ESTipTar)
        {
            TipoTarifaController TT = new TipoTarifaController();
            ViewBag.ListadoTipoTarifa = new SelectList(TT.ListadoTipoTarifa().Where(x => x.EstTipTar == true).ToList(), "CodTipTar", "DescTipTar",ESTipTar.CodTipTar);

            string Modifica = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoSubTipoTarifa", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodSTipTar", ESTipTar.CodSTipTar);
                        cmd.Parameters.AddWithValue("@DescSTipTar", ESTipTar.DescSTipTar.ToUpper());
                        cmd.Parameters.AddWithValue("@CodTipTar", ESTipTar.CodTipTar);
                        cmd.Parameters.AddWithValue("@EstTipTar",ESTipTar.EstTipTar);
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", Modifica);
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Evento", 2);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se modifico Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al modificar : " + ex.Message.ToString();
                        return View(ESTipTar);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListaSTipoTarifa");
            }
        }


        public ActionResult ListaSTipoTarifa()
        {

            return View(ListadoSTipoTarifa());

        }

        public List<E_Sub_Tipo_Tarifa> ListadoSTipoTarifa()
        {
            List<E_Sub_Tipo_Tarifa> Lista = new List<E_Sub_Tipo_Tarifa>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_STipo_Tarifa", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Sub_Tipo_Tarifa Etip = new E_Sub_Tipo_Tarifa();

                            Etip.CodSTipTar = dr.GetString(0);
                            Etip.DescSTipTar = dr.GetString(1);
                            Etip.CodTipTar = dr.GetString(2);
                            Etip.EstTipTar = dr.GetBoolean(3);
                            Etip.TipoTarifa = dr.GetString(4);
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