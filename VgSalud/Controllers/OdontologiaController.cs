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

        public ActionResult ListadoCarnetOdontologiaEnEspera()
        {
            ViewBag.lista = ListadoCarnet_Odontologia_DelDia_EnEspera();
            ViewBag.postergado = Usp_Odontologia_Postergados();
            return View();
        }

        [HttpPost]
        public ActionResult ListadoCarnetOdontologiaEnEspera(E_CSOdontologia odo)
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


        public List<E_CSOdontologia> Usp_Odontologia_Postergados()
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSOdontologia> ListaCarnetOdo = new List<E_CSOdontologia>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_Odontologia_Postergados", db))
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
                    using (cmd = new SqlCommand("Usp_GenerarOdontograma", db))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdOdontologia", IdOdontologia);

                        // -- DIENTE # 18 --
                        if (odo.hiddenD18LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente18LT", odo.hiddenD18LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente18LT", "");
                        }
                        //*
                        if (odo.hiddenD18LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente18LL", odo.hiddenD18LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente18LL", "");
                        }
                        //*
                        if (odo.hiddenD18LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente18LC", odo.hiddenD18LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente18LC", "");
                        }
                        //*
                        if (odo.hiddenD18LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente18LR", odo.hiddenD18LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente18LR", "");
                        }
                        //*
                        if (odo.hiddenD18LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente18LB", odo.hiddenD18LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente18LB", "");
                        }

                        // -- DIENTE # 17 --
                        if (odo.hiddenD17LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente17LT", odo.hiddenD17LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente17LT", "");
                        }
                        //*
                        if (odo.hiddenD17LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente17LL", odo.hiddenD17LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente17LL", "");
                        }
                        //*
                        if (odo.hiddenD17LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente17LC", odo.hiddenD17LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente17LC", "");
                        }
                        //*
                        if (odo.hiddenD17LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente17LR", odo.hiddenD17LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente17LR", "");
                        }
                        //*
                        if (odo.hiddenD17LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente17LB", odo.hiddenD17LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente17LB", "");
                        }

                        // -- DIENTE # 16 --
                        if (odo.hiddenD16LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente16LT", odo.hiddenD16LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente16LT", "");
                        }
                        //*
                        if (odo.hiddenD16LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente16LL", odo.hiddenD16LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente16LL", "");
                        }
                        //*
                        if (odo.hiddenD16LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente16LC", odo.hiddenD16LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente16LC", "");
                        }
                        //*
                        if (odo.hiddenD16LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente16LR", odo.hiddenD16LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente16LR", "");
                        }
                        //*
                        if (odo.hiddenD16LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente16LB", odo.hiddenD16LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente16LB", "");
                        }

                        // -- DIENTE # 15 --
                        if (odo.hiddenD15LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente15LT", odo.hiddenD15LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente15LT", "");
                        }
                        //*
                        if (odo.hiddenD15LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente15LL", odo.hiddenD15LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente15LL", "");
                        }
                        //*
                        if (odo.hiddenD15LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente15LC", odo.hiddenD15LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente15LC", "");
                        }
                        //*
                        if (odo.hiddenD15LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente15LR", odo.hiddenD15LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente15LR", "");
                        }
                        //*
                        if (odo.hiddenD15LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente15LB", odo.hiddenD15LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente15LB", "");
                        }

                        // -- DIENTE # 14 --
                        if (odo.hiddenD14LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente14LT", odo.hiddenD14LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente14LT", "");
                        }
                        //*
                        if (odo.hiddenD14LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente14LL", odo.hiddenD14LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente14LL", "");
                        }
                        //*
                        if (odo.hiddenD14LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente14LC", odo.hiddenD14LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente14LC", "");
                        }
                        //*
                        if (odo.hiddenD14LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente14LR", odo.hiddenD14LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente14LR", "");
                        }
                        //*
                        if (odo.hiddenD14LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente14LB", odo.hiddenD14LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente14LB", "");
                        }

                        // -- DIENTE # 13 --
                        if (odo.hiddenD13LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente13LT", odo.hiddenD13LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente13LT", "");
                        }
                        //*
                        if (odo.hiddenD13LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente13LL", odo.hiddenD13LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente13LL", "");
                        }
                        //*
                        if (odo.hiddenD13LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente13LC", odo.hiddenD13LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente13LC", "");
                        }
                        //*
                        if (odo.hiddenD13LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente13LR", odo.hiddenD13LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente13LR", "");
                        }
                        //*
                        if (odo.hiddenD13LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente13LB", odo.hiddenD13LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente13LB", "");
                        }

                        // -- DIENTE # 12 --
                        if (odo.hiddenD12LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente12LT", odo.hiddenD12LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente12LT", "");
                        }
                        //*
                        if (odo.hiddenD12LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente12LL", odo.hiddenD12LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente12LL", "");
                        }
                        //*
                        if (odo.hiddenD12LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente12LC", odo.hiddenD12LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente12LC", "");
                        }
                        //*
                        if (odo.hiddenD12LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente12LR", odo.hiddenD12LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente12LR", "");
                        }
                        //*
                        if (odo.hiddenD12LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente12LB", odo.hiddenD12LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente12LB", "");
                        }


                        // -- DIENTE # 11 --
                        if (odo.hiddenD11LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente11LT", odo.hiddenD11LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente11LT", "");
                        }
                        //*
                        if (odo.hiddenD11LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente11LL", odo.hiddenD11LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente11LL", "");
                        }
                        //*
                        if (odo.hiddenD11LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente11LC", odo.hiddenD11LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente11LC", "");
                        }
                        //*
                        if (odo.hiddenD11LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente11LR", odo.hiddenD11LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente11LR", "");
                        }
                        //*
                        if (odo.hiddenD11LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente11LB", odo.hiddenD11LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente11LB", "");
                        }

                        //CUADRANTE SUPERIOR IZQUIERDA

                        // -- DIENTE # 21 --
                        if (odo.hiddenD21LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente21LT", odo.hiddenD21LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente21LT", "");
                        }
                        //*
                        if (odo.hiddenD21LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente21LL", odo.hiddenD21LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente21LL", "");
                        }
                        //*
                        if (odo.hiddenD21LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente21LC", odo.hiddenD21LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente21LC", "");
                        }
                        //*
                        if (odo.hiddenD21LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente21LR", odo.hiddenD21LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente21LR", "");
                        }
                        //*
                        if (odo.hiddenD21LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente21LB", odo.hiddenD21LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente21LB", "");
                        }

                        // -- DIENTE # 22 --
                        if (odo.hiddenD22LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente22LT", odo.hiddenD22LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente22LT", "");
                        }
                        //*
                        if (odo.hiddenD22LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente22LL", odo.hiddenD22LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente22LL", "");
                        }
                        //*
                        if (odo.hiddenD22LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente22LC", odo.hiddenD22LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente22LC", "");
                        }
                        //*
                        if (odo.hiddenD22LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente22LR", odo.hiddenD22LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente22LR", "");
                        }
                        //*
                        if (odo.hiddenD22LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente22LB", odo.hiddenD22LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente22LB", "");
                        }

                        // -- DIENTE # 23 --
                        if (odo.hiddenD23LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente23LT", odo.hiddenD23LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente23LT", "");
                        }
                        //*
                        if (odo.hiddenD23LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente23LL", odo.hiddenD23LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente23LL", "");
                        }
                        //*
                        if (odo.hiddenD23LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente23LC", odo.hiddenD23LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente23LC", "");
                        }
                        //*
                        if (odo.hiddenD23LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente23LR", odo.hiddenD23LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente23LR", "");
                        }
                        //*
                        if (odo.hiddenD23LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente23LB", odo.hiddenD23LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente23LB", "");
                        }

                        // -- DIENTE # 24 --
                        if (odo.hiddenD24LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente24LT", odo.hiddenD24LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente24LT", "");
                        }
                        //*
                        if (odo.hiddenD24LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente24LL", odo.hiddenD24LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente24LL", "");
                        }
                        //*
                        if (odo.hiddenD24LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente24LC", odo.hiddenD24LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente24LC", "");
                        }
                        //*
                        if (odo.hiddenD24LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente24LR", odo.hiddenD24LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente24LR", "");
                        }
                        //*
                        if (odo.hiddenD24LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente24LB", odo.hiddenD24LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente24LB", "");
                        }

                        // -- DIENTE # 25 --
                        if (odo.hiddenD25LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente25LT", odo.hiddenD25LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente25LT", "");
                        }
                        //*
                        if (odo.hiddenD25LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente25LL", odo.hiddenD25LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente25LL", "");
                        }
                        //*
                        if (odo.hiddenD25LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente25LC", odo.hiddenD25LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente25LC", "");
                        }
                        //*
                        if (odo.hiddenD25LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente25LR", odo.hiddenD25LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente25LR", "");
                        }
                        //*
                        if (odo.hiddenD25LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente25LB", odo.hiddenD25LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente25LB", "");
                        }

                        // -- DIENTE # 26 --
                        if (odo.hiddenD26LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente26LT", odo.hiddenD26LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente26LT", "");
                        }
                        //*
                        if (odo.hiddenD26LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente26LL", odo.hiddenD26LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente26LL", "");
                        }
                        //*
                        if (odo.hiddenD26LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente26LC", odo.hiddenD26LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente26LC", "");
                        }
                        //*
                        if (odo.hiddenD26LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente26LR", odo.hiddenD26LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente26LR", "");
                        }
                        //*
                        if (odo.hiddenD26LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente26LB", odo.hiddenD26LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente26LB", "");
                        }

                        // -- DIENTE # 27 --
                        if (odo.hiddenD27LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente27LT", odo.hiddenD27LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente27LT", "");
                        }
                        //*
                        if (odo.hiddenD27LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente27LL", odo.hiddenD27LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente27LL", "");
                        }
                        //*
                        if (odo.hiddenD27LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente27LC", odo.hiddenD27LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente27LC", "");
                        }
                        //*
                        if (odo.hiddenD27LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente27LR", odo.hiddenD27LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente27LR", "");
                        }
                        //*
                        if (odo.hiddenD27LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente27LB", odo.hiddenD27LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente27LB", "");
                        }


                        // -- DIENTE # 28 --
                        if (odo.hiddenD28LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente28LT", odo.hiddenD28LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente28LT", "");
                        }
                        //*
                        if (odo.hiddenD28LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente28LL", odo.hiddenD28LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente28LL", "");
                        }
                        //*
                        if (odo.hiddenD28LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente28LC", odo.hiddenD28LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente28LC", "");
                        }
                        //*
                        if (odo.hiddenD28LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente28LR", odo.hiddenD28LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente28LR", "");
                        }
                        //*
                        if (odo.hiddenD28LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente28LB", odo.hiddenD28LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente28LB", "");
                        }


                        //CUADRANTE INFERIOR DERECHO

                        // -- DIENTE # 48 --
                        if (odo.hiddenD48LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente48LT", odo.hiddenD48LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente48LT", "");
                        }
                        //*
                        if (odo.hiddenD48LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente48LL", odo.hiddenD48LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente48LL", "");
                        }
                        //*
                        if (odo.hiddenD48LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente48LC", odo.hiddenD48LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente48LC", "");
                        }
                        //*
                        if (odo.hiddenD48LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente48LR", odo.hiddenD48LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente48LR", "");
                        }
                        //*
                        if (odo.hiddenD48LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente48LB", odo.hiddenD48LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente48LB", "");
                        }

                        // -- DIENTE # 47 --
                        if (odo.hiddenD47LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente47LT", odo.hiddenD47LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente47LT", "");
                        }
                        //*
                        if (odo.hiddenD47LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente47LL", odo.hiddenD47LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente47LL", "");
                        }
                        //*
                        if (odo.hiddenD47LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente47LC", odo.hiddenD47LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente47LC", "");
                        }
                        //*
                        if (odo.hiddenD47LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente47LR", odo.hiddenD47LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente47LR", "");
                        }
                        //*
                        if (odo.hiddenD47LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente47LB", odo.hiddenD47LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente47LB", "");
                        }

                        // -- DIENTE # 46 --
                        if (odo.hiddenD46LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente46LT", odo.hiddenD46LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente46LT", "");
                        }
                        //*
                        if (odo.hiddenD46LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente46LL", odo.hiddenD46LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente46LL", "");
                        }
                        //*
                        if (odo.hiddenD46LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente46LC", odo.hiddenD46LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente46LC", "");
                        }
                        //*
                        if (odo.hiddenD46LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente46LR", odo.hiddenD46LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente46LR", "");
                        }
                        //*
                        if (odo.hiddenD46LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente46LB", odo.hiddenD46LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente46LB", "");
                        }

                        // -- DIENTE # 45 --
                        if (odo.hiddenD45LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente45LT", odo.hiddenD45LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente45LT", "");
                        }
                        //*
                        if (odo.hiddenD45LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente45LL", odo.hiddenD45LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente45LL", "");
                        }
                        //*
                        if (odo.hiddenD45LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente45LC", odo.hiddenD45LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente45LC", "");
                        }
                        //*
                        if (odo.hiddenD45LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente45LR", odo.hiddenD45LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente45LR", "");
                        }
                        //*
                        if (odo.hiddenD45LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente45LB", odo.hiddenD45LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente45LB", "");
                        }

                        // -- DIENTE # 44 --
                        if (odo.hiddenD44LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente44LT", odo.hiddenD44LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente44LT", "");
                        }
                        //*
                        if (odo.hiddenD44LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente44LL", odo.hiddenD44LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente44LL", "");
                        }
                        //*
                        if (odo.hiddenD44LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente44LC", odo.hiddenD44LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente44LC", "");
                        }
                        //*
                        if (odo.hiddenD44LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente44LR", odo.hiddenD44LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente44LR", "");
                        }
                        //*
                        if (odo.hiddenD44LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente44LB", odo.hiddenD44LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente44LB", "");
                        }

                        // -- DIENTE # 43 --
                        if (odo.hiddenD43LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente43LT", odo.hiddenD43LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente43LT", "");
                        }
                        //*
                        if (odo.hiddenD43LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente43LL", odo.hiddenD43LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente43LL", "");
                        }
                        //*
                        if (odo.hiddenD43LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente43LC", odo.hiddenD43LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente43LC", "");
                        }
                        //*
                        if (odo.hiddenD43LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente43LR", odo.hiddenD43LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente43LR", "");
                        }
                        //*
                        if (odo.hiddenD43LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente43LB", odo.hiddenD43LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente43LB", "");
                        }

                        // -- DIENTE # 42 --
                        if (odo.hiddenD42LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente42LT", odo.hiddenD42LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente42LT", "");
                        }
                        //*
                        if (odo.hiddenD42LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente42LL", odo.hiddenD42LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente42LL", "");
                        }
                        //*
                        if (odo.hiddenD42LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente42LC", odo.hiddenD42LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente42LC", "");
                        }
                        //*
                        if (odo.hiddenD42LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente42LR", odo.hiddenD42LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente42LR", "");
                        }
                        //*
                        if (odo.hiddenD42LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente42LB", odo.hiddenD42LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente42LB", "");
                        }


                        // -- DIENTE # 41 --
                        if (odo.hiddenD41LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente41LT", odo.hiddenD41LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente41LT", "");
                        }
                        //*
                        if (odo.hiddenD41LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente41LL", odo.hiddenD41LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente41LL", "");
                        }
                        //*
                        if (odo.hiddenD41LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente41LC", odo.hiddenD41LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente41LC", "");
                        }
                        //*
                        if (odo.hiddenD41LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente41LR", odo.hiddenD41LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente41LR", "");
                        }
                        //*
                        if (odo.hiddenD41LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente41LB", odo.hiddenD41LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente41LB", "");
                        }


                        //CUADRANTE INFERIOR IZQUIERDO

                        // -- DIENTE # 31 --
                        if (odo.hiddenD31LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente31LT", odo.hiddenD31LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente31LT", "");
                        }
                        //*
                        if (odo.hiddenD31LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente31LL", odo.hiddenD31LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente31LL", "");
                        }
                        //*
                        if (odo.hiddenD31LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente31LC", odo.hiddenD31LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente31LC", "");
                        }
                        //*
                        if (odo.hiddenD31LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente31LR", odo.hiddenD31LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente31LR", "");
                        }
                        //*
                        if (odo.hiddenD31LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente31LB", odo.hiddenD31LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente31LB", "");
                        }

                        // -- DIENTE # 32 --
                        if (odo.hiddenD32LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente32LT", odo.hiddenD32LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente32LT", "");
                        }
                        //*
                        if (odo.hiddenD32LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente32LL", odo.hiddenD32LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente32LL", "");
                        }
                        //*
                        if (odo.hiddenD32LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente32LC", odo.hiddenD32LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente32LC", "");
                        }
                        //*
                        if (odo.hiddenD32LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente32LR", odo.hiddenD32LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente32LR", "");
                        }
                        //*
                        if (odo.hiddenD32LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente32LB", odo.hiddenD32LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente32LB", "");
                        }

                        // -- DIENTE # 33 --
                        if (odo.hiddenD33LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente33LT", odo.hiddenD33LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente33LT", "");
                        }
                        //*
                        if (odo.hiddenD33LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente33LL", odo.hiddenD33LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente33LL", "");
                        }
                        //*
                        if (odo.hiddenD33LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente33LC", odo.hiddenD33LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente33LC", "");
                        }
                        //*
                        if (odo.hiddenD33LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente33LR", odo.hiddenD33LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente33LR", "");
                        }
                        //*
                        if (odo.hiddenD33LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente33LB", odo.hiddenD33LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente33LB", "");
                        }

                        // -- DIENTE # 34 --
                        if (odo.hiddenD34LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente34LT", odo.hiddenD34LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente34LT", "");
                        }
                        //*
                        if (odo.hiddenD34LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente34LL", odo.hiddenD34LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente34LL", "");
                        }
                        //*
                        if (odo.hiddenD34LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente34LC", odo.hiddenD34LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente34LC", "");
                        }
                        //*
                        if (odo.hiddenD34LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente34LR", odo.hiddenD34LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente34LR", "");
                        }
                        //*
                        if (odo.hiddenD34LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente34LB", odo.hiddenD34LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente34LB", "");
                        }

                        // -- DIENTE # 35 --
                        if (odo.hiddenD35LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente35LT", odo.hiddenD35LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente35LT", "");
                        }
                        //*
                        if (odo.hiddenD35LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente35LL", odo.hiddenD35LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente35LL", "");
                        }
                        //*
                        if (odo.hiddenD35LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente35LC", odo.hiddenD35LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente35LC", "");
                        }
                        //*
                        if (odo.hiddenD35LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente35LR", odo.hiddenD35LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente35LR", "");
                        }
                        //*
                        if (odo.hiddenD35LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente35LB", odo.hiddenD35LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente35LB", "");
                        }

                        // -- DIENTE # 36 --
                        if (odo.hiddenD36LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente36LT", odo.hiddenD36LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente36LT", "");
                        }
                        //*
                        if (odo.hiddenD36LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente36LL", odo.hiddenD36LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente36LL", "");
                        }
                        //*
                        if (odo.hiddenD36LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente36LC", odo.hiddenD36LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente36LC", "");
                        }
                        //*
                        if (odo.hiddenD36LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente36LR", odo.hiddenD36LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente36LR", "");
                        }
                        //*
                        if (odo.hiddenD36LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente36LB", odo.hiddenD36LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente36LB", "");
                        }

                        // -- DIENTE # 37 --
                        if (odo.hiddenD37LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente37LT", odo.hiddenD37LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente37LT", "");
                        }
                        //*
                        if (odo.hiddenD37LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente37LL", odo.hiddenD37LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente37LL", "");
                        }
                        //*
                        if (odo.hiddenD37LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente37LC", odo.hiddenD37LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente37LC", "");
                        }
                        //*
                        if (odo.hiddenD37LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente37LR", odo.hiddenD37LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente37LR", "");
                        }
                        //*
                        if (odo.hiddenD37LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente37LB", odo.hiddenD37LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente37LB", "");
                        }


                        // -- DIENTE # 38 --
                        if (odo.hiddenD41LT != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente38LT", odo.hiddenD38LT);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente38LT", "");
                        }
                        //*
                        if (odo.hiddenD38LL != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente38LL", odo.hiddenD38LL);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente38LL", "");
                        }
                        //*
                        if (odo.hiddenD38LC != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente38LC", odo.hiddenD38LC);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente38LC", "");
                        }
                        //*
                        if (odo.hiddenD38LR != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente38LR", odo.hiddenD38LR);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente38LR", "");
                        }
                        //*
                        if (odo.hiddenD38LB != null)
                        {
                            cmd.Parameters.AddWithValue("@Diente38LB", odo.hiddenD38LB);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Diente38LB", "");
                        }

                        if (odo.Observacion != null)
                        {
                            cmd.Parameters.AddWithValue("@Observacion", odo.Observacion);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Observacion", "");
                        }
                        cmd.Parameters.AddWithValue("@Apto", odo.Apto);
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
        


        public ActionResult ListaPacientesAtendidosDelDia()
        {
            ViewBag.Lista = ListarPacientesAtendidosEnElDia();

            return View();
        }


        public List<E_CSOdontologia> ListarPacientesAtendidosEnElDia()
        {
            string sede = Session["CodSede"].ToString();

            List<E_CSOdontologia> ListaPacientesOdo = new List<E_CSOdontologia>();

            using (db = new SqlConnection(cadena))
            {
                using (cmd = new SqlCommand("Usp_ListadoPacientesAtendidosEnElDia_CSOdontologia", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
                        ListaPacientesOdo.Add(Odo);

                    }


                }
            }

            return ListaPacientesOdo;
        }

    }
}