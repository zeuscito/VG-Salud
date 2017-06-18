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
    public class MedicoTipoTarifaController : Controller
    {

        public ActionResult ListarMedicoTipoTarifas() {

            return View(listamedicotarifas()); 
        }

        public List<E_Medico_TipoTarifa> listamedicotarifas() {

            List <E_Medico_TipoTarifa> Lista = new List<E_Medico_TipoTarifa>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_lista_Medico_TipoTarifa", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Medico_TipoTarifa Etip = new E_Medico_TipoTarifa();


                           
                            Etip.nomTip = dr.GetString(0); 
                            Etip.NomMed = dr.GetString(1);
                            Etip.porcentaje = dr.GetDecimal(2);
                            Etip.CodTipTar = dr.GetString(3).ToUpper();
                            Etip.CodMed = dr.GetString(4).ToUpper();
                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
              
            }
            return Lista; 

    }



        public ActionResult RegistrarMedicoTipoTarifa() {
            MedicosController med = new MedicosController();
            TipoTarifaController tt = new TipoTarifaController();
            ViewBag.medico = new SelectList(med.ListadoMedico().Where(x => x.EstMed==true), "CodMed", "Nommed");
            ViewBag.tipotarifa =new SelectList( tt.ListadoTipoTarifa().Where(x => x.EstTipTar == true), "CodTipTar", "DescTipTar"); 

            return View();
        }
        



        public ActionResult ModificarMedicoTipoTarifa(string id, string id1) {
            MedicosController med = new MedicosController();
            TipoTarifaController tt = new TipoTarifaController();
            ViewBag.medico = new SelectList(med.ListadoMedico(), "CodMed", "Nommed");
            ViewBag.tipotarifa = new SelectList(tt.ListadoTipoTarifa(), "CodTipTar","DescTipTar");
            var lista = (from x in listamedicotarifas() where x.CodTipTar == id && x.CodMed == id1 select x).FirstOrDefault();
            return View(lista); 

        }

        public ActionResult Eliminar(string id , string id1) {

            string elimina = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Mantenimiento_Medico_tipotarifa", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodTipTar",id);
                        cmd.Parameters.AddWithValue("@CodMed", id1);
                        cmd.Parameters.AddWithValue("@porcentaje",0);
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", "");
                        cmd.Parameters.AddWithValue("@Elimina", elimina);
                        cmd.Parameters.AddWithValue("@Evento", 3);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se actualizo Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                        return View(id);
                    }
                    finally
                    {
                        con.Close();

                    }
                }


            }


          return  RedirectToAction("ListarMedicoTipoTarifas"); 

        }






        [HttpPost]
        public ActionResult ModificarMedicoTipoTarifa(E_Medico_TipoTarifa mt)
        {

            string modifica = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Mantenimiento_Medico_tipotarifa", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodTipTar", mt.CodTipTar);
                        cmd.Parameters.AddWithValue("@CodMed", mt.CodMed);
                        cmd.Parameters.AddWithValue("@porcentaje", mt.porcentaje);
                        cmd.Parameters.AddWithValue("@Crea","");
                        cmd.Parameters.AddWithValue("@Modifica", modifica);
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Evento", 2);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se actualizo Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                        return View(mt);
                    }
                    finally
                    {
                        con.Close();

                    }
                }


            }


            return RedirectToAction("ListarMedicoTipoTarifas");

        }

        [HttpPost]
        public ActionResult RegistrarMedicoTipoTarifa(E_Medico_TipoTarifa mt) {

            string Crea = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;
            MedicosController med = new MedicosController();
            TipoTarifaController tt = new TipoTarifaController();
            ViewBag.medico = new SelectList(med.ListadoMedico().Where(x => x.EstMed == true), "CodMed", "Nommed");
            ViewBag.tipotarifa = new SelectList(tt.ListadoTipoTarifa().Where(x => x.EstTipTar == true), "CodTipTar", "DescTipTar");
            try {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_Mantenimiento_Medico_tipotarifa", con))
                    {
                        try
                        {

                            cmd.Parameters.AddWithValue("@CodTipTar", mt.CodTipTar);
                            cmd.Parameters.AddWithValue("@CodMed", mt.CodMed);
                            cmd.Parameters.AddWithValue("@porcentaje", mt.porcentaje);
                            cmd.Parameters.AddWithValue("@Crea", Crea);
                            cmd.Parameters.AddWithValue("@Modifica", "");
                            cmd.Parameters.AddWithValue("@Elimina", "");
                            cmd.Parameters.AddWithValue("@Evento", 1);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            ViewBag.Mensaje = "Se registro Satisfactoriamente";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Mensaje = "Error Datos [NO VALIDOS]";
                            return View(mt);
                        }
                        finally
                        {
                            con.Close();

                        }
                    }


                }
            } catch (Exception) {

                ViewBag.mensaje = "Error Datos [NO VALIDOS]";
                return View(mt);

            }
        

            return RedirectToAction("ListarMedicoTipoTarifas");

        }



    }
}