using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using VgSalud.Models;
using System.Globalization;

namespace VgSalud.Controllers
{
    public class PacientesController : Controller
    {


        public PacientesController()
        {

        }
        // GET: Pacientes

        // GET: Especialidad


        public ActionResult RegistrarPaciente()
        {
            string sede = Session["codSede"].ToString();
            var util = new DatosGeneralesController().LISTA_DISTRITO_DEFAULT(sede).FirstOrDefault(); 
            DatosGeneralesController da = new DatosGeneralesController();
            E_Datos_Generales d = da.listadatogenerales().FirstOrDefault();
            ViewBag.muestraAnecedente = d.MUESTRA_ANTECEDENTE;
            ViewBag.coddistrito = util.CodDist; 
            DocumentoIdentidadController DocIden = new DocumentoIdentidadController();
            ViewBag.ListaDocumentoIdentidad = new SelectList(DocIden.ListadoDocumentoIdentidad().Where(x => x.Estado == true).ToList(), "CodDocIdent", "NomDocIdent");

            TipoFiliacionController TipFil = new TipoFiliacionController();
            ViewBag.ListaTipoFilia = new SelectList(TipFil.ListadoTipoFiliacion().Where(x => x.Estado == true).ToList(), "CodTipFil", "DescTipFil");

            CategoriaPacienteController CatPac = new CategoriaPacienteController();
            ViewBag.listaCatePaciente = new SelectList(CatPac.listadoCategoriaCliente().Where(x => x.EstCatPac == true).ToList(), "CodCatPac", "DescCatPac");

            UtilitarioController Uti = new UtilitarioController();
            ViewBag.ListaSexo = new SelectList(Uti.ListadoSexo(), "CodSexo", "NomSexo");
            ViewBag.ListaCivil = new SelectList(Uti.ListadoEstadoCivil(), "CodEstCivil", "NomEstCivil");
            ViewBag.ListaPais = new SelectList(Uti.ListadoPais(), "CodPais", "NomPais");

            ViewBag.ListaDepartamento = new SelectList(Uti.ListadoDepartamentoSimple(), "CodDep", "NomDep");

            ViewBag.depa = (List<E_Departamento>)Uti.ListadoDepartamentoSimple();
            ViewBag.prov = (List<E_Provincia>)Uti.ListadoProvinciaSimple();
            ViewBag.Dist = (List<E_Distrito>)Uti.ListadoDistritoSimple();
            E_Master hora = Uti.ListadoHoraServidor().FirstOrDefault();
            ViewBag.horaSis = hora.HoraServidor.ToShortDateString();

            ViewBag.ListaUsuario = new SelectList(ListadoUsuarioPaciente(), "CodigoUsuario", "AliasUsu");

            return View();

        }

