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
    public class FichaElectronicaController : Controller
    {

        public List<E_Medico> listaMedicoUsuario(string CodUsu)
        {
            List<E_Medico> Lista = new List<E_Medico>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("listaUsuariosMedicos", con))
                {
                    cmd.Parameters.AddWithValue("@CodUsu", CodUsu);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Medico me = new E_Medico();
                            me.CodMed = dr.GetString(0);
                            me.NomMed = dr.GetString(1);
                            me.DniMed = dr.GetString(2);
                            me.TipPrfMed = dr.GetString(3);
                            me.ColgMed = dr.GetString(4);
                            me.RneMed = dr.GetString(5);
                            me.DireccMed = dr.GetString(6);
                            me.TelfMed = dr.GetString(8);
                            me.CodServ = dr.GetString(10);
                            me.CodEspec = dr.GetString(11);
                            me.CodUsu = dr.GetString(13);
                            me.EnLista = dr.GetBoolean(20);
                            me.EjecFichaElec = dr.GetBoolean(21);

                            Lista.Add(me);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }
        public List<E_Ficha_Electronica> listaUltimaAtencion(int CodHis, string CodEspe)
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("ultimaConsulta", con))
                {
                    cmd.Parameters.AddWithValue("@CodHis", CodHis);
                    cmd.Parameters.AddWithValue("@CodEspe", CodEspe);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Ficha_Electronica me = new E_Ficha_Electronica();
                            me.FE = dr.GetInt32(0);
                            me.Historia = dr.GetInt32(1);
                            me.FC = dr.GetString(2);
                            me.PA = dr.GetString(3);
                            me.FR = dr.GetString(4);
                            me.Tax = dr.GetDecimal(5);
                            me.Tanal = dr.GetDecimal(6);
                            me.Peso = dr.GetDecimal(7);
                            me.talla = dr.GetDecimal(8);
                            me.IMC = dr.GetDecimal(9);
                            me.FUR = dr.GetString(10);
                            me.MotivoConsulta = dr.GetString(11);
                            me.Relato = dr.GetString(12);
                            me.TiempoEnfermedad = dr.GetString(13);
                            me.Inicio = dr.GetString(14);
                            me.Curso = dr.GetString(15);
                            me.sed = dr.GetString(16);
                            me.sueño = dr.GetString(17);
                            me.Orina = dr.GetString(18);
                            me.apetito = dr.GetString(19);
                            me.FecRegMed = dr.GetString(48);

                            Lista.Add(me);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Ficha_Electronica> ListadoPerfilesFichaElectronica()
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
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
                            E_Ficha_Electronica Etip = new E_Ficha_Electronica();

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

        public List<E_Ficha_Electronica> ListadoPerfilesConsentimiento()
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaConsentimiento", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Ficha_Electronica Etip = new E_Ficha_Electronica();

                            Etip.idPFA = dr.GetInt32(0);
                            Etip.Nombre = dr.GetString(1);
                            Etip.contenido = dr.GetString(2);
                            Etip.Estado = dr.GetBoolean(3);

                            Lista.Add(Etip);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Ficha_Electronica> AntecedentexUsuario(int Historia)
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaAntecedentexUsuario", con))
                {
                    cmd.Parameters.AddWithValue("@Historia", Historia);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Ficha_Electronica me = new E_Ficha_Electronica();
                            me.CodAnt = dr.GetInt32(0);
                            me.Historia = dr.GetInt32(1);
                            me.CancerP = dr.GetBoolean(2);
                            me.DiabetesP = dr.GetBoolean(3);
                            me.ACVP = dr.GetBoolean(4);
                            me.AlergiaP = dr.GetBoolean(5);
                            me.HipertArtP = dr.GetBoolean(6);
                            me.OtrosP = dr.GetBoolean(7);
                            me.CancerF = dr.GetBoolean(8);
                            me.DiabetesF = dr.GetBoolean(9);
                            me.ACVF = dr.GetBoolean(10);
                            me.AlergiaF = dr.GetBoolean(11);
                            me.HipertArtF = dr.GetBoolean(12);
                            me.OtrosF = dr.GetBoolean(13);
                            me.ObservacionP = dr.GetString(14);
                            me.ObservacionF = dr.GetString(15);


                            Lista.Add(me);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Ficha_Electronica> listCie10()
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaCie10", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Ficha_Electronica me = new E_Ficha_Electronica();
                            me.CIe10 = dr.GetString(0);
                            me.Descripcion = dr.GetString(1);
                            me.EstadoCie10 = dr.GetBoolean(2);
                            me.ConcatCie10 = dr.GetString(0) + " - " + dr.GetString(1);

                            Lista.Add(me);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_Ficha_Electronica> BandejaCitas(string CodEspec)
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaCuentasDetalleBandejaAtencionCitas", con))
                {
                    cmd.Parameters.AddWithValue("@CodEspec", CodEspec);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Ficha_Electronica Ser = new E_Ficha_Electronica();
                            Ser.CodCue = dr.GetInt32(0);
                            Ser.Item = dr.GetInt32(1);
                            Ser.RegMedico = dr.GetString(2);
                            Ser.EstDet = dr.GetString(3);
                            Ser.CodTar = dr.GetString(4);
                            Ser.CodEspec = dr.GetString(5);
                            Ser.DescTar = dr.GetString(6);
                            Ser.Historia = dr.GetInt32(7);
                            Ser.Paciente = dr.GetString(8);
                            Ser.Item = dr.GetInt32(9);
                            Ser.total = dr.GetDecimal(10);
                            Ser.Cantidad = dr.GetInt32(11);
                            Ser.Procedencia = dr.GetInt32(12);
                            Ser.FechaAtencion = dr.GetDateTime(13);
                            Ser.HoraInicio = dr.GetTimeSpan(14);
                            Ser.HoraFin = dr.GetTimeSpan(15);
                            Ser.CodServ = dr.GetString(16);
                            Ser.NomMed = (dr["NomMed"] is DBNull) ? string.Empty : dr["NomMed"].ToString();
                            Ser.Turno = dr.GetString(17);
                            Ser.CodTipTar = dr.GetString(19);
                            Ser.DescTipTar = dr.GetString(20);
                            Ser.FechaAtenReg = dr.GetString(21);
                            Ser.EstadoCuenta = dr.GetString(22);
                            Ser.FEL = (dr["FE"] is DBNull) ? -1 : (int)dr["FE"];
                            Ser.Modulo = dr.GetInt32(24);
                            Ser.url = dr.GetString(25);
                            Ser.urlImprime = dr.GetString(26);
                            Ser.urlImprimeReceta = dr.GetString(27);

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_Ficha_Electronica> BandejaAtenciones(string CodEspec)
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaCuentasDetalleBandejaAtencionVarias", con))
                {
                    cmd.Parameters.AddWithValue("@CodEspec", CodEspec);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Ficha_Electronica Ser = new E_Ficha_Electronica();
                            Ser.CodCue = dr.GetInt32(0);
                            Ser.Item = dr.GetInt32(1);
                            Ser.RegMedico = dr.GetString(2);
                            Ser.EstDet = dr.GetString(3);
                            Ser.CodTar = dr.GetString(4);
                            Ser.CodEspec = dr.GetString(5);
                            Ser.DescTar = dr.GetString(6);
                            Ser.Historia = dr.GetInt32(7);
                            Ser.Paciente = dr.GetString(8);
                            Ser.Item = dr.GetInt32(9);
                            Ser.total = dr.GetDecimal(10);
                            Ser.Cantidad = dr.GetInt32(11);
                            Ser.Procedencia = dr.GetInt32(12);
                            Ser.FechaAtencion = dr.GetDateTime(13);
                            Ser.CodServ = dr.GetString(14);
                            Ser.Turno = dr.GetString(15);
                            Ser.NomMed = (dr["NomMed"] is DBNull) ? string.Empty : dr["NomMed"].ToString();
                            Ser.CodTipTar = dr.GetString(17);
                            Ser.DescTipTar = dr.GetString(18);
                            Ser.FechaAtenReg = dr.GetString(19);
                            Ser.EstadoCuenta = dr.GetString(20);
                            Ser.FEL = (dr["FE"] is DBNull) ? -1 : (int)dr["FE"];
                            Ser.Modulo = dr.GetInt32(22);
                            Ser.url = dr.GetString(23);
                            Ser.urlImprime = dr.GetString(24);
                            Ser.urlImprimeReceta = dr.GetString(25);

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Ficha_Electronica> ListaCuentaDetalleFichaE(int CodCue, int Item)
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listadoCuentaFichaE", con))
                {
                    cmd.Parameters.AddWithValue("@CodCue", CodCue);
                    cmd.Parameters.AddWithValue("@Item", Item);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Ficha_Electronica Ser = new E_Ficha_Electronica();
                            Ser.CodCue = dr.GetInt32(0);
                            Ser.Item = dr.GetInt32(1);
                            Ser.Historia = dr.GetInt32(2);
                            Ser.Paciente = dr.GetString(3);
                            Ser.FechaNac = dr.GetDateTime(4);
                            Ser.NomServ = dr.GetString(5);
                            Ser.DescTar = dr.GetString(6);
                            Ser.CodTar = dr.GetString(7);
                            Ser.CodEspec = dr.GetString(8);
                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public ActionResult BandejaDeAtenciones(int? CodCue = null, string turno = null)
        {
            UtilitarioController mas = new UtilitarioController();
            E_Master m = mas.ListadoHoraServidor().FirstOrDefault();
            if (turno == null)
            {
                if (m.HoraServidor.Hour < 13)
                {
                    turno = "MAÑANA";
                }
                else
                {
                    turno = "TARDE";
                }
            }
            ViewBag.turno = turno;
            string CodUsu = (string)Session["UserID"];
            E_Medico med = listaMedicoUsuario(CodUsu).FirstOrDefault();

            if (CodCue == null)
            {
                ViewBag.CodCue = "";
            }
            else
            {
                ViewBag.CodCue = CodCue;
            }

            return View();

        }

        public ActionResult BandejaDeAtencionesListado(int? CodCue = null, string turno = null)
        {
            try {
                UtilitarioController mas = new UtilitarioController();
                ServiciosController serv = new ServiciosController();
                E_Master m = mas.ListadoHoraServidor().FirstOrDefault();
                if (turno == null)
                {
                    if (m.HoraServidor.Hour < 13)
                    {
                        turno = "MAÑANA";
                    }
                    else
                    {
                        turno = "TARDE";
                    }
                }
                ViewBag.turno = turno;
                string CodUsu = (string)Session["UserID"];
                E_Medico med = listaMedicoUsuario(CodUsu).FirstOrDefault();
                DatosGeneralesController da = new DatosGeneralesController();
                E_Datos_Generales dat = da.listadatogenerales().FirstOrDefault();
                ViewBag.TipoMed = med.EnLista;
                if (med != null)
                {
                    E_Servicios ser = serv.ListadoServicios().Where(x => x.CodServ == med.CodServ).FirstOrDefault();
                    ViewBag.servicio = ser.NomServ.ToString();

                    if (CodCue == null)
                    {
                        ViewBag.CodCue = "";
                        if (med.EnLista == true)
                        {
                            if (dat.ATENCIONESPAGADAS == true)
                            {
                                ViewBag.cita = BandejaCitas(ser.CodEspec).Where(x => x.Turno == turno && x.CodServ == med.CodServ && x.EstadoCuenta.Equals("F") && x.RegMedico.Trim() == med.CodMed && x.FechaAtencion.ToShortDateString() == m.HoraServidor.ToShortDateString() || (x.FechaAtenReg == m.HoraServidor.ToShortDateString() && x.RegMedico.Trim() == med.CodMed)).ToList();
                                ViewBag.atenciones = BandejaAtenciones(ser.CodEspec).Where(x => x.Turno == turno && x.CodServ == med.CodServ && x.EstadoCuenta.Equals("F") && x.RegMedico.Trim() == med.CodMed && x.FechaAtencion.ToShortDateString() == m.HoraServidor.ToShortDateString() || (x.FechaAtenReg == m.HoraServidor.ToShortDateString() && x.RegMedico.Trim() == med.CodMed)).ToList();
                            }
                            else {
                                ViewBag.cita = BandejaCitas(ser.CodEspec).Where(x => x.Turno == turno && x.CodServ == med.CodServ && x.RegMedico.Trim() == med.CodMed && x.FechaAtencion.ToShortDateString() == m.HoraServidor.ToShortDateString() || (x.FechaAtenReg == m.HoraServidor.ToShortDateString() && x.RegMedico.Trim() == med.CodMed)).ToList();
                                ViewBag.atenciones = BandejaAtenciones(ser.CodEspec).Where(x => x.Turno == turno && x.CodServ == med.CodServ && x.RegMedico.Trim() == med.CodMed && x.FechaAtencion.ToShortDateString() == m.HoraServidor.ToShortDateString() || (x.FechaAtenReg == m.HoraServidor.ToShortDateString() && x.RegMedico.Trim() == med.CodMed)).ToList();
                            }
                        }
                        else
                        {
                            if (dat.ATENCIONESPAGADAS == true)
                            {
                                ViewBag.cita = BandejaCitas(ser.CodEspec).Where(x => x.Turno == turno && x.CodServ == med.CodServ && x.EstadoCuenta.Equals("F") && x.FechaAtencion.ToShortDateString() == m.HoraServidor.ToShortDateString() || x.FechaAtenReg == m.HoraServidor.ToShortDateString()).ToList();
                                ViewBag.atenciones = BandejaAtenciones(ser.CodEspec).Where(x => x.Turno == turno && x.CodServ == med.CodServ && x.EstadoCuenta.Equals("F") && x.FechaAtencion.ToShortDateString() == m.HoraServidor.ToShortDateString() || x.FechaAtenReg == m.HoraServidor.ToShortDateString()).ToList();
                            }
                            else
                            {
                                ViewBag.cita = BandejaCitas(ser.CodEspec).Where(x => x.Turno == turno && x.CodServ == med.CodServ && x.FechaAtencion.ToShortDateString() == m.HoraServidor.ToShortDateString() || x.FechaAtenReg == m.HoraServidor.ToShortDateString()).ToList();
                                ViewBag.atenciones = BandejaAtenciones(ser.CodEspec).Where(x => x.Turno == turno && x.CodServ == med.CodServ && x.FechaAtencion.ToShortDateString() == m.HoraServidor.ToShortDateString() || x.FechaAtenReg == m.HoraServidor.ToShortDateString()).ToList();
                            }
                        }
                    }
                    else
                    {
                        ViewBag.CodCue = CodCue;
                        if (med.EnLista == true)
                        {
                            if (dat.ATENCIONESPAGADAS == true)
                            {
                                ViewBag.cita = BandejaCitas(ser.CodEspec).Where(x => x.EstadoCuenta.Equals("F") && x.CodCue == CodCue).ToList();
                                ViewBag.atenciones = BandejaAtenciones(ser.CodEspec).Where(x => x.EstadoCuenta.Equals("F") && x.CodCue == CodCue).ToList();
                            }
                            else
                            {
                                ViewBag.cita = BandejaCitas(ser.CodEspec).Where(x => x.CodCue == CodCue).ToList();
                                ViewBag.atenciones = BandejaAtenciones(ser.CodEspec).Where(x => x.CodCue == CodCue).ToList();
                            }
                        }
                        else
                        {
                            if (dat.ATENCIONESPAGADAS == true)
                            {
                                ViewBag.cita = BandejaCitas(ser.CodEspec).Where(x => x.CodCue == CodCue && x.EstadoCuenta.Equals("F")).ToList();
                                ViewBag.atenciones = BandejaAtenciones(ser.CodEspec).Where(x => x.CodCue == CodCue && x.EstadoCuenta.Equals("F")).ToList();
                            }
                            else
                            {
                                ViewBag.cita = BandejaCitas(ser.CodEspec).Where(x => x.CodCue == CodCue).ToList();
                                ViewBag.atenciones = BandejaAtenciones(ser.CodEspec).Where(x => x.CodCue == CodCue).ToList();
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                ViewBag.mensaje = "Es posible que el usuario no este asignado a un medico";
            }


            return View();

        }


        public ActionResult RegistroDeAtencionesBasica(int CodCue, int Item, int Historia, int Tipo, int? FE = null, int? idProc = null)
        {
            string CodUsu = (string)Session["UserID"];
            E_Ficha_Electronica f = ListaCuentaDetalleFichaE(CodCue, Item).FirstOrDefault();
            PacientesController pa = new PacientesController();
            E_Pacientes pac = pa.ListadoPacientes().Find(x => x.Historia == Historia);
            int edad = DateTime.Today.AddTicks(-f.FechaNac.Ticks).Year - 1;
            ViewBag.tab = "1";
            ViewBag.cuenta = CodCue;
            ViewBag.item = Item;
            ViewBag.historia = Historia;
            ViewBag.nombrePac = f.Paciente;
            ViewBag.Tipo = Tipo;
            ViewBag.edad = edad;
            ViewBag.servicio = f.NomServ;
            ViewBag.tarifa = f.DescTar;
            ViewBag.CodTar = f.CodTar;
            if (FE != null)
            {
                int fe = (int)FE;
                ViewBag.FE = FE;
                var lista = (from x in ListaFichaElectronicaxFE(fe) select x).FirstOrDefault();
                return View(lista);
            }
            else if (idProc != null)
            {
                int idpro = (int)idProc;
                ViewBag.idProc = idProc;
                var lista = (from x in ListaProcedimiento(idpro) select x).FirstOrDefault();
                return View(lista);
            }
            else
            {
                ViewBag.FE = -1;
                ViewBag.idProc = -1;
            }


            return View();
        }

        [HttpPost]
        public ActionResult RegistroDeAtencionesBasica(E_Ficha_Electronica fi)
        {
            string usuario = Session["UserID"].ToString();
            E_Medico me = listaMedicoUsuario(usuario).FirstOrDefault();
            E_Ficha_Electronica f = ListaCuentaDetalleFichaE(fi.CodCue, fi.Item).FirstOrDefault();
            PacientesController pa = new PacientesController();
            E_Pacientes pac = pa.ListadoPacientes().Find(x => x.Historia == fi.Historia);
            E_Ficha_Electronica fe = ListaCuentaDetalleFichaE(fi.CodCue, fi.Item).FirstOrDefault();
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string Crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;
            int edad = DateTime.Today.AddTicks(-f.FechaNac.Ticks).Year - 1;
            ViewBag.tab = "1";
            ViewBag.cuenta = fi.CodCue;
            ViewBag.item = fi.Item;
            ViewBag.historia = fi.Historia;
            ViewBag.nombrePac = f.Paciente;
            ViewBag.edad = edad;
            ViewBag.servicio = f.NomServ;
            ViewBag.tarifa = f.DescTar;
            ViewBag.CodTar = f.CodTar;
            if (fi.FE != -1)
            {
                ViewBag.FE = fi.FE;
            }
            else
            {
                ViewBag.FE = -1;
            }

            if (fi.tipoFichaElectronica == 1)
            {
                if (fi.FE == -1)
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Ficha_Electronica", con))
                        {
                            try
                            {
                                da.CommandType = CommandType.StoredProcedure;
                                da.Parameters.AddWithValue("@FE", "");
                                da.Parameters.AddWithValue("@Historia", fi.Historia);
                                if (fi.FC == null) { da.Parameters.AddWithValue("@FC", ""); } else { da.Parameters.AddWithValue("@FC", fi.FC); }
                                if (fi.PA == null) { da.Parameters.AddWithValue("@PA", ""); } else { da.Parameters.AddWithValue("@PA", fi.PA); }
                                if (fi.FR == null) { da.Parameters.AddWithValue("@FR", ""); } else { da.Parameters.AddWithValue("@FR", fi.FR); }
                                da.Parameters.AddWithValue("@Tax", fi.Tax);
                                da.Parameters.AddWithValue("@Tanal", fi.Tanal);
                                da.Parameters.AddWithValue("@Peso", fi.Peso);
                                da.Parameters.AddWithValue("@talla", fi.talla);
                                da.Parameters.AddWithValue("@IMC", fi.IMC);
                                if (fi.FUR == null) { da.Parameters.AddWithValue("@FUR", ""); } else { da.Parameters.AddWithValue("@FUR", fi.FUR); }
                                if (fi.MotivoConsulta == null) { da.Parameters.AddWithValue("@MotivoConsulta", ""); } else { da.Parameters.AddWithValue("@MotivoConsulta", fi.MotivoConsulta); }
                                if (fi.Relato == null) { da.Parameters.AddWithValue("@Relato", ""); } else { da.Parameters.AddWithValue("@Relato", fi.Relato); }
                                if (fi.TiempoEnfermedad == null) { da.Parameters.AddWithValue("@TiempoEnfermedad", ""); } else { da.Parameters.AddWithValue("@TiempoEnfermedad", fi.TiempoEnfermedad); }
                                if (fi.Inicio == null) { da.Parameters.AddWithValue("@Inicio", ""); } else { da.Parameters.AddWithValue("@Inicio", fi.Inicio); }
                                if (fi.Curso == null) { da.Parameters.AddWithValue("@Curso", ""); } else { da.Parameters.AddWithValue("@Curso", fi.Curso); }
                                if (fi.sed == null) { da.Parameters.AddWithValue("@sed", ""); } else { da.Parameters.AddWithValue("@sed", fi.sed); }
                                if (fi.sueño == null) { da.Parameters.AddWithValue("@sueno", ""); } else { da.Parameters.AddWithValue("@sueno", fi.sueño); }
                                if (fi.Orina == null) { da.Parameters.AddWithValue("@orina", ""); } else { da.Parameters.AddWithValue("@orina", fi.Orina); }
                                if (fi.apetito == null) { da.Parameters.AddWithValue("@apetito", ""); } else { da.Parameters.AddWithValue("@apetito", fi.apetito); }
                                da.Parameters.AddWithValue("@paridad", fi.paridad);
                                if (fi.FPP == null) { da.Parameters.AddWithValue("@FPP", ""); } else { da.Parameters.AddWithValue("@FPP", fi.FPP); }
                                da.Parameters.AddWithValue("@EdadGestacional", fi.EdadGestacional);
                                if (fi.PIG == null) { da.Parameters.AddWithValue("@PIG", ""); } else { da.Parameters.AddWithValue("@PIG", fi.PIG); }
                                if (fi.UltimoPap == null) { da.Parameters.AddWithValue("@UltimoPap", ""); } else { da.Parameters.AddWithValue("@UltimoPap", fi.UltimoPap); }
                                if (fi.ResultPap == null) { da.Parameters.AddWithValue("@ResultPap", ""); } else { da.Parameters.AddWithValue("@ResultPap", fi.ResultPap); }
                                if (fi.MACtipTime == null) { da.Parameters.AddWithValue("@MACtipTime", ""); } else { da.Parameters.AddWithValue("@MACtipTime", fi.MACtipTime); }
                                if (fi.Otrosginec == null) { da.Parameters.AddWithValue("@Otrosginec", ""); } else { da.Parameters.AddWithValue("@Otrosginec", fi.Otrosginec); }
                                if (fi.ProxCita == null) { da.Parameters.AddWithValue("@ProxCita", ""); } else { da.Parameters.AddWithValue("@ProxCita", fi.ProxCita); }
                                if (fi.ExaGeneral == null) { da.Parameters.AddWithValue("@ExaGeneral", ""); } else { da.Parameters.AddWithValue("@ExaGeneral", fi.ExaGeneral); }
                                if (fi.UbicEspTampa == null) { da.Parameters.AddWithValue("@UbicEspTampa", ""); } else { da.Parameters.AddWithValue("@UbicEspTampa", fi.UbicEspTampa); }
                                if (fi.EstNutricion == null) { da.Parameters.AddWithValue("@EstNutricion", ""); } else { da.Parameters.AddWithValue("@EstNutricion", fi.EstNutricion); }
                                if (fi.EstHidratacion == null) { da.Parameters.AddWithValue("@EstHidratacion", ""); } else { da.Parameters.AddWithValue("@EstHidratacion", fi.EstHidratacion); }
                                if (fi.PielFanerasTejido == null) { da.Parameters.AddWithValue("@PielFanerasTejido", ""); } else { da.Parameters.AddWithValue("@PielFanerasTejido", fi.PielFanerasTejido); }
                                if (fi.Mamas == null) { da.Parameters.AddWithValue("@Mamas", ""); } else { da.Parameters.AddWithValue("@Mamas", fi.Mamas); }
                                if (fi.SisRespiratorio == null) { da.Parameters.AddWithValue("@SisRespiratorio", ""); } else { da.Parameters.AddWithValue("@SisRespiratorio", fi.SisRespiratorio); }
                                if (fi.CabezaCuello == null) { da.Parameters.AddWithValue("@CabezaCuello", ""); } else { da.Parameters.AddWithValue("@CabezaCuello", fi.CabezaCuello); }
                                if (fi.SisOsteoMuscular == null) { da.Parameters.AddWithValue("@SisOsteoMuscular", ""); } else { da.Parameters.AddWithValue("@SisOsteoMuscular", fi.SisOsteoMuscular); }
                                if (fi.SisCardiovascular == null) { da.Parameters.AddWithValue("@SisCardiovascular", ""); } else { da.Parameters.AddWithValue("@SisCardiovascular", fi.SisCardiovascular); }
                                if (fi.AbdomenPelvis == null) { da.Parameters.AddWithValue("@AbdomenPelvis", ""); } else { da.Parameters.AddWithValue("@AbdomenPelvis", fi.AbdomenPelvis); }
                                if (fi.ExaObstetrico == null) { da.Parameters.AddWithValue("@ExaObstetrico", ""); } else { da.Parameters.AddWithValue("@ExaObstetrico", fi.ExaObstetrico); }
                                if (fi.SisGenitourinario == null) { da.Parameters.AddWithValue("@SisGenitourinario", ""); } else { da.Parameters.AddWithValue("@SisGenitourinario", fi.SisGenitourinario); }
                                if (fi.SisNervioso == null) { da.Parameters.AddWithValue("@SisNervioso", ""); } else { da.Parameters.AddWithValue("@SisNervioso", fi.SisNervioso); }
                                da.Parameters.AddWithValue("@Estado", "");
                                da.Parameters.AddWithValue("@Asistente", me.CodMed);
                                da.Parameters.AddWithValue("@FecRegAsist", horaSis.HoraServidor.ToShortDateString());
                                da.Parameters.AddWithValue("@Medico", "");
                                da.Parameters.AddWithValue("@Servicio", me.CodServ);
                                da.Parameters.AddWithValue("@FecRegMed", "");
                                if (fi.InterConsulta == null) { da.Parameters.AddWithValue("@InterConsulta", ""); } else { da.Parameters.AddWithValue("@InterConsulta", fi.InterConsulta); }
                                da.Parameters.AddWithValue("@Tarifa", fe.CodTar);
                                da.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                da.Parameters.AddWithValue("@Item", fi.Item);
                                da.Parameters.AddWithValue("@Crea", Crea);
                                da.Parameters.AddWithValue("@Modifica", "");
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", 1);
                                da.ExecuteNonQuery(); ViewBag.mensaje = "Pedido registrado";

                            }
                            catch (Exception ex)
                            {
                                return RedirectPermanent("../Master");
                            }
                            finally
                            {
                                con.Close();
                            }
                        }
                    }
                }

                else
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Ficha_Electronica", con))
                        {
                            try
                            {
                                da.CommandType = CommandType.StoredProcedure;
                                da.Parameters.AddWithValue("@FE", fi.FE);
                                da.Parameters.AddWithValue("@Historia", fi.Historia);
                                if (fi.FC == null) { da.Parameters.AddWithValue("@FC", ""); } else { da.Parameters.AddWithValue("@FC", fi.FC); }
                                if (fi.PA == null) { da.Parameters.AddWithValue("@PA", ""); } else { da.Parameters.AddWithValue("@PA", fi.PA); }
                                if (fi.FR == null) { da.Parameters.AddWithValue("@FR", ""); } else { da.Parameters.AddWithValue("@FR", fi.FR); }
                                da.Parameters.AddWithValue("@Tax", fi.Tax);
                                da.Parameters.AddWithValue("@Tanal", fi.Tanal);
                                da.Parameters.AddWithValue("@Peso", fi.Peso);
                                da.Parameters.AddWithValue("@talla", fi.talla);
                                da.Parameters.AddWithValue("@IMC", fi.IMC);
                                if (fi.FUR == null) { da.Parameters.AddWithValue("@FUR", ""); } else { da.Parameters.AddWithValue("@FUR", fi.FUR); }
                                if (fi.MotivoConsulta == null) { da.Parameters.AddWithValue("@MotivoConsulta", ""); } else { da.Parameters.AddWithValue("@MotivoConsulta", fi.MotivoConsulta); }
                                if (fi.Relato == null) { da.Parameters.AddWithValue("@Relato", ""); } else { da.Parameters.AddWithValue("@Relato", fi.Relato); }
                                if (fi.TiempoEnfermedad == null) { da.Parameters.AddWithValue("@TiempoEnfermedad", ""); } else { da.Parameters.AddWithValue("@TiempoEnfermedad", fi.TiempoEnfermedad); }
                                if (fi.Inicio == null) { da.Parameters.AddWithValue("@Inicio", ""); } else { da.Parameters.AddWithValue("@Inicio", fi.Inicio); }
                                if (fi.Curso == null) { da.Parameters.AddWithValue("@Curso", ""); } else { da.Parameters.AddWithValue("@Curso", fi.Curso); }
                                if (fi.sed == null) { da.Parameters.AddWithValue("@sed", ""); } else { da.Parameters.AddWithValue("@sed", fi.sed); }
                                if (fi.sueño == null) { da.Parameters.AddWithValue("@sueno", ""); } else { da.Parameters.AddWithValue("@sueno", fi.sueño); }
                                if (fi.Orina == null) { da.Parameters.AddWithValue("@orina", ""); } else { da.Parameters.AddWithValue("@orina", fi.Orina); }
                                if (fi.apetito == null) { da.Parameters.AddWithValue("@apetito", ""); } else { da.Parameters.AddWithValue("@apetito", fi.apetito); }
                                da.Parameters.AddWithValue("@paridad", fi.paridad);
                                if (fi.FPP == null) { da.Parameters.AddWithValue("@FPP", ""); } else { da.Parameters.AddWithValue("@FPP", fi.FPP); }
                                da.Parameters.AddWithValue("@EdadGestacional", fi.EdadGestacional);
                                if (fi.PIG == null) { da.Parameters.AddWithValue("@PIG", ""); } else { da.Parameters.AddWithValue("@PIG", fi.PIG); }
                                if (fi.UltimoPap == null) { da.Parameters.AddWithValue("@UltimoPap", ""); } else { da.Parameters.AddWithValue("@UltimoPap", fi.UltimoPap); }
                                if (fi.ResultPap == null) { da.Parameters.AddWithValue("@ResultPap", ""); } else { da.Parameters.AddWithValue("@ResultPap", fi.ResultPap); }
                                if (fi.MACtipTime == null) { da.Parameters.AddWithValue("@MACtipTime", ""); } else { da.Parameters.AddWithValue("@MACtipTime", fi.MACtipTime); }
                                if (fi.Otrosginec == null) { da.Parameters.AddWithValue("@Otrosginec", ""); } else { da.Parameters.AddWithValue("@Otrosginec", fi.Otrosginec); }
                                if (fi.ProxCita == null) { da.Parameters.AddWithValue("@ProxCita", ""); } else { da.Parameters.AddWithValue("@ProxCita", fi.ProxCita); }
                                if (fi.ExaGeneral == null) { da.Parameters.AddWithValue("@ExaGeneral", ""); } else { da.Parameters.AddWithValue("@ExaGeneral", fi.ExaGeneral); }
                                if (fi.UbicEspTampa == null) { da.Parameters.AddWithValue("@UbicEspTampa", ""); } else { da.Parameters.AddWithValue("@UbicEspTampa", fi.UbicEspTampa); }
                                if (fi.EstNutricion == null) { da.Parameters.AddWithValue("@EstNutricion", ""); } else { da.Parameters.AddWithValue("@EstNutricion", fi.EstNutricion); }
                                if (fi.EstHidratacion == null) { da.Parameters.AddWithValue("@EstHidratacion", ""); } else { da.Parameters.AddWithValue("@EstHidratacion", fi.EstHidratacion); }
                                if (fi.PielFanerasTejido == null) { da.Parameters.AddWithValue("@PielFanerasTejido", ""); } else { da.Parameters.AddWithValue("@PielFanerasTejido", fi.PielFanerasTejido); }
                                if (fi.Mamas == null) { da.Parameters.AddWithValue("@Mamas", ""); } else { da.Parameters.AddWithValue("@Mamas", fi.Mamas); }
                                if (fi.SisRespiratorio == null) { da.Parameters.AddWithValue("@SisRespiratorio", ""); } else { da.Parameters.AddWithValue("@SisRespiratorio", fi.SisRespiratorio); }
                                if (fi.CabezaCuello == null) { da.Parameters.AddWithValue("@CabezaCuello", ""); } else { da.Parameters.AddWithValue("@CabezaCuello", fi.CabezaCuello); }
                                if (fi.SisOsteoMuscular == null) { da.Parameters.AddWithValue("@SisOsteoMuscular", ""); } else { da.Parameters.AddWithValue("@SisOsteoMuscular", fi.SisOsteoMuscular); }
                                if (fi.SisCardiovascular == null) { da.Parameters.AddWithValue("@SisCardiovascular", ""); } else { da.Parameters.AddWithValue("@SisCardiovascular", fi.SisCardiovascular); }
                                if (fi.AbdomenPelvis == null) { da.Parameters.AddWithValue("@AbdomenPelvis", ""); } else { da.Parameters.AddWithValue("@AbdomenPelvis", fi.AbdomenPelvis); }
                                if (fi.ExaObstetrico == null) { da.Parameters.AddWithValue("@ExaObstetrico", ""); } else { da.Parameters.AddWithValue("@ExaObstetrico", fi.ExaObstetrico); }
                                if (fi.SisGenitourinario == null) { da.Parameters.AddWithValue("@SisGenitourinario", ""); } else { da.Parameters.AddWithValue("@SisGenitourinario", fi.SisGenitourinario); }
                                if (fi.SisNervioso == null) { da.Parameters.AddWithValue("@SisNervioso", ""); } else { da.Parameters.AddWithValue("@SisNervioso", fi.SisNervioso); }
                                da.Parameters.AddWithValue("@Estado", "");
                                da.Parameters.AddWithValue("@Asistente", me.CodMed);
                                da.Parameters.AddWithValue("@FecRegAsist", horaSis.HoraServidor.ToShortDateString());
                                da.Parameters.AddWithValue("@Medico", "");
                                da.Parameters.AddWithValue("@Servicio", me.CodServ);
                                da.Parameters.AddWithValue("@FecRegMed", "");
                                if (fi.InterConsulta == null) { da.Parameters.AddWithValue("@InterConsulta", ""); } else { da.Parameters.AddWithValue("@InterConsulta", fi.InterConsulta); }
                                da.Parameters.AddWithValue("@Tarifa", fe.CodTar);
                                da.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                da.Parameters.AddWithValue("@Item", fi.Item);
                                da.Parameters.AddWithValue("@Crea", Crea);
                                da.Parameters.AddWithValue("@Modifica", "");
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", 2);
                                da.ExecuteNonQuery(); ViewBag.mensaje = "Pedido registrado";

                            }
                            catch (Exception ex)
                            {
                                return RedirectPermanent("../Master");
                            }
                            finally
                            {
                                con.Close();
                            }
                        }

                    }

                }

            }



            else if(fi.tipoFichaElectronica == 2)
            {

                if (fi.idProc == -1)
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Procedimientos", con))
                        {
                            try
                            {
                                da.CommandType = CommandType.StoredProcedure;
                                da.Parameters.AddWithValue("@idProc", "");
                                if (fi.FC == null) { da.Parameters.AddWithValue("@FC", ""); } else { da.Parameters.AddWithValue("@FC", fi.FC); }
                                if (fi.PA == null) { da.Parameters.AddWithValue("@PA", ""); } else { da.Parameters.AddWithValue("@PA", fi.PA); }
                                if (fi.FR == null) { da.Parameters.AddWithValue("@FR", ""); } else { da.Parameters.AddWithValue("@FR", fi.FR); }
                                da.Parameters.AddWithValue("@Tax", fi.Tax);
                                da.Parameters.AddWithValue("@Tanal", fi.Tanal);
                                if (fi.DescripcionProc == null) { da.Parameters.AddWithValue("@Descripcion", ""); } else { da.Parameters.AddWithValue("@Descripcion", fi.DescripcionProc); }
                                if (fi.CodTar == null) { da.Parameters.AddWithValue("@CodTar", ""); } else { da.Parameters.AddWithValue("@CodTar", fi.CodTar); }
                                da.Parameters.AddWithValue("@item", fi.Item);
                                da.Parameters.AddWithValue("@Historia", fi.Historia);
                                da.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                if (fi.Observ == null) { da.Parameters.AddWithValue("@Observ", ""); } else { da.Parameters.AddWithValue("@Observ", fi.Observ); }
                                if (fi.ProximaProc == null) { da.Parameters.AddWithValue("@ProximaProc", ""); } else { da.Parameters.AddWithValue("@ProximaProc", fi.ProximaProc); }
                                da.Parameters.AddWithValue("@Asistente", me.CodMed);
                                da.Parameters.AddWithValue("@FecRegAsist", horaSis.HoraServidor.ToShortDateString());
                                da.Parameters.AddWithValue("@CodMed", "");
                                da.Parameters.AddWithValue("@FecRegMed", "");
                                da.Parameters.AddWithValue("@CodServ", me.CodServ);
                                if (fi.InterConsulta == null) { da.Parameters.AddWithValue("@InterConsulta", ""); } else { da.Parameters.AddWithValue("@InterConsulta", fi.InterConsulta); }
                                da.Parameters.AddWithValue("@Estado", "");
                                da.Parameters.AddWithValue("@Crea", Crea);
                                da.Parameters.AddWithValue("@Modifica", "");
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", 1);
                                da.ExecuteNonQuery(); ViewBag.mensaje = "Pedido registrado";

                            }
                            catch (Exception ex)
                            {
                                return RedirectPermanent("../Master");
                            }
                            finally
                            {
                                con.Close();
                            }
                        }
                    }

                }
                else
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Procedimientos", con))
                        {
                            try
                            {
                                da.CommandType = CommandType.StoredProcedure;
                                da.Parameters.AddWithValue("@idProc", fi.idProc);
                                if (fi.FC == null) { da.Parameters.AddWithValue("@FC", ""); } else { da.Parameters.AddWithValue("@FC", fi.FC); }
                                if (fi.PA == null) { da.Parameters.AddWithValue("@PA", ""); } else { da.Parameters.AddWithValue("@PA", fi.PA); }
                                if (fi.FR == null) { da.Parameters.AddWithValue("@FR", ""); } else { da.Parameters.AddWithValue("@FR", fi.FR); }
                                da.Parameters.AddWithValue("@Tax", fi.Tax);
                                da.Parameters.AddWithValue("@Tanal", fi.Tanal);
                                if (fi.DescripcionProc == null) { da.Parameters.AddWithValue("@Descripcion", ""); } else { da.Parameters.AddWithValue("@Descripcion", fi.DescripcionProc); }
                                if (fi.CodTar == null) { da.Parameters.AddWithValue("@CodTar", ""); } else { da.Parameters.AddWithValue("@CodTar", fi.CodTar); }
                                da.Parameters.AddWithValue("@item", fi.Item);
                                da.Parameters.AddWithValue("@Historia", fi.Historia);
                                da.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                if (fi.Observ == null) { da.Parameters.AddWithValue("@Observ", ""); } else { da.Parameters.AddWithValue("@Observ", fi.Observ); }
                                if (fi.ProximaProc == null) { da.Parameters.AddWithValue("@ProximaProc", ""); } else { da.Parameters.AddWithValue("@ProximaProc", fi.ProximaProc); }
                                da.Parameters.AddWithValue("@Asistente", me.CodMed);
                                da.Parameters.AddWithValue("@FecRegAsist", horaSis.HoraServidor.ToShortDateString());
                                da.Parameters.AddWithValue("@CodMed", "");
                                da.Parameters.AddWithValue("@FecRegMed", "");
                                da.Parameters.AddWithValue("@CodServ", me.CodServ);
                                if (fi.InterConsulta == null) { da.Parameters.AddWithValue("@InterConsulta", ""); } else { da.Parameters.AddWithValue("@InterConsulta", fi.InterConsulta); }
                                da.Parameters.AddWithValue("@Estado", "");
                                da.Parameters.AddWithValue("@Crea", Crea);
                                da.Parameters.AddWithValue("@Modifica", "");
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", 2);
                                da.ExecuteNonQuery(); ViewBag.mensaje = "Pedido registrado";

                            }
                            catch (Exception ex)
                            {
                                return RedirectPermanent("../Master");
                            }
                            finally
                            {
                                con.Close();
                            }
                        }
                    }

                }


            }


            else if (fi.tipoFichaElectronica == 3)
            {

                if (fi.idProc == -1)
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_ExamenAuxiliar", con))
                        {
                            try
                            {
                                da.CommandType = CommandType.StoredProcedure;
                                da.Parameters.AddWithValue("@idExAux", "");
                                if (fi.FC == null) { da.Parameters.AddWithValue("@FC", ""); } else { da.Parameters.AddWithValue("@FC", fi.FC); }
                                if (fi.PA == null) { da.Parameters.AddWithValue("@PA", ""); } else { da.Parameters.AddWithValue("@PA", fi.PA); }
                                if (fi.FR == null) { da.Parameters.AddWithValue("@FR", ""); } else { da.Parameters.AddWithValue("@FR", fi.FR); }
                                da.Parameters.AddWithValue("@Tax", fi.Tax);
                                da.Parameters.AddWithValue("@Tanal", fi.Tanal);
                                if (fi.DescripcionProc == null) { da.Parameters.AddWithValue("@Descripcion", ""); } else { da.Parameters.AddWithValue("@Descripcion", fi.DescripcionProc); }
                                if (fi.CodTar == null) { da.Parameters.AddWithValue("@CodTar", ""); } else { da.Parameters.AddWithValue("@CodTar", fi.CodTar); }
                                da.Parameters.AddWithValue("@item", fi.Item);
                                da.Parameters.AddWithValue("@Historia", fi.Historia);
                                da.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                if (fi.Observ == null) { da.Parameters.AddWithValue("@Observ", ""); } else { da.Parameters.AddWithValue("@Observ", fi.Observ); }
                                if (fi.ProximaProc == null) { da.Parameters.AddWithValue("@ProximaProc", ""); } else { da.Parameters.AddWithValue("@ProximaProc", fi.ProximaProc); }
                                da.Parameters.AddWithValue("@Asistente", me.CodMed);
                                da.Parameters.AddWithValue("@FecRegAsist", horaSis.HoraServidor.ToShortDateString());
                                da.Parameters.AddWithValue("@CodMed", "");
                                da.Parameters.AddWithValue("@FecRegMed", "");
                                da.Parameters.AddWithValue("@CodServ", me.CodServ);
                                if (fi.InterConsulta == null) { da.Parameters.AddWithValue("@InterConsulta", ""); } else { da.Parameters.AddWithValue("@InterConsulta", fi.InterConsulta); }
                                da.Parameters.AddWithValue("@Estado", "");
                                da.Parameters.AddWithValue("@Crea", Crea);
                                da.Parameters.AddWithValue("@Modifica", "");
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", 1);
                                da.ExecuteNonQuery(); ViewBag.mensaje = "Pedido registrado";

                            }
                            catch (Exception ex)
                            {
                                return RedirectPermanent("../Master");
                            }
                            finally
                            {
                                con.Close();
                            }
                        }
                    }

                }
                else
                {

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_ExamenAuxiliar", con))
                        {
                            try
                            {
                                da.CommandType = CommandType.StoredProcedure;
                                da.Parameters.AddWithValue("@idExAux", fi.idProc);
                                if (fi.FC == null) { da.Parameters.AddWithValue("@FC", ""); } else { da.Parameters.AddWithValue("@FC", fi.FC); }
                                if (fi.PA == null) { da.Parameters.AddWithValue("@PA", ""); } else { da.Parameters.AddWithValue("@PA", fi.PA); }
                                if (fi.FR == null) { da.Parameters.AddWithValue("@FR", ""); } else { da.Parameters.AddWithValue("@FR", fi.FR); }
                                da.Parameters.AddWithValue("@Tax", fi.Tax);
                                da.Parameters.AddWithValue("@Tanal", fi.Tanal);
                                if (fi.DescripcionProc == null) { da.Parameters.AddWithValue("@Descripcion", ""); } else { da.Parameters.AddWithValue("@Descripcion", fi.DescripcionProc); }
                                if (fi.CodTar == null) { da.Parameters.AddWithValue("@CodTar", ""); } else { da.Parameters.AddWithValue("@CodTar", fi.CodTar); }
                                da.Parameters.AddWithValue("@item", fi.Item);
                                da.Parameters.AddWithValue("@Historia", fi.Historia);
                                da.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                if (fi.Observ == null) { da.Parameters.AddWithValue("@Observ", ""); } else { da.Parameters.AddWithValue("@Observ", fi.Observ); }
                                if (fi.ProximaProc == null) { da.Parameters.AddWithValue("@ProximaProc", ""); } else { da.Parameters.AddWithValue("@ProximaProc", fi.ProximaProc); }
                                da.Parameters.AddWithValue("@Asistente", me.CodMed);
                                da.Parameters.AddWithValue("@FecRegAsist", horaSis.HoraServidor.ToShortDateString());
                                da.Parameters.AddWithValue("@CodMed", "");
                                da.Parameters.AddWithValue("@FecRegMed", "");
                                da.Parameters.AddWithValue("@CodServ", me.CodServ);
                                if (fi.InterConsulta == null) { da.Parameters.AddWithValue("@InterConsulta", ""); } else { da.Parameters.AddWithValue("@InterConsulta", fi.InterConsulta); }
                                da.Parameters.AddWithValue("@Estado", "");
                                da.Parameters.AddWithValue("@Crea", Crea);
                                da.Parameters.AddWithValue("@Modifica", "");
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", 2);
                                da.ExecuteNonQuery(); ViewBag.mensaje = "Pedido registrado";

                            }
                            catch (Exception ex)
                            {
                                return RedirectPermanent("../Master");
                            }
                            finally
                            {
                                con.Close();
                            }
                        }
                    }

                }


            }


            return RedirectToAction("BandejaDeAtenciones");
        }

        public ActionResult RegistroDeAtencionesMedica(int CodCue, int Item, int Historia, int? FE = null)
        {
            string usuario = Session["UserID"].ToString();
            MedicosController m = new MedicosController();
            E_Medico med = m.ListadoMedico().Find(x => x.CodUsu == usuario);
            ViewBag.ListaUltimaConsulta = (List<E_Ficha_Electronica>)listaUltimaAtencion(Historia, med.CodEspec).ToList();

            E_Ficha_Electronica ant = AntecedentexUsuario(Historia).FirstOrDefault();
            ViewBag.CodAnt = ant.CodAnt;
            ViewBag.CancerP = ant.CancerP;
            ViewBag.DiabetesP = ant.DiabetesP;
            ViewBag.ACVP = ant.ACVP;
            ViewBag.AlergiaP = ant.AlergiaP;
            ViewBag.HipertArtP = ant.HipertArtP;
            ViewBag.OtrosP = ant.OtrosP;
            ViewBag.ObservacionP = ant.ObservacionP;

            ViewBag.CancerF = ant.CancerF;
            ViewBag.DiabetesF = ant.DiabetesF;
            ViewBag.ACVF = ant.ACVF;
            ViewBag.AlergiaF = ant.AlergiaF;
            ViewBag.HipertArtF = ant.HipertArtF;
            ViewBag.OtrosF = ant.OtrosF;
            ViewBag.ObservacionF = ant.ObservacionF;

            if (FE != null)
            {
                string sede = Session["codSede"].ToString();
                ViewBag.antecedentes = (List<E_Ficha_Electronica>)AntecedentexUsuario(Historia);
                EspecialidadController e = new EspecialidadController();
                ViewBag.antecedentes = (List<E_Ficha_Electronica>)AntecedentexUsuario(Historia);
                ViewBag.listaEspe = (List<E_Especialidades>)e.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true).ToList();
                ViewBag.listaEspecialidad = new SelectList(e.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true), "CodEspec", "NomEspec");
                ViewBag.listaCie10 = new SelectList(listCie10(), "CIe10", "ConcatCie10");
                E_Ficha_Electronica f = ListaCuentaDetalleFichaE(CodCue, Item).FirstOrDefault();
                PacientesController pa = new PacientesController();
                E_Pacientes pac = pa.ListadoPacientes().Find(x => x.Historia == Historia);
                int edad = DateTime.Today.AddTicks(-f.FechaNac.Ticks).Year - 1;
                RecetasController rec = new RecetasController();
                ViewBag.listaFormaFarm = new SelectList(rec.ListadoFormaFarmaceutica().Where(x => x.Estado == true), "idFormFarm", "Descripcion");
                ViewBag.listaFrecuencia = new SelectList(rec.ListadoFrecuenciaRecetas().Where(x => x.Estado == true), "idFrec", "Descripcion");
                ViewBag.tab = "1";
                ViewBag.sexo = pac.CodSexo;
                ViewBag.cuenta = CodCue;
                ViewBag.item = Item;
                ViewBag.historia = Historia;
                ViewBag.nombrePac = f.Paciente;
                ViewBag.edad = edad;
                ViewBag.servicio = f.NomServ;
                ViewBag.FE = FE;
                ViewBag.tarifa = f.DescTar;


                //Sesion de diagnostico
                if (Session["diagnostico"] == null)
                {
                    Session["diagnostico"] = new List<E_Ficha_Electronica>();
                }
                else
                {
                    Session.Remove("diagnostico");
                    Session["diagnostico"] = new List<E_Ficha_Electronica>();
                }

                //Sesion de examen auxiliar
                if (Session["examenAuxiliar"] == null)
                {
                    Session["examenAuxiliar"] = new List<E_Ficha_Electronica>();
                }
                else
                {
                    Session.Remove("examenAuxiliar");
                    Session["examenAuxiliar"] = new List<E_Ficha_Electronica>();
                }

                //Sesion de recetas
                if (Session["recetas"] == null)
                {
                    Session["recetas"] = new List<E_Ficha_Electronica>();
                }
                else
                {
                    Session.Remove("recetas");
                    Session["recetas"] = new List<E_Ficha_Electronica>();
                }


                int Fe = (int)FE;
                var lista = (from x in ListaFichaElectronicaxFE(Fe) select x).FirstOrDefault();
                return View(lista);

            }

            else
            {

                string sede = Session["codSede"].ToString();
                PacientesController pa = new PacientesController();
                E_Pacientes pac = pa.ListadoPacientes().Find(x => x.Historia == Historia);
                EspecialidadController e = new EspecialidadController();
                ViewBag.antecedentes = (List<E_Ficha_Electronica>)AntecedentexUsuario(Historia);
                ViewBag.listaEspe = (List<E_Especialidades>)e.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true).ToList();
                ViewBag.listaEspecialidad = new SelectList(e.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true), "CodEspec", "NomEspec");
                ViewBag.listaCie10 = new SelectList(listCie10(), "CIe10", "ConcatCie10");
                E_Ficha_Electronica f = ListaCuentaDetalleFichaE(CodCue, Item).FirstOrDefault();
                int edad = DateTime.Today.AddTicks(-f.FechaNac.Ticks).Year - 1;
                RecetasController rec = new RecetasController();
                ViewBag.listaFormaFarm = new SelectList(rec.ListadoFormaFarmaceutica().Where(x => x.Estado == true), "idFormFarm", "Descripcion");
                ViewBag.listaFrecuencia = new SelectList(rec.ListadoFrecuenciaRecetas().Where(x => x.Estado == true), "idFrec", "Descripcion");
                ViewBag.tab = "1";
                ViewBag.sexo = pac.CodSexo;
                ViewBag.FE = -1;
                ViewBag.cuenta = CodCue;
                ViewBag.item = Item;
                ViewBag.historia = Historia;
                ViewBag.nombrePac = f.Paciente;
                ViewBag.edad = edad;
                ViewBag.servicio = f.NomServ;
                ViewBag.tarifa = f.DescTar;

                //Sesiones

                //Sesion de diagnostico
                if (Session["diagnostico"] == null)
                {
                    Session["diagnostico"] = new List<E_Ficha_Electronica>();
                }
                else
                {
                    Session.Remove("diagnostico");
                    Session["diagnostico"] = new List<E_Ficha_Electronica>();
                }

                //Sesion de examen auxiliar
                if (Session["examenAuxiliar"] == null)
                {
                    Session["examenAuxiliar"] = new List<E_Ficha_Electronica>();
                }
                else
                {
                    Session.Remove("examenAuxiliar");
                    Session["examenAuxiliar"] = new List<E_Ficha_Electronica>();
                }

                //Sesion de recetas
                if (Session["recetas"] == null)
                {
                    Session["recetas"] = new List<E_Ficha_Electronica>();
                }
                else
                {
                    Session.Remove("recetas");
                    Session["recetas"] = new List<E_Ficha_Electronica>();
                }


                return View();
            }
        }


        [HttpPost]
        public ActionResult RegistroDeAtencionesMedica(E_Ficha_Electronica fi)
        {
            string sede = Session["codSede"].ToString();
            string usuario = Session["UserID"].ToString();
            MedicosController m = new MedicosController();
            E_Medico med = m.ListadoMedico().Find(x => x.CodUsu == usuario);
            ViewBag.ListaUltimaConsulta = (List<E_Ficha_Electronica>)listaUltimaAtencion(fi.Historia, med.CodEspec).ToList();
            E_Medico me = listaMedicoUsuario(usuario).FirstOrDefault();
            ViewBag.antecedentes = (List<E_Ficha_Electronica>)AntecedentexUsuario(fi.Historia);
            E_Ficha_Electronica fe = ListaCuentaDetalleFichaE(fi.CodCue, fi.Item).FirstOrDefault();
            EspecialidadController e = new EspecialidadController();
            ViewBag.listaEspe = (List<E_Especialidades>)e.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true).ToList();
            ViewBag.listaEspecialidad = new SelectList(e.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true).ToList(), "CodEspec", "NomEspec", fi.Especialidad);
            ViewBag.listaCie10 = new SelectList(listCie10(), "CIe10", "ConcatCie10", fi.CIe10);
            PacientesController pa = new PacientesController();
            E_Pacientes pac = pa.ListadoPacientes().Find(x => x.Historia == fi.Historia);
            E_Ficha_Electronica f = ListaCuentaDetalleFichaE(fi.CodCue, fi.Item).FirstOrDefault();
            RecetasController rec = new RecetasController();
            ViewBag.listaFormaFarm = new SelectList(rec.ListadoFormaFarmaceutica().Where(x => x.Estado == true), "idFormFarm", "Descripcion", fi.FormaFarmec);
            ViewBag.listaFrecuencia = new SelectList(rec.ListadoFrecuenciaRecetas().Where(x => x.Estado == true), "idFrec", "Descripcion", fi.Frecuencia);
            int edad = DateTime.Today.AddTicks(-f.FechaNac.Ticks).Year - 1;
            ViewBag.sexo = pac.CodSexo;
            ViewBag.cuenta = fi.CodCue;
            ViewBag.item = fi.Item;
            ViewBag.historia = f.Historia;
            ViewBag.nombrePac = f.Paciente;
            ViewBag.FE = fi.FE;
            ViewBag.edad = edad;
            ViewBag.servicio = f.NomServ;
            ViewBag.tarifa = f.DescTar;



            ViewBag.CodAnt = fi.CodAnt;
            ViewBag.CancerP = fi.CancerP;
            ViewBag.DiabetesP = fi.DiabetesP;
            ViewBag.ACVP = fi.ACVP;
            ViewBag.AlergiaP = fi.AlergiaP;
            ViewBag.HipertArtP = fi.HipertArtP;
            ViewBag.OtrosP = fi.OtrosP;
            ViewBag.ObservacionP = fi.ObservacionP;

            ViewBag.CancerF = fi.CancerF;
            ViewBag.DiabetesF = fi.DiabetesF;
            ViewBag.ACVF = fi.ACVF;
            ViewBag.AlergiaF = fi.AlergiaF;
            ViewBag.HipertArtF = fi.HipertArtF;
            ViewBag.OtrosF = fi.OtrosF;
            ViewBag.ObservacionF = fi.ObservacionF;




            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();

            string Crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            if (fi.evento == 1)
            {
                if (fi.FE == -1)
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                        using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Ficha_Electronica", con, tr))
                        {
                            try
                            {
                                da.CommandType = CommandType.StoredProcedure;
                                da.Parameters.AddWithValue("@FE", "");
                                da.Parameters.AddWithValue("@Historia", fi.Historia);
                                if (fi.FC == null) { da.Parameters.AddWithValue("@FC", ""); } else { da.Parameters.AddWithValue("@FC", fi.FC); }
                                if (fi.PA == null) { da.Parameters.AddWithValue("@PA", ""); } else { da.Parameters.AddWithValue("@PA", fi.PA); }
                                if (fi.FR == null) { da.Parameters.AddWithValue("@FR", ""); } else { da.Parameters.AddWithValue("@FR", fi.FR); }
                                da.Parameters.AddWithValue("@Tax", fi.Tax);
                                da.Parameters.AddWithValue("@Tanal", fi.Tanal);
                                da.Parameters.AddWithValue("@Peso", fi.Peso);
                                da.Parameters.AddWithValue("@talla", fi.talla);
                                da.Parameters.AddWithValue("@IMC", fi.IMC);
                                if (fi.FUR == null) { da.Parameters.AddWithValue("@FUR", ""); } else { da.Parameters.AddWithValue("@FUR", fi.FUR); }
                                if (fi.MotivoConsulta == null) { da.Parameters.AddWithValue("@MotivoConsulta", ""); } else { da.Parameters.AddWithValue("@MotivoConsulta", fi.MotivoConsulta); }
                                if (fi.Relato == null) { da.Parameters.AddWithValue("@Relato", ""); } else { da.Parameters.AddWithValue("@Relato", fi.Relato); }
                                if (fi.TiempoEnfermedad == null) { da.Parameters.AddWithValue("@TiempoEnfermedad", ""); } else { da.Parameters.AddWithValue("@TiempoEnfermedad", fi.TiempoEnfermedad); }
                                if (fi.Inicio == null) { da.Parameters.AddWithValue("@Inicio", ""); } else { da.Parameters.AddWithValue("@Inicio", fi.Inicio); }
                                if (fi.Curso == null) { da.Parameters.AddWithValue("@Curso", ""); } else { da.Parameters.AddWithValue("@Curso", fi.Curso); }
                                if (fi.sed == null) { da.Parameters.AddWithValue("@sed", ""); } else { da.Parameters.AddWithValue("@sed", fi.sed); }
                                if (fi.sueño == null) { da.Parameters.AddWithValue("@sueno", ""); } else { da.Parameters.AddWithValue("@sueno", fi.sueño); }
                                if (fi.Orina == null) { da.Parameters.AddWithValue("@orina", ""); } else { da.Parameters.AddWithValue("@orina", fi.Orina); }
                                if (fi.apetito == null) { da.Parameters.AddWithValue("@apetito", ""); } else { da.Parameters.AddWithValue("@apetito", fi.apetito); }
                                da.Parameters.AddWithValue("@paridad", fi.paridad);
                                if (fi.FPP == null) { da.Parameters.AddWithValue("@FPP", ""); } else { da.Parameters.AddWithValue("@FPP", fi.FPP); }
                                da.Parameters.AddWithValue("@EdadGestacional", fi.EdadGestacional);
                                if (fi.PIG == null) { da.Parameters.AddWithValue("@PIG", ""); } else { da.Parameters.AddWithValue("@PIG", fi.PIG); }
                                if (fi.UltimoPap == null) { da.Parameters.AddWithValue("@UltimoPap", ""); } else { da.Parameters.AddWithValue("@UltimoPap", fi.UltimoPap); }
                                if (fi.ResultPap == null) { da.Parameters.AddWithValue("@ResultPap", ""); } else { da.Parameters.AddWithValue("@ResultPap", fi.ResultPap); }
                                if (fi.MACtipTime == null) { da.Parameters.AddWithValue("@MACtipTime", ""); } else { da.Parameters.AddWithValue("@MACtipTime", fi.MACtipTime); }
                                if (fi.Otrosginec == null) { da.Parameters.AddWithValue("@Otrosginec", ""); } else { da.Parameters.AddWithValue("@Otrosginec", fi.Otrosginec); }
                                if (fi.ProxCita == null) { da.Parameters.AddWithValue("@ProxCita", ""); } else { da.Parameters.AddWithValue("@ProxCita", fi.ProxCita); }
                                if (fi.ExaGeneral == null) { da.Parameters.AddWithValue("@ExaGeneral", ""); } else { da.Parameters.AddWithValue("@ExaGeneral", fi.ExaGeneral); }
                                if (fi.UbicEspTampa == null) { da.Parameters.AddWithValue("@UbicEspTampa", ""); } else { da.Parameters.AddWithValue("@UbicEspTampa", fi.UbicEspTampa); }
                                if (fi.EstNutricion == null) { da.Parameters.AddWithValue("@EstNutricion", ""); } else { da.Parameters.AddWithValue("@EstNutricion", fi.EstNutricion); }
                                if (fi.EstHidratacion == null) { da.Parameters.AddWithValue("@EstHidratacion", ""); } else { da.Parameters.AddWithValue("@EstHidratacion", fi.EstHidratacion); }
                                if (fi.PielFanerasTejido == null) { da.Parameters.AddWithValue("@PielFanerasTejido", ""); } else { da.Parameters.AddWithValue("@PielFanerasTejido", fi.PielFanerasTejido); }
                                if (fi.Mamas == null) { da.Parameters.AddWithValue("@Mamas", ""); } else { da.Parameters.AddWithValue("@Mamas", fi.Mamas); }
                                if (fi.SisRespiratorio == null) { da.Parameters.AddWithValue("@SisRespiratorio", ""); } else { da.Parameters.AddWithValue("@SisRespiratorio", fi.SisRespiratorio); }
                                if (fi.CabezaCuello == null) { da.Parameters.AddWithValue("@CabezaCuello", ""); } else { da.Parameters.AddWithValue("@CabezaCuello", fi.CabezaCuello); }
                                if (fi.SisOsteoMuscular == null) { da.Parameters.AddWithValue("@SisOsteoMuscular", ""); } else { da.Parameters.AddWithValue("@SisOsteoMuscular", fi.SisOsteoMuscular); }
                                if (fi.SisCardiovascular == null) { da.Parameters.AddWithValue("@SisCardiovascular", ""); } else { da.Parameters.AddWithValue("@SisCardiovascular", fi.SisCardiovascular); }
                                if (fi.AbdomenPelvis == null) { da.Parameters.AddWithValue("@AbdomenPelvis", ""); } else { da.Parameters.AddWithValue("@AbdomenPelvis", fi.AbdomenPelvis); }
                                if (fi.ExaObstetrico == null) { da.Parameters.AddWithValue("@ExaObstetrico", ""); } else { da.Parameters.AddWithValue("@ExaObstetrico", fi.ExaObstetrico); }
                                if (fi.SisGenitourinario == null) { da.Parameters.AddWithValue("@SisGenitourinario", ""); } else { da.Parameters.AddWithValue("@SisGenitourinario", fi.SisGenitourinario); }
                                if (fi.SisNervioso == null) { da.Parameters.AddWithValue("@SisNervioso", ""); } else { da.Parameters.AddWithValue("@SisNervioso", fi.SisNervioso); }
                                da.Parameters.AddWithValue("@Estado", "");
                                if (fi.Asistente == null) { da.Parameters.AddWithValue("@Asistente", ""); } else { da.Parameters.AddWithValue("@Asistente", fi.Asistente); }
                                if (fi.FecRegAsist == null) { da.Parameters.AddWithValue("@FecRegAsist", ""); } else { da.Parameters.AddWithValue("@FecRegAsist", fi.FecRegAsist); }
                                da.Parameters.AddWithValue("@Medico", me.CodMed);
                                da.Parameters.AddWithValue("@Servicio", me.CodServ);
                                da.Parameters.AddWithValue("@FecRegMed", horaSis.HoraServidor.ToShortDateString());
                                if (fi.InterConsulta == null) { da.Parameters.AddWithValue("@InterConsulta", ""); } else { da.Parameters.AddWithValue("@InterConsulta", fi.InterConsulta); }
                                da.Parameters.AddWithValue("@Tarifa", fe.CodTar);
                                da.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                da.Parameters.AddWithValue("@Item", fi.Item);
                                da.Parameters.AddWithValue("@Crea", Crea);
                                da.Parameters.AddWithValue("@Modifica", "");
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", 1);

                                int Resu = (int)da.ExecuteScalar();

                                using (SqlCommand cmd1 = new SqlCommand("Usp_Mantenimiento_Antecedentes", con, tr))
                                {
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.AddWithValue("@CodAnt", fi.CodAnt);
                                    cmd1.Parameters.AddWithValue("@Historia", fi.Historia);
                                    cmd1.Parameters.AddWithValue("@CancerP", fi.CancerP);
                                    cmd1.Parameters.AddWithValue("@DiabetesP", fi.DiabetesP);
                                    cmd1.Parameters.AddWithValue("@ACVP", fi.ACVP);
                                    cmd1.Parameters.AddWithValue("@AlergiaP", fi.AlergiaP);
                                    cmd1.Parameters.AddWithValue("@HipertArtP", fi.HipertArtP);
                                    cmd1.Parameters.AddWithValue("@OtrosP", fi.OtrosP);
                                    cmd1.Parameters.AddWithValue("@CancerF", fi.CancerF);
                                    cmd1.Parameters.AddWithValue("@DiabetesF", fi.DiabetesF);
                                    cmd1.Parameters.AddWithValue("@ACVF", fi.ACVF);
                                    cmd1.Parameters.AddWithValue("@AlergiaF", fi.AlergiaF);
                                    cmd1.Parameters.AddWithValue("@HipertArtF", fi.HipertArtF);
                                    cmd1.Parameters.AddWithValue("@OtrosF", fi.OtrosF);
                                    cmd1.Parameters.AddWithValue("@ObservacionP", fi.ObservacionP == null ? "" : fi.ObservacionP);
                                    cmd1.Parameters.AddWithValue("@ObservacionF", fi.ObservacionF == null ? "" : fi.ObservacionF);
                                    cmd1.Parameters.AddWithValue("@Usuario", usuario);
                                    cmd1.Parameters.AddWithValue("@Crea", "");
                                    cmd1.Parameters.AddWithValue("@Modifica", Crea);
                                    cmd1.Parameters.AddWithValue("@Elimina", "");
                                    cmd1.Parameters.AddWithValue("@Evento", 2);
                                    cmd1.ExecuteNonQuery();

                                    using (SqlCommand dd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr))
                                    {
                                        string turno = "";
                                        if (horaSis.HoraServidor.Hour <= 13)
                                        {
                                            turno = "MAÑANA";
                                        }
                                        else
                                        {
                                            turno = "TARDE";
                                        }
                                        dd.CommandType = CommandType.StoredProcedure;
                                        dd.Parameters.AddWithValue("@Procedencia", "");
                                        dd.Parameters.AddWithValue("@CodCue", fi.CodCue);
                                        dd.Parameters.AddWithValue("@Item", fi.Item);
                                        dd.Parameters.AddWithValue("@Tarifa", "");
                                        dd.Parameters.AddWithValue("@CodProce", "");
                                        dd.Parameters.AddWithValue("@CodDetalleP", "");
                                        dd.Parameters.AddWithValue("@CodSede", "");
                                        dd.Parameters.AddWithValue("@Cantidad", 0);
                                        dd.Parameters.AddWithValue("@precioUni", 0);
                                        dd.Parameters.AddWithValue("@precio", 0);
                                        dd.Parameters.AddWithValue("@igv", 0);
                                        dd.Parameters.AddWithValue("@total", 0);
                                        dd.Parameters.AddWithValue("@EstDet", "");
                                        dd.Parameters.AddWithValue("@FechaAten", horaSis.HoraServidor.ToShortDateString());
                                        dd.Parameters.AddWithValue("@TurnoAten", turno);
                                        dd.Parameters.AddWithValue("@RegMedico", me.CodMed);
                                        dd.Parameters.AddWithValue("@MedicoEnvia", "");
                                        dd.Parameters.AddWithValue("@Crea", "");
                                        dd.Parameters.AddWithValue("@Modifica", Crea);
                                        dd.Parameters.AddWithValue("@Elimina", "");
                                        dd.Parameters.AddWithValue("@Evento", "3");
                                        dd.ExecuteNonQuery();

                                        int cuentaDiag = 0;
                                        foreach (E_Ficha_Electronica it in (List<E_Ficha_Electronica>)Session["diagnostico"])
                                        {
                                            cuentaDiag++;

                                            using (SqlCommand av = new SqlCommand("Usp_Mantenimiento_Diagnostico", con, tr))
                                            {

                                                av.CommandType = CommandType.StoredProcedure;
                                                av.Parameters.AddWithValue("@Cie10", it.CIe10);
                                                av.Parameters.AddWithValue("@CodCue", fi.CodCue);
                                                av.Parameters.AddWithValue("@Procedencia", 1);
                                                av.Parameters.AddWithValue("@FE", Resu);
                                                av.Parameters.AddWithValue("@Historia", fi.Historia);
                                                av.Parameters.AddWithValue("@Descripcion", it.Descripcion.ToUpper());
                                                av.Parameters.AddWithValue("@Estado", 1);
                                                av.Parameters.AddWithValue("@Medico", me.CodMed);
                                                av.Parameters.AddWithValue("@Servicio", me.CodServ);
                                                av.Parameters.AddWithValue("@Item", cuentaDiag);
                                                av.Parameters.AddWithValue("@tipoDiagnostico", it.tipoDiagnostico);
                                                av.Parameters.AddWithValue("@Crea", Crea);
                                                av.Parameters.AddWithValue("@Modifica", "");
                                                av.Parameters.AddWithValue("@Elimina", "");
                                                av.Parameters.AddWithValue("@Evento", "1");

                                                av.ExecuteNonQuery();

                                            }
                                        }

                                        cuentaDiag = 0;
                                        foreach (E_Ficha_Electronica it in (List<E_Ficha_Electronica>)Session["examenAuxiliar"])
                                        {

                                            using (SqlCommand avc = new SqlCommand("Usp_Mantenimiento_ExamenAxiliares", con, tr))
                                            {
                                                cuentaDiag++;

                                                avc.CommandType = CommandType.StoredProcedure;
                                                avc.Parameters.AddWithValue("@EA", "");
                                                avc.Parameters.AddWithValue("@CodCue", fi.CodCue);
                                                avc.Parameters.AddWithValue("@FE", Resu);
                                                avc.Parameters.AddWithValue("@Especialidad", it.Especialidad);
                                                avc.Parameters.AddWithValue("@Tarifa", it.TarifaExamen);
                                                avc.Parameters.AddWithValue("@cant", it.cant);
                                                if (it.Otros == null)
                                                {
                                                    avc.Parameters.AddWithValue("@Otros", "");
                                                }
                                                else
                                                {
                                                    avc.Parameters.AddWithValue("@Otros", it.Otros);
                                                }
                                                avc.Parameters.AddWithValue("@Estado", 1);
                                                avc.Parameters.AddWithValue("@Item", cuentaDiag);
                                                avc.Parameters.AddWithValue("@Crea", Crea);
                                                avc.Parameters.AddWithValue("@Modifica", "");
                                                avc.Parameters.AddWithValue("@Evento", "1");

                                                avc.ExecuteNonQuery();

                                            }
                                        }

                                        cuentaDiag = 0;
                                        foreach (E_Ficha_Electronica it in (List<E_Ficha_Electronica>)Session["recetas"])
                                        {

                                            using (SqlCommand avcd = new SqlCommand("Usp_Mantenimiento_Recetas", con, tr))
                                            {
                                                cuentaDiag++;

                                                avcd.CommandType = CommandType.StoredProcedure;
                                                avcd.Parameters.AddWithValue("@CodFarmaco", "");
                                                avcd.Parameters.AddWithValue("@Descripcion", it.Descripcion);
                                                avcd.Parameters.AddWithValue("@FE", Resu);
                                                avcd.Parameters.AddWithValue("@Procedencia", 1);
                                                avcd.Parameters.AddWithValue("@Concentra", it.Concentra);
                                                avcd.Parameters.AddWithValue("@FormaFarmec", it.FormaFarmec);
                                                avcd.Parameters.AddWithValue("@Dosis", it.Dosis);
                                                avcd.Parameters.AddWithValue("@Frecuencia", it.Frecuencia);
                                                avcd.Parameters.AddWithValue("@ViaAdmin", it.ViaAdmin);
                                                avcd.Parameters.AddWithValue("@Duracion", it.Duracion);
                                                avcd.Parameters.AddWithValue("@CodCue", fi.CodCue);
                                                avcd.Parameters.AddWithValue("@Cantidad", it.Cantidad);
                                                avcd.Parameters.AddWithValue("@Estado", 1);
                                                avcd.Parameters.AddWithValue("@CodMed", me.CodMed);
                                                avcd.Parameters.AddWithValue("@Servicio", me.CodServ);
                                                if (it.Obsv == null)
                                                {
                                                    avcd.Parameters.AddWithValue("@Obsv", "");
                                                }
                                                else
                                                {
                                                    avcd.Parameters.AddWithValue("@Obsv", it.Obsv);
                                                }
                                                avcd.Parameters.AddWithValue("@Item", cuentaDiag);
                                                avcd.Parameters.AddWithValue("@Crea", Crea);
                                                avcd.Parameters.AddWithValue("@Modifica", "");
                                                avcd.Parameters.AddWithValue("@Elimina", "");
                                                avcd.Parameters.AddWithValue("@Evento", "1");

                                                avcd.ExecuteNonQuery();

                                            }
                                        }

                                    }


                                }


                                tr.Commit();
                                ViewBag.mensaje = "Pedido registrado";

                            }
                            catch (Exception ex)
                            {
                                tr.Rollback();
                                return RedirectPermanent("../Master");
                            }
                            finally
                            {
                                con.Close();
                            }
                        }

                    }

                    if (Session["diagnostico"] != null)
                    {
                        Session.Remove("diagnostico");
                    }

                    if (Session["examenAuxiliar"] != null)
                    {
                        Session.Remove("examenAuxiliar");
                    }

                    if (Session["recetas"] != null)
                    {
                        Session.Remove("recetas");
                    }


                    return RedirectToAction("BandejaDeAtenciones");
                }





                else
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                        using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Ficha_Electronica", con, tr))
                        {
                            try
                            {
                                da.CommandType = CommandType.StoredProcedure;
                                da.Parameters.AddWithValue("@FE", fi.FE);
                                da.Parameters.AddWithValue("@Historia", fi.Historia);
                                if (fi.FC == null) { da.Parameters.AddWithValue("@FC", ""); } else { da.Parameters.AddWithValue("@FC", fi.FC); }
                                if (fi.PA == null) { da.Parameters.AddWithValue("@PA", ""); } else { da.Parameters.AddWithValue("@PA", fi.PA); }
                                if (fi.FR == null) { da.Parameters.AddWithValue("@FR", ""); } else { da.Parameters.AddWithValue("@FR", fi.FR); }
                                da.Parameters.AddWithValue("@Tax", fi.Tax);
                                da.Parameters.AddWithValue("@Tanal", fi.Tanal);
                                da.Parameters.AddWithValue("@Peso", fi.Peso);
                                da.Parameters.AddWithValue("@talla", fi.talla);
                                da.Parameters.AddWithValue("@IMC", fi.IMC);
                                if (fi.FUR == null) { da.Parameters.AddWithValue("@FUR", ""); } else { da.Parameters.AddWithValue("@FUR", fi.FUR); }
                                if (fi.MotivoConsulta == null) { da.Parameters.AddWithValue("@MotivoConsulta", ""); } else { da.Parameters.AddWithValue("@MotivoConsulta", fi.MotivoConsulta); }
                                if (fi.Relato == null) { da.Parameters.AddWithValue("@Relato", ""); } else { da.Parameters.AddWithValue("@Relato", fi.Relato); }
                                if (fi.TiempoEnfermedad == null) { da.Parameters.AddWithValue("@TiempoEnfermedad", ""); } else { da.Parameters.AddWithValue("@TiempoEnfermedad", fi.TiempoEnfermedad); }
                                if (fi.Inicio == null) { da.Parameters.AddWithValue("@Inicio", ""); } else { da.Parameters.AddWithValue("@Inicio", fi.Inicio); }
                                if (fi.Curso == null) { da.Parameters.AddWithValue("@Curso", ""); } else { da.Parameters.AddWithValue("@Curso", fi.Curso); }
                                if (fi.sed == null) { da.Parameters.AddWithValue("@sed", ""); } else { da.Parameters.AddWithValue("@sed", fi.sed); }
                                if (fi.sueño == null) { da.Parameters.AddWithValue("@sueno", ""); } else { da.Parameters.AddWithValue("@sueno", fi.sueño); }
                                if (fi.Orina == null) { da.Parameters.AddWithValue("@orina", ""); } else { da.Parameters.AddWithValue("@orina", fi.Orina); }
                                if (fi.apetito == null) { da.Parameters.AddWithValue("@apetito", ""); } else { da.Parameters.AddWithValue("@apetito", fi.apetito); }
                                da.Parameters.AddWithValue("@paridad", fi.paridad);
                                if (fi.FPP == null) { da.Parameters.AddWithValue("@FPP", ""); } else { da.Parameters.AddWithValue("@FPP", fi.FPP); }
                                da.Parameters.AddWithValue("@EdadGestacional", fi.EdadGestacional);
                                if (fi.PIG == null) { da.Parameters.AddWithValue("@PIG", ""); } else { da.Parameters.AddWithValue("@PIG", fi.PIG); }
                                if (fi.UltimoPap == null) { da.Parameters.AddWithValue("@UltimoPap", ""); } else { da.Parameters.AddWithValue("@UltimoPap", fi.UltimoPap); }
                                if (fi.ResultPap == null) { da.Parameters.AddWithValue("@ResultPap", ""); } else { da.Parameters.AddWithValue("@ResultPap", fi.ResultPap); }
                                if (fi.MACtipTime == null) { da.Parameters.AddWithValue("@MACtipTime", ""); } else { da.Parameters.AddWithValue("@MACtipTime", fi.MACtipTime); }
                                if (fi.Otrosginec == null) { da.Parameters.AddWithValue("@Otrosginec", ""); } else { da.Parameters.AddWithValue("@Otrosginec", fi.Otrosginec); }
                                if (fi.ProxCita == null) { da.Parameters.AddWithValue("@ProxCita", ""); } else { da.Parameters.AddWithValue("@ProxCita", fi.ProxCita); }
                                if (fi.ExaGeneral == null) { da.Parameters.AddWithValue("@ExaGeneral", ""); } else { da.Parameters.AddWithValue("@ExaGeneral", fi.ExaGeneral); }
                                if (fi.UbicEspTampa == null) { da.Parameters.AddWithValue("@UbicEspTampa", ""); } else { da.Parameters.AddWithValue("@UbicEspTampa", fi.UbicEspTampa); }
                                if (fi.EstNutricion == null) { da.Parameters.AddWithValue("@EstNutricion", ""); } else { da.Parameters.AddWithValue("@EstNutricion", fi.EstNutricion); }
                                if (fi.EstHidratacion == null) { da.Parameters.AddWithValue("@EstHidratacion", ""); } else { da.Parameters.AddWithValue("@EstHidratacion", fi.EstHidratacion); }
                                if (fi.PielFanerasTejido == null) { da.Parameters.AddWithValue("@PielFanerasTejido", ""); } else { da.Parameters.AddWithValue("@PielFanerasTejido", fi.PielFanerasTejido); }
                                if (fi.Mamas == null) { da.Parameters.AddWithValue("@Mamas", ""); } else { da.Parameters.AddWithValue("@Mamas", fi.Mamas); }
                                if (fi.SisRespiratorio == null) { da.Parameters.AddWithValue("@SisRespiratorio", ""); } else { da.Parameters.AddWithValue("@SisRespiratorio", fi.SisRespiratorio); }
                                if (fi.CabezaCuello == null) { da.Parameters.AddWithValue("@CabezaCuello", ""); } else { da.Parameters.AddWithValue("@CabezaCuello", fi.CabezaCuello); }
                                if (fi.SisOsteoMuscular == null) { da.Parameters.AddWithValue("@SisOsteoMuscular", ""); } else { da.Parameters.AddWithValue("@SisOsteoMuscular", fi.SisOsteoMuscular); }
                                if (fi.SisCardiovascular == null) { da.Parameters.AddWithValue("@SisCardiovascular", ""); } else { da.Parameters.AddWithValue("@SisCardiovascular", fi.SisCardiovascular); }
                                if (fi.AbdomenPelvis == null) { da.Parameters.AddWithValue("@AbdomenPelvis", ""); } else { da.Parameters.AddWithValue("@AbdomenPelvis", fi.AbdomenPelvis); }
                                if (fi.ExaObstetrico == null) { da.Parameters.AddWithValue("@ExaObstetrico", ""); } else { da.Parameters.AddWithValue("@ExaObstetrico", fi.ExaObstetrico); }
                                if (fi.SisGenitourinario == null) { da.Parameters.AddWithValue("@SisGenitourinario", ""); } else { da.Parameters.AddWithValue("@SisGenitourinario", fi.SisGenitourinario); }
                                if (fi.SisNervioso == null) { da.Parameters.AddWithValue("@SisNervioso", ""); } else { da.Parameters.AddWithValue("@SisNervioso", fi.SisNervioso); }
                                da.Parameters.AddWithValue("@Estado", "");
                                if (fi.Asistente == null) { da.Parameters.AddWithValue("@Asistente", ""); } else { da.Parameters.AddWithValue("@Asistente", fi.Asistente); }
                                if (fi.FecRegAsist == null) { da.Parameters.AddWithValue("@FecRegAsist", ""); } else { da.Parameters.AddWithValue("@FecRegAsist", fi.FecRegAsist); }
                                da.Parameters.AddWithValue("@Medico", me.CodMed);
                                da.Parameters.AddWithValue("@Servicio", me.CodServ);
                                da.Parameters.AddWithValue("@FecRegMed", horaSis.HoraServidor.ToShortDateString());
                                if (fi.InterConsulta == null) { da.Parameters.AddWithValue("@InterConsulta", ""); } else { da.Parameters.AddWithValue("@InterConsulta", fi.InterConsulta); }
                                da.Parameters.AddWithValue("@Tarifa", fe.CodTar);
                                da.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                da.Parameters.AddWithValue("@Item", fi.Item);
                                da.Parameters.AddWithValue("@Crea", Crea);
                                da.Parameters.AddWithValue("@Modifica", "");
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", 2);

                                da.ExecuteNonQuery();

                                using (SqlCommand cmd1 = new SqlCommand("Usp_Mantenimiento_Antecedentes", con, tr))
                                {
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.AddWithValue("@CodAnt", fi.CodAnt);
                                    cmd1.Parameters.AddWithValue("@Historia", fi.Historia);
                                    cmd1.Parameters.AddWithValue("@CancerP", fi.CancerP);
                                    cmd1.Parameters.AddWithValue("@DiabetesP", fi.DiabetesP);
                                    cmd1.Parameters.AddWithValue("@ACVP", fi.ACVP);
                                    cmd1.Parameters.AddWithValue("@AlergiaP", fi.AlergiaP);
                                    cmd1.Parameters.AddWithValue("@HipertArtP", fi.HipertArtP);
                                    cmd1.Parameters.AddWithValue("@OtrosP", fi.OtrosP);
                                    cmd1.Parameters.AddWithValue("@CancerF", fi.CancerF);
                                    cmd1.Parameters.AddWithValue("@DiabetesF", fi.DiabetesF);
                                    cmd1.Parameters.AddWithValue("@ACVF", fi.ACVF);
                                    cmd1.Parameters.AddWithValue("@AlergiaF", fi.AlergiaF);
                                    cmd1.Parameters.AddWithValue("@HipertArtF", fi.HipertArtF);
                                    cmd1.Parameters.AddWithValue("@OtrosF", fi.OtrosF);
                                    cmd1.Parameters.AddWithValue("@ObservacionP", fi.ObservacionP == null ? "" : fi.ObservacionP);
                                    cmd1.Parameters.AddWithValue("@ObservacionF", fi.ObservacionF == null ? "" : fi.ObservacionF);
                                    cmd1.Parameters.AddWithValue("@Usuario", usuario);
                                    cmd1.Parameters.AddWithValue("@Crea", "");
                                    cmd1.Parameters.AddWithValue("@Modifica", Crea);
                                    cmd1.Parameters.AddWithValue("@Elimina", "");
                                    cmd1.Parameters.AddWithValue("@Evento", 2);
                                    cmd1.ExecuteNonQuery();

                                    using (SqlCommand dd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr))
                                    {
                                        string turno = "";
                                        if (horaSis.HoraServidor.Hour <= 13)
                                        {
                                            turno = "MAÑANA";
                                        }
                                        else
                                        {
                                            turno = "TARDE";
                                        }
                                        dd.CommandType = CommandType.StoredProcedure;
                                        dd.Parameters.AddWithValue("@Procedencia", "");
                                        dd.Parameters.AddWithValue("@CodCue", fi.CodCue);
                                        dd.Parameters.AddWithValue("@Item", fi.Item);
                                        dd.Parameters.AddWithValue("@Tarifa", "");
                                        dd.Parameters.AddWithValue("@CodProce", "");
                                        dd.Parameters.AddWithValue("@CodDetalleP", "");
                                        dd.Parameters.AddWithValue("@CodSede", "");
                                        dd.Parameters.AddWithValue("@Cantidad", 0);
                                        dd.Parameters.AddWithValue("@precioUni", 0);
                                        dd.Parameters.AddWithValue("@precio", 0);
                                        dd.Parameters.AddWithValue("@igv", 0);
                                        dd.Parameters.AddWithValue("@total", 0);
                                        dd.Parameters.AddWithValue("@EstDet", "");
                                        dd.Parameters.AddWithValue("@FechaAten", horaSis.HoraServidor.ToShortDateString());
                                        dd.Parameters.AddWithValue("@TurnoAten", turno);
                                        dd.Parameters.AddWithValue("@RegMedico", me.CodMed);
                                        dd.Parameters.AddWithValue("@MedicoEnvia", "");
                                        dd.Parameters.AddWithValue("@Crea", "");
                                        dd.Parameters.AddWithValue("@Modifica", Crea);
                                        dd.Parameters.AddWithValue("@Elimina", "");
                                        dd.Parameters.AddWithValue("@Evento", "3");
                                        dd.ExecuteNonQuery();

                                        int cuentaDiag = 0;
                                        foreach (E_Ficha_Electronica it in (List<E_Ficha_Electronica>)Session["diagnostico"])
                                        {
                                            cuentaDiag++;

                                            using (SqlCommand av = new SqlCommand("Usp_Mantenimiento_Diagnostico", con, tr))
                                            {

                                                av.CommandType = CommandType.StoredProcedure;
                                                av.Parameters.AddWithValue("@Cie10", it.CIe10);
                                                av.Parameters.AddWithValue("@CodCue", fi.CodCue);
                                                av.Parameters.AddWithValue("@Procedencia", 1);
                                                av.Parameters.AddWithValue("@FE", fi.FE);
                                                av.Parameters.AddWithValue("@Historia", fi.Historia);
                                                av.Parameters.AddWithValue("@Descripcion", it.Descripcion);
                                                av.Parameters.AddWithValue("@Estado", 1);
                                                av.Parameters.AddWithValue("@Medico", me.CodMed);
                                                av.Parameters.AddWithValue("@Servicio", me.CodServ);
                                                av.Parameters.AddWithValue("@Item", cuentaDiag);
                                                av.Parameters.AddWithValue("@tipoDiagnostico", it.tipoDiagnostico);
                                                av.Parameters.AddWithValue("@Crea", Crea);
                                                av.Parameters.AddWithValue("@Modifica", "");
                                                av.Parameters.AddWithValue("@Elimina", "");
                                                av.Parameters.AddWithValue("@Evento", "1");

                                                av.ExecuteNonQuery();

                                            }
                                        }

                                        foreach (E_Ficha_Electronica it in (List<E_Ficha_Electronica>)Session["examenAuxiliar"])
                                        {

                                            using (SqlCommand avc = new SqlCommand("Usp_Mantenimiento_ExamenAxiliares", con, tr))
                                            {
                                                cuentaDiag++;

                                                avc.CommandType = CommandType.StoredProcedure;
                                                avc.Parameters.AddWithValue("@EA", "");
                                                avc.Parameters.AddWithValue("@CodCue", fi.CodCue);
                                                avc.Parameters.AddWithValue("@FE", fi.FE);
                                                avc.Parameters.AddWithValue("@Especialidad", it.Especialidad);
                                                avc.Parameters.AddWithValue("@Tarifa", it.TarifaExamen);
                                                avc.Parameters.AddWithValue("@cant", it.cant);
                                                if (it.Otros == null)
                                                {
                                                    avc.Parameters.AddWithValue("@Otros", "");
                                                }
                                                else
                                                {
                                                    avc.Parameters.AddWithValue("@Otros", it.Otros);
                                                }
                                                avc.Parameters.AddWithValue("@Estado", 1);
                                                avc.Parameters.AddWithValue("@Item", cuentaDiag);
                                                avc.Parameters.AddWithValue("@Crea", Crea);
                                                avc.Parameters.AddWithValue("@Modifica", "");
                                                avc.Parameters.AddWithValue("@Evento", "1");

                                                avc.ExecuteNonQuery();

                                            }
                                        }

                                        foreach (E_Ficha_Electronica it in (List<E_Ficha_Electronica>)Session["recetas"])
                                        {

                                            using (SqlCommand avcd = new SqlCommand("Usp_Mantenimiento_Recetas", con, tr))
                                            {
                                                cuentaDiag++;

                                                avcd.CommandType = CommandType.StoredProcedure;
                                                avcd.Parameters.AddWithValue("@CodFarmaco", "");
                                                avcd.Parameters.AddWithValue("@Descripcion", it.Descripcion);
                                                avcd.Parameters.AddWithValue("@FE", fi.FE);
                                                avcd.Parameters.AddWithValue("@Procedencia", 1);
                                                avcd.Parameters.AddWithValue("@Concentra", it.Concentra);
                                                avcd.Parameters.AddWithValue("@FormaFarmec", it.FormaFarmec);
                                                avcd.Parameters.AddWithValue("@Dosis", it.Dosis);
                                                avcd.Parameters.AddWithValue("@Frecuencia", it.Frecuencia);
                                                avcd.Parameters.AddWithValue("@ViaAdmin", it.ViaAdmin);
                                                avcd.Parameters.AddWithValue("@Duracion", it.Duracion);
                                                avcd.Parameters.AddWithValue("@CodCue", fi.CodCue);
                                                avcd.Parameters.AddWithValue("@Cantidad", it.Cantidad);
                                                avcd.Parameters.AddWithValue("@Estado", 1);
                                                avcd.Parameters.AddWithValue("@CodMed", me.CodMed);
                                                avcd.Parameters.AddWithValue("@Servicio", me.CodServ);
                                                if (it.Obsv == null)
                                                {
                                                    avcd.Parameters.AddWithValue("@Obsv", "");
                                                }
                                                else
                                                {
                                                    avcd.Parameters.AddWithValue("@Obsv", it.Obsv);
                                                }
                                                avcd.Parameters.AddWithValue("@Item", cuentaDiag);
                                                avcd.Parameters.AddWithValue("@Crea", Crea);
                                                avcd.Parameters.AddWithValue("@Modifica", "");
                                                avcd.Parameters.AddWithValue("@Elimina", "");
                                                avcd.Parameters.AddWithValue("@Evento", "1");

                                                avcd.ExecuteNonQuery();

                                            }
                                        }

                                    }


                                }


                                tr.Commit();
                                ViewBag.mensaje = "Pedido registrado";

                            }
                            catch (Exception ex)
                            {
                                tr.Rollback();
                                return RedirectPermanent("../Master");
                            }
                            finally
                            {
                                con.Close();
                            }
                        }

                    }

                    if (Session["diagnostico"] != null)
                    {
                        Session.Remove("diagnostico");
                    }

                    if (Session["examenAuxiliar"] != null)
                    {
                        Session.Remove("examenAuxiliar");
                    }

                    if (Session["recetas"] != null)
                    {
                        Session.Remove("recetas");
                    }


                    return RedirectToAction("BandejaDeAtenciones");

                }


            }
            else if (fi.evento == 2)
            {
                ViewBag.tab = "4";
                var formularios = (List<E_Ficha_Electronica>)Session["diagnostico"];
                E_Ficha_Electronica item = new E_Ficha_Electronica();
                E_Ficha_Electronica BuscaCie10 = listCie10().Find(x => x.CIe10 == fi.CIe10);

                item.CIe10 = fi.CIe10;
                item.Descripcion = BuscaCie10.Descripcion;
                item.tipoDiagnostico = fi.tipoDiagnostico;
                formularios.Add(item);
                Session["diagnostico"] = formularios;

            }
            else if (fi.evento == 3)
            {
                ViewBag.tab = "4";
                var formularios = (List<E_Ficha_Electronica>)Session["diagnostico"];
                var registro = formularios.Where(x => x.CIe10.Equals(fi.EliminaDiagnostico)).FirstOrDefault();
                formularios.Remove(registro);
                Session["diagnostico"] = formularios;

            }
            if (fi.evento == 4)
            {

                ViewBag.tab = "5";
                EspecialidadController es = new EspecialidadController();
                TarifarioController t = new TarifarioController();
                var formularios = (List<E_Ficha_Electronica>)Session["examenAuxiliar"];
                E_Ficha_Electronica item = new E_Ficha_Electronica();
                item.idSesionExamen = formularios.Count();
                if (fi.Especialidad != null)
                {
                    E_Especialidades esp = es.ListadoEspecialidades().Find(x => x.CodEspec == fi.Especialidad);
                    item.DescrEspecialidad = esp.NomEspec;
                    item.Especialidad = fi.Especialidad;
                }
                else
                {
                    item.DescrEspecialidad = "[NO SELECCIONADO]";
                    item.Especialidad = "";
                }
                if (fi.TarifaExamen != "nulo")
                {
                    E_Tarifario tar = t.ListadoTarifa().Find(x => x.CodTar == fi.TarifaExamen);
                    item.TarifaExamen = fi.TarifaExamen;
                    item.DescrTarifaExamen = tar.DescTar;
                    ViewBag.DescrTarifaExamen = tar.DescTar;
                }
                else
                {
                    item.TarifaExamen = "";
                    item.DescrTarifaExamen = "[NO SELECCIONADO]";
                    ViewBag.DescrTarifaExamen = "";
                }

                item.cant = fi.cant;
                item.Otros = fi.Otros;

                formularios.Add(item);
                Session["examenAuxiliar"] = formularios;

                ViewBag.TarifaExamen = fi.TarifaExamen;
            }
            else if (fi.evento == 5)
            {
                ViewBag.tab = "5";
                var formularios = (List<E_Ficha_Electronica>)Session["examenAuxiliar"];
                var registro = formularios.Where(x => x.idSesionExamen == fi.EliminaExamen).FirstOrDefault();
                formularios.Remove(registro);
                Session["examenAuxiliar"] = formularios;

            }
            else if (fi.evento == 6)
            {
                ViewBag.tab = "6";
                var formularios = (List<E_Ficha_Electronica>)Session["recetas"];
                E_Ficha_Electronica item = new E_Ficha_Electronica();
                item.idRecetaSesion = formularios.Count();
                item.Descripcion = fi.Descripcion.ToUpper();
                item.Concentra = fi.Concentra.ToUpper();
                item.FormaFarmec = fi.FormaFarmec;
                item.Dosis = fi.Dosis.ToUpper();
                item.Frecuencia = fi.Frecuencia;
                item.ViaAdmin = fi.ViaAdmin.ToUpper();
                item.Duracion = fi.Duracion.ToUpper();
                item.Cantidad = fi.Cantidad;
                if (fi.Obsv == null)
                {
                    item.Obsv = "";
                }
                else
                {
                    item.Obsv = fi.Obsv.ToUpper();
                }

                ViewBag.borraDatos = 1;

                formularios.Add(item);
                Session["recetas"] = formularios;

            }

            else if (fi.evento == 7)
            {
                ViewBag.tab = "6";
                var formularios = (List<E_Ficha_Electronica>)Session["recetas"];
                var registro = formularios.Where(x => x.idRecetaSesion == fi.idRecetaSesion).FirstOrDefault();
                formularios.Remove(registro);
                Session["recetas"] = formularios;

            }

            else if (fi.evento == 8)
            {
                var lista = (from x in ListaFichaElectronica(fi.Historia, me.CodServ) select x).FirstOrDefault();
                if (lista == null)
                {
                    ViewBag.tab = "1";
                    ViewBag.mensaje = "No existe registro diario de este paciente en el servicio";
                    return View(fi);
                }
                else
                {
                    return RedirectPermanent("RegistroDeAtencionesMedica/?CodCue=" + fi.CodCue + "&Item=" + fi.Item + "&Historia=" + fi.Historia + "&FE=" + lista.FE);
                }
            }

            //Lista Sesiones
            ViewBag.Listadiagnostico = (List<E_Ficha_Electronica>)Session["diagnostico"];
            ViewBag.ListaExamenAuxiliar = (List<E_Ficha_Electronica>)Session["examenAuxiliar"];
            ViewBag.ListaRecetas = (List<E_Ficha_Electronica>)Session["recetas"];

            return View(fi);

        }


        

        public ActionResult RegistroDeProcedimientosMedicos(int CodCue, int Item, int Historia, int? FE = null)
        {
            if (FE != null)
            {
                string sede = Session["codSede"].ToString();
                TarifarioController ta = new TarifarioController();
                ViewBag.antecedentes = (List<E_Ficha_Electronica>)AntecedentexUsuario(Historia);
                string usuario = Session["UserID"].ToString();
                E_Medico me = listaMedicoUsuario(usuario).FirstOrDefault();
                EspecialidadController e = new EspecialidadController();
                ViewBag.antecedentes = (List<E_Ficha_Electronica>)AntecedentexUsuario(Historia);
                ViewBag.listaEspe = (List<E_Especialidades>)e.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true).ToList();
                ViewBag.listaEspecialidad = new SelectList(e.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true), "CodEspec", "NomEspec");
                ViewBag.listaCie10 = new SelectList(listCie10(), "CIe10", "ConcatCie10");
                E_Ficha_Electronica f = ListaCuentaDetalleFichaE(CodCue, Item).FirstOrDefault();
                PacientesController pa = new PacientesController();
                E_Pacientes pac = pa.ListadoPacientes().Find(x => x.Historia == Historia);
                E_Tarifario tar = ta.ListadoTarifa().Find(x => x.CodTar == f.CodTar);
                E_Tarifario per = ta.ListadoPerfilesFichaElectronica().Find(x => x.idPFA == tar.idPFA);
                int edad = DateTime.Today.AddTicks(-f.FechaNac.Ticks).Year - 1;
                ViewBag.perfil = new SelectList(ta.ListadoPerfilesFichaElectronica().Where(x => x.CodEspec == f.CodEspec || x.CodEspec == "ES004"), "idPFA", "Nombre");
                RecetasController rec = new RecetasController();
                ViewBag.listaFormaFarm = new SelectList(rec.ListadoFormaFarmaceutica().Where(x => x.Estado == true), "idFormFarm", "Descripcion");
                ViewBag.listaFrecuencia = new SelectList(rec.ListadoFrecuenciaRecetas().Where(x => x.Estado == true), "idFrec", "Descripcion");
                ViewBag.CodPerf = tar.idPFA;
                ViewBag.tab = "1";
                ViewBag.sexo = pac.CodSexo;
                ViewBag.cuenta = CodCue;
                ViewBag.item = Item;
                ViewBag.historia = Historia;
                ViewBag.idProc = FE;
                ViewBag.nombrePac = f.Paciente;
                ViewBag.edad = edad;
                ViewBag.servicio = f.NomServ;
                ViewBag.tarifa = f.DescTar;
                ViewBag.contenido = per.contenido;
                ViewBag.CodServMed = me.CodServ;

                if (Session["diagnosticoProc"] == null)
                {
                    Session["diagnosticoProc"] = new List<E_Ficha_Electronica>();
                }
                else
                {
                    Session.Remove("diagnosticoProc");
                    Session["diagnosticoProc"] = new List<E_Ficha_Electronica>();
                }

                if (Session["recetaProc"] == null)
                {
                    Session["recetaProc"] = new List<E_Ficha_Electronica>();
                }
                else
                {
                    Session.Remove("recetaProc");
                    Session["recetaProc"] = new List<E_Ficha_Electronica>();
                }


                int idpro = (int)FE;
                var lista = (from x in ListaProcedimiento(idpro) select x).FirstOrDefault();
                ViewBag.Asistente = lista.Asistente;
                ViewBag.FecRegAsist = lista.FecRegAsist;

                return View(lista);

            }

            else
            {

                string sede = Session["codSede"].ToString();
                PacientesController pa = new PacientesController();
                TarifarioController ta = new TarifarioController();
                string usuario = Session["UserID"].ToString();
                E_Medico me = listaMedicoUsuario(usuario).FirstOrDefault();
                E_Pacientes pac = pa.ListadoPacientes().Find(x => x.Historia == Historia);
                EspecialidadController e = new EspecialidadController();
                ViewBag.antecedentes = (List<E_Ficha_Electronica>)AntecedentexUsuario(Historia);
                ViewBag.listaEspe = (List<E_Especialidades>)e.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true).ToList();
                ViewBag.listaEspecialidad = new SelectList(e.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true), "CodEspec", "NomEspec");
                ViewBag.listaCie10 = new SelectList(listCie10(), "CIe10", "ConcatCie10");
                E_Ficha_Electronica f = ListaCuentaDetalleFichaE(CodCue, Item).FirstOrDefault();
                E_Tarifario tar = ta.ListadoTarifa().Find(x => x.CodTar == f.CodTar);
                E_Tarifario per = ta.ListadoPerfilesFichaElectronica().Find(x => x.idPFA == tar.idPFA);
                RecetasController rec = new RecetasController();
                ViewBag.listaFormaFarm = new SelectList(rec.ListadoFormaFarmaceutica().Where(x => x.Estado == true), "idFormFarm", "Descripcion");
                ViewBag.listaFrecuencia = new SelectList(rec.ListadoFrecuenciaRecetas().Where(x => x.Estado == true), "idFrec", "Descripcion");
                int edad = DateTime.Today.AddTicks(-f.FechaNac.Ticks).Year - 1;
                ViewBag.tab = "1";
                ViewBag.perfil = new SelectList(ta.ListadoPerfilesFichaElectronica().Where(x => x.CodEspec == f.CodEspec || x.CodEspec == "ES004"), "idPFA", "Nombre");
                ViewBag.CodPerf = tar.idPFA;
                ViewBag.sexo = pac.CodSexo;
                ViewBag.cuenta = CodCue;
                ViewBag.item = Item;
                ViewBag.idProc = -1;
                ViewBag.historia = Historia;
                ViewBag.nombrePac = f.Paciente;
                ViewBag.edad = edad;
                ViewBag.servicio = f.NomServ;
                ViewBag.tarifa = f.DescTar;
                ViewBag.contenido = per.contenido;
                ViewBag.CodServMed = me.CodServ;

                //Sesiones

                //Sesion de diagnostico
                if (Session["diagnosticoProc"] == null)
                {
                    Session["diagnosticoProc"] = new List<E_Ficha_Electronica>();
                }
                else
                {
                    Session.Remove("diagnosticoProc");
                    Session["diagnosticoProc"] = new List<E_Ficha_Electronica>();
                }

                if (Session["recetaProc"] == null)
                {
                    Session["recetaProc"] = new List<E_Ficha_Electronica>();
                }
                else
                {
                    Session.Remove("recetaProc");
                    Session["recetaProc"] = new List<E_Ficha_Electronica>();
                }


                return View();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult RegistroDeProcedimientosMedicos(E_Ficha_Electronica fe)
        {
            string sede = Session["codSede"].ToString();
            PacientesController pa = new PacientesController();
            TarifarioController ta = new TarifarioController();
            E_Pacientes pac = pa.ListadoPacientes().Find(x => x.Historia == fe.Historia);
            EspecialidadController e = new EspecialidadController();
            string usuario = Session["UserID"].ToString();
            ViewBag.contenido = fe.DescripcionProc;
            E_Medico me = listaMedicoUsuario(usuario).FirstOrDefault();
            ViewBag.antecedentes = (List<E_Ficha_Electronica>)AntecedentexUsuario(fe.Historia);
            ViewBag.listaEspe = (List<E_Especialidades>)e.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true).ToList();
            ViewBag.listaEspecialidad = new SelectList(e.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true), "CodEspec", "NomEspec");
            RecetasController rec = new RecetasController();
            ViewBag.listaFormaFarm = new SelectList(rec.ListadoFormaFarmaceutica().Where(x => x.Estado == true), "idFormFarm", "Descripcion", fe.FormaFarmec);
            ViewBag.listaFrecuencia = new SelectList(rec.ListadoFrecuenciaRecetas().Where(x => x.Estado == true), "idFrec", "Descripcion", fe.Frecuencia);
            ViewBag.listaCie10 = new SelectList(listCie10(), "CIe10", "ConcatCie10");
            E_Ficha_Electronica f = ListaCuentaDetalleFichaE(fe.CodCue, fe.Item).FirstOrDefault();
            int edad = DateTime.Today.AddTicks(-f.FechaNac.Ticks).Year - 1;
            ViewBag.tab = "1";
            ViewBag.perfil = new SelectList(ta.ListadoPerfilesFichaElectronica().Where(x => x.CodEspec == f.CodEspec || x.CodEspec == "ES004"), "idPFA", "Nombre");
            ViewBag.CodPerf = fe.idPFA;
            ViewBag.Asistente = fe.Asistente;
            ViewBag.FecRegAsist = fe.FecRegAsist;
            ViewBag.sexo = pac.CodSexo;
            ViewBag.cuenta = fe.CodCue;
            ViewBag.item = fe.Item;
            ViewBag.idProc = fe.idProc;
            ViewBag.historia = fe.Historia;
            ViewBag.nombrePac = f.Paciente;
            ViewBag.edad = edad;
            ViewBag.servicio = f.NomServ;
            ViewBag.tarifa = f.DescTar;

            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string Crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            if (fe.evento == 1)
            {
                if (fe.idProc == -1)
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                        using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Procedimientos", con, tr))
                        {
                            try
                            {
                                da.CommandType = CommandType.StoredProcedure;
                                da.Parameters.AddWithValue("@idProc", "");
                                if (fe.FC == null) { da.Parameters.AddWithValue("@FC", ""); } else { da.Parameters.AddWithValue("@FC", fe.FC); }
                                if (fe.PA == null) { da.Parameters.AddWithValue("@PA", ""); } else { da.Parameters.AddWithValue("@PA", fe.PA); }
                                if (fe.FR == null) { da.Parameters.AddWithValue("@FR", ""); } else { da.Parameters.AddWithValue("@FR", fe.FR); }
                                da.Parameters.AddWithValue("@Tax", fe.Tax);
                                da.Parameters.AddWithValue("@Tanal", fe.Tanal);
                                if (fe.DescripcionProc == null) { da.Parameters.AddWithValue("@Descripcion", ""); } else { da.Parameters.AddWithValue("@Descripcion", fe.DescripcionProc); }
                                da.Parameters.AddWithValue("@CodTar", f.CodTar);
                                da.Parameters.AddWithValue("@item", fe.Item);
                                da.Parameters.AddWithValue("@Historia", fe.Historia);
                                da.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                if (fe.Observ == null) { da.Parameters.AddWithValue("@Observ", ""); } else { da.Parameters.AddWithValue("@Observ", fe.Observ); }
                                if (fe.ProximaProc == null) { da.Parameters.AddWithValue("@ProximaProc", ""); } else { da.Parameters.AddWithValue("@ProximaProc", fe.ProximaProc); }
                                da.Parameters.AddWithValue("@Asistente", "");
                                da.Parameters.AddWithValue("@FecRegAsist", "");
                                da.Parameters.AddWithValue("@CodMed", me.CodMed);
                                da.Parameters.AddWithValue("@FecRegMed", horaSis.HoraServidor.ToShortDateString());
                                da.Parameters.AddWithValue("@CodServ", me.CodServ);
                                if (fe.InterConsulta == null) { da.Parameters.AddWithValue("@InterConsulta", ""); } else { da.Parameters.AddWithValue("@InterConsulta", fe.InterConsulta.ToUpper()); }
                                da.Parameters.AddWithValue("@Estado", "");
                                da.Parameters.AddWithValue("@Crea", Crea);
                                da.Parameters.AddWithValue("@Modifica", "");
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", 1);

                                int Resu = (int)da.ExecuteScalar();


                                using (SqlCommand dd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr))
                                {
                                    string turno = "";
                                    if (horaSis.HoraServidor.Hour <= 13)
                                    {
                                        turno = "MAÑANA";
                                    }
                                    else
                                    {
                                        turno = "TARDE";
                                    }
                                    dd.CommandType = CommandType.StoredProcedure;
                                    dd.Parameters.AddWithValue("@Procedencia", "");
                                    dd.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                    dd.Parameters.AddWithValue("@Item", fe.Item);
                                    dd.Parameters.AddWithValue("@Tarifa", "");
                                    dd.Parameters.AddWithValue("@CodProce", "");
                                    dd.Parameters.AddWithValue("@CodDetalleP", "");
                                    dd.Parameters.AddWithValue("@CodSede", "");
                                    dd.Parameters.AddWithValue("@Cantidad", 0);
                                    dd.Parameters.AddWithValue("@precioUni", 0);
                                    dd.Parameters.AddWithValue("@precio", 0);
                                    dd.Parameters.AddWithValue("@igv", 0);
                                    dd.Parameters.AddWithValue("@total", 0);
                                    dd.Parameters.AddWithValue("@EstDet", "");
                                    dd.Parameters.AddWithValue("@FechaAten", horaSis.HoraServidor.ToShortDateString());
                                    dd.Parameters.AddWithValue("@TurnoAten", turno);
                                    dd.Parameters.AddWithValue("@RegMedico", me.CodMed);
                                    dd.Parameters.AddWithValue("@MedicoEnvia", "");
                                    dd.Parameters.AddWithValue("@Crea", "");
                                    dd.Parameters.AddWithValue("@Modifica", Crea);
                                    dd.Parameters.AddWithValue("@Elimina", "");
                                    dd.Parameters.AddWithValue("@Evento", "3");
                                    dd.ExecuteNonQuery();

                                    int cuentaDiag = 0;
                                    foreach (E_Ficha_Electronica it in (List<E_Ficha_Electronica>)Session["diagnosticoProc"])
                                    {
                                        cuentaDiag++;

                                        using (SqlCommand av = new SqlCommand("Usp_Mantenimiento_Diagnostico", con, tr))
                                        {

                                            av.CommandType = CommandType.StoredProcedure;
                                            av.Parameters.AddWithValue("@Cie10", it.CIe10);
                                            av.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                            av.Parameters.AddWithValue("@Procedencia", 2);
                                            av.Parameters.AddWithValue("@FE", Resu);
                                            av.Parameters.AddWithValue("@Historia", fe.Historia);
                                            av.Parameters.AddWithValue("@Descripcion", it.Descripcion.ToUpper());
                                            av.Parameters.AddWithValue("@Estado", 1);
                                            av.Parameters.AddWithValue("@Medico", me.CodMed);
                                            av.Parameters.AddWithValue("@Servicio", me.CodServ);
                                            av.Parameters.AddWithValue("@Item", cuentaDiag);
                                            av.Parameters.AddWithValue("@tipoDiagnostico", it.tipoDiagnostico);
                                            av.Parameters.AddWithValue("@Crea", Crea);
                                            av.Parameters.AddWithValue("@Modifica", "");
                                            av.Parameters.AddWithValue("@Elimina", "");
                                            av.Parameters.AddWithValue("@Evento", "1");

                                            av.ExecuteNonQuery();

                                        }
                                    }

                                    cuentaDiag = 0;
                                    foreach (E_Ficha_Electronica it in (List<E_Ficha_Electronica>)Session["recetaProc"])
                                    {

                                        using (SqlCommand avcd = new SqlCommand("Usp_Mantenimiento_Recetas", con, tr))
                                        {
                                            cuentaDiag++;

                                            avcd.CommandType = CommandType.StoredProcedure;
                                            avcd.Parameters.AddWithValue("@CodFarmaco", "");
                                            avcd.Parameters.AddWithValue("@Descripcion", it.Descripcion);
                                            avcd.Parameters.AddWithValue("@FE", Resu);
                                            avcd.Parameters.AddWithValue("@Procedencia", 2);
                                            avcd.Parameters.AddWithValue("@Concentra", it.Concentra);
                                            avcd.Parameters.AddWithValue("@FormaFarmec", it.FormaFarmec);
                                            avcd.Parameters.AddWithValue("@Dosis", it.Dosis);
                                            avcd.Parameters.AddWithValue("@Frecuencia", it.Frecuencia);
                                            avcd.Parameters.AddWithValue("@ViaAdmin", it.ViaAdmin);
                                            avcd.Parameters.AddWithValue("@Duracion", it.Duracion);
                                            avcd.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                            avcd.Parameters.AddWithValue("@Cantidad", it.Cantidad);
                                            avcd.Parameters.AddWithValue("@Estado", 1);
                                            avcd.Parameters.AddWithValue("@CodMed", me.CodMed);
                                            avcd.Parameters.AddWithValue("@Servicio", me.CodServ);
                                            if (it.Obsv == null)
                                            {
                                                avcd.Parameters.AddWithValue("@Obsv", "");
                                            }
                                            else
                                            {
                                                avcd.Parameters.AddWithValue("@Obsv", it.Obsv);
                                            }
                                            avcd.Parameters.AddWithValue("@Item", cuentaDiag);
                                            avcd.Parameters.AddWithValue("@Crea", Crea);
                                            avcd.Parameters.AddWithValue("@Modifica", "");
                                            avcd.Parameters.AddWithValue("@Elimina", "");
                                            avcd.Parameters.AddWithValue("@Evento", "1");

                                            avcd.ExecuteNonQuery();

                                        }
                                    }

                                }
                                tr.Commit();
                                ViewBag.mensaje = "Pedido registrado";

                            }
                            catch (Exception ex)
                            {
                                tr.Rollback();
                                return RedirectPermanent("../Master");
                            }
                            finally
                            {
                                con.Close();
                            }
                        }

                    }



                    if (Session["diagnosticoProc"] != null)
                    {
                        Session.Remove("diagnosticoProc");
                    }

                    if (Session["recetaProc"] != null)
                    {
                        Session.Remove("recetaProc");
                    }


                    return RedirectToAction("BandejaDeAtenciones");
                }

                else
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                        using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_Procedimientos", con, tr))
                        {
                            try
                            {
                                da.CommandType = CommandType.StoredProcedure;
                                da.Parameters.AddWithValue("@idProc", fe.idProc);
                                if (fe.FC == null) { da.Parameters.AddWithValue("@FC", ""); } else { da.Parameters.AddWithValue("@FC", fe.FC); }
                                if (fe.PA == null) { da.Parameters.AddWithValue("@PA", ""); } else { da.Parameters.AddWithValue("@PA", fe.PA); }
                                if (fe.FR == null) { da.Parameters.AddWithValue("@FR", ""); } else { da.Parameters.AddWithValue("@FR", fe.FR); }
                                da.Parameters.AddWithValue("@Tax", fe.Tax);
                                da.Parameters.AddWithValue("@Tanal", fe.Tanal);
                                if (fe.DescripcionProc == null) { da.Parameters.AddWithValue("@Descripcion", ""); } else { da.Parameters.AddWithValue("@Descripcion", fe.DescripcionProc); }
                                da.Parameters.AddWithValue("@CodTar", f.CodTar);
                                da.Parameters.AddWithValue("@item", fe.Item);
                                da.Parameters.AddWithValue("@Historia", fe.Historia);
                                da.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                if (fe.Observ == null) { da.Parameters.AddWithValue("@Observ", ""); } else { da.Parameters.AddWithValue("@Observ", fe.Observ); }
                                if (fe.ProximaProc == null) { da.Parameters.AddWithValue("@ProximaProc", ""); } else { da.Parameters.AddWithValue("@ProximaProc", fe.ProximaProc); }
                                da.Parameters.AddWithValue("@Asistente", fe.Asistente);
                                da.Parameters.AddWithValue("@FecRegAsist", fe.FecRegAsist);
                                da.Parameters.AddWithValue("@CodMed", me.CodMed);
                                da.Parameters.AddWithValue("@FecRegMed", horaSis.HoraServidor.ToShortDateString());
                                da.Parameters.AddWithValue("@CodServ", me.CodServ);
                                if (fe.InterConsulta == null) { da.Parameters.AddWithValue("@InterConsulta", ""); } else { da.Parameters.AddWithValue("@InterConsulta", fe.InterConsulta); }
                                da.Parameters.AddWithValue("@Estado", "");
                                da.Parameters.AddWithValue("@Crea", Crea);
                                da.Parameters.AddWithValue("@Modifica", "");
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", 2);
                                da.ExecuteNonQuery();


                                using (SqlCommand dd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr))
                                {
                                    string turno = "";
                                    if (horaSis.HoraServidor.Hour <= 13)
                                    {
                                        turno = "MAÑANA";
                                    }
                                    else
                                    {
                                        turno = "TARDE";
                                    }
                                    dd.CommandType = CommandType.StoredProcedure;
                                    dd.Parameters.AddWithValue("@Procedencia", "");
                                    dd.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                    dd.Parameters.AddWithValue("@Item", fe.Item);
                                    dd.Parameters.AddWithValue("@Tarifa", "");
                                    dd.Parameters.AddWithValue("@CodProce", "");
                                    dd.Parameters.AddWithValue("@CodDetalleP", "");
                                    dd.Parameters.AddWithValue("@CodSede", "");
                                    dd.Parameters.AddWithValue("@Cantidad", 0);
                                    dd.Parameters.AddWithValue("@precioUni", 0);
                                    dd.Parameters.AddWithValue("@precio", 0);
                                    dd.Parameters.AddWithValue("@igv", 0);
                                    dd.Parameters.AddWithValue("@total", 0);
                                    dd.Parameters.AddWithValue("@EstDet", "");
                                    dd.Parameters.AddWithValue("@FechaAten", horaSis.HoraServidor.ToShortDateString());
                                    dd.Parameters.AddWithValue("@TurnoAten", turno);
                                    dd.Parameters.AddWithValue("@RegMedico", me.CodMed);
                                    dd.Parameters.AddWithValue("@MedicoEnvia", "");
                                    dd.Parameters.AddWithValue("@Crea", "");
                                    dd.Parameters.AddWithValue("@Modifica", Crea);
                                    dd.Parameters.AddWithValue("@Elimina", "");
                                    dd.Parameters.AddWithValue("@Evento", "3");
                                    dd.ExecuteNonQuery();

                                    int cuentaDiag = 0;
                                    foreach (E_Ficha_Electronica it in (List<E_Ficha_Electronica>)Session["diagnosticoProc"])
                                    {
                                        cuentaDiag++;

                                        using (SqlCommand av = new SqlCommand("Usp_Mantenimiento_Diagnostico", con, tr))
                                        {

                                            av.CommandType = CommandType.StoredProcedure;
                                            av.Parameters.AddWithValue("@Cie10", it.CIe10);
                                            av.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                            av.Parameters.AddWithValue("@Procedencia", 2);
                                            av.Parameters.AddWithValue("@FE", fe.idProc);
                                            av.Parameters.AddWithValue("@Historia", fe.Historia);
                                            av.Parameters.AddWithValue("@Descripcion", it.Descripcion);
                                            av.Parameters.AddWithValue("@Estado", 1);
                                            av.Parameters.AddWithValue("@Medico", me.CodMed);
                                            av.Parameters.AddWithValue("@Servicio", me.CodServ);
                                            av.Parameters.AddWithValue("@Item", cuentaDiag);
                                            av.Parameters.AddWithValue("@tipoDiagnostico", it.tipoDiagnostico);
                                            av.Parameters.AddWithValue("@Crea", Crea);
                                            av.Parameters.AddWithValue("@Modifica", "");
                                            av.Parameters.AddWithValue("@Elimina", "");
                                            av.Parameters.AddWithValue("@Evento", "1");

                                            av.ExecuteNonQuery();

                                        }
                                    }


                                    foreach (E_Ficha_Electronica it in (List<E_Ficha_Electronica>)Session["recetaProc"])
                                    {

                                        using (SqlCommand avcd = new SqlCommand("Usp_Mantenimiento_Recetas", con, tr))
                                        {
                                            cuentaDiag++;

                                            avcd.CommandType = CommandType.StoredProcedure;
                                            avcd.Parameters.AddWithValue("@CodFarmaco", "");
                                            avcd.Parameters.AddWithValue("@Descripcion", it.Descripcion);
                                            avcd.Parameters.AddWithValue("@FE", fe.idProc);
                                            avcd.Parameters.AddWithValue("@Procedencia", 2);
                                            avcd.Parameters.AddWithValue("@Concentra", it.Concentra);
                                            avcd.Parameters.AddWithValue("@FormaFarmec", it.FormaFarmec);
                                            avcd.Parameters.AddWithValue("@Dosis", it.Dosis);
                                            avcd.Parameters.AddWithValue("@Frecuencia", it.Frecuencia);
                                            avcd.Parameters.AddWithValue("@ViaAdmin", it.ViaAdmin);
                                            avcd.Parameters.AddWithValue("@Duracion", it.Duracion);
                                            avcd.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                            avcd.Parameters.AddWithValue("@Cantidad", it.Cantidad);
                                            avcd.Parameters.AddWithValue("@Estado", 1);
                                            avcd.Parameters.AddWithValue("@CodMed", me.CodMed);
                                            avcd.Parameters.AddWithValue("@Servicio", me.CodServ);
                                            if (it.Obsv == null)
                                            {
                                                avcd.Parameters.AddWithValue("@Obsv", "");
                                            }
                                            else
                                            {
                                                avcd.Parameters.AddWithValue("@Obsv", it.Obsv);
                                            }
                                            avcd.Parameters.AddWithValue("@Item", cuentaDiag);
                                            avcd.Parameters.AddWithValue("@Crea", Crea);
                                            avcd.Parameters.AddWithValue("@Modifica", "");
                                            avcd.Parameters.AddWithValue("@Elimina", "");
                                            avcd.Parameters.AddWithValue("@Evento", "1");

                                            avcd.ExecuteNonQuery();

                                        }
                                    }

                                }
                                tr.Commit();
                                ViewBag.mensaje = "Pedido registrado";

                            }
                            catch (Exception ex)
                            {
                                tr.Rollback();
                                return RedirectPermanent("../Master");
                            }
                            finally
                            {
                                con.Close();
                            }
                        }

                    }



                    if (Session["diagnosticoProc"] != null)
                    {
                        Session.Remove("diagnosticoProc");
                    }

                    if (Session["recetaProc"] != null)
                    {
                        Session.Remove("recetaProc");
                    }


                    return RedirectToAction("BandejaDeAtenciones");

                }

            }

            if (fe.evento == 2)
            {
                E_Ficha_Electronica fie = ListadoPerfilesFichaElectronica().Find(x => x.idPFA == fe.idPFA);
                ViewBag.contenido = fie.contenido;
                return View(fe);
            }

            else if (fe.evento == 3)
            {
                ViewBag.tab = "2";
                var formularios = (List<E_Ficha_Electronica>)Session["diagnosticoProc"];
                E_Ficha_Electronica item = new E_Ficha_Electronica();
                E_Ficha_Electronica BuscaCie10 = listCie10().Find(x => x.CIe10 == fe.CIe10);

                item.CIe10 = fe.CIe10;
                item.Descripcion = BuscaCie10.Descripcion;
                item.tipoDiagnostico = fe.tipoDiagnostico;
                formularios.Add(item);
                Session["diagnosticoProc"] = formularios;

            }
            else if (fe.evento == 4)
            {
                ViewBag.tab = "2";
                var formularios = (List<E_Ficha_Electronica>)Session["diagnosticoProc"];
                var registro = formularios.Where(x => x.CIe10.Equals(fe.EliminaDiagnostico)).FirstOrDefault();
                formularios.Remove(registro);
                Session["diagnosticoProc"] = formularios;

            }
            else if (fe.evento == 5)
            {
                ViewBag.tab = "3";
                var formularios = (List<E_Ficha_Electronica>)Session["recetaProc"];
                E_Ficha_Electronica item = new E_Ficha_Electronica();
                item.idRecetaSesion = formularios.Count();
                item.Descripcion = fe.Descripcion.ToUpper();
                item.Concentra = fe.Concentra.ToUpper();
                item.FormaFarmec = fe.FormaFarmec;
                item.Dosis = fe.Dosis.ToUpper();
                item.Frecuencia = fe.Frecuencia;
                item.ViaAdmin = fe.ViaAdmin.ToUpper();
                item.Duracion = fe.Duracion.ToUpper();
                item.Cantidad = fe.Cantidad;
                if (fe.Obsv == null)
                {
                    item.Obsv = "";
                }
                else
                {
                    item.Obsv = fe.Obsv.ToUpper();
                }
                ViewBag.borraDatos = 1;
                formularios.Add(item);
                Session["recetaProc"] = formularios;
            }

            else if (fe.evento == 6)
            {
                ViewBag.tab = "3";
                var formularios = (List<E_Ficha_Electronica>)Session["recetaProc"];
                var registro = formularios.Where(x => x.idRecetaSesion == fe.idRecetaSesion).FirstOrDefault();
                formularios.Remove(registro);
                Session["recetaProc"] = formularios;

            }

            ViewBag.Listadiagnostico = (List<E_Ficha_Electronica>)Session["diagnosticoProc"];
            ViewBag.ListaRecetas = (List<E_Ficha_Electronica>)Session["recetaProc"];

            return View(fe);
        }




        public ActionResult RegistroDeExAuxiliares(int CodCue, int Item, int Historia, int? FE = null)
        {
            if (FE != null)
            {
                string sede = Session["codSede"].ToString();
                TarifarioController ta = new TarifarioController();
                string usuario = Session["UserID"].ToString();
                E_Medico me = listaMedicoUsuario(usuario).FirstOrDefault();
                E_Ficha_Electronica f = ListaCuentaDetalleFichaE(CodCue, Item).FirstOrDefault();
                PacientesController pa = new PacientesController();
                E_Pacientes pac = pa.ListadoPacientes().Find(x => x.Historia == Historia);
                E_Tarifario tar = ta.ListadoTarifa().Find(x => x.CodTar == f.CodTar);
                E_Tarifario per = ta.ListadoPerfilesFichaElectronica().Find(x => x.idPFA == tar.idPFA);
                int edad = DateTime.Today.AddTicks(-f.FechaNac.Ticks).Year - 1;
                ViewBag.perfil = new SelectList(ta.ListadoPerfilesFichaElectronica().Where(x => x.CodEspec == f.CodEspec || x.CodEspec == "ES004"), "idPFA", "Nombre");
                ViewBag.CodPerf = tar.idPFA;
                ViewBag.tab = "1";
                ViewBag.sexo = pac.CodSexo;
                ViewBag.cuenta = CodCue;
                ViewBag.item = Item;
                ViewBag.historia = Historia;
                ViewBag.idAux = FE;
                ViewBag.nombrePac = f.Paciente;
                ViewBag.edad = edad;
                ViewBag.servicio = f.NomServ;
                ViewBag.tarifa = f.DescTar;
                ViewBag.CodServMed = me.CodServ;


                int idauxi = (int)FE;
                var lista = (from x in ListaModuloExamenAuxiliar(idauxi) select x).FirstOrDefault();
                ViewBag.Asistente = lista.Asistente;
                ViewBag.FecRegAsist = lista.FecRegAsist;

                return View(lista);

            }

            else
            {

                string sede = Session["codSede"].ToString();
                TarifarioController ta = new TarifarioController();
                string usuario = Session["UserID"].ToString();
                E_Medico me = listaMedicoUsuario(usuario).FirstOrDefault();
                E_Ficha_Electronica f = ListaCuentaDetalleFichaE(CodCue, Item).FirstOrDefault();
                PacientesController pa = new PacientesController();
                E_Pacientes pac = pa.ListadoPacientes().Find(x => x.Historia == Historia);
                E_Tarifario tar = ta.ListadoTarifa().Find(x => x.CodTar == f.CodTar);
                E_Tarifario per = ta.ListadoPerfilesFichaElectronica().Find(x => x.idPFA == tar.idPFA);
                int edad = DateTime.Today.AddTicks(-f.FechaNac.Ticks).Year - 1;
                ViewBag.perfil = new SelectList(ta.ListadoPerfilesFichaElectronica().Where(x => x.CodEspec == f.CodEspec || x.CodEspec == "ES004"), "idPFA", "Nombre");
                ViewBag.CodPerf = tar.idPFA;
                ViewBag.tab = "1";
                ViewBag.sexo = pac.CodSexo;
                ViewBag.cuenta = CodCue;
                ViewBag.item = Item;
                ViewBag.historia = Historia;
                ViewBag.idAux = -1;
                ViewBag.nombrePac = f.Paciente;
                ViewBag.edad = edad;
                ViewBag.servicio = f.NomServ;
                ViewBag.tarifa = f.DescTar;
                ViewBag.contenido = per.contenido;
                ViewBag.CodServMed = me.CodServ;

                return View();
            }
        }

        


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult RegistroDeExAuxiliares(E_Ficha_Electronica fe)
        {
            string sede = Session["codSede"].ToString();
            PacientesController pa = new PacientesController();
            TarifarioController ta = new TarifarioController();
            E_Pacientes pac = pa.ListadoPacientes().Find(x => x.Historia == fe.Historia);
            EspecialidadController e = new EspecialidadController();
            string usuario = Session["UserID"].ToString();
            ViewBag.contenido = fe.DescripcionProc;
            E_Medico me = listaMedicoUsuario(usuario).FirstOrDefault();
            ViewBag.antecedentes = (List<E_Ficha_Electronica>)AntecedentexUsuario(fe.Historia);
            ViewBag.listaEspe = (List<E_Especialidades>)e.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true).ToList();
            ViewBag.listaEspecialidad = new SelectList(e.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true), "CodEspec", "NomEspec");
            RecetasController rec = new RecetasController();
            ViewBag.listaFormaFarm = new SelectList(rec.ListadoFormaFarmaceutica().Where(x => x.Estado == true), "idFormFarm", "Descripcion", fe.FormaFarmec);
            ViewBag.listaFrecuencia = new SelectList(rec.ListadoFrecuenciaRecetas().Where(x => x.Estado == true), "idFrec", "Descripcion", fe.Frecuencia);
            ViewBag.listaCie10 = new SelectList(listCie10(), "CIe10", "ConcatCie10");
            E_Ficha_Electronica f = ListaCuentaDetalleFichaE(fe.CodCue, fe.Item).FirstOrDefault();
            int edad = DateTime.Today.AddTicks(-f.FechaNac.Ticks).Year - 1;
            ViewBag.tab = "1";
            ViewBag.perfil = new SelectList(ta.ListadoPerfilesFichaElectronica().Where(x => x.CodEspec == f.CodEspec || x.CodEspec == "ES004"), "idPFA", "Nombre");
            ViewBag.CodPerf = fe.idPFA;
            ViewBag.Asistente = fe.Asistente;
            ViewBag.FecRegAsist = fe.FecRegAsist;
            ViewBag.sexo = pac.CodSexo;
            ViewBag.cuenta = fe.CodCue;
            ViewBag.item = fe.Item;
            ViewBag.idAux = fe.idExAux;
            ViewBag.historia = fe.Historia;
            ViewBag.nombrePac = f.Paciente;
            ViewBag.edad = edad;
            ViewBag.servicio = f.NomServ;
            ViewBag.tarifa = f.DescTar;
            
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();
            string Crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            if (fe.evento == 1)
            {
                if (fe.idExAux == -1)
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                        using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_ExamenAuxiliar", con, tr))
                        {
                            try
                            {
                                da.CommandType = CommandType.StoredProcedure;
                                da.Parameters.AddWithValue("@idExAux", "");
                                if (fe.FC == null) { da.Parameters.AddWithValue("@FC", ""); } else { da.Parameters.AddWithValue("@FC", fe.FC); }
                                if (fe.PA == null) { da.Parameters.AddWithValue("@PA", ""); } else { da.Parameters.AddWithValue("@PA", fe.PA); }
                                if (fe.FR == null) { da.Parameters.AddWithValue("@FR", ""); } else { da.Parameters.AddWithValue("@FR", fe.FR); }
                                da.Parameters.AddWithValue("@Tax", fe.Tax);
                                da.Parameters.AddWithValue("@Tanal", fe.Tanal);
                                if (fe.DescripcionProc == null) { da.Parameters.AddWithValue("@Descripcion", ""); } else { da.Parameters.AddWithValue("@Descripcion", fe.DescripcionProc); }
                                da.Parameters.AddWithValue("@CodTar", f.CodTar);
                                da.Parameters.AddWithValue("@item", fe.Item);
                                da.Parameters.AddWithValue("@Historia", fe.Historia);
                                da.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                if (fe.Observ == null) { da.Parameters.AddWithValue("@Observ", ""); } else { da.Parameters.AddWithValue("@Observ", fe.Observ); }
                                if (fe.ProximaProc == null) { da.Parameters.AddWithValue("@ProximaProc", ""); } else { da.Parameters.AddWithValue("@ProximaProc", fe.ProximaProc); }
                                da.Parameters.AddWithValue("@Asistente", "");
                                da.Parameters.AddWithValue("@FecRegAsist", "");
                                da.Parameters.AddWithValue("@CodMed", me.CodMed);
                                da.Parameters.AddWithValue("@FecRegMed", horaSis.HoraServidor.ToShortDateString());
                                da.Parameters.AddWithValue("@CodServ", me.CodServ);
                                if (fe.InterConsulta == null) { da.Parameters.AddWithValue("@InterConsulta", ""); } else { da.Parameters.AddWithValue("@InterConsulta", fe.InterConsulta.ToUpper()); }
                                da.Parameters.AddWithValue("@Estado", "");
                                da.Parameters.AddWithValue("@Crea", Crea);
                                da.Parameters.AddWithValue("@Modifica", "");
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", 1);

                                int Resu = (int)da.ExecuteScalar();


                                using (SqlCommand dd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr))
                                {
                                    string turno = "";
                                    if (horaSis.HoraServidor.Hour <= 13)
                                    {
                                        turno = "MAÑANA";
                                    }
                                    else
                                    {
                                        turno = "TARDE";
                                    }
                                    dd.CommandType = CommandType.StoredProcedure;
                                    dd.Parameters.AddWithValue("@Procedencia", "");
                                    dd.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                    dd.Parameters.AddWithValue("@Item", fe.Item);
                                    dd.Parameters.AddWithValue("@Tarifa", "");
                                    dd.Parameters.AddWithValue("@CodProce", "");
                                    dd.Parameters.AddWithValue("@CodDetalleP", "");
                                    dd.Parameters.AddWithValue("@CodSede", "");
                                    dd.Parameters.AddWithValue("@Cantidad", 0);
                                    dd.Parameters.AddWithValue("@precioUni", 0);
                                    dd.Parameters.AddWithValue("@precio", 0);
                                    dd.Parameters.AddWithValue("@igv", 0);
                                    dd.Parameters.AddWithValue("@total", 0);
                                    dd.Parameters.AddWithValue("@EstDet", "");
                                    dd.Parameters.AddWithValue("@FechaAten", horaSis.HoraServidor.ToShortDateString());
                                    dd.Parameters.AddWithValue("@TurnoAten", turno);
                                    dd.Parameters.AddWithValue("@RegMedico", me.CodMed);
                                    dd.Parameters.AddWithValue("@MedicoEnvia", "");
                                    dd.Parameters.AddWithValue("@Crea", "");
                                    dd.Parameters.AddWithValue("@Modifica", Crea);
                                    dd.Parameters.AddWithValue("@Elimina", "");
                                    dd.Parameters.AddWithValue("@Evento", "3");
                                    dd.ExecuteNonQuery();
                                                                      

                                }
                                tr.Commit();
                                ViewBag.mensaje = "Pedido registrado";

                            }
                            catch (Exception ex)
                            {
                                tr.Rollback();
                                return RedirectPermanent("../Master");
                            }
                            finally
                            {
                                con.Close();
                            }
                        }

                    }


                    return RedirectToAction("BandejaDeAtenciones");
                }
                
                else
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                    {
                        con.Open();
                        SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                        using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_ExamenAuxiliar", con, tr))
                        {
                            try
                            {
                                da.CommandType = CommandType.StoredProcedure;
                                da.Parameters.AddWithValue("@idExAux", fe.idProc);
                                if (fe.FC == null) { da.Parameters.AddWithValue("@FC", ""); } else { da.Parameters.AddWithValue("@FC", fe.FC); }
                                if (fe.PA == null) { da.Parameters.AddWithValue("@PA", ""); } else { da.Parameters.AddWithValue("@PA", fe.PA); }
                                if (fe.FR == null) { da.Parameters.AddWithValue("@FR", ""); } else { da.Parameters.AddWithValue("@FR", fe.FR); }
                                da.Parameters.AddWithValue("@Tax", fe.Tax);
                                da.Parameters.AddWithValue("@Tanal", fe.Tanal);
                                if (fe.DescripcionProc == null) { da.Parameters.AddWithValue("@Descripcion", ""); } else { da.Parameters.AddWithValue("@Descripcion", fe.DescripcionProc); }
                                da.Parameters.AddWithValue("@CodTar", f.CodTar);
                                da.Parameters.AddWithValue("@item", fe.Item);
                                da.Parameters.AddWithValue("@Historia", fe.Historia);
                                da.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                if (fe.Observ == null) { da.Parameters.AddWithValue("@Observ", ""); } else { da.Parameters.AddWithValue("@Observ", fe.Observ); }
                                if (fe.ProximaProc == null) { da.Parameters.AddWithValue("@ProximaProc", ""); } else { da.Parameters.AddWithValue("@ProximaProc", fe.ProximaProc); }
                                da.Parameters.AddWithValue("@Asistente", fe.Asistente);
                                da.Parameters.AddWithValue("@FecRegAsist", fe.FecRegAsist);
                                da.Parameters.AddWithValue("@CodMed", me.CodMed);
                                da.Parameters.AddWithValue("@FecRegMed", horaSis.HoraServidor.ToShortDateString());
                                da.Parameters.AddWithValue("@CodServ", me.CodServ);
                                if (fe.InterConsulta == null) { da.Parameters.AddWithValue("@InterConsulta", ""); } else { da.Parameters.AddWithValue("@InterConsulta", fe.InterConsulta); }
                                da.Parameters.AddWithValue("@Estado", "");
                                da.Parameters.AddWithValue("@Crea", Crea);
                                da.Parameters.AddWithValue("@Modifica", "");
                                da.Parameters.AddWithValue("@Elimina", "");
                                da.Parameters.AddWithValue("@Evento", 2);
                                da.ExecuteNonQuery();


                                using (SqlCommand dd = new SqlCommand("Usp_MtoCuentasDetalle", con, tr))
                                {
                                    string turno = "";
                                    if (horaSis.HoraServidor.Hour <= 13)
                                    {
                                        turno = "MAÑANA";
                                    }
                                    else
                                    {
                                        turno = "TARDE";
                                    }
                                    dd.CommandType = CommandType.StoredProcedure;
                                    dd.Parameters.AddWithValue("@Procedencia", "");
                                    dd.Parameters.AddWithValue("@CodCue", fe.CodCue);
                                    dd.Parameters.AddWithValue("@Item", fe.Item);
                                    dd.Parameters.AddWithValue("@Tarifa", "");
                                    dd.Parameters.AddWithValue("@CodProce", "");
                                    dd.Parameters.AddWithValue("@CodDetalleP", "");
                                    dd.Parameters.AddWithValue("@CodSede", "");
                                    dd.Parameters.AddWithValue("@Cantidad", 0);
                                    dd.Parameters.AddWithValue("@precioUni", 0);
                                    dd.Parameters.AddWithValue("@precio", 0);
                                    dd.Parameters.AddWithValue("@igv", 0);
                                    dd.Parameters.AddWithValue("@total", 0);
                                    dd.Parameters.AddWithValue("@EstDet", "");
                                    dd.Parameters.AddWithValue("@FechaAten", horaSis.HoraServidor.ToShortDateString());
                                    dd.Parameters.AddWithValue("@TurnoAten", turno);
                                    dd.Parameters.AddWithValue("@RegMedico", me.CodMed);
                                    dd.Parameters.AddWithValue("@MedicoEnvia", "");
                                    dd.Parameters.AddWithValue("@Crea", "");
                                    dd.Parameters.AddWithValue("@Modifica", Crea);
                                    dd.Parameters.AddWithValue("@Elimina", "");
                                    dd.Parameters.AddWithValue("@Evento", "3");
                                    dd.ExecuteNonQuery();
                                    
                                }
                                tr.Commit();
                                ViewBag.mensaje = "Pedido registrado";

                            }
                            catch (Exception ex)
                             {
                                tr.Rollback();
                                return RedirectPermanent("../Master");
                            }
                            finally
                            {
                                con.Close();
                            }
                        }

                    }
                    

                    return RedirectToAction("BandejaDeAtenciones");

                }

            }

            if (fe.evento == 2)
            {
                E_Ficha_Electronica fie = ListadoPerfilesFichaElectronica().Find(x => x.idPFA == fe.idPFA);
                ViewBag.contenido = fie.contenido;
                return View(fe);
            }

            
            return View(fe);
        }


        public ActionResult TraerDatos(int Historia, string CodServ)
        {

            var evalua = (List<E_Ficha_Electronica>)ListaFichaElectronica(Historia, CodServ).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
        }


        public ActionResult PerfilFichaElectronica(int? idPFA = null)
        {
            string sede = Session["codSede"].ToString();
            EspecialidadController e = new EspecialidadController();
            TarifarioController t = new TarifarioController();
            ViewBag.listaEspecialidad = new SelectList(e.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true), "CodEspec", "NomEspec");

            ViewBag.lista = (List<E_Tarifario>)t.ListadoPerfilesFichaElectronica().ToList();

            if (idPFA != null)
            {
                ViewBag.idPFA = idPFA;
                ViewBag.modificar = "2";

                var lista = (from x in ListadoPerfilesFichaElectronica() where x.idPFA == idPFA select x).FirstOrDefault();

                return View(lista);
            }
            else
            {
                ViewBag.idPFA = "";
                ViewBag.modificar = "1";
                return View();
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PerfilFichaElectronica(E_Ficha_Electronica f)
        {
            string sede = Session["codSede"].ToString();
            EspecialidadController e = new EspecialidadController();
            TarifarioController t = new TarifarioController();
            ViewBag.listaEspecialidad = new SelectList(e.ListadoEspecialidades().Where(x => x.CodSed == sede && x.EstEspec == true), "CodEspec", "NomEspec");

            ViewBag.lista = (List<E_Tarifario>)t.ListadoPerfilesFichaElectronica().ToList();

            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();

            string crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            if (f.evento == 1)
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                    con.Open();
                    using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_PerfilFichaElectronica", con))
                    {
                        try
                        {
                            da.CommandType = CommandType.StoredProcedure;
                            da.Parameters.AddWithValue("@idPFA", "");
                            da.Parameters.AddWithValue("@Nombre", f.Nombre.ToUpper());
                            da.Parameters.AddWithValue("@contenido", f.contenido);
                            da.Parameters.AddWithValue("@CodEspec", f.CodEspec);
                            da.Parameters.AddWithValue("@estado", f.Estado);
                            da.Parameters.AddWithValue("@Crea", crea);
                            da.Parameters.AddWithValue("@Modifica", "");
                            da.Parameters.AddWithValue("@Elimina", "");
                            da.Parameters.AddWithValue("@Evento", "1");

                            da.ExecuteNonQuery();


                            ViewBag.mensaje = "Pedido registrado";

                        }
                        catch (Exception ex)
                        {
                            ViewBag.mensaje = ex.Message;
                            return View(f);
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }

                return RedirectToAction("PerfilFichaElectronica");

            }
            else if (f.evento == 2)
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                    con.Open();
                    using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_PerfilFichaElectronica", con))
                    {
                        try
                        {
                            da.CommandType = CommandType.StoredProcedure;
                            da.Parameters.AddWithValue("@idPFA", f.idPFA);
                            da.Parameters.AddWithValue("@Nombre", f.Nombre.ToUpper());
                            da.Parameters.AddWithValue("@contenido", f.contenido);
                            da.Parameters.AddWithValue("@CodEspec", f.CodEspec);
                            da.Parameters.AddWithValue("@estado", f.Estado);
                            da.Parameters.AddWithValue("@Crea", "");
                            da.Parameters.AddWithValue("@Modifica", crea);
                            da.Parameters.AddWithValue("@Elimina", "");
                            da.Parameters.AddWithValue("@Evento", "2");

                            da.ExecuteNonQuery();


                            ViewBag.mensaje = "Pedido registrado";

                        }
                        catch (Exception ex)
                        {
                            ViewBag.mensaje = ex.Message;
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }

                return RedirectToAction("PerfilFichaElectronica");

            }
            else if (f.evento == 3)
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                    con.Open();
                    using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_PerfilFichaElectronica", con))
                    {
                        try
                        {
                            da.CommandType = CommandType.StoredProcedure;
                            da.Parameters.AddWithValue("@idPFA", f.EliminaPerfil);
                            da.Parameters.AddWithValue("@Nombre", "");
                            da.Parameters.AddWithValue("@contenido", "");
                            da.Parameters.AddWithValue("@CodEspec", "");
                            da.Parameters.AddWithValue("@estado", 0);
                            da.Parameters.AddWithValue("@Crea", "");
                            da.Parameters.AddWithValue("@Modifica", "");
                            da.Parameters.AddWithValue("@Elimina", crea);
                            da.Parameters.AddWithValue("@Evento", "3");

                            da.ExecuteNonQuery();


                            ViewBag.mensaje = "Pedido registrado";

                        }
                        catch (Exception ex)
                        {
                            ViewBag.mensaje = ex.Message;
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }

                return RedirectToAction("PerfilFichaElectronica");

            }
            else if (f.evento == 4)
            {
                return RedirectPermanent("~/FichaElectronica/PerfilFichaElectronica?idPFA=" + f.EditaPerfil);
            }
            else
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                    con.Open();
                    using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_PerfilFichaElectronica", con))
                    {
                        try
                        {
                            da.CommandType = CommandType.StoredProcedure;
                            da.Parameters.AddWithValue("@idPFA", f.EliminaPerfil);
                            da.Parameters.AddWithValue("@Nombre", "");
                            da.Parameters.AddWithValue("@contenido", "");
                            da.Parameters.AddWithValue("@CodEspec", "");
                            da.Parameters.AddWithValue("@estado", 1);
                            da.Parameters.AddWithValue("@Crea", "");
                            da.Parameters.AddWithValue("@Modifica", crea);
                            da.Parameters.AddWithValue("@Elimina", "");
                            da.Parameters.AddWithValue("@Evento", "4");

                            da.ExecuteNonQuery();

                            ViewBag.mensaje = "Pedido registrado";

                        }
                        catch (Exception ex)
                        {
                            ViewBag.mensaje = ex.Message;
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }
                return RedirectToAction("PerfilFichaElectronica");
            }

        }

        public ActionResult PerfilConsentimiento(int? idPFA = null)
        {
            ViewBag.lista = (List<E_Ficha_Electronica>)ListadoPerfilesConsentimiento().ToList();

            if (idPFA != null)
            {
                ViewBag.idPFA = idPFA;
                ViewBag.modificar = "2";

                var lista = (from x in ListadoPerfilesConsentimiento() where x.idPFA == idPFA select x).FirstOrDefault();

                return View(lista);
            }
            else
            {
                ViewBag.idPFA = "";
                ViewBag.modificar = "1";
                return View();
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PerfilConsentimiento(E_Ficha_Electronica f)
        {
            ViewBag.lista = (List<E_Ficha_Electronica>)ListadoPerfilesConsentimiento().ToList();
            UtilitarioController ut = new UtilitarioController();
            var horaSis = (from x in ut.ListadoHoraServidor() select x).FirstOrDefault();

            string crea = Session["usuario"] + " " + horaSis.HoraServidor.ToString() + " " + Environment.MachineName;

            if (f.evento == 1)
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                    con.Open();
                    using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_PerfilConsentimiento", con))
                    {
                        try
                        {
                            da.CommandType = CommandType.StoredProcedure;
                            da.Parameters.AddWithValue("@id", "");
                            da.Parameters.AddWithValue("@titulo", f.Nombre.ToUpper());
                            da.Parameters.AddWithValue("@contenido", f.contenido);
                            da.Parameters.AddWithValue("@estado", f.Estado);
                            da.Parameters.AddWithValue("@Crea", crea);
                            da.Parameters.AddWithValue("@Modifica", "");
                            da.Parameters.AddWithValue("@Elimina", "");
                            da.Parameters.AddWithValue("@Evento", "1");

                            da.ExecuteNonQuery();


                            ViewBag.mensaje = "Pedido registrado";

                        }
                        catch (Exception ex)
                        {
                            ViewBag.mensaje = ex.Message;
                            return View(f);
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }

                return RedirectToAction("PerfilConsentimiento");

            }
            else if (f.evento == 2)
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                    con.Open();
                    using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_PerfilConsentimiento", con))
                    {
                        try
                        {
                            da.CommandType = CommandType.StoredProcedure;
                            da.Parameters.AddWithValue("@id", f.idPFA);
                            da.Parameters.AddWithValue("@titulo", f.Nombre.ToUpper());
                            da.Parameters.AddWithValue("@contenido", f.contenido);
                            da.Parameters.AddWithValue("@estado", f.Estado);
                            da.Parameters.AddWithValue("@Crea", "");
                            da.Parameters.AddWithValue("@Modifica", crea);
                            da.Parameters.AddWithValue("@Elimina", "");
                            da.Parameters.AddWithValue("@Evento", "2");
                            

                            da.ExecuteNonQuery();


                            ViewBag.mensaje = "Pedido registrado";

                        }
                        catch (Exception ex)
                        {
                            ViewBag.mensaje = ex.Message;
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }

                return RedirectToAction("PerfilConsentimiento");

            }
            else if (f.evento == 3)
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                    con.Open();
                    using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_PerfilConsentimiento", con))
                    {
                        try
                        {
                            da.CommandType = CommandType.StoredProcedure;
                            da.Parameters.AddWithValue("@id", f.EliminaPerfil);
                            da.Parameters.AddWithValue("@titulo", "");
                            da.Parameters.AddWithValue("@contenido", "");
                            da.Parameters.AddWithValue("@estado", 0);
                            da.Parameters.AddWithValue("@Crea", "");
                            da.Parameters.AddWithValue("@Modifica", "");
                            da.Parameters.AddWithValue("@Elimina", crea);
                            da.Parameters.AddWithValue("@Evento", "3");
                            
                            da.ExecuteNonQuery();


                            ViewBag.mensaje = "Pedido registrado";

                        }
                        catch (Exception ex)
                        {
                            ViewBag.mensaje = ex.Message;
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }

                return RedirectToAction("PerfilConsentimiento");

            }
            else if (f.evento == 4)
            {
                return RedirectPermanent("~/FichaElectronica/PerfilConsentimiento/?idPFA=" + f.EditaPerfil);
            }
            else
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString.ToString()))
                {
                    con.Open();
                    using (SqlCommand da = new SqlCommand("Usp_Mantenimiento_PerfilConsentimiento", con))
                    {
                        try
                        {
                            da.CommandType = CommandType.StoredProcedure;
                            da.Parameters.AddWithValue("@id", f.EliminaPerfil);
                            da.Parameters.AddWithValue("@titulo", "");
                            da.Parameters.AddWithValue("@contenido", "");
                            da.Parameters.AddWithValue("@estado", 1);
                            da.Parameters.AddWithValue("@Crea", "");
                            da.Parameters.AddWithValue("@Modifica", crea);
                            da.Parameters.AddWithValue("@Elimina", "");
                            da.Parameters.AddWithValue("@Evento", "4");

                            da.ExecuteNonQuery();

                            ViewBag.mensaje = "Pedido registrado";

                        }
                        catch (Exception ex)
                        {
                            ViewBag.mensaje = ex.Message;
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }

                return RedirectToAction("PerfilConsentimiento");

            }

        }

        public ActionResult ObtenerTarifa(string CodEsp)
        {
            string sede = Session["codSede"].ToString();
            TarifarioController t = new TarifarioController();
            var evalua = (List<E_Tarifario>)t.ListadoTarifa().Where(x => x.CodEspec == CodEsp && x.EstTar == true).ToList();
            if (evalua.Count() != 0)
            {
                return Json(evalua, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public List<E_Ficha_Electronica> ListaFichaElectronica(int Historia, string CodServ)
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaFichaElectronica", con))
                {
                    cmd.Parameters.AddWithValue("@Historia", Historia);
                    cmd.Parameters.AddWithValue("@CodServ", CodServ);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Ficha_Electronica Ser = new E_Ficha_Electronica();
                            Ser.FE = dr.GetInt32(0);
                            Ser.Historia = dr.GetInt32(1);
                            Ser.FC = dr.GetString(2);
                            Ser.PA = dr.GetString(3);
                            Ser.FR = dr.GetString(4);
                            Ser.Tax = dr.GetDecimal(5);
                            Ser.Tanal = dr.GetDecimal(6);
                            Ser.Peso = dr.GetDecimal(7);
                            Ser.talla = dr.GetDecimal(8);
                            Ser.IMC = dr.GetDecimal(9);
                            Ser.FUR = dr.GetString(10);
                            Ser.MotivoConsulta = dr.GetString(11);
                            Ser.Relato = dr.GetString(12);
                            Ser.TiempoEnfermedad = dr.GetString(13);
                            Ser.Inicio = dr.GetString(14);
                            Ser.Curso = dr.GetString(15);
                            Ser.sed = dr.GetString(16);
                            Ser.sueño = dr.GetString(17);
                            Ser.Orina = dr.GetString(18);
                            Ser.apetito = dr.GetString(19);
                            Ser.paridad = dr.GetInt32(20);
                            Ser.FPP = dr.GetString(21);
                            Ser.EdadGestacional = dr.GetInt32(22);
                            Ser.PIG = dr.GetString(23);
                            Ser.UltimoPap = dr.GetString(24);
                            Ser.ResultPap = dr.GetString(25);
                            Ser.MACtipTime = dr.GetString(26);
                            Ser.Otrosginec = dr.GetString(27);
                            Ser.ProxCita = dr.GetString(28);
                            Ser.ExaGeneral = dr.GetString(29);
                            Ser.UbicEspTampa = dr.GetString(30);
                            Ser.EstNutricion = dr.GetString(31);
                            Ser.EstHidratacion = dr.GetString(32);
                            Ser.PielFanerasTejido = dr.GetString(33);
                            Ser.Mamas = dr.GetString(34);
                            Ser.SisRespiratorio = dr.GetString(35);
                            Ser.CabezaCuello = dr.GetString(36);
                            Ser.SisOsteoMuscular = dr.GetString(37);
                            Ser.SisCardiovascular = dr.GetString(38);
                            Ser.AbdomenPelvis = dr.GetString(39);
                            Ser.ExaObstetrico = dr.GetString(40);
                            Ser.SisGenitourinario = dr.GetString(41);
                            Ser.SisNervioso = dr.GetString(42);
                            Ser.Estado = dr.GetBoolean(43);
                            Ser.Asistente = dr.GetString(44);
                            Ser.FecRegAsist = dr.GetString(45);
                            Ser.Medico = dr.GetString(46);
                            Ser.Servicio = dr.GetString(47);
                            Ser.FecRegMed = dr.GetString(48);
                            Ser.InterConsulta = dr.GetString(50);
                            Ser.Tarifa = dr.GetString(50);
                            Ser.CodCue = dr.GetInt32(51);
                            Ser.Item = dr.GetInt32(52);

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Ficha_Electronica> ListaDiagnostico(int FE, int Procedencia)
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("uspListadoDiagnostico", con))
                {
                    cmd.Parameters.AddWithValue("@FE", FE);
                    cmd.Parameters.AddWithValue("@Procedencia", Procedencia);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Ficha_Electronica Ser = new E_Ficha_Electronica();
                            Ser.CIe10 = dr.GetString(0);
                            Ser.CodCue = dr.GetInt32(1);
                            Ser.Procedencia = dr.GetInt32(2);
                            Ser.FE = dr.GetInt32(3);
                            Ser.Historia = dr.GetInt32(4);
                            Ser.Descripcion = dr.GetString(5);
                            Ser.Estado = dr.GetBoolean(6);
                            Ser.Medico = dr.GetString(7);
                            Ser.Servicio = dr.GetString(8);
                            Ser.Item = dr.GetInt32(9);
                            Ser.tipoDiagnostico = dr.GetString(13);

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public ActionResult imprimeFichaElectronica(int fe, int procedencia, int CodCue, string turno)
        {
            ViewBag.listaFe = (List<E_Ficha_Electronica>)ListaFichaElectronicaxFE(fe).ToList();
            ViewBag.listaExamen = (List<E_Ficha_Electronica>)ListaExamenAuxiliar(fe).ToList();
            ViewBag.listaReceta = (List<E_Ficha_Electronica>)ListaReceta(fe, procedencia).ToList();
            ViewBag.listaDiagnostico = (List<E_Ficha_Electronica>)ListaDiagnostico(fe, procedencia).ToList();
            E_Ficha_Electronica fi = ListaFichaElectronicaxFE(fe).FirstOrDefault();
            ViewBag.medico = fi.NomMed;
            ViewBag.historiaPac = fi.Historia;
            ViewBag.servicio = fi.NomServ;
            ViewBag.proxCita = fi.ProxCita;
            ViewBag.InterCon = fi.InterConsulta;
            ViewBag.abrev = fi.Abreviatura;

            PacientesController p = new PacientesController();
            E_Pacientes pa = p.ListadoPacientes().Find(x => x.Historia==fi.Historia);
            string CancerP, DiabetesP, ACVP, AlergiaP, HipertArtP, OtrosP;
            if (pa.CancerP == true) { CancerP = "SI"; } else { CancerP = "NO"; }
            if (pa.DiabetesP == true) { DiabetesP = "SI"; } else { DiabetesP = "NO"; }
            if (pa.ACVP == true) { ACVP = "SI"; } else { ACVP = "NO"; }
            if (pa.AlergiaP == true) { AlergiaP = "SI"; } else { AlergiaP = "NO"; }
            if (pa.HipertArtP == true) { HipertArtP = "SI"; } else { HipertArtP = "NO"; }
            if (pa.OtrosP == true) { OtrosP = "SI"; } else { OtrosP = "NO"; }

            ViewBag.antecendenteP = "CANCER: " + CancerP + " / " + "DIABETES: " + DiabetesP + " / " + "ACV: " + ACVP + " / " + "ALERGIA: " + AlergiaP + " / " + "HIPER. ART: " + HipertArtP + " / " + "OTROS: " + OtrosP + " / " + "OBS: " + pa.ObservacionP;

            string CancerF, DiabetesF, ACVF, AlergiaF, HipertArtF, OtrosF;
            if (pa.CancerF == true) { CancerF = "SI"; } else { CancerF = "NO"; }
            if (pa.DiabetesF == true) { DiabetesF = "SI"; } else { DiabetesF = "NO"; }
            if (pa.ACVF == true) { ACVF = "SI"; } else { ACVF = "NO"; }
            if (pa.AlergiaF == true) { AlergiaF = "SI"; } else { AlergiaF = "NO"; }
            if (pa.HipertArtF == true) { HipertArtF = "SI"; } else { HipertArtF = "NO"; }
            if (pa.OtrosF == true) { OtrosF = "SI"; } else { OtrosF = "NO"; }

            ViewBag.antecendenteF = "CANCER: " + CancerF + " / " + "DIABETES: " + DiabetesF + " / " + "ACV: " + ACVF + " / " + "ALERGIA: " + AlergiaF + " / " + "HIPER. ART: " + HipertArtF + " / " + "OTROS: " + OtrosF + " / " + "OBS: " + pa.ObservacionF;

            return View();
        }


        public ActionResult imprimeProcedimiento(int fe, int procedencia, int CodCue, string turno)
        {
            ViewBag.listaFe = (List<E_Ficha_Electronica>)ListaProcedimiento(fe).ToList();
            ViewBag.listaReceta = (List<E_Ficha_Electronica>)ListaReceta(fe, procedencia).ToList();
            ViewBag.listaDiagnostico = (List<E_Ficha_Electronica>)ListaDiagnostico(fe, procedencia).ToList();
            E_Ficha_Electronica fi = ListaProcedimiento(fe).FirstOrDefault();
            ViewBag.medico = fi.NomMed;
            ViewBag.servicio = fi.NomServ;
            ViewBag.proxCita = fi.ProximaProc;
            ViewBag.InterCon = fi.InterConsulta;
            ViewBag.abrev = fi.Abreviatura;
            ViewBag.historiaPac = fi.Historia;

            return View();
        }

        public ActionResult imprimeExAuxiliares(int fe, int procedencia, int CodCue, string turno)
        {
            ViewBag.listaFe = (List<E_Ficha_Electronica>)ListaModuloExamenAuxiliar(fe).ToList();
            E_Ficha_Electronica fi = ListaModuloExamenAuxiliar(fe).FirstOrDefault();
            ViewBag.medico = fi.NomMed;
            ViewBag.servicio = fi.NomServ;
            ViewBag.proxCita = fi.ProximaProc;
            ViewBag.InterCon = fi.InterConsulta;
            ViewBag.abrev = fi.Abreviatura;
            ViewBag.historiaPac = fi.Historia;

            return View();
        }

        public List<E_Ficha_Electronica> ListaReceta(int FE, int Procedencia)
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("uspListadoReceta", con))
                {
                    cmd.Parameters.AddWithValue("@FE", FE);
                    cmd.Parameters.AddWithValue("@Procedencia", Procedencia);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Ficha_Electronica Ser = new E_Ficha_Electronica();
                            Ser.CodFarmaco = dr.GetInt32(0);
                            Ser.Item = dr.GetInt32(1);
                            Ser.Descripcion = dr.GetString(2);
                            Ser.FE = dr.GetInt32(3);
                            Ser.Procedencia = dr.GetInt32(4);
                            Ser.Concentra = dr.GetString(5);
                            Ser.FormaFarmec = dr.GetInt32(6);
                            Ser.Dosis = dr.GetString(7);
                            Ser.Frecuencia = dr.GetInt32(8);
                            Ser.ViaAdmin = dr.GetString(9);
                            Ser.Duracion = dr.GetString(10);
                            Ser.CodCue = dr.GetInt32(11);
                            Ser.Cantidad = dr.GetInt32(12);
                            Ser.Estado = dr.GetBoolean(13);
                            Ser.CodMed = dr.GetString(14);
                            Ser.Servicio = dr.GetString(15);
                            Ser.Observ = dr.GetString(16);
                            Ser.DesFromFar = dr.GetString(20);
                            Ser.DesFre = dr.GetString(21);

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_Ficha_Electronica> ListaExamenAuxiliar(int FE)
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("uspListadoExamen_Axiliares", con))
                {
                    cmd.Parameters.AddWithValue("@FE", FE);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Ficha_Electronica Ser = new E_Ficha_Electronica();
                            Ser.EA = dr.GetInt32(0);
                            Ser.CodCue = dr.GetInt32(1);
                            Ser.FE = dr.GetInt32(2);
                            Ser.Especialidad = dr.GetString(3);
                            Ser.Tarifa = dr.GetString(4);
                            Ser.cant = dr.GetInt32(5);
                            Ser.Otros = dr.GetString(6);
                            Ser.Estado = dr.GetBoolean(7);
                            Ser.Item = dr.GetInt32(8);
                            Ser.DescrEspecialidad = dr["NomEspec"] is DBNull ? "" : dr["NomEspec"].ToString();
                            Ser.DescTar = dr["DescTar"] is DBNull ? "" : dr["DescTar"].ToString();

                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }



        public List<E_Ficha_Electronica> ListaFichaElectronicaxFE(int FE)
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_listaFichaElectronicaXfe", con))
                {
                    cmd.Parameters.AddWithValue("@FE", FE);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Ficha_Electronica Ser = new E_Ficha_Electronica();
                            Ser.FE = dr.GetInt32(0);
                            Ser.Historia = dr.GetInt32(1);
                            Ser.FC = dr.GetString(2);
                            Ser.PA = dr.GetString(3);
                            Ser.FR = dr.GetString(4);
                            Ser.Tax = dr.GetDecimal(5);
                            Ser.Tanal = dr.GetDecimal(6);
                            Ser.Peso = dr.GetDecimal(7);
                            Ser.talla = dr.GetDecimal(8);
                            Ser.IMC = dr.GetDecimal(9);
                            Ser.FUR = dr.GetString(10);
                            Ser.MotivoConsulta = dr.GetString(11);
                            Ser.Relato = dr.GetString(12);
                            Ser.TiempoEnfermedad = dr.GetString(13);
                            Ser.Inicio = dr.GetString(14);
                            Ser.Curso = dr.GetString(15);
                            Ser.sed = dr.GetString(16);
                            Ser.sueño = dr.GetString(17);
                            Ser.Orina = dr.GetString(18);
                            Ser.apetito = dr.GetString(19);
                            Ser.paridad = dr.GetInt32(20);
                            Ser.FPP = dr.GetString(21);
                            Ser.EdadGestacional = dr.GetInt32(22);
                            Ser.PIG = dr.GetString(23);
                            Ser.UltimoPap = dr.GetString(24);
                            Ser.ResultPap = dr.GetString(25);
                            Ser.MACtipTime = dr.GetString(26);
                            Ser.Otrosginec = dr.GetString(27);
                            Ser.ProxCita = dr.GetString(28);
                            Ser.ExaGeneral = dr.GetString(29);
                            Ser.UbicEspTampa = dr.GetString(30);
                            Ser.EstNutricion = dr.GetString(31);
                            Ser.EstHidratacion = dr.GetString(32);
                            Ser.PielFanerasTejido = dr.GetString(33);
                            Ser.Mamas = dr.GetString(34);
                            Ser.SisRespiratorio = dr.GetString(35);
                            Ser.CabezaCuello = dr.GetString(36);
                            Ser.SisOsteoMuscular = dr.GetString(37);
                            Ser.SisCardiovascular = dr.GetString(38);
                            Ser.AbdomenPelvis = dr.GetString(39);
                            Ser.ExaObstetrico = dr.GetString(40);
                            Ser.SisGenitourinario = dr.GetString(41);
                            Ser.SisNervioso = dr.GetString(42);
                            Ser.Estado = dr.GetBoolean(43);
                            Ser.Asistente = dr.GetString(44);
                            Ser.FecRegAsist = dr.GetString(45);
                            Ser.Medico = dr.GetString(46);
                            Ser.Servicio = dr.GetString(47);
                            Ser.FecRegMed = dr.GetString(48);
                            Ser.InterConsulta = dr.GetString(49);
                            Ser.Tarifa = dr.GetString(50);
                            Ser.CodCue = dr.GetInt32(51);
                            Ser.Item = dr.GetInt32(52);
                            Ser.Nombre = dr.GetString(56);
                            Ser.edad = dr.GetInt32(57);
                            Ser.numDoc = dr.GetString(58);
                            Ser.sexo = dr.GetString(59);
                            Ser.DescTar = dr.GetString(60);
                            Ser.NomMed = dr["NomMed"] is DBNull ? "" : dr["NomMed"].ToString();
                            Ser.NomServ = dr.GetString(62);
                            Ser.Abreviatura = dr["abreviatura"] is DBNull ? "" : dr["abreviatura"].ToString();
                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }


        public List<E_Ficha_Electronica> ListaProcedimiento(int idProc)
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ListaProcedimientos", con))
                {
                    cmd.Parameters.AddWithValue("@idProc", idProc);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Ficha_Electronica Ser = new E_Ficha_Electronica();
                            Ser.idProc = dr.GetInt32(0);
                            Ser.FC = dr.GetString(1);
                            Ser.PA = dr.GetString(2);
                            Ser.FR = dr.GetString(3);
                            Ser.Tax = dr.GetDecimal(4);
                            Ser.Tanal = dr.GetDecimal(5);
                            Ser.DescripcionProc = dr.GetString(6);
                            Ser.CodTar = dr.GetString(7);
                            Ser.Item = dr.GetInt32(8);
                            Ser.Historia = dr.GetInt32(9);
                            Ser.CodCue = dr.GetInt32(10);
                            Ser.Observ = dr.GetString(11);
                            Ser.ProximaProc = dr.GetString(12);
                            Ser.Asistente = dr.GetString(13);
                            Ser.FecRegAsist = dr.GetString(14);
                            Ser.CodMed = dr.GetString(15);
                            Ser.FecRegMedProc = dr.GetString(16);
                            Ser.CodServ = dr.GetString(17);
                            Ser.InterConsulta = dr.GetString(18);
                            Ser.Estado = dr.GetBoolean(19);
                            Ser.Nombre = dr.GetString(23);
                            Ser.edad = dr.GetInt32(24);
                            Ser.numDoc = dr.GetString(25);
                            Ser.sexo = dr.GetString(26);
                            Ser.DescTar = dr.GetString(27);
                            Ser.NomMed = dr["NomMed"] is DBNull ? "" : dr["NomMed"].ToString();
                            Ser.NomServ = dr. GetString(29);
                            Ser.Abreviatura = dr["abreviatura"] is DBNull ? "" : dr["abreviatura"].ToString();
                            Lista.Add(Ser);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }



        public List<E_Ficha_Electronica> ListaModuloExamenAuxiliar(int idExAux)
        {
            List<E_Ficha_Electronica> Lista = new List<E_Ficha_Electronica>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ListaExamenAuxiliar", con))
                {
                    cmd.Parameters.AddWithValue("@idExAux", idExAux);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Ficha_Electronica Ser = new E_Ficha_Electronica();
                            Ser.idExAux = dr.GetInt32(0);
                            Ser.FC = dr.GetString(1);
                            Ser.PA = dr.GetString(2);
                            Ser.FR = dr.GetString(3);
                            Ser.Tax = dr.GetDecimal(4);
                            Ser.Tanal = dr.GetDecimal(5);
                            Ser.DescripcionProc = dr.GetString(6);
                            Ser.CodTar = dr.GetString(7);
                            Ser.Item = dr.GetInt32(8);
                            Ser.Historia = dr.GetInt32(9);
                            Ser.CodCue = dr.GetInt32(10);
                            Ser.Observ = dr.GetString(11);
                            Ser.ProximaProc = dr.GetString(12);
                            Ser.Asistente = dr.GetString(13);
                            Ser.FecRegAsist = dr.GetString(14);
                            Ser.CodMed = dr.GetString(15);
                            Ser.FecRegMedProc = dr.GetString(16);
                            Ser.CodServ = dr.GetString(17);
                            Ser.InterConsulta = dr.GetString(18);
                            Ser.Estado = dr.GetBoolean(19);
                            Ser.Nombre = dr.GetString(23);
                            Ser.edad = dr.GetInt32(24);
                            Ser.numDoc = dr.GetString(25);
                            Ser.sexo = dr.GetString(26);
                            Ser.DescTar = dr.GetString(27);
                            Ser.NomMed = dr["NomMed"] is DBNull ? "" : dr["NomMed"].ToString();
                            Ser.NomServ = dr.GetString(29);
                            Ser.Abreviatura = dr["abreviatura"] is DBNull ? "" : dr["abreviatura"].ToString();
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