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
    public class ExamenLaboratorioController : Controller
    {
        string cadena = ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString;


        TipoTarifaController tt = new TipoTarifaController();
        EspecialidadController es = new EspecialidadController();
        ServiciosController ser = new ServiciosController();
        MedicosController med = new MedicosController();
        TarifarioController tar = new TarifarioController();
        AtencionVariasController aten = new AtencionVariasController();
        CitasController cit = new CitasController();
        public ActionResult RegistroExamenLaboratorio(int historia, E_Examen_Laboratorio exa)
        {
            exa.Historia = historia;
            string sede = Session["CodSede"].ToString();
            ViewBag.ListaTipoExamenLaboratorio = new SelectList(getListaTipoExamenLaboratorio(), "idTipoExamen", "nomTipoExamen");
            ViewBag.listadoTari = new SelectList(cit.ListadoTarifaAtencion().Where(x => x.CodEspec == "ES006" && x.CodSede == sede), "CodTar", "DescTar");
            return View(exa);
        }
        [HttpPost]
        public ActionResult RegistroExamenLaboratorio(E_Examen_Laboratorio exa)
        {
            string sede = Session["CodSede"].ToString();
            ViewBag.ListaTipoExamenLaboratorio = new SelectList(getListaTipoExamenLaboratorio(), "idTipoExamen", "nomTipoExamen");
            var codusu = Session["UserID"];
            var util = new DatosGeneralesController().listadatogenerales().FirstOrDefault();



            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            var medicos = new MedicosController().ListadoMedico().Where(X => X.CodMed == exa.CodMed).FirstOrDefault();
            var pacientes = new PacientesController().ListadoPacientes().Where(x => x.Historia == exa.Historia).FirstOrDefault();

            string Crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
            string usuario = Session["UserID"].ToString();

            int Resu = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                    con.Open();
                    SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                    using (SqlCommand da = new SqlCommand("Usp_MtoCuentas", con, tr))
                    {
                        try
                        {
                            da.CommandType = CommandType.StoredProcedure;

                            if (exa.CodCue == 0)
                            {

                                da.Parameters.AddWithValue("@CodCue", "");
                                da.Parameters.AddWithValue("@CodSede", sede);
                                da.Parameters.AddWithValue("@Historia", exa.Historia);
                                da.Parameters.AddWithValue("@CodcatPac", pacientes.CodCatPac);
                                da.Parameters.AddWithValue("@STotCue", exa.precio);
                                da.Parameters.AddWithValue("@IgvCue", exa.igv);
                                da.Parameters.AddWithValue("@TotCue", exa.total);
                                da.Parameters.AddWithValue("@FecCrea", "");
                                da.Parameters.AddWithValue("@FecAnul", "");
                                da.Parameters.AddWithValue("@EstCue", "1");
                                da.Parameters.AddWithValue("@EstGene", "");
                                da.Parameters.AddWithValue("@SecFact", "");
                                da.Parameters.AddWithValue("@Usuario", usuario);
                                da.Parameters.AddWithValue("@UsuarioAnula", "");
                                da.Parameters.AddWithValue("@Crea", Crea);
                                da.Parameters.AddWithValue("@Modifica", "");
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", "1");

                            }
                            else
                            {




                                da.Parameters.AddWithValue("@CodCue", exa.CodCue);
                                da.Parameters.AddWithValue("@CodSede", "");
                                da.Parameters.AddWithValue("@Historia", "");
                                da.Parameters.AddWithValue("@CodcatPac", "");
                                da.Parameters.AddWithValue("@STotCue", exa.precio);
                                da.Parameters.AddWithValue("@IgvCue", exa.igv);
                                da.Parameters.AddWithValue("@TotCue", exa.total);
                                da.Parameters.AddWithValue("@FecCrea", "");
                                da.Parameters.AddWithValue("@FecAnul", "");
                                da.Parameters.AddWithValue("@EstCue", "1");
                                da.Parameters.AddWithValue("@EstGene", "");
                                da.Parameters.AddWithValue("@SecFact", "");
                                da.Parameters.AddWithValue("@Usuario", "");
                                da.Parameters.AddWithValue("@UsuarioAnula", "");
                                da.Parameters.AddWithValue("@Crea", "");
                                da.Parameters.AddWithValue("@Modifica", Crea);
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", "2");

                            }

                            Resu = (int)da.ExecuteScalar();


                            using (SqlCommand dd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr))
                            {
                                dd.CommandType = CommandType.StoredProcedure;
                                dd.Parameters.AddWithValue("@Procedencia", 1);
                                dd.Parameters.AddWithValue("@CodCue", Resu);
                                if (exa.CodCue == 0)
                                {
                                    dd.Parameters.AddWithValue("@Item", 1);
                                }
                                else
                                {
                                    dd.Parameters.AddWithValue("@Item", 1);
                                }
                                dd.Parameters.AddWithValue("@Tarifa", exa.CodTar);
                                dd.Parameters.AddWithValue("@CodProce", 3);
                                dd.Parameters.AddWithValue("@CodDetalleP", 3);
                                dd.Parameters.AddWithValue("@CodSede", sede);
                                dd.Parameters.AddWithValue("@Cantidad", 1);
                                dd.Parameters.AddWithValue("@precioUni", exa.total);
                                dd.Parameters.AddWithValue("@precio", exa.precio);
                                dd.Parameters.AddWithValue("@igv", exa.igv);
                                dd.Parameters.AddWithValue("@total", exa.total);
                                dd.Parameters.AddWithValue("@EstDet", "1");
                                dd.Parameters.AddWithValue("@FechaAten", "");
                                dd.Parameters.AddWithValue("@TurnoAten", "");
                                dd.Parameters.AddWithValue("@RegMedico", "");
                                dd.Parameters.AddWithValue("@MedicoEnvia", "");
                                dd.Parameters.AddWithValue("@Crea", Crea);
                                dd.Parameters.AddWithValue("@Modifica", "");
                                dd.Parameters.AddWithValue("@Elimina", "");
                                dd.Parameters.AddWithValue("@Evento", "1");

                                dd.ExecuteNonQuery();

                                tr.Commit();




                                using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_Examen_Laboratorio", con))
                                {
                                    try
                                    {
                                        cmd.Parameters.AddWithValue("@IdExamen", exa.idExamen);
                                        cmd.Parameters.AddWithValue("@IdTipoExamen", exa.idTipoExamen);
                                        cmd.Parameters.AddWithValue("@Resultado", exa.resultado);
                                        cmd.Parameters.AddWithValue("@Fecha", "");
                                        cmd.Parameters.AddWithValue("@Crea", Crea);
                                        cmd.Parameters.AddWithValue("@Modifica", "");
                                        cmd.Parameters.AddWithValue("@Elimina", "");
                                        cmd.Parameters.AddWithValue("@Estado", true);
                                        cmd.Parameters.AddWithValue("@CodCue", Resu);
                                        cmd.Parameters.AddWithValue("@Evento", 1);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.ExecuteNonQuery();


                                        ViewBag.Mensaje = "Se registro Satisfactoriamente";
                                    }
                                    catch (Exception ex)
                                    {
                                        ViewBag.mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                                        return View(exa);
                                    }







                                }




                                if (util.GENERARCUENTAAUTO)
                                {
                                    return RedirectPermanent("~/Caja/RegistrarCaja?id=" + Resu);
                                }
                                else
                                {
                                    if (util.PREGXATENCIONPROGRAMADAS)
                                    {
                                        ViewBag.activaAlerta = 1;
                                    }
                                    else
                                    {
                                        return RedirectPermanent("~/Cuentas/VerificaCuenta/" + Resu);
                                    }
                                }


                                ViewBag.historia = exa.Historia;
                                ViewBag.cuenta = Resu;
                            }

                        }
                        catch (Exception e)
                        {
                            tr.Rollback();

                        }
                    }
                }






            }
            catch (Exception e)
            {
                return RedirectPermanent("../Master");
            }








            return RedirectToAction("../Pacientes/ListaPacientes");
        }




        public ActionResult ListaTipoExamenLaboratorio()
        {
            return View(getListaTipoExamenLaboratorio());

        }
        public List<E_Tipo_Examen_Laboratorio> getListaTipoExamenLaboratorio()
        {
            List<E_Tipo_Examen_Laboratorio> lista = new List<E_Tipo_Examen_Laboratorio>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Tipo_Examen", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        E_Tipo_Examen_Laboratorio tip = new E_Tipo_Examen_Laboratorio();
                        tip.idTipoExamen = Convert.ToInt32(dr["IdTipoExamen"]);
                        tip.nomTipoExamen = dr["NomTipoExamen"].ToString();
                        lista.Add(tip);
                    }
                    con.Close();
                }
            }
            return lista;
        }

        public JsonResult VerPrecios(string codtar, string codpac)
        {

            var paciente = new PacientesController().ListadoPacientes().Where(x => x.Historia == Convert.ToInt32(codpac)).FirstOrDefault();
            var evalua = tar.ListadoCategoriaPacienteTarifa(codtar).Find(x => x.CodCatPac == paciente.CodCatPac);
            var evalua1 = tar.ListadoTarifa().Find(x => x.CodTar == codtar);

            E_Tarifario tari = new E_Tarifario();
            if (tari != null)
            {
                if (evalua1.AfecIgcv == true)
                {
                    DatosGeneralesController da = new DatosGeneralesController();
                    E_Datos_Generales dat = da.listadatogenerales().FirstOrDefault();
                    tari.Precio = decimal.Round(evalua.Precio / (dat.igv + 1), 2);
                    var result = ((evalua.Precio / decimal.Parse("1.18")) * dat.igv);
                    tari.igv = decimal.Round(result, 2);
                    tari.total = decimal.Round(tari.Precio + tari.igv, 2);

                }
                else
                {
                    tari.Precio = evalua.Precio;
                    tari.igv = 0;
                    tari.total = evalua.Precio;
                }
            }
            else
            {
                tari.Precio = 0;
                tari.igv = 0;
                tari.total = 0;
            }

            tari.CodTipTar = evalua1.CodTipTar;
            tari.TiempoApox = evalua1.TiempoApox;

            List<E_Tarifario> lista = new List<E_Tarifario>();
            lista.Add(tari);

            if (lista.Count() != 0)
            {
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            return null;

        }

        public ActionResult VISTA()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VISTA(E_Examen_Laboratorio exa)
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cn1"].ConnectionString))
            {

               con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Mantenimiento_Examen_Laboratorio", con))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@IdExamen", exa.idExamen);
                        cmd.Parameters.AddWithValue("@IdTipoExamen", exa.idTipoExamen);
                        cmd.Parameters.AddWithValue("@Resultado", exa.resultado);
                        cmd.Parameters.AddWithValue("@Fecha", "");
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", "");
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Estado", true);
                        cmd.Parameters.AddWithValue("@CodCue", "");
                        cmd.Parameters.AddWithValue("@Evento", 1);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();


                        ViewBag.Mensaje = "Se registro Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.mensaje = "Ocurrio algun error al registrar : " + ex.Message.ToString();
                        return View(exa);
                    }
                }
                return RedirectToAction("VISTA");
            }
        }
    }
}




