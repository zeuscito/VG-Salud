using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using VgSalud.Models;
using System.Configuration;
using System.Globalization;

namespace VgSalud.Controllers
{
    public class PagoResumenController : Controller
    {
        public List<E_CajaResumen> BuscaMedico(string codigo, string dia)
        {
            List<E_CajaResumen> Lista = new List<E_CajaResumen>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_buscaMedico_CodEsp", con))
                {
                    cmd.Parameters.AddWithValue("@CodServ", codigo);
                    cmd.Parameters.AddWithValue("@dia", dia);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_CajaResumen Ser = new E_CajaResumen();
                            
                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public ActionResult Index()
        {
            return View();
        }
    }
}