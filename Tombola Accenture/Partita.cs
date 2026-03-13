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

            // 1. Percentuali classiche della tombola
            double premioAmbo = montepremi * 0.08;
            double premioTerno = montepremi * 0.12;
            double premioQuaterna = montepremi * 0.15;
            double premioCinquina = montepremi * 0.25;
            double premioTombola = montepremi * 0.40;

            double fondiDaRedistribuire = 0;
            
            // 2. Controllo valore minimo del premio
            if (premioAmbo < CostoCartella) { fondiDaRedistribuire += premioAmbo; premioAmbo = 0; }
            if (premioTerno < CostoCartella) { fondiDaRedistribuire += premioTerno; premioTerno = 0; }
            if (premioQuaterna < CostoCartella) { fondiDaRedistribuire += premioQuaterna; premioQuaterna = 0; }
            if (premioCinquina < CostoCartella) { fondiDaRedistribuire += premioCinquina; premioCinquina = 0; }
            
            // 3. Spostiamo i fondi residui nel premio massimo
            premioTombola += fondiDaRedistribuire;
            
            // 4. Inizializziamo gli oggetti Premio veri e propri solo se validi e li inseriamo in lista
            if (premioAmbo > 0) PremiAttivi.Add(new Premio(Premio.TipoPremio.Ambo, Math.Round(premioAmbo, 2)));
            if (premioTerno > 0) PremiAttivi.Add(new Premio(Premio.TipoPremio.Terno, Math.Round(premioTerno, 2)));
            if (premioQuaterna > 0) PremiAttivi.Add(new Premio(Premio.TipoPremio.Quaterna, Math.Round(premioQuaterna, 2)));
            if (premioCinquina > 0) PremiAttivi.Add(new Premio(Premio.TipoPremio.Cinquina, Math.Round(premioCinquina, 2)));
            if (premioTombola > 0) PremiAttivi.Add(new Premio(Premio.TipoPremio.Tombola, Math.Round(premioTombola, 2)));
            
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