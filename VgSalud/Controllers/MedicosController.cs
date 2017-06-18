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
    public class MedicosController : Controller
    {

        public ActionResult EliminarT(string id)
        {
            var formularios = (List<E_Medico>)Session["pagoTarMed"];

            var registro = formularios.Where(x => x.codTipTar.Equals(id)).FirstOrDefault();
            formularios.Remove(registro);

            Session["atenciones"] = formularios;
            ViewBag.pago = (List<E_Medico>)Session["pagoTarMed"];
            ViewBag.abreModal = "1";

            return RedirectToAction("VerPrecios");
        }



        public ActionResult RegistrarMedico()
        {

            TipoTarifaController ttap = new TipoTarifaController();
            ViewBag.abreModal = "";
            //if (id1 == null && cadena == null) {

            if (Session["pagoTarMed"] == null)
            {
                Session["pagoTarMed"] = new List<E_Medico>();
                var formularios = (List<E_Medico>)Session["pagoTarMed"];

                var listado = ttap.ListadoTipoTarifa().Where(x => x.EstTipTar == true).ToList();

                foreach (var i in listado)
                {
                    E_Medico med = new E_Medico();
                    med.codTipTar = i.CodTipTar;
                    med.TipoTarifa = i.DescTipTar;
                    med.porcentaje = 0;
                    formularios.Add(med);
                    Session["pagoTarMed"] = formularios;
                }

            }
            else
            {
                Session.Remove("pagoTarMed");
                Session["pagoTarMed"] = new List<E_Medico>();
                var formularios = (List<E_Medico>)Session["pagoTarMed"];

                var listado = ttap.ListadoTipoTarifa().Where(x => x.EstTipTar == true).ToList();

                foreach (var i in listado)
                {
                    E_Medico med = new E_Medico();
                    med.codTipTar = i.CodTipTar;
                    med.TipoTarifa = i.DescTipTar;
                    med.porcentaje = 0;
                    formularios.Add(med);
                    Session["pagoTarMed"] = formularios;
                }

            }

            string sede = Session["codSede"].ToString();


            ViewBag.ListaTipoTarifa = new SelectList(ttap.ListadoTipoTarifa().Where(x => x.EstTipTar == true).ToList(), "CodTipTar", "DescTipTar");

            ServiciosController Ser = new ServiciosController();
            ViewBag.ListaServicios = new SelectList(Ser.ListadoServicios().Where(x => x.EstServ == true && x.CodSede == sede).ToList(), "CodServ", "NomServ");

            EspecialidadController Espe = new EspecialidadController();
            ViewBag.ListaEspecialidad = new SelectList(Espe.ListadoEspecialidades().Where(x => x.EstEspec == true).ToList(), "CodEspec", "NomEspec");

            EmpresaTerceroController EmpTer = new EmpresaTerceroController();
            ViewBag.ListaEmpresaTercero = new SelectList(EmpTer.ListadoEmpresaTercero(sede).Where(x => x.EstEmp == true && x.CodSede == sede).ToList(), "CodEmp", "RazonEmp");

            ViewBag.ListaProfesion = new SelectList(ListadoProfesion().Where(x => x.EstaProfe == true).ToList(), "TipPrfMed", "TipPrfMed");

            SedesController Sed = new SedesController();
            ViewBag.ListaSedes = new SelectList(Sed.ListadoSedes(), "CodSede", "NomSede");
            ViewBag.Sedes = (List<E_Sede>)Sed.ListadoSedes();

            PacientesController Pac = new PacientesController();
            ViewBag.ListaUsuario = new SelectList(Pac.ListadoUsuarioPaciente(), "CodigoUsuario", "AliasUsu");

            ViewBag.pago = (List<E_Medico>)Session["pagoTarMed"];

            return View();

        }

        [HttpPost]
        public ActionResult RegistrarMedico(E_Medico EMed)
        {
            if (EMed.CrearUsu == true) {
                ViewBag.alias = EMed.Alias;
            }
           
            if (EMed.ObservMed != null)
            {
                ViewBag.observacion = EMed.ObservMed.ToUpper();
            }
            
            string sede = Session["codSede"].ToString();

            ViewBag.PagoTurno = EMed.PagoTurno;
            ViewBag.serv = EMed.CodServ;
            TipoTarifaController ttap = new TipoTarifaController();
            ViewBag.ListaTipoTarifa = new SelectList(ttap.ListadoTipoTarifa().Where(x => x.EstTipTar == true).ToList(), "CodTipTar", "DescTipTar", EMed.codTipTar);

            ServiciosController Ser = new ServiciosController();
            ViewBag.ListaServicios = new SelectList(Ser.ListadoServicios().Where(x => x.EstServ == true && x.CodSede == sede), "CodServ", "NomServ", EMed.CodServ);

            EspecialidadController Espe = new EspecialidadController();
            ViewBag.ListaEspecialidad = new SelectList(Espe.ListadoEspecialidades(), "CodEspec", "NomEspec", EMed.CodEspec);

            EmpresaTerceroController EmpTer = new EmpresaTerceroController();
            ViewBag.ListaEmpresaTercero = new SelectList(EmpTer.ListadoEmpresaTercero(sede).Where(x => x.EstEmp == true && x.CodSede == sede), "CodEmp", "RazonEmp", EMed.CodEmp);

            ViewBag.ListaProfesion = new SelectList(ListadoProfesion(), "TipPrfMed", "TipPrfMed", EMed.TipPrfMed);

            SedesController Sed = new SedesController();
            ViewBag.ListaSedes = new SelectList(Sed.ListadoSedes(), "CodSede", "NomSede", EMed.CodSede);

            PacientesController Pac = new PacientesController();
            ViewBag.ListaUsuario = new SelectList(Pac.ListadoUsuarioPaciente(), "CodigoUsuario", "AliasUsu", EMed.CodUsu);

            ViewBag.pago = (List<E_Medico>)Session["pagoTarMed"];

            string CodSedeLog = (string)Session["CodSede"];
            string Crea = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            if (EMed.evento == 1)
            {
                string CodUsu = ""; 
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                    using (SqlCommand cmd = new SqlCommand("Usp_MtoMedicos", con, tr))
                    {
                        try
                        {
                            if (EMed.CrearUsu)
                            {
                                UtilitarioController ut = new UtilitarioController();
                                var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
                                string crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
                                MasterController ma = new MasterController();
                                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Usuario", con, tr))
                                {
                                    try
                                    {
                                        string encripta = ma.EncryptKey(EMed.DniMed);
                                        da.CommandType = CommandType.StoredProcedure;
                                        da.Parameters.AddWithValue("@CodUsu", "");
                                        da.Parameters.AddWithValue("@AliasUsu", EMed.Alias);
                                        da.Parameters.AddWithValue("@PwdUsu", encripta);
                                        da.Parameters.AddWithValue("@DniUsu", EMed.DniMed);
                                        da.Parameters.AddWithValue("@ApePaterUsu", ".");
                                        da.Parameters.AddWithValue("ApeMaterUsu", ".");
                                        da.Parameters.AddWithValue("@NomUsu", EMed.NomMed);
                                        da.Parameters.AddWithValue("@FecNacUsu", "");
                                        da.Parameters.AddWithValue("@EstUsu", EMed.EstMed);
                                        da.Parameters.AddWithValue("@Crea", crea);
                                        da.Parameters.AddWithValue("@Modifica", "");
                                        da.Parameters.AddWithValue("@Elimina", "");
                                        da.Parameters.AddWithValue("@Tipo", "1");
                                        CodUsu=da.ExecuteScalar().ToString();
                                    }
                                    catch (Exception e)
                                    {
                                        ViewBag.Mensaje = "3";
                                        tr.Rollback();

                                        return View(EMed);
                                    }
                                }
                                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_UsuarioSede", con,tr))
                                {
                                    try
                                    {
                                        da.CommandType = CommandType.StoredProcedure;
                                        da.Parameters.AddWithValue("@CodUsu", CodUsu);
                                        da.Parameters.AddWithValue("@CodSede",sede);
                                        da.Parameters.AddWithValue("@Tipo", "1");


                                        var result = (int)da.ExecuteScalar();
                                        if (result == 0)
                                        {
                                            ViewBag.mensaje = "Error: Ya se encuentra asignado el Usuario";
                                            tr.Rollback();
                                            return View(EMed);
                                        }
                                        else
                                        {
                                            ViewBag.success = "Se Registro Correctamente";
                                        }

                                    }
                                    catch (Exception e)
                                    {
                                        ViewBag.mensaje = "Error: " + e.Message;
                                        return View(EMed);
                                    }
                                   
                                }
                                using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Usuario_Servicio", con,tr))
                                {
                                    try
                                    {
                                        da.CommandType = CommandType.StoredProcedure;
                                        da.Parameters.AddWithValue("@codUsu", CodUsu);
                                        da.Parameters.AddWithValue("@codSer",EMed.CodServ);
                                        da.Parameters.AddWithValue("@Tipo", "1");
                                        var result = (int)da.ExecuteScalar();
                                        if (result == 0)
                                        {
                                            ViewBag.mensaje = "Error el Usuario ya se encuentra Asignado";
                                            tr.Rollback();
                                            return View(EMed);
                                        }
                                        else
                                        {
                                            ViewBag.success = "Se Registro con exito";
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        ViewBag.mensaje = "Error: " + e.Message;
                                        tr.Rollback();
                                        return View(EMed);
                                    }
                                    
                                }
                            }

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CodMed", "");
                            cmd.Parameters.AddWithValue("@NomMed", EMed.NomMed.ToUpper());
                            cmd.Parameters.AddWithValue("@DniMed", EMed.DniMed.ToUpper());
                            if (EMed.TipPrfMed == null) { cmd.Parameters.AddWithValue("@TipPrfMed", ""); } else { cmd.Parameters.AddWithValue("@TipPrfMed", EMed.TipPrfMed); }
                            if (EMed.ColgMed == null) { cmd.Parameters.AddWithValue("@ColgMed", ""); } else { cmd.Parameters.AddWithValue("@ColgMed", EMed.ColgMed); }
                            if (EMed.RneMed == null) { cmd.Parameters.AddWithValue("@RneMed", ""); } else { cmd.Parameters.AddWithValue("@RneMed", EMed.RneMed); }
                            cmd.Parameters.AddWithValue("@DireccMed", EMed.DireccMed.ToUpper());
                            cmd.Parameters.AddWithValue("@FecIngMed", DateTime.Now);
                            cmd.Parameters.AddWithValue("@TelfMed", EMed.TelfMed);
                            if (EMed.ObservMed == null) { cmd.Parameters.AddWithValue("@ObservMed", " "); } else { cmd.Parameters.AddWithValue("@ObservMed", EMed.ObservMed.Trim()); }

                            cmd.Parameters.AddWithValue("@CodServ", EMed.CodServ);
                            cmd.Parameters.AddWithValue("@CodEspec", EMed.CodEspec);
                            cmd.Parameters.AddWithValue("@CodEmp", EMed.CodEmp);
                            if (EMed.CodUsu == null) { cmd.Parameters.AddWithValue("@CodUsu",CodUsu); } else { cmd.Parameters.AddWithValue("@CodUsu", EMed.CodUsu); }

                            cmd.Parameters.AddWithValue("@CodSede", CodSedeLog);
                            cmd.Parameters.AddWithValue("@EstMed", EMed.EstMed);
                            cmd.Parameters.AddWithValue("@PagoTurno", EMed.PagoTurno);
                            cmd.Parameters.AddWithValue("@EnLista", EMed.EnLista);
                            cmd.Parameters.AddWithValue("@EjecFichaElec", EMed.EjecFichaElec);
                            cmd.Parameters.AddWithValue("@abreviatura", EMed.abreviatura);
                            cmd.Parameters.AddWithValue("@Crea", Crea);
                            cmd.Parameters.AddWithValue("@Modifica", "");
                            cmd.Parameters.AddWithValue("@Elimina", "");
                            cmd.Parameters.AddWithValue("@Evento", 1);
                            string Medico = (string)cmd.ExecuteScalar();
                            foreach (E_Medico it in (List<E_Medico>)Session["pagoTarMed"])
                            {

                                SqlCommand da = new SqlCommand("usp_Mantenimiento_Medico_tipotarifa", con, tr);
                                da.CommandType = CommandType.StoredProcedure;
                                da.Parameters.AddWithValue("@CodTipTar", it.codTipTar);
                                da.Parameters.AddWithValue("@CodMed", Medico);
                                da.Parameters.AddWithValue("@porcentaje", it.porcentaje);
                                da.Parameters.AddWithValue("@Crea", Crea);
                                da.Parameters.AddWithValue("@Modifica", "");
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", "1");
                                da.ExecuteNonQuery();
                                ViewBag.Mensaje = "Se actualizo Satisfactoriamente";
                            }
                            tr.Commit();
                            ViewBag.Mensaje = "Se registro Satisfactoriamente";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Mensaje = "3";
                            tr.Rollback();
                           
                            return View(EMed);
                        }
                        finally
                        {

                            con.Close();
                        }
                    }

                    Session.Remove("pagoTarMed");
                    return RedirectToAction("ListarMedico");
                }

            }
            else if (EMed.evento == 2)
            {
                var formularios = (List<E_Medico>)Session["pagoTarMed"];

                var registro = formularios.Where(x => x.codTipTar.Equals(EMed.codTipTar)).FirstOrDefault();

                if (registro == null)
                {

                    E_Tipo_Tarifa listado = ttap.ListadoTipoTarifa().Find(x => x.CodTipTar == EMed.codTipTar);

                    E_Medico med = new E_Medico();
                    med.codTipTar = EMed.codTipTar;
                    med.TipoTarifa = listado.DescTipTar;
                    med.porcentaje = EMed.porcentaje;
                    formularios.Add(med);
                    Session["pagoTarMed"] = formularios;
                    ViewBag.abreModal = "1";

                }
                else
                {
                    var formulario = (List<E_Medico>)Session["pagoTarMed"];
                    var index = formulario.Where(x => x.codTipTar == EMed.codTipTar).FirstOrDefault();
                    formulario.Remove(index);
                    E_Medico NewMedico = new E_Medico();
                    TipoTarifaController tar = new TipoTarifaController();
                    var TT = tar.ListadoTipoTarifa().Where(x => x.CodTipTar == EMed.codTipTar).FirstOrDefault();
                    NewMedico.codTipTar = EMed.codTipTar;
                    NewMedico.TipoTarifa = TT.DescTipTar;
                    NewMedico.porcentaje = EMed.porcentaje;
                    formulario.Add(NewMedico);
                    Session["pagoTarMed"] = formulario;
                    ViewBag.abreModal = "1";
                    tar = null;

                }

                ViewBag.pago = (List<E_Medico>)Session["pagoTarMed"];

                return View(EMed);

            }
            else if (EMed.evento == 3)
            {

                var formularios = (List<E_Medico>)Session["pagoTarMed"];

                var registro = formularios.Where(x => x.codTipTar.Equals(EMed.codTipTarElimina)).FirstOrDefault();
                formularios.Remove(registro);
                Session["pagoTarMed"] = formularios;
                ViewBag.pago = (List<E_Medico>)Session["pagoTarMed"];
                ViewBag.abreModal = "1";

                return View(EMed);

            }
            return View(EMed);

        }


        public ActionResult Eliminar(E_Medico Emed)
        {

            string elimina = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Mantenimiento_Medico_tipotarifa", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodTipTar", Emed.codTipTar);
                        cmd.Parameters.AddWithValue("@CodMed", Emed.CodMed);
                        cmd.Parameters.AddWithValue("@porcentaje", "");
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", "");
                        cmd.Parameters.AddWithValue("@Elimina", elimina);
                        cmd.Parameters.AddWithValue("@Evento", 3);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se actualizo Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al registrar : ";
                        return View(Emed);
                    }
                    finally
                    {
                        con.Close();

                    }
                }


            }


            return RedirectPermanent("ModificarMedico?id=" + Emed.CodMed);

        }






        public ActionResult ModificarMedico(string Id)
        {
            string sede = Session["codSede"].ToString();

            EspecialidadController Espe = new EspecialidadController();
            MedicoTipoTarifaController medTipTap = new MedicoTipoTarifaController();
            var tarifario = new TarifarioController().ListadoTarifa();
            ViewBag.tarifario = new SelectList(tarifario, "CodTar", "DescTar");
            ViewBag.TarifaMedico = Usp_ListaGeneral_Tarifa_Medico();
            TipoTarifaController ttap = new TipoTarifaController();
            ViewBag.ListaTipoTarifa = new SelectList(ttap.ListadoTipoTarifa().Where(x => x.EstTipTar == true).ToList(), "CodTipTar", "DescTipTar");
            ViewBag.listaMedTip = medTipTap.listamedicotarifas().Where(x => x.CodMed == Id).ToList();

            ServiciosController Ser = new ServiciosController();
            ViewBag.ListaServicios = new SelectList(Ser.ListadoServicios().Where(x => x.EstServ == true && x.CodSede == sede), "CodServ", "NomServ");


            ViewBag.ListaEspecialidad = new SelectList(Espe.ListadoEspecialidades(), "CodEspec", "NomEspec");

            EmpresaTerceroController EmpTer = new EmpresaTerceroController();
            ViewBag.ListaEmpresaTercero = new SelectList(EmpTer.ListadoEmpresaTercero(sede).Where(x => x.EstEmp == true && x.CodSede == sede), "CodEmp", "RazonEmp");

            ViewBag.ListaProfesion = new SelectList(ListadoProfesion().Where(x => x.EstaProfe == true).ToList(), "TipPrfMed", "TipPrfMed");

            SedesController Sed = new SedesController();
            ViewBag.ListaSedes = new SelectList(Sed.ListadoSedes(), "CodSede", "NomSede");

            ViewBag.Sedes = (List<E_Sede>)Sed.ListadoSedes();

            PacientesController Pac = new PacientesController();
            ViewBag.ListaUsuario = new SelectList(Pac.ListadoUsuarioPaciente(), "CodigoUsuario", "AliasUsu");

            var lista = (from x in ListadoMedico() where x.CodMed == Id select x).FirstOrDefault();

            E_Especialidades reg = Espe.ListadoEspecialidades().Find(x => x.CodEspec == lista.CodEspec);
            ViewBag.nombreEspe = reg.NomEspec;
            E_Empresa_Tercero EmpT = EmpTer.ListadoEmpresaTerceroSinSede().Find(x => x.CodEmp == lista.CodEmp);
            ViewBag.NomEmpresa = EmpT.RazonEmp;

            return View(lista);

        }

        [HttpPost]
        public ActionResult ModificarMedico(E_Medico EMed)
        {
            string sede = Session["codSede"].ToString();
            ViewBag.TarifaMedico = Usp_ListaGeneral_Tarifa_Medico();
            var tarifario = new TarifarioController().ListadoTarifa();
            ViewBag.tarifario = new SelectList(tarifario, "CodTar", "DescTar");
            TipoTarifaController ttap = new TipoTarifaController();

            ViewBag.ListaTipoTarifa = new SelectList(ttap.ListadoTipoTarifa().Where(x => x.EstTipTar == true).ToList(), "CodTipTar", "DescTipTar");
            MedicoTipoTarifaController medTipTap = new MedicoTipoTarifaController();
            ViewBag.listaMedTip = medTipTap.listamedicotarifas().Where(x => x.CodMed == EMed.CodMed).ToList();

            ServiciosController Ser = new ServiciosController();
            ViewBag.ListaServicios = new SelectList(Ser.ListadoServicios().Where(x => x.CodSede == sede), "CodServ", "NomServ", EMed.CodServ);

            EspecialidadController Espe = new EspecialidadController();
            ViewBag.ListaEspecialidad = new SelectList(Espe.ListadoEspecialidades(), "CodEspec", "NomEspec", EMed.CodEspec);

            EmpresaTerceroController EmpTer = new EmpresaTerceroController();
            ViewBag.ListaEmpresaTercero = new SelectList(EmpTer.ListadoEmpresaTercero(sede), "CodEmp", "RazonEmp", EMed.CodEmp);

            ViewBag.ListaProfesion = new SelectList(ListadoProfesion(), "TipPrfMed", "TipPrfMed", EMed.TipPrfMed);

            SedesController Sed = new SedesController();
            ViewBag.ListaSedes = new SelectList(Sed.ListadoSedes(), "CodSede", "NomSede", EMed.CodSede);

            PacientesController Pac = new PacientesController();
            ViewBag.ListaUsuario = new SelectList(Pac.ListadoUsuarioPaciente(), "CodigoUsuario", "AliasUsu", EMed.CodUsu);

            string CodSedeLog = (string)Session["CodSede"];
            string Modificar = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            var lista = (from x in ListadoMedico() where x.CodMed == EMed.CodMed select x).FirstOrDefault();

            E_Especialidades reg = Espe.ListadoEspecialidades().Find(x => x.CodEspec == lista.CodEspec);
            ViewBag.nombreEspe = reg.NomEspec;
            E_Empresa_Tercero EmpT = EmpTer.ListadoEmpresaTercero(sede).Find(x => x.CodEmp == lista.CodEmp);
            ViewBag.NomEmpresa = EmpT.RazonEmp;

            if (EMed.evento == 1)
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    SqlCommand da = new SqlCommand("usp_Mantenimiento_Medico_tipotarifa", con);
                    da.CommandType = CommandType.StoredProcedure;
                    da.Parameters.AddWithValue("@CodTipTar", EMed.codTipTar);
                    da.Parameters.AddWithValue("@CodMed", EMed.CodMed);
                    da.Parameters.AddWithValue("@porcentaje", EMed.porcentaje);
                    da.Parameters.AddWithValue("@Crea", Modificar);
                    da.Parameters.AddWithValue("@Modifica", "");
                    da.Parameters.AddWithValue("@Elimina", "");
                    da.Parameters.AddWithValue("@Evento", "4");
                    da.ExecuteNonQuery();
                  

                }
                ViewBag.listaMedTip = medTipTap.listamedicotarifas().Where(x => x.CodMed == EMed.CodMed).ToList();
                //var medico = ListadoMedico().Where(x => x.CodMed == EMed.CodMed).FirstOrDefault(); 
                ViewBag.mensaje = "2";
                return View(EMed);
            }

            else if (EMed.evento == 2)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                    {
                        con.Open();
                        SqlCommand da = new SqlCommand("Usp_Mantenimiento_TarifaMedicoPorcentaje", con);
                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@CodTar", EMed.CodTar);
                        da.Parameters.AddWithValue("@CodMed", EMed.CodMed);
                        da.Parameters.AddWithValue("@porcentaje", EMed.porcentaje1);
                        da.Parameters.AddWithValue("@Evento", 1);
                        da.ExecuteNonQuery();
                        ViewBag.mensaje = "1";
                    }
                }
                catch (Exception)
                {
                    ViewBag.mensaje = "3";
                }

                ViewBag.modal = "show";
                ViewBag.TarifaMedico = Usp_ListaGeneral_Tarifa_Medico().Where(x => x.CodMed == EMed.CodMed).ToList();

                return View(EMed);
            }
            else if (EMed.evento == 3)
            {

                string elimina = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_Mantenimiento_Medico_tipotarifa", con))
                    {
                        try
                        {

                            cmd.Parameters.AddWithValue("@CodTipTar", EMed.CodTipTar1);
                            cmd.Parameters.AddWithValue("@CodMed", EMed.CodMed);
                            cmd.Parameters.AddWithValue("@porcentaje", 0);
                            cmd.Parameters.AddWithValue("@Crea", "");
                            cmd.Parameters.AddWithValue("@Modifica", "");
                            cmd.Parameters.AddWithValue("@Elimina", elimina);
                            cmd.Parameters.AddWithValue("@Evento", 3);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            ViewBag.Mensaje = "2";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Mensaje = "3";
                            return View(EMed);
                        }
                        finally
                        {
                            con.Close();

                        }
                    }


                }
                ViewBag.listaMedTip = medTipTap.listamedicotarifas().Where(x => x.CodMed == EMed.CodMed).ToList();
                return View(EMed);

            }
            else if (EMed.evento == 4)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                    {
                        con.Open();
                        SqlCommand da = new SqlCommand("Usp_Mantenimiento_TarifaMedicoPorcentaje", con);
                        da.CommandType = CommandType.StoredProcedure;
                        da.Parameters.AddWithValue("@CodTar", EMed.CodTar1);
                        da.Parameters.AddWithValue("@CodMed", EMed.CodMed);
                        da.Parameters.AddWithValue("@porcentaje", 0);
                        da.Parameters.AddWithValue("@Evento", 2);
                        da.ExecuteNonQuery();
                        ViewBag.mensaje = "1";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.mensaje = "3";
                }

                ViewBag.modal = "show";
                ViewBag.TarifaMedico = Usp_ListaGeneral_Tarifa_Medico().Where(x => x.CodMed == EMed.CodMed).ToList();
                return View(EMed);
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoMedicos", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodMed", EMed.CodMed);
                        cmd.Parameters.AddWithValue("@NomMed", EMed.NomMed.ToUpper());
                        cmd.Parameters.AddWithValue("@DniMed", EMed.DniMed.ToUpper());
                        if (EMed.TipPrfMed == null) { cmd.Parameters.AddWithValue("@TipPrfMed", ""); } else { cmd.Parameters.AddWithValue("@TipPrfMed", EMed.TipPrfMed); }
                        if (EMed.ColgMed == null) { cmd.Parameters.AddWithValue("@ColgMed", ""); } else { cmd.Parameters.AddWithValue("@ColgMed", EMed.ColgMed); }
                        if (EMed.RneMed == null) { cmd.Parameters.AddWithValue("@RneMed", ""); } else { cmd.Parameters.AddWithValue("@RneMed", EMed.RneMed); }
                        cmd.Parameters.AddWithValue("@DireccMed", EMed.DireccMed.ToUpper());
                        cmd.Parameters.AddWithValue("@FecIngMed", DateTime.Now);
                        cmd.Parameters.AddWithValue("@TelfMed", EMed.TelfMed);
                        if (EMed.ObservMed == null)
                        {
                            cmd.Parameters.AddWithValue("@ObservMed", " ");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@ObservMed", EMed.ObservMed.Trim());
                        }
                        cmd.Parameters.AddWithValue("@CodServ", EMed.CodServ);
                        cmd.Parameters.AddWithValue("@CodEspec", EMed.CodEspec);
                        cmd.Parameters.AddWithValue("@CodEmp", EMed.CodEmp);
                        if (EMed.CodUsu == null) { cmd.Parameters.AddWithValue("@CodUsu", ""); } else { cmd.Parameters.AddWithValue("@CodUsu", EMed.CodUsu); }

                        cmd.Parameters.AddWithValue("@CodSede", CodSedeLog);
                        cmd.Parameters.AddWithValue("@EstMed", EMed.EstMed);
                        cmd.Parameters.AddWithValue("@PagoTurno", EMed.PagoTurno);
                        cmd.Parameters.AddWithValue("@EnLista", EMed.EnLista);
                        cmd.Parameters.AddWithValue("@EjecFichaElec", EMed.EjecFichaElec);
                        cmd.Parameters.AddWithValue("@abreviatura", EMed.abreviatura);
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", Modificar);
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Evento", 2);
                        cmd.CommandType = CommandType.StoredProcedure;

                        int fila = (int)cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        //ViewBag.Mensaje = "Se Modifico Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.mensaje = "3";

                    }
                    finally
                    {
                        ViewBag.mensaje = "2";
                        con.Close();
                    }
                }
                return RedirectToAction("ListarMedico");
            }

        }


        public ActionResult ListarMedico()
        {
            string IdSede = (string)Session["CodSede"];
            return View(ListadoMedico().Where(y => y.CodSede == IdSede));

        }

        public List<E_Medico> ListadoMedico()
        {
            List<E_Medico> Lista = new List<E_Medico>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Medicos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Medico Med = new E_Medico();

                            Med.CodMed = dr.GetString(0);
                            Med.NomMed = dr.GetString(1);
                            Med.DniMed = dr.GetString(2);
                            Med.TipPrfMed = dr.GetString(3);
                            Med.ColgMed = dr.GetString(4);
                            Med.RneMed = dr.GetString(5);
                            Med.DireccMed = dr.GetString(6);
                            Med.FecIngMed = dr.GetDateTime(7);
                            Med.TelfMed = dr.GetString(8);
                            Med.ObservMed = dr.GetString(9).Trim();
                            Med.CodServ = dr.GetString(10);
                            Med.CodEspec = dr.GetString(11);
                            Med.CodEmp = dr.GetString(12);
                            Med.CodSede = dr.GetString(13);
                            Med.EstMed = dr.GetBoolean(14);
                            Med.Servicio = dr.GetString(15);
                            Med.Especialidad = dr.GetString(16);
                            Med.empresa = dr.GetString(17);
                            Med.PagoTurno = dr["PagoTurno"] is DBNull ? 0 : dr.GetDecimal(18);
                            Med.EnLista = dr.GetBoolean(19);
                            Med.EjecFichaElec = dr.GetBoolean(20);
                            Med.abreviatura = dr["abreviatura"] is DBNull ? "" : dr.GetString(21);
                            Med.CodUsu = dr["CodUsu"] is DBNull ? "" : dr.GetString(22);

                            Lista.Add(Med);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }



        public ActionResult ListarProfesion()
        {

            return View(ListadoProfesion());

        }

        public List<E_Profesion> ListadoProfesion()
        {
            List<E_Profesion> Lista = new List<E_Profesion>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Profesion", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Profesion Prof = new E_Profesion();

                            Prof.TipPrfMed = dr.GetString(0);
                            Prof.DescProfe = dr.GetString(1);
                            Prof.EstaProfe = dr.GetBoolean(2);



                            Lista.Add(Prof);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_HorarioMedico> ListadoHorarioMedicoBusca(int mes, int anio)
        {
            List<E_HorarioMedico> Lista = new List<E_HorarioMedico>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ListadoHorarioMedicoBusca", con))
                {
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_HorarioMedico hor = new E_HorarioMedico();

                            hor.CodHor = dr.GetString(0);
                            hor.dia = dr.GetDateTime(1);
                            hor.horaInicio = dr.GetTimeSpan(2);
                            hor.horaFin = dr.GetTimeSpan(3);
                            hor.IntMin = dr.GetInt32(4);
                            hor.Turno = dr.GetString(5);
                            hor.CodMed = dr.GetString(6);
                            hor.Consultorio = dr.GetString(10);
                            hor.Estado = dr.GetBoolean(11);
                            hor.Asistencia = dr.GetString(12);
                            hor.NomCons = dr.GetString(13);
                            hor.nombreDia = dr.GetString(14);
                            Lista.Add(hor);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_HorarioMedico> ListadoHorarioMedico()
        {
            List<E_HorarioMedico> Lista = new List<E_HorarioMedico>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ListadoHorarioMedico", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_HorarioMedico hor = new E_HorarioMedico();

                            hor.CodHor = dr.GetString(0);
                            hor.dia = dr.GetDateTime(1);
                            hor.horaInicio = dr.GetTimeSpan(2);
                            hor.horaFin = dr.GetTimeSpan(3);
                            hor.IntMin = dr.GetInt32(4);
                            hor.Turno = dr.GetString(5);
                            hor.CodMed = dr.GetString(6);
                            hor.Consultorio = dr.GetString(10);
                            hor.Estado = dr.GetBoolean(11);
                            hor.Asistencia = dr.GetString(12);

                            Lista.Add(hor);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_HorarioMedico> BuscaHorarioMedico(string id)
        {
            List<E_HorarioMedico> Lista = new List<E_HorarioMedico>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_BuscaHorarioMedico", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodHor", id);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_HorarioMedico hor = new E_HorarioMedico();

                            hor.CodHor = dr.GetString(0);
                            hor.dia = dr.GetDateTime(1);
                            hor.horaInicio = dr.GetTimeSpan(2);
                            hor.horaFin = dr.GetTimeSpan(3);
                            hor.IntMin = dr.GetInt32(4);
                            hor.Turno = dr.GetString(5);
                            hor.CodMed = dr.GetString(6);
                            hor.Consultorio = dr.GetString(10);
                            hor.Estado = dr.GetBoolean(11);
                            hor.Asistencia = dr.GetString(12);

                            Lista.Add(hor);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }



        public ActionResult HorarioMedico(string id, string dd = null)
        {
            E_Medico reg = ListadoMedico().Find(x => x.CodMed.Equals(id));
            ViewBag.codnom = reg.CodMed + "-" + reg.NomMed;
            ConsultorioController con = new ConsultorioController();
            ViewBag.listadoConsultorio = new SelectList(con.ListadoConsultorio().Where(x => x.CodServ == reg.CodServ || x.Mixto == true).ToList(), "IdConsul", "DescConsul");

            UtilitarioController u = new UtilitarioController();
            var fechaSistema = u.ListadoHoraServidor().FirstOrDefault();
            DateTime fecha = fechaSistema.HoraServidor;
            DateTime fechaS = fechaSistema.HoraServidor;

            fechaS = fechaS.AddYears(1);
            string horaYa = fecha.ToString("mm:hh tt");
            string mes = fecha.ToString("MM");
            string anioActual = fecha.ToString("yyyy");
            string anioSiguiente = fechaS.ToString("yyyy");
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            int dias = DateTime.DaysInMonth(year, month);

            ViewBag.dias = dias;
            ViewBag.mes = month;
            ViewBag.anio = year;
            ViewBag.horaI = horaYa;
            ViewBag.horaF = horaYa;
            ViewBag.min = 10;
            ViewBag.turno = "nada";
            ViewBag.consultorio = "nada";

            if (dd != null)
            {
                string[] fija = dd.Split(',');
                int i = 1;
                foreach (string item in fija)
                {
                    if (i == 1)
                    {
                        ViewBag.diasCon = int.Parse(item);
                    }
                    else if (i == 2)
                    {
                        ViewBag.mesCon = int.Parse(item);
                    }
                    else if (i == 3)
                    {
                        ViewBag.anioCon = int.Parse(item);
                    }
                    else if (i == 4)
                    {
                        ViewBag.horaICon = item;
                    }
                    else if (i == 5)
                    {
                        ViewBag.horaFCon = item;
                    }
                    else if (i == 6)
                    {
                        ViewBag.minCon = int.Parse(item);
                    }
                    else if (i == 7)
                    {
                        ViewBag.turnoCon = item;

                    }
                    else
                    {
                        ViewBag.consultorioCon = item;
                    }
                    i++;
                }
            }

            ViewBag.mes = mes;
            ViewBag.anioActual = anioActual;
            ViewBag.anioSiguiente = anioSiguiente;
            if (ViewBag.turnoCon != null)
            {
                ViewBag.TurnoSelec = ViewBag.turnoCon;
            }
            else { ViewBag.TurnoSelec = "MAÑANA"; }

            ViewBag.codigo = id;
            ViewBag.listaHorarios = (List<E_HorarioMedico>)ListadoHorarioMedico().Where(x => x.CodMed == id).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult HorarioMedico(E_HorarioMedico hor, string accion = null)
        {
            //}
            ViewBag.codigo = hor.CodMed;

            E_Medico reg = ListadoMedico().Find(x => x.CodMed.Equals(hor.CodMed));
            ViewBag.codnom = reg.CodMed + "-" + reg.NomMed;
            ConsultorioController con = new ConsultorioController();
            ViewBag.listadoConsultorio = new SelectList(con.ListadoConsultorio().Where(x => x.CodServ == reg.CodServ || x.Mixto == true).ToList(), "IdConsul", "DescConsul");

            UtilitarioController u = new UtilitarioController();
            var fechaSistema = u.ListadoHoraServidor().FirstOrDefault();
            DateTime fecha = fechaSistema.HoraServidor;
            DateTime fechaS = fechaSistema.HoraServidor;
            ViewBag.minCon = hor.IntMin;
            fechaS = fechaS.AddYears(1);
            string horaYa = fecha.ToString("mm:hh tt");
            string mes = fecha.ToString("MM");
            string anioActual = fecha.ToString("yyyy");
            string anioSiguiente = fechaS.ToString("yyyy");
            ViewBag.anioActual = anioActual;
            ViewBag.anioSiguiente = anioSiguiente;
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            int dias = DateTime.DaysInMonth(year, month);

            ViewBag.dias = dias;
            ViewBag.mes = month.ToString();
            ViewBag.anio = year;
            ViewBag.horaI = horaYa;
            ViewBag.horaF = horaYa;
            ViewBag.min = 10;
            ViewBag.turno = "nada";
            ViewBag.consultorio = "nada";
            ViewBag.TurnoSelec = hor.Turno;
            //
            E_Medico regM = ListadoMedico().Find(x => x.CodMed.Equals(hor.CodMed));
            ConsultorioController cn = new ConsultorioController();
            ViewBag.listadoConsultorio = new SelectList(cn.ListadoConsultorio().Where(x => x.CodServ == regM.CodServ || x.Mixto == true).ToList(), "IdConsul", "DescConsul", hor.Consultorio);


            E_Master reg1 = u.ListadoHoraServidor().FirstOrDefault();

            string crear = Session["usuario"] + " " + reg1.HoraServidor + " " + Environment.MachineName;




            if (hor.array != null)
            {
                foreach (string ia in hor.array)
                {
                    DateTime uno = DateTime.Parse(hor.horaInicioB);
                    DateTime dos = DateTime.Parse(hor.horaFinB);
                    TimeSpan unoFinal = TimeSpan.Parse(uno.ToString("HH:mm"));
                    TimeSpan dosFinal = TimeSpan.Parse(dos.ToString("HH:mm"));

                    DateTime av = DateTime.Parse(ia);


                    var listaHorarios = (List<E_HorarioMedico>)ListadoHorarioMedico().Where(x => x.CodMed == hor.CodMed && x.horaInicio < dosFinal && x.horaFin > unoFinal && x.dia == av && x.Estado == true).ToList();

                    if (listaHorarios.Count() == 0)
                    {

                        using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                        {
                            con1.Open();
                            using (SqlCommand cmd = new SqlCommand("Usp_MtoHorarioMedicos", con1))
                            {
                                try
                                {
                                    cmd.Parameters.AddWithValue("@CodHor", "");
                                    cmd.Parameters.AddWithValue("@dia", ia);
                                    cmd.Parameters.AddWithValue("@horaInicio", hor.horaInicioB);
                                    cmd.Parameters.AddWithValue("@horaFin", hor.horaFinB);
                                    cmd.Parameters.AddWithValue("@IntMin", hor.IntMin);
                                    cmd.Parameters.AddWithValue("@Turno", hor.Turno);
                                    cmd.Parameters.AddWithValue("@CodMed", hor.CodMed);
                                    cmd.Parameters.AddWithValue("@Crea", crear);
                                    cmd.Parameters.AddWithValue("@Modifica", "");
                                    cmd.Parameters.AddWithValue("@Elimina", "");
                                    cmd.Parameters.AddWithValue("@Evento", "1");
                                    cmd.Parameters.AddWithValue("@Consultorio", hor.Consultorio.ToUpper());
                                    cmd.Parameters.AddWithValue("@Estado", "");
                                    cmd.Parameters.AddWithValue("@Asistencia", "");

                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                    ViewBag.Mensaje = "Se Modifico Satisfactoriamente";
                                }
                                catch (Exception ex)
                                {

                                    ViewBag.Mensaje = "Error al Modificar" + ex.Message.ToString();
                                    return View(hor);

                                }
                                finally
                                {
                                    con1.Close();
                                }
                            }
                        }


                    }
                    else
                    {
                        Response.Write("<script language=javascript> alert('No se pudo registrar porque hay fechas que coenciden'); </script>");
                        Response.Write("<script language=javascript> history.back(1); </script>");
                        return null;
                    }
                }
            }
            else
            {
                ViewBag.mensaje = "Error Datos no Validos";
                return View(hor);
            }


            return RedirectPermanent("../ListaHorarioMedico?id=" + hor.CodMed + "&accion=" + accion);
        }


        public ActionResult ListaHorarioMedico(string id, string accion = null, int? mes = null, int? anio = null)
        {
            ViewBag.dimension = accion;
            ViewBag.codigo = id;
            var medico = ListadoMedico().Where(x => x.CodMed == id).FirstOrDefault();
            ViewBag.CodNom = medico.CodMed + "-" + medico.NomMed;
            if (mes != null && anio != null)
            {
                ViewBag.mes = mes;
                ViewBag.anio = anio;
            }
            else
            {
                ViewBag.mes = null;
                ViewBag.anio = null;
            }



            UtilitarioController u = new UtilitarioController();
            var fechaSistema = u.ListadoHoraServidor().FirstOrDefault();

            DateTime fecha = fechaSistema.HoraServidor;
            DateTime fechaS = fechaSistema.HoraServidor;

            fechaS = fechaS.AddYears(1);
            string anioActual = fecha.ToString("yyyy");
            string mesctual = fecha.ToString("MM");
            string anioSiguiente = fechaS.ToString("yyyy");

            int AnioActuall = int.Parse(anioActual);
            int MesActuall = int.Parse(mesctual);

            ViewBag.anioActual = anioActual;
            ViewBag.anioSiguiente = anioSiguiente;

            if (mes != null && anio != null)
            {
                int mess = int.Parse(mes.ToString());
                int anios = int.Parse(anio.ToString());
                List<E_HorarioMedico> Pedidos = (List<E_HorarioMedico>)ListadoHorarioMedicoBusca(mess, anios).Where(x => x.CodMed == id).ToList();
                ViewBag.anioSelect = anios;
                ViewBag.mesSelect = mess;
                return View(Pedidos);
            }
            else
            {
                List<E_HorarioMedico> Pedidos = (List<E_HorarioMedico>)ListadoHorarioMedicoBusca(MesActuall, AnioActuall).Where(x => x.CodMed == id).ToList();
                ViewBag.anioSelect = AnioActuall;
                ViewBag.mesSelect = MesActuall;
                return View(Pedidos);
            }
        }

        public ActionResult ObtenerFechaFor(E_HorarioMedico h)
        {

            int mes = int.Parse(h.MesConsulta);
            int año = int.Parse(h.AnioConsulta);
            string hi = h.horaInicioB;
            string hf = h.horaFinB;
            int min = h.IntMin;
            string t = h.Turno;
            string con = h.Consultorio;
            int dias = DateTime.DaysInMonth(año, mes);

            string dd = dias + "," + mes + "," + año + "," + hi + "," + hf + "," + min + "," + t + "," + con;

            return RedirectPermanent("HorarioMedico/?id=" + h.CodMed + "&dd=" + dd);

        }
        public ActionResult Elimina(string id, string CodMed, string accion = null)
        {
            UtilitarioController u = new UtilitarioController();
            E_Master reg = u.ListadoHoraServidor().FirstOrDefault();
            string eliminar = Session["usuario"] + " " + reg.HoraServidor + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoHorarioMedicos", con))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@CodHor", id);
                        cmd.Parameters.AddWithValue("@dia", "");
                        cmd.Parameters.AddWithValue("@horaInicio", "");
                        cmd.Parameters.AddWithValue("@horaFin", "");
                        cmd.Parameters.AddWithValue("@IntMin", "");
                        cmd.Parameters.AddWithValue("@Turno", "");
                        cmd.Parameters.AddWithValue("@CodMed", "");
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", "");
                        cmd.Parameters.AddWithValue("@Elimina", eliminar);
                        cmd.Parameters.AddWithValue("@Evento", "3");
                        cmd.Parameters.AddWithValue("@Consultorio", "");
                        cmd.Parameters.AddWithValue("@Estado", "");
                        cmd.Parameters.AddWithValue("@Asistencia", "");

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se Modifico Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Error al Modificar" + ex.Message.ToString();

                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return RedirectPermanent("./ListaHorarioMedico?id=" + CodMed + "&accion=" + accion);
        }

        public ActionResult EvaluaHora(string HoraE)
        {
            DateTime fechaCambia = DateTime.Parse(HoraE);
            string ff = fechaCambia.ToString("HH:mm");
            TimeSpan evalua = TimeSpan.Parse(ff);
            TimeSpan dia = TimeSpan.Parse("13:59");
            TimeSpan tarde = TimeSpan.Parse("23:59");

            List<E_Citas> resultRows = new List<E_Citas>();

            string respuesta = " ";
            if (evalua <= dia)
            {
                respuesta = "MAÑANA";
                resultRows.Add(new E_Citas { numero = 1, Turno = respuesta });
            }
            else if (evalua <= tarde)
            {
                respuesta = "TARDE";
                resultRows.Add(new E_Citas { numero = 2, Turno = respuesta });
            }

            if (resultRows.Count() != 0)
            {
                return Json(resultRows, JsonRequestBehavior.AllowGet);

            }

            return null;
        }
        public ActionResult VerHorario(string id)
        {
            ViewBag.listaHorarios = (List<E_HorarioMedico>)ListadoHorarioMedico().Where(x => x.CodMed == id).ToList();
            return View();
        }

        public ActionResult Asistencia(string fecha = null, string turno = null, string servicio = null)
        {

            string sede = Session["codSede"].ToString();

            ServiciosController ser = new ServiciosController();

            ViewBag.listaServicio = new SelectList(ser.ListadoServicios().Where(x => x.EstServ == true && x.CodSede == sede).ToList(), "CodServ", "NomServ", servicio);
            try
            {
                if (fecha != null && turno != null && servicio != null)
                {
                    DateTime fechaRecibe = DateTime.Parse(fecha);
                    List<E_HorarioMedico> Pedidos = (List<E_HorarioMedico>)ListadoParaAsistencia().Where(x => x.dia == fechaRecibe && x.Turno == turno && x.CodServ == servicio && x.Estado == true).ToList();
                    string fechaFormat = fechaRecibe.ToString("dd/MM/yyyy");
                    ViewBag.fecha = fechaFormat;
                    ViewBag.turno = turno;
                    ViewBag.servicio = servicio;
                    return View(Pedidos);
                }
                else
                {
                    UtilitarioController u = new UtilitarioController();
                    E_Master reg = u.ListadoHoraServidor().FirstOrDefault();
                    string fechaFormat = reg.HoraServidor.ToString("dd/MM/yyyy");

                    ViewBag.fecha = fechaFormat;
                    ViewBag.turno = "ninguna";
                    ViewBag.servicio = "ninguna";
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.mensaje = "Error: Datos Incorrectos";
                return View();
            }
        }

        public List<E_HorarioMedico> ListadoParaAsistencia()
        {
            List<E_HorarioMedico> Lista = new List<E_HorarioMedico>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ListadoMedicoxHorario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_HorarioMedico hor = new E_HorarioMedico();

                            hor.CodMed = dr.GetString(0);
                            hor.CodHor = dr.GetString(1);
                            hor.NomMed = dr.GetString(2);
                            hor.NomServ = dr.GetString(3);
                            hor.CodServ = dr.GetString(4);
                            hor.horaInicio = dr.GetTimeSpan(5);
                            hor.horaFin = dr.GetTimeSpan(6);
                            hor.dia = dr.GetDateTime(7);
                            hor.Turno = dr.GetString(8);
                            hor.Asistencia = dr.GetString(9);
                            hor.Estado = dr.GetBoolean(10);

                            Lista.Add(hor);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public int validar_Usuario_Medico(string codusu)
        {
            string codsede = Session["CodSede"].ToString();
            int resultado = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_validar_UsuarioMedico", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codusu", codusu);
                    cmd.Parameters.AddWithValue("@codsede", codsede);
                    resultado = (int)cmd.ExecuteScalar();
                    return resultado;
                }
            }
        }

        public List<E_Medico> Usp_ListaGeneral_Tarifa_Medico()
        {
            List<E_Medico> Lista = new List<E_Medico>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListaGeneral_Tarifa_Medico", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Medico Med = new E_Medico();
                            Med.CodTar = dr.GetString(0);
                            Med.CodMed = dr.GetString(1);
                            Med.porcentaje1 = Convert.ToInt32(dr.GetDecimal(2));
                            Med.DescTar = dr.GetString(3);
                            Lista.Add(Med);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        [HttpPost]
        public ActionResult RegistraAsistencia(string[] evalua, string fecha, string turno, string servicio)
        {

            UtilitarioController u = new UtilitarioController();

            E_Master reg = u.ListadoHoraServidor().FirstOrDefault();

            string modifica = Session["usuario"] + " " + reg.HoraServidor + " " + Environment.MachineName;

            string CodHor = "";
            string asist = "";
            try {
                foreach (string ia in evalua)
                {

                    string[] fija = ia.Split(',');
                    int i = 1;
                    foreach (string item in fija)
                    {
                        if (i == 1)
                        {
                            CodHor = item;
                        }
                        else if (i == 2)
                        {
                            asist = item;
                        }
                        i++;
                    }


                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("Usp_MtoHorarioMedicos", con))
                        {
                            try
                            {
                                cmd.Parameters.AddWithValue("@CodHor", CodHor);
                                cmd.Parameters.AddWithValue("@dia", "");
                                cmd.Parameters.AddWithValue("@horaInicio", "");
                                cmd.Parameters.AddWithValue("@horaFin", "");
                                cmd.Parameters.AddWithValue("@IntMin", "");
                                cmd.Parameters.AddWithValue("@Turno", "");
                                cmd.Parameters.AddWithValue("@CodMed", "");
                                cmd.Parameters.AddWithValue("@Crea", "");
                                cmd.Parameters.AddWithValue("@Modifica", modifica);
                                cmd.Parameters.AddWithValue("@Elimina", "");
                                cmd.Parameters.AddWithValue("@Evento", "2");
                                cmd.Parameters.AddWithValue("@Consultorio", "");
                                cmd.Parameters.AddWithValue("@Estado", "");
                                cmd.Parameters.AddWithValue("@Asistencia", asist);

                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                                ViewBag.Mensaje = "Se Modifico Satisfactoriamente";
                            }
                            catch (Exception ex)
                            {
                                ViewBag.mensaje = "3";
                                
                            }
                            finally
                            {
                                con.Close();
                            }
                        }
                    }

                }

            }
            catch (Exception ex) {
                ViewBag.mensaje = "3";
               
            }
       
            Response.Write("<script language=javascript> history.back(1); </script>");
            return RedirectPermanent("Asistencia?fecha=" + fecha + "&turno=" + turno + "&servicio=" + servicio);

        }
        public bool ValidarDni_Medico(string dni)
        {
            bool medico = false;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("ValidarDniXMedico", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@dni", dni);
                    if (int.Parse(cmd.ExecuteScalar().ToString()) == 0)
                    {
                        medico = false;
                    }
                    else
                    {
                        medico = true;
                    }
                }
            }
            return medico;
        }

        public int ValidarMedicoUsuario_Existe(string codusu, string codmed)
        {
            string codsede = Session["CodSede"].ToString();
            int resultado = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Validar_MedicoUsuario_Existente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodMed", codmed);
                    cmd.Parameters.AddWithValue("@codusu", codusu);
                    cmd.Parameters.AddWithValue("@codsede", codsede);
                    resultado = (int)cmd.ExecuteScalar();
                    return resultado;
                }

            }

        }

        public JsonResult ValidarDniXMedico(string dni = null)
        {
            if (dni != null)
            {
                bool valor = ValidarDni_Medico(dni);
                return Json(valor, JsonRequestBehavior.AllowGet);
            }
            return null;

        }

        public JsonResult ValidarUsuarioMedico(string codusu = null)
        {
            if (!string.IsNullOrWhiteSpace(codusu))
            {
                int validado = validar_Usuario_Medico(codusu);
                return Json(validado, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ValidandoUsuarioMedicoExistente(string codusu, string codmed)
        {
            bool resultado = false;
            if (!string.IsNullOrWhiteSpace(codusu) && !string.IsNullOrWhiteSpace(codmed))
            {
                int validador1 = validar_Usuario_Medico(codusu);
                if (validador1 == 1)
                {
                    int validador2 = ValidarMedicoUsuario_Existe(codusu, codmed);
                    if (validador2 == 1)
                    {
                        resultado = false;
                        return Json(resultado, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        resultado = true;
                        return Json(resultado, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    resultado = false;
                    return Json(resultado, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                resultado = false;
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
        }


    }
}