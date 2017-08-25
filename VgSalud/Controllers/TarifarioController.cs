using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using VgSalud.Models;
using System.Text;

namespace VgSalud.Controllers
{
    public class TarifarioController : Controller
    {

        EspecialidadController e = new EspecialidadController();
        TipoTarifaController tt = new TipoTarifaController();
        STipoTarifaController st = new STipoTarifaController();
        TipoMonedaController tm = new TipoMonedaController();
        CategoriaPacienteController cat = new CategoriaPacienteController();

        public List<E_Tarifario> ListadoCuentaContable()
        {
            List<E_Tarifario> Lista = new List<E_Tarifario>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaCuentaContable", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Tarifario Etip = new E_Tarifario();

                            Etip.IdCtaCont = dr.GetInt32(0);
                            Etip.codigo = dr.GetString(1);
                            Etip.Descrip = dr.GetString(2);
                            Etip.Estado = dr.GetBoolean(3);
                            Etip.ConcatenadoCuenta = dr.GetString(1) + " " + dr.GetString(2);

                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Tarifario> ListadoPerfilesFichaElectronica()
        {
            List<E_Tarifario> Lista = new List<E_Tarifario>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaPerfilFichaElectronica", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Tarifario Etip = new E_Tarifario();

                            Etip.idPFA = dr.GetInt32(0);
                            Etip.Nombre = dr.GetString(1);
                            Etip.contenido = dr.GetString(2);
                            Etip.CodEspec = dr.GetString(3);
                            Etip.Estado = dr.GetBoolean(4);

                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Tarifario> ListadoPerfilesFichaElectronicaRegTar(string CodEspe, string CodSede)
        {
            List<E_Tarifario> Lista = new List<E_Tarifario>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaPerfilFichaElectronica_RegTarifa", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodEsp", CodEspe);
                    cmd.Parameters.AddWithValue("@CodSede", CodSede);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Tarifario Etip = new E_Tarifario();

                            Etip.idPFA = dr.GetInt32(0);
                            Etip.Nombre = dr.GetString(1);
                            Etip.contenido = dr.GetString(2);
                            Etip.CodEspec = dr.GetString(3);
                            Etip.Estado = dr.GetBoolean(4);

                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Tarifario> ListadoTarifa()
        {
            List<E_Tarifario> Lista = new List<E_Tarifario>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Tarifario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Tarifario Etip = new E_Tarifario();

                            Etip.CodTar = dr.GetString(0);
                            Etip.DescTar = dr.GetString(1);
                            Etip.Precio = dr.GetDecimal(2);
                            Etip.AfecIgcv = dr.GetBoolean(3);
                            Etip.ModPrecTar = dr.GetBoolean(4);
                            Etip.CodEspec = dr.GetString(5);
                            Etip.CodTipTar = dr.GetString(6);
                            Etip.CodSTipTar = dr.GetString(7);
                            Etip.CodTipMon = dr.GetString(8);
                            Etip.CodSede = dr.GetString(9);
                            Etip.ModPrecio = dr.GetBoolean(10);
                            Etip.EstTar = dr.GetBoolean(11);
                            Etip.IdCtaCont = dr.GetInt32(15);
                            Etip.idPFA = dr["idPFA"] is DBNull ? 0 : dr.GetInt32(16);
                            Etip.TiempoApox = dr["TiempoApox"] is DBNull ? 0 : dr.GetInt32(17);
                            Etip.CodTarE = dr.GetString(18);

                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Tarifario> ObtenerTarifario(string id)
        {
            List<E_Tarifario> Lista = new List<E_Tarifario>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_obtenerTarifa", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Tarifario Etip = new E_Tarifario();

                            Etip.CodTar = dr.GetString(0);
                            Etip.DescTar = dr.GetString(1);
                            Etip.Precio = dr.GetDecimal(2);
                            Etip.AfecIgcv = dr.GetBoolean(3);
                            Etip.ModPrecTar = dr.GetBoolean(4);
                            Etip.CodEspec = dr.GetString(5);
                            Etip.CodTipTar = dr.GetString(6);
                            Etip.CodSTipTar = dr.GetString(7);
                            Etip.CodTipMon = dr.GetString(8);
                            Etip.CodSede = dr.GetString(9);
                            Etip.ModPrecio = dr.GetBoolean(10);
                            Etip.EstTar = dr.GetBoolean(11);
                            Etip.IdCtaCont = dr.GetInt32(15);
                            Etip.idPFA = dr["idPFA"] is DBNull ? 0 : dr.GetInt32(16);
                            Etip.TiempoApox = dr["TiempoApox"] is DBNull ? 0 : dr.GetInt32(17);
                            Etip.CodTarE = dr.GetString(18);

                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Tarifa_CategoriaPaciente> ListadoCategoriaPacienteTarifa()
        {
            List<E_Tarifa_CategoriaPaciente> Lista = new List<E_Tarifa_CategoriaPaciente>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listarTarifaCategoriaPaciente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Tarifa_CategoriaPaciente Etip = new E_Tarifa_CategoriaPaciente();

                            Etip.CodTarCate = dr.GetInt32(0);
                            Etip.CodCatPac = dr.GetString(1);
                            Etip.CodTar = dr.GetString(2);
                            Etip.Precio = dr.GetDecimal(3);


                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Tarifa_CategoriaPaciente> ListadoCategoriaPacienteTarifa(string id)
        {
            List<E_Tarifa_CategoriaPaciente> Lista = new List<E_Tarifa_CategoriaPaciente>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listarTarifaCategoriaPaciente_CodTar", con))
                {
                    cmd.Parameters.AddWithValue("@CodTar", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Tarifa_CategoriaPaciente Etip = new E_Tarifa_CategoriaPaciente();

                            Etip.CodTarCate = dr.GetInt32(0);
                            Etip.CodCatPac = dr.GetString(1);
                            Etip.CodTar = dr.GetString(2);
                            Etip.Precio = dr.GetDecimal(3);
                            Etip.DescCatPac = dr.GetString(4);

                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult ListarTarifario()
        {

            string sede = Session["codSede"].ToString();

            return View(ListadoTarifa().Where(x => x.CodSede == sede).ToList());
        }

        public ActionResult RegistrarTarifario(string cadena = null)
        {

            if (cadena != null)
            {
                ViewBag.cadena = cadena;
                //string cadena = t.demo + "," + t.Precio + "," + t.espe + "," + t.tipoTar + "," + t.tipoSubTar + "," + t.TipMoneda;
                string[] Ncadena = cadena.Split(',');


                ViewBag.descripcion = Ncadena[0].ToString();
                ViewBag.costo = Ncadena[1];
                ViewBag.especialidad = Ncadena[2];
                ViewBag.tipoTar = Ncadena[3];
                ViewBag.tipoSubTar = Ncadena[4];
                ViewBag.IdCtaCont = Ncadena[6];
                ViewBag.idPFA = Ncadena[7];
                ViewBag.codcat = Ncadena[8];
                //ViewBag.precio = t.Precio;
                //    @ViewBag.precio
                //    @ViewBag.especialidad
                //    @ViewBag.tipot
                //    @ViewBag.subtarifa


            }
            else
            {
                ViewBag.cadena = "";
                ViewBag.descripcion = "";
                ViewBag.precio = "";
                ViewBag.especialidad = "";
                ViewBag.tipot = "";
                ViewBag.subtarifa = "";
                ViewBag.IdCtaCont = "";
                ViewBag.idPFA = "";

            }
            string sede = Session["codSede"].ToString();

            if (Session["catePaciente"] == null)
            {
                Session["catePaciente"] = new List<E_Tarifa_CategoriaPaciente>();
            }

            SedesController Sed = new SedesController();
            ViewBag.ListaSedes = new SelectList(Sed.ListadoSedes().Where(x => x.EstSede == true).ToList(), "CodSede", "NomSede");

            ViewBag.catePaciente = (List<E_Tarifa_CategoriaPaciente>)Session["catePaciente"];

            ViewBag.listadoPerfilFE = new SelectList(ListadoPerfilesFichaElectronica().Where(x => x.idPFA == 1), "idPFA", "Nombre");
            ViewBag.listadoCuentaContable = new SelectList(ListadoCuentaContable(), "IdCtaCont", "ConcatenadoCuenta");
            ViewBag.listadoEspecialidad = new SelectList(e.ListadoEspecialidades().Where(x => x.EstEspec == true && x.CodSed == sede), "CodEspec", "NomEspec");
            ViewBag.listadoCategoria = new SelectList(cat.listadoCategoriaCliente().Where(d => d.EstCatPac == true), "CodCatPac", "DescCatPac", ViewBag.codcat);
            ViewBag.listadoTipoTarifa = new SelectList(tt.ListadoTipoTarifa().Where(d => d.EstTipTar == true), "CodTipTar", "DescTipTar");
            ViewBag.listadoSubTipoTarifa = new SelectList(st.ListadoSTipoTarifa().Where(d => d.EstTipTar == true), "CodSTipTar", "DescSTipTar");

            return View();
        }

        public ActionResult VistaDemo()
        {


            ViewBag.catePaciente = (List<E_Tarifa_CategoriaPaciente>)Session["catePaciente"];
            return View();
        }
        
        [HttpPost]
        public JsonResult RegistrarTarifario(string descripcion, double costo, int duracion, string especialidad, string tipoTar,
            string subTipTar, int perfil, int cuentaConta, string moneda, int estado, int afecIGV, int regDoc, int precMod, string[] arrayPrecios = null)
        {
            if (arrayPrecios == null)
            {
                var listaCatPre = cat.listadoCategoriaCliente().Where(x => x.EstCatPac == true);
                int contador = 0;
                arrayPrecios = new string[listaCatPre.Count()];
                foreach (var a in listaCatPre)
                {
                    arrayPrecios[contador] = a.CodCatPac + "," + a.DescCatPac + "," + 0;
                    contador++;
                }
            }
            //else
            //{
            //    E_Tarifario t = new E_Tarifario();
            //    List<E_Tarifario> list = new List<E_Tarifario>();
            //    List<E_Tarifario> listGeneral = new List<E_Tarifario>();
            //    foreach (var a in arrayPrecios)
            //    {
            //        E_Tarifario tt = new E_Tarifario();
            //        tt.contenido = a;
            //        listGeneral.Add(tt);
            //        string[] returnData = a.Split(',');
            //        t.CodCatPac = returnData[0].ToString();
            //        list.Add(t);
            //    }
            //    var listaCatPre = from c in cat.listadoCategoriaCliente().Where(x => x.EstCatPac == true) where !(from o in list select o.CodCatPac).Contains(c.CodCatPac) select c;
            //    int contador = arrayPrecios.Length;
            //    foreach (var a in listaCatPre)
            //    {
            //        E_Tarifario tt = new E_Tarifario();
            //        tt.contenido = a.CodCatPac + "," + a.DescCatPac + "," + 0;
            //        listGeneral.Add(tt);
            //    }
            //    arrayPrecios = new string[listGeneral.Count];
            //    int cuenta = 0;
            //    foreach (var a in listGeneral)
            //    {
            //        arrayPrecios[cuenta] = a.contenido;
            //        cuenta++;
            //    }
            //}


            string sede = Session["codSede"].ToString();
            string Crea = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;
            
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                using (SqlCommand da = new SqlCommand("Usp_MtoTarifario", con, tr))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;

                        da.Parameters.AddWithValue("@CodTar", "");
                        da.Parameters.AddWithValue("@DescTar", descripcion.ToUpper());
                        da.Parameters.AddWithValue("@Costo", costo);
                        da.Parameters.AddWithValue("@AfecIgcv", afecIGV);
                        da.Parameters.AddWithValue("@ModPrecTar", regDoc);
                        da.Parameters.AddWithValue("@CodEspec", especialidad);
                        da.Parameters.AddWithValue("@CodTipTar", tipoTar);
                        da.Parameters.AddWithValue("@CodSTipTar", subTipTar);
                        da.Parameters.AddWithValue("@CodTipMon", moneda);
                        da.Parameters.AddWithValue("@CodSede", sede);
                        da.Parameters.AddWithValue("@ModPrecio", precMod);
                        da.Parameters.AddWithValue("@EstTar", estado);
                        da.Parameters.AddWithValue("@IdCtaCont", cuentaConta);
                        da.Parameters.AddWithValue("@idPFA", perfil);
                        da.Parameters.AddWithValue("@TiempoApox", duracion);
                        da.Parameters.AddWithValue("@Crea", Crea);
                        da.Parameters.AddWithValue("@Modifica", "");
                        da.Parameters.AddWithValue("@Elimina", "");
                        da.Parameters.AddWithValue("@Evento", "1");


                        string Resu = (string)da.ExecuteScalar();
                        string Codigo = Convert.ToString(Resu);
                        
                        foreach (var a in arrayPrecios)
                        {
                            string[] returnData = a.Split(',');
                            SqlCommand cmd = new SqlCommand("usp_MtoTarifa_CategoriaPaciente", con, tr);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@CodTarCate", "");
                            cmd.Parameters.AddWithValue("@CodCatPac", returnData[0]);
                            cmd.Parameters.AddWithValue("@CodTar", Codigo);
                            cmd.Parameters.AddWithValue("@Precio", returnData[2]);
                            cmd.Parameters.AddWithValue("@Crea", Crea);
                            cmd.Parameters.AddWithValue("@Modifica", "");
                            cmd.Parameters.AddWithValue("@Elimina", "");
                            cmd.Parameters.AddWithValue("@Evento", "1");
                            cmd.ExecuteNonQuery();

                        }

                        tr.Commit();
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception e)
                    {
                        tr.Rollback();
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                    }
                    finally { con.Close(); }
                }
            }
        }


        public ActionResult AsignarPrecio(E_Tarifa_CategoriaPaciente t)
        {


            var lista = (List<E_Categoria_Paciente>)cat.listadoCategoriaCliente();

            ViewBag.listadoCategoria = new SelectList(cat.listadoCategoriaCliente(), "CodCatPac", "DescCatPac", t.CodCatPac);

            E_Categoria_Paciente reg = cat.listadoCategoriaCliente().Find(x => x.CodCatPac == t.CodCatPac);
            E_TipoMoneda tmo = tm.ListadoTipoMoneda().Where(x => x.CodTipMon == t.TipMoneda).FirstOrDefault();

            var formularios = (List<E_Tarifa_CategoriaPaciente>)Session["catePaciente"];

            bool existe = formularios.Any(x => x.CodCatPac == t.CodCatPac);



            if (formularios.Count() == 0)
            {

                if (t.CodCatPac == null)
                {
                    foreach (var ECT in lista)
                    {
                        if (ECT.EstCatPac)
                        {
                            var formularios1 = (List<E_Tarifa_CategoriaPaciente>)Session["catePaciente"];
                            bool existe1 = formularios1.Any(x => x.CodCatPac == ECT.CodCatPac);
                            if (existe1 == false)
                            {
                                E_Tarifa_CategoriaPaciente item = new E_Tarifa_CategoriaPaciente();

                                item.CodCatPac = ECT.CodCatPac;
                                item.DescCatPac = ECT.DescCatPac;

                                if (tmo.TipoCambio == 0)
                                {
                                    item.Precio = t.Precio;
                                }
                                else
                                {
                                    item.Precio = t.Precio * tmo.TipoCambio;
                                }


                                formularios.Add(item);
                                Session["catePaciente"] = formularios;

                                ViewBag.mensaje = "registro Agregado";

                            }
                        }

                    }

                }
                else
                {
                    if (existe == false)
                    {

                        E_Tarifa_CategoriaPaciente item = new E_Tarifa_CategoriaPaciente();

                        item.CodCatPac = reg.CodCatPac;
                        item.DescCatPac = reg.DescCatPac;

                        if (tmo.TipoCambio == 0)
                        {
                            item.Precio = t.Precio;
                        }
                        else
                        {
                            item.Precio = t.Precio * tmo.TipoCambio;
                        }


                        formularios.Add(item);
                        Session["catePaciente"] = formularios;

                        ViewBag.mensaje = "registro Agregado";

                        //Response.Write("<script language=javascript> history.back(1); </script>");
                        //return null;

                    }

                }

            }
            else
            {
                if (t.CodCatPac == null)
                {
                    foreach (var E in lista)
                    {

                        var formularios1 = (List<E_Tarifa_CategoriaPaciente>)Session["catePaciente"];
                        bool existe1 = formularios1.Any(x => x.CodCatPac == E.CodCatPac);
                        if (existe1 == false)
                        {
                            E_Tarifa_CategoriaPaciente item = new E_Tarifa_CategoriaPaciente();

                            item.CodCatPac = E.CodCatPac;
                            item.DescCatPac = E.DescCatPac;

                            if (tmo.TipoCambio == 0)
                            {
                                item.Precio = t.Precio;
                            }
                            else
                            {
                                item.Precio = t.Precio * tmo.TipoCambio;
                            }

                            formularios.Add(item);
                            Session["catePaciente"] = formularios;

                            ViewBag.mensaje = "registro Agregado";

                        }
                        else
                        {
                            var cat = formularios1.Where(x => x.CodCatPac == E.CodCatPac).FirstOrDefault();
                            formularios1.Remove(cat);
                            Session["catePaciente"] = formularios1;
                            cat.Precio = t.Precio;
                            formularios1.Add(cat);
                            Session["catePaciente"] = formularios1;

                        }
                    }



                }
                else
                {
                    if (existe == true)
                    {
                        var formularios1 = (List<E_Tarifa_CategoriaPaciente>)Session["catePaciente"];
                        var remove = formularios1.Where(x => x.CodCatPac == t.CodCatPac).FirstOrDefault();
                        formularios1.Remove(remove);
                        Session["catePaciente"] = formularios1;
                        if (tmo.TipoCambio == 0)
                        {
                            remove.Precio = t.Precio;
                        }
                        else
                        {
                            remove.Precio = t.Precio * tmo.TipoCambio;
                        }
                        formularios1.Add(remove);

                        Session["catePaciente"] = formularios1;

                        ViewBag.mensaje = "registro Agregado";

                        //Response.Write("<script language=javascript> history.back(1); </script>");
                        //return null;

                    }
                    else
                    {
                        E_Tarifa_CategoriaPaciente item = new E_Tarifa_CategoriaPaciente();

                        item.CodCatPac = reg.CodCatPac;
                        item.DescCatPac = reg.DescCatPac;

                        if (tmo.TipoCambio == 0)
                        {
                            item.Precio = t.Precio;
                        }
                        else
                        {
                            item.Precio = t.Precio * tmo.TipoCambio;
                        }


                        formularios.Add(item);
                        Session["catePaciente"] = formularios;

                        ViewBag.mensaje = "registro Agregado";
                    }

                }

            }


            string cadena = t.demo + "," + t.costo + "," + t.espe + "," + t.tipoTar + "," + t.tipoSubTar + "," + t.TipMoneda + "," + t.IdCtaCont + "," + t.idPFA + "," + t.CodCatPac;
            return RedirectPermanent("RegistrarTarifario?cadena=" + cadena);

        }

        public ActionResult Delete(string id)
        {

            var formularios = (List<E_Tarifa_CategoriaPaciente>)Session["catePaciente"];
            var registro = formularios.Where(x => x.CodCatPac.Equals(id)).FirstOrDefault();
            formularios.Remove(registro);

            Session["catePaciente"] = formularios;

            registro.Precio = 0;
            formularios.Add(registro);
            Session["catePaciente"] = formularios;

            Response.Write("<script language=javascript> history.back(1); </script>");

            return null;
        }


        public ActionResult Eliminar(string id)
        {

            string eliminar = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_MtoTarifario", con))
                {
                    try
                    {

                        da.CommandType = CommandType.StoredProcedure;

                        da.Parameters.AddWithValue("@CodTar", id);
                        da.Parameters.AddWithValue("@DescTar", "");
                        da.Parameters.AddWithValue("@Costo", 0);
                        da.Parameters.AddWithValue("@AfecIgcv", "");
                        da.Parameters.AddWithValue("@ModPrecTar", "");
                        da.Parameters.AddWithValue("@CodEspec", "");
                        da.Parameters.AddWithValue("@CodTipTar", "");
                        da.Parameters.AddWithValue("@CodSTipTar", "");
                        da.Parameters.AddWithValue("@CodTipMon", "");
                        da.Parameters.AddWithValue("@CodSede", "");
                        da.Parameters.AddWithValue("@ModPrecio", "");
                        da.Parameters.AddWithValue("@IdCtaCont", "");
                        da.Parameters.AddWithValue("@EstTar", "");
                        da.Parameters.AddWithValue("@idPFA", "");
                        da.Parameters.AddWithValue("@TiempoApox", "");
                        da.Parameters.AddWithValue("@Crea", "");
                        da.Parameters.AddWithValue("@Modifica", "");
                        da.Parameters.AddWithValue("@Elimina", eliminar);
                        da.Parameters.AddWithValue("@Evento", 3);

                        da.ExecuteNonQuery();
                        ViewBag.mensaje = "Se elimino correctamente";
                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "Error no se pudo eliminar";
                    }
                    finally { con.Close(); }
                }
                return RedirectToAction("ListarTarifario");

            }
        }



        public ActionResult Activar(string id)
        {

            string modifica = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("Usp_MtoTarifario", con))
                {
                    try
                    {

                        da.CommandType = CommandType.StoredProcedure;

                        da.Parameters.AddWithValue("@CodTar", id);
                        da.Parameters.AddWithValue("@DescTar", "");
                        da.Parameters.AddWithValue("@Costo", 0);
                        da.Parameters.AddWithValue("@AfecIgcv", "");
                        da.Parameters.AddWithValue("@ModPrecTar", "");
                        da.Parameters.AddWithValue("@CodEspec", "");
                        da.Parameters.AddWithValue("@CodTipTar", "");
                        da.Parameters.AddWithValue("@CodSTipTar", "");
                        da.Parameters.AddWithValue("@CodTipMon", "");
                        da.Parameters.AddWithValue("@CodSede", "");
                        da.Parameters.AddWithValue("@ModPrecio", "");
                        da.Parameters.AddWithValue("@IdCtaCont", "");

                        da.Parameters.AddWithValue("@idPFA", "");
                        da.Parameters.AddWithValue("@TiempoApox", 0);
                        da.Parameters.AddWithValue("@EstTar", "");
                        da.Parameters.AddWithValue("@Crea", "");
                        da.Parameters.AddWithValue("@Modifica", modifica);
                        da.Parameters.AddWithValue("@Elimina", "");
                        da.Parameters.AddWithValue("@Evento", 4);

                        da.ExecuteNonQuery();
                        ViewBag.mensaje = "Se elimino correctamente";
                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "Error no se pudo eliminar";
                    }
                    finally { con.Close(); }
                }
                return RedirectToAction("ListarTarifario");

            }
        }


        public ActionResult ObtenerTipoTarifa(string CodEspec)
        {
            var evalua = (List<E_Tipo_Tarifa>)(from a in tt.ListadoTipoTarifa() where a.CodEspec == CodEspec && a.EstTipTar == true select a).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult ObtenerPerfilFichaElectronica(string CodEspec)
        {
            string CodSede = Session["codSede"].ToString();
            var evalua = (List<E_Tarifario>)(from a in ListadoPerfilesFichaElectronicaRegTar(CodEspec, CodSede) select a).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult ObtenerSubTipoTarifa(string TipoTar)
        {
            var evalua = (List<E_Sub_Tipo_Tarifa>)(from a in st.ListadoSTipoTarifa() where a.CodTipTar == TipoTar && a.EstTipTar == true select a).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
        }


        public ActionResult ModificarTarifa(string id, string var = null)
        {
            var traeData = ObtenerTarifario(id).FirstOrDefault();

            string sede = Session["codSede"].ToString();

            if (Session["catePacienteM"] == null)
            {
                Session["catePacienteM"] = new List<E_Tarifa_CategoriaPaciente>();
            }
            SedesController Sed = new SedesController();
            ViewBag.ListaSedes = new SelectList(Sed.ListadoSedes(), "CodSede", "NomSede");

            ViewBag.catePaciente = (List<E_Tarifa_CategoriaPaciente>)Session["catePacienteM"];
            ViewBag.listadocatePaciente = (List<E_Tarifa_CategoriaPaciente>)ListadoCategoriaPacienteTarifa(id);
            ViewBag.listadoEspecialidad = new SelectList(e.ListadoEspecialidades().Where(x => x.EstEspec == true && x.CodSed == sede), "CodEspec", "NomEspec");
            ViewBag.listadoCategoria = new SelectList(cat.listadoCategoriaCliente().Where(d => d.EstCatPac == true), "CodCatPac", "DescCatPac",var);
            ViewBag.listadoTipoTarifa = new SelectList(tt.ListadoTipoTarifa().Where(d => d.EstTipTar == true), "CodTipTar", "DescTipTar");
            ViewBag.listadoSubTipoTarifa = new SelectList(st.ListadoSTipoTarifa().Where(d => d.EstTipTar == true), "CodSTipTar", "DescSTipTar");
            ViewBag.listadoPerfilFE = new SelectList(ListadoPerfilesFichaElectronica().Where(x => x.idPFA == 1), "idPFA", "Nombre", traeData.idPFA);
            ViewBag.listadoCuentaContable = new SelectList(ListadoCuentaContable(), "IdCtaCont", "ConcatenadoCuenta");

            var lista = (from x in ListadoTarifa() where x.CodTar == id select x).FirstOrDefault();

            ViewBag.idPFA = lista.idPFA;

            return View(lista);
        }

        [HttpPost]
        public ActionResult ModificarTarifa(E_Tarifario tar)
        {
            string sede = Session["codSede"].ToString();
            ViewBag.listadocatePaciente = (List<E_Tarifa_CategoriaPaciente>)ListadoCategoriaPacienteTarifa(tar.CodTar);
            ViewBag.listadoEspecialidad = new SelectList(e.ListadoEspecialidades().Where(x => x.EstEspec == true && x.CodSed == sede), "CodEspec", "NomEspec", tar.CodEspec);
            ViewBag.listadoCategoria = new SelectList(cat.listadoCategoriaCliente().Where(d => d.EstCatPac == true), "CodCatPac", "DescCatPac",tar.CodCatPac);
            ViewBag.listadoPerfilFE = new SelectList(ListadoPerfilesFichaElectronica(), "idPFA", "Nombre", tar.idPFA);
            ViewBag.listadoTipoTarifa = new SelectList(tt.ListadoTipoTarifa().Where(d => d.EstTipTar == true), "CodTipTar", "DescTipTar");
            ViewBag.listadoSubTipoTarifa = new SelectList(st.ListadoSTipoTarifa().Where(d => d.EstTipTar == true), "CodSTipTar", "DescSTipTar");
            ViewBag.listadoCuentaContable = new SelectList(ListadoCuentaContable(), "IdCtaCont", "ConcatenadoCuenta", tar.IdCtaCont);
           
            string modifica = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            SedesController Sed = new SedesController();
            ViewBag.ListaSedes = new SelectList(Sed.ListadoSedes(), "CodSede", "NomSede", tar.CodSede);

            var evaluaF = (List<E_Categoria_Paciente>)cat.listadoCategoriaCliente().ToList();
            var evaluaSession = (List<E_Tarifa_CategoriaPaciente>)Session["catePacienteM"];
            var categoriaPac = new CategoriaPacienteController().listadoCategoriaCliente();
            if (tar.Evento == "1")
            {

                if (tar.CodCatPac == null)
                {
                    foreach (var item in ListadoCategoriaPacienteTarifa(tar.CodTar))
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                        {
                            con.Open();
                            using (SqlCommand da = new SqlCommand("usp_MtoTarifa_CategoriaPaciente", con))
                            {
                                da.CommandType = CommandType.StoredProcedure;
                                da.Parameters.AddWithValue("@CodTarCate", item.CodTarCate);
                                da.Parameters.AddWithValue("@CodCatPac", "");
                                da.Parameters.AddWithValue("@CodTar", "");
                                da.Parameters.AddWithValue("@Precio", tar.PrecioD);
                                da.Parameters.AddWithValue("@Crea", "");
                                da.Parameters.AddWithValue("@Modifica", "");
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", 3);
                                da.ExecuteNonQuery();
                            }
                        }
                    }
                    return RedirectPermanent("~/Tarifario/ModificarTarifa?id=" + tar.CodTar + "&var=" + tar.CodCatPac);
                }



                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_MtoTarifa_CategoriaPaciente", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodTarCate", "");
                    cmd.Parameters.AddWithValue("@CodCatPac", tar.CodCatPac);
                    cmd.Parameters.AddWithValue("@CodTar", tar.CodTar);
                    cmd.Parameters.AddWithValue("@Precio", tar.PrecioD);
                    cmd.Parameters.AddWithValue("@Crea", modifica);
                    cmd.Parameters.AddWithValue("@Modifica", "");
                    cmd.Parameters.AddWithValue("@Elimina", "");
                    cmd.Parameters.AddWithValue("@Evento", "1");
                    cmd.ExecuteNonQuery();
                }
                ViewBag.listadocatePaciente = (List<E_Tarifa_CategoriaPaciente>)ListadoCategoriaPacienteTarifa(tar.CodTar);
                return View(tar);

            }



            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                using (SqlCommand da = new SqlCommand("Usp_MtoTarifario", con, tr))
                {
                    try
                    {
                        da.CommandType = CommandType.StoredProcedure;

                        da.Parameters.AddWithValue("@CodTar", tar.CodTar);
                        da.Parameters.AddWithValue("@DescTar", tar.DescTar.ToUpper());
                        da.Parameters.AddWithValue("@Costo", tar.Precio);
                        da.Parameters.AddWithValue("@AfecIgcv", tar.AfecIgcv);
                        da.Parameters.AddWithValue("@ModPrecTar", tar.ModPrecTar);
                        da.Parameters.AddWithValue("@CodEspec", tar.CodEspec);
                        da.Parameters.AddWithValue("@CodTipTar", tar.CodTipTar);
                        if (tar.CodSTipTar == null)
                        {
                            da.Parameters.AddWithValue("@CodSTipTar", "     ");
                        }
                        else
                        {
                            da.Parameters.AddWithValue("@CodSTipTar", tar.CodSTipTar);
                        }
                        da.Parameters.AddWithValue("@CodTipMon", tar.CodTipMon);
                        da.Parameters.AddWithValue("@CodSede", sede);
                        da.Parameters.AddWithValue("@ModPrecio", tar.ModPrecio);
                        da.Parameters.AddWithValue("@EstTar", tar.EstTar);
                        da.Parameters.AddWithValue("@IdCtaCont", tar.IdCtaCont);
                        da.Parameters.AddWithValue("@idPFA", tar.idPFA);
                        da.Parameters.AddWithValue("@TiempoApox", tar.TiempoApox);
                        da.Parameters.AddWithValue("@Crea", "");
                        da.Parameters.AddWithValue("@Modifica", modifica);
                        da.Parameters.AddWithValue("@Elimina", "");
                        da.Parameters.AddWithValue("@Evento", "2");

                        da.ExecuteNonQuery();


                        tr.Commit();
                        ViewBag.mensaje = "Pedido registrado";

                        return RedirectToAction("ListarTarifario");

                    }
                    catch (Exception e)
                    {
                        tr.Rollback();
                        return RedirectToAction("ListarTarifario");
                    }
                    finally
                    {
                        con.Close();
                        Session.Remove("catePacienteM");
                    }
                }

            }
        }


        public ActionResult EliminaDetalle(int id, string codForm)
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
            {
                con.Open();
                using (SqlCommand da = new SqlCommand("usp_MtoTarifa_CategoriaPaciente", con))
                {
                    da.CommandType = CommandType.StoredProcedure;
                    da.Parameters.AddWithValue("@CodTarCate", id);
                    da.Parameters.AddWithValue("@CodCatPac", "");
                    da.Parameters.AddWithValue("@CodTar", "");
                    da.Parameters.AddWithValue("@Precio", 0);
                    da.Parameters.AddWithValue("@Crea", "");
                    da.Parameters.AddWithValue("@Modifica", "");
                    da.Parameters.AddWithValue("@Elimina", "");
                    da.Parameters.AddWithValue("@Evento", 2);
                    try
                    {
                        da.ExecuteNonQuery();
                        ViewBag.mensaje = "Se registro correctamente";
                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "Error no se pudo registrar";
                    }
                    finally { con.Close(); }
                }
                return RedirectToAction("ModificarTarifa/" + codForm);
            }
        }
        public ActionResult AsignarPrecioM(E_Tarifa_CategoriaPaciente t)
        {
            ViewBag.listadoCategoria = new SelectList(cat.listadoCategoriaCliente(), "CodCatPac", "DescCatPac", t.CodCatPac);

            E_Categoria_Paciente reg = cat.listadoCategoriaCliente().Find(x => x.CodCatPac == t.CodCatPac);
            E_TipoMoneda tmo = tm.ListadoTipoMoneda().Where(x => x.CodTipMon == t.TipMoneda).FirstOrDefault();

            return RedirectToAction("ModificarTarifa/" + t.CodTar);
        }

        public ActionResult DeleteM(string id, string codigo)
        {

            var formularios = (List<E_Tarifa_CategoriaPaciente>)Session["catePacienteM"];
            var registro = formularios.Where(x => x.CodCatPac.Equals(id)).FirstOrDefault();
            formularios.Remove(registro);

            Session["catePacienteM"] = formularios;

            return RedirectToAction("ModificarTarifa/" + codigo);
        }

        public ActionResult GetCategoria(string codtar)
        {

            var catpac = ListadoCategoriaPacienteTarifa(codtar).ToList();

            string table = "";
            foreach (var item in catpac)
            {
                table += "<tr style='height:5px;width:15px'><td style='height:5px;width:15px'>" + item.DescCatPac + "</td>";
                table += "<td style='height:5px;width:15px'>" + item.Precio + "</td></tr>";
            }

            if (!string.IsNullOrWhiteSpace(table))
            {
                return Json(table, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public JsonResult ListaCategoriaPacientes(double precio)
        {
            var listado = cat.listadoCategoriaCliente().Where(d => d.EstCatPac == true).ToList();
            
            return Json(new { listado, success = true }, JsonRequestBehavior.AllowGet);

        }


    }
}