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
    public class ConsultorioController : Controller
    {
        // GET: Consultorio


        public ActionResult RegistrarConsultorio()
        {
            string sede = Session["codSede"].ToString();
            ServiciosController Ser = new ServiciosController();
            ViewBag.ListaServicios = new SelectList(Ser.ListadoServicios().Where(x => x.EstServ == true && x.CodSede == sede).ToList(), "CodServ", "NomServ");

            return View();

        }

        [HttpPost]
        public ActionResult RegistrarConsultorio(E_Consultorio EConsul)
        {
            string sede = Session["codSede"].ToString();
            
            ServiciosController Ser = new ServiciosController();
            ViewBag.ListaServicios = new SelectList(Ser.ListadoServicios().Where(x => x.EstServ == true && x.CodSede == sede).ToList(), "CodServ", "NomServ", EConsul.CodServ);

            string Crea = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_Consultorio", con))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@IdConsul", "");
                        cmd.Parameters.AddWithValue("@DescConsul", EConsul.DescConsul.ToUpper());
                        cmd.Parameters.AddWithValue("@CodSede", sede);
                        cmd.Parameters.AddWithValue("@CodServ", EConsul.CodServ);
                        cmd.Parameters.AddWithValue("@Mixto", EConsul.Mixto);
                        cmd.Parameters.AddWithValue("@EstConsul",true );
                        cmd.Parameters.AddWithValue("@Crea", Crea);
                        cmd.Parameters.AddWithValue("@Modifica","" );
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Tipo", 1);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se registro Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "3";
                        return View(EConsul);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListaConsultorio");
            }

        }






        public ActionResult ModificarConsultorio(string Id)
        {
            string sede = Session["codSede"].ToString();
         
            ServiciosController Ser = new ServiciosController();
            ViewBag.ListaServicios = new SelectList(Ser.ListadoServicios().Where(x => x.EstServ == true && x.CodSede == sede).ToList(), "CodServ", "NomServ");

            var lista = (from x in ListadoConsultorio() where x.IdConsul == Id  select x).FirstOrDefault();
            return View(lista);

        }

        [HttpPost]
        public ActionResult ModificarConsultorio(E_Consultorio EConsul)
        {
            string sede = Session["codSede"].ToString();

            ServiciosController Ser = new ServiciosController();
            ViewBag.ListaServicios = new SelectList(Ser.ListadoServicios().Where(x => x.EstServ == true && x.CodSede == sede), "CodServ", "NomServ", EConsul.CodServ);


            string Modificar = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_Consultorio", con))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@IdConsul", EConsul.IdConsul);
                        cmd.Parameters.AddWithValue("@DescConsul", EConsul.DescConsul.ToUpper());
                        cmd.Parameters.AddWithValue("@CodSede", sede);
                        cmd.Parameters.AddWithValue("@CodServ", EConsul.CodServ);
                        cmd.Parameters.AddWithValue("@Mixto", EConsul.Mixto);
                        cmd.Parameters.AddWithValue("@EstConsul", EConsul.EstConsul);
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", Modificar);
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Tipo", 2);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se Modifico Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "3";
                        return View(EConsul);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListaConsultorio");
            }

        }


        public ActionResult ListaConsultorio()
        {
            string sede = Session["codSede"].ToString();
            return View(ListadoConsultorio().Where(x => x.CodSede == sede).ToList());
            
        }

        public List<E_Consultorio> ListadoConsultorio()
        {
            List<E_Consultorio> Lista = new List<E_Consultorio>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Consultorio", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Consultorio Consu = new E_Consultorio();

                            Consu.IdConsul = dr.GetString(0);
                            Consu.DescConsul = dr.GetString(1);
                            Consu.CodSede = dr.GetString(2);
                            Consu.CodServ = dr.GetString(3);
                            Consu.Mixto = dr.GetBoolean(4);
                            Consu.EstConsul = dr.GetBoolean(5);
                            Consu.NomServ = dr.GetString(6);

                            Lista.Add(Consu);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }




    }
}