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


        public ActionResult ListadoCarnetLaboratorioEnEspera(int? CodCue, string NumDoc = null)
        {
            ViewBag.lista = ListadoCarnet_Laboratorio_DelDia_EnEspera();

            ViewBag.NumDoc = NumDoc;
            ViewBag.CodCue = CodCue;
            int Cuenta = 0;
            if (CodCue == null)
            {
                Cuenta = Convert.ToInt32(CodCue);
            }
            ViewBag.postergado = Usp_Laboratorio_Postergados(Cuenta, NumDoc);
            return View();
        }

        [HttpPost]
        public ActionResult ListadoCarnetLaboratorioEnEspera(E_CSLaboratorio lab, int? CodCue, string NumDoc = null)
        {
            int Cuenta = 0;
            if (NumDoc != "" || CodCue != null)
            {
                ViewBag.lista = ListadoCarnet_Laboratorio_DelDia_EnEspera();
                Cuenta = Convert.ToInt32(CodCue);
                ViewBag.postergado = Usp_Laboratorio_Postergados(Cuenta, NumDoc);
                ViewBag.NumDoc = NumDoc;
                ViewBag.CodCue = CodCue;

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
                    Usp_Mantenimiento(lab.Id, 5);
                    ViewBag.lista = ListadoCarnet_Laboratorio_DelDia_EnEspera();
                    ViewBag.NumDoc = null;
                    ViewBag.CodCue = null;
                    return RedirectToAction("ListadoCarnetLaboratorioEnEspera");

                }
                catch (Exception)
                {
                    ViewBag.mensaje = "Error: No se pudo procesar la Actualizacion Correctamente";
                    return RedirectToAction("ListadoCarnetLaboratorioEnEspera");
                }
            }
            else
            {
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
                //ViewBag.lista = ListadoCarnet_Laboratorio_DelDia_EnEspera();
            
        }

        public List<E_CSLaboratorio> Usp_Laboratorio_Postergados(int Cuenta, string NumDoc = null)
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSLaboratorio> ListaCarnetLab_E = new List<E_CSLaboratorio>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_Laboratorio_Postergados", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (NumDoc == null || NumDoc == "")
                    {
                        cmd.Parameters.AddWithValue("@NumDoc", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@NumDoc", NumDoc);
                    }
                    if (Cuenta == 0)
                    {
                        cmd.Parameters.AddWithValue("@CodCue", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CodCue", Cuenta);
                    }
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

                            if (lab.Apto != null)
                            {
                                cmd.Parameters.AddWithValue("@Apto", lab.Apto);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Apto", "");
                            }

                            //cmd.Parameters.AddWithValue("@Apto", lab.Apto);
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

                            if (lab.Apto != null)
                            {
                                cmd.Parameters.AddWithValue("@Apto", lab.Observacion);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Apto", "");
                            }

                            //cmd.Parameters.AddWithValue("@Apto", lab.Apto);
                            cmd.Parameters.AddWithValue("@Modifica", modifica);
                            cmd.Parameters.AddWithValue("@Sede", sede);
                            db.Open();
                            SqlDataReader dr = cmd.ExecuteReader();

                            return RedirectToAction("ListadoCarnetLaboratorioEnEspera");
                        }
                        
                    }

                }
            }
            catch (Exception e)
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



        public ActionResult ListaPacientesAtendidosDelDia(int? CodCue, string NumDoc = null, DateTime? FechaAten = null)
        {
            ViewBag.CodCue = CodCue;
            ViewBag.NumDoc = NumDoc;
            
            ViewBag.FechaAten = FechaAten;
            int Cuenta = 0;
            if (CodCue == null)
            {
                Cuenta = 0;
            }
            else
            {
                Cuenta = Convert.ToInt32(CodCue);
            }
            ViewBag.Lista = ListarPacientesAtendidosEnElDia(Cuenta, NumDoc, FechaAten);

            return View();
        }

        public List<E_CSLaboratorio> ListarPacientesAtendidosEnElDia(int Cuenta, string NumDoc, DateTime? FechaAten)
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSLaboratorio> ListaPacientes = new List<E_CSLaboratorio>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_ListadoPacientesAtendidosEnElDia_CSLaboratorio", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (NumDoc == null || NumDoc == "")
                    {
                        cmd.Parameters.AddWithValue("@NumDoc", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@NumDoc", NumDoc);
                    }
                    if (Cuenta == 0)
                    {
                        cmd.Parameters.AddWithValue("@CodCue", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CodCue", Cuenta);
                    }

                    if (FechaAten == null)
                    {
                        cmd.Parameters.AddWithValue("@FechaAte", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@FechaAte", FechaAten);
                    }

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
                        lab.Apto = dr["Apto"].ToString();
                        ListaPacientes.Add(lab);

                    }


                }
            }

            return ListaPacientes;
        }

       

        public ActionResult ModificarDatosPacientesAtendidosDelDia(int IdLaboratorio)
        {
            ViewBag.lista = MostrarDatosDeListaDeCarnetSanidadCSLaboratorio(IdLaboratorio);

            var MostrarDatosLaboratorio = Buscar_Y_MostrarDatosCSLaboratorioPorId(IdLaboratorio).FirstOrDefault();

            //ViewBag.MuestraS = MostrarDatosLaboratorio.MuestraS;

            ViewBag.MuestraS = MostrarDatosLaboratorio.MuestraS;
            ViewBag.MuestraH = MostrarDatosLaboratorio.MuestraH;

            ViewBag.RPR = MostrarDatosLaboratorio.RPR;
            ViewBag.Parasitologico = MostrarDatosLaboratorio.Parasitologico;
            
            ViewBag.Observacion = MostrarDatosLaboratorio.Observacion;

            ViewBag.Apto = MostrarDatosLaboratorio.Apto;

            return View(MostrarDatosLaboratorio);
        }


        [HttpPost]
        public ActionResult ModificarDatosPacientesAtendidosDelDia(int Id, E_CSLaboratorio lab)
        {
            string sede = Session["CodSede"].ToString();

            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string modifica = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
            lab.Id = Id;

            try
            {
                using (db = new SqlConnection(cadena))
                {

                    using (cmd = new SqlCommand("Usp_ModificarDatosPacientesAtendidosDelDia", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", lab.Id);
                        
                        if (lab.RPR != null)
                        {
                            cmd.Parameters.AddWithValue("@RPR", lab.RPR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@RPR", "");
                        }

                        if (lab.Parasitologico != null)
                        {
                            cmd.Parameters.AddWithValue("@Parasitologico", lab.Parasitologico);
                        }
                        else
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

                        if (lab.Apto != null)
                        {
                            cmd.Parameters.AddWithValue("@Apto", lab.Apto);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Apto", "");
                        }

                        cmd.Parameters.AddWithValue("@Modifica", modifica);
                        cmd.Parameters.AddWithValue("@Sede", sede);
                        db.Open();
                        SqlDataReader dr = cmd.ExecuteReader();

                        return RedirectToAction("ListaPacientesAtendidosDelDia");
                    }
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ModificarDatosPacientesAtendidosDelDia");
            }

        }


        public List<E_CSLaboratorio> Buscar_Y_MostrarDatosCSLaboratorioPorId(int IdLaboratorio)
        {
            string CodSede = Session["CodSede"].ToString();

            List<E_CSLaboratorio> Lista = new List<E_CSLaboratorio>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Buscar_Y_MostrarDatosCSLaboratorioPorId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdLaboratorio", IdLaboratorio);
                    cmd.Parameters.AddWithValue("@CodSede", CodSede);
                    using (
                        SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_CSLaboratorio CSLab = new E_CSLaboratorio();

                            CSLab.Id = dr.GetInt32(0);
                            CSLab.MuestraS = dr.GetBoolean(1);
                            CSLab.MuestraH = dr.GetBoolean(2);
                            CSLab.Parasitologico = dr.GetString(3);
                            CSLab.RPR = dr.GetString(4);
                            CSLab.Observacion = dr.GetString(5);
                            CSLab.Apto = dr.GetString(6);

                            Lista.Add(CSLab);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        


    }
}