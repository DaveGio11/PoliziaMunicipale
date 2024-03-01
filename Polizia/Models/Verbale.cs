using System;

namespace Polizia.Models
{
    public class Verbale
    {
        public int IdVerbale { get; set; }
        public DateTime DataViolazione { get; set; }
        public string IndirizzoViolazione { set; get; }
        public string IdentificativoAgente { get; set; }
        public DateTime DataVerbale { get; set; }
        public int Importo { get; set; }
        public int DecurtamentoPunti { get; set; }
        public int IdViolazione { get; set; }
        public int IdAnagrafica { get; set; }

        public string Cognome { get; set; }
        public string Nome { get; set; }

        public string Descrizione_Verbale { get; set; }
    }
}