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
    public class AlertasController : Controller
    {
        // GET: Alertas
        public ActionResult Index()
        {
            string usuario = Session["UserID"].ToString();
            ViewBag.data = Usp_DataCorteCaja(usuario);
            var dataCount = CantidadAlertasNoLeidas(usuario).FirstOrDefault();
            ViewBag.noLeido = dataCount.cantidad;
            return View();
        }

        public void registroAlertas(string asunto, string mensaje, string usuarioRecibe, string usuarioManda)
        {
            UtilitarioController ut = new UtilitarioController();
            E_Master hora = ut.ListadoHoraServidor().FirstOrDefault();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cp = new SqlCommand("usp_insertaAlerta", con))
                {
                    cp.CommandType = CommandType.StoredProcedure;
                    cp.Parameters.AddWithValue("@asunto", asunto);
                    cp.Parameters.AddWithValue("@mensaje", mensaje);
                    cp.Parameters.AddWithValue("@usuarioRecibe", usuarioRecibe);
                    cp.Parameters.AddWithValue("@usuarioManda", usuarioManda);
                    cp.Parameters.AddWithValue("@fechaRegistro", hora.HoraServidor);
                    cp.Parameters.AddWithValue("@horaRegistro", hora.HoraServidor.TimeOfDay);
                    cp.ExecuteNonQuery();

                    con.Close();
                }
            }
        }

        public void LeerAlerta(string usuarioRecibe)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cp = new SqlCommand("usp_leerAlerta", con))
                {
                    cp.CommandType = CommandType.StoredProcedure;
                    cp.Parameters.AddWithValue("@usuario", usuarioRecibe);
                    cp.ExecuteNonQuery();

                    con.Close();
                }
            }
        }

        public List<Alertas> Usp_DataCorteCaja(string CodUsuario)
        {
            List<Alertas> Lista = new List<Alertas>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaAlerta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario", CodUsuario);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Alertas ser = new Alertas();
                            ser.id = dr["id"] is DBNull ? 0 : dr.GetInt32(0);
                            ser.asunto = dr["asunto"] is DBNull ? "" : dr.GetString(1);
                            ser.mensaje = dr["mensaje"] is DBNull ? "" : dr.GetString(2);
                            ser.usuarioRecibe = dr["usuarioRecibe"] is DBNull ? "" : dr.GetString(3);
                            ser.estado = dr["estado"] is DBNull ? true : dr.GetBoolean(4);
                            ser.usuarioManda = dr["usuarioManda"] is DBNull ? "" : dr.GetString(5);
                            ser.fechaRegistro = dr["fechaRegistro"] is DBNull ? DateTime.Now : dr.GetDateTime(6);
                            ser.horaRegistro = dr["horaRegistro"] is DBNull ? DateTime.Now.TimeOfDay : dr.GetTimeSpan(7);
                            ser.duracion = dr["duracion"] is DBNull ? "" : dr.GetString(8);
                            ser.nombreEnvia = dr["nombreEnvia"] is DBNull ? "" : dr.GetString(9);
                            Lista.Add(ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<Alertas> CantidadAlertasNoLeidas(string CodUsuario)
        {
            List<Alertas> Lista = new List<Alertas>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_aletaNoLeida", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario", CodUsuario);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Alertas ser = new Alertas();
                            ser.cantidad = dr["total"] is DBNull ? 0 : dr.GetInt32(0);
                            Lista.Add(ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        [HttpPost]
        public JsonResult LeerTodasAlertas(string CodUsuario)
        {
            LeerAlerta(CodUsuario);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }


    }
}