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
    public class SedesEmpresaController : Controller
    {
        // GET: SedesEmpresa
        public ActionResult RegistrarSedesEmpresa()
        {
            SedesController Sed = new SedesController();
            ViewBag.ListaSedes = new SelectList(Sed.ListadoSedes().Where(x=>x.EstSede == true).ToList(), "CodSede", "NomSede");
            string sede = Session["codsede"].ToString();
            EmpresaTerceroController Emp = new EmpresaTerceroController();
            ViewBag.ListaEmpresaTercero = new SelectList(Emp.ListadoEmpresaTerceroSinSede().Where(x=>x.EstEmp == true).ToList(), "CodEmp", "RazonEmp");

            // ViewBag.ListaAsignados = ListadoSedesEmpresa();
            ViewBag.sedes = Sed.ListadoSedes().Where(x => x.EstSede == true).ToList();
            ViewBag.sedesEmpresa = ListadoSedesEmpresa();

            return View();
        }

        [HttpPost]
        public ActionResult RegistrarSedesEmpresa(E_SedesEmpresa Ese)
        {
            SedesController Sed = new SedesController();
            ViewBag.ListaSedes = new SelectList(Sed.ListadoSedes().Where(X=>X.EstSede == true), "CodSede", "NomSede",Ese.CodSede);

            EmpresaTerceroController Emp = new EmpresaTerceroController();
            ViewBag.ListaEmpresaTercero = new SelectList(Emp.ListadoEmpresaTerceroSinSede(), "CodEmp", "RazonEmp",Ese.CodEmp);

            ViewBag.sedes = Sed.ListadoSedes().Where(x => x.EstSede == true).ToList();
            ViewBag.sedesEmpresa = ListadoSedesEmpresa();

            try {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Usp_MtoSedEEmpresa", con))
                    {
                        try
                        {

                            cmd.Parameters.AddWithValue("@CodSede", Ese.CodSede);
                            cmd.Parameters.AddWithValue("@CodEmp", Ese.CodEmp);
                            cmd.Parameters.AddWithValue("@Evento", 1);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            ViewBag.Mensaje = "Se registro Satisfactoriamente";
                            ViewBag.sedesEmpresa = ListadoSedesEmpresa();
                            return View(Ese);
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Mensaje = "Error Datos [NO VALIDOS]";
                            return View(Ese);
                        }
                        finally
                        {
                            con.Close();

                        }
                    }
                    return View(Ese);
                }
            } catch (Exception) {
                ViewBag.mensaje = "Error Datos No Validos";
                return View();
            }

      
        }



        public ActionResult ModificarSedesEmpresa(string CodSede, string codEmp)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoSedEEmpresa", con))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@CodSede", CodSede);
                        cmd.Parameters.AddWithValue("@CodEmp", codEmp);
                        cmd.Parameters.AddWithValue("@Evento", 3);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se registro Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();

                    }
                    finally
                    {
                        con.Close();
                    }

                }
                return RedirectToAction("RegistrarSedesEmpresa");
            }
        }


        public ActionResult ListaSedesEmpresa()
        {
            return View(ListadoSedesEmpresa());
        }

        public List<E_SedesEmpresa> ListadoSedesEmpresa()
        {
            List<E_SedesEmpresa> Lista = new List<E_SedesEmpresa>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_SedesEmpresa", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_SedesEmpresa Esp = new E_SedesEmpresa();

                            Esp.CodSede = dr.GetString(0);
                            Esp.CodEmp = dr.GetString(1);
                            Esp.Sede = dr.GetString(2);
                            Esp.Empresa = dr.GetString(3);

                            Lista.Add(Esp);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }







    }
}