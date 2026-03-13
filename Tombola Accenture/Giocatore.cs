namespace Tombola_Accenture
{
    public class Giocatore
    {
        public string Nome { get; set; }
        
        public double Bilancio { get; set; } 
        public List<Cartella> Cartelle { get; set; }

        public Giocatore(string nome)
        {
            Nome = nome;
            Bilancio = 0.0;
            Cartelle = new List<Cartella>();
        }

        public void Incassa_premio(double valorePremio)
        {
            Bilancio += valorePremio;
        }

        public bool Compra_cartella(int id, Partita partita)
        {
            // Rimosso il controllo "if (Portafogli < costo)" -> il budget è illimitato

            if (partita.IdCartellePrese.Contains(id))
            {
                Console.WriteLine($"La cartella {id} è già stata acquistata da un altro giocatore.");
                return false;
            }
            
            // Scaliamo il costo dal bilancio (che diventerà negativo)
            Bilancio -= partita.CostoCartella;
            
            Cartella nuovaCartella = new Cartella(id);
            Cartelle.Add(nuovaCartella);
            partita.IdCartellePrese.Add(id);
                
            Console.WriteLine($"{Nome} ha acquistato la cartella {id}. Bilancio parziale: {Bilancio:C}");
            
            return true;
        }
    }
}