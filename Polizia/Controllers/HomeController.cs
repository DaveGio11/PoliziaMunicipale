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
    }
}


