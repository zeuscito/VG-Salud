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
    public class SedesController : Controller
    {
        // GET: Sedes

        public ActionResult RegistrarSedes()
        {
            CentroCostoController cc = new CentroCostoController();
            ViewBag.cc = new SelectList(cc.ListaCentroCosto().Where(x => x.Estado == true), "Idcc", "Descripcion");
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarSedes(E_Sede Ese)
        {
            string Crea = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;
            CentroCostoController cc = new CentroCostoController();
            ViewBag.cc = new SelectList(cc.ListaCentroCosto().Where(x => x.Estado == true), "Idcc", "Descripcion");
            UtilitarioController util = new UtilitarioController();
            DatosGeneralesController dat = new DatosGeneralesController();
            var DG = dat.Getdatogenerales();
            int SedesCountDG =  DG.sedes == null ? 0 : Convert.ToInt32(DG.sedes);
            int SedesCount = ListadoSedes().Count();
            if (SedesCountDG > SedesCount)
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Usp_MtoSedes", con))
                    {
                        try
                        {
                            cmd.Parameters.AddWithValue("@CodSede", "");
                            cmd.Parameters.AddWithValue("@NomSede", Ese.NomSede.ToUpper());
                            cmd.Parameters.AddWithValue("@DireccSede", Ese.DireccSede.ToUpper());
                            cmd.Parameters.AddWithValue("@TelfSede", Ese.TelfSede);
                            cmd.Parameters.AddWithValue("@EstSede", "");
                            cmd.Parameters.AddWithValue("@Crea", Crea);
                            cmd.Parameters.AddWithValue("@Modifica", "");
                            cmd.Parameters.AddWithValue("@Elimina", "");
                            cmd.Parameters.AddWithValue("@Idcc", Ese.Idcc);
                            cmd.Parameters.AddWithValue("@Evento", 1);

                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            ViewBag.mensaje = "Se registro Satisfactoriamente";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.mensaje = "Error Datos [NO VALIDOS]";
                            return View(Ese);
                        }
                        finally
                        {
                            con.Close();

                        }
                    }
                    return RedirectToAction("ListaSedes");
                }
            }
            else {
                ViewBag.mensaje = "Error Excedio el Limite Permitido de Registros!!!...Consulte con su Administrador";
                return View(Ese); 

            }
    
        }


        public ActionResult ModificarSedes(string Id)
        {
            var lista = (from x in ListadoSedes() where x.CodSede == Id select x).FirstOrDefault();
            CentroCostoController cc = new CentroCostoController();
            ViewBag.cc = new SelectList(cc.ListaCentroCosto().Where(x=>x.Estado == true), "Idcc", "Descripcion",lista.Idcc);

            return View(lista);
        }

        [HttpPost]
        public ActionResult ModificarSedes(E_Sede Ese)
        {
            string modifica = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoSedes", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodSede", Ese.CodSede);
                        cmd.Parameters.AddWithValue("@NomSede", Ese.NomSede.ToUpper());
                        cmd.Parameters.AddWithValue("@DireccSede", Ese.DireccSede.ToUpper());
                        cmd.Parameters.AddWithValue("@TelfSede", Ese.TelfSede);
                        cmd.Parameters.AddWithValue("@EstSede", Ese.EstSede);
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", modifica);
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Idcc",Ese.Idcc);
                        cmd.Parameters.AddWithValue("@Evento", 2);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se Modifico Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al modificar : " + ex.Message.ToString();
                        return View(Ese);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListaSedes");
            }
        }


        public ActionResult ListaSedes()
        {
                return View(ListadoSedes());
        }

        public List<E_Sede> ListadoSedes()
        {
            List<E_Sede> Lista = new List<E_Sede>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Sedes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Sede Esp = new E_Sede();

                            Esp.CodSede = dr.GetString(0);
                            Esp.NomSede = dr["NomSede"] is DBNull ? string.Empty : dr["NomSede"].ToString().ToUpper();
                            Esp.DireccSede = dr["DireccSede"] is DBNull ? string.Empty : dr["DireccSede"].ToString().ToUpper();
                            Esp.TelfSede = dr.GetString(3);
                            Esp.EstSede = dr.GetBoolean(4);
                            Esp.Idcc = dr["Idcc"] is DBNull ? string.Empty :  dr["Idcc"].ToString() ;
                            Esp.DescripcionCC = dr["Descripcion"] is DBNull ? string.Empty : dr["Descripcion"].ToString();
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