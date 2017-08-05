using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VgSalud.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace VgSalud.Controllers
{
    public class EspecialidadController : Controller
    {

        // GET: Especialidad

        public ActionResult RegistrarEspecialidad()
        {
            string sede = Session["codSede"].ToString();
            TarifarioController t = new TarifarioController();
            ViewBag.Tarifario = new SelectList(t.ListadoTarifa().Where(x => x.EstTar == true && x.CodSede == sede), "CodTar", "DescTar");
            return View();

        }

        [HttpPost]
        public ActionResult RegistrarEspecialidad(E_Especialidades EEsp)
        {
            string sede = Session["codSede"].ToString();
            var verifica = (List<E_Especialidades>)ListadoEspecialidades().Where(x => x.CodSed == sede).ToList();

            bool resultado = false;

            foreach (var i in verifica)
            {
                if (i.General == true)
                {
                    resultado = true;
                    break;
                }
                else
                {
                    resultado = false;
                }
            }

            if (EEsp.General == true && resultado == true)
            {

                TarifarioController t = new TarifarioController();
                ViewBag.Tarifario = new SelectList(t.ListadoTarifa().Where(x => x.EstTar == true && x.CodSede == sede), "CodTar", "DescTar", EEsp.CodTar);

                ViewBag.Mensaje = "Error, ya esta asignado la especialidad general.";
                return View(EEsp);

            }
            else
            {

                TarifarioController t = new TarifarioController();
                ViewBag.Tarifario = new SelectList(t.ListadoTarifa().Where(x => x.EstTar == true && x.CodSede == sede), "CodTar", "DescTar", EEsp.CodTar);

                string Crea = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_MtoEspecialidades", con))
                    {
                        try
                        {
                            cmd.Parameters.AddWithValue("@CodEspec", "");
                            cmd.Parameters.AddWithValue("@NomEspec", EEsp.NomEspec.ToUpper());
                            cmd.Parameters.AddWithValue("@DescEspec", EEsp.DescEspec.ToUpper());
                            cmd.Parameters.AddWithValue("@CodSede", sede);
                            if (EEsp.CodTar == null)
                            {
                                cmd.Parameters.AddWithValue("@CodTar", "");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@CodTar", EEsp.CodTar);
                            }
                            cmd.Parameters.AddWithValue("@EstEspec", EEsp.EstEspec);
                            cmd.Parameters.AddWithValue("@Crea", Crea);
                            cmd.Parameters.AddWithValue("@Modifica", "");
                            cmd.Parameters.AddWithValue("@Elimina", "");
                            cmd.Parameters.AddWithValue("@Evento", 1);
                            cmd.Parameters.AddWithValue("@General", EEsp.General);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            ViewBag.Mensaje = "Se registro Satisfactoriamente";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Mensaje = "3";
                            return View(EEsp);
                        }
                        finally
                        {
                            con.Close();

                        }
                    }
                    return RedirectToAction("ListaEspecialidad");
                }
            }

        }


        public ActionResult ModificarEspecialidad(string Id)
        {
            string sede = Session["codSede"].ToString();

            TarifarioController t = new TarifarioController();
            ViewBag.Tarifario = new SelectList(t.ListadoTarifa().Where(x => x.EstTar == true && x.CodSede == sede), "CodTar", "DescTar");
            var lista = (from x in ListadoEspecialidades() where x.CodEspec == Id select x).FirstOrDefault();
            return View(lista);

        }

        [HttpPost]
        public ActionResult ModificarEspecialidad(E_Especialidades EEsp)
        {
            string sede = Session["codSede"].ToString();
            var verifica = (List<E_Especialidades>)ListadoEspecialidades().Where(x => x.CodSed == sede).ToList();

            bool resultado = false;
            string codigo = "";
            foreach (var i in verifica)
            {
                if (i.General == true)
                {
                    resultado = true;
                    codigo = i.CodEspec;
                    break;
                }
                else
                {
                    resultado = false;
                }
            }
            if (codigo != EEsp.CodEspec && EEsp.General == true && resultado == true)
            {
                TarifarioController t = new TarifarioController();
                ViewBag.Tarifario = new SelectList(t.ListadoTarifa().Where(x => x.EstTar == true && x.CodSede == sede), "CodTar", "DescTar", EEsp.CodTar);

                ViewBag.Mensaje = "Error, ya esta asignado la especialidad general.";
                return View(EEsp);

            }
            else
            {

                TarifarioController t = new TarifarioController();
                ViewBag.Tarifario = new SelectList(t.ListadoTarifa().Where(x => x.EstTar == true && x.CodSede == sede), "CodTar", "DescTar", EEsp.CodTar);
                string Modificar = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_MtoEspecialidades", con))
                    {
                        try
                        {

                            cmd.Parameters.AddWithValue("@CodEspec", EEsp.CodEspec);
                            cmd.Parameters.AddWithValue("@NomEspec", EEsp.NomEspec.ToUpper());
                            cmd.Parameters.AddWithValue("@DescEspec", EEsp.DescEspec.ToUpper());
                            cmd.Parameters.AddWithValue("@CodSede", sede);
                            if (EEsp.CodTar == null)
                            {
                                cmd.Parameters.AddWithValue("@CodTar", "");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@CodTar", EEsp.CodTar);
                            }
                            cmd.Parameters.AddWithValue("@EstEspec", EEsp.EstEspec);
                            cmd.Parameters.AddWithValue("@Crea", "");
                            cmd.Parameters.AddWithValue("@Modifica", Modificar);
                            cmd.Parameters.AddWithValue("@Elimina", "");
                            cmd.Parameters.AddWithValue("@Evento", 2);
                            cmd.Parameters.AddWithValue("@General", EEsp.General);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            ViewBag.Mensaje = "Se registro Satisfactoriamente";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Mensaje = "Ocurrio algun error al modificar : " + ex.Message.ToString();
                            return View(EEsp);
                        }
                        finally
                        {
                            con.Close();

                        }
                    }
                    return RedirectToAction("ListaEspecialidad");
                }
            }

        }


        public ActionResult ListaEspecialidad()
        {
            string sede = Session["codSede"].ToString();
            return View(ListadoEspecialidades().Where(x => x.CodSed == sede).ToList());

        }

        public List<E_Especialidades> ListadoEspecialidades()
        {
            List<E_Especialidades> Lista = new List<E_Especialidades>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Especialidades", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Especialidades Esp = new E_Especialidades();

                            Esp.CodEspec = dr.GetString(0);
                            Esp.NomEspec = dr["NomEspec"] is DBNull ? string.Empty : dr["NomEspec"].ToString().ToUpper();
                            Esp.DescEspec = dr["DescEspec"] is DBNull ? string.Empty : dr["DescEspec"].ToString().ToUpper();
                            Esp.EstEspec = dr.GetBoolean(3);
                            Esp.CodTar = dr.GetString(4);
                            Esp.CodSed = dr.GetString(5);
                            Esp.General = dr.GetBoolean(6);

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