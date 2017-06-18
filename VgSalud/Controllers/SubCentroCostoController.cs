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
    public class SubCentroCostoController : Controller
    {
        CentroCostoController cc = new CentroCostoController();
        public List<E_Sub_Centro_Costo> ListaSubCentroCosto()
        {
            List<E_Sub_Centro_Costo> Lista = new List<E_Sub_Centro_Costo>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_General_Sub_Centro_Costos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Sub_Centro_Costo scc = new E_Sub_Centro_Costo();
                            scc.IdScc = dr.GetString(0);
                            var CentroC = cc.ListaCentroCosto().Where(x => x.Idcc == dr.GetString(2)).FirstOrDefault();
                            scc.Idcc = dr.GetString(2) + "-" + CentroC.Descripcion ;
                            scc.Descripcion = dr.GetString(1);
                            scc.Estado = dr.GetBoolean(3);
                            Lista.Add(scc);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public ActionResult RegistrarSubCentroCosto()
        {
            ViewBag.boton = "Registrar";
            ViewBag.centrocosto = new SelectList(cc.ListaCentroCosto().Where(x => x.Estado == true), "Idcc", "Descripcion");
            ViewBag.lista = ListaSubCentroCosto();
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarSubCentroCosto(E_Sub_Centro_Costo scc)
        {
            CentroCostoController cc = new CentroCostoController();
            ViewBag.centrocosto = new SelectList(cc.ListaCentroCosto().Where(x => x.Estado == true), "Idcc", "Descripcion");
            ViewBag.lista = ListaSubCentroCosto();
            ViewBag.boton = "Registrar";
            try {
                if (scc.Evento == "1")
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_Sub_Centro_Costos", con))
                            {

                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@IdScc", "");
                                cmd.Parameters.AddWithValue("@Descripcion", scc.Descripcion.ToUpper());
                                cmd.Parameters.AddWithValue("@Idcc", scc.Idcc);
                                cmd.Parameters.AddWithValue("@Estado", "");
                                cmd.Parameters.AddWithValue("@Evento", 1);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        scc.Descripcion = null;
                        ViewBag.lista = ListaSubCentroCosto();
                        ViewBag.boton = "Registrar";
                        return View();
                    }
                    catch (Exception) {
                        ViewBag.mensaje = "3";
                        scc.Descripcion = null;
                        ViewBag.lista = ListaSubCentroCosto();
                        ViewBag.boton = "Registrar";
                        return View();
                    }
            
                }
                else if (scc.Evento == "2")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_Sub_Centro_Costos", con))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@IdScc", scc.IdScc);
                            cmd.Parameters.AddWithValue("@Descripcion", scc.Descripcion.ToUpper());
                            cmd.Parameters.AddWithValue("@Idcc", scc.Idcc);
                            cmd.Parameters.AddWithValue("@Estado", "");
                            cmd.Parameters.AddWithValue("@Evento", 2);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    ViewBag.lista = ListaSubCentroCosto();
                    ViewBag.boton = "Registrar";
                    scc.Descripcion = null;
                    return View();
                }
                else if (scc.Evento == "3")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_Sub_Centro_Costos", con))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@IdScc", scc.IdScc);
                            cmd.Parameters.AddWithValue("@Descripcion", "");
                            cmd.Parameters.AddWithValue("@Idcc", "");
                            cmd.Parameters.AddWithValue("@Estado", "");
                            cmd.Parameters.AddWithValue("@Evento", 3);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    ViewBag.lista = ListaSubCentroCosto();
                    ViewBag.boton = "Registrar";
                    return View();
                }
                else if (scc.Evento == "4")
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_Sub_Centro_Costos", con))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@IdScc", scc.IdScc);
                            cmd.Parameters.AddWithValue("@Descripcion", "");
                            cmd.Parameters.AddWithValue("@Idcc", "");
                            cmd.Parameters.AddWithValue("@Estado", "");
                            cmd.Parameters.AddWithValue("@Evento", 4);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    ViewBag.lista = ListaSubCentroCosto();
                    ViewBag.boton = "Registrar";
                    return View();
                }
                else if (scc.Evento == "5")
                {
                    ViewBag.lista = ListaSubCentroCosto();
                    ViewBag.boton = "Modificar";
                    var editar = ListaSubCentroCosto().Where(x => x.IdScc == scc.IdScc).FirstOrDefault();
                    string value = scc.Idcc;
                    string[] split = value.Split('-');
                    scc.Idcc = split[0];
                    editar.Idcc = scc.Idcc;
                    ViewBag.select = editar.Idcc;
                    ViewBag.centrocosto = new SelectList(cc.ListaCentroCosto().Where(x => x.Estado == true).ToList(), "Idcc", "Descripcion", editar.Idcc);
                    return View(editar);
                }

            }
            catch (Exception ex) {
                ViewBag.mensaje = "Error Datos no Validos" + ex;
                return View(scc);
            }
       
            return View();
        }
    }
    }