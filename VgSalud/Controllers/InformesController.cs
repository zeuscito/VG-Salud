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
        

        public ActionResult ListaPacienteResultadoApto(string nombre = null, string dni = null)
        {
            ViewBag.nombre = nombre;
            ViewBag.dni = dni;



            return View(ListaFiltroPaciente(nombre, dni));
        }
        public List<E_Informes> ListaFiltroPaciente(string nombre, string dni)
        {/*var usuario = Session["usuario"].ToString();*/
            List<E_Informes> lista = new List<E_Informes>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Busca_PacienteApto_MedicinaS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (nombre == null || nombre == "")
                    {
                        cmd.Parameters.AddWithValue("@nombre", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                    }
                    if (dni == null || dni == "")
                    {
                        cmd.Parameters.AddWithValue("@dni", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@dni", dni);
                    }
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
                            inf.FechaAtenOdo = dr.GetDateTime(8);
                            inf.ObservacionOdon = dr.GetString(9);
                            inf.ReevaluadoOndon = dr.GetString(10);
                            inf.AptoLab = dr.GetString(11);
                            inf.AptoOdon = dr.GetString(12);
                            inf.AptoMed = dr.GetString(13);
                            inf.IdMedicina = dr.GetInt32(14);
                            inf.Historia = dr.GetInt32(15);
                            inf.nroCarnet = dr.GetInt32(16);
                            inf.FechaAtenMed = dr.GetDateTime(17);
                            inf.Observaciones = dr.GetString(18);
                            inf.ReevaluadoMed = dr.GetString(19);
                            lista.Add(inf);
                        }
                        con.Close();
                    }
                }
                return lista;


            }
        }


        public List<E_Informes> Usp_Informes_Pacientes_Aptos(int historia, string codsede)
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
     
        public ActionResult PasarMedicina(int id)//,string AptoLab, string AptoOdon)
        {
            string sede = Session["codSede"].ToString();
            string Modifica = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

         
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Usp_ActualizarEstadoCSMedicina", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdMedicina", id);
                        //cmd.Parameters.AddWithValue("@IdEstado", 5);
                        cmd.Parameters.AddWithValue("@CodSede", sede);
                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("ListaPacienteResultadoApto");
            }
           catch(Exception e)
            {
                throw;
            }
                

        }
      



        public List<E_Informes> ListaObservReevalGeneral(int id)
        {/*var usuario = Session["usuario"].ToString();*/
            string sede = Session["codSede"].ToString();
            List<E_Informes> lista = new List<E_Informes>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Detalle_Informes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@sede", sede);
                    SqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                    E_Informes inf = new E_Informes();
                                    inf.ObservacionLab = dr.GetString(0);
                                    inf.ReevaluadoLab = dr.GetString(1);
                                    inf.ObservacionOdon = dr.GetString(2);
                                    inf.ReevaluadoOndon = dr.GetString(3);
                                    inf.Observaciones = dr.GetString(4);
                                    inf.ReevaluadoMed = dr.GetString(5);
                                    lista.Add(inf);
                            }
                    con.Close();
                    }
                   }


            return lista;
        }
        

        public JsonResult ObtenerDetalle(int id = 0)
        {
            E_Informes i = new E_Informes();

            var datoprueba = i.nroCarnet;
            string Tbody = "";
            if (id != 0)
            {
                var IdCarnet = i.IdNroCarnet;

                var resultado = ListaObservReevalGeneral(id);
                foreach (var item in resultado)
                {
                    Tbody += $"<tr><td>{item.ObservacionLab}</td>";
                    Tbody += $"<td>{item.ReevaluadoLab}</td>";
                    Tbody += $"<td></td>";
                    Tbody += $"<td>{item.ObservacionOdon}</td>";
                    Tbody += $"<td>{item.ReevaluadoOndon}</td>";
                    Tbody += $"<td></td>";
                    Tbody += $"<td>{item.Observaciones}</td>";
                    Tbody += $"<td>{item.ReevaluadoMed}</td></tr>";
                }
                return Json(Tbody, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
          
        }




        public ActionResult GenerarQR()
        {


            return View();
        }


        
    }
}