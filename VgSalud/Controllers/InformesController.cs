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


        public ActionResult ListaPacienteResultadoApto(string nombre = "", string dni = "")
        {
            //ViewBag.nombre = nombre;
            //ViewBag.dni = dni;

            return View(ListaFiltroPaciente(nombre, dni));
        }
        public List<E_Informes> ListaFiltroPaciente(string nombre, string dni)
        {/*var usuario = Session["usuario"].ToString();*/
            List<E_Informes> lista = new List<E_Informes>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
               
                using (SqlCommand cmd = new SqlCommand("Usp_Busca_PacienteApto_MedicinaS", con))
                {
                
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@dni", dni);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Informes inf = new E_Informes();
                            inf.Carnet = dr.GetString(0);
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
                            inf.IdMedicina = dr.GetInt32(14);
                            inf.Historia = dr.GetInt32(15);
                            lista.Add(inf);
                        }
                        con.Close();
                    }
                }
                return lista;


            }
        }


        public List<E_Informes> Usp_Informes_Pacientes_Aptos(int historia , string codsede)
        {/*var usuario = Session["usuario"].ToString();*/
            List<E_Informes> lista = new List<E_Informes>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Informes_Pacientes_Aptos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@historia", historia);
                    cmd.Parameters.AddWithValue("@CodSede", codsede);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Informes inf = new E_Informes();
                            inf.AptoLab = dr.GetString(0);
                            inf.AptoOdon = dr.GetString(1);
                            inf.AptoMed = dr.GetString(2);
                            inf.IdMedicina = dr.GetInt32(3);
                            lista.Add(inf);
                        }
                        con.Close();
                    }
                }
                return lista;


            }
        }

        public ActionResult PasarMedicina(int IdMedicina)//,string AptoLab, string AptoOdon)
        {
            string sede = Session["codSede"].ToString();
            string Modifica = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            //string AptoLa = AptoLab;
            //string AptoOdo = AptoOdon;

            //if (AptoLa == "NO" && AptoOdo=="NO")
            //{
            //    ViewBag.mensaje = "Para enviarlos a medicina deben de ser pacientes APTOS en Odontologia y Laboratorio";

            //}
            //if(AptoLa == "SI" && AptoOdo == "NO")
            //{
            //    ViewBag.mensaje = "Para enviarlos a medicina deben de ser pacientes APTOS en Odontologia y Laboratorio";
            //}
            //if (AptoLa == "NO" && AptoOdo == "SI")
            //{
            //    ViewBag.mensaje = "Para enviarlos a medicina deben de ser pacientes APTOS en Odontologia y Laboratorio";
            //}
            //if (AptoLa == "SI" && AptoOdo == "SI")
            //{
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ActualizarEstadoCSMedicina", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", IdMedicina);
                    cmd.Parameters.AddWithValue("@IdEstado", 5);
                    //cmd.Parameters.AddWithValue("@Modifica", Modifica);
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                }
            }
            return RedirectToAction("ListaPacienteResultadoApto");
            //}


            //return RedirectToAction("ListaPacienteResultadoApto");
        }

       
        //public JsonResult GetDetalle(int codigo=0)
        //{
        //    string Tbody = ""; 
        //    string codsede = Session["codSede"].ToString();
        //    if (codigo != 0)
        //    {
        //        var resultado = Usp_Informes_Pacientes_Aptos(codigo, codsede);
        //        foreach (var item in resultado)
        //        {
        //            Tbody += $"<tr><td>{item.AptoLab}</td>";
        //            Tbody += $"<td>{item.AptoOdon}</td>";
        //            Tbody += $"<td>{item.AptoMed}</td>";
        //            Tbody += "<td><button type='submit' class='btn btn-success' name='evento' value='1'>Enviar Medicina</button></td>";
        //        }
        //        return Json(Tbody, JsonRequestBehavior.AllowGet);
        //    }
        //    else {
        //        return Json(null, JsonRequestBehavior.AllowGet);
        //    }

        //}

    }
}