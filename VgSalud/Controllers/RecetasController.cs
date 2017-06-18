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
    public class RecetasController : Controller
    {

        public List<E_FormaFarmaceutica> ListadoFormaFarmaceutica()
        {
            List<E_FormaFarmaceutica> Lista = new List<E_FormaFarmaceutica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ListadoFormaFarmaceutica", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_FormaFarmaceutica Etip = new E_FormaFarmaceutica();

                            Etip.idFormFarm = dr.GetInt32(0);
                            Etip.Descripcion = dr.GetString(1);
                            Etip.Estado = dr.GetBoolean(2);

                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_FrecuenciaReceta> ListadoFrecuenciaRecetas()
        {
            List<E_FrecuenciaReceta> Lista = new List<E_FrecuenciaReceta>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ListadoFrecuenciaReceta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_FrecuenciaReceta Etip = new E_FrecuenciaReceta();

                            Etip.idFrec = dr.GetInt32(0);
                            Etip.Descripcion = dr.GetString(1);
                            Etip.Estado = dr.GetBoolean(2);

                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }



    }
}