        [HttpPost]
        public ActionResult RegistrarPaciente(E_Pacientes EPac)
        {

            DatosGeneralesController da = new DatosGeneralesController();
            E_Datos_Generales d = da.listadatogenerales().FirstOrDefault();
            ViewBag.muestraAnecedente = d.MUESTRA_ANTECEDENTE;

            DocumentoIdentidadController DocIden = new DocumentoIdentidadController();
            ViewBag.ListaDocumentoIdentidad = new SelectList(DocIden.ListadoDocumentoIdentidad(), "CodDocIdent", "NomDocIdent", EPac.CodDocIdent);

            TipoFiliacionController TipFil = new TipoFiliacionController();
            ViewBag.ListaTipoFilia = new SelectList(TipFil.ListadoTipoFiliacion(), "CodTipFil", "DescTipFil", EPac.CodTipFil);

            CategoriaPacienteController CatPac = new CategoriaPacienteController();
            ViewBag.listaCatePaciente = new SelectList(CatPac.listadoCategoriaCliente(), "CodCatPac", "DescCatPac", EPac.CodCatPac);

            UtilitarioController Uti = new UtilitarioController();
            ViewBag.ListaSexo = new SelectList(Uti.ListadoSexo(), "CodSexo", "NomSexo", EPac.CodSexo);
            ViewBag.ListaCivil = new SelectList(Uti.ListadoEstadoCivil(), "CodEstCivil", "NomEstCivil", EPac.CodEstCivil);
            ViewBag.ListaPais = new SelectList(Uti.ListadoPais().Where(x => x.CodPais == "1"), "CodPais", "NomPais", EPac.CodPais);

            ViewBag.ListaPais = new SelectList(Uti.ListadoPais(), "CodPais", "NomPais", EPac.CodPais);

            ViewBag.depa = (List<E_Departamento>)Uti.ListadoDepartamentoSimple();
            ViewBag.prov = (List<E_Provincia>)Uti.ListadoProvinciaSimple();
            ViewBag.Dist = (List<E_Distrito>)Uti.ListadoDistritoSimple();

            ViewBag.ListaUsuario = new SelectList(ListadoUsuarioPaciente(), "CodigoUsuario", "AliasUsu", EPac.CodUsu);

            string Crea = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;
            var usuario = Session["usuario"].ToString();
            int codResu = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                using (SqlCommand cmd = new SqlCommand("usp_MtoPacientes", con, tr))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@Historia", "");
                        cmd.Parameters.AddWithValue("@ApePat", EPac.ApePat.ToUpper());
                        if (EPac.ApeMat == null) { cmd.Parameters.AddWithValue("@ApeMat", ""); } else { cmd.Parameters.AddWithValue("@ApeMat", EPac.ApeMat.ToUpper()); }

                        cmd.Parameters.AddWithValue("@NomPac", EPac.NomPac.ToUpper());
                        cmd.Parameters.AddWithValue("@FecNac", EPac.FecNac);
                        if (EPac.Sector == null) { cmd.Parameters.AddWithValue("@Sector", ""); } else { cmd.Parameters.AddWithValue("@Sector", EPac.Sector); }

                        cmd.Parameters.AddWithValue("@NumDoc", EPac.NumDoc);
                        if (EPac.Ruc == null) { cmd.Parameters.AddWithValue("@Ruc", ""); } else { cmd.Parameters.AddWithValue("@Ruc", EPac.Ruc); }

                        if (EPac.Essalud == null) { cmd.Parameters.AddWithValue("@Essalud", ""); } else { cmd.Parameters.AddWithValue("@Essalud", EPac.Essalud); }

                        if (EPac.CodAseg == null) { cmd.Parameters.AddWithValue("@CodAseg", ""); } else { cmd.Parameters.AddWithValue("@CodAseg", EPac.CodAseg); }


                        if (EPac.LugarNac == null) { cmd.Parameters.AddWithValue("@LugarNac", ""); } else { cmd.Parameters.AddWithValue("@LugarNac", EPac.LugarNac.ToUpper()); }

                        
                        if(EPac.Direcc == null)
                        {
                            cmd.Parameters.AddWithValue("@Direcc", "");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Direcc", EPac.Direcc);
                        }

                        if (EPac.Email == null) { cmd.Parameters.AddWithValue("@Email", ""); } else { cmd.Parameters.AddWithValue("@Email", EPac.Email); }

                        if (EPac.TelfFijo==null)
                        {
                            cmd.Parameters.AddWithValue("@TelfFijo", "");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@TelfFijo", EPac.TelfFijo);
                        }
                        

                        if (EPac.TelfCel == null) { cmd.Parameters.AddWithValue("@TelfCel", ""); } else { cmd.Parameters.AddWithValue("@TelfCel", EPac.TelfCel); }
                        if (EPac.CodTipPac == 0) { cmd.Parameters.AddWithValue("@CodTipPac", 1); } else { cmd.Parameters.AddWithValue("@CodTipPac", EPac.CodTipPac); }

                        if (EPac.Observ == null) { cmd.Parameters.AddWithValue("@Observ", ""); } else { cmd.Parameters.AddWithValue("@Observ", EPac.Observ); }

                        cmd.Parameters.AddWithValue("@Discap", EPac.Discap);

                        if (EPac.DiscaObs == null) { cmd.Parameters.AddWithValue("@DiscaObs", ""); } else { cmd.Parameters.AddWithValue("@DiscaObs", EPac.DiscaObs.ToUpper()); }

                        if (EPac.TipSang == null) { cmd.Parameters.AddWithValue("@TipSang", ""); } else { cmd.Parameters.AddWithValue("@TipSang", EPac.TipSang.ToUpper()); }

                        if (EPac.Alerg == null) { cmd.Parameters.AddWithValue("@Alerg", ""); } else { cmd.Parameters.AddWithValue("@Alerg", EPac.Alerg.ToUpper()); }

                        if (EPac.TitParent == null) { cmd.Parameters.AddWithValue("@TitParent", ""); } else { cmd.Parameters.AddWithValue("@TitParent", EPac.TitParent); }

                        if (EPac.TitNom == null) { cmd.Parameters.AddWithValue("@TitNom", ""); } else { cmd.Parameters.AddWithValue("@TitNom", EPac.TitNom.ToUpper()); }

                        if (EPac.TitDni == null) { cmd.Parameters.AddWithValue("@TitDni", ""); } else { cmd.Parameters.AddWithValue("@TitDni", EPac.TitDni); }

                        if (EPac.TitObs == null) { cmd.Parameters.AddWithValue("@TitObs", ""); } else { cmd.Parameters.AddWithValue("@TitObs", EPac.TitObs.ToUpper()); }

                        if (EPac.Ocup == null) { cmd.Parameters.AddWithValue("@Ocup", ""); } else { cmd.Parameters.AddWithValue("@Ocup", EPac.Ocup.ToUpper()); }

                        if (EPac.DirTrab == null) { cmd.Parameters.AddWithValue("@DirTrab", ""); } else { cmd.Parameters.AddWithValue("@DirTrab", EPac.DirTrab.ToUpper()); }

                        if (EPac.TelTrab == null) { cmd.Parameters.AddWithValue("@TelTrab", ""); } else { cmd.Parameters.AddWithValue("@TelTrab", EPac.TelTrab); }

                        cmd.Parameters.AddWithValue("@CodCatPac", EPac.CodCatPac);
                        cmd.Parameters.AddWithValue("@CodEstCivil", EPac.CodEstCivil);
                        cmd.Parameters.AddWithValue("@CodSexo", EPac.CodSexo);
                        cmd.Parameters.AddWithValue("@CodTipFil", EPac.CodTipFil);
                        cmd.Parameters.AddWithValue("@CodDocIdent", EPac.CodDocIdent);
                        cmd.Parameters.AddWithValue("@CodDist", EPac.CodDist);
                        cmd.Parameters.AddWithValue("@CodProv", EPac.CodProv);
                        cmd.Parameters.AddWithValue("@CodDep", EPac.CodDep);
                        cmd.Parameters.AddWithValue("@CodPais", EPac.CodPais);
                        if (EPac.CodUsu == null) { cmd.Parameters.AddWithValue("@CodUsu", ""); } else { cmd.Parameters.AddWithValue("@CodUsu", EPac.CodUsu); }

                        cmd.Parameters.AddWithValue("@EstPac", EPac.EstPac);
                        cmd.Parameters.AddWithValue("@Crea", Crea);
                        cmd.Parameters.AddWithValue("@Modifica", "");
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Evento", 1);

                        cmd.CommandType = CommandType.StoredProcedure;

                        codResu = (int)cmd.ExecuteScalar();
                        cmd.Dispose();
                        if (codResu == 0)
                        {
                            ViewBag.Mensaje = "Verificar sus Datos,este paciente ya existe";
                        }
                        else
                        {
                            using (SqlCommand cmd1 = new SqlCommand("Usp_Mantenimiento_Antecedentes", con, tr))
                            {
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@CodAnt", 0);
                                cmd1.Parameters.AddWithValue("@Historia", codResu);
                                cmd1.Parameters.AddWithValue("@CancerP", EPac.CancerP);
                                cmd1.Parameters.AddWithValue("@DiabetesP", EPac.DiabetesP);
                                cmd1.Parameters.AddWithValue("@ACVP", EPac.ACVP);
                                cmd1.Parameters.AddWithValue("@AlergiaP", EPac.AlergiaP);
                                cmd1.Parameters.AddWithValue("@HipertArtP", EPac.HipertArtP);
                                cmd1.Parameters.AddWithValue("@OtrosP", EPac.OtrosP);
                                cmd1.Parameters.AddWithValue("@CancerF", EPac.CancerF);
                                cmd1.Parameters.AddWithValue("@DiabetesF", EPac.DiabetesF);
                                cmd1.Parameters.AddWithValue("@ACVF", EPac.ACVF);
                                cmd1.Parameters.AddWithValue("@AlergiaF", EPac.AlergiaF);
                                cmd1.Parameters.AddWithValue("@HipertArtF", EPac.HipertArtF);
                                cmd1.Parameters.AddWithValue("@OtrosF", EPac.OtrosF);
                                cmd1.Parameters.AddWithValue("@ObservacionP", EPac.ObservacionP == null ? "" : EPac.ObservacionP);
                                cmd1.Parameters.AddWithValue("@ObservacionF", EPac.ObservacionF == null ? "" : EPac.ObservacionF);
                                cmd1.Parameters.AddWithValue("@Usuario", usuario);
                                cmd1.Parameters.AddWithValue("@Crea", Crea);
                                cmd1.Parameters.AddWithValue("@Modifica", "");
                                cmd1.Parameters.AddWithValue("@Elimina", "");
                                cmd1.Parameters.AddWithValue("@Evento", 1);
                                cmd1.ExecuteNonQuery();

                            }

                            tr.Commit();
                            ViewBag.Mensaje = "Se registro Satisfactoriamente";

                        }

                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        ViewBag.Mensaje = "3";
                        return View(EPac);


                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return RedirectPermanent("./RegistroPaciente?id=" + codResu);
            }

        }

