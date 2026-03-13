namespace Tombola_Accenture
{
    public class Partita
    {
        public double CostoCartella { get; set; }
        public List<int> IdCartellePrese { get; set; }
        public List<int> NumeriNelSacchetto { get; set; }
        
        // Calcolato dinamicamente
        public double SoldiSpesiTotali => IdCartellePrese.Count * CostoCartella;

        // Lista diretta ed efficiente di oggetti Premio (sostituisce il vecchio Dictionary)
        public List<Premio> PremiAttivi { get; private set; }

        private Random _random = new Random();

        public Partita(double costoCartella)
        {
            CostoCartella = costoCartella;
            IdCartellePrese = new List<int>();
            NumeriNelSacchetto = new List<int>();
            PremiAttivi = new List<Premio>();
            
            for (int i = 1; i <= 90; i++)
            {
                NumeriNelSacchetto.Add(i);
            }
        }
        
        public int Estrai_numero()
        {
            if (NumeriNelSacchetto.Count == 0) return -1;
            
            int indice = _random.Next(NumeriNelSacchetto.Count);
            int numeroEstratto = NumeriNelSacchetto[indice];
            
            NumeriNelSacchetto.RemoveAt(indice);

            return numeroEstratto;
        }

        public void InizializzaPremi()
        {
            PremiAttivi.Clear();
            double montepremi = SoldiSpesiTotali;

            if (montepremi == 0)
            {
                Console.WriteLine("Nessuna cartella venduta. Impossibile inizializzare i premi.");
                return;
            }

            // 1. Percentuali classiche della tombola (nessun limite minimo)
            double premioAmbo = montepremi * 0.08;
            double premioTerno = montepremi * 0.12;
            double premioQuaterna = montepremi * 0.15;
            double premioCinquina = montepremi * 0.25;
            double premioTombola = montepremi * 0.40;

            // 2. Inizializziamo gli oggetti Premio veri e propri e li inseriamo in lista
            PremiAttivi.Add(new Premio(Premio.TipoPremio.Ambo, Math.Round(premioAmbo, 2)));
            PremiAttivi.Add(new Premio(Premio.TipoPremio.Terno, Math.Round(premioTerno, 2)));
            PremiAttivi.Add(new Premio(Premio.TipoPremio.Quaterna, Math.Round(premioQuaterna, 2)));
            PremiAttivi.Add(new Premio(Premio.TipoPremio.Cinquina, Math.Round(premioCinquina, 2)));
            PremiAttivi.Add(new Premio(Premio.TipoPremio.Tombola, Math.Round(premioTombola, 2)));
            
            // Stampa riepilogo
            Console.WriteLine($"\n=== MONTEPREMI TOTALE: {montepremi:C} ===");
            foreach (var premio in PremiAttivi)
            {
                Console.WriteLine($"- {premio.Tipo}: {premio.Valore:C}");
            }
            Console.WriteLine("===================================\n");
        }
    }
}