namespace Tombola_Accenture
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("      BENVENUTI ALLA TOMBOLA!           ");
            Console.WriteLine("========================================");

            // 1. CREAZIONE DEL PARTY
            Party party = new Party();
            Console.WriteLine("\n--- CREAZIONE PARTY ---");
            while (true)
            {
                Console.Write("Inserisci il nome del giocatore (oppure premi Invio per terminare): ");
                string nome = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nome)) break;

                Giocatore g = new Giocatore(nome, 50.00);
                party.Aggiungi_giocatore(g);
            }

            if (party.Giocatori.Count == 0)
            {
                Console.WriteLine("Nessun giocatore inserito. Uscita dal gioco.");
                return;
            }

            // 2. STABILIRE COSTO CARTELLA
            Console.WriteLine("\n--- IMPOSTAZIONI PARTITA ---");
            double costoCartella = 0;
            while (costoCartella <= 0)
            {
                Console.Write("Inserisci il costo di una singola cartella (es. 2,50): ");
                if (double.TryParse(Console.ReadLine(), out double costo))
                {
                    costoCartella = costo;
                }
                else
                {
                    Console.WriteLine("Valore non valido. Riprova.");
                }
            }
            
            Partita partita = new Partita(costoCartella);
            Tabellone tabellone = new Tabellone();

            // 3. ACQUISTO CARTELLE
            Console.WriteLine("\n--- ACQUISTO CARTELLE ---");
            int idCartellaCorrente = 1; 
            
            foreach (var giocatore in party.Giocatori)
            {
                Console.WriteLine($"\nTurno di {giocatore.Nome} (Budget: {giocatore.Portafogli:C})");
                Console.Write("Quante cartelle vuoi comprare? ");
                if (int.TryParse(Console.ReadLine(), out int numCartelle))
                {
                    for (int i = 0; i < numCartelle; i++)
                    {
                        giocatore.Compra_cartella(idCartellaCorrente, partita);
                        idCartellaCorrente++;
                    }
                }
            }

            // 4. CREAZIONE DEI PREMI
            Console.WriteLine("\n--- INIZIALIZZAZIONE PREMI ---");
            partita.InizializzaPremi();

            // Recuperiamo la lista degli oggetti Premio già creata (più efficiente, niente LINQ o Dictionary!)
            var premiAttivi = partita.PremiAttivi;

            if (premiAttivi.Count == 0)
            {
                Console.WriteLine("Non ci sono soldi sufficienti nel montepremi per giocare. Partita annullata.");
                return;
            }

            int indicePremioAttuale = 0;
            Premio premioInPalio = premiAttivi[indicePremioAttuale];
            Console.WriteLine($"\n-> SI GIOCA PER: {premioInPalio.Tipo.ToString().ToUpper()} ({premioInPalio.Valore:C}) <-");

            // 5. INIZIO PARTITA
            Console.WriteLine("\n--- INIZIO ESTRAZIONE ---");
            bool partitaFinita = false;

            while (!partitaFinita)
            {
                Console.WriteLine("\nPremi INVIO per estrarre un numero (o scrivi 'esci' per terminare)...");
                string input = Console.ReadLine();
                if (input?.ToLower() == "esci") break;

                int estratto = partita.Estrai_numero();
                if (estratto == -1) break; 

                Console.WriteLine($"\n========================================");
                Console.WriteLine($"   NUMERO ESTRATTO: {estratto} !!!");
                Console.WriteLine($"========================================");

                tabellone.SegnaNumero(estratto);
                tabellone.Visualizza();

                List<Giocatore> vincitoriTurno = new List<Giocatore>();

                Console.WriteLine("--- AGGIORNAMENTO CARTELLE ---");
                foreach (var giocatore in party.Giocatori)
                {
                    bool haVintoPremioCorrente = false;
                    foreach (var cartella in giocatore.Cartelle)
                    {
                        cartella.SegnaNumero(estratto);
                        cartella.Visualizza();

                        // Confrontiamo direttamente le enumerazioni TipoPremio (grazie all'enum int order)
                        if (cartella.PremioMassimo.HasValue && cartella.PremioMassimo.Value >= premioInPalio.Tipo)
                        {
                            haVintoPremioCorrente = true;
                        }
                    }
                    
                    if (haVintoPremioCorrente)
                    {
                        vincitoriTurno.Add(giocatore);
                    }
                }

                if (vincitoriTurno.Count > 0)
                {
                    Console.WriteLine($"\n!!! ABBIAMO DEI VINCITORI PER IL PREMIO: {premioInPalio.Tipo.ToString().ToUpper()} !!!");
                    
                    double premioDaDividere = premioInPalio.Valore / vincitoriTurno.Count;

                    foreach (var vincitore in vincitoriTurno)
                    {
                        Console.WriteLine($"> {vincitore.Nome} fa {premioInPalio.Tipo}!");
                        vincitore.Incassa_premio(premioDaDividere);
                    }

                    indicePremioAttuale++;
                    
                    if (indicePremioAttuale >= premiAttivi.Count)
                    {
                        Console.WriteLine("\n!!! TOMBOLA RAGGIUNTA! LA PARTITA E' FINITA !!!");
                        partitaFinita = true;
                    }
                    else
                    {

                        premioInPalio = premiAttivi[indicePremioAttuale];
                        Console.WriteLine($"\n-> ORA SI GIOCA PER: {premioInPalio.Tipo.ToString().ToUpper()} ({premioInPalio.Valore:C}) <-");
                    }
                }
            }

            // 6. RIEPILOGO FINALE
            Console.WriteLine("\n========================================");
            Console.WriteLine("          RIEPILOGO FINALE              ");
            Console.WriteLine("========================================");
            foreach (var giocatore in party.Giocatori)
            {
                Console.WriteLine($"- {giocatore.Nome}: Budget finale = {giocatore.Portafogli:C} (Bilancio: {giocatore.Portafogli - 50.00:C})");
            }
            Console.WriteLine("========================================");
            Console.WriteLine("Grazie per aver giocato!");
            Console.ReadLine();
        }
    }
}