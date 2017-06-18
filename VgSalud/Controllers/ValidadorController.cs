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
    public class ValidadorController : Controller
    {

        public List<E_Validador> Buscar_Parametros_Cuenta(int? cuenta = null)
        {
            List<E_Validador> Lista = new List<E_Validador>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Buscar_Parametros_Cuenta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nroCuenta", cuenta);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Validador val = new E_Validador();

                            val.CodServ = dr["CodServ"] is DBNull ? null : dr["CodServ"].ToString();
                            val.Medico = dr["CodMed"] is DBNull ? null : dr["CodMed"].ToString();
                            val.Fecha = dr["Fecha"] is DBNull ? string.Empty : dr["Fecha"].ToString();
                            val.turno = dr["Turno"] is DBNull ? string.Empty : dr["Turno"].ToString();
                            val.Registrado = dr["Registrado"] is DBNull ? string.Empty : dr["Registrado"].ToString();
                            Lista.Add(val);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public string Usp_Paciente_por_Cuenta(int? cuenta = null)
        {
            string paciente = string.Empty;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Paciente_por_Cuenta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nroCuenta", cuenta);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            paciente = dr["paciente"].ToString();
                        }
                        con.Close();
                    }

                }
                return paciente;
            }
        }

        public List<E_Validador> BuscarCuenta(int? cuenta = null)
        {
            List<E_Validador> Lista = new List<E_Validador>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Buscar_Cuenta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nroCuenta", cuenta);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Validador val = new E_Validador();

                            val.Item = dr.GetInt32(0);
                            val.codtar = dr.GetString(1);
                            val.DescTar = dr.GetString(2);
                            val.cantidad = dr.GetInt32(3);
                            val.precioUni = dr.GetDecimal(4);
                            val.Total = dr.GetDecimal(5);
                            val.EstadoLiq = dr["Estado"] is DBNull ? string.Empty : dr["Estado"].ToString();
                            Lista.Add(val);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Validador> ListadoEspecialidadesGeneral()
        {
            List<E_Validador> Lista = new List<E_Validador>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_General_Validando", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Validador val = new E_Validador();

                            val.CodUsu = dr.GetString(0);
                            val.CodServ = dr.GetString(1);
                            val.servicio = dr.GetString(2);
                            val.CodSede = dr.GetString(3);
                            val.General = dr.GetBoolean(4);

                            Lista.Add(val);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult CuentasIntegradas()
        {

            string sede = Session["codSede"].ToString();
            string user = Session["UserID"].ToString();
            var Administratito = ListadoEspecialidadesGeneral().Find(x => x.General == true && x.CodSede == sede);
            if (Administratito != null)
            {
                ViewBag.listaServicio = new SelectList(ListadoEspecialidadesGeneral().Where(x => x.CodSede == sede), "CodServ", "servicio");
            }
            else
            {
                ViewBag.listaServicio = new SelectList(ListadoEspecialidadesGeneral().Where(x => x.CodSede == sede && x.CodUsu == user), "CodServ", "servicio");
            }

            MedicosController med = new MedicosController();
            return View();
        }

        [HttpPost]
        public ActionResult CuentasIntegradas(E_Validador val)
        {

            string sede = Session["codSede"].ToString();
            string user = Session["UserID"].ToString();


            var Administratito = ListadoEspecialidadesGeneral().Find(x => x.General == true && x.CodSede == sede);
            if (Administratito != null)
            {
                ViewBag.listaServicio = new SelectList(ListadoEspecialidadesGeneral().Where(x => x.CodSede == sede), "CodServ", "servicio");
            }
            else
            {
                ViewBag.listaServicio = new SelectList(ListadoEspecialidadesGeneral().Where(x => x.CodSede == sede && x.CodUsu == user), "CodServ", "servicio");
            }

            try
            {
                MedicosController med = new MedicosController();
                if (val.Evento == "1")
                {
                    if (val.check == "1")
                    {
                        ViewBag.check = "1";
                        if (val.Medico != null) { ViewBag.medico = val.Medico; }
                        if (val.CodServ != null) { ViewBag.servicio = val.CodServ; }
                        if (val.Fecha != null) { ViewBag.fecha = val.Fecha; }
                        if (val.turno != null) { ViewBag.turno = val.turno; }
                        var parameters = Buscar_Parametros_Cuenta(val.cuenta).FirstOrDefault();
                        ViewBag.Registrado = parameters.Registrado;
                    }

                    else
                    {
                        ViewBag.check = null;
                        ViewBag.medico = null;
                        ViewBag.servicio = null;
                        ViewBag.fecha = null;
                        ViewBag.turno = null;
                        if (val.cuenta != 0)
                        {

                            var parameters = Buscar_Parametros_Cuenta(val.cuenta).FirstOrDefault();
                            if (parameters.CodServ != null) { ViewBag.servicio = parameters.CodServ; }
                            if (parameters.Medico != null) { ViewBag.medico = parameters.Medico.Trim(); }


                            if (parameters.Fecha != "")
                            {
                                DateTime demo = DateTime.Parse(parameters.Fecha);
                                ViewBag.fecha = demo.ToShortDateString();
                            }
                            if (parameters.turno != null) { ViewBag.turno = parameters.turno; }
                            ViewBag.cuenta = BuscarCuenta(val.cuenta);



                            DateTime demo1 = new DateTime();
                            if (parameters != null)
                            {
                                ViewBag.Registrado = parameters.Registrado;
                                if (parameters.Fecha != "")
                                {
                                    demo1 = DateTime.Parse(parameters.Fecha);
                                    ViewBag.fecha = demo1.ToShortDateString();
                                }
                                var encontro = Filtrar_CuentaDetalle(parameters.CodServ, demo1.ToShortDateString(), parameters.Medico, parameters.turno).ToList();
                                if (encontro.Count() != 0)
                                {
                                    var items = 0; var cuenta = 0;
                                    foreach (var item in encontro)
                                    {
                                        cuenta = cuenta + 1;
                                        items += item.Item;
                                    }
                                    ViewBag.itemss = items;
                                    ViewBag.cuentass = cuenta;
                                }
                                else
                                {
                                    ViewBag.itemss = 0;
                                    ViewBag.cuentass = 0;
                                }
                            }

                        }
                    }

                    ViewBag.cuenta = BuscarCuenta(val.cuenta);
                    ViewBag.paciente = Usp_Paciente_por_Cuenta(val.cuenta);

                    return View(val);
                }
                else if (val.Evento == "2")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                    {

                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("Actualizar_Validacion", con))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CodCue", val.cuenta);
                            cmd.Parameters.AddWithValue("@Servicio", val.CodServ);

                            cmd.Parameters.AddWithValue("@fecha", val.Fecha);
                            cmd.Parameters.AddWithValue("@Medico", val.Medico);
                            cmd.Parameters.AddWithValue("@Turno", val.turno);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    if (val.check == "1")
                    {
                        ViewBag.check = "1";
                        if (val.Medico != null) { ViewBag.medico = val.Medico; }
                        if (val.CodServ != null) { ViewBag.servicio = val.CodServ; }
                        if (val.Fecha != null) { ViewBag.fecha = val.Fecha; }
                        if (val.turno != null) { ViewBag.turno = val.turno; }
                        var parameters = Buscar_Parametros_Cuenta(val.cuenta).FirstOrDefault();
                        ViewBag.Registrado = parameters.Registrado;

                    }
                    else
                    {
                        ViewBag.check = null;
                        ViewBag.medico = null;
                        ViewBag.servicio = null;
                        ViewBag.fecha = null;
                        ViewBag.turno = null;
                        if (val.cuenta != 0)
                        {
                            var parameters = Buscar_Parametros_Cuenta(val.cuenta).FirstOrDefault();
                            if (parameters.CodServ != null) { ViewBag.servicio = parameters.CodServ; }
                            if (parameters.Medico != null) { ViewBag.medico = parameters.Medico.Trim(); }


                            if (parameters.Fecha != "")
                            {
                                DateTime demo = DateTime.Parse(parameters.Fecha);
                                ViewBag.fecha = demo.ToShortDateString();
                            }
                            if (parameters.turno != null) { ViewBag.turno = parameters.turno; }
                            ViewBag.cuenta = BuscarCuenta(val.cuenta);


                            DateTime demo1 = new DateTime();
                            if (parameters != null)
                            {
                                ViewBag.Registrado = parameters.Registrado;
                                if (parameters.Fecha != "")
                                {
                                    demo1 = DateTime.Parse(parameters.Fecha);
                                    ViewBag.fecha = demo1.ToShortDateString();
                                }


                                var encontro = Filtrar_CuentaDetalle(parameters.CodServ, demo1.ToShortDateString(), parameters.Medico, parameters.turno).ToList();
                                if (encontro.Count() != 0)
                                {
                                    var items = 0; var cuenta = 0;
                                    foreach (var item in encontro)
                                    {
                                        cuenta = cuenta + 1;
                                        items += item.Item;
                                    }
                                    ViewBag.itemss = items;
                                    ViewBag.cuentass = cuenta;
                                }
                                else
                                {
                                    ViewBag.itemss = 0;
                                    ViewBag.cuentass = 0;
                                }
                            }
                        }

                    }
                    ViewBag.cuenta = BuscarCuenta(val.cuenta);
                    ViewBag.paciente = Usp_Paciente_por_Cuenta(val.cuenta);
                    return View(val);
                }

                return View(val);

            }
            catch (Exception)
            {
                ViewBag.mensaje = "Error Datos No Validos";
                return View(val);
            }

        }



        public List<E_Validador> Filtrar_CuentaDetalle(string servicio = null, string fecha = null, string medico = null, string turno = null)
        {
            List<E_Validador> Lista = new List<E_Validador>();
            string date = "";
            if (fecha != "") { DateTime d = DateTime.Parse(fecha); date = d.ToShortDateString(); }
            else
            {
                date = fecha;
            }
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_filtrar_CuentaDetalles", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Servicio", servicio);
                    cmd.Parameters.AddWithValue("@fecha", date);
                    cmd.Parameters.AddWithValue("@Medico", medico);
                    cmd.Parameters.AddWithValue("@Turno", turno);
                    using (IDataReader dr1 = cmd.ExecuteReader())
                    {
                        while (dr1.Read())
                        {
                            E_Validador val = new E_Validador();

                            val.cuenta = dr1.GetInt32(0);
                            val.Item = dr1.GetInt32(1);
                            Lista.Add(val);

                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public JsonResult Obteneritemcuenta(string servicio = null, string fecha = null, string medico = null, string turno = null)
        {
            var cuenta = 0; var items = 0;
            if (servicio != null && fecha != null && medico != "NULL" && turno != null)
            {
                var encontro = Filtrar_CuentaDetalle(servicio, fecha, medico, turno);
                if (encontro != null)
                {

                    return Json(encontro, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }


        }



        public JsonResult ObtenerMedico(string servicio = null)
        {
            if (servicio != null)
            {
                MedicosController med = new MedicosController();
                var medico = med.ListadoMedico().Where(x => x.CodServ == servicio).ToList();
                return Json(medico, JsonRequestBehavior.AllowGet);

            }
            return null;
        }




    }
}