        public JsonResult ValidarDni(string NumDoc = null)
        {
            if (NumDoc != null)
            {
                var paciente = ListadoPacientes().Where(x => x.NumDoc == NumDoc).FirstOrDefault();
                if (paciente != null)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            return null;

        }


        public ActionResult ModificarPacientes(int Id)
        {

            DatosGeneralesController da = new DatosGeneralesController();
            E_Datos_Generales d = da.listadatogenerales().FirstOrDefault();
            ViewBag.muestraAnecedente = d.MUESTRA_ANTECEDENTE;

            string sede = Session["codSede"].ToString();

            DocumentoIdentidadController DocIden = new DocumentoIdentidadController();
            ViewBag.ListaDocumentoIdentidad = new SelectList(DocIden.ListadoDocumentoIdentidad(), "CodDocIdent", "NomDocIdent");

            TipoFiliacionController TipFil = new TipoFiliacionController();
            ViewBag.ListaTipoFilia = new SelectList(TipFil.ListadoTipoFiliacion(), "CodTipFil", "DescTipFil");

            CategoriaPacienteController CatPac = new CategoriaPacienteController();
            ViewBag.listaCatePaciente = new SelectList(CatPac.listadoCategoriaCliente(), "CodCatPac", "DescCatPac");

            UtilitarioController Uti = new UtilitarioController();

            var ListarSexo = ListarSexoPaciente(Id).FirstOrDefault(); 
            ViewBag.Sexo = ListarSexo.CodSexo;
            //ViewBag.ListaSexo = new SelectList(Uti.ListadoSexo(), "CodSexo", "NomSexo");
            ViewBag.ListaCivil = new SelectList(Uti.ListadoEstadoCivil(), "CodEstCivil", "NomEstCivil");
            ViewBag.ListaPais = new SelectList(Uti.ListadoPais(), "CodPais", "NomPais", "1");

            //ViewBag.ListaPais = new SelectList(Uti.ListadoPais().Where(x => x.CodPais == "1"), "CodPais", "NomPais");

            ViewBag.ListaDepartamento = new SelectList(Uti.ListadoDepartamentoSimple(), "CodDep", "NomDep");
            ViewBag.ListaProvincia = new SelectList(Uti.ListadoProvinciaSimple(), "CodProv", "NomProv");
     

            ViewBag.depa = (List<E_Departamento>)Uti.ListadoDepartamentoSimple();
            ViewBag.prov = (List<E_Provincia>)Uti.ListadoProvinciaSimple();
            ViewBag.Dist = (List<E_Distrito>)Uti.ListadoDistritoSimple();

            ViewBag.ListaUsuario = new SelectList(ListadoUsuarioPaciente(), "CodigoUsuario", "AliasUsu");

            var lista = (from x in ListadoPacientes() where x.Historia == Id select x).FirstOrDefault();
            ViewBag.ListaDistrito = new SelectList(Uti.ListadoDistritoSimple(), "CodDist", "NomDist",lista.CodDist);
            ViewBag.fechaNac = lista.FecNac.ToShortDateString();
            return View(lista);

        }

        [HttpPost]
        public ActionResult ModificarPacientes(E_Pacientes EPac)
        {
            DatosGeneralesController da = new DatosGeneralesController();
            E_Datos_Generales d = da.listadatogenerales().FirstOrDefault();
            ViewBag.muestraAnecedente = d.MUESTRA_ANTECEDENTE;

            DocumentoIdentidadController DocIden = new DocumentoIdentidadController();
            ViewBag.ListaDocumentoIdentidad = new SelectList(DocIden.ListadoDocumentoIdentidad(), "CodDocIdent", "NomDocIdent", EPac.CodDocIdent);

            TipoFiliacionController TipFil = new TipoFiliacionController();
            ViewBag.ListaTipoFilia = new SelectList(TipFil.ListadoTipoFiliacion(), "CodTipFil", "DescTipFil", EPac.CodTipFil);

            CategoriaPacienteController CatPac = new CategoriaPacienteController();
            ViewBag.listaCatePaciente = new SelectList(CatPac.listadoCategoriaCliente(), "CodCatPac", "DescCatPac", EPac.CodCatPac);

            UtilitarioController Uti = new UtilitarioController();
            ViewBag.ListaSexo = new SelectList(Uti.ListadoSexo(), "CodSexo", "NomSexo", EPac.CodSexo);
            ViewBag.ListaCivil = new SelectList(Uti.ListadoEstadoCivil(), "CodEstCivil", "NomEstCivil", EPac.CodEstCivil);
            ViewBag.ListaPais = new SelectList(Uti.ListadoPais(), "CodPais", "NomPais", EPac.CodPais);
            ViewBag.ListaDepartamento = new SelectList(Uti.ListadoDepartamentoSimple(), "CodDep", "NomDep", EPac.CodDep);
            ViewBag.ListaProvincia = new SelectList(Uti.ListadoProvinciaSimple(), "CodProv", "NomProv", EPac.CodProv);
            ViewBag.ListaDistrito = new SelectList(Uti.ListadoDistritoSimple(), "CodDist", "NomDist", EPac.CodDist);

            ViewBag.ListaUsuario = new SelectList(ListadoUsuarioPaciente(), "CodigoUsuario", "AliasUsu", EPac.CodUsu);

            string Modificar = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;
            string usuario = Session["usuario"].ToString();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                using (SqlCommand cmd = new SqlCommand("usp_MtoPacientes", con, tr))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@Historia", EPac.Historia);
                        cmd.Parameters.AddWithValue("@ApePat", EPac.ApePat.ToUpper());
                        if (EPac.ApeMat == null) { cmd.Parameters.AddWithValue("@ApeMat", ""); } else { cmd.Parameters.AddWithValue("@ApeMat", EPac.ApeMat.ToUpper()); }
                        cmd.Parameters.AddWithValue("@NomPac", EPac.NomPac.ToUpper());
                        cmd.Parameters.AddWithValue("@FecNac", EPac.FecNac);
                        if (EPac.Sector == null) { cmd.Parameters.AddWithValue("@Sector", ""); } else { cmd.Parameters.AddWithValue("@Sector", EPac.Sector); }
                        cmd.Parameters.AddWithValue("@NumDoc", EPac.NumDoc);

                        if (EPac.Ruc == null) { cmd.Parameters.AddWithValue("@Ruc", ""); } else { cmd.Parameters.AddWithValue("@Ruc", EPac.Ruc); }

                        if (EPac.Essalud == null) { cmd.Parameters.AddWithValue("@Essalud", ""); } else { cmd.Parameters.AddWithValue("@Essalud", EPac.Essalud); }

                        if (EPac.CodAseg == null) { cmd.Parameters.AddWithValue("@CodAseg", ""); } else { cmd.Parameters.AddWithValue("@CodAseg", EPac.CodAseg); }


                        if (EPac.LugarNac == null) { cmd.Parameters.AddWithValue("@LugarNac", ""); } else { cmd.Parameters.AddWithValue("@LugarNac", EPac.LugarNac.ToUpper()); }

                        if (EPac.Direcc==null)
                        {
                            cmd.Parameters.AddWithValue("@Direcc", "");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Direcc", EPac.Direcc);
                        }
                        

                        if (EPac.DiscaObs == null) { cmd.Parameters.AddWithValue("@DiscaObs", ""); } else { cmd.Parameters.AddWithValue("@DiscaObs", EPac.DiscaObs.ToUpper()); }

                        if (EPac.Email == null) { cmd.Parameters.AddWithValue("@Email", ""); } else { cmd.Parameters.AddWithValue("@Email", EPac.Email.ToUpper()); }


                        if (EPac.TelfFijo == null) { cmd.Parameters.AddWithValue("@TelfFijo", ""); } else { cmd.Parameters.AddWithValue("@TelfFijo", EPac.TelfFijo); }

                        if (EPac.TelfCel == null) { cmd.Parameters.AddWithValue("@TelfCel", ""); } else { cmd.Parameters.AddWithValue("@TelfCel", EPac.TelfCel); }

                        cmd.Parameters.AddWithValue("@CodTipPac", EPac.CodTipPac);

                        //cmd.Parameters.AddWithValue("@CodSexo", EPac.CodSexo);

                        //if (EPac.CodSexo == 0)
                        //{
                        //    cmd.parameters.addwithvalue("@codtippac", 1);
                        //}
                        //else
                        //{
                        //    cmd.parameters.addwithvalue("@codtippac", epac.codtippac);
                        //}

                        if (EPac.Observ == null) { cmd.Parameters.AddWithValue("@Observ", ""); } else { cmd.Parameters.AddWithValue("@Observ", EPac.Observ.ToUpper()); }

                        cmd.Parameters.AddWithValue("@Discap", EPac.Discap);
                        if (EPac.TipSang == null) { cmd.Parameters.AddWithValue("@TipSang", ""); } else { cmd.Parameters.AddWithValue("@TipSang", EPac.TipSang.ToUpper()); }

                        if (EPac.Alerg == null) { cmd.Parameters.AddWithValue("@Alerg", ""); } else { cmd.Parameters.AddWithValue("@Alerg", EPac.Alerg.ToUpper()); }

                        if (EPac.TitParent == null) { cmd.Parameters.AddWithValue("@TitParent", ""); } else { cmd.Parameters.AddWithValue("@TitParent", EPac.TitParent.ToUpper()); }

                        if (EPac.TitNom == null) { cmd.Parameters.AddWithValue("@TitNom", ""); } else { cmd.Parameters.AddWithValue("@TitNom", EPac.TitNom.ToUpper()); }

                        if (EPac.TitDni == null) { cmd.Parameters.AddWithValue("@TitDni", ""); } else { cmd.Parameters.AddWithValue("@TitDni", EPac.TitDni); }

                        if (EPac.TitObs == null) { cmd.Parameters.AddWithValue("@TitObs", ""); } else { cmd.Parameters.AddWithValue("@TitObs", EPac.TitObs.ToUpper()); }

                        if (EPac.Ocup == null) { cmd.Parameters.AddWithValue("@Ocup", ""); } else { cmd.Parameters.AddWithValue("@Ocup", EPac.Ocup.ToUpper()); }

                        if (EPac.DirTrab == null) { cmd.Parameters.AddWithValue("@DirTrab", ""); } else { cmd.Parameters.AddWithValue("@DirTrab", EPac.DirTrab.ToUpper()); }

                        if (EPac.TelTrab == null) { cmd.Parameters.AddWithValue("@TelTrab", ""); } else { cmd.Parameters.AddWithValue("@TelTrab", EPac.TelTrab); }

                        cmd.Parameters.AddWithValue("@CodCatPac", EPac.CodCatPac);
                        cmd.Parameters.AddWithValue("@CodEstCivil", EPac.CodEstCivil);
                        cmd.Parameters.AddWithValue("@CodSexo", EPac.CodSexo);
                        cmd.Parameters.AddWithValue("@CodTipFil", EPac.CodTipFil);
                        cmd.Parameters.AddWithValue("@CodDocIdent", EPac.CodDocIdent);
                        cmd.Parameters.AddWithValue("@CodDist", EPac.CodDist);
                        cmd.Parameters.AddWithValue("@CodProv", EPac.CodProv);
                        cmd.Parameters.AddWithValue("@CodDep", EPac.CodDep);
                        cmd.Parameters.AddWithValue("@CodPais", EPac.CodPais);

                        if (EPac.CodUsu == null) { cmd.Parameters.AddWithValue("@CodUsu", ""); } else { cmd.Parameters.AddWithValue("@CodUsu", EPac.CodUsu); }

                        //cmd.Parameters.AddWithValue("@CodUsu", EPac.CodUsu);
                        cmd.Parameters.AddWithValue("@EstPac", EPac.EstPac);
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", Modificar);
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Evento", 2);
                        cmd.CommandType = CommandType.StoredProcedure;

                        int Historia = int.Parse(cmd.ExecuteScalar().ToString());
                        if (Historia != 0)
                        {

                            using (SqlCommand cmd1 = new SqlCommand("Usp_Mantenimiento_Antecedentes", con, tr))
                            {
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@CodAnt", EPac.CodAnt);
                                cmd1.Parameters.AddWithValue("@Historia", Historia);
                                cmd1.Parameters.AddWithValue("@CancerP", EPac.CancerP);
                                cmd1.Parameters.AddWithValue("@DiabetesP", EPac.DiabetesP);
                                cmd1.Parameters.AddWithValue("@ACVP", EPac.ACVP);
                                cmd1.Parameters.AddWithValue("@AlergiaP", EPac.AlergiaP);
                                cmd1.Parameters.AddWithValue("@HipertArtP", EPac.HipertArtP);
                                cmd1.Parameters.AddWithValue("@OtrosP", EPac.OtrosP);
                                cmd1.Parameters.AddWithValue("@CancerF", EPac.CancerF);
                                cmd1.Parameters.AddWithValue("@DiabetesF", EPac.DiabetesF);
                                cmd1.Parameters.AddWithValue("@ACVF", EPac.ACVF);
                                cmd1.Parameters.AddWithValue("@AlergiaF", EPac.AlergiaF);
                                cmd1.Parameters.AddWithValue("@HipertArtF", EPac.HipertArtF);
                                cmd1.Parameters.AddWithValue("@OtrosF", EPac.OtrosF);
                                cmd1.Parameters.AddWithValue("@ObservacionP", EPac.ObservacionP == null ? "" : EPac.ObservacionP);
                                cmd1.Parameters.AddWithValue("@ObservacionF", EPac.ObservacionF == null ? "" : EPac.ObservacionF);
                                cmd1.Parameters.AddWithValue("@Usuario", usuario);
                                cmd1.Parameters.AddWithValue("@Crea", "");
                                cmd1.Parameters.AddWithValue("@Modifica", Modificar);
                                cmd1.Parameters.AddWithValue("@Elimina", "");
                                cmd1.Parameters.AddWithValue("@Evento", 2);
                                cmd1.ExecuteNonQuery();

                            }

                            tr.Commit();
                            ViewBag.Mensaje = "Se registro Satisfactoriamente";
                        }

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
                return RedirectPermanent("RegistroPaciente?id=" + EPac.Historia);
            }

        }


