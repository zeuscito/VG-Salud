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
    public class CentroCostoController : Controller
    {

        public List<E_Centro_Costo> ListaCentroCosto()
        {
            List<E_Centro_Costo> Lista = new List<E_Centro_Costo>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_General_Centro_Costos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Centro_Costo cc = new E_Centro_Costo();
                            cc.Idcc = dr.GetString(0);
                            cc.Descripcion = dr.GetString(1);
                            cc.Estado = dr.GetBoolean(2);
                            Lista.Add(cc);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public ActionResult RegistrarCentroCosto() {
            ViewBag.boton = "Registrar";
            ViewBag.lista = ListaCentroCosto() ;
            return View();
        }

       [HttpPost]
        public ActionResult RegistrarCentroCosto(E_Centro_Costo cc) {
            ViewBag.boton = "Registrar";
            if (cc.Evento == "1") {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();

                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_Centro_Costos", con))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Idcc", "");
                            cmd.Parameters.AddWithValue("@Descripcion", cc.Descripcion.ToUpper());
                            cmd.Parameters.AddWithValue("@Estado", "");
                            cmd.Parameters.AddWithValue("@Evento", 1);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e) {
                        ViewBag.mensaje = "3";
                    }
                }
                ViewBag.lista = ListaCentroCosto();
                ViewBag.boton = "Registrar";
                return View(cc.Descripcion = null);
            }
            else if (cc.Evento == "2")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_Centro_Costos", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Idcc",cc.Idcc);
                        cmd.Parameters.AddWithValue("@Descripcion", cc.Descripcion.ToUpper());
                        cmd.Parameters.AddWithValue("@Estado", "");
                        cmd.Parameters.AddWithValue("@Evento", 2);
                        cmd.ExecuteNonQuery();
                     
                    }
                }
                ViewBag.boton = "Registrar";
                ViewBag.lista = ListaCentroCosto();
              
                return View(cc.Descripcion = null);
            }
            else if (cc.Evento == "3") {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_Centro_Costos", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Idcc", cc.Idcc);
                        cmd.Parameters.AddWithValue("@Descripcion", "");
                        cmd.Parameters.AddWithValue("@Estado", "");
                        cmd.Parameters.AddWithValue("@Evento", 3);
                        cmd.ExecuteNonQuery();
                       
                    }
                }
                ViewBag.boton = "Registrar";
                ViewBag.lista = ListaCentroCosto();
                return View();
            }
            else if (cc.Evento == "4")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_Centro_Costos", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Idcc", cc.Idcc);
                        cmd.Parameters.AddWithValue("@Descripcion", "");
                        cmd.Parameters.AddWithValue("@Estado", "");
                        cmd.Parameters.AddWithValue("@Evento", 4);
                        cmd.ExecuteNonQuery();
                       
                    }
                }
                ViewBag.boton = "Registrar";
                ViewBag.lista = ListaCentroCosto();
                return View();
            }
            else if ( cc.Evento == "5" ) {
                ViewBag.boton = "Modificar";
                ViewBag.lista = ListaCentroCosto();
                var editar = ListaCentroCosto().Where(x => x.Idcc == cc.Idcc).FirstOrDefault();
               
                return View(editar);
            }
      
            return View();
        }


    }
}