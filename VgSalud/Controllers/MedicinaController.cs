using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VgSalud.Models;

namespace VgSalud.Controllers
{
    public class MedicinaController : Controller
    {
        SqlCommand cmd;
        SqlConnection db;
        string cadena = ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString;

        public ActionResult ListarCarnet_MedicinaDelDia_EnEspera() {
            ViewBag.ListaEspera = ListadoCarnetMedicina_DelDia_EnEspera();
            ViewBag.ListaPostergados = Usp_Medicina_Postergados();
            return View();
        }

        [HttpPost]
        public ActionResult ListarCarnet_MedicinaDelDia_EnEspera(E_CSLaboratorio lab)
        {
            ViewBag.ListaEspera = ListadoCarnetMedicina_DelDia_EnEspera();
            ViewBag.ListaPostergados = Usp_Medicina_Postergados();
            try
            {
                if (lab.evento == "1")
                {
                    Usp_Mantenimiento(lab.Id, 1);
                }
                else if (lab.evento == "2")
                {
                    Usp_Mantenimiento(lab.Id, 2);
                }
                else if (lab.evento == "3")
                {
                    Usp_Mantenimiento(lab.Id, 3);
                }
                else if (lab.evento == "4")
                {
                    Usp_Mantenimiento(lab.Id, 4);
                }
                else if (lab.evento == "5")
                {
                    Usp_Mantenimiento(lab.Id, 5);
                }
                return RedirectToAction("ListarCarnet_MedicinaDelDia_EnEspera");

            }
            catch (Exception)
            {
                ViewBag.mensaje = "Error: No se pudo procesar la Actualizacion Correctamente";
                return RedirectToAction("ListarCarnet_MedicinaDelDia_EnEspera");
            }


        }
        public bool Usp_Mantenimiento(int id, int evento)
        {
            try
            {
                using (db = new SqlConnection(cadena))
                {
                    using (cmd = new SqlCommand("Usp_Validaciones_CsMedicina", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@Evento", evento);
                        db.Open();
                        int ide = cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<E_CSMedicina> Usp_Medicina_Postergados()
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSMedicina> ListaCarnetLab_E = new List<E_CSMedicina>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_Medicina_Postergados", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_CSMedicina med = new E_CSMedicina();

                        med.Id = int.Parse(dr["Id"].ToString());
                        med.CodCue = int.Parse(dr["CodCue"].ToString());
                        med.Paciente = dr["NombrePaciente"].ToString();
                        med.DesTipoCarnet = dr["DescCarnet"].ToString();
                        med.Manipulador = dr["Manipulador"].ToString();
                        med.Prioridad = Convert.ToInt32(dr["Prioridad"].ToString());
                        med.idEstado = Convert.ToInt32(dr["IdEstado"].ToString());
                        ListaCarnetLab_E.Add(med);

                    }


                }
            }

            return ListaCarnetLab_E;
        }

        public List<E_CSMedicina> ListadoCarnetMedicina_DelDia_EnEspera()
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSMedicina> ListaCarnetOdo = new List<E_CSMedicina>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("usp_ListadoCarnetMedicina_EnEspera", db))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_CSMedicina med = new E_CSMedicina();