        public ActionResult ListaPacientes(string fecha = null, string dni = null, string nombre = null)
        {
            UtilitarioController u = new UtilitarioController();
            E_Master hor = u.ListadoHoraServidor().FirstOrDefault();

            ViewBag.fecha = fecha;
            ViewBag.dni = dni;
            ViewBag.nombre = nombre;

            if (fecha == null || fecha == "")
            {
                fecha = hor.HoraServidor.ToShortDateString();
            }


            return View(ListadoPacientesFiltro(fecha, dni, nombre));

        }

        public ActionResult RegistroPaciente(int id)
        {
            
            var registro = ListadoPacientes().Where(x => x.Historia == id).FirstOrDefault();
            DocumentoIdentidadController DocIden = new DocumentoIdentidadController();
            ViewBag.ListaDocumentoIdentidad = DocIden.ListadoDocumentoIdentidad().Where(x => x.CodDocIdent == registro.CodDocIdent).FirstOrDefault().NomDocIdent;

            TipoFiliacionController TipFil = new TipoFiliacionController();
            ViewBag.ListaTipoFilia = TipFil.ListadoTipoFiliacion().Where(x => x.CodTipFil == registro.CodTipFil).FirstOrDefault().DescTipFil;
            CategoriaPacienteController CatPac = new CategoriaPacienteController();

            ViewBag.listaCatePaciente = CatPac.listadoCategoriaCliente().Where(x => x.CodCatPac == registro.CodCatPac).FirstOrDefault().DescCatPac;
            UtilitarioController Uti = new UtilitarioController();
            ViewBag.ListaSexo = Uti.ListadoSexo().Where(x => x.CodSexo == registro.CodSexo).FirstOrDefault().NomSexo;
            ViewBag.ListaCivil = Uti.ListadoEstadoCivil().Where(x => x.CodEstCivil == registro.CodEstCivil).FirstOrDefault().NomEstCivil;
            string codPais = registro.CodPais.Trim();
            ViewBag.ListaPais = Uti.ListadoPais().Where(x => x.CodPais == codPais).FirstOrDefault().NomPais;
            ViewBag.ListaDepartamento = Uti.ListadoDepartamentoSimple().Where(x => x.CodDep == registro.CodDep).FirstOrDefault().NomDep;
            ViewBag.ListaProvincia = Uti.ListadoProvinciaSimple().Where(x => x.CodProv == registro.CodProv).FirstOrDefault().NomProv;
            ViewBag.ListaDistrito = Uti.ListadoDistritoSimple().Where(x => x.CodDist == registro.CodDist).FirstOrDefault().NomDist;
            DatosGeneralesController da = new DatosGeneralesController();
            E_Datos_Generales d = da.listadatogenerales().FirstOrDefault();
            ViewBag.muestraAnecedente = d.MUESTRA_ANTECEDENTE;
            E_Master hora = Uti.ListadoHoraServidor().FirstOrDefault();
            ViewBag.horaSis = hora.HoraServidor.ToShortDateString();

            if (!string.IsNullOrWhiteSpace(registro.CodUsu))
            {
                ViewBag.ListaUsuario = ListadoUsuarioPaciente().Where(x => x.CodUsu == registro.CodUsu).FirstOrDefault().AliasUsu;
            }

            return View(registro);
        }

