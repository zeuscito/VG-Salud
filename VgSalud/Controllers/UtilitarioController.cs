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
    public class UtilitarioController : Controller
    {
        // GET: Utilitario
        public ActionResult ListaSexo()
        {
            return View(ListadoSexo());
        }

        public List<E_Sexo> ListadoSexo()
        {
            List<E_Sexo> Lista = new List<E_Sexo>();
            using(SqlConnection con=new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using(SqlCommand cmd=new SqlCommand("Usp_Lista_Sexo", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Sexo Sex = new E_Sexo();
                            Sex.CodSexo = dr.GetString(0);
                            Sex.NomSexo = dr.GetString(1).ToUpper();
                            Sex.Estado = dr.GetBoolean(2);
                            Lista.Add(Sex);
                        }
                        con.Close();
                    }
                }
                return Lista;
            }
            
        }


        public List<E_Usuario> ListadoUsuarioEspecialidadGeneral(string CodUsu)
        {
            List<E_Usuario> Lista = new List<E_Usuario>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_UsuarioEspecialidadGeneral", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodUsu", CodUsu);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Usuario Sex = new E_Usuario();
                            Sex.codUsu = dr.GetString(0);
                            Sex.Concatena = dr.GetString(1);
                            Lista.Add(Sex);
                        }
                        con.Close();
                    }
                }
                return Lista;
            }

        }

        public List<E_Master> ListadoHoraServidor()
        {
            List<E_Master> Lista = new List<E_Master>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_hora", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Master de = new E_Master();
                            de.HoraServidor = dr.GetDateTime(0);
                            Lista.Add(de);
                        }
                        con.Close();
                    }
                }
                return Lista;
            }

        }



        public ActionResult ListaDocumentoIdentidad()
        {
           
                return View(ListadoDocumentoIdentidad());
          
        }

        public List<E_Documento_Identidad> ListadoDocumentoIdentidad()
        {
            List<E_Documento_Identidad> Lista = new List<E_Documento_Identidad>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Documento_Identidad", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Documento_Identidad EDoc = new E_Documento_Identidad();

                            EDoc.CodDocIdent = dr.GetString(0);
                            EDoc.NomDocIdent = dr.GetString(1);
                            EDoc.Estado = dr.GetBoolean(2);
                            Lista.Add(EDoc);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }





        public ActionResult ListaEstadoCivil()
        {

            return View(ListadoEstadoCivil());

        }

        public List<E_EstadoCivil> ListadoEstadoCivil()
        {
            List<E_EstadoCivil> Lista = new List<E_EstadoCivil>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Estado_Civil", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_EstadoCivil ECiv = new E_EstadoCivil();

                            ECiv.CodEstCivil = dr.GetString(0);
                            ECiv.NomEstCivil = dr.GetString(1).ToUpper();
                            ECiv.Estado = dr.GetBoolean(2);

                            Lista.Add(ECiv);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult ListaPais()
        {

            return View(ListadoPais());

        }

        public List<E_Pais> ListadoPais()
        {
            List<E_Pais> Lista = new List<E_Pais>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Pais", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Pais EPac = new E_Pais();

                            EPac.CodPais = dr.GetString(0).Trim();
                            EPac.NomPais = dr.GetString(1);
                            EPac.AbrvPais = dr.GetString(2);
                            EPac.EstPais = dr.GetBoolean(3);
                            Lista.Add(EPac);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult ListaDepartamento(string id)
        {
            var evalua = (List<E_Departamento>)(from a in ListadoDepartamento(id) select a).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;

        }

        public List<E_Departamento> ListadoDepartamento(string CodPais)
        {
            List<E_Departamento> Lista = new List<E_Departamento>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Departamento", con))
                {
                    cmd.Parameters.AddWithValue("@CodPais", CodPais);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Departamento ECiv = new E_Departamento();

                            ECiv.CodDep = dr.GetString(0);
                            ECiv.NomDep = dr.GetString(1);
                            ECiv.EstDep = dr.GetBoolean(2);
                            Lista.Add(ECiv);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public ActionResult ListaProvincia(string id)
        {
            var evalua = (List<E_Provincia>)(from a in ListadoProvincia(id) select a).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
          
        }

        public List<E_Provincia> ListadoProvincia(string CodDep)
        {
            List<E_Provincia> Lista = new List<E_Provincia>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Provincia", con))
                {
                    cmd.Parameters.AddWithValue("@CodDep", CodDep);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Provincia ECiv = new E_Provincia();

                            ECiv.CodProv = dr.GetString(0);
                            ECiv.NomProv = dr.GetString(1);
                            ECiv.EstProv = dr.GetBoolean(2);
                            Lista.Add(ECiv);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public ActionResult ListaDistrito(string id)
        {
            var evalua = (List<E_Distrito>)(from a in ListadoDistrito(id) select a).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
         
        }

        public List<E_Distrito> ListadoDistrito(string CodProv)
        {
            List<E_Distrito> Lista = new List<E_Distrito>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Distrito", con))
                {
                    cmd.Parameters.AddWithValue("@CodProv", CodProv);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Distrito EDis = new E_Distrito();

                            EDis.CodDist = dr.GetString(0);
                            EDis.NomDist = dr.GetString(1);
                            EDis.EstDist = dr.GetBoolean(2);
                            Lista.Add(EDis);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        //-------------


        public ActionResult ListaDepartamentoSimple()
        {
            return View(ListadoDepartamentoSimple());

        }

        public List<E_Departamento> ListadoDepartamentoSimple()
        {
            List<E_Departamento> Lista = new List<E_Departamento>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Departamento_Simple", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Departamento ECiv = new E_Departamento();

                            ECiv.CodDep = dr.GetString(0);
                            ECiv.NomDep = dr.GetString(1);
                            ECiv.EstDep = dr.GetBoolean(2);
                            Lista.Add(ECiv);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public ActionResult ListaProvinciaSimple()
        {
            return View(ListadoProvinciaSimple());

        }

        public List<E_Provincia> ListadoProvinciaSimple()
        {
            List<E_Provincia> Lista = new List<E_Provincia>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Provincia_Simple", con))
                {
                   
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Provincia ECiv = new E_Provincia();

                            ECiv.CodProv = dr.GetString(0);
                            ECiv.NomProv = dr.GetString(1);
                            ECiv.EstProv = dr.GetBoolean(2);
                            Lista.Add(ECiv);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public ActionResult ListaDistritoSimple()
        {

            return View(ListadoDistritoSimple());

        }

        public List<E_Distrito> ListadoDistritoSimple()
        {
            List<E_Distrito> Lista = new List<E_Distrito>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Distrito_Simple", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Distrito EDis = new E_Distrito();

                            EDis.CodDist = dr.GetString(0);
                            EDis.NomDist = dr.GetString(1);
                            EDis.EstDist = dr.GetBoolean(2);
                            Lista.Add(EDis);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public ActionResult ListaSede_CodEmp(string id)
        {
            var evalua = (List<E_Sede>)(from a in ListadoSede_CodEmp(id) select a).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;

        }



        public List<E_Sede> ListadoSede_CodEmp(string CodEmp)
        {
            List<E_Sede> Lista = new List<E_Sede>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Sedes_CodEmp", con))
                {
                    cmd.Parameters.AddWithValue("@CodEmp", CodEmp);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Sede ESed = new E_Sede();

                            ESed.CodSede = dr.GetString(0);
                            ESed.NomSede = dr.GetString(1);

                            Lista.Add(ESed);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }




    }
}