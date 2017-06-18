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
    public class DatosGeneralesController : Controller
    {
        //actualizar 100
        public List<E_Datos_Generales> listadatogenerales()
        {
            List<E_Datos_Generales> Lista = new List<E_Datos_Generales>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("USP_LISTA_GENERALES", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Datos_Generales datos = new E_Datos_Generales();

                            datos.igv = dr.GetDecimal(0);
                            datos.Tipo_Cambio = dr.GetDecimal(1);
                            datos.MUESTRA_ANTECEDENTE = dr.GetBoolean(2);
                            datos.ATENCIONESPAGADAS = dr.GetBoolean(3);
                            datos.MODULOS = dr["MODULOS"] is DBNull ? string.Empty : DesEncriptar(dr["MODULOS"].ToString());
                            if (!string.IsNullOrWhiteSpace(datos.MODULOS))
                            {
                                string[] splite = datos.MODULOS.Split('-');
                                datos.fecha = splite[0];
                                datos.sedes = splite[1];
                                datos.servicio = splite[2];
                            }
                            datos.PREGXATENCIONPROGRAMADAS = dr["PREGXATENCIONPROGRAMADAS"] is DBNull ? false : Convert.ToBoolean(dr["PREGXATENCIONPROGRAMADAS"]);
                            datos.PREGXATENCIONNOPROGRAMADAS = dr["PREGXATENCIONNOPROGRAMADAS"] is DBNull ? false : Convert.ToBoolean(dr["PREGXATENCIONNOPROGRAMADAS"]);
                            datos.GENERARCUENTAAUTO = dr["GENERARCUENTAAUTO"] is DBNull ? false : Convert.ToBoolean(dr["GENERARCUENTAAUTO"]);
                            datos.MOSTRARPACIENTETICKET = dr["MOSTRARPACIENTETICKET"] is DBNull ? false : Convert.ToBoolean(dr["MOSTRARPACIENTETICKET"].ToString());
                            Lista.Add(datos);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Datos_Generales> LISTA_DISTRITO_DEFAULT(string sede)
        {
            List<E_Datos_Generales> lista = new List<E_Datos_Generales>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Distrito_Default", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codSede", sede);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Datos_Generales datos = new E_Datos_Generales(); 
                            datos.CodDist = dr["DISTXDEFECT"] is DBNull ? string.Empty : dr["DISTXDEFECT"].ToString();
                            datos.CODSEDE = dr["CODSEDE"] is DBNull ? string.Empty : dr["CODSEDE"].ToString();
                            datos.NomDist = dr["NOMDIST"] is DBNull ? "[NO SELECCIONADO]" : dr["NOMDIST"].ToString();
                            lista.Add(datos);
                        }
                        con.Close();
                    }
                }
                return lista;
            }
        }

        public E_Datos_Generales Getdatogenerales()
        {
            E_Datos_Generales get = new E_Datos_Generales();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("USP_LISTA_GENERALES", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            get.igv = dr.GetDecimal(0);
                            get.Tipo_Cambio = dr.GetDecimal(1);
                            get.MUESTRA_ANTECEDENTE = dr.GetBoolean(2);
                            get.ATENCIONESPAGADAS = dr.GetBoolean(3);
                            get.MODULOS = dr["MODULOS"] is DBNull ? string.Empty : DesEncriptar(dr["MODULOS"].ToString());
                            if (!string.IsNullOrWhiteSpace(get.MODULOS))
                            {
                                string[] splite = get.MODULOS.Split('-');
                                get.fecha = splite[0];
                                get.sedes = splite[1];
                                get.servicio = splite[2];
                            }
                            get.PREGXATENCIONPROGRAMADAS = dr["PREGXATENCIONPROGRAMADAS"] is DBNull ? false : Convert.ToBoolean(dr["PREGXATENCIONPROGRAMADAS"]);
                            get.PREGXATENCIONNOPROGRAMADAS = dr["PREGXATENCIONNOPROGRAMADAS"] is DBNull ? false : Convert.ToBoolean(dr["PREGXATENCIONNOPROGRAMADAS"]);
                            get.GENERARCUENTAAUTO = dr["GENERARCUENTAAUTO"] is DBNull ? false : Convert.ToBoolean(dr["GENERARCUENTAAUTO"]);
                            get.GENERARCUENTAAUTO = dr["GENERARCUENTAAUTO"] is DBNull ? false : Convert.ToBoolean(dr["GENERARCUENTAAUTO"]);
                            get.MOSTRARPACIENTETICKET = dr["MOSTRARPACIENTETICKET"] is DBNull ? false : Convert.ToBoolean(dr["MOSTRARPACIENTETICKET"].ToString());
                        }
                        con.Close();
                    }

                }
                return get;
            }
        }

        public string Encriptar(string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        public string DesEncriptar(string _cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }


        public ActionResult PermisosAdmin()
        {
            ViewBag.lista = listadatogenerales();
            return View();
        }

        public bool getUser(string user)
        {
            var usuario = new UsuarioController();
            var user1 = usuario.listaUsuarios().Where(x => x.codUsu == user).FirstOrDefault();
            if (user1.codUsu == "00001")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public ActionResult PermisosAdmin(E_Datos_Generales Datos)
        {
            var userID = Session["UserID"].ToString();
            ViewBag.lista = listadatogenerales();
            try
            {

                if (!string.IsNullOrWhiteSpace(Datos.fecha) && !string.IsNullOrWhiteSpace(Datos.servicio) && !string.IsNullOrWhiteSpace(Datos.sedes))
                {
                    if (Datos.servicio.Length == 4 && Datos.sedes.Length == 4)
                    {
                        if (userID == "00001")
                        {

                            string fecha = Datos.fecha;
                            Datos.MODULOS = $"{fecha}-{Datos.sedes}-{Datos.servicio}";
                            string encript = Encriptar(Datos.MODULOS);
                            string sql = "update DATOS_GENERALES set MODULOS = @Modulos";
                            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                            {
                                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.AddWithValue("@Modulos", encript);
                                    cnn.Open();
                                    if (cmd.ExecuteNonQuery() > 0)
                                    {
                                        ViewBag.mensaje = "2";
                                        ViewBag.lista = listadatogenerales();
                                        return View(Datos);
                                    };
                                }
                            }

                        }
                        else
                        {

                            ViewBag.mensaje = "Usuario no Valido para Ejecutar esta Accion";
                            return View(Datos);
                        }

                    }
                    else
                    {
                        ViewBag.mensaje = "Error Datos no Precisos";
                        return View(Datos);
                    }

                }
                else
                {
                    ViewBag.mensaje = "Error Datos no Precisos";
                    return View(Datos);
                }

            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Error Datos no Precisos";
                return View(Datos);
            }

            return View(Datos);
        }

        public ActionResult ListarDatosGenerales()
        {
            var sede = Session["CodSede"].ToString();
            var result = LISTA_DISTRITO_DEFAULT(sede);
            if (result.Count > 0)
            {
                ViewBag.distrito = result.FirstOrDefault().NomDist;
            }
            else
            {
                ViewBag.distrito = "[NO SELECCIONADO]";
            }
            return View(listadatogenerales());
        }

        public ActionResult ModificarDatos(string igv, string tipo)
        {
            var sede = Session["CodSede"].ToString();
            var result = LISTA_DISTRITO_DEFAULT(sede);
            if (result.Count > 0)
            {
                ViewBag.distrito = result.FirstOrDefault().NomDist;
                ViewBag.coddist = result.FirstOrDefault().CodDist;
            }
            else
            {
                ViewBag.distrito = "[NO SELECCIONADO]";
            }
            var lista = (from x in listadatogenerales() where x.igv == Convert.ToDecimal(igv) && x.Tipo_Cambio == Convert.ToDecimal(tipo) select x).FirstOrDefault();
            ViewBag.listadistrito = new SelectList(new UtilitarioController().ListadoDistrito("P0128"), "CodDist", "NomDist", ViewBag.coddist);

            return View(lista);
        }

        [HttpPost]
        public ActionResult ModificarDatos(E_Datos_Generales dat)
        {
            var sede = Session["CodSede"].ToString();
            var result = LISTA_DISTRITO_DEFAULT(sede);
            if (result.Count > 0)
            {
                ViewBag.coddist = LISTA_DISTRITO_DEFAULT(sede).FirstOrDefault().CodDist;
            }
            else
            {
                ViewBag.distrito = "[NO SELECCIONADO]";
            }
     
            ViewBag.listadistrito = new SelectList(new UtilitarioController().ListadoDistrito("P0128"), "CodDist", "NomDist", ViewBag.coddist);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("USP_DATOS_GENERALES", con))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@IGV", dat.igv);
                        da.Parameters.AddWithValue("@TIPO_CAMBIO", dat.Tipo_Cambio);
                        da.Parameters.AddWithValue("@MUESTRA_ANTECEDENTE", dat.MUESTRA_ANTECEDENTE);
                        da.Parameters.AddWithValue("@ATENCIONESPAGADAS", dat.ATENCIONESPAGADAS);
                        da.Parameters.AddWithValue("@ATENCIONPROGRAMADAS", dat.PREGXATENCIONPROGRAMADAS);
                        da.Parameters.AddWithValue("@ATENCIONNOPROGRAMADAS", dat.PREGXATENCIONNOPROGRAMADAS);
                        da.Parameters.AddWithValue("@GENERARCUENTAAUTO", dat.GENERARCUENTAAUTO);
                        da.Parameters.AddWithValue("@MOSTRARPACIENTETICKET", dat.MOSTRARPACIENTETICKET);
                        if (da.ExecuteNonQuery() > 0)
                        {
                            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                            {
                                using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_DISTRITO_DEFAULT", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@DISTXDEFECT", dat.CodDist == null ? string.Empty : dat.CodDist);
                                    cmd.Parameters.AddWithValue("@CODSEDE", sede);
                                    cmd.ExecuteNonQuery();

                                }
                            }
                        }
                        else
                        {
                            ViewBag.mensaje = "Error Datos No Validos";
                            return View(dat);
                        }

                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "Error Datos No Validos";
                        return View(dat);
                    }
                    finally { con.Close(); }
                }

            }
            return RedirectToAction("ListarDatosGenerales");
        }



    }
}