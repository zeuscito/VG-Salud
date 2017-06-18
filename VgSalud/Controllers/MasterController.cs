using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using VgSalud.Models;
using System.Text;
using System.Security.Cryptography;

namespace VgSalud.Controllers
{
    public class MasterController : Controller
    {
        string key = "ABCDEFG54669525PQRSTUVWXYZabcdef852846opqrstuvwxyz";
        public List<E_Master> ListaFormularios()
        {
            List<E_Master> Lista = new List<E_Master>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ListaFormularios", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Master acc = new E_Master();

                            acc.IdForm = dr.GetInt32(0);
                            acc.NombreForm = dr.GetString(1);
                            acc.AliasForm = dr.GetString(2);
                            

                            Lista.Add(acc);
                        }
                        con.Close();
                    }

                }
                return Lista;
            }
        }

        public List<E_Master> Listados()
        {
            List<E_Master> lista = new List<E_Master>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VG_SALUD"].ToString()))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("master_tablas", con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = com.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            E_Master c = new E_Master();
                            c.nombre = dr.GetString(0);
                            c.cantidad = dr.GetString(1);

                            lista.Add(c);
                        }
                        con.Close();
                    }
                }
                return lista;
            }
        }

        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Login/Index");
            }
        }


    

        public string EncryptKey(string cadena)
        {
            
            byte[] keyArray;
            byte[] Arreglo_a_Cifrar =
            UTF8Encoding.UTF8.GetBytes(cadena);

            MD5CryptoServiceProvider hashmd5 =
            new MD5CryptoServiceProvider();

            keyArray = hashmd5.ComputeHash(
            UTF8Encoding.UTF8.GetBytes(key));

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes =
            new TripleDESCryptoServiceProvider();

            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform =
            tdes.CreateEncryptor();

            byte[] ArrayResultado =
            cTransform.TransformFinalBlock(Arreglo_a_Cifrar,
            0, Arreglo_a_Cifrar.Length);

            tdes.Clear();

            return Convert.ToBase64String(ArrayResultado,
                   0, ArrayResultado.Length);

        }

        public string DecryptKey(string clave)
        {

            byte[] keyArray;
            byte[] Array_a_Descifrar =
            Convert.FromBase64String(clave);

            MD5CryptoServiceProvider hashmd5 =
            new MD5CryptoServiceProvider();

            keyArray = hashmd5.ComputeHash(
            UTF8Encoding.UTF8.GetBytes(key));

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes =
            new TripleDESCryptoServiceProvider();

            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform =
             tdes.CreateDecryptor();

            byte[] resultArray =
            cTransform.TransformFinalBlock(Array_a_Descifrar,
            0, Array_a_Descifrar.Length);

            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);

        }


    }
}