        public List<E_Pacientes> ListadoPacientes()
        {
            List<E_Pacientes> Lista = new List<E_Pacientes>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Pacientes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Pacientes Pac = new E_Pacientes();

                            Pac.Historia = dr.GetInt32(0);
                            Pac.ApePat = dr.GetString(1);
                            Pac.ApeMat = dr.GetString(2);
                            Pac.NomPac = dr.GetString(3);
                            Pac.FecNac = dr.GetDateTime(4);
                            Pac.Sector = dr.GetString(5);
                            Pac.NumDoc = dr.GetString(6);
                            Pac.Ruc = dr.GetString(7);
                            Pac.Essalud = dr.GetString(8);
                            Pac.CodAseg = dr.GetString(9);
                            Pac.FecAfil = dr.GetDateTime(10);
                            Pac.LugarNac = dr.GetString(11);
                            Pac.Direcc = dr.GetString(12);
                            Pac.Email = dr.GetString(13);
                            Pac.TelfFijo = dr.GetString(14);
                            Pac.TelfCel = dr.GetString(15);
                            Pac.CodTipPac = dr.GetInt32(16);
                            Pac.Observ = dr.GetString(17);
                            Pac.Discap = dr.GetBoolean(18);
                            Pac.TipSang = dr.GetString(19);
                            Pac.Alerg = dr.GetString(20);
                            Pac.TitParent = dr.GetString(21);
                            Pac.TitNom = dr.GetString(22);
                            Pac.TitDni = dr.GetString(23);
                            Pac.TitObs = dr.GetString(24);
                            Pac.Ocup = dr.GetString(25);
                            Pac.DirTrab = dr.GetString(26);
                            Pac.TelTrab = dr.GetString(27);
                            Pac.CodCatPac = dr.GetString(28);
                            Pac.CodEstCivil = dr.GetString(29);


                            Pac.CodSexo = dr.GetString(30);
                            Pac.CodTipFil = dr.GetString(31);
                            Pac.CodDocIdent = dr.GetString(32);
                            Pac.CodDist = dr.GetString(33);
                            Pac.CodProv = dr.GetString(34);
                            Pac.CodDep = dr.GetString(35);
                            Pac.CodPais = dr.GetString(36);
                            Pac.CodUsu = dr.GetString(37);
                            Pac.EstPac = dr.GetBoolean(38);
                            Pac.nombCompleto = dr.GetInt32(0) + " - " + dr.GetString(3) + " " + dr.GetString(1) + " " + dr.GetString(2);
                            Pac.CodAnt = dr.GetInt32(39);
                            Pac.CancerP = dr.GetBoolean(40);
                            Pac.DiabetesP = dr.GetBoolean(41);
                            Pac.ACVP = dr.GetBoolean(42);
                            Pac.AlergiaP = dr.GetBoolean(43);
                            Pac.HipertArtP = dr.GetBoolean(44);
                            Pac.OtrosP = dr.GetBoolean(45);

                            Pac.CancerF = dr.GetBoolean(46);

                            Pac.DiabetesF = dr.GetBoolean(47);
                            Pac.ACVF = dr.GetBoolean(48);
                            Pac.AlergiaF = dr.GetBoolean(49);
                            Pac.HipertArtF = dr.GetBoolean(50);
                            Pac.OtrosF = dr.GetBoolean(51);
                            Pac.ObservacionP = dr.GetString(52);
                            Pac.ObservacionF = dr.GetString(53);
                            Pac.DiscaObs = (dr["DiscaObs"] is DBNull) ? string.Empty : dr["DiscaObs"].ToString();

                            Lista.Add(Pac);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_Pacientes> ListadoPacientesFiltro(string fecha = null, string dni = null, string nombre = null)
        {
            List<E_Pacientes> Lista = new List<E_Pacientes>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Busca_Pacientes_Lista", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (fecha == null || fecha == "")
                    {
                        cmd.Parameters.AddWithValue("@FecAfil", System.Data.SqlTypes.SqlDateTime.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@FecAfil", fecha);
                    }
                    if (dni == null || dni == "")
                    {
                        cmd.Parameters.AddWithValue("@dni", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@dni", dni);
                    }
                    if (nombre == null || nombre == "")
                    {
                        cmd.Parameters.AddWithValue("@nombre", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                    }




                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Pacientes Pac = new E_Pacientes();

                            Pac.Historia = dr.GetInt32(0);
                            Pac.ApePat = dr.GetString(1);
                            Pac.ApeMat = dr.GetString(2);
                            Pac.NomPac = dr.GetString(3);
                            Pac.FecNac = dr.GetDateTime(4);
                            Pac.Sector = dr.GetString(5);
                            Pac.NumDoc = dr.GetString(6);
                            Pac.Ruc = dr.GetString(7);
                            Pac.Essalud = dr.GetString(8);
                            Pac.CodAseg = dr.GetString(9);
                            Pac.FecAfil = dr.GetDateTime(10);
                            Pac.LugarNac = dr.GetString(11);
                            Pac.Direcc = dr.GetString(12);
                            Pac.Email = dr.GetString(13);
                            Pac.TelfFijo = dr.GetString(14);
                            Pac.TelfCel = dr.GetString(15);
                            Pac.CodTipPac = dr.GetInt32(16);
                            Pac.Observ = dr.GetString(17);
                            Pac.Discap = dr.GetBoolean(18);
                            Pac.TipSang = dr.GetString(19);
                            Pac.Alerg = dr.GetString(20);
                            Pac.TitParent = dr.GetString(21);
                            Pac.TitNom = dr.GetString(22);
                            Pac.TitDni = dr.GetString(23);
                            Pac.TitObs = dr.GetString(24);
                            Pac.Ocup = dr.GetString(25);
                            Pac.DirTrab = dr.GetString(26);
                            Pac.TelTrab = dr.GetString(27);
                            Pac.CodCatPac = dr.GetString(28);
                            Pac.CodEstCivil = dr.GetString(29);


                            Pac.CodSexo = dr.GetString(30);
                            Pac.CodTipFil = dr.GetString(31);
                            Pac.CodDocIdent = dr.GetString(32);
                            Pac.CodDist = dr.GetString(33);
                            Pac.CodProv = dr.GetString(34);
                            Pac.CodDep = dr.GetString(35);
                            Pac.CodPais = dr.GetString(36);
                            Pac.CodUsu = dr.GetString(37);
                            Pac.EstPac = dr.GetBoolean(38);
                            Pac.nombCompleto = dr.GetInt32(0) + " - " + dr.GetString(3) + " " + dr.GetString(1) + " " + dr.GetString(2);
                            Pac.CodAnt = dr.GetInt32(39);
                            Pac.CancerP = dr.GetBoolean(40);
                            Pac.DiabetesP = dr.GetBoolean(41);
                            Pac.ACVP = dr.GetBoolean(42);
                            Pac.AlergiaP = dr.GetBoolean(43);
                            Pac.HipertArtP = dr.GetBoolean(44);
                            Pac.OtrosP = dr.GetBoolean(45);

                            Pac.CancerF = dr.GetBoolean(46);

                            Pac.DiabetesF = dr.GetBoolean(47);
                            Pac.ACVF = dr.GetBoolean(48);
                            Pac.AlergiaF = dr.GetBoolean(49);
                            Pac.HipertArtF = dr.GetBoolean(50);
                            Pac.OtrosF = dr.GetBoolean(51);
                            Pac.ObservacionP = dr.GetString(52);
                            Pac.ObservacionF = dr.GetString(53);
                            Pac.DiscaObs = (dr["DiscaObs"] is DBNull) ? string.Empty : dr["DiscaObs"].ToString();

                            Lista.Add(Pac);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }



        public List<E_Pacientes> BuscaPacientes(int historia)
        {
            List<E_Pacientes> Lista = new List<E_Pacientes>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Busca_Pacientes", con))
                {
                    cmd.Parameters.AddWithValue("@Historia", historia);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Pacientes Pac = new E_Pacientes();

                            Pac.Historia = dr.GetInt32(0);
                            Pac.ApePat = dr.GetString(1);
                            Pac.ApeMat = dr.GetString(2);
                            Pac.NomPac = dr.GetString(3);
                            Pac.FecNac = dr.GetDateTime(4);
                            Pac.Sector = dr.GetString(5);
                            Pac.NumDoc = dr.GetString(6);
                            Pac.Ruc = dr.GetString(7);
                            Pac.Essalud = dr.GetString(8);
                            Pac.CodAseg = dr.GetString(9);
                            Pac.FecAfil = dr.GetDateTime(10);
                            Pac.LugarNac = dr.GetString(11);
                            Pac.Direcc = dr.GetString(12);
                            Pac.Email = dr.GetString(13);
                            Pac.TelfFijo = dr.GetString(14);
                            Pac.TelfCel = dr.GetString(15);
                            Pac.CodTipPac = dr.GetInt32(16);
                            Pac.Observ = dr.GetString(17);
                            Pac.Discap = dr.GetBoolean(18);
                            Pac.TipSang = dr.GetString(19);
                            Pac.Alerg = dr.GetString(20);
                            Pac.TitParent = dr.GetString(21);
                            Pac.TitNom = dr.GetString(22);
                            Pac.TitDni = dr.GetString(23);
                            Pac.TitObs = dr.GetString(24);
                            Pac.Ocup = dr.GetString(25);
                            Pac.DirTrab = dr.GetString(26);
                            Pac.TelTrab = dr.GetString(27);
                            Pac.CodCatPac = dr.GetString(28);
                            Pac.CodEstCivil = dr.GetString(29);


                            Pac.CodSexo = dr.GetString(30);
                            Pac.CodTipFil = dr.GetString(31);
                            Pac.CodDocIdent = dr.GetString(32);
                            Pac.CodDist = dr.GetString(33);
                            Pac.CodProv = dr.GetString(34);
                            Pac.CodDep = dr.GetString(35);
                            Pac.CodPais = dr.GetString(36);
                            Pac.CodUsu = dr.GetString(37);
                            Pac.EstPac = dr.GetBoolean(38);


                            Lista.Add(Pac);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_Login> ListadoUsuarioPaciente()
        {
            List<E_Login> Lista = new List<E_Login>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Usuario_Pacientes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Login Log = new E_Login();

                            Log.CodigoUsuario = dr.GetString(0);
                            Log.AliasUsu = dr.GetString(1);
                            //Log.Nombres = dr.GetString(2);


                            Lista.Add(Log);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_Historial> Usp_Historia_Paciente(int id)
        {
            List<E_Historial> Lista = new List<E_Historial>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Historia_Paciente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codpac", id);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Historial Log = new E_Historial();
                            // e.CodEspec , s.NomServ , m.NomMed , fe.FecRegMed ,cd.TurnoAten  , t.DescTar  , fe.item , fe.FE
                            Log.CodEspec = dr.GetString(0);
                            Log.NomServ = dr.GetString(1);
                            Log.NomMed = dr.GetString(2);
                            Log.FecRegMed = dr.GetString(3);
                            Log.Turno = dr["TurnoAten"] is DBNull ? string.Empty : dr["TurnoAten"].ToString();
                            Log.DescripcionProc = dr["DescTar"] is DBNull ? string.Empty : dr["DescTar"].ToString();
                            Log.Item = dr.GetInt32(6);
                            Log.FE = dr.GetInt32(7);
                            Log.FechaAtencion = DateTime.Parse(dr.GetString(9));
                            Log.FechaPago = dr.GetDateTime(10);
                            Lista.Add(Log);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_Ficha_Electronica> Usp_Diagnostico_Paciente(int id)
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Diagnostico_Paciente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codpac", id);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Ficha_Electronica Log = new E_Ficha_Electronica();
                            // fe.FE , d.Cie10 , d.Descripcion , esp.CodEspec
                            Log.FE = dr.GetInt32(0);
                            Log.CIe10 = dr.GetString(1);
                            Log.Descripcion = dr.GetString(2);
                            Log.Especialidad = dr.GetString(3);
                            Lista.Add(Log);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public List<E_Especialidades> Usp_Especialidades_XPac(int id)
        {
            List<E_Especialidades> Lista = new List<E_Especialidades>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Especialidades_XPac", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codpac", id);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Especialidades Log = new E_Especialidades();

                            Log.CodEspec = dr.GetString(0);
                            Log.NomEspec = dr.GetString(1);

                            Lista.Add(Log);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_Usuario_Servicio> usp_UsuarioEspecialidadGeneral(string id)
        {
            List<E_Usuario_Servicio> Lista = new List<E_Usuario_Servicio>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_UsuarioEspecialidadGeneral", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodUsu", id);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Usuario_Servicio Log = new E_Usuario_Servicio();

                            Log.CodUsu = dr.GetString(0);
                            Log.CodServ = dr.GetString(1);

                            Lista.Add(Log);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Ficha_Electronica> Usp_Receta_Paciente(int id)
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Receta_Paciente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codpac", id);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            E_Ficha_Electronica Log = new E_Ficha_Electronica();
                            Log.FE = dr.GetInt32(0);
                            Log.Descripcion = dr.GetString(1);
                            Log.CodEspec = dr.GetString(2);
                            Log.Dosis = dr.GetString(3);
                            Log.cant = dr.GetInt32(4);
                            Log.Duracion = dr.GetString(5);
                            Log.Frecuencia = dr.GetInt32(6);
                            Log.ViaAdmin = dr.GetString(7);
                            Lista.Add(Log);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        //        Usp_Examen_Axiliares --1
        //@codpac int 
        //as
        //select fe.Fe , esp.CodEspec , t.DescTar , ea.cant , esp.CodEspec


        public string getedadActual(string fecha)
            {
            string edad = ""; 
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("getedadActual", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                           edad = dr.GetString(0);

                        }
                        con.Close();
                    }

                }
               
            }
            return edad;
        }

        public JsonResult getedad(string fecha) {
            if (!string.IsNullOrWhiteSpace(fecha))
            {
                var x = getedadActual(fecha);
                return Json(x, JsonRequestBehavior.AllowGet);
            }
            else {
                return null; 
            }
        }

        public List<E_Ficha_Electronica> Usp_Examen_Axiliares(int id)
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Examen_Axiliares", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codpac", id);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            E_Ficha_Electronica Log = new E_Ficha_Electronica();
                            Log.FE = dr.GetInt32(0);
                            Log.CodEspec = dr.GetString(1);
                            Log.DescTar = dr["DescTar"] is DBNull ? string.Empty : dr["DescTar"].ToString();
                            Log.cant = dr.GetInt32(3);

                            Lista.Add(Log);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }




        public ActionResult HistoriaPacienteXespecialidad(int id)
        {
            ViewBag.modal = "2";
            string sede = Session["codSede"].ToString();
            ViewBag.historia = id;
            var pac = ListadoPacientes().Where(x => x.Historia == id).FirstOrDefault();
            var especialidad = new EspecialidadController().ListadoEspecialidades().Where(x => x.CodSed == sede).ToList();
            ViewBag.especialidad = new SelectList(especialidad, "CodEspec", "NomEspec");
            ViewBag.Nombre = pac.nombCompleto;
            ViewBag.historial = Usp_Historia_Paciente(id);
            return View();
        }

        [HttpPost]
        public ActionResult HistoriaPacienteXespecialidad(E_Historial model, string[] arreglo = null)
        {

            ViewBag.modal = "2";
            string sede = Session["codSede"].ToString();
            var especialidad = new EspecialidadController().ListadoEspecialidades().Where(x => x.CodSed == sede).ToList();
            ViewBag.especialidad = new SelectList(especialidad, "CodEspec", "NomEspec");
            ViewBag.historia = model.historia;
            var pac = ListadoPacientes().Where(x => x.Historia == model.historia).FirstOrDefault();
            ViewBag.Nombre = pac.nombCompleto;
            //ViewBag.especialidad = Usp_Especialidades_XPac(id);
            ViewBag.fechaI = model.fechaI;
            ViewBag.fechaF = model.fechaF;
            var query = Usp_Historia_Paciente(model.historia).ToList();

            if (model.Evento == "1")
            {

                if (!string.IsNullOrWhiteSpace(model.fechaI) && !string.IsNullOrWhiteSpace(model.fechaF))
                {

                    if (model.CodEspec != null)
                    {
                        query = query.Where(x => x.FechaAtencion >= DateTime.Parse(model.fechaI) && x.FechaAtencion <= DateTime.Parse(model.fechaF) && x.CodEspec == model.CodEspec).ToList();
                    }
                    else
                    {
                        query = query.Where(x => x.FechaAtencion >= DateTime.Parse(model.fechaI) && x.FechaAtencion <= DateTime.Parse(model.fechaF)).ToList();
                    }

                    ViewBag.historial = query.ToList();
                    return View();

                }
                else
                {
                    if (model.CodEspec != null)
                    {
                        query = query.Where(x => x.CodEspec == model.CodEspec).ToList();
                        ViewBag.historial = query.ToList();
                    }
                    else
                    {
                        ViewBag.historial = query.ToList();
                    }
                }
            }

            if (model.Evento == "2")
            {
                if (arreglo.Count() != 0)
                {
                    string[] split = null;
                    foreach (var item in arreglo)
                    {
                        split = item.Split(',');
                    }
                    model.FE = Convert.ToInt32(split[0].ToString());
                    string codesp = split[1].ToString();
                    var esp = Usp_Diagnostico_Paciente(model.historia);
                    var diagnostico = esp.Where(x => x.Especialidad == codesp && x.FE == model.FE).ToList();
                    var receta = Usp_Receta_Paciente(model.historia).Where(x => x.CodEspec == codesp && x.FE == model.FE).ToList();
                    var examen = Usp_Examen_Axiliares(model.historia).Where(x => x.CodEspec == codesp && x.FE == model.FE).ToList();
                    if (diagnostico.Count() == 0) ViewBag.diagnostico = null; else ViewBag.diagnostico = diagnostico;
                    if (receta.Count() == 0) { ViewBag.receta = null; } else { ViewBag.receta = receta; }
                    if (examen.Count() == 0) { ViewBag.examen = null; } else { ViewBag.examen = examen; }
                    ViewBag.modal = "1";

                    if (!string.IsNullOrWhiteSpace(model.fechaI) && !string.IsNullOrWhiteSpace(model.fechaF))
                    {

                        if (model.CodEspec != null)
                        {
                            query = query.Where(x => x.FechaAtencion >= DateTime.Parse(model.fechaI) && x.FechaAtencion <= DateTime.Parse(model.fechaF) && x.CodEspec == model.CodEspec).ToList();
                        }
                        else
                        {
                            query = query.Where(x => x.FechaAtencion >= DateTime.Parse(model.fechaI) && x.FechaAtencion <= DateTime.Parse(model.fechaF)).ToList();
                        }

                        ViewBag.historial = query.ToList();
                        return View();

                    }
                    else
                    {
                        if (model.CodEspec != null)
                        {
                            query = query.Where(x => x.CodEspec == model.CodEspec).ToList();
                            ViewBag.historial = query.ToList();
                        }
                        else
                        {
                            ViewBag.historial = query.ToList();
                        }
                    }



                }
                else
                {
                    ViewBag.diagnostico = null;
                    ViewBag.receta = null;
                    ViewBag.examen = null;
                    ViewBag.modal = "2";
                }

            }



            return View();

        }

        public ActionResult Informes(string CodEspec = null, string CodServ = null, string fechaCita = null, string CodMed = null, string Turno = null, int? id = null, string cadena = null, string CodCue = null, string cuenta = null, int? dimension = null)
        {
            CitasController citas = new CitasController();
            EspecialidadController es = new EspecialidadController();
            int x1 = 0;
            ViewBag.id = id;
            if (dimension != null)
            {
                x1 = (int)dimension;
            }

            string sede = Session["codSede"].ToString();
            if (CodCue != null)
            {
                ViewBag.cuenta = CodCue;
            }
            else
            {
                ViewBag.cuenta = "";
            }


            MedicosController h = new MedicosController();
            ConsultorioController co = new ConsultorioController();

            try
            {

                if (CodEspec != null && CodServ != null && fechaCita != null && CodMed != null && Turno != null && CodCue != null)
                {
                    ViewBag.especialidad = CodEspec;
                    ViewBag.servicio = CodServ;
                    ViewBag.idCliente = id;
                    ViewBag.turno = Turno;
                    ViewBag.fechaSeleccionada = fechaCita;
                    ViewBag.medico = CodMed;
                    ViewBag.dimension = x1;
                    DateTime fechaCambia = DateTime.Parse(fechaCita);
                    CultureInfo culture = new CultureInfo("es-PE");
                    string demo, demo1, demo2;
                    demo2 = fechaCambia.ToString("yyyy-MM-dd", culture);
                    DateTime nuevo = DateTime.Parse(demo2);
                    E_HorarioMedico reg = h.ListadoHorarioMedico().Find(x => x.Turno == Turno && x.CodMed == CodMed && x.dia == nuevo);
                    E_Consultorio reg1 = co.ListadoConsultorio().Find(x => x.IdConsul.Equals(reg.Consultorio));

                    ViewBag.consultorio = reg1.DescConsul;

                    demo = fechaCambia.ToString("dddd", culture);
                    demo1 = fechaCambia.ToString("dd/MM/yyyy", culture);
                    demo = demo.TrimEnd('.');



                    ViewBag.listaConsulta = (List<E_Citas>)citas.ConsultaCitas(CodEspec, CodServ, CodMed, fechaCita, Turno);
                    ViewBag.listaCitasHoy = (List<E_Citas>)citas.ConsultaCitasEspecial(CodMed, Turno, demo1);

                }
                else
                {
                    ViewBag.especialidad = "";
                    ViewBag.servicio = "";
                    ViewBag.idCliente = "";
                    ViewBag.turno = "";
                    ViewBag.fechaSeleccionada = "";
                    ViewBag.medico = "";
                    ViewBag.dimension = x1;
                }



                ViewBag.listadoEspecialidad = new SelectList(es.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true).ToList(), "CodEspec", "NomEspec");


            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Ingrese datos correctos";

            }


            return View();

        }


        public ActionResult InformeGenerada(int id, string cadena = null, string cuenta = null)
        {
            string sede = Session["codSede"].ToString();
            CitasController citas = new CitasController();
            EspecialidadController es = new EspecialidadController();
            if (cuenta != null)
            {
                ViewBag.cuenta = cuenta;
            }
            else
            {
                ViewBag.cuenta = "";
            }


            string especialidad = "";
            string servicio = "";
            ViewBag.cadena = cadena;
            ViewBag.dimension = "";
            string turno = "";
            string medico = "";
            string fecha = "";
            string dimension = "";


            string[] fija = cadena.Split(',');
            int i = 1;
            foreach (string item in fija)
            {
                if (i == 1)
                {
                    especialidad = item;
                }
                else if (i == 2)
                {
                    servicio = item;
                }
                else if (i == 3)
                {
                    fecha = item;
                }
                else if (i == 4)
                {
                    medico = item;
                }
                else if (i == 5)
                {
                    turno = item;
                }
                else if (i == 6)
                {
                    dimension = item;
                }

                i++;

            }

            MedicosController h = new MedicosController();
            ConsultorioController co = new ConsultorioController();

            try
            {

                ViewBag.especialidad = especialidad;
                ViewBag.servicio = servicio;
                ViewBag.idCliente = id;
                ViewBag.turno = turno;
                ViewBag.fechaSeleccionada = fecha;
                ViewBag.medico = medico;
                ViewBag.dimension = dimension;
                DateTime fechaCambia = DateTime.Parse(fecha);
                CultureInfo culture = new CultureInfo("es-PE");
                string demo, demo1, demo2;
                demo2 = fechaCambia.ToString("yyyy-MM-dd", culture);
                DateTime nuevo = DateTime.Parse(demo2);
                E_HorarioMedico reg = h.ListadoHorarioMedico().Find(x => x.Turno == turno && x.CodMed == medico && x.dia == nuevo);
                E_Consultorio reg1 = co.ListadoConsultorio().Find(x => x.IdConsul.Equals(reg.Consultorio));

                ViewBag.consultorio = reg1.DescConsul;

                demo = fechaCambia.ToString("dddd", culture);
                demo1 = fechaCambia.ToString("dd/MM/yyyy", culture);
                demo = demo.TrimEnd('.');

                ViewBag.listadoEspecialidad = new SelectList(es.ListadoEspecialidades().Where(x => x.CodSed == sede).ToList(), "CodEspec", "NomEspec", especialidad);
                ViewBag.listaConsulta = (List<E_Citas>)citas.ConsultaCitas(especialidad, servicio, medico, fecha, turno);
                ViewBag.listaCitasHoy = (List<E_Citas>)citas.ConsultaCitasEspecial(medico, turno, demo1);

            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Ingrese datos correctos";

            }
            return View();


        }

        public ActionResult BusquedaPaciente(E_Citas c, string fecha = null, string dni = null, string nombre = null)
        {
            UtilitarioController u = new UtilitarioController();
            E_Master hor = u.ListadoHoraServidor().FirstOrDefault();

            ViewBag.fecha = fecha;
            ViewBag.dni = dni;
            ViewBag.nombre = nombre;

            if (fecha == null || fecha == "")
            {
                fecha = hor.HoraServidor.ToShortDateString();
            }

            ViewBag.turno = c.Turno;
            ViewBag.fechaSeleccionada = c.fechaCita;
            ViewBag.consultorio = c.Consultorio;
            ViewBag.copiaIntervalo = c.intervalo;
            ViewBag.CodMed = c.CodMed;
            ViewBag.CodServ = c.CodServ;
            ViewBag.CodEspec = c.CodEspec;
            ViewBag.NomMed = c.NomMed;
            ViewBag.HoraInicio = c.HoraInicio;
            ViewBag.HoraFin = c.HoraFin;
            ViewBag.CodCue = c.CodCue;


            return View(ListadoPacientesFiltro(fecha, dni, nombre));
        }





        public ActionResult ImprimirHistoriaClinica(int Historia)
        {
            var Lista = ListarDatosHistoriaClinica(Historia).FirstOrDefault();
            //return RedirectPermanent("~/Pacientes/ImprimirHistoriaClinica?Historia=" + Historia);
            return View(Lista);
        }



        public List<E_Pacientes> ListarDatosHistoriaClinica(int Historia)
        {
            List<E_Pacientes> Lista = new List<E_Pacientes>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("UspListarDatosHistoriaClinica", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Historia", Historia);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            E_Pacientes p = new E_Pacientes();
                            p.Historia = Convert.ToInt32(dr["Historia"]);
                            p.nombCompleto = dr["NombrePaciente"].ToString();
                            p.NonSex = dr["NomSexo"].ToString();
                            p.NomEstCivil = dr["NomEstCivil"].ToString();
                            p.LugarNac = dr["LugarNac"].ToString();
                            p.FecNac = Convert.ToDateTime(dr["FecNac"].ToString());
                            p.Edad = dr["Edad"].ToString();
                            p.NomDist = dr["NomDist"].ToString();
                            p.Direcc = dr["Direcc"].ToString();
                            p.TelfFijo = dr["TelfFijo"].ToString();
                            p.TelfCel = dr["TelfCel"].ToString();
                            p.Email = dr["Email"].ToString();
                            p.FecAfil = Convert.ToDateTime(dr["FecAfil"].ToString());
                            p.Crea= dr["Crea"].ToString();

                            //p.DescTar = dr["DescTar"] is DBNull ? string.Empty : dr["DescTar"].ToString();

                            Lista.Add(p);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }




        public List<E_Pacientes> ListarSexoPaciente(int Id)
        {
            List<E_Pacientes> Lista = new List<E_Pacientes>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_ListarSexoPaciente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Historia", Id);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            E_Pacientes p = new E_Pacientes();
                            p.CodSexo = dr["CodSexo"].ToString();

                            Lista.Add(p);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public JsonResult VenderConsulta()
        {
            E_Servicios Ser = new E_Servicios();

            string Tbody = "";

            var resultado = ListarServicios();
            foreach (var item in resultado)
            {
                Tbody += $"<tr><td>{item.CodServ}</td>";
                Tbody += $"<td>{item.NomServ}</td>";
                Tbody += $"<td><a style='cursor:pointer' onclick='AgregarConsulta();' ><span style='Color:green' class='fa fa-plus-square'></span></a></td></tr>";
            }
            return Json(Tbody, JsonRequestBehavior.AllowGet);
            
        }

        public List<E_Servicios> ListarServicios()
        {
            SqlCommand cmd;
            SqlConnection db;
            string cadena = ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString;

            string sede = Session["CodSede"].ToString();

            List<E_Servicios> ListaServ = new List<E_Servicios>();

            try
            {
                using (db = new SqlConnection(cadena))
                {
                    using (cmd = new SqlCommand("Usp_ListarServicios_VentaRapida", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@CodOdontograma", CodOdo);
                        cmd.Parameters.AddWithValue("@CodSede", sede);
                        db.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        
                        while (dr.Read())
                        {
                            E_Servicios Serv = new E_Servicios();

                            Serv.CodServ = dr["CodServ"].ToString();
                            Serv.NomServ = dr["NomServ"].ToString();
                            Serv.CodEspec = dr["CodEspec"].ToString();

                            ListaServ.Add(Serv);

                        }

                    }
                }

                return ListaServ;
            }
            catch (Exception e)
            {
                throw;
            }

        }





    }
}