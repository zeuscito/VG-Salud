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
    public class MedioPagoController : Controller
    {



        public List<E_Medios_Pago> listaMedioPago()
        {
            List<E_Medios_Pago> Lista = new List<E_Medios_Pago>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Medios_pago", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Medios_Pago medio = new E_Medios_Pago();

                            medio.CODMEDIOS = dr.GetString(0);
                            medio.DESCRIPCION = dr.GetString(1);
                            medio.ESTADO = dr.GetBoolean(2);
                            
                            Lista.Add(medio);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }





        public ActionResult ListarMedioPagos()
        {
            return View(listaMedioPago());
        }


        public ActionResult RegistrarMedioPago()
        {
          
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarMedioPago(E_Medios_Pago med)
        {


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Medios_Pago", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@codmedios", "");
                        da.Parameters.AddWithValue("@descripcion", med.DESCRIPCION.ToUpper());
                        da.Parameters.AddWithValue("@estado", med.ESTADO);
                        da.Parameters.AddWithValue("@Evento",1);
                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "3";

                        return View(med);
                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListarMedioPagos");

        }

        public ActionResult Activar(string id)
        {


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Medios_Pago", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;


                        da.Parameters.AddWithValue("@codmedios", id);
                        da.Parameters.AddWithValue("@descripcion", "");
                        da.Parameters.AddWithValue("@estado", "");
                        da.Parameters.AddWithValue("@Evento", 4);



                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {

                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListarMedioPagos");
        }



        public ActionResult Eliminar(string id)
        {


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Medios_Pago", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;


                        da.Parameters.AddWithValue("@codmedios",id);
                        da.Parameters.AddWithValue("@descripcion", "");
                        da.Parameters.AddWithValue("@estado","");
                        da.Parameters.AddWithValue("@Evento", 3);



                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {

                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListarMedioPagos");
        }


        public ActionResult ModificarMediosPago(string id)
        {

            var lista = (from x in listaMedioPago() where x.CODMEDIOS == id select x).FirstOrDefault();

            return View(lista);
        }


        [HttpPost]
        public ActionResult ModificarMediosPago(E_Medios_Pago med)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Medios_Pago", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure; 
                        da.Parameters.AddWithValue("@codmedios", med.CODMEDIOS);
                        da.Parameters.AddWithValue("@descripcion", med.DESCRIPCION.ToUpper());
                        da.Parameters.AddWithValue("@estado", med.ESTADO);
                        da.Parameters.AddWithValue("@Evento", 2);

                        da.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {

                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListarMedioPagos");
        }




    }
}