                        med.Id = int.Parse(dr["Id"].ToString());
                        med.CodCue = int.Parse(dr["CodCue"].ToString());
                        med.Paciente = dr["NombrePaciente"].ToString();
                        med.DesTipoCarnet = dr["DescCarnet"].ToString();
                        med.Manipulador = dr["Manipulador"].ToString();
                        med.Prioridad = Convert.ToInt32(dr["Prioridad"].ToString());
                        med.idEstado = Convert.ToInt32(dr["IdEstado"].ToString());
                        med.Edad = int.Parse(dr["Edad"].ToString());
                        ListaCarnetOdo.Add(med);

                    }


                }
            }

            return ListaCarnetOdo;
        }


        public List<E_CSMedicina> MostrarDatosDeListaDeCarnetSanidadCSMedicina(int Id)
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSMedicina> ListaCarnetMed = new List<E_CSMedicina>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_MostrarDatosDeListaDeCarnetSanidadCSMedicina", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Sede", sede);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_CSMedicina med = new E_CSMedicina();

                        med.Id = int.Parse(dr["Id"].ToString());
                        med.CodCue = int.Parse(dr["CodCue"].ToString());
                        med.Paciente = dr["NombrePaciente"].ToString();
                        med.DesTipoCarnet = dr["DescCarnet"].ToString();
                        med.Manipulador = dr["Manipulador"].ToString();
                        med.Prioridad = Convert.ToInt32(dr["Prioridad"].ToString());
                        med.idEstado = Convert.ToInt32(dr["IdEstado"].ToString());
                        med.Edad = Convert.ToInt32(dr["edad"]);
                        med.NumeroCarnet = dr["NumeroCarnet"].ToString();
                        med.Procedencia = dr["Procedencia"].ToString();
                        med.NroCarnet = Convert.ToInt32(dr["NroCarnet"]);

                        med.AptoOdon = dr["AptoOdo"].ToString();
                        med.AptoLab = dr["AptoLab"].ToString();

                        med.MuestraSangre = Convert.ToBoolean(dr["LabMuestraSangre"].ToString());
                        if (med.MuestraSangre==true)
                        {
                            med.Lab_MuestraSangre = "SI";
                            med.TituloMuestraSangre = "MUESTRA SANGRE: ";
                        }
                        else
                        {
                            med.Lab_MuestraSangre = "NO";
                            med.TituloMuestraSangre = "MUESTRA SANGRE: ";
                        }

                        med.MuestraHeces = Convert.ToBoolean(dr["LabMuestraHeces"].ToString());
                        if (med.MuestraHeces == true)
                        {
                            med.Lab_MuestraHeces = "SI";
                            med.TituloMuestraHeces= "MUESTRA HECES: ";
                        }
                        else
                        {
                            med.Lab_MuestraHeces = "NO";
                            med.TituloMuestraHeces = "MUESTRA HECES: ";
                        }

                        if(med.Lab_RPR !=null || med.Lab_RPR != "")
                        {
                            med.TituloRPR = "RPR: ";
                            med.Lab_RPR = dr["LabRPR"].ToString();
                        }
                        else
                        {
                            med.TituloRPR = "RPR: ";
                        }

                        if (med.Lab_Parasitologico != null || med.Lab_Parasitologico != "")
                        {
                            med.TituloParasitologico = "PARASITOLOGICO: ";
                            med.Lab_Parasitologico = dr["LabParasitologico"].ToString();
                        }
                        else
                        {
                            med.TituloParasitologico = "PARASITOLOGICO: ";
                        }


                        if (med.Lab_Observacion != null || med.Lab_Observacion != "")
                        {
                            med.TituloObservacion = "OBSERVACIÓN: ";
                            med.Lab_Observacion = dr["LabObservacion"].ToString();
                        }
                        else
                        {
                            med.TituloObservacion = "OBSERVACIÓN: ";
                        }

                        if (med.OdoObservacion != null || med.OdoObservacion != "")
                        {
                            med.TituloObservacion = "OBSERVACIÓN: ";
                            med.OdoObservacion = dr["OdoObservacion"].ToString();
                        }
                        else
                        {
                            med.TituloObservacion = "OBSERVACIÓN: ";
                        }


                        //med.IdOdontologia = Convert.ToInt32(dr["IdOdontologia"].ToString());
                        

                        ListaCarnetMed.Add(med);

                    }


                }
            }

            return ListaCarnetMed;
        }

        public ActionResult ActualizarDatosAtencionMedicina(int id,E_CSMedicina Med )
        {
            
            var MostrarDatosMedicina = MostrarDatosDeListaDeCarnetSanidadCSMedicina(id).FirstOrDefault();
            

            ViewBag.ListaFechaOdontograma = ListadoDeFechaDeOdontogramasRegistrados(id);
            // por terminar
            

            return View(MostrarDatosMedicina);
        }


        [HttpPost]
        public ActionResult ActualizarDatosAtencionMedicina(int Id, string procedencia, E_CSMedicina med)
        {
            string sede = Session["CodSede"].ToString();
            UtilitarioController ut = new UtilitarioController();
            var horasis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string modifica = Session["usuario"] + " " + horasis.HoraServidor.ToString() + " " + Environment.MachineName;
            med.Id = Id;
            
            med.Procedencia = procedencia;
            try
            {
                using (db = new SqlConnection(cadena))
                {
                    if (med.Procedencia == "1".ToString())
                    {
                        using (cmd = new SqlCommand("Usp_ActualizarDatosCSMedicinaCS", db))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Id",med.Id);
                            if (med.Conclusion!=null)
                            {
                                cmd.Parameters.AddWithValue("@Conclusion",med.Conclusion);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Conclusion", "");
                            }
                            if (med.AptoMed != null)
                            {
                                cmd.Parameters.AddWithValue("@Apto", med.AptoMed);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Apto", "");
                            }
                            if (med.Reevaluado != null)
                            {
                                cmd.Parameters.AddWithValue("@Reevaluado", med.Reevaluado);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Reevaluado", "NO");
                            }
                            if (med.Observaciones != null)
                            {
                                cmd.Parameters.AddWithValue("@Observaciones", med.Observaciones);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Observaciones", "");
                            }
                            cmd.Parameters.AddWithValue("@Modifica", modifica);
                            cmd.Parameters.AddWithValue("@Sede", sede);
                            db.Open();
                            SqlDataReader dr = cmd.ExecuteReader();
                            return RedirectToAction("ListarCarnet_MedicinaDelDia_EnEspera");

                        }
                    }
                    else
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", med.Id);
                        //cmd.Parameters.AddWithValue("@Manipulador", med.Manipulador);
                        if (med.Conclusion != null)
                        {
                            cmd.Parameters.AddWithValue("@Conclusion", med.Conclusion);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Conclusion", "");
                        }
                        if (med.AptoMed != null)
                        {
                            cmd.Parameters.AddWithValue("@Apto", med.AptoMed);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Apto", "");
                        }
                        if (med.Reevaluado != null)
                        {
                            cmd.Parameters.AddWithValue("@Reevaluado", med.Reevaluado);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Reevaluado", "");
                        }
                        if (med.Observaciones != null)
                        {
                            cmd.Parameters.AddWithValue("@Observaciones", med.Observaciones);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Observaciones", "");
                        }
                        cmd.Parameters.AddWithValue("@Modifica", modifica);
                        cmd.Parameters.AddWithValue("@Sede", sede);
                        db.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        return RedirectToAction("ListarCarnet_MedicinaDelDia_EnEspera");
                    }

                }
            } 
            catch (Exception ex)
            {
                return RedirectToAction("ActualizarDatosAtencionLaboratorio");
            }
        }





        public List<E_CSMedicina> ListadoDeFechaDeOdontogramasRegistrados(int id)
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSMedicina> ListaFechaOdo = new List<E_CSMedicina>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_ListarFechaRegistroOdontograma", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdOdontograma", id);
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_CSMedicina med = new E_CSMedicina();

                        med.CodOdontograma = Convert.ToInt32(dr["CodOdontograma"].ToString());
                        med.FechaRegistroOdontograma = Convert.ToDateTime(dr["FechaRegistro"].ToString());

                        ListaFechaOdo.Add(med);
                        
                    }


                }
            }

            return ListaFechaOdo;
        }




        public JsonResult VerHistorialOdontogramaPorFecha(int CodOdo)
        {
            E_CSMedicina Med = new E_CSMedicina();

            
            //var datoprueba = i.nroCarnet;

            string Tbody = "";
            if (CodOdo != 0)
            {
                //var IdCarnet = i.IdNroCarnet;

                var resultado = ListarDatosDeOdontograma(CodOdo);
                foreach (var item in resultado)
                {
                    Tbody = item.ArregloOdontograma;
                }
                return Json(Tbody, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }


        public List<E_CSMedicina> ListarDatosDeOdontograma(int CodOdo)
        {

            string sede = Session["CodSede"].ToString();

            List<E_CSMedicina> ListaArregloOdo = new List<E_CSMedicina>();

            try
            {
                using (db = new SqlConnection(cadena))
                {
                    using (cmd = new SqlCommand("Usp_MostrarDatosDeOdontogramaPorCodOdontograma", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodOdontograma", CodOdo);
                        cmd.Parameters.AddWithValue("@CodSede", sede);
                        db.Open();
                        SqlDataReader dr = cmd.ExecuteReader();


                        while (dr.Read())
                        {
                            E_CSMedicina med = new E_CSMedicina();

                            med.ArregloOdontograma = dr["ArregloOdontograma"].ToString();

                            ListaArregloOdo.Add(med);

                        }

                    }
                }

                return ListaArregloOdo;
            }
            catch (Exception e)
            {
                throw;
            }
            
        }



        public ActionResult ListaPacientesAtendidosDelDia()
        {
            ViewBag.Lista = ListarPacientesAtendidosEnElDia_CSMedicina();
            return View();
        }


        public List<E_CSMedicina> ListarPacientesAtendidosEnElDia_CSMedicina()
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSMedicina> ListaPacientes = new List<E_CSMedicina>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_ListadoPacientesAtendidosEnElDia_CSMedicina", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_CSMedicina Med = new E_CSMedicina();

                        Med.Id = int.Parse(dr["Id"].ToString());
                        Med.CodCue = int.Parse(dr["CodCue"].ToString());
                        Med.Paciente = dr["NombrePaciente"].ToString();
                        Med.DesTipoCarnet = dr["DescCarnet"].ToString();
                        Med.Manipulador = dr["Manipulador"].ToString();
                        Med.idEstado = Convert.ToInt32(dr["IdEstado"].ToString());
                        Med.Edad = Convert.ToInt32(dr["Edad"]);
                        ListaPacientes.Add(Med);

                    }


                }
            }

            return ListaPacientes;
        }


        public ActionResult ModificarDatosPacientesAtendidos(int id)
        {
            var MostrarDatosMedicina = MostrarDatosDeListaDeCarnetSanidadCSMedicina(id).FirstOrDefault();
            
            ViewBag.ListaFechaOdontograma = ListadoDeFechaDeOdontogramasRegistrados(id);

            var DatosDeCSMedicina = ListarDatosDeCSMedicinaPorId(id).FirstOrDefault();
            ViewBag.Conclusion = DatosDeCSMedicina.Conclusion;
            ViewBag.AptoMed = DatosDeCSMedicina.AptoMed;
            ViewBag.Reevaluado = DatosDeCSMedicina.Reevaluado;
            ViewBag.Observaciones = DatosDeCSMedicina.Observaciones;
            //ViewBag.DatosDeCSMedicina = ListarDatosDeCSMedicinaPorId(id);

            return View(MostrarDatosMedicina);
        }


        public List<E_CSMedicina> ListarDatosDeCSMedicinaPorId(int id)
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSMedicina> ListarDatos = new List<E_CSMedicina>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_MostrarDatosDeCSMedicinaPorId", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdMedicina", id);
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_CSMedicina med = new E_CSMedicina();

                        med.Conclusion = dr["Conclusion"].ToString();
                        med.AptoMed = dr["Apto"].ToString();
                        med.Reevaluado = dr["Reevaluado"].ToString();
                        med.Observaciones = dr["Observaciones"].ToString();

                        ListarDatos.Add(med);

                    }


                }
            }

            return ListarDatos;
        }

        [HttpPost]
        public ActionResult ModificarDatosPacientesAtendidos(int Id,E_CSMedicina med)
        {
            string sede = Session["CodSede"].ToString();
            UtilitarioController ut = new UtilitarioController();
            var horasis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string modifica = Session["usuario"] + " " + horasis.HoraServidor.ToString() + " " + Environment.MachineName;

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_ActualizarDatosCSMedicinaCS", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    if (med.Conclusion != null)
                    {
                        cmd.Parameters.AddWithValue("@Conclusion", med.Conclusion);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Conclusion", "");
                    }
                    if (med.AptoMed != null)
                    {
                        cmd.Parameters.AddWithValue("@Apto", med.AptoMed);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Apto", "");
                    }
                    if (med.Reevaluado != null)
                    {
                        cmd.Parameters.AddWithValue("@Reevaluado", med.Reevaluado);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Reevaluado", "NO");
                    }
                    if (med.Observaciones != null)
                    {
                        cmd.Parameters.AddWithValue("@Observaciones", med.Observaciones);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Observaciones", "");
                    }
                    cmd.Parameters.AddWithValue("@Modifica", modifica);
                    cmd.Parameters.AddWithValue("@Sede", sede);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    return RedirectToAction("ListaPacientesAtendidosDelDia");

                }
            }
            
        }





    }

}