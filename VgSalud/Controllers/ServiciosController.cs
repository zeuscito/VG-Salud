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
    public class ServiciosController : Controller
    {
        // GET: Servicios
      
        public ActionResult RegistrarServicios()
        {
            string sede = Session["codSede"].ToString();
            ViewBag.abreModal = "";
            TipoTarifaController tipot = new TipoTarifaController(); 
            ViewBag.tipotarifa = new SelectList(tipot.ListadoTipoTarifa().Where(x=>x.EstTipTar == true).ToList(), "CodTipTar", "DescTipTar"); 
            EspecialidadController Espe = new EspecialidadController();
            ViewBag.ListaEspecialidad = new SelectList(Espe.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true), "CodEspec", "NomEspec");
            EmpresaTerceroController EmpTer = new EmpresaTerceroController();
            ViewBag.ListaEmpresaTercero = new SelectList(EmpTer.ListadoEmpresaTercero(sede).Where(x =>x.EstEmp == true), "CodEmp", "RazonEmp");

            SedesController Sed = new SedesController();
            ViewBag.ListaSedes = new SelectList(Sed.ListadoSedes(), "CodSede", "NomSede");

            ViewBag.Sedes = (List<E_Sede>)Sed.ListadoSedes();


            if (Session["PagoTipoTarifa"] == null)
            {
                List<E_Servicios> Lista = new List<E_Servicios>(); 
                foreach (var item in tipot.ListadoTipoTarifa())
                {
                    E_Servicios serv = new E_Servicios();
                    serv.CodTipTar = item.CodTipTar;
                    serv.DescTipoTar = item.DescTipTar;
                    serv.porcentaje = 0;
                    Lista.Add(serv); 
                }
                Session["PagoTipoTarifa"] = Lista;


            }
            else {
                Session.Remove("PagoTipoTarifa");
                Session["PagoTipoTarifa"] = new List<E_Servicios>();
                List<E_Servicios> Lista = new List<E_Servicios>();
                var formulario = (List<E_Servicios>)Session["PagoTipoTarifa"]; 
                foreach (var item in tipot.ListadoTipoTarifa().Where(x=>x.EstTipTar == true).ToList())
                {
                    E_Servicios serv = new E_Servicios();
                    serv.CodTipTar = item.CodTipTar;
                    serv.DescTipoTar = item.DescTipTar;
                    serv.porcentaje = 0;
                    formulario.Add(serv); 
                }
                Session["PagoTipoTarifa"] = Lista;

            }

            ViewBag.pagoTarifa = Session["PagoTipoTarifa"];
            return View();
            
        }

        [HttpPost]
        public ActionResult RegistrarServicios(E_Servicios ESer)
        {
            string sede = Session["codSede"].ToString();
            TipoTarifaController tipot = new TipoTarifaController();
            ViewBag.tipotarifa = new SelectList(tipot.ListadoTipoTarifa().Where(x => x.EstTipTar == true).ToList(), "CodTipTar", "DescTipTar");
            EspecialidadController Espe = new EspecialidadController();
            ViewBag.ListaEspecialidad = new SelectList(Espe.ListadoEspecialidades().Where(x => x.CodSed == sede), "CodEspec", "NomEspec", ESer.CodEspec);
            EmpresaTerceroController EmpTer = new EmpresaTerceroController();
            ViewBag.ListaEmpresaTercero = new SelectList(EmpTer.ListadoEmpresaTercero(sede), "CodEmp", "RazonEmp", ESer.CodEmp);
            SedesController Sed = new SedesController();
            ViewBag.ListaSedes = new SelectList(Sed.ListadoSedes(), "CodSede", "NomSede", ESer.CodSede);
            ViewBag.pagoTarifa = Session["PagoTipoTarifa"];

            string Crea = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;
            DatosGeneralesController dat = new DatosGeneralesController();
         
            try {
                if (ESer.Evento == "1")
                {
                    var DG = dat.Getdatogenerales();
                    int ServicioCountDG = DG.servicio == null ? 0 : Convert.ToInt32(DG.servicio);
                    int ServicioCount = ListadoServicios().Count();
                    if (ServicioCountDG > ServicioCount)
                    {

                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand("Usp_MtoServicios", con))
                            {
                                try
                                {
                                    cmd.Parameters.AddWithValue("@CodServ", "");
                                    cmd.Parameters.AddWithValue("@NomServ", ESer.NomServ.ToUpper());
                                    cmd.Parameters.AddWithValue("@CodEspec", ESer.CodEspec.ToUpper());
                                    cmd.Parameters.AddWithValue("@CodEmp", ESer.CodEmp);
                                    cmd.Parameters.AddWithValue("@CodSede", sede);
                                    cmd.Parameters.AddWithValue("@EstServ", true);
                                    cmd.Parameters.AddWithValue("@Crea", Crea);
                                    cmd.Parameters.AddWithValue("@Modifica", "");
                                    cmd.Parameters.AddWithValue("@Elimina", "");
                                    cmd.Parameters.AddWithValue("@Evento", 1);
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    string servicio = cmd.ExecuteScalar().ToString();
                                    cmd.Dispose();
                                    ViewBag.Mensaje = "Se registro Satisfactoriamente";
                                    if (servicio.Length != 0)
                                    {
                                        var formulario = (List<E_Servicios>)Session["PagoTipoTarifa"];
                                        foreach (var item in formulario)
                                        {
                                            using (SqlCommand cmd1 = new SqlCommand("usp_Mantenimiento_servicio_TipoTarifa", con))
                                            {

                                                cmd1.Parameters.AddWithValue("@CodServ", servicio);
                                                cmd1.Parameters.AddWithValue("@CodTipTar", item.CodTipTar);
                                                cmd1.Parameters.AddWithValue("@porcentaje", item.porcentaje);
                                                cmd1.Parameters.AddWithValue("@Crea", Crea);
                                                cmd1.Parameters.AddWithValue("@Modifica", "");
                                                cmd1.Parameters.AddWithValue("@Elimina", "");
                                                cmd1.Parameters.AddWithValue("@Evento", 1);
                                                cmd1.CommandType = CommandType.StoredProcedure;

                                                cmd1.ExecuteNonQuery();
                                                cmd.Dispose();
                                                ViewBag.Mensaje = "Se actualizo Satisfactoriamente";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.Mensaje = "Error";

                                    }
                                }
                                catch (Exception ex)
                                {
                                    ViewBag.mensaje = "Error : Datos [NO VALIDOS]";
                                    ViewBag.abreModal = "";
                                    return View(ESer);
                                }
                                finally
                                {
                                    con.Close();

                                }
                            }

                        }
                        return RedirectToAction("ListaServicios");

                    }
                    else {
                        ViewBag.mensaje = "Error Excedio el Limite Permitido de Registros!!!... Consulte con su Administrador";
                        ViewBag.abreModal = ""; 
                        return View(ESer); 
                    }

                   
                }
                else if (ESer.Evento == "2")
                {
                    var formulario = (List<E_Servicios>)Session["PagoTipoTarifa"];
                    var validador = formulario.Where(x => x.CodTipTar == ESer.CodTipTar).FirstOrDefault();
                    if (validador == null)
                    {

                        E_Servicios serv = new E_Servicios();
                        serv.CodTipTar = ESer.CodTipTar;
                        var TipoTarifa = tipot.ListadoTipoTarifa().Where(x => x.CodTipTar == ESer.CodTipTar).FirstOrDefault();
                        serv.DescTipoTar = TipoTarifa.DescTipTar;
                        serv.porcentaje = ESer.porcentaje;
                        formulario.Add(serv);
                        Session["PagoTipoTarifa"] = formulario;
                        ViewBag.pagoTarifa = (List<E_Servicios>)Session["PagoTipoTarifa"];

                    }
                    else
                    {

                        E_Servicios serv = new E_Servicios();
                        var index = formulario.Where(x => x.CodTipTar == ESer.CodTipTar).FirstOrDefault();
                        var formularios = (List<E_Servicios>)Session["PagoTipoTarifa"];
                        formularios.Remove(index);
                        Session["PagoTipoTarifa"] = formularios;
                        var TipoTarifa = tipot.ListadoTipoTarifa().Where(x => x.CodTipTar == ESer.CodTipTar).FirstOrDefault();
                        E_Servicios modificado = new E_Servicios();
                        modificado.CodTipTar = ESer.CodTipTar;
                        modificado.DescTipoTar = TipoTarifa.DescTipTar;
                        modificado.porcentaje = ESer.porcentaje;
                        formularios.Add(modificado);
                        Session["PagoTipoTarifa"] = formularios;
                        ViewBag.pagoTarifa = (List<E_Servicios>)Session["PagoTipoTarifa"];

                    }

                    ViewBag.abreModal = "2";
                    return View(ESer);

                }
                else if (ESer.Evento == "3")
                {
                    var formulario = (List<E_Servicios>)Session["PagoTipoTarifa"];
                    var index = formulario.Where(x => x.CodTipTar == ESer.CodTipTar).FirstOrDefault();
                    formulario.Remove(index);
                    Session["PagoTipoTarifa"] = formulario;
                    ViewBag.pagoTarifa = (List<E_Servicios>)Session["PagoTipoTarifa"];
                    ViewBag.abreModal = "3";
                    return View(ESer);

                }
            } catch (Exception) {
                ViewBag.mensaje = "Error: Datos [No Validos]";
                ViewBag.abreModal = "";
                return View(ESer);

            }
        
           
            return RedirectToAction("ListaServicios");

        }


        public ActionResult ModificarServicios(string Id)
        {

            string sede = Session["codSede"].ToString();
            ServicioTipoTarifaController servTip = new ServicioTipoTarifaController();
            var servTipTar = servTip.listaservicioTipoTarifa().Where(x => x.codserv == Id).ToList(); 
            ViewBag.tarifa = servTipTar; 
            SedesController Sed = new SedesController();
            ViewBag.ListaSedes = new SelectList(Sed.ListadoSedes(), "CodSede", "NomSede");

            ViewBag.Sedes = (List<E_Sede>)Sed.ListadoSedes();
            TipoTarifaController tipot = new TipoTarifaController();
            ViewBag.tipotarifa = new SelectList(tipot.ListadoTipoTarifa().Where(x => x.EstTipTar == true).ToList(), "CodTipTar", "DescTipTar");

            EspecialidadController Espe = new EspecialidadController();
            ViewBag.ListaEspecialidad = new SelectList(Espe.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true), "CodEspec", "NomEspec");
            EmpresaTerceroController EmpTer = new EmpresaTerceroController();
            ViewBag.ListaEmpresaTercero = new SelectList(EmpTer.ListadoEmpresaTercero(sede).Where(x =>x.EstEmp == true), "CodEmp", "RazonEmp");

            var lista = (from x in ListadoServicios() where x.CodServ == Id select x).FirstOrDefault();
                return View(lista);
           
        }

        [HttpPost]
        public ActionResult ModificarServicios(E_Servicios ESer)
        {
            string sede = Session["codSede"].ToString();
            ServicioTipoTarifaController servTip = new ServicioTipoTarifaController();

            var servTipTar = servTip.listaservicioTipoTarifa().Where(x => x.codserv == ESer.CodServ).ToList();
            ViewBag.tarifa = servTipTar;

            EspecialidadController Espe = new EspecialidadController();
            ViewBag.ListaEspecialidad = new SelectList(Espe.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true), "CodEspec", "NomEspec", ESer.CodEspec);

            TipoTarifaController tipot = new TipoTarifaController();
            ViewBag.tipotarifa = new SelectList(tipot.ListadoTipoTarifa().Where(x => x.EstTipTar == true), "CodTipTar", "DescTipTar",ESer.CodTipTar);

            EmpresaTerceroController EmpTer = new EmpresaTerceroController();
            ViewBag.ListaEmpresaTercero = new SelectList(EmpTer.ListadoEmpresaTercero(sede).Where(x =>x.EstEmp == true), "CodEmp", "RazonEmp",ESer.CodEmp);

            SedesController Sed = new SedesController();
            ViewBag.ListaSedes = new SelectList(Sed.ListadoSedes(), "CodSede", "NomSede", ESer.CodSede);

            string Modificar = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            if (ESer.Evento == "1")
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd1 = new SqlCommand("usp_Mantenimiento_servicio_TipoTarifa", con))
                    {

                        cmd1.Parameters.AddWithValue("@CodServ", ESer.CodServ);
                        cmd1.Parameters.AddWithValue("@CodTipTar", ESer.CodTipTar);
                        cmd1.Parameters.AddWithValue("@porcentaje", ESer.porcentaje);
                        cmd1.Parameters.AddWithValue("@Crea", Modificar);
                        cmd1.Parameters.AddWithValue("@Modifica", Modificar);
                        cmd1.Parameters.AddWithValue("@Elimina", "");
                        cmd1.Parameters.AddWithValue("@Evento", 1);
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.ExecuteNonQuery();
                        cmd1.Dispose();


                    }
                } 

                ViewBag.tarifa = servTip.listaservicioTipoTarifa().Where(x => x.codserv == ESer.CodServ).ToList();
                return View(ESer);

            }
            else if (ESer.Evento == "2")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd1 = new SqlCommand("usp_Mantenimiento_servicio_TipoTarifa", con))
                    {

                        cmd1.Parameters.AddWithValue("@CodServ", ESer.CodServ);
                        cmd1.Parameters.AddWithValue("@CodTipTar", ESer.CodTipTar);
                        cmd1.Parameters.AddWithValue("@porcentaje","");
                        cmd1.Parameters.AddWithValue("@Crea", "");
                        cmd1.Parameters.AddWithValue("@Modifica", "");
                        cmd1.Parameters.AddWithValue("@Elimina", "");
                        cmd1.Parameters.AddWithValue("@Evento", 3);
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.ExecuteNonQuery();
                        cmd1.Dispose();


                    }
                }

                ViewBag.tarifa = servTip.listaservicioTipoTarifa().Where(x => x.codserv == ESer.CodServ).ToList();
                return RedirectPermanent("ModificarServicios?id=" + ESer.CodServ);
            }
        

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_MtoServicios", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodServ", ESer.CodServ);
                        cmd.Parameters.AddWithValue("@NomServ", ESer.NomServ.ToUpper());
                        cmd.Parameters.AddWithValue("@CodEspec", ESer.CodEspec.ToUpper());
                        cmd.Parameters.AddWithValue("@CodEmp", ESer.CodEmp);
                        cmd.Parameters.AddWithValue("@CodSede", sede);
                        cmd.Parameters.AddWithValue("@EstServ", ESer.EstServ);
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica", Modificar);
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Evento", 2);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        
                
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al modificar : " + ex.Message.ToString();
                        return View(ESer);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectToAction("ListaServicios");
            }
        }

        public ActionResult Eliminar(E_Servicios Eser) {

            string Modificar = Session["usuario"] + " " + DateTime.Now + " " + Environment.MachineName;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Mantenimiento_servicio_TipoTarifa", con))
                {
                    try
                    {

                        cmd.Parameters.AddWithValue("@CodServ", Eser.CodServ);
                        cmd.Parameters.AddWithValue("@CodTipTar",Eser.CodTipTar);
                        cmd.Parameters.AddWithValue("@porcentaje", 0);
                        cmd.Parameters.AddWithValue("@Crea", "");
                        cmd.Parameters.AddWithValue("@Modifica","");
                        cmd.Parameters.AddWithValue("@Elimina", "");
                        cmd.Parameters.AddWithValue("@Evento", 3);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        ViewBag.Mensaje = "Se actualizo Satisfactoriamente";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Mensaje = "Ocurrio algun error al modificar : " + ex.Message.ToString();
                        return View(Eser);
                    }
                    finally
                    {
                        con.Close();

                    }
                }
                return RedirectPermanent("ModificarServicios?id=" + Eser.CodServ);
            }

        }


        public ActionResult ListaServicios()
        {
                string sede = Session["codSede"].ToString();
                return View(ListadoServicios().Where(x => x.CodSede == sede));
          
        }

        public List<E_Servicios> ListadoServicios()
        {
            List<E_Servicios> Lista = new List<E_Servicios>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Lista_Servicios", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Servicios Ser = new E_Servicios();

                            Ser.CodServ = dr.GetString(0);
                            Ser.NomServ = dr.GetString(1).ToUpper();
                            Ser.CodEspec = dr.GetString(2);
                            Ser.CodEmp = dr.GetString(3);
                            Ser.CodSede = dr.GetString(4);
                            Ser.EstServ = dr.GetBoolean(5);
                            Ser.Especialidad = dr.GetString(6).ToUpper();
                            Ser.Empresa = dr.GetString(7).ToUpper();
                            Ser.Sede = dr.GetString(8).ToUpper();
                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Servicios> ListadoServiciosVentaRapida(string sede, int historia, string descripcion)
        {
            List<E_Servicios> Lista = new List<E_Servicios>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_lista_Servicio_EspecialidadXSede", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    cmd.Parameters.AddWithValue("@historia", historia);
                    cmd.Parameters.AddWithValue("@descripcion", descripcion);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Servicios Ser = new E_Servicios();

                            Ser.CodServ = dr.GetString(0);
                            Ser.NomServ = dr.GetString(1).ToUpper();
                            Ser.CodEspec = dr.GetString(2);
                            Ser.CodEmp = dr.GetString(3);
                            Ser.CodSede = dr.GetString(4);
                            Ser.EstServ = dr.GetBoolean(5);
                            Ser.Especialidad = dr.GetString(6).ToUpper();
                            Ser.Empresa = dr.GetString(7).ToUpper();
                            Ser.Sede = dr.GetString(8).ToUpper();
                            Ser.CodTar = dr.GetString(9);
                            Ser.precio = dr["Precio"] is DBNull ? 0 : dr.GetDecimal(10);
                            Ser.DescTipoTar = dr["DescTar"] is DBNull ? "" : dr.GetString(11);
                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Servicios> ListadoTarifaEspecial(int historia, string tarifa)
        {
            List<E_Servicios> Lista = new List<E_Servicios>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_obtenerTarifaEspecial", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@historia", historia);
                    cmd.Parameters.AddWithValue("@CodTar", tarifa);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Servicios Ser = new E_Servicios();
                            Ser.CodTar = dr.GetString(0);
                            Ser.DescTipoTar = dr.GetString(1);
                            Ser.CodEspec = dr.GetString(2);
                            Ser.CodServ = dr.GetString(3);
                            Ser.precio = dr["Precio"] is DBNull ? 0 : dr.GetDecimal(4);
                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }



        public List<E_Servicios> BuscaServicio_CodEsp(string codigo, string codSede)
        {
            List<E_Servicios> Lista = new List<E_Servicios>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_buscaServicio_CodEsp", con))
                {
                    cmd.Parameters.AddWithValue("@CodEspec", codigo);
                    cmd.Parameters.AddWithValue("@CodSede", codSede);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Servicios Ser = new E_Servicios();

                            Ser.CodServ = dr.GetString(0);
                            Ser.NomServ = dr.GetString(1);
                            Ser.CodEspec = dr.GetString(2);
                            Ser.CodEmp = dr.GetString(3);
                            Ser.EstServ = dr.GetBoolean(5);
                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public ActionResult Buscar_EspecialidadEmpresa_Servicio(string id)
        {
            var evalua = (List<E_Servicios>)(from a in Buscaqueda_EspecialidadEmpresa_Servicio(id) select a).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;

        }
        public List<E_Servicios> Buscaqueda_EspecialidadEmpresa_Servicio(string codigo)
        {
            List<E_Servicios> Lista = new List<E_Servicios>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Usp_Busqueda_EspecialidadEmpresa_Servicio", con))
                {
                    cmd.Parameters.AddWithValue("@CodServ", codigo);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Servicios Ser = new E_Servicios();

                            Ser.CodEspec = dr.GetString(0);
                            Ser.Especialidad = dr.GetString(1);
                            Ser.CodEmp = dr.GetString(2);
                            Ser.Empresa = dr.GetString(3);
                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


    }
}