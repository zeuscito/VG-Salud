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
    public class ServicioTipoTarifaController : Controller
    {

        public ActionResult ListarServiciosTipoTarifas()
        {

            return View(listaservicioTipoTarifa());
        }

        public List<E_Servicio_TipoTarifa> listaservicioTipoTarifa()
        {

            List<E_Servicio_TipoTarifa> Lista = new List<E_Servicio_TipoTarifa>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("listar_servicio_tipo_tarifa", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Servicio_TipoTarifa Etip = new E_Servicio_TipoTarifa();

                            Etip.codserv = dr.GetString(0);
                            Etip.nomser = dr.GetString(1);
                            Etip.CodtipoTar = dr.GetString(2).ToUpper();
                            Etip.DescTipTar = dr.GetString(3).ToUpper();
                            Etip.porcentaje = dr.GetDecimal(4);
                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }

            }
            return Lista;

        }



        public ActionResult RegistrarServicioTipoTarifa()
        {
            TipoTarifaController tp = new TipoTarifaController();
            ServiciosController tt = new ServiciosController();
           
            ViewBag.servicio = new SelectList(tt.ListadoServicios().Where(x => x.EstServ == true), "CodServ", "NomServ");
            ViewBag.tipotarifa = new SelectList(tp.ListadoTipoTarifa().Where(x => x.EstTipTar==true), "CodTipTar", "DescTipTar");
            return View();
        }




        public ActionResult ModificarServicioTipoTarifa(string id, string id1)
        {
            string sede = Session["codSede"].ToString();
            TipoTarifaController tp = new TipoTarifaController();
            ServiciosController tt = new ServiciosController();
            ViewBag.servicio = new SelectList(tt.ListadoServicios().Where(x=>x.CodSede == sede), "CodServ", "NomServ");
            ViewBag.tipotarifa = new SelectList(tp.ListadoTipoTarifa().Where(x => x.EstTipTar == true), "CodTipTar", "DescTipTar");
            var lista = (from x in listaservicioTipoTarifa() where x.codserv == id && x.CodtipoTar == id1 select x).FirstOrDefault();
            return View(lista);

        }


        public ActionResult Eliminar(string id, string id1)
        {

            string elimina = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Mantenimiento_servicio_TipoTarifa", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodServ", id);
                        cmd.Parameters.AddWithValue("@CodTipTar", id1);
                        cmd.Parameters.AddWithValue("@porcentaje", "");
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", "");
                        cmd.Parameters.AddWithValue("@Elimina", elimina);
                        cmd.Parameters.AddWithValue("@Evento", 3);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se actualizo Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                        return View(id);
                    }
                    finally
                    {
                        con.Close();

                    }
                }


            }


            return RedirectToAction("ListarServiciosTipoTarifas");

        }
                        


        [HttpPost]
        public ActionResult ModificarServicioTipoTarifa(E_Servicio_TipoTarifa ms)
        {

            string modifica = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;
            TipoTarifaController tp = new TipoTarifaController();
            ServiciosController tt = new ServiciosController();

            ViewBag.servicio = new SelectList(tt.ListadoServicios().Where(x => x.EstServ == true), "CodServ", "NomServ", ms.codserv);
            ViewBag.tipotarifa = new SelectList(tp.ListadoTipoTarifa().Where(x => x.EstTipTar == true), "CodTipTar", "DescTipTar", ms.CodtipoTar);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Mantenimiento_servicio_TipoTarifa", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodServ", ms.codserv);
                        cmd.Parameters.AddWithValue("@CodTipTar",ms.CodtipoTar);
                        cmd.Parameters.AddWithValue("@porcentaje", ms.porcentaje);
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", modifica);
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Evento", 2);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se actualizo Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                        return View(ms);
                    }
                    finally
                    {
                        con.Close();

                    }
                }


            }


            return RedirectToAction("ListarServiciosTipoTarifas");

        }

        [HttpPost]
        public ActionResult RegistrarServicioTipoTarifa(E_Servicio_TipoTarifa ms)
        {

            string Crea = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            TipoTarifaController tp = new TipoTarifaController();
            ServiciosController tt = new ServiciosController();

            ViewBag.servicio = new SelectList(tt.ListadoServicios().Where(x => x.EstServ == true), "CodServ", "NomServ",ms.codserv);
            ViewBag.tipotarifa = new SelectList(tp.ListadoTipoTarifa().Where(x => x.EstTipTar == true), "CodTipTar", "DescTipTar",ms.CodtipoTar);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Mantenimiento_servicio_TipoTarifa", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodServ", ms.codserv);
                        cmd.Parameters.AddWithValue("@CodTipTar", ms.CodtipoTar);
                        cmd.Parameters.AddWithValue("@porcentaje", ms.porcentaje);
                        cmd.Parameters.AddWithValue("@Crea", Crea);
                        cmd.Parameters.AddWithValue("@Modifica","");
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Evento", 1);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se registro Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                        return View(ms);
                    }
                    finally
                    {
                        con.Close();

                    }
                }


            }
            return RedirectToAction("ListarServiciosTipoTarifas");

        }


    }
}