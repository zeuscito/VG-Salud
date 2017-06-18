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
    public class CategoriaPacienteController : Controller
    {

        public List<E_Categoria_Paciente> listadoCategoriaCliente()
        {
            List<E_Categoria_Paciente> Lista = new List<E_Categoria_Paciente>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Categoria_Paciente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Categoria_Paciente Ser = new E_Categoria_Paciente();
                            Ser.CodCatPac = dr.GetString(0);
                            Ser.DescCatPac = dr.GetString(1);
                            Ser.EstCatPac = dr.GetBoolean(2);

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult listaCategoriaCliente()
        {
            return View(listadoCategoriaCliente());
        }


        public ActionResult RegistrarCategoriaCliente()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarCategoriaCliente(E_Categoria_Paciente cat)
        {
            
            TarifarioController ta = new TarifarioController();

            string Crea = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                using (SqlCommand cmd = new SqlCommand("usp_MtoCategoria_Paciente", con, tr))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodCatPac", "");
                        cmd.Parameters.AddWithValue("@DescCatPac", cat.DescCatPac.ToUpper());
                        cmd.Parameters.AddWithValue("@EstCatPac", cat.EstCatPac);
                        cmd.Parameters.AddWithValue("@Crea", Crea);
                        cmd.Parameters.AddWithValue("@Modifica", "");
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Evento", "1");
                         
                        string Resu = (string)cmd.ExecuteScalar();
                        string Codigo = Convert.ToString(Resu);
                        
                            if (Resu.Length == 5)
                            {
                                foreach (E_Tarifario it in (List<E_Tarifario>)ta.ListadoTarifa().ToList())
                                {
                                SqlCommand da = new SqlCommand("usp_MtoTarifa_CategoriaPaciente", con, tr);
                                da.CommandType = CommandType.StoredProcedure;

                                da.Parameters.AddWithValue("@CodTarCate", "");
                                da.Parameters.AddWithValue("@CodCatPac", Codigo);
                                da.Parameters.AddWithValue("@CodTar", it.CodTar);
                                da.Parameters.AddWithValue("@Precio", 0);
                                da.Parameters.AddWithValue("@Crea", "");
                                da.Parameters.AddWithValue("@Modifica", Crea);
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", "1");
                                da.ExecuteNonQuery();

                                }
                                tr.Commit();
                                ViewBag.mensaje = "Pedido registrado";
                            }                            
                        else
                        {
                            ViewBag.Mensaje = "Ocurrio algun error al registrar";
                        }


                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "3";
                        return View(cat);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("listaCategoriaCliente");
            }

        }



        public ActionResult ModificarCategoriaCliente(string id) {
            
            var lista = (from x in listadoCategoriaCliente() where x.CodCatPac == id select x).FirstOrDefault();
            return View(lista);

        }

        [HttpPost]
        public ActionResult ModificarCategoriaCliente(E_Categoria_Paciente cat)
        {
            string modificar = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_MtoCategoria_Paciente", con))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodCatPac", cat.CodCatPac);
                        cmd.Parameters.AddWithValue("@DescCatPac", cat.DescCatPac.ToUpper());
                        cmd.Parameters.AddWithValue("@EstCatPac", cat.EstCatPac);
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", modificar);
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Evento", "2");
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se registro Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                        return View(cat);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("listaCategoriaCliente");
            }

        }


        public ActionResult Eliminar(string id)
        {

            string eliminar = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("usp_MtoCategoria_Paciente", con))
                {
                    try
                    {

                        da.CommandType = CommandType.StoredProcedure;

                        da.Parameters.AddWithValue("@CodCatPac", id);
                        da.Parameters.AddWithValue("@DescCatPac", "");
                        da.Parameters.AddWithValue("@EstCatPac", "");
                        da.Parameters.AddWithValue("@Crea", "");
                        da.Parameters.AddWithValue("@Modifica", "");
                        da.Parameters.AddWithValue("@Elimina", eliminar);
                        da.Parameters.AddWithValue("@Evento", "3");

                        da.ExecuteNonQuery();
                        ViewBag.mensaje = "Se elimino correctamente";
                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "Error no se pudo eliminar";
                    }
                    finally { con.Close(); }
                }
                return RedirectToAction("listaCategoriaCliente");

            }
        }


        public ActionResult Activar(string id)
        {

            string modifica = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("usp_MtoCategoria_Paciente", con))
                {
                    try
                    {

                        da.CommandType = CommandType.StoredProcedure;

                        da.Parameters.AddWithValue("@CodCatPac", id);
                        da.Parameters.AddWithValue("@DescCatPac", "");
                        da.Parameters.AddWithValue("@EstCatPac", "");
                        da.Parameters.AddWithValue("@Crea", "");
                        da.Parameters.AddWithValue("@Modifica", modifica);
                        da.Parameters.AddWithValue("@Elimina", "");
                        da.Parameters.AddWithValue("@Evento", "4");

                        da.ExecuteNonQuery();
                        ViewBag.mensaje = "Se elimino correctamente";
                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "Error no se pudo eliminar";
                    }
                    finally { con.Close(); }
                }
                return RedirectToAction("listaCategoriaCliente");

            }
        }




    }
}