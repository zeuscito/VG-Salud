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
    public class CuentasController : Controller
    {

        public List<E_CuentasEstado> Usp_ListaCuentasXEstado(int historia)
        {
            List<E_CuentasEstado> Lista = new List<E_CuentasEstado>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaCuentasXEstado", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@historia", historia); 
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_CuentasEstado c = new E_CuentasEstado();
                            c.CodCue = dr.GetInt32(0);
                            c.Item = dr.GetInt32(1);
                            c.fecha = dr.GetDateTime(2).ToShortDateString();
                            c.Servicio = dr.GetString(3);
                            c.Tarifa = dr.GetString(4);
                            c.Cantidad = dr.GetInt32(5);
                            c.Total = dr.GetDecimal(6);
                            c.Estado = dr.GetString(7);
                            Lista.Add(c);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public ActionResult ListaCuentaEstado(int id)
        {
            ViewBag.historia = id;
            ViewBag.nombrecompleto = new PacientesController().ListadoPacientes().Where(x => x.Historia == id).FirstOrDefault().nombCompleto; 
            return View();
        }


        public JsonResult CuentaEstado(int id,string F, string G, string L, string C)
        {
            string table = "";
            if (!string.IsNullOrWhiteSpace(F) || !string.IsNullOrWhiteSpace(G) || !string.IsNullOrWhiteSpace(L) || !string.IsNullOrWhiteSpace(C))
            {
                var Datatable = Usp_ListaCuentasXEstado(id).Where(x => x.Estado == F || x.Estado == G || x.Estado == L || x.Estado == C).ToList();

                foreach (var item in Datatable)
                {
                    table += "<tr><td>" + item.fecha + "</td>" + "<td>" + item.Servicio.ToUpper() + "</td>" + "<td>" + item.Tarifa + "</td>" + "</td>" + "<td>" + item.Cantidad + "</td>" + "<td>" + item.Total + "</td>" + "<td>" + item.Estado + "</td></tr>";
                }
                return Json(table, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }



        }

        public List<E_Cuentas> listaPerfilConsentimiento()
        {
            List<E_Cuentas> Lista = new List<E_Cuentas>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listPerfilConsentimiento", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Cuentas a = new E_Cuentas();

                            a.TipoCons = dr.GetInt32(0);
                            a.desConsentimiento = dr.GetString(1);
                            a.textoConsentimiento = dr.GetString(2);
                            a.estadoConsentimiento = dr.GetBoolean(3);

                            Lista.Add(a);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Cuentas> BuscaCuenta(int id)
        {
            List<E_Cuentas> Lista = new List<E_Cuentas>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_BuscaCuenta", con))
                {
                    cmd.Parameters.AddWithValue("@CodCue", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Cuentas a = new E_Cuentas();

                            a.CodCue = dr.GetInt32(0);
                            a.CodSede = dr.GetString(1);
                            a.Historia = dr.GetInt32(2);
                            a.CodcatPac = dr.GetString(3);
                            a.STotCue = dr.GetDecimal(4);
                            a.IgvCue = dr.GetDecimal(5);
                            a.TotCue = dr.GetDecimal(6);
                            a.FecCrea = dr.GetDateTime(7);
                            a.FecAnul = dr.GetString(8);
                            a.EstCue = dr.GetBoolean(9);
                            a.EstGene = dr.GetString(10);
                            a.SecFact = dr.GetString(11);
                            a.Usuario = dr.GetString(12);
                            a.UsuarioAnula = dr.GetString(13);

                            Lista.Add(a);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_Cuentas> ListadoCuenta(string CodSede)
        {
            List<E_Cuentas> Lista = new List<E_Cuentas>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaCuentas_Especial", con))
                {

                    cmd.Parameters.AddWithValue("@CodSede", CodSede);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Cuentas a = new E_Cuentas();

                            a.CodCue = dr.GetInt32(0);
                            a.Historia = dr.GetInt32(1);
                            a.nomCompleto = dr.GetString(2);
                            a.CodcatPac = dr.GetString(3);
                            a.STotCue = dr.GetDecimal(4);
                            a.IgvCue = dr.GetDecimal(5);
                            a.TotCue = dr.GetDecimal(6);
                            a.EstGene = dr.GetString(7);
                            a.FecCrea = dr.GetDateTime(8);

                            Lista.Add(a);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_Cuentas> ListadoCuentaFiltro(string CodSede, string FechaI = null, string FechaF = null, int? Cuenta = null)
        {
            List<E_Cuentas> Lista = new List<E_Cuentas>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaCuentas_Especial_Filtro", con))
                {

                    cmd.Parameters.AddWithValue("@CodSede", CodSede);
                    if (FechaI == null || FechaI == "")
                    {
                        cmd.Parameters.AddWithValue("@FechaI", System.Data.SqlTypes.SqlDateTime.Null);
                    }
                    else {
                        cmd.Parameters.AddWithValue("@FechaI", FechaI);
                    }
                    if (FechaF == null || FechaF == "")
                    {
                        cmd.Parameters.AddWithValue("@FechaF", System.Data.SqlTypes.SqlDateTime.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@FechaF", FechaF);
                    }
                    if (Cuenta == null)
                    {
                        cmd.Parameters.AddWithValue("@Cuenta", System.Data.SqlTypes.SqlInt32.Null);
                    }
                    else
                    {
                        int cuentaF = (int)Cuenta;
                        cmd.Parameters.AddWithValue("@Cuenta", cuentaF);
                    }
                                        
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Cuentas a = new E_Cuentas();

                            a.CodCue = dr.GetInt32(0);
                            a.Historia = dr.GetInt32(1);
                            a.nomCompleto = dr.GetString(2);
                            a.CodcatPac = dr.GetString(3);
                            a.STotCue = dr.GetDecimal(4);
                            a.IgvCue = dr.GetDecimal(5);
                            a.TotCue = dr.GetDecimal(6);
                            a.EstGene = dr.GetString(7);
                            a.FecCrea = dr.GetDateTime(8);

                            Lista.Add(a);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_CuentaDetalle> ListadoCuentaDetalle(int CodCue)
        {
            List<E_CuentaDetalle> Lista = new List<E_CuentaDetalle>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaCuentasDetalle", con))
                {

                    cmd.Parameters.AddWithValue("@CodCue", CodCue);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_CuentaDetalle a = new E_CuentaDetalle();

                            a.Procedencia = dr.GetInt32(0);
                            a.CodCue = dr.GetInt32(1);
                            a.Tarifa = dr.GetString(3);
                            a.CodProce = dr.GetInt32(4);
                            a.CodDetalleP = dr.GetInt32(5);
                            a.Cantidad = dr.GetInt32(7);
                            a.precioUni = dr.GetDecimal(8);
                            a.precio = dr.GetDecimal(9);
                            a.igv = dr.GetDecimal(10);
                            a.total = dr.GetDecimal(11);
                            a.EstDet = dr.GetString(12);
                            a.Item = dr.GetInt32(2);
                            a.CodCuentaGeneral = dr.GetString(20);
                            a.NombreTarifario = dr.GetString(21);

                            Lista.Add(a);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult VerificaCuenta(int id)
        {
            string sede = Session["codSede"].ToString();
            ViewBag.CodCue = id;
            FichaElectronicaController f = new FichaElectronicaController();
            ViewBag.perfil = new SelectList(f.ListadoPerfilesConsentimiento().Where(x => x.Estado == true), "idPFA", "Nombre");
            var lista = ListadoCuenta(sede).Where(x => x.CodCue == id).ToList();
            ViewBag.listaDetalle = (List<E_CuentaDetalle>)ListadoCuentaDetalle(id).Where(x => x.EstDet == "G").ToList();

            return View(lista);
        }

        public ActionResult imprimeConsentimiento(E_Cuentas c)
        {
            try
            {
                     PacientesController pa = new PacientesController();
                    E_Pacientes p = pa.ListadoPacientes().Find(x => x.Historia == c.Historia);
                    DocumentoIdentidadController d = new DocumentoIdentidadController();
                    E_Documento_Identidad doc = d.ListadoDocumentoIdentidad().Find(x => x.CodDocIdent == p.CodDocIdent);
                    UtilitarioController uti = new UtilitarioController();
                    E_Distrito dis = uti.ListadoDistritoSimple().Find(x => x.CodDist == p.CodDist);
                    UtilitarioController u = new UtilitarioController();
                    E_Master ho = u.ListadoHoraServidor().FirstOrDefault();
                    E_Cuentas cuenta = listaPerfilConsentimiento().Find(x => x.TipoCons == c.TipoCons);
                    ViewBag.contenido = cuenta.textoConsentimiento;
                    ViewBag.fecha = ho.HoraServidor.ToShortDateString();
                    ViewBag.nombreCompleto = p.NomPac + " " + p.ApePat + " " + p.ApeMat;
                    ViewBag.NomDocIdent = doc.NomDocIdent;
                    ViewBag.NumDoc = p.NumDoc;
                    ViewBag.consInf = c.consInf;
                    ViewBag.TipoCons = c.TipoCons;
                    ViewBag.Direcc = p.Direcc;
                    ViewBag.NomDist = dis.NomDist;

                    Response.Write("<script language='JavaScript' type ='text/javascript'>" +
                  " window.open('../../Cuentas/VerificaCuenta/" + c.CodCue + "', '_blank'); </script>");

                    return View();
            
            }
            catch (Exception)
            {
                ViewBag.mensaje = "Error Datos No Validos";
                return RedirectPermanent("~/Cuentas/VerificaCuenta?id=" + c.CodCue);
            }
          
            
      
        }


        public ActionResult EliminarDetalle(int CodCue, int CodPro, int item)
        {
            AtencionVariasController avc = new AtencionVariasController();

            string Elimina = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;
            string usuario = Session["UserID"].ToString();

            E_CuentaDetalle reg = ListadoCuentaDetalle(CodCue).Find(x => x.CodProce == CodPro);

            var listaCuentasDetatalle = ListadoCuentaDetalle(CodCue).Where(x => x.EstDet == "G").ToList();

            decimal precio = 0, igv = 0, total = 0;
            foreach (var ii in listaCuentasDetatalle)
            {
                if (ii.CodProce != CodPro || ii.Item != item)
                {

                    precio = ii.precio + precio;
                    igv = ii.igv + igv;
                    total = ii.total + total;
                    
                }
            }


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                using (SqlCommand cmd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Procedencia", 2);
                        cmd.Parameters.AddWithValue("@CodCue", CodCue);
                        cmd.Parameters.AddWithValue("@Item", item);
                        cmd.Parameters.AddWithValue("@Tarifa", "");
                        cmd.Parameters.AddWithValue("@CodProce", CodPro);
                        cmd.Parameters.AddWithValue("@CodDetalleP", "");
                        cmd.Parameters.AddWithValue("@CodSede", "");
                        cmd.Parameters.AddWithValue("@Cantidad", 0);
                        cmd.Parameters.AddWithValue("@precioUni", 0);
                        cmd.Parameters.AddWithValue("@precio", 0);
                        cmd.Parameters.AddWithValue("@igv", 0);
                        cmd.Parameters.AddWithValue("@total", 0);
                        cmd.Parameters.AddWithValue("@EstDet", "");
                        cmd.Parameters.AddWithValue("@FechaAten", "");
                        cmd.Parameters.AddWithValue("@TurnoAten", "");
                        cmd.Parameters.AddWithValue("@RegMedico", "");
                        cmd.Parameters.AddWithValue("@MedicoEnvia", "");
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", "");
                        cmd.Parameters.AddWithValue("@Elimina", Elimina);
                        cmd.Parameters.AddWithValue("@Evento", "2");
                        cmd.ExecuteNonQuery();


                        using (SqlCommand da = new SqlCommand("Usp_MtoCuentas", con, tr))
                        {
                            da.CommandType = CommandType.StoredProcedure;
                            da.Parameters.AddWithValue("@CodCue", CodCue);
                            da.Parameters.AddWithValue("@CodSede", "");
                            da.Parameters.AddWithValue("@Historia", "");
                            da.Parameters.AddWithValue("@CodcatPac", "");
                            da.Parameters.AddWithValue("@STotCue", precio);
                            da.Parameters.AddWithValue("@IgvCue", igv);
                            da.Parameters.AddWithValue("@TotCue", total);
                            da.Parameters.AddWithValue("@FecCrea", "");
                            da.Parameters.AddWithValue("@FecAnul", "");
                            da.Parameters.AddWithValue("@EstCue", "1");
                            da.Parameters.AddWithValue("@EstGene", "");
                            da.Parameters.AddWithValue("@SecFact", "");
                            da.Parameters.AddWithValue("@Usuario", "");
                            da.Parameters.AddWithValue("@UsuarioAnula", "");
                            da.Parameters.AddWithValue("@Crea", "");
                            da.Parameters.AddWithValue("@Modifica", Elimina);
                            da.Parameters.AddWithValue("@Elimina", "");
                            da.Parameters.AddWithValue("@Evento", "2");
                            da.ExecuteNonQuery();

                            if (reg.Procedencia == 1)
                            {

                                using (SqlCommand dr = new SqlCommand("Usp_MtoCitas", con, tr))
                                {
                                    dr.CommandType = CommandType.StoredProcedure;
                                    dr.Parameters.AddWithValue("@CodCita", CodPro);
                                    dr.Parameters.AddWithValue("@Historia", "");
                                    dr.Parameters.AddWithValue("@CodEspec", "");
                                    dr.Parameters.AddWithValue("@CodServ", "");
                                    dr.Parameters.AddWithValue("@CodMed", "");
                                    dr.Parameters.AddWithValue("@TipoPac", "");
                                    dr.Parameters.AddWithValue("@Obser", "SE ELIMINO EL DETALLE DE LA CUENTA");
                                    dr.Parameters.AddWithValue("@CodCatPac", "");
                                    dr.Parameters.AddWithValue("@precio", 0);
                                    dr.Parameters.AddWithValue("@igv", 0);
                                    dr.Parameters.AddWithValue("@total", 0);
                                    dr.Parameters.AddWithValue("@CodTar", "");
                                    dr.Parameters.AddWithValue("@CodTipTar", "");
                                    dr.Parameters.AddWithValue("@HoraInicio", "");
                                    dr.Parameters.AddWithValue("@HoraFin", "");
                                    dr.Parameters.AddWithValue("@FechaRegistro", "");
                                    dr.Parameters.AddWithValue("@FechaCita", "");
                                    dr.Parameters.AddWithValue("@Turno", "");
                                    dr.Parameters.AddWithValue("@CodCue", "");
                                    dr.Parameters.AddWithValue("@Usuario", "");
                                    dr.Parameters.AddWithValue("@TipoRegistro", "");
                                    dr.Parameters.AddWithValue("@CodSede", "");
                                    dr.Parameters.AddWithValue("@TipoConsu", "");
                                    dr.Parameters.AddWithValue("@MedInter", "");
                                    dr.Parameters.AddWithValue("@Consultorio", "");
                                    dr.Parameters.AddWithValue("@Crea", "");
                                    dr.Parameters.AddWithValue("@Modifica", "");
                                    dr.Parameters.AddWithValue("@Elimina", Elimina);
                                    dr.Parameters.AddWithValue("@Evento", "3");

                                    dr.ExecuteNonQuery();

                                    tr.Commit();
                                    ViewBag.mensaje = "Se Elimino Correctamente";
                                }

                            }
                            else if (reg.Procedencia == 2)
                            {

                                var listaAtencionesVarias = avc.ListadoAtencionDetalle().Where(x => x.CodAten == CodPro).ToList();

                                decimal precioAV = 0, igvAV = 0, totalAV = 0;
                                foreach (var ia in listaAtencionesVarias)
                                {
                                    if (ia.CodAtenDet != reg.CodDetalleP)
                                    {
                                        precioAV = ia.SubTotal + precioAV;
                                        igvAV = ia.Igv + igvAV;
                                        totalAV = ia.Total + totalAV;
                                    }
                                }

                                using (SqlCommand av = new SqlCommand("Usp_MtoAtencionVarias", con, tr))
                                {
                                    av.CommandType = CommandType.StoredProcedure;

                                    av.Parameters.AddWithValue("@CodAten", CodPro);
                                    av.Parameters.AddWithValue("@CodSede", "");
                                    av.Parameters.AddWithValue("@CodCue", "");
                                    av.Parameters.AddWithValue("@Historia", 1);
                                    av.Parameters.AddWithValue("@NomPac", "");
                                    av.Parameters.AddWithValue("@CodCatPac", "");
                                    av.Parameters.AddWithValue("@SubTotal", precioAV);
                                    av.Parameters.AddWithValue("@Igv", igvAV);
                                    av.Parameters.AddWithValue("@Total", totalAV);
                                    av.Parameters.AddWithValue("@Fecha", "");
                                    av.Parameters.AddWithValue("@CodUsu", "");
                                    av.Parameters.AddWithValue("@EstConsul", "");
                                    av.Parameters.AddWithValue("@Crea", "");
                                    av.Parameters.AddWithValue("@Modifica", Elimina);
                                    av.Parameters.AddWithValue("@Elimina", "");
                                    av.Parameters.AddWithValue("@Evento", "2");

                                    av.ExecuteNonQuery();

                                    using (SqlCommand avd = new SqlCommand("Usp_MtoAtencionVarias_Detalle", con, tr))
                                    {

                                        avd.CommandType = CommandType.StoredProcedure;
                                        avd.Parameters.AddWithValue("@CodAtenDet", reg.CodDetalleP);
                                        avd.Parameters.AddWithValue("@CodAten", "");
                                        avd.Parameters.AddWithValue("@item", "");
                                        avd.Parameters.AddWithValue("@CodSede", "");
                                        avd.Parameters.AddWithValue("@CodEspec", "");
                                        avd.Parameters.AddWithValue("@CodServ", "");
                                        avd.Parameters.AddWithValue("@CodMed", "");
                                        avd.Parameters.AddWithValue("@CodTar", "");
                                        avd.Parameters.AddWithValue("@CodTipTar", "");
                                        avd.Parameters.AddWithValue("@CodSTipTar", "");
                                        avd.Parameters.AddWithValue("@Cantida", 0);
                                        avd.Parameters.AddWithValue("@SubTotal", 0);
                                        avd.Parameters.AddWithValue("@Igv", 0);
                                        avd.Parameters.AddWithValue("@Total", 0);
                                        avd.Parameters.AddWithValue("@MedicoEnvia", "");
                                        avd.Parameters.AddWithValue("@EspeciEnvia", "");
                                        avd.Parameters.AddWithValue("@Turno", "");
                                        avd.Parameters.AddWithValue("@Estado", "");
                                        avd.Parameters.AddWithValue("@CodUsu", "");
                                        avd.Parameters.AddWithValue("@Crea", "");
                                        avd.Parameters.AddWithValue("@Modifica", "");
                                        avd.Parameters.AddWithValue("@Elimina", Elimina);
                                        avd.Parameters.AddWithValue("@Evento", "3");

                                        avd.ExecuteNonQuery();

                                        tr.Commit();
                                        ViewBag.mensaje = "Se Elimino Correctamente";
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                        return View();
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectPermanent("../ListaCuentasDetalle/" + CodCue);
            }

        }



        public ActionResult EliminarCuenta(int CodCue)
        {
            AtencionVariasController avc = new AtencionVariasController();

            string Elimina = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;
            string usuario = Session["UserID"].ToString();

            var listaAtencionesDetalle = new List<E_AtencionVarias_Detalle>();
            listaAtencionesDetalle = (from x in avc.ListadoAtencionDetalle() select x).ToList();


            var listaDetalle = (List<E_CuentaDetalle>)ListadoCuentaDetalle(CodCue).ToList();


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                using (SqlCommand da = new SqlCommand("Usp_MtoCuentas", con, tr))
                {

                    try
                    {

                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@CodCue", CodCue);
                        da.Parameters.AddWithValue("@CodSede", "");
                        da.Parameters.AddWithValue("@Historia", 0);
                        da.Parameters.AddWithValue("@CodcatPac", "");
                        da.Parameters.AddWithValue("@STotCue", 0);
                        da.Parameters.AddWithValue("@IgvCue", 0);
                        da.Parameters.AddWithValue("@TotCue", 0);
                        da.Parameters.AddWithValue("@FecCrea", "");
                        da.Parameters.AddWithValue("@FecAnul", "");
                        da.Parameters.AddWithValue("@EstCue", "0");
                        da.Parameters.AddWithValue("@EstGene", "");
                        da.Parameters.AddWithValue("@SecFact", "");
                        da.Parameters.AddWithValue("@Usuario", "");
                        da.Parameters.AddWithValue("@UsuarioAnula", usuario);
                        da.Parameters.AddWithValue("@Crea", "");
                        da.Parameters.AddWithValue("@Modifica", "");
                        da.Parameters.AddWithValue("@Elimina", Elimina);
                        da.Parameters.AddWithValue("@Evento", "3");
                        da.ExecuteNonQuery();

                        foreach (var it in listaDetalle)
                        {

                            SqlCommand cmd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Procedencia", "");
                            cmd.Parameters.AddWithValue("@CodCue", it.CodCue);
                            cmd.Parameters.AddWithValue("@Item", it.Item);
                            cmd.Parameters.AddWithValue("@Tarifa", "");
                            cmd.Parameters.AddWithValue("@CodProce", it.CodProce);
                            cmd.Parameters.AddWithValue("@CodDetalleP", "");
                            cmd.Parameters.AddWithValue("@CodSede", "");
                            cmd.Parameters.AddWithValue("@Cantidad", 0);
                            cmd.Parameters.AddWithValue("@precioUni", 0);
                            cmd.Parameters.AddWithValue("@precio", 0);
                            cmd.Parameters.AddWithValue("@igv", 0);
                            cmd.Parameters.AddWithValue("@total", 0);
                            cmd.Parameters.AddWithValue("@EstDet", "");
                            cmd.Parameters.AddWithValue("@FechaAten", "");
                            cmd.Parameters.AddWithValue("@TurnoAten", "");
                            cmd.Parameters.AddWithValue("@RegMedico", "");
                            cmd.Parameters.AddWithValue("@MedicoEnvia", "");
                            cmd.Parameters.AddWithValue("@Crea", "");
                            cmd.Parameters.AddWithValue("@Modifica", "");
                            cmd.Parameters.AddWithValue("@Elimina", Elimina);
                            cmd.Parameters.AddWithValue("@Evento", "2");
                            cmd.ExecuteNonQuery();

                            if (it.Procedencia == 1)
                            {

                                using (SqlCommand dr = new SqlCommand("Usp_MtoCitas", con, tr))
                                {
                                    dr.CommandType = CommandType.StoredProcedure;
                                    dr.Parameters.AddWithValue("@CodCita", it.CodProce);
                                    dr.Parameters.AddWithValue("@Historia", "");
                                    dr.Parameters.AddWithValue("@CodEspec", "");
                                    dr.Parameters.AddWithValue("@CodServ", "");
                                    dr.Parameters.AddWithValue("@CodMed", "");
                                    dr.Parameters.AddWithValue("@TipoPac", "");
                                    dr.Parameters.AddWithValue("@Obser", "SE ELIMINO LA CUENTA");
                                    dr.Parameters.AddWithValue("@CodCatPac", "");
                                    dr.Parameters.AddWithValue("@precio", 0);
                                    dr.Parameters.AddWithValue("@igv", 0);
                                    dr.Parameters.AddWithValue("@total", 0);
                                    dr.Parameters.AddWithValue("@CodTar", "");
                                    dr.Parameters.AddWithValue("@CodTipTar", "");
                                    dr.Parameters.AddWithValue("@HoraInicio", "");
                                    dr.Parameters.AddWithValue("@HoraFin", "");
                                    dr.Parameters.AddWithValue("@FechaRegistro", "");
                                    dr.Parameters.AddWithValue("@FechaCita", "");
                                    dr.Parameters.AddWithValue("@Turno", "");
                                    dr.Parameters.AddWithValue("@CodCue", "");
                                    dr.Parameters.AddWithValue("@Usuario", "");
                                    dr.Parameters.AddWithValue("@TipoRegistro", "");
                                    dr.Parameters.AddWithValue("@CodSede", "");
                                    dr.Parameters.AddWithValue("@TipoConsu", "");
                                    dr.Parameters.AddWithValue("@MedInter", "");
                                    dr.Parameters.AddWithValue("@Consultorio", "");
                                    dr.Parameters.AddWithValue("@Crea", "");
                                    dr.Parameters.AddWithValue("@Modifica", "");
                                    dr.Parameters.AddWithValue("@Elimina", Elimina);
                                    dr.Parameters.AddWithValue("@Evento", "3");

                                    dr.ExecuteNonQuery();

                                }

                            }
                            else if (it.Procedencia == 2)
                            {

                                using (SqlCommand av = new SqlCommand("Usp_MtoAtencionVarias", con, tr))
                                {
                                    av.CommandType = CommandType.StoredProcedure;

                                    av.Parameters.AddWithValue("@CodAten", it.CodProce);
                                    av.Parameters.AddWithValue("@CodSede", "");
                                    av.Parameters.AddWithValue("@CodCue", "");
                                    av.Parameters.AddWithValue("@Historia", 0);
                                    av.Parameters.AddWithValue("@NomPac", "");
                                    av.Parameters.AddWithValue("@CodCatPac", "");
                                    av.Parameters.AddWithValue("@SubTotal", 0);
                                    av.Parameters.AddWithValue("@Igv", 0);
                                    av.Parameters.AddWithValue("@Total", 0);
                                    av.Parameters.AddWithValue("@Fecha", "");
                                    av.Parameters.AddWithValue("@CodUsu", "");
                                    av.Parameters.AddWithValue("@EstConsul", "");
                                    av.Parameters.AddWithValue("@Crea", "");
                                    av.Parameters.AddWithValue("@Modifica", Elimina);
                                    av.Parameters.AddWithValue("@Elimina", "");
                                    av.Parameters.AddWithValue("@Evento", "3");

                                    av.ExecuteNonQuery();



                                    listaAtencionesDetalle = listaAtencionesDetalle.Where(x => x.CodAten == it.CodProce).ToList();

                                    foreach (var detalle in listaAtencionesDetalle)
                                    {

                                        SqlCommand avd = new SqlCommand("Usp_MtoAtencionVarias_Detalle", con, tr);
                                        avd.CommandType = CommandType.StoredProcedure;


                                        avd.CommandType = CommandType.StoredProcedure;
                                        avd.Parameters.AddWithValue("@CodAtenDet", detalle.CodAtenDet);
                                        avd.Parameters.AddWithValue("@CodAten", "");
                                        avd.Parameters.AddWithValue("@item", "");
                                        avd.Parameters.AddWithValue("@CodSede", "");
                                        avd.Parameters.AddWithValue("@CodEspec", "");
                                        avd.Parameters.AddWithValue("@CodServ", "");
                                        avd.Parameters.AddWithValue("@CodMed", "");
                                        avd.Parameters.AddWithValue("@CodTar", "");
                                        avd.Parameters.AddWithValue("@CodTipTar", "");
                                        avd.Parameters.AddWithValue("@CodSTipTar", "");
                                        avd.Parameters.AddWithValue("@Cantida", 0);
                                        avd.Parameters.AddWithValue("@SubTotal", 0);
                                        avd.Parameters.AddWithValue("@Igv", 0);
                                        avd.Parameters.AddWithValue("@Total", 0);
                                        avd.Parameters.AddWithValue("@MedicoEnvia", "");
                                        avd.Parameters.AddWithValue("@EspeciEnvia", "");
                                        avd.Parameters.AddWithValue("@Turno", "");
                                        avd.Parameters.AddWithValue("@Estado", "");
                                        avd.Parameters.AddWithValue("@CodUsu", "");
                                        avd.Parameters.AddWithValue("@Crea", "");
                                        avd.Parameters.AddWithValue("@Modifica", "");
                                        avd.Parameters.AddWithValue("@Elimina", Elimina);
                                        avd.Parameters.AddWithValue("@Evento", "3");
                                        avd.ExecuteNonQuery();

                                    }                                    
                                }
                            }                            
                        }
                        tr.Commit();
                        ViewBag.mensaje = "Se Elimino Correctamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                        return View();
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListaCuentas");
            }

        }


        public ActionResult ListaCuentas(string FechaI = null, string FechaF = null, int? Cuenta = null)
        {
            ViewBag.FechaI = FechaI;
            ViewBag.FechaF = FechaF;
            ViewBag.Cuenta = Cuenta;
            string sede = Session["codSede"].ToString();
            return View(ListadoCuentaFiltro(sede, FechaI, FechaF, Cuenta));
        }

        public ActionResult ListaCuentasDetalle(int id)
        {
            string sede = Session["codSede"].ToString();
            E_Cuentas reg = ListadoCuenta(sede).Find(x => x.CodCue==id);

            ViewBag.codigoCuenta = id;
            ViewBag.historia = reg.Historia;
            ViewBag.categoriaP = reg.CodcatPac;
            ViewBag.nombrePac = reg.nomCompleto;
            ViewBag.precio = reg.STotCue;
            ViewBag.igv = reg.IgvCue;
            ViewBag.total = reg.TotCue;
            ViewBag.estado = reg.EstGene;

          

            return View(ListadoCuentaDetalle(id));
        }


        
        

    }
}