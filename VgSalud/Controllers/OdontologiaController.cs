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
    public class OdontologiaController : Controller
    {
        // GET: Odontologia
        public ActionResult Index()
        {
            return View();
        }

        string cadena = ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString;
        SqlConnection db;
        SqlCommand cmd;

        public ActionResult ListadoCarnetOdontologiaEnEspera(int? CodCue,string NumDoc = null)
        {
            ViewBag.lista = ListadoCarnet_Odontologia_DelDia_EnEspera();
            ViewBag.NumDoc = NumDoc;
            ViewBag.CodCue = CodCue;
            int Cuenta = 0;
            if(CodCue==null)
            {
                Cuenta = Convert.ToInt32(CodCue);
            }
            ViewBag.postergado = ListadoPostergadosOdontologia(Cuenta, NumDoc);
            
            return View();
        }

        [HttpPost]
        public ActionResult ListadoCarnetOdontologiaEnEspera(E_CSOdontologia odo, int? CodCue,string NumDoc = null)
        {
            int Cuenta = 0;
            if (NumDoc != "" || CodCue!=null)
            {
                ViewBag.lista = ListadoCarnet_Odontologia_DelDia_EnEspera();
                Cuenta = Convert.ToInt32(CodCue);
                ViewBag.postergado = ListadoPostergadosOdontologia(Cuenta, NumDoc);
                ViewBag.NumDoc = NumDoc;
                ViewBag.CodCue = CodCue;

                try
                {
                    if (odo.evento == "1")
                    {
                        Usp_MantenimientoOdontologia(odo.Id, 1);
                    }
                    else if (odo.evento == "2")
                    {
                        Usp_MantenimientoOdontologia(odo.Id, 2);
                    }
                    else if (odo.evento == "3")
                    {
                        Usp_MantenimientoOdontologia(odo.Id, 3);
                    }
                    else if (odo.evento == "4")
                    {
                        Usp_MantenimientoOdontologia(odo.Id, 4);
                    }
                    else if (odo.evento == "5")
                    {
                        Usp_MantenimientoOdontologia(odo.Id, 5);
                        ViewBag.lista = ListadoCarnet_Odontologia_DelDia_EnEspera();
                        ViewBag.NumDoc = null;
                        ViewBag.CodCue = null;
                        //return View("ListadoCarnetOdontologiaEnEspera");
                        return RedirectToAction("ListadoCarnetOdontologiaEnEspera");
                    }

                }
                catch (Exception)
                {
                    ViewBag.mensaje = "Error: No se pudo procesar la Actualizacion Correctamente";
                    return RedirectToAction("ListadoCarnetOdontologiaEnEspera");
                }

                return View("ListadoCarnetOdontologiaEnEspera");
            }
            else
            {
                ViewBag.lista = ListadoCarnet_Odontologia_DelDia_EnEspera();
                try
                {
                    if (odo.evento == "1")
                    {
                        Usp_MantenimientoOdontologia(odo.Id, 1);
                    }
                    else if (odo.evento == "2")
                    {
                        Usp_MantenimientoOdontologia(odo.Id, 2);
                    }
                    else if (odo.evento == "3")
                    {
                        Usp_MantenimientoOdontologia(odo.Id, 3);
                    }
                    else if (odo.evento == "4")
                    {
                        Usp_MantenimientoOdontologia(odo.Id, 4);
                    }
                    else if (odo.evento == "5")
                    {
                        Usp_MantenimientoOdontologia(odo.Id, 5);
                    }
                    return RedirectToAction("ListadoCarnetOdontologiaEnEspera");
                }
                catch (Exception)
                {
                    ViewBag.mensaje = "Error: No se pudo procesar la Actualizacion Correctamente";
                    return RedirectToAction("ListadoCarnetOdontologiaEnEspera");
                }
            }
            
        }


        public List<E_CSOdontologia> ListadoCarnet_Odontologia_DelDia_EnEspera()
        {
            string sede = Session["CodSede"].ToString();
            
            List<E_CSOdontologia> ListaCarnetOdo = new List<E_CSOdontologia>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("usp_ListadoCarnetOdontologia_EnEspera", db))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_CSOdontologia odo = new E_CSOdontologia();

                        odo.Id = int.Parse(dr["Id"].ToString());
                        odo.CodCue = int.Parse(dr["CodCue"].ToString());
                        odo.Paciente = dr["NombrePaciente"].ToString();
                        odo.DesTipoCarnet = dr["DescCarnet"].ToString();
                        odo.Manipulador = dr["Manipulador"].ToString();
                        odo.Prioridad = Convert.ToInt32(dr["Prioridad"].ToString());
                        odo.idEstado = Convert.ToInt32(dr["IdEstado"].ToString());
                        odo.Edad = int.Parse(dr["Edad"].ToString());
                        ListaCarnetOdo.Add(odo);

                    }


                }
            }

            return ListaCarnetOdo;
        }



        //POR CONTINUAR
        public List<E_CSOdontologia> ListadoCarnetPospuestosPorDocumento_Fecha()
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSOdontologia> ListaCarnetOdo = new List<E_CSOdontologia>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("usp_ListadoCarnetOdontologia_EnEspera", db))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_CSOdontologia odo = new E_CSOdontologia();

                        odo.Id = int.Parse(dr["Id"].ToString());
                        odo.CodCue = int.Parse(dr["CodCue"].ToString());
                        odo.Paciente = dr["NombrePaciente"].ToString();
                        odo.DesTipoCarnet = dr["DescCarnet"].ToString();
                        odo.Manipulador = dr["Manipulador"].ToString();
                        odo.Prioridad = Convert.ToInt32(dr["Prioridad"].ToString());
                        odo.idEstado = Convert.ToInt32(dr["IdEstado"].ToString());
                        odo.Edad = int.Parse(dr["Edad"].ToString());
                        ListaCarnetOdo.Add(odo);

                    }


                }
            }

            return ListaCarnetOdo;
        }


        public bool Usp_MantenimientoOdontologia(int id, int evento)
        {
            try
            {
                using (db = new SqlConnection(cadena))
                {
                    using (cmd = new SqlCommand("Usp_Validaciones_CsOdontologia", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@Evento", evento);
                        db.Open();
                        int ide = cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

        }


        public List<E_CSOdontologia> ListadoPostergadosOdontologia(int Cuenta, string NumDoc=null)
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSOdontologia> ListaCarnetOdo = new List<E_CSOdontologia>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_Odontologia_Postergados", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (NumDoc==null || NumDoc=="")
                    {
                        cmd.Parameters.AddWithValue("@NumDoc", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@NumDoc", NumDoc);
                    }
                    if (Cuenta == 0)
                    {
                        cmd.Parameters.AddWithValue("@CodCue", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CodCue", Cuenta);
                    }
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    db.Open();
                   SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_CSOdontologia odo = new E_CSOdontologia();

                        odo.Id = int.Parse(dr["Id"].ToString());
                        odo.CodCue = int.Parse(dr["CodCue"].ToString());
                        odo.Paciente = dr["NombrePaciente"].ToString();
                        odo.DesTipoCarnet = dr["DescCarnet"].ToString();
                        odo.Manipulador = dr["Manipulador"].ToString();
                        odo.Prioridad = Convert.ToInt32(dr["Prioridad"].ToString());
                        odo.idEstado = Convert.ToInt32(dr["IdEstado"].ToString());
                        ListaCarnetOdo.Add(odo);

                    }


                }
            }

            return ListaCarnetOdo;
        }



        public ActionResult ActualizarDatosAtencionOdontologia(int Id)
        {
            //E_CSOdontologia Odon = new E_CSOdontologia();
            //Odon.IdOdontologia = Id;
            ViewBag.lista = MostrarDatosDeListaDeCarnetSanidadCSOdontologia(Id);
            return View();
        }

        public List<E_CSOdontologia> MostrarDatosDeListaDeCarnetSanidadCSOdontologia(int Id)
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSOdontologia> ListaCarnetOdo = new List<E_CSOdontologia>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_MostrarDatosDeListaDeCarnetSanidadCSOdontologia", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Sede", sede);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_CSOdontologia odo = new E_CSOdontologia();

                        odo.Id = int.Parse(dr["Id"].ToString());
                        odo.CodCue = int.Parse(dr["CodCue"].ToString());
                        odo.Paciente = dr["NombrePaciente"].ToString();
                        odo.DesTipoCarnet = dr["DescCarnet"].ToString();
                        odo.Manipulador = dr["Manipulador"].ToString();
                        odo.Prioridad = Convert.ToInt32(dr["Prioridad"].ToString());
                        odo.idEstado = Convert.ToInt32(dr["IdEstado"].ToString());
                        odo.Edad = Convert.ToInt32(dr["edad"]);
                        odo.NumeroCarnet = dr["NumeroCarnet"].ToString();
                        odo.Procedencia = dr["Procedencia"].ToString();
                        ListaCarnetOdo.Add(odo);

                    }


                }
            }

            return ListaCarnetOdo;
        }


        [HttpPost]
        public ActionResult ActualizarDatosAtencionOdontologia(string IdO,E_CSOdontologia odo)
        {
            int IdOdontologia =Convert.ToInt32(IdO);

            string sede = Session["CodSede"].ToString();

            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();

            string Crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            try
            {
                using (db = new SqlConnection(cadena))
                {
                    using (cmd = new SqlCommand("Usp_ActualizarOdontograma_Y_CSOdontologia", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdOdontologia", IdOdontologia);

                        if (odo.NuevoArregloOdontograma != null)
                        {
                            cmd.Parameters.AddWithValue("@ArregloOdontograma", odo.NuevoArregloOdontograma);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@ArregloOdontograma", "");
                        }

                        if (odo.Observacion != null)
                        {
                            cmd.Parameters.AddWithValue("@Observacion", odo.Observacion);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Observacion", "");
                        }

                        if (odo.Apto != null)
                        {
                            cmd.Parameters.AddWithValue("@Apto", odo.Apto);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Apto", "");
                        }
                        
                        cmd.Parameters.AddWithValue("@Crea", Crea);
                        cmd.Parameters.AddWithValue("@CodSede", sede);
                        db.Open();
                        int ide = cmd.ExecuteNonQuery();
                        RedirectToAction("ListadoCarnetOdontologiaEnEspera");
                    }
                }
            }
            catch (Exception e)

            {
                RedirectToAction("ListadoCarnetOdontologiaEnEspera");
            }

            //Usp_GenerarOdontograma(IdO, hiddenD18LT, hiddenD18LL, hiddenD18LC,hiddenD18LR,hiddenD18LB, hiddenD17LT, hiddenD17LL, hiddenD17LC, hiddenD17LR, hiddenD17LB, hiddenD16LT, hiddenD16LL, hiddenD16LC, hiddenD16LR, hiddenD16LB, hiddenD15LT, hiddenD15LL, hiddenD15LC, hiddenD15LR, hiddenD15LB, hiddenD14LT, hiddenD14LL, hiddenD14LC, hiddenD14LR, hiddenD14LB, hiddenD13LT, hiddenD13LL, hiddenD13LC, hiddenD13LR, hiddenD13LB, hiddenD12LT, hiddenD12LL, hiddenD12LC,  hiddenD12LR,  hiddenD12LB, hiddenD11LT, hiddenD11LL, hiddenD11LC, hiddenD11LR, hiddenD11LB,hiddenD21LT, hiddenD21LL, hiddenD21LC, hiddenD21LR, hiddenD21LB, hiddenD22LT, hiddenD22LL, hiddenD22LC, hiddenD22LR, hiddenD22LB, hiddenD23LT, hiddenD23LL, hiddenD23LC, hiddenD23LR, hiddenD23LB, hiddenD24LT, hiddenD24LL, hiddenD24LC, hiddenD24LR, hiddenD24LB, hiddenD25LT, hiddenD25LL, hiddenD25LC, hiddenD25LR, hiddenD25LB, hiddenD26LT, hiddenD26LL, hiddenD26LC, hiddenD26LR, hiddenD26LB, hiddenD27LT, hiddenD27LL, hiddenD27LC, hiddenD27LR, hiddenD27LB,  hiddenD28LT,  hiddenD28LL,  hiddenD28LC,  hiddenD28LR,  hiddenD28LB, hiddenD48LT, hiddenD48LL, hiddenD48LC, hiddenD48LR, hiddenD48LB, hiddenD47LT, hiddenD47LL, hiddenD47LC, hiddenD47LR, hiddenD47LB, hiddenD46LT, hiddenD46LL, hiddenD46LC, hiddenD46LR, hiddenD46LB, hiddenD45LT, hiddenD45LL, hiddenD45LC, hiddenD45LR, hiddenD45LB, hiddenD44LT, hiddenD44LL, hiddenD44LC, hiddenD44LR, hiddenD44LB, hiddenD43LT, hiddenD43LL, hiddenD43LC, hiddenD43LR, hiddenD43LB, hiddenD42LT, hiddenD42LL, hiddenD42LC, hiddenD42LR, hiddenD42LB, hiddenD41LT, hiddenD41LL, hiddenD41LC, hiddenD41LR, hiddenD41LB, hiddenD31LT, hiddenD31LL, hiddenD31LC, hiddenD31LR, hiddenD31LB, hiddenD32LT, hiddenD32LL, hiddenD32LC, hiddenD32LR, hiddenD32LB, hiddenD33LT, hiddenD33LL, hiddenD33LC, hiddenD33LR, hiddenD33LB, hiddenD34LT, hiddenD34LL, hiddenD34LC, hiddenD34LR, hiddenD34LB, hiddenD35LT, hiddenD35LL,  hiddenD35LC, hiddenD35LR, hiddenD35LB, hiddenD36LT, hiddenD36LL, hiddenD36LC, hiddenD36LR, hiddenD36LB, hiddenD37LT, hiddenD37LL, hiddenD37LC, hiddenD37LR, hiddenD37LB, hiddenD38LT, hiddenD38LL, hiddenD38LC, hiddenD38LR, hiddenD38LB);

            return RedirectToAction("ListadoCarnetOdontologiaEnEspera");
        }
        


        public ActionResult ListaPacientesAtendidosDelDia(int? CodCue,string NumDoc = null, DateTime? FechaAten = null)
        {
            ViewBag.CodCue = CodCue;
            ViewBag.NumDoc = NumDoc;

            ViewBag.FechaAten = FechaAten;
            int Cuenta = 0;
            if(CodCue==null)
            {
                Cuenta = 0;
            }
            else
            {
                Cuenta = Convert.ToInt32(CodCue);
            }
            ViewBag.Lista = ListarPacientesAtendidosEnElDia(Cuenta, NumDoc, FechaAten);

            return View();
        }


        public List<E_CSOdontologia> ListarPacientesAtendidosEnElDia(int Cuenta, string NumDoc, DateTime? FechaAten)
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSOdontologia> ListaPacientesOdo = new List<E_CSOdontologia>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_ListadoPacientesAtendidosEnElDia_CSOdontologia", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (FechaAten == null)
                    {
                        cmd.Parameters.AddWithValue("@FechaAte", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@FechaAte", FechaAten);
                    }

                    if (NumDoc == null || NumDoc == "")
                    {
                        cmd.Parameters.AddWithValue("@NumDoc", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@NumDoc", NumDoc);
                    }

                    if (Cuenta == 0)
                    {
                        cmd.Parameters.AddWithValue("@CodCue", System.Data.SqlTypes.SqlString.Null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CodCue", Cuenta);
                    }
                    cmd.Parameters.AddWithValue("@CodSede", sede);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_CSOdontologia Odo = new E_CSOdontologia();

                        Odo.Id = int.Parse(dr["Id"].ToString());
                        Odo.CodCue = int.Parse(dr["CodCue"].ToString());
                        Odo.Paciente = dr["NombrePaciente"].ToString();
                        Odo.DesTipoCarnet = dr["DescCarnet"].ToString();
                        Odo.Manipulador = dr["Manipulador"].ToString();
                        Odo.idEstado = Convert.ToInt32(dr["IdEstado"].ToString());
                        Odo.Edad = Convert.ToInt32(dr["Edad"]);
                        Odo.Apto = dr["Apto"].ToString();
                        ListaPacientesOdo.Add(Odo);

                    }


                }
            }

            return ListaPacientesOdo;
        }


        public ActionResult ModificarDatosOdontograma_Y_CSOdontologia(int Id)
        {
            var MostraDatos = MostrarDatosDeListaDeCarnetSanidadCSOdontologia_ParaModificar(Id).FirstOrDefault();
            //ViewBag.lista = MostrarDatosDeListaDeCarnetSanidadCSOdontologia_ParaModificar(Id);
            
            return View(MostraDatos);
        }


        public List<E_CSOdontologia> MostrarDatosDeListaDeCarnetSanidadCSOdontologia_ParaModificar(int Id)
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSOdontologia> ListaCarnetOdo = new List<E_CSOdontologia>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_MostrarDatosDeListaDeCarnetSanidadCSOdontologia_ParaModificar", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Sede", sede);
                    db.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        E_CSOdontologia odo = new E_CSOdontologia();

                        odo.Id = int.Parse(dr["Id"].ToString());
                        odo.CodOdontograma = int.Parse(dr["CodOdontograma"].ToString());
                        odo.DetalleArreglo = dr["ArregloOdontograma"].ToString();
                        odo.CodCue = int.Parse(dr["CodCue"].ToString());
                        odo.Paciente = dr["NombrePaciente"].ToString();
                        odo.DesTipoCarnet = dr["DescCarnet"].ToString();
                        odo.Manipulador = dr["Manipulador"].ToString();
                        odo.Prioridad = Convert.ToInt32(dr["Prioridad"]);
                        odo.idEstado = Convert.ToInt32(dr["IdEstado"]);
                        odo.Edad = Convert.ToInt32(dr["edad"]);
                        odo.NumeroCarnet = dr["NumeroCarnet"].ToString();
                        odo.Procedencia = dr["Procedencia"].ToString();
                        odo.Observacion = dr["OdoObservacion"].ToString();
                        odo.Apto = dr["OdoApto"].ToString();
                        odo.FechaAtencion=  dr["FechaAtencion"].ToString();
                        ListaCarnetOdo.Add(odo);

                    }


                }
            }

            return ListaCarnetOdo;
        }

        [HttpPost]
        public ActionResult ModificarDatosOdontograma_Y_CSOdontologia(string IdO, E_CSOdontologia Odo)
        {
            var arreglo = Odo.ArregloOdontograma;

            var NewArreglo = Odo.NuevoArregloOdontograma;

            int IdOdontologia = Convert.ToInt32(IdO);

            string sede = Session["CodSede"].ToString();

            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();

            string Crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            if (arreglo!= NewArreglo)
            {


                try
                {
                    using (db = new SqlConnection(cadena))
                    {
                        using (cmd = new SqlCommand("Usp_ActualizarOdontograma_Y_CSOdontologiaReevaluado", db))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@IdOdontologia", IdOdontologia);

                            if (Odo.NuevoArregloOdontograma != null)
                            {
                                cmd.Parameters.AddWithValue("@ArregloOdontograma", Odo.NuevoArregloOdontograma);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@ArregloOdontograma", "");
                            }

                            if (Odo.Observacion != null)
                            {
                                cmd.Parameters.AddWithValue("@Observacion", Odo.Observacion);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Observacion", "");
                            }

                            if (Odo.Apto != null)
                            {
                                cmd.Parameters.AddWithValue("@Apto", Odo.Apto);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Apto", "");
                            }

                            if (Odo.Reevaluado == "NO")
                            {
                                cmd.Parameters.AddWithValue("@Reeva", "NO");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Reeva", "SI");
                            }

                            cmd.Parameters.AddWithValue("@Crea", Crea);
                            cmd.Parameters.AddWithValue("@CodSede", sede);
                            db.Open();
                            int ide = cmd.ExecuteNonQuery();
                            RedirectToAction("ListaPacientesAtendidosDelDia");
                        }
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            else
            {
                try
                {
                    using (db = new SqlConnection(cadena))
                    {
                        using (cmd = new SqlCommand("Usp_ModificarSoloDatosDeCSOdontologia_CSOdontoNO", db))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@IdOdontologia", IdOdontologia);
                            

                            if (Odo.Observacion != null)
                            {
                                cmd.Parameters.AddWithValue("@Observacion", Odo.Observacion);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Observacion", "");
                            }

                            if (Odo.Apto != null)
                            {
                                cmd.Parameters.AddWithValue("@Apto", Odo.Apto);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Apto", "");
                            }

                            if (Odo.Reevaluado == "NO")
                            {
                                cmd.Parameters.AddWithValue("@Reeva", "NO");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Reeva", "SI");
                            }

                            cmd.Parameters.AddWithValue("@Crea", Crea);
                            cmd.Parameters.AddWithValue("@CodSede", sede);
                            db.Open();
                            int ide = cmd.ExecuteNonQuery();
                            RedirectToAction("ListaPacientesAtendidosDelDia");
                        }
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
                return RedirectToAction("ListaPacientesAtendidosDelDia");
            }

            
            return RedirectToAction("ListaPacientesAtendidosDelDia");
        }


    }
}