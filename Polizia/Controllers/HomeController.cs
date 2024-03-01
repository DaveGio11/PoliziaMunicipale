using Polizia.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Polizia.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Violazione()
        {
            List<Violazione> violazione = new List<Violazione>();
            string connectionString = ConfigurationManager.ConnectionStrings["Polizia"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM TipoViolazioni";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Violazione d = new Violazione();
                        d.IdViolazione = Convert.ToInt16(reader["IdViolazione"]);

                        d.Descrizione = reader["Descrizione"].ToString();
                        d.IdVerbale = Convert.ToInt16(reader["IdVerbale"]);
                        violazione.Add(d);
                    }

                }
                catch (Exception ex)
                {
                    Response.Write("Errore: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
                return View(violazione);
            }
        }

        public ActionResult Verbale()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Verbale(Verbale verbale)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Polizia"].ConnectionString.ToString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Verbal (DataViolazione, IndirizzoViolazione, IdentificativoAgente, DataVerbale, Importo, DecurtamentoPunti, IdViolazione, IdAnagrafica) VALUES (@DataViolazione, @IndirizzoViolazione, @IdentificativoAgente, @DataVerbale, @Importo, @DecurtamentoPunti, @IdViolazione, @IdAnagrafica)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DataViolazione", verbale.DataViolazione);
                        command.Parameters.AddWithValue("@IndirizzoViolazione", verbale.IndirizzoViolazione);
                        command.Parameters.AddWithValue("@IdentificativoAgente", verbale.IdentificativoAgente);
                        command.Parameters.AddWithValue("@DataVerbale", verbale.DataVerbale);
                        command.Parameters.AddWithValue("@Importo", verbale.Importo);
                        command.Parameters.AddWithValue("@DecurtamentoPunti", verbale.DecurtamentoPunti);
                        command.Parameters.AddWithValue("@IdViolazione", verbale.IdViolazione);
                        command.Parameters.AddWithValue("@IdAnagrafica", verbale.IdAnagrafica);


                        command.ExecuteNonQuery();
                    }
                }


            }

            return View(verbale);
        }

        public ActionResult Anagrafica()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Anagrafica(Anagrafica anagrafica)
        {

            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Polizia"].ConnectionString.ToString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = "INSERT INTO Anagrafica (Nome, Cognome, Citta, Cap, CodiceFiscale, Indirizzo) VALUES (@Nome, @Cognome, @Citta, @Cap, @CodiceFiscale, @Indirizzo)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        connection.Open();
                        command.Parameters.AddWithValue("@Nome", anagrafica.Nome);
                        command.Parameters.AddWithValue("@Cognome", anagrafica.Cognome);

                        command.Parameters.AddWithValue("@Citta", anagrafica.Citta);
                        command.Parameters.AddWithValue("@Cap", anagrafica.Cap);
                        command.Parameters.AddWithValue("@CodiceFiscale", anagrafica.CodiceFiscale);
                        command.Parameters.AddWithValue("@Indirizzo", anagrafica.Indirizzo);
                        command.ExecuteNonQuery();
                    }
                }
            }
            return View(anagrafica);

        }

        public ActionResult Multe(Verbale verbale)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Polizia"].ConnectionString.ToString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Verbal WHERE Importo > 400";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DataViolazione", verbale.DataViolazione);
                        command.Parameters.AddWithValue("@IndirizzoViolazione", verbale.IndirizzoViolazione);
                        command.Parameters.AddWithValue("@IdentificativoAgente", verbale.IdentificativoAgente);
                        command.Parameters.AddWithValue("@DataVerbale", verbale.DataVerbale);
                        command.Parameters.AddWithValue("@Importo", verbale.Importo);
                        command.Parameters.AddWithValue("@DecurtamentoPunti", verbale.DecurtamentoPunti);
                        command.Parameters.AddWithValue("@IdViolazione", verbale.IdViolazione);
                        command.Parameters.AddWithValue("@IdAnagrafica", verbale.IdAnagrafica);


                        command.ExecuteNonQuery();
                    }
                }


            }

            return View(verbale);
        }

        public ActionResult Trasgressori()
        {
            List<Trasgressori> trasgressori = new List<Trasgressori>();
            string connectionString = ConfigurationManager.ConnectionStrings["Polizia"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT Anagrafica.Nome, Anagrafica.Cognome, COUNT(ANAGRAFICA.IdAnagrafica) AS TOTALE FROM Anagrafica JOIN Verbal ON Anagrafica.IdAnagrafica = Verbal.IdAnagrafica GROUP BY Anagrafica.Nome, Anagrafica.Cognome,";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Trasgressori d = new Trasgressori();
                        d.Cognome = reader["Cognome"].ToString();
                        d.totale = Convert.ToInt32(reader["TOTALE"]);
                        d.trasgressori = reader["Nome"].ToString();

                        trasgressori.Add(d);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Errore: " + ex.Message;
                }
                finally
                {
                    conn.Close();
                }
            }

            return View(trasgressori);
        }

        public ActionResult Punti()
        {
            List<Trasgressori> trasgressori = new List<Trasgressori>();
            string connectionString = ConfigurationManager.ConnectionStrings["Polizia"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT Anagrafica.Nome, Anagrafica.Cognome,, SUM(Verbale.DecurtamentoPunti) AS TOTALE FROM Anagrafica JOIN Verbal ON Anagrafica.IdAnagrafica = Verbal.IdAnagrafica GROUP BY Anagrafica.Nome, Anagrafica.Cognome,";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Trasgressori d = new Trasgressori();
                        d.Cognome = reader["Cognome"].ToString();
                        d.totale = Convert.ToInt32(reader["TOTALE"]);
                        d.trasgressori = reader["Nome"].ToString();

                        trasgressori.Add(d);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Errore: " + ex.Message;
                }
                finally
                {
                    conn.Close();
                }
            }

            return View(trasgressori);
        }

        public ActionResult Piudieci()
        {

            List<Verbale> trasgressori = new List<Verbale>();
            string connectionString = ConfigurationManager.ConnectionStrings["Polizia"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
                try
                {
                    conn.Open();
                    string query = "SELECT v.*, tv.Descrizione_Verbale, a.Nome, a.Cognome " +
                                   "FROM Verbale v " +
                                   "INNER JOIN Violazioni tv ON v.IdViolazione = tv.IdViolazione " +
                                   "INNER JOIN Anagrafica a ON v.IdAnagrafica = a.IdAnagrafica " +
                                   "WHERE v.DecurtamentoPunti > 10";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Verbale v = new Verbale();
                        v.IdVerbale = Convert.ToInt32(reader["IdVerbale"]);
                        v.DataViolazione = Convert.ToDateTime(reader["DataViolazione"]);
                        v.IndirizzoViolazione = reader["IndirizzoViolazione"].ToString();
                        v.IdentificativoAgente = reader["IdentificativoAgente"].ToString();
                        v.DataVerbale = Convert.ToDateTime(reader["DataVerbale"]);
                        v.Importo = Convert.ToInt16(reader["Importo"]);
                        v.DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"]);
                        v.IdAnagrafica = Convert.ToInt32(reader["IdAnagrafica"]);
                        v.IdViolazione = Convert.ToInt32(reader["IdViolazione"]);
                        v.Descrizione_Verbale = reader["Descrizione_Verbale"].ToString();
                        v.Nome = reader["Nome"].ToString();
                        v.Cognome = reader["Cognome"].ToString();
                        trasgressori.Add(v);
                    }
                }
                catch (Exception ex)
                {
                    Response.Write($"Errore durante il recupero dei dati: {ex.Message}");
                }
                finally
                {
                    conn.Close();
                }
            return View(trasgressori);

        }
    }
}


