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
    public class EmpresaTerceroController : Controller
    {
        // GET: EmpresaTercero
        public ActionResult RegistrarEmpresaTercero()
        {

            return View();
        }
        [HttpPost]
        public ActionResult RegistrarEmpresaTercero(E_Empresa_Tercero EEmp)
        {
            string Crea = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoEmpresa_Tercero", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodEmp", "");
                        cmd.Parameters.AddWithValue("@RazonEmp", EEmp.RazonEmp.ToUpper());
                        cmd.Parameters.AddWithValue("@RucEmp", EEmp.RucEmp.ToUpper());
                        cmd.Parameters.AddWithValue("@DireccEmp", EEmp.DireccEmp.ToUpper());
                        if (EEmp.NomGeren == null) { cmd.Parameters.AddWithValue("@NomGeren", ""); } else { cmd.Parameters.AddWithValue("@NomGeren", EEmp.NomGeren.ToUpper()); }
                        cmd.Parameters.AddWithValue("@NomContacto", EEmp.NomContacto.ToUpper());
                        cmd.Parameters.AddWithValue("@Tel1", EEmp.Tel1);
                        if (EEmp.Tel2 == null) { cmd.Parameters.AddWithValue("@Tel2", ""); } else { cmd.Parameters.AddWithValue("@Tel2", EEmp.Tel2); }
                        cmd.Parameters.AddWithValue("@Correo1", EEmp.Correo1.ToUpper());
                        if (EEmp.Correo2 == null) { cmd.Parameters.AddWithValue("@Correo2", ""); } else { cmd.Parameters.AddWithValue("@Correo2", EEmp.Correo2); }
                        cmd.Parameters.AddWithValue("@EstEmp", true);
                        cmd.Parameters.AddWithValue("@Crea", Crea);
                        cmd.Parameters.AddWithValue("@Modifica", "");
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Evento", 1);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "1";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.mensaje = "3";
                        return View();
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return RedirectToAction("ListaEmpresaTercero");
            }
        }
        public ActionResult ModificarEmpresaTercero(string Id)
        {
    
            var lista = (from x in ListadoEmpresaTerceroSinSede() where x.CodEmp == Id select x).FirstOrDefault();
            return View(lista);
        }

        [HttpPost]
        public ActionResult ModificarEmpresaTercero(E_Empresa_Tercero EEmp)
        {
            string modifica = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoEmpresa_Tercero", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodEmp", EEmp.CodEmp);
                        cmd.Parameters.AddWithValue("@RazonEmp", EEmp.RazonEmp.ToUpper());
                        cmd.Parameters.AddWithValue("@RucEmp", EEmp.RucEmp.ToUpper());
                        cmd.Parameters.AddWithValue("@DireccEmp", EEmp.DireccEmp.ToUpper());
                        if (EEmp.NomGeren == null) { cmd.Parameters.AddWithValue("@NomGeren", ""); } else { cmd.Parameters.AddWithValue("@NomGeren", EEmp.NomGeren.ToUpper()); }
                        cmd.Parameters.AddWithValue("@NomContacto", EEmp.NomContacto.ToUpper());
                        cmd.Parameters.AddWithValue("@Tel1", EEmp.Tel1);
                        if (EEmp.Tel2 == null) { cmd.Parameters.AddWithValue("@Tel2", ""); } else { cmd.Parameters.AddWithValue("@Tel2", EEmp.Tel2); }
                        cmd.Parameters.AddWithValue("@Correo1", EEmp.Correo1);
                        if (EEmp.Correo2 == null) { cmd.Parameters.AddWithValue("@Correo2", ""); } else { cmd.Parameters.AddWithValue("@Correo2", EEmp.Correo2); }
                        cmd.Parameters.AddWithValue("@EstEmp", true);
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", modifica);
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Evento", 2);
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
                return RedirectToAction("ListaEmpresaTercero");
            }
        }



        public ActionResult ListaEmpresaTercero()
        {
            string sede = Session["codSede"].ToString();
            return View(ListadoEmpresaTerceroSinSede());
        }

        public List<E_Empresa_Tercero> ListadoEmpresaTercero(string codsede)
        {
            List<E_Empresa_Tercero> Lista = new List<E_Empresa_Tercero>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Empresa_Tercero", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codsede", codsede);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Empresa_Tercero Emp = new E_Empresa_Tercero();

                            Emp.CodEmp = dr.GetString(0);
                            Emp.RazonEmp = dr.GetString(1).ToUpper();
                            Emp.RucEmp = dr.GetString(2);
                            Emp.DireccEmp = dr.GetString(3);
                            Emp.NomGeren = dr.GetString(4);
                            Emp.NomContacto = dr.GetString(5);
                            Emp.Tel1 = dr.GetString(6);
                            Emp.Tel2 = dr.GetString(7);
                            Emp.Correo1 = dr.GetString(8);
                            Emp.Correo2 = dr.GetString(9);
                            Emp.EstEmp = dr.GetBoolean(10);

                            Lista.Add(Emp);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_Empresa_Tercero> ListadoEmpresaTerceroSinSede()
        {
            List<E_Empresa_Tercero> Lista = new List<E_Empresa_Tercero>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Empresa_Tercero_SinSede", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Empresa_Tercero Emp = new E_Empresa_Tercero();

                            Emp.CodEmp = dr.GetString(0);
                            Emp.RazonEmp = dr["RazonEmp"] is DBNull ? string.Empty : dr["RazonEmp"].ToString().ToUpper();
                            Emp.RucEmp = dr["RucEmp"] is DBNull ? string.Empty : dr["RucEmp"].ToString().ToUpper();
                            Emp.DireccEmp = dr["DireccEmp"] is DBNull ? string.Empty : dr["DireccEmp"].ToString().ToUpper() ;
                            Emp.NomGeren = dr["NomGeren"] is DBNull ? string.Empty : dr["NomGeren"].ToString().ToUpper(); 
                            Emp.NomContacto = dr["NomContacto"] is DBNull ? string.Empty : dr["NomContacto"].ToString().ToUpper();
                            Emp.Tel1 = dr.GetString(6);
                            Emp.Tel2 = dr.GetString(7);
                            Emp.Correo1 = dr["Correo1"] is DBNull ? string.Empty : dr["Correo1"].ToString().ToUpper();
                            Emp.Correo2 = dr["Correo2"] is DBNull ? string.Empty : dr["Correo2"].ToString().ToUpper();
                            Emp.EstEmp = dr.GetBoolean(10);

                            Lista.Add(Emp);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public ActionResult EliminarEmpresaTercero(string id)
        {
            string Elimina = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoEmpresa_Tercero", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodEmp", id);
                        cmd.Parameters.AddWithValue("@RazonEmp", "");
                        cmd.Parameters.AddWithValue("@RucEmp", "");
                        cmd.Parameters.AddWithValue("@DireccEmp", "");
                        cmd.Parameters.AddWithValue("@NomGeren", "");
                        cmd.Parameters.AddWithValue("@NomContacto", "");
                        cmd.Parameters.AddWithValue("@Tel1", "");
                        cmd.Parameters.AddWithValue("@Tel2", "");
                        cmd.Parameters.AddWithValue("@Correo1", "");
                        cmd.Parameters.AddWithValue("@Correo2", "");
                        cmd.Parameters.AddWithValue("@EstEmp", true);
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", "");
                        cmd.Parameters.AddWithValue("@Elimina", Elimina);
                        cmd.Parameters.AddWithValue("@Evento", 3);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se eliminó Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al eliminar : " + ex.Message.ToString();

                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return RedirectToAction("ListaEmpresaTercero");
            }
        }

        public ActionResult ActivarEmpresaTercero(string id)
        {
            string Activar = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoEmpresa_Tercero", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodEmp", id);
                        cmd.Parameters.AddWithValue("@RazonEmp", "");
                        cmd.Parameters.AddWithValue("@RucEmp", "");
                        cmd.Parameters.AddWithValue("@DireccEmp", "");
                        cmd.Parameters.AddWithValue("@NomGeren", "");
                        cmd.Parameters.AddWithValue("@NomContacto", "");
                        cmd.Parameters.AddWithValue("@Tel1", "");
                        cmd.Parameters.AddWithValue("@Tel2", "");
                        cmd.Parameters.AddWithValue("@Correo1", "");
                        cmd.Parameters.AddWithValue("@Correo2", "");
                        cmd.Parameters.AddWithValue("@EstEmp", true);
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", Activar);
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Evento", 4);
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
                return RedirectToAction("ListaEmpresaTercero");
            }
        }



    }
}