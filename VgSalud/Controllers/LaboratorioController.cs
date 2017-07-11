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
    public class LaboratorioController : Controller
    {
        // GET: Laboratorio
        public ActionResult Index()
        {
            return View();
        }

        string cadena = ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString;
        SqlConnection db;
        SqlCommand cmd;

        public bool Usp_Mantenimiento(int id, int evento)
        {
            try
            {
                using (db = new SqlConnection(cadena))
                {
                    using (cmd = new SqlCommand("Usp_Validaciones_Cslaboratorios", db))
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
        public ActionResult ListadoCarnetLaboratorioEnEspera()
        {
            ViewBag.lista = ListadoCarnet_Laboratorio_DelDia_EnEspera();
            ViewBag.postergado = Usp_Laboratorio_Postergados();
            return View();
        }

        [HttpPost]
        public ActionResult ListadoCarnetLaboratorioEnEspera(E_CSLaboratorio lab)
        {
            ViewBag.lista = ListadoCarnet_Laboratorio_DelDia_EnEspera();
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
                return RedirectToAction("ListadoCarnetLaboratorioEnEspera");

            }
            catch (Exception)
            {
                ViewBag.mensaje = "Error: No se pudo procesar la Actualizacion Correctamente";
                return RedirectToAction("ListadoCarnetLaboratorioEnEspera");
            }


        }

        public List<E_CSLaboratorio> Usp_Laboratorio_Postergados()
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSLaboratorio> ListaCarnetLab_E = new List<E_CSLaboratorio>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_Laboratorio_Postergados", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_CSLaboratorio lab = new E_CSLaboratorio();

                        lab.Id = int.Parse(dr["Id"].ToString());
                        lab.CodCue = int.Parse(dr["CodCue"].ToString());
                        lab.Paciente = dr["NombrePaciente"].ToString();
                        lab.DesTipoCarnet = dr["DescCarnet"].ToString();
                        lab.Manipulador = dr["Manipulador"].ToString();
                        lab.Prioridad = Convert.ToInt32(dr["Prioridad"].ToString());
                        lab.idEstado = Convert.ToInt32(dr["IdEstado"].ToString());
                        ListaCarnetLab_E.Add(lab);

                    }


                }
            }

            return ListaCarnetLab_E;
        }

        public List<E_CSLaboratorio> ListadoCarnet_Laboratorio_DelDia_EnEspera()
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSLaboratorio> ListaCarnetLab_E = new List<E_CSLaboratorio>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("usp_ListadoCarnetLaboratorio_EnEspera", db))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_CSLaboratorio lab = new E_CSLaboratorio();

                        lab.Id = int.Parse(dr["Id"].ToString());
                        lab.CodCue = int.Parse(dr["CodCue"].ToString());
                        lab.Paciente = dr["NombrePaciente"].ToString();
                        lab.DesTipoCarnet = dr["DescCarnet"].ToString();
                        lab.Manipulador = dr["Manipulador"].ToString();
                        lab.Prioridad = Convert.ToInt32(dr["Prioridad"].ToString());
                        lab.idEstado = Convert.ToInt32(dr["IdEstado"].ToString());
                        lab.Edad = Convert.ToInt32(dr["edad"]);
                        ListaCarnetLab_E.Add(lab);

                    }


                }
            }

            return ListaCarnetLab_E;
        }


        public ActionResult ActualizarDatosAtencionLaboratorio(int Id)
        {
            
            //Usp_ListarDatosCSLaboratorio(Id);
            ViewBag.lista = MostrarDatosDeListaDeCarnetSanidadCSLaboratorio(Id);
            //return RedirectToAction("ActualizarDatosAtencionLaboratorio");
            return View();
        }

        [HttpPost]
        public ActionResult ActualizarDatosAtencionLaboratorio(int Id,string procedencia, E_CSLaboratorio lab)
        {
            string sede = Session["CodSede"].ToString();

            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string modifica= Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
            lab.Id = Id;
            lab.Procedencia = procedencia;

            try
            {
                using (db = new SqlConnection(cadena))
                {
                    if (lab.Procedencia == "1".ToString())
                    {
                        using (cmd = new SqlCommand("Usp_ActualizarDatosCSLaboratorioCS", db))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Id", lab.Id);
                            cmd.Parameters.AddWithValue("@MuestraSangre", lab.MuestraS);
                            cmd.Parameters.AddWithValue("@MuestraHeces", lab.MuestraH);

                            if (lab.RPR != null)
                            {
                                cmd.Parameters.AddWithValue("@RPR", lab.RPR);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@RPR", "");
                            }

                            if (lab.Parasitologico!=null)
                            {
                                cmd.Parameters.AddWithValue("@Parasitologico", lab.Parasitologico);
                            }else
                            {
                                cmd.Parameters.AddWithValue("@Parasitologico", "");
                            }

                            if (lab.Observacion != null)
                            {
                                cmd.Parameters.AddWithValue("@Observacion", lab.Observacion);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Observacion", "");
                            }
                            cmd.Parameters.AddWithValue("@Apto", lab.Apto);
                            cmd.Parameters.AddWithValue("@Modifica", modifica);
                            cmd.Parameters.AddWithValue("@Sede", sede);
                            db.Open();
                            SqlDataReader dr = cmd.ExecuteReader();
                            
                            return RedirectToAction("ListadoCarnetLaboratorioEnEspera"); 
                        }
                    }
                    else
                    {
                        using (cmd = new SqlCommand("Usp_ActualizarDatosCSLaboratorioPN", db))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Id", lab.Id);
                            cmd.Parameters.AddWithValue("@MuestraSangre", lab.MuestraS);
                            cmd.Parameters.AddWithValue("@MuestraHeces", lab.MuestraH);
                            if (lab.RPR != null)
                            {
                                cmd.Parameters.AddWithValue("@RPR", lab.RPR);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@RPR", "");
                            }
                            
                            if (lab.HIV != null)
                            {
                                cmd.Parameters.AddWithValue("@HIV", lab.HIV);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@HIV", "");
                            }

                            if (lab.GrupoFactor != null)
                            {
                                cmd.Parameters.AddWithValue("@GrupoFactor", lab.GrupoFactor);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@GrupoFactor", "");
                            }

                            if (lab.Observacion != null)
                            {
                                cmd.Parameters.AddWithValue("@Observacion", lab.Observacion);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Observacion", "");
                            }
                            cmd.Parameters.AddWithValue("@Apto", lab.Apto);
                            cmd.Parameters.AddWithValue("@Modifica", modifica);
                            cmd.Parameters.AddWithValue("@Sede", sede);
                            db.Open();
                            SqlDataReader dr = cmd.ExecuteReader();

                            return RedirectToAction("ListadoCarnetLaboratorioEnEspera");
                        }
                        
                    }

                }
            }
            catch (Exception)
            {
                return RedirectToAction("ActualizarDatosAtencionLaboratorio");
            }
        }
        
        public bool Usp_ActualizarDatosCSLaboratorio(int id,string Procedencia)
        {
            try
            {
                string sede = Session["CodSede"].ToString();

                using (db = new SqlConnection(cadena))
                {
                    if (Procedencia == "1".ToString())
                    {
                        using (cmd = new SqlCommand("Usp_ActualizarDatosCSLaboratorioCS", db))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.Parameters.AddWithValue("@MuestraSangre", sede);
                            cmd.Parameters.AddWithValue("@MuestraHeces", sede);
                            cmd.Parameters.AddWithValue("@RPR", sede);
                            cmd.Parameters.AddWithValue("@Parasitologico", sede);
                            cmd.Parameters.AddWithValue("@Observacion", sede);
                            cmd.Parameters.AddWithValue("@Apto", sede);
                            cmd.Parameters.AddWithValue("@Modifica", "");
                            cmd.Parameters.AddWithValue("@Sede", sede);
                            db.Open();
                            SqlDataReader dr = cmd.ExecuteReader();
                            return true;
                        }
                    }else
                    {
                        return false;
                    }
                        
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Usp_ListarDatosCSLaboratorio(int id)
        {
            try
            {
                string sede = Session["CodSede"].ToString();
                List<E_CSLaboratorio> ListaCarnetLab = new List<E_CSLaboratorio>();

                using (db = new SqlConnection(cadena))
                {
                    using (cmd = new SqlCommand("Usp_MostrarDatosDeListaDeCarnetSanidadCSLaboratorio", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@Sede", sede);
                        db.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            E_CSLaboratorio lab = new E_CSLaboratorio();

                            lab.Id = int.Parse(dr["Id"].ToString());
                            lab.CodCue = int.Parse(dr["CodCue"].ToString());
                            lab.Paciente = dr["NombrePaciente"].ToString();
                            lab.DesTipoCarnet = dr["DescCarnet"].ToString();
                            lab.Manipulador = dr["Manipulador"].ToString();
                            lab.Prioridad = Convert.ToInt32(dr["Prioridad"].ToString());
                            lab.idEstado = Convert.ToInt32(dr["IdEstado"].ToString());
                            lab.Edad = Convert.ToInt32(dr["edad"]);
                            lab.NumeroCarnet = dr["NumeroCarnet"].ToString();
                            ListaCarnetLab.Add(lab);

                        }
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            
        }


        public List<E_CSLaboratorio> MostrarDatosDeListaDeCarnetSanidadCSLaboratorio(int Id)
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSLaboratorio> ListaCarnetLab = new List<E_CSLaboratorio>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_MostrarDatosDeListaDeCarnetSanidadCSLaboratorio", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Sede", sede);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_CSLaboratorio lab = new E_CSLaboratorio();

                        lab.Id = int.Parse(dr["Id"].ToString());
                        lab.CodCue = int.Parse(dr["CodCue"].ToString());
                        lab.Paciente = dr["NombrePaciente"].ToString();
                        lab.DesTipoCarnet = dr["DescCarnet"].ToString();
                        lab.Manipulador = dr["Manipulador"].ToString();
                        lab.Prioridad = Convert.ToInt32(dr["Prioridad"].ToString());
                        lab.idEstado = Convert.ToInt32(dr["IdEstado"].ToString());
                        lab.Edad = Convert.ToInt32(dr["edad"]);
                        lab.NumeroCarnet = dr["NumeroCarnet"].ToString();
                        lab.Procedencia = dr["Procedencia"].ToString();
                        ListaCarnetLab.Add(lab);

                    }


                }
            }

            return ListaCarnetLab;
        }



        public ActionResult ListaPacientesAtendidosDelDia()
        {
            ViewBag.Lista = ListarPacientesAtendidosEnElDia();

            return View();
        }

        public List<E_CSLaboratorio> ListarPacientesAtendidosEnElDia()
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSLaboratorio> ListaPacientes = new List<E_CSLaboratorio>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_ListadoPacientesAtendidosEnElDia_CSLaboratorio", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_CSLaboratorio lab = new E_CSLaboratorio();

                        lab.Id = int.Parse(dr["Id"].ToString());
                        lab.CodCue = int.Parse(dr["CodCue"].ToString());
                        lab.Paciente = dr["NombrePaciente"].ToString();
                        lab.DesTipoCarnet = dr["DescCarnet"].ToString();
                        lab.Manipulador = dr["Manipulador"].ToString();
                        lab.idEstado = Convert.ToInt32(dr["IdEstado"].ToString());
                        lab.Edad = Convert.ToInt32(dr["Edad"]);
                        ListaPacientes.Add(lab);

                    }


                }
            }

            return ListaPacientes;
        }

    }
}