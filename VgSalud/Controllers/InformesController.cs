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
    public class InformesController : Controller
    {
        

      public ActionResult ListaPacienteResultadoApto(string nombre=null,string dni=null)
        {
            ViewBag.nombre = nombre;
            ViewBag.dni = dni;

            return View(ListaFiltroPaciente(nombre,dni));
        }
        public List<E_Informes> ListaFiltroPaciente(string nombre = null,string dni=null)
        {/*var usuario = Session["usuario"].ToString();*/
            List<E_Informes> lista = new List<E_Informes>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd  =new SqlCommand("Usp_Busca_PacienteApto_Medicina", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (dni == null || dni == "")
                    {
                        cmd.Parameters.AddWithValue("@dni", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@dni", dni);
                    }
                    if (nombre == null || nombre == "")
                    {
                        cmd.Parameters.AddWithValue("@nombre", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                    }
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Informes inf = new E_Informes();
                            inf.NroCarnet = dr.GetInt32(0);
                            inf.NumDoc = dr.GetString(1);
                            inf.ApePat = dr.GetString(2);
                            inf.ApeMat = dr.GetString(3);
                            inf.NomPac = dr.GetString(4);
                            inf.FechaAtenLab = dr.GetDateTime(5);
                            inf.ObservacionLab = dr.GetString(6);
                            inf.ReevaluadoLab = dr.GetString(7);
                            inf.FechaAtenOdon = dr.GetDateTime(8);
                            inf.ObservacionOdon = dr.GetString(9);
                            inf.ReevaluadoOndon = dr.GetString(10);
                            inf.AptoLab = dr.GetString(11);
                            inf.AptoOdon = dr.GetString(12);
                            inf.AptoMed = dr.GetString(13);
                            lista.Add(inf);
                        }
                        con.Close();
                    }
                }
                return lista;
            }
        }
        public ActionResult PasarMedicina(E_CSMedicina med,string id)
        {
            string sede = Session["codSede"].ToString();
            string Modifica = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;
            
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ActualizarDatosCSMedicina", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id",id);
                    cmd.Parameters.AddWithValue("@FechaAten","");
                    cmd.Parameters.AddWithValue("@Prioridad", 2);
                    cmd.Parameters.AddWithValue("@Estado",5);
                    cmd.Parameters.AddWithValue("@Modifica", Modifica);
                    cmd.Parameters.AddWithValue("@Sedes", sede);
                }
            }
            return RedirectToAction("ListaPacienteResultadoApto");
        }
    }
}