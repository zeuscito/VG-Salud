using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using VgSalud.Models;
using System.Configuration;
namespace VgSalud.Controllers
{
    public class PromoController : Controller
    {
        public ActionResult RegistrarPromocion()
        {
            ViewBag.codesp = null;
            ViewBag.carrito = null; Session["formulario"] = null;
            EspecialidadController esp = new EspecialidadController();
            ServiciosController ser = new ServiciosController();
            ViewBag.especialidad = new SelectList(esp.ListadoEspecialidades().Where(x=>x.EstEspec == true), "CodEspec", "NomEspec");
            ViewBag.servicio = new SelectList(ser.ListadoServicios(), "CodServ", "NomServ");
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarPromocion(E_Promo promo)
        {
            int IdPC = 0;
            ViewBag.codesp = promo.CodEspec.ToString();
            ViewBag.codtar = promo.CodTar.ToString();
            EspecialidadController esp = new EspecialidadController();
            ServiciosController ser = new ServiciosController();
            TarifarioController tar = new TarifarioController();
            ViewBag.especialidad = new SelectList(esp.ListadoEspecialidades().Where(x => x.EstEspec == true), "CodEspec", "NomEspec", promo.CodEspec);
            ViewBag.servicio = new SelectList(ser.ListadoServicios(), "CodServ", "NomServ", promo.CodServ);

            if (promo.Evento == "1")
            {
                var carrito = (List<E_Promo>)Session["formulario"];


                if (carrito == null)
                {
                    var tarifario = tar.ListadoTarifa().Where(x => x.CodTar == promo.CodTar).FirstOrDefault();
                    List<E_Promo> lista = new List<E_Promo>();
                    E_Promo prom = new E_Promo();
                    prom.CodTar = promo.CodTar;
                    prom.DescTar = tarifario.DescTar;
                    prom.Precio = promo.Precio;
                    prom.cantidad = promo.cantidad;
                    prom.afectaigv = tarifario.AfecIgcv;
                    prom.SubtotalD = promo.cantidad * promo.Precio;
                    lista.Add(prom);
                    Session["formulario"] = lista;
                    ViewBag.carrito = (List<E_Promo>)Session["formulario"];
                    decimal subtotal = 0;
                    foreach (var item in (List<E_Promo>)Session["formulario"])
                    {
                        subtotal += item.SubtotalD;
                    }
                    ViewBag.subtotal = subtotal;

                    return View(promo);
                }
                else
                {
                    var buscar = carrito.Where(x => x.CodTar == promo.CodTar).FirstOrDefault();
                    if (buscar != null)
                    {
                        var remove = carrito.Where(x => x.CodTar == promo.CodTar).FirstOrDefault();
                        carrito.Remove(remove);
                        Session["formulario"] = carrito;
                        var tarifario = tar.ListadoTarifa().Where(x => x.CodTar == promo.CodTar).FirstOrDefault();

                        E_Promo prom = new E_Promo();
                        prom.CodTar = promo.CodTar;
                        prom.DescTar = tarifario.DescTar;
                        prom.cantidad = promo.cantidad;
                        prom.Precio = promo.Precio;
                        prom.afectaigv = tarifario.AfecIgcv;
                        prom.SubtotalD = promo.Precio * promo.cantidad;
                        carrito.Add(prom);
                        Session["formulario"] = carrito;
                        ViewBag.carrito = (List<E_Promo>)Session["formulario"];
                        decimal subtotal = 0;
                        foreach (var item in (List<E_Promo>)Session["formulario"])
                        {
                            subtotal += item.SubtotalD;
                        }
                        ViewBag.subtotal = subtotal;

                        return View(promo);
                    }
                    else
                    {
                        var tarifario = tar.ListadoTarifa().Where(x => x.CodTar == promo.CodTar).FirstOrDefault();
                        E_Promo prom = new E_Promo();
                        prom.CodTar = promo.CodTar;
                        prom.DescTar = tarifario.DescTar;
                        prom.Precio = promo.Precio;
                        prom.cantidad = promo.cantidad;
                        prom.afectaigv = tarifario.AfecIgcv;
                        prom.SubtotalD = promo.cantidad * promo.Precio;
                        carrito.Add(prom);
                        Session["formulario"] = carrito;
                        ViewBag.carrito = (List<E_Promo>)Session["formulario"];
                        decimal subtotal = 0; decimal igv = 0; decimal total = 0;
                        foreach (var item in (List<E_Promo>)Session["formulario"])
                        {
                            subtotal += item.SubtotalD;
                        }
                        ViewBag.subtotal = subtotal;
                        return View(promo);
                    }
                }
            }
            else if (promo.Evento == "2")
            {
                var carrito = (List<E_Promo>)Session["formulario"];
                var remove = carrito.Where(x => x.CodTar == promo.CodTar1).FirstOrDefault();
                carrito.Remove(remove);
                Session["formulario"] = carrito;
                ViewBag.carrito = (List<E_Promo>)Session["formulario"];
                decimal subtotal = 0;
                if (carrito.Count() != 0)
                {
                    foreach (var item in (List<E_Promo>)Session["formulario"])
                    {
                        subtotal += item.SubtotalD;

                    }
                    ViewBag.subtotal = subtotal;
                }
                else
                {
                    ViewBag.subtotal = 0;

                }
            }
            else if (promo.Evento == "3")
            {
                EspecialidadController especialidad = new EspecialidadController();
                var sede = especialidad.ListadoEspecialidades().Where(x => x.CodEspec == promo.CodEspec).FirstOrDefault();
                UtilitarioController ut = new UtilitarioController();
                var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
                string crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
                MasterController ma = new MasterController();

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                    con.Open();
                    SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                    using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_PromoCabecera", con, tr))
                    {
                        try
                        {
                            da.CommandType = CommandType.StoredProcedure;
                            da.Parameters.AddWithValue("@IdPC", "");
                            da.Parameters.AddWithValue("@Descripcion", promo.Descripcion);
                            da.Parameters.AddWithValue("@CodEspec", promo.CodEspec);
                            da.Parameters.AddWithValue("@CodSede", sede.CodSed);
                            da.Parameters.AddWithValue("@Total", promo.TotalC);
                            da.Parameters.AddWithValue("@Crea", crea);
                            da.Parameters.AddWithValue("@Modifica", "");
                            da.Parameters.AddWithValue("@Eliminar", "");
                            da.Parameters.AddWithValue("@Evento", "1");
                            IdPC = (int)da.ExecuteScalar();

                            TarifarioController tarifario = new TarifarioController();
                 
                            foreach (var item in (List<E_Promo>)Session["formulario"])
                            {
                                using (SqlCommand da1 = new SqlCommand("Usp_Mantenimiento_PromoDetalle", con, tr))
                                {
                                    var tarifa = tarifario.ListadoTarifa().Where(x => x.CodTar == item.CodTar).FirstOrDefault();
                                    da1.CommandType = CommandType.StoredProcedure;
                                    da1.Parameters.AddWithValue("@IdPC", IdPC);
                                    da1.Parameters.AddWithValue("@item", "");
                                    da1.Parameters.AddWithValue("@CodTar", item.CodTar);
                                    da1.Parameters.AddWithValue("@CodTipTar", tarifa.CodTipTar);
                                    da1.Parameters.AddWithValue("@Precio", item.Precio);
                                    da1.Parameters.AddWithValue("@cant", item.cantidad);
                                    da1.Parameters.AddWithValue("@SubTotal", item.SubtotalD);
                                    da1.Parameters.AddWithValue("@Crea", crea);
                                    da1.Parameters.AddWithValue("@Modifica", "");
                                    da1.Parameters.AddWithValue("@Eliminar", "");
                                    da1.Parameters.AddWithValue("@Evento", "1");
                                    da1.ExecuteNonQuery();
                                }
                            }



                            ViewBag.mensaje = "Se registro Correctamente";
                            tr.Commit();

                        }
                        catch (Exception e)
                        {
                            ViewBag.mensaje = "Error: " + e.Message;
                            tr.Rollback();
                            return View(promo);
                        }
                        finally { con.Close(); }
                    }

                }
                return RedirectPermanent("RegistroPromo?id=" + IdPC);
            }
            else if (promo.Evento == "4")
            {
                var carrito = (List<E_Promo>)Session["formulario"];
                var remove = carrito.Where(x => x.CodTar == promo.CodTar1).FirstOrDefault();
                carrito.Remove(remove);
                Session["formulario"] = carrito;
                var tarifario = tar.ListadoTarifa().Where(x => x.CodTar == promo.CodTar1).FirstOrDefault();
                E_Promo prom = new E_Promo();
                prom.CodTar = promo.CodTar1;
                prom.DescTar = tarifario.DescTar;
                prom.cantidad = promo.cantidad;
                prom.afectaigv = tarifario.AfecIgcv;
                prom.Precio = promo.Precio;
                prom.SubtotalD = prom.Precio * prom.cantidad;
                prom.TotalD = prom.SubtotalD * Convert.ToDecimal(1.18);
                prom.IgvD = prom.SubtotalD * Convert.ToDecimal(0.18);

                carrito.Add(prom);
                Session["formulario"] = carrito;
                ViewBag.carrito = (List<E_Promo>)Session["formulario"];
                decimal subtotal = 0; decimal igv = 0; decimal total = 0;
                foreach (var item in (List<E_Promo>)Session["formulario"])
                {
                    subtotal += item.SubtotalD;
                    igv += item.IgvD;
                    total += item.TotalD;
                }
                ViewBag.subtotal = subtotal;
                ViewBag.igv = igv;
                ViewBag.total = total;
                return View(promo);
            }

            return View();
        }

        public List<E_Promo> ListaPromoCabecera()
        {
            EspecialidadController esp = new EspecialidadController();
            List<E_Promo> Lista = new List<E_Promo>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_listaPromocionesCabecera", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        E_Promo e = new E_Promo();
                        e.IdPC = dr.GetInt32(0);
                        e.Descripcion = dr.GetString(1);
                        e.CodEspec = dr.GetString(2);
                        var DescEsp = esp.ListadoEspecialidades().Where(x => x.CodEspec == dr.GetString(2)).FirstOrDefault();
                        e.DescEsp = DescEsp.NomEspec;
                        e.TotalC = dr.GetDecimal(4);
                        e.estado = dr.GetBoolean(8);
                        Lista.Add(e);
                    }
                }
            }
            return Lista;
        }

        public List<E_Promo> ListaPromoDetalle(int id)
        {
            TarifarioController tar = new TarifarioController();
            List<E_Promo> Lista = new List<E_Promo>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Usp_ListaPromocionesDetalle", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Idpc", id);
                    con.Open();
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        E_Promo e = new E_Promo();
                        e.IdPC = dr.GetInt32(0);
                        e.item = dr.GetInt32(1);
                        e.CodTar = dr.GetString(2);
                        e.DescTar = (new TarifarioController().ListadoTarifa().Where(x => x.CodTar == dr.GetString(2))).FirstOrDefault().DescTar;
                        e.Descripcion = (new TipoTarifaController().ListadoTipoTarifa().Where(x => x.CodTipTar == dr.GetString(3)).FirstOrDefault().DescTipTar);
                        e.Precio = dr.GetDecimal(4);
                        e.cantidad = dr.GetInt32(5);
                        e.TotalD = dr.GetDecimal(6);
                        e.estado = dr.GetBoolean(10);
                        Lista.Add(e);
                    }
                }
            }
            return Lista;
        }

        public ActionResult Modificar(int id)
        {

            EspecialidadController esp = new EspecialidadController();
            ServiciosController ser = new ServiciosController();
            TarifarioController tar = new TarifarioController();

            var Cabecera = ListaPromoCabecera().Where(x => x.IdPC == id).FirstOrDefault();
            ViewBag.subtotal = Cabecera.TotalC;
            ViewBag.especialidad = new SelectList(esp.ListadoEspecialidades(), "CodEspec", "NomEspec", Cabecera.CodEspec);
            var model = ListaPromoCabecera().Where(x => x.IdPC == id).FirstOrDefault();
            ViewBag.carrito = ListaPromoDetalle(id);
            return View(model);
        }

        public Decimal TotalGeneral(int IdPC) {
            decimal total = 0;
            foreach (var db in ListaPromoDetalle(IdPC))
            {
                total += db.TotalD;
            }
            return total; 
        }

        [HttpPost]
        public ActionResult Modificar(E_Promo promo)
        {
            EspecialidadController esp = new EspecialidadController();
            ServiciosController ser = new ServiciosController();
            TarifarioController tar = new TarifarioController();
            ViewBag.codtar = promo.CodTar;
            ViewBag.codesp = promo.CodEspec; 
            var sede = esp.ListadoEspecialidades().Where(x => x.CodEspec == promo.CodEspec).FirstOrDefault();

            ViewBag.especialidad = new SelectList(esp.ListadoEspecialidades(), "CodEspec", "NomEspec",promo.CodEspec);
            var model = ListaPromoCabecera().Where(x => x.IdPC == promo.IdPC).FirstOrDefault();
            ViewBag.carrito = ListaPromoDetalle(promo.IdPC);
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string Modifica = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
            if (promo.Evento == "1")
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    try
                    {
                        con.Open();
                        TarifarioController tarifario = new TarifarioController();
                        using (SqlCommand da1 = new SqlCommand("Usp_Mantenimiento_PromoDetalle", con))
                        {
                            var tarifa = tarifario.ListadoTarifa().Where(x => x.CodTar == promo.CodTar).FirstOrDefault();
                            da1.CommandType = CommandType.StoredProcedure;
                            da1.Parameters.AddWithValue("@IdPC", promo.IdPC);
                            da1.Parameters.AddWithValue("@item", "");
                            da1.Parameters.AddWithValue("@CodTar", promo.CodTar);
                            da1.Parameters.AddWithValue("@CodTipTar", tarifa.CodTipTar);
                            da1.Parameters.AddWithValue("@Precio", promo.Precio);
                            da1.Parameters.AddWithValue("@cant", promo.cantidad);
                            da1.Parameters.AddWithValue("@SubTotal",(promo.cantidad * promo.Precio));
                            da1.Parameters.AddWithValue("@Crea", Modifica);
                            da1.Parameters.AddWithValue("@Modifica", "");
                            da1.Parameters.AddWithValue("@Eliminar", "");
                            da1.Parameters.AddWithValue("@Evento", "2");
                            da1.ExecuteNonQuery();
                        }
                        ViewBag.carrito = ListaPromoDetalle(promo.IdPC);
                        ViewBag.subtotal = TotalGeneral(promo.IdPC);
                        ViewBag.mensaje = "Se registro Correctamente";
                        return View(promo);
                    }
                    catch (Exception e)
                    {
                        ViewBag.mensaje = "Error: " + e.Message;

                        return View(promo);
                    }
                    finally { con.Close(); }

                }

            }

            else if (promo.Evento == "2")
            {
                try
                {
                    decimal TotalPD = 0;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                    {
                        using (SqlCommand da1 = new SqlCommand("Usp_Mantenimiento_PromoDetalle", cnn))
                        {
                            cnn.Open();
                            var tarifa = tar.ListadoTarifa().Where(x => x.CodTar == promo.CodTar).FirstOrDefault();
                            da1.CommandType = CommandType.StoredProcedure;
                            da1.Parameters.AddWithValue("@IdPC", promo.IdPC);
                            da1.Parameters.AddWithValue("@item", "");
                            da1.Parameters.AddWithValue("@CodTar", promo.CodTar1);
                            da1.Parameters.AddWithValue("@CodTipTar", "");
                            da1.Parameters.AddWithValue("@Precio", 0);
                            da1.Parameters.AddWithValue("@cant", 0);
                            da1.Parameters.AddWithValue("@SubTotal", 0);
                            da1.Parameters.AddWithValue("@Crea", "");
                            da1.Parameters.AddWithValue("@Modifica", "");
                            da1.Parameters.AddWithValue("@Eliminar", Modifica);
                            da1.Parameters.AddWithValue("@Evento", "3");
                            da1.ExecuteNonQuery();
                        }
                        TotalPD = TotalGeneral(promo.IdPC);

                        using (SqlCommand da1 = new SqlCommand("Actualizar_Total_Promo", cnn))
                        {
                            da1.CommandType = CommandType.StoredProcedure;
                            da1.Parameters.AddWithValue("@IdPC", promo.IdPC);
                            da1.Parameters.AddWithValue("@Total", TotalPD);
                            da1.ExecuteNonQuery();
                        }
                    }
                    ViewBag.carrito = ListaPromoDetalle(promo.IdPC);
                    ViewBag.subtotal = TotalPD;
                    return View(promo);
                }
                catch (Exception)
                {
                    ViewBag.mensaje = "Error Datos no Validos";
                    return View(promo);
                }
               
            }
            else if (promo.Evento == "3")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                    con.Open();
                     using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_PromoCabecera", con))
                    {
                        try
                        {
                            da.CommandType = CommandType.StoredProcedure;
                            da.Parameters.AddWithValue("@IdPC", promo.IdPC);
                            da.Parameters.AddWithValue("@Descripcion", promo.Descripcion);
                            da.Parameters.AddWithValue("@CodEspec", promo.CodEspec);
                            da.Parameters.AddWithValue("@CodSede", sede.CodSed);
                            da.Parameters.AddWithValue("@Total", promo.TotalC);
                            da.Parameters.AddWithValue("@Crea", "");
                            da.Parameters.AddWithValue("@Modifica", Modifica);
                            da.Parameters.AddWithValue("@Eliminar", "");
                            da.Parameters.AddWithValue("@Evento", "2");
                            da.ExecuteNonQuery();
                            return RedirectToAction("ListarPromociones");
                        }
                         
                        catch (Exception)
                        {
                            ViewBag.mensaje = "Error Datos no Validos";
                            return View();
                        }
                        finally {
                            con.Close();
                        }
                    }
                }
            }
            return View();

        }

        public ActionResult ListarPromociones()
        {
            ViewBag.lista = ListaPromoCabecera();

            return View();
        }

        public ActionResult RegistroPromo(int id)
        {
            var model = ListaPromoCabecera().Where(x => x.IdPC == id).FirstOrDefault();
            ViewBag.subtotal = TotalGeneral(id);
            ViewBag.carrito = ListaPromoDetalle(id);
            return View(model);

        }


        public JsonResult ObtenerTarifa(string codesp)
        {
            TarifarioController tar = new TarifarioController();
            if (!string.IsNullOrWhiteSpace(codesp))
            {
                List<E_Tarifario> tarifa = tar.ListadoTarifa().Where(x => x.CodEspec == codesp).ToList();
                return Json(tarifa, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

    }
}