using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using VgSalud.Models;
using System.IO;

namespace VgSalud.Controllers
{
    public class GastosController : Controller
    {
        public List<E_Gastos> Usp_Lista_Acuenta(int IdGastos)
        {
            List<E_Gastos> Lista = new List<E_Gastos>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Acuenta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idgastosC", IdGastos);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Gastos cc = new E_Gastos();
                            cc.IdgastosC = dr.GetInt32(1);
                            cc.FechaPago = dr.GetDateTime(4).ToShortDateString();
                            cc.TipoPago = dr.GetString(5);
                            cc.Monto = dr.GetDecimal(6);
                            cc.Observ = dr.GetString(7);
                            Lista.Add(cc);
                        }
                        con.Close();
                    }
                }
                return Lista;
            }
        }
        public E_Gastos Lista_Gastos_Cabecera_XID(int IdGastos)
        {
            E_Gastos cc = null;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Lista_Gastos_Cabecera_XID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idgastosC", IdGastos);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cc = new E_Gastos();
                            cc.IdgastosC = dr.GetInt32(0);
                            cc.NroOperacion = dr.GetString(1);
                            cc.RazonS = dr.GetString(2);
                            cc.Ruc = dr.GetString(3);
                            cc.Direccion = dr.GetString(4);
                            cc.TipoPersona = dr.GetString(5);
                            cc.Observ = dr.GetString(6);
                            cc.Titulo = dr.GetString(7);
                            cc.DescTipMon = dr.GetString(8);
                            cc.TipoCambio = dr.GetDecimal(9);
                            cc.FechaProxPago = dr.GetDateTime(10);
                            cc.DescripcionTipoDocumento = dr.GetString(11);
                            cc.Serie = dr.GetString(12);
                            cc.Documento = dr.GetString(13);
                            cc.TipoPago = dr.GetString(14);
                            cc.FechaEmision = dr.GetDateTime(15);
                            cc.FechaPago = dr.GetDateTime(16).ToShortDateString();
                            cc.glosa = dr.GetString(17);
                            cc.Observ = dr.GetString(18);
                            cc.NroDetraccion = dr.GetString(19);
                            cc.FechaDetraccion = dr.GetDateTime(20).ToShortDateString();
                            cc.igv = dr.GetDecimal(21);
                            cc.Retencion = dr.GetDecimal(22);
                            cc.Detraccion = dr.GetDecimal(23);
                            cc.Total = dr.GetDecimal(24);
                            cc.DescripcionCentroCosto = dr.GetString(25);
                            cc.DescripcionSubCentroCosto = dr.GetString(26);
                            cc.Renta = dr.GetDecimal(27);

                        }
                        con.Close();
                    }

                }
                return cc;
            }
        }
        public List<E_Gastos> Lista_Gastos_Detalle_XID(int IdGastos)
        {
            List<E_Gastos> Lista = new List<E_Gastos>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Lista_Gastos_Detalle_XID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idgastosC", IdGastos);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Gastos cc = new E_Gastos();
                            cc.IdCtaCont = Convert.ToInt32(dr["CtaContable"].ToString());
                            cc.DescripcionCtaContable = dr.GetString(1);
                            cc.Item = dr.GetInt32(2);
                            cc.PrecioU = dr.GetDecimal(3);
                            cc.Cant = dr.GetInt32(4);
                            cc.Total = dr.GetDecimal(5);
                            Lista.Add(cc);

                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public ActionResult DetalleGastos(int id = 0)
        {
            E_Gastos gastos = Lista_Gastos_Cabecera_XID(id);
            ViewBag.detalle = Lista_Gastos_Detalle_XID(id);
            var lista = Usp_Lista_Acuenta(id);
            decimal total =0; 
            foreach (var item in lista) {
                total += item.Monto;
            }
            ViewBag.totalAcuenta = total;
            ViewBag.saldo = gastos.Total - total; 
            return View(gastos);
        }

        [HttpPost]
        public ActionResult DetalleGastos(E_Gastos egas)
        {
            if (egas.Evento == "1")
            {
                if (egas.foto == null || egas.foto.ContentLength == 0)
                {
                    ViewBag.mensaje = "Selecciona la foto";
                    return View(egas);
                }
                //validar que sea jpg
                if (egas.foto.ContentType != "image/jpeg")
                {
                    ViewBag.mensaje = "Formato Jpg";
                    return View(egas);
                }

                //definir la direccion de la carpeta imagenes
                var carpeta = "~/imagenes";
                var imagendir = Path.Combine(Server.MapPath(carpeta), egas.foto.FileName);
                var imagenurl = Path.Combine(carpeta, egas.foto.FileName);

                //almaceno la imagen utilizando la direccion absoluta
                egas.foto.SaveAs(imagendir);

                //almacenar el registro a la base de datos
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    SqlCommand cmd =
                new SqlCommand("Mantenimiento_Gastos_Imagenes", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdgastosC", 1);
                    cmd.Parameters.AddWithValue("@IdImagen", "");
                    cmd.Parameters.AddWithValue("@Ruta", imagenurl);
                    cmd.Parameters.AddWithValue("@Item", "");
                    cmd.Parameters.AddWithValue("@Evento", 1); //direccion relativa
                    con.Open();
                    try { cmd.ExecuteNonQuery(); ViewBag.mensaje = "Agregado"; }
                    catch (SqlException ex) { ViewBag.mensaje = ex.Message; }
                    finally { con.Close(); }
                    return View(egas);
                }
            }
            return View();
        }


        public string Generador_De_Codigo()
        {
            string codigo = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {

                try
                {
                    SqlCommand cmd = new SqlCommand("Generar_NroOperacion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NroOperacion", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    codigo = cmd.Parameters["@NroOperacion"].Value.ToString();
                }
                catch (Exception ex)
                {
                    ViewBag.mensaje = "error" + ex.Message;
                }


            }

            return codigo;
        }
        public List<E_Gastos> ListaTipoDocumento()
        {
            List<E_Gastos> Lista = new List<E_Gastos>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_General_TipoDocumento", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Gastos cc = new E_Gastos();

                            cc.CodigoTipoDocumento = dr.GetInt32(0);
                            cc.DescripcionTipoDocumento = dr.GetString(1).ToUpper();
                            cc.EstadoCentroCosto = dr.GetBoolean(2);
                            Lista.Add(cc);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public List<E_Gastos> ListaCentroCostos()
        {
            List<E_Gastos> Lista = new List<E_Gastos>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_General_Centro_Costos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Gastos cc = new E_Gastos();

                            cc.IdCentroCosto = dr.GetString(0);
                            cc.DescripcionCentroCosto = dr.GetString(1);
                            cc.EstadoCentroCosto = dr.GetBoolean(2);
                            Lista.Add(cc);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public List<E_Gastos> ListaSubCentroCostos()
        {
            List<E_Gastos> Lista = new List<E_Gastos>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_General_Sub_Centro_Costos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Gastos cc = new E_Gastos();

                            cc.IdSubCentroCosto = dr.GetString(0);
                            cc.DescripcionSubCentroCosto = dr.GetString(1);
                            cc.IdCentroCosto = dr.GetString(2);
                            cc.EstadoSubCentroCosto = dr.GetBoolean(3);
                            Lista.Add(cc);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public List<E_Gastos> ListaProveedor()
        {
            List<E_Gastos> Lista = new List<E_Gastos>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_General_Proveedor", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Gastos pro = new E_Gastos();

                            pro.IdPro = dr.GetInt32(0);
                            pro.RazonS = dr.GetString(1);
                            pro.Ruc = dr.GetString(2);
                            pro.Direc = dr.GetString(3);
                            pro.Observ = dr.GetString(7);
                            pro.TipoPersona = dr.GetString(9);
                            Lista.Add(pro);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public List<E_Gastos> ListaBienesServicio()
        {
            List<E_Gastos> Lista = new List<E_Gastos>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_General_Bienes_Servicio", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Gastos Bs = new E_Gastos();
                            Bs.IDbs = dr.GetInt32(0);
                            Bs.Porcentaje = dr.GetInt32(1);
                            Bs.Titulo = dr.GetString(2);
                            Lista.Add(Bs);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public List<E_Gastos> Lista_CuentaContable()
        {

            List<E_Gastos> Lista = new List<E_Gastos>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_General_CuentaContable", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            E_Gastos CtaContable = new E_Gastos();
                            CtaContable.IDctaContable = dr.GetInt32(0);
                            CtaContable.CodigoContable = dr.GetString(1);
                            CtaContable.DescripcionCtaContable = dr.GetString(2);
                            CtaContable.EstadoCtaContable = dr.GetBoolean(3);
                            Lista.Add(CtaContable);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }

        }

        public ActionResult RegistrarGastos()

        {
            ViewBag.NroOperacion = Generador_De_Codigo();
            Session["acuenta"] = null;
            Session["gastos"] = null;
            ViewBag.modal = "";
            ViewBag.tipodocumento = new SelectList(ListaTipoDocumento(), "CodigoTipoDocumento", "DescripcionTipoDocumento");
            ViewBag.centrocosto = new SelectList(ListaCentroCostos(), "IdCentroCosto", "DescripcionCentroCosto");
            ViewBag.ctacontable = new SelectList(Lista_CuentaContable(), "CodigoContable", "DescripcionCtaContable");
            ViewBag.BienesServicio = new SelectList(ListaBienesServicio(), "IDbs", "Titulo");
            ViewBag.proveedor = new SelectList(ListaProveedor(), "IdPro", "RazonS");
            UtilitarioController u = new UtilitarioController();
            E_Master m = u.ListadoHoraServidor().FirstOrDefault();
            ViewBag.FechaActual = m.HoraServidor.ToString().Substring(0, 10);
            TipoMonedaController tip = new TipoMonedaController();
            ViewBag.tipomoneda = new SelectList(tip.ListadoTipoMoneda().Where(x => x.EstTipMon == true).ToList(), "CodTipMon", "DescTipMon");
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarGastos(E_Gastos Egas)
        {

            ViewBag.NroOperacion = Generador_De_Codigo();
            if (Session["gastos"] != null) { ViewBag.gastos = (List<E_Gastos>)Session["gastos"]; };
            if (Session["acuenta"] != null) { ViewBag.listaAcuenta = (List<E_Gastos>)Session["acuenta"]; };

            if (Egas.Observ != null) { ViewBag.observacion = Egas.Observ; } else { ViewBag.observacion = ""; }
            if (Egas.IdPro != 0) { ViewBag.pro = Egas.IdPro; }
            if (Egas.TipoPago != null) { ViewBag.TipoPago = Egas.TipoPago; }
            if (Egas.CodigoTipoDocumento != 0) { ViewBag.TipoDoc = Egas.CodigoTipoDocumento; }
            if (Egas.IDbs != 0) { ViewBag.IDbs = Egas.IDbs; }
            if (Egas.CodTipMon != null) { ViewBag.CodTipMon = Egas.CodTipMon; }
            if (Egas.glosa != null) { ViewBag.glosa = Egas.glosa; }
            if (Egas.ObservacionC != null) { ViewBag.observacionC = Egas.ObservacionC; }
            if (Egas.IdCentroCosto != null) { ViewBag.IDcentrocosto = Egas.IdCentroCosto; }
            if (Egas.IdSubCentroCosto != null) { ViewBag.IDSubCentrocosto = Egas.IdSubCentroCosto; }

            ViewBag.importe = Egas.Total; ViewBag.igv = Egas.igvCS; ViewBag.retencion = Egas.Retencion; ViewBag.detraccion = Egas.Detraccion; ViewBag.total = Egas.TotalNeto;
            ViewBag.totalbruto = Egas.TotalNeto;
            ViewBag.modal = "";
            decimal montototal = 0;
            ViewBag.tipodocumento = new SelectList(ListaTipoDocumento(), "CodigoTipoDocumento", "DescripcionTipoDocumento");
            ViewBag.ctacontable = new SelectList(Lista_CuentaContable(), "CodigoContable", "DescripcionCtaContable");
            ViewBag.centrocosto = new SelectList(ListaCentroCostos(), "IdCentroCosto", "DescripcionCentroCosto", Egas.IdCentroCosto);
            ViewBag.BienesServicio = new SelectList(ListaBienesServicio(), "IDbs", "Titulo");
            ViewBag.proveedor = new SelectList(ListaProveedor(), "IdPro", "RazonS");
            TipoMonedaController tip = new TipoMonedaController();
            ViewBag.tipomoneda = new SelectList(tip.ListadoTipoMoneda().Where(x => x.EstTipMon == true).ToList(), "CodTipMon", "DescTipMon");

            try
            {

                if (Egas.Evento == "1")
                {
                    UtilitarioController ut = new UtilitarioController();
                    var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
                    var proveedor = ListaProveedor().Where(x => x.IdPro == Egas.IdPro).FirstOrDefault();
                    if (proveedor != null) Egas.RazonS = proveedor.RazonS;
                    string crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                        using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Gastos_Cabecera", con, tr))
                        {
                            try
                            {

                                da.CommandType = CommandType.StoredProcedure;
                                da.Parameters.AddWithValue("@IdgastosC", 0);
                                da.Parameters.AddWithValue("@NroOperacion", Egas.NroOperacion);
                                da.Parameters.AddWithValue("@IdPro", Egas.IdPro);
                                da.Parameters.AddWithValue("@Idcc", Egas.IdCentroCosto);
                                da.Parameters.AddWithValue("@IdScc", Egas.IdSubCentroCosto);
                                da.Parameters.AddWithValue("@RazonS", Egas.RazonS);
                                da.Parameters.AddWithValue("@Ruc", Egas.Ruc);
                                da.Parameters.AddWithValue("@Direccion", proveedor.Direc);
                                da.Parameters.AddWithValue("@TipoPago", Egas.TipoPagoC);
                                da.Parameters.AddWithValue("@Total", Egas.TotalNeto);
                                da.Parameters.AddWithValue("@Igv", Egas.igvCS);
                                da.Parameters.AddWithValue("@Renta", Egas.Renta);
                                da.Parameters.AddWithValue("@NroDetraccion", Egas.NroDetraccion);
                                da.Parameters.AddWithValue("@FechaDetraccion", Egas.FechaDetraccion);
                                da.Parameters.AddWithValue("@Detraccion", Egas.Detraccion);
                                da.Parameters.AddWithValue("@FechaEmision", Egas.FechaEmision);
                                da.Parameters.AddWithValue("@FechaProxPago", Egas.FechaProxPago);
                                da.Parameters.AddWithValue("@FechaPago", Egas.FechaPago);
                                da.Parameters.AddWithValue("@FechaReg", ut.ListadoHoraServidor().FirstOrDefault().HoraServidor);
                                da.Parameters.AddWithValue("@TipoVenta", Egas.IDbs);
                                da.Parameters.AddWithValue("@Documento", Egas.Documento);
                                da.Parameters.AddWithValue("@Serie", Egas.Serie);
                                da.Parameters.AddWithValue("@codigo", Egas.CodigoTipoDocumento);
                                da.Parameters.AddWithValue("@TipoPersona", Egas.TipoPersona);
                                if (Egas.glosa == null) { da.Parameters.AddWithValue("@glosa", ""); } else { da.Parameters.AddWithValue("@glosa", Egas.glosa); }
                                da.Parameters.AddWithValue("@TipoCompra", Convert.ToString(Egas.CodigoTipoDocumento));
                                da.Parameters.AddWithValue("@CodTipMon", Egas.CodTipMon);
                                da.Parameters.AddWithValue("@TipoCambio", Egas.TipoCambio);
                                da.Parameters.AddWithValue("@Retencion", Egas.Retencion);
                                if (Egas.ObservacionC == null) { da.Parameters.AddWithValue("@Observ", ""); } else { da.Parameters.AddWithValue("@Observ", Egas.ObservacionC); }
                                da.Parameters.AddWithValue("@Crea", crea);
                                da.Parameters.AddWithValue("@Modifica", "");
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Estado", 1);
                                da.Parameters.AddWithValue("@Evento", 1);

                                Egas.IdgastosC = int.Parse(da.ExecuteScalar().ToString());

                                var formulario = (List<E_Gastos>)Session["gastos"];
                                foreach (var item in formulario)
                                {
                                    using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_Detalle", con, tr))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@IdgastosC", Egas.IdgastosC);
                                        cmd.Parameters.AddWithValue("@CtaContable", item.CodigoContable);
                                        cmd.Parameters.AddWithValue("@Item", item.ItemG);
                                        cmd.Parameters.AddWithValue("@PrecioU", item.PrecioU);
                                        cmd.Parameters.AddWithValue("@Cant", item.Cant);
                                        cmd.Parameters.AddWithValue("@importe", item.Total);

                                        cmd.Parameters.AddWithValue("@Estado", "");
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                if (Session["acuenta"] != null)
                                {
                                    var formulario1 = (List<E_Gastos>)Session["acuenta"];
                                    if (formulario1 != null || formulario1.Count() != 0)
                                    {
                                        foreach (var item in formulario1)
                                        {
                                            using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_Acuenta", con, tr))
                                            {

                                                cmd.CommandType = CommandType.StoredProcedure;

                                                cmd.Parameters.AddWithValue("@Idgasto", "");
                                                cmd.Parameters.AddWithValue("@IdgastosC", Egas.IdgastosC);
                                                cmd.Parameters.AddWithValue("@CodTipMon", item.CodTipMon);
                                                cmd.Parameters.AddWithValue("@TipoCambio", item.TipoCambio);
                                                cmd.Parameters.AddWithValue("@Fecha", item.FechaPago);
                                                cmd.Parameters.AddWithValue("@TipoPago", item.TipoPago);
                                                cmd.Parameters.AddWithValue("@Monto", item.Monto);
                                                cmd.Parameters.AddWithValue("@Observ", item.Observ);
                                                cmd.Parameters.AddWithValue("@Estado", "");
                                                cmd.ExecuteNonQuery();
                                                ViewBag.pro = Egas.IdPro.ToString();
                                            }
                                        }
                                    }
                                }

                                tr.Commit();
                                ViewBag.mensaje = "Se registro con exito";

                            }
                            catch (Exception e)
                            {
                                tr.Rollback();
                                ViewBag.mensaje = "Error: " + e.Message;
                                return View(Egas);

                            }
                            finally { con.Close(); }
                        }

                    }



                }
                else if (Egas.Evento == "2") { }
                else if (Egas.Evento == "3")
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Proveedor", con))
                        {
                            try
                            {
                                da.CommandType = CommandType.StoredProcedure;

                                da.Parameters.AddWithValue("@IdPro", "");
                                da.Parameters.AddWithValue("@Razon", Egas.RazonS == null ? Egas.RazonS = "" : Egas.RazonS.ToUpper());
                                da.Parameters.AddWithValue("@Ruc", Egas.Ruc);
                                da.Parameters.AddWithValue("@Direc", Egas.Direccion == null ? Egas.Direccion = "" : Egas.Direccion.ToUpper());
                                da.Parameters.AddWithValue("@Contacto", Egas.Contacto.ToUpper());
                                da.Parameters.AddWithValue("@Telef", Egas.Telef);
                                da.Parameters.AddWithValue("@Correo", Egas.Correo == null ? Egas.Correo = "" : Egas.Correo.ToUpper());
                                da.Parameters.AddWithValue("@Observ",Egas.Observ == null ? Egas.Observ = "" : Egas.Observ.ToUpper());
                                da.Parameters.AddWithValue("@Rubro", Egas.Rubro.ToUpper());
                                da.Parameters.AddWithValue("@TipoPersona", Egas.TipoPersona == null ? Egas.TipoPersona = "" : Egas.TipoPersona.ToUpper());
                                da.Parameters.AddWithValue("@Estado", "");
                                da.Parameters.AddWithValue("@Evento", "1");


                                Egas.IdPro = int.Parse(da.ExecuteScalar().ToString());
                                ViewBag.pro = Egas.IdPro.ToString();
                            }
                            catch (Exception e)
                            {
                                ViewBag.mensaje = "Error: Datos No Validos";

                            }
                            finally { con.Close(); }
                        }
                    }
                    ViewBag.proveedor = new SelectList(ListaProveedor(), "IdPro", "RazonS");
                    return View(Egas);
                }


                else if (Egas.Evento == "4")
                {
                    if (Egas.CodigoTipoDocumento == 0) { Egas.CodigoTipoDocumento = 0; };
                    if (Egas.CodigoTipoDocumento == 2)
                    {

                        if (Egas.Serie != null) { ViewBag.serie = Egas.Serie; } else { ViewBag.serie = ""; }
                    }
                    else
                    {

                        if (Egas.Serie != null) { ViewBag.serie = Egas.Serie; } else { ViewBag.serie = ""; }
                    }
                    TipoMonedaController tipo = new TipoMonedaController();
                    var formulario = (List<E_Gastos>)Session["acuenta"];

                    if (formulario == null)
                    {
                        List<E_Gastos> lista = new List<E_Gastos>();
                        E_Gastos gasto = new E_Gastos();
                        gasto.ItemACta = 1;
                        gasto.CodTipMon = Egas.CodTipMon;
                        var tipoMoneda = tipo.ListadoTipoMoneda().Where(x => x.CodTipMon == Egas.CodTipMon).FirstOrDefault();
                        gasto.DescripTip = tipoMoneda.DescTipMon;
                        gasto.TipoCambio = Egas.TipoCambio;
                        gasto.FechaPago = Egas.FechaPago;
                        gasto.TipoPago = Egas.TipoPago;
                        gasto.Monto = Egas.Monto;
                        gasto.Observ = Egas.Observ == null ? "" : Egas.Observ.ToUpper();
                        lista.Add(gasto);
                        Session["acuenta"] = lista;
                        ViewBag.listaAcuenta = (List<E_Gastos>)Session["acuenta"];
                        ViewBag.modal = "1";
                        montototal = gasto.Monto;
                        ViewBag.MontoTotal = montototal;

                        return View(Egas);
                    }
                    else
                    {

                        E_Gastos gasto = new E_Gastos();
                        var firt = formulario.LastOrDefault();
                        gasto.ItemACta = firt.ItemACta + 1;
                        gasto.CodTipMon = Egas.CodTipMon;
                        var tipoMoneda = tipo.ListadoTipoMoneda().Where(x => x.CodTipMon == Egas.CodTipMon).FirstOrDefault();
                        gasto.DescripTip = tipoMoneda.DescTipMon;
                        gasto.TipoCambio = Egas.TipoCambio;
                        gasto.FechaPago = Egas.FechaPago;
                        gasto.TipoPago = Egas.TipoPago;
                        gasto.Monto = Egas.Monto;
                        gasto.Observ = Egas.Observ == null ? "" : Egas.Observ.ToUpper();
                        formulario.Add(gasto);
                        Session["acuenta"] = formulario;
                        ViewBag.listaAcuenta = (List<E_Gastos>)Session["acuenta"];
                        foreach (var item in formulario)
                        {
                            montototal += item.Monto;
                        }

                        ViewBag.MontoTotal = montototal;
                        ViewBag.modal = "1";
                        if (Session["gastos"] != null) ViewBag.gastos = (List<E_Gastos>)Session["gastos"];
                        return View(Egas);
                    }

                }
                else if (Egas.Evento == "5")
                {
                    if (Egas.CodigoTipoDocumento == 0) { Egas.CodigoTipoDocumento = 0; };
                    if (Egas.CodigoTipoDocumento == 2)
                    {

                        if (Egas.Serie != null) { ViewBag.serie = Egas.Serie; } else { ViewBag.serie = ""; }
                    }
                    else
                    {

                        if (Egas.Serie != null) { ViewBag.serie = Egas.Serie; } else { ViewBag.serie = ""; }
                    }
                    //Eliminar 
                    var FormularioE = (List<E_Gastos>)Session["acuenta"];
                    var remove = FormularioE.Find(x => x.ItemACta == Egas.ItemACta);
                    FormularioE.Remove(remove);
                    Session["acuenta"] = FormularioE;
                    if (FormularioE.Count() == 0)
                    {
                        ViewBag.renta = 0;
                        Session["acuenta"] = null;

                    }
                    else
                    {
                        decimal Montototal = 0;
                        foreach (var item in FormularioE)
                        {
                            Montototal += item.Monto;
                        }
                        ViewBag.MontoTotal = Montototal;
                        ViewBag.incrementoMonto = remove.Monto;
                        Session["acuenta"] = FormularioE;
                        ViewBag.listaAcuenta = (List<E_Gastos>)Session["acuenta"];
                    }

                    ViewBag.modal = "1";
                    return View(Egas);

                }
                else if (Egas.Evento == "6")
                {

                    var formularioGastos = (List<E_Gastos>)Session["gastos"];

                    if (formularioGastos == null)
                    {

                        E_Gastos gastos = new E_Gastos();
                        List<E_Gastos> Lista = new List<E_Gastos>();
                        var ctacontable = Lista_CuentaContable().Where(x => x.CodigoContable == Egas.CodigoContable).FirstOrDefault();
                        gastos.ItemG = 1;
                        gastos.CodigoContable = Egas.CodigoContable;
                        gastos.DescripcionCtaContable = ctacontable.DescripcionCtaContable;
                        gastos.Cant = Egas.Cant;
                        gastos.PrecioU = Egas.PrecioU;
                        gastos.Total = Egas.Cant * Egas.PrecioU;
                        gastos.TotalBruto = gastos.Total;
                        Lista.Add(gastos);
                        ViewBag.totalbruto = gastos.TotalBruto;
                        gastos.IgvC = gastos.TotalBruto * Convert.ToDecimal(0.18);
                        //total
                        ViewBag.totalbruto = gastos.Total + gastos.IgvC;
                        //acuenta 
                        var acuenta = (List<E_Gastos>)Session["acuenta"];
                        if (acuenta != null)
                        {
                            foreach (var item in acuenta)
                            {
                                ViewBag.MontoTotal += item.Monto;
                            }
                        }
                        ViewBag.igv = gastos.IgvC;
                        if (Egas.CodigoTipoDocumento == 0) { Egas.CodigoTipoDocumento = 0; };
                        if (Egas.CodigoTipoDocumento == 2)
                        {
                            ViewBag.renta = gastos.Total * Convert.ToDecimal("0.08");
                            if (Egas.Serie != null) { ViewBag.serie = Egas.Serie; } else { ViewBag.serie = ""; }
                        }
                        else
                        {
                            ViewBag.renta = 0;
                            if (Egas.Serie != null) { ViewBag.serie = Egas.Serie; } else { ViewBag.serie = ""; }
                        }

                        ViewBag.importe = gastos.TotalBruto; ViewBag.igv = gastos.IgvC; ViewBag.retencion = Egas.Retencion; ViewBag.detraccion = Egas.Detraccion; ViewBag.total = gastos.TotalBruto + gastos.IgvC;
                        Session["gastos"] = Lista;
                        ViewBag.gastos = (List<E_Gastos>)Session["gastos"];
                        if (Egas.CodigoTipoDocumento != 0) { Egas.CodigoTipoDocumento = 0; };
                        if (Egas.CodigoTipoDocumento == 2)
                        {
                            ViewBag.renta = gastos.Total * Convert.ToDecimal("0.08");
                            if (Egas.Serie != null) { ViewBag.serie = Egas.Serie; } else { ViewBag.serie = ""; }
                        }
                        else
                        {
                            ViewBag.renta = 0;
                            if (Egas.Serie != null) { ViewBag.serie = Egas.Serie; } else { ViewBag.serie = ""; }
                        }
                        var total = gastos.TotalBruto + gastos.IgvC;
                        if (total > 700)
                        {
                            if (Egas.IDbs != 0)
                            {
                                var bienes = ListaBienesServicio().Where(x => x.IDbs == Egas.IDbs).FirstOrDefault();
                                ViewBag.porcentaje = bienes.Porcentaje;
                                ViewBag.detraccion = (total * bienes.Porcentaje) / 100;
                            }
                        }
                        return View(Egas);
                    }
                    else
                    {

                        E_Gastos gastos = new E_Gastos();
                        var last = formularioGastos.LastOrDefault();
                        var ctacontable = Lista_CuentaContable().Where(x => x.CodigoContable == Egas.CodigoContable).FirstOrDefault();
                        gastos.ItemG = last.ItemG + 1;
                        gastos.CodigoContable = Egas.CodigoContable;
                        gastos.DescripcionCtaContable = ctacontable.DescripcionCtaContable;
                        gastos.Cant = Egas.Cant;
                        gastos.PrecioU = Egas.PrecioU;
                        gastos.Total = Egas.Cant * Egas.PrecioU;
                        if (Egas.CodigoTipoDocumento != 0) { Egas.CodigoTipoDocumento = 0; };
                        if (Egas.CodigoTipoDocumento == 2)
                        {
                            ViewBag.renta = gastos.Total * Convert.ToDecimal("0.08");
                            if (Egas.Serie != null) { ViewBag.serie = Egas.Serie; } else { ViewBag.serie = ""; }
                        }
                        else
                        {
                            ViewBag.renta = 0;
                            if (Egas.Serie != null) { ViewBag.serie = Egas.Serie; } else { ViewBag.serie = ""; }
                        }
                        foreach (var item in formularioGastos)
                        {
                            gastos.TotalBruto = gastos.TotalBruto + item.Total;
                        }

                        gastos.IgvC = (gastos.TotalBruto + gastos.Total) * Convert.ToDecimal(0.18);
                        gastos.TotalBruto = (gastos.TotalBruto + gastos.Total);
                        ViewBag.importe = gastos.TotalBruto; ViewBag.igv = gastos.IgvC; ViewBag.retencion = Egas.Retencion; ViewBag.detraccion = Egas.Detraccion; ViewBag.total = gastos.TotalBruto + gastos.IgvC;
                        ViewBag.totalbruto = gastos.TotalBruto + gastos.IgvC;
                        //acuenta 
                        var acuenta = (List<E_Gastos>)Session["acuenta"];
                        if (acuenta != null)
                        {
                            decimal monto = 0;
                            foreach (var item in acuenta)
                            {
                                monto += item.Monto;
                            }
                            ViewBag.MontoTotal = monto;
                        }

                        ViewBag.igv = gastos.IgvC;
                        //ViewBag.MontoTotal = gastos.Total * gastos.IgvC;
                        formularioGastos.Add(gastos);
                        Session["gastos"] = formularioGastos;
                        ViewBag.gastos = (List<E_Gastos>)Session["gastos"];

                        var total = gastos.TotalBruto + gastos.IgvC;
                        if (total > 700)
                        {
                            if (Egas.IDbs != 0)
                            {
                                var bienes = ListaBienesServicio().Where(x => x.IDbs == Egas.IDbs).FirstOrDefault();
                                ViewBag.porcentaje = bienes.Porcentaje;
                                ViewBag.detraccion = (total * bienes.Porcentaje) / 100;
                            }
                        }
                        return View(Egas);
                    }
                }

                else if (Egas.Evento == "7")
                {
                    var formularioE = (List<E_Gastos>)Session["gastos"];
                    var remove = formularioE.Where(x => x.ItemG == Egas.ItemG).FirstOrDefault();
                    formularioE.Remove(remove);
                    if (formularioE.Count() == 0)
                    {
                        ViewBag.totalbruto = 0;
                        ViewBag.renta = 0;
                        ViewBag.importe = 0; ViewBag.igv = 0; ViewBag.retencion = Egas.Retencion; ViewBag.detraccion = Egas.Detraccion; ViewBag.total = 0;
                        Session["gastos"] = null;
                        ViewBag.gastos = null;
                    }
                    else
                    {
                        ViewBag.importe = (Egas.Total - remove.Total); ViewBag.igv = (Egas.Total - remove.Total) * decimal.Parse("0.18"); ViewBag.retencion = Egas.Retencion; ViewBag.detraccion = Egas.Detraccion; ViewBag.total = (Egas.Total - remove.Total) * decimal.Parse("1.18");
                        var acuenta = (List<E_Gastos>)Session["acuenta"];
                        if (acuenta != null)
                        {
                            decimal monto = 0;
                            foreach (var item in acuenta)
                            {
                                monto += item.Monto;
                            }
                            ViewBag.MontoTotal = monto;
                        }

                        Session["gastos"] = formularioE;
                        ViewBag.gastos = (List<E_Gastos>)Session["gastos"];
                        var gastos = (List<E_Gastos>)Session["gastos"];
                        decimal gastototal = 0;
                        foreach (var item in gastos)
                        {
                            gastototal = item.Total;
                        }
                        ViewBag.totalbruto = gastototal * decimal.Parse("1.18");
                    }
                    return View(Egas);
                }
            }
            catch (Exception)
            {
                ViewBag.mensaje = "Error: Datos no Validos";
                return View(Egas);
            }

            return RedirectPermanent("DetalleGastos?id=" + Egas.IdgastosC);
        }

        public JsonResult ObtenerSubCentroCosto(string id = null)
        {
            if (id != null)
            {
                var lista = ListaSubCentroCostos().Where(x => x.IdCentroCosto == id).ToList();
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public JsonResult ObtenerTipoCambio(string value = null)
        {

            if (value != null)
            {
                TipoMonedaController moneda = new TipoMonedaController();
                var monedaT = moneda.ListadoTipoMoneda().Where(x => x.CodTipMon == value).FirstOrDefault();
                return Json(monedaT.TipoCambio, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public JsonResult ObtenerPorcentajeBienesServicio(int? valor = null)
        {
            if (valor != null)
            {
                var lista = ListaBienesServicio().Where(x => x.IDbs == valor).FirstOrDefault();

                return Json(lista.Porcentaje, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var lista = ListaBienesServicio().Where(x => x.IDbs == valor).FirstOrDefault();

                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult ObtenerDatosProveedor(int? valor = null)
        {

            if (valor != null)
            {
                var lista = ListaProveedor().Where(x => x.IdPro == valor).ToList();
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            return null;
        }


        public JsonResult ListaAcuenta(int? id = null)
        {
            string Table = ""; 
            
            if (!string.IsNullOrWhiteSpace(id.ToString()))
            {
                var lista = Usp_Lista_Acuenta(Convert.ToInt32(id));
                foreach (var item in lista) {
                    Table += "<tr><td>" + item.FechaPago  + "</td>"+ "<td>" + item.TipoPago + "</td>" + "<td>" + item.Monto + "</td></tr>";
           
                }
                return Json(Table, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

    }
}