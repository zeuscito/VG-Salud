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
    }
}