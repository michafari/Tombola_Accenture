using System;
using System.Collections.Generic;
using System.Linq;

namespace Tombola_Accenture
{
    class Program
    {
        static void Main(string[] args)
        {
            Party party = new Party();
            Partita partita = null;
            Tabellone tabellone = new Tabellone();
            
            double costoCartella = 0;
            int idCartellaCorrente = 1; // Contatore globale per ID univoci
            List<int> cartelleDisponibili = Enumerable.Range(1, 90).ToList(); // Simula un pool di 90 cartelle

            bool avviaGioco = false;

            // ==========================================
            // MENU PRINCIPALE
            // ==========================================
            while (!avviaGioco)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("      BENVENUTI ALLA TOMBOLA!           ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Scegliere costo cartelle");
                Console.WriteLine("2. Aggiungere giocatori");
                Console.WriteLine("3. Rimuovere giocatori");
                Console.WriteLine("4. Visualizza party");
                Console.WriteLine("5. GIOCA!");
                Console.WriteLine("========================================");
                Console.Write("Scegli un'opzione: ");
                
                string scelta = Console.ReadLine();

                switch (scelta)
                {
                    case "1":
                        Console.Write("\nInserisci il costo di una singola cartella (es. 2,50): ");
                        if (double.TryParse(Console.ReadLine(), out double costo) && costo > 0)
                        {
                            costoCartella = costo;
                            partita = new Partita(costoCartella);
                            Console.WriteLine($"Costo impostato a {costoCartella:C}. Partita inizializzata.");
                        }
                        else
                        {
                            Console.WriteLine("Valore non valido. Premi Invio per riprovare.");
                        }
                        Console.ReadLine();
                        break;

                    case "2":
                        if (partita == null)
                        {
                            Console.WriteLine("\nERRORE: Devi prima impostare il costo delle cartelle (Opzione 1)!");
                            Console.ReadLine();
                            break;
                        }

                        Console.Write("\nInserisci il nome del nuovo giocatore: ");
                        string nome = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nome)) break;

                        Giocatore nuovoGiocatore = new Giocatore(nome);
                        bool menuGiocatoreAttivo = true;

                        // MENU AGGIUNTA GIOCATORE
                        while (menuGiocatoreAttivo)
                        {
                            Console.Clear();
                            Console.WriteLine($"--- MENU GIOCATORE: {nuovoGiocatore.Nome} ---");
                            Console.WriteLine("1. Scegli di essere il tabellone (6 cartelle)");
                            Console.WriteLine("2. Scegli le cartelle");
                            Console.WriteLine("3. Esci e salva giocatore");
                            Console.Write("Scelta: ");
                            
                            string sceltaGiocatore = Console.ReadLine();

                            if (sceltaGiocatore == "1")
                            {
                                // Assegna 6 cartelle al giocatore usando gli ID speciali per il tabellone (-1 a -6)
                                Console.WriteLine("\nAssegnazione delle 6 cartelle del tabellone...");
                                for (int i = -1; i >= -6; i--)
                                {
                                    nuovoGiocatore.Compra_cartella(i, partita);
                                }
                                Console.WriteLine("Cartelle del tabellone assegnate con successo! Premi Invio.");
                                Console.ReadLine();
                                menuGiocatoreAttivo = false; // Forza l'uscita dopo aver preso il tabellone
                            }
                            else if (sceltaGiocatore == "2")
                            {
                                // MENU SCELTA CARTELLE
                                bool menuCartelleAttivo = true;
                                while (menuCartelleAttivo)
                                {
                                    Console.Clear();
                                    Console.WriteLine("--- CARTELLE DISPONIBILI ---");
                                    // Mostra un'anteprima degli ID disponibili
                                    var disponibiliDaMostrare = cartelleDisponibili.Take(20);
                                    Console.WriteLine("ID Disponibili: " + string.Join(", ", disponibiliDaMostrare) + (cartelleDisponibili.Count > 20 ? "..." : ""));
                                    
                                    Console.Write("\nInserisci l'ID della cartella da visualizzare/acquistare (oppure 'esci' per tornare indietro): ");
                                    string inputCartella = Console.ReadLine();

                                    if (inputCartella.ToLower() == "esci")
                                    {
                                        menuCartelleAttivo = false;
                                    }
                                    else if (int.TryParse(inputCartella, out int idScelto) && cartelleDisponibili.Contains(idScelto))
                                    {
                                        Console.WriteLine($"\n[Anteprima Cartella ID: {idScelto}]");
                                        Console.WriteLine("Generazione anteprima in corso...");
                                        new Cartella(idScelto).Visualizza();
                                        
                                        Console.Write($"Vuoi confermare l'acquisto della cartella {idScelto}? (S/N): ");
                                        if (Console.ReadLine()?.ToUpper() == "S")
                                        {
                                            nuovoGiocatore.Compra_cartella(idScelto, partita);
                                            cartelleDisponibili.Remove(idScelto);
                                            // Aggiorniamo il contatore globale
                                            if (idScelto >= idCartellaCorrente) idCartellaCorrente = idScelto + 1;
                                            
                                            Console.WriteLine("Cartella acquistata! Premi Invio.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Acquisto annullato. Premi Invio.");
                                        }
                                        Console.ReadLine();
                                    }
                                    else
                                    {
                                        Console.WriteLine("ID non valido o non disponibile. Premi Invio.");
                                        Console.ReadLine();
                                    }
                                }
                            }
                            else if (sceltaGiocatore == "3")
                            {
                                menuGiocatoreAttivo = false;
                            }
                        }

                        party.Aggiungi_giocatore(nuovoGiocatore);
                        Console.WriteLine($"\nGiocatore {nuovoGiocatore.Nome} aggiunto al party! Premi Invio.");
                        Console.ReadLine();
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("--- RIMUOVI GIOCATORE ---");
                        if (party.Giocatori.Count == 0)
                        {
                            Console.WriteLine("Nessun giocatore presente.");
                        }
                        else
                        {
                            for (int i = 0; i < party.Giocatori.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {party.Giocatori[i].Nome}");
                            }
                            Console.Write("\nInserisci il numero del giocatore da rimuovere (o 'esci'): ");
                            string inputRemove = Console.ReadLine();
                            if (int.TryParse(inputRemove, out int indexToRemove) && indexToRemove > 0 && indexToRemove <= party.Giocatori.Count)
                            {
                                string nomeRimosso = party.Giocatori[indexToRemove - 1].Nome;
                                party.Giocatori.RemoveAt(indexToRemove - 1);
                                Console.WriteLine($"Giocatore {nomeRimosso} rimosso con successo.");
                            }
                        }
                        Console.ReadLine();
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("--- PARTY ATTUALE ---");
                        if (party.Giocatori.Count == 0)
                        {
                            Console.WriteLine("Il party è vuoto.");
                        }
                        else
                        {
                            foreach (var g in party.Giocatori)
                            {
                                Console.WriteLine($"> {g.Nome.ToUpper()}");
                                var idsCartelle = g.Cartelle.Select(c => c.Id.ToString()).ToList(); 
                                string listaIds = idsCartelle.Count > 0 ? string.Join(", ", idsCartelle) : "Nessuna cartella";
                                Console.WriteLine($"  Cartelle (ID): {listaIds}");
                            }
                        }
                        Console.WriteLine("\nPremi Invio per tornare al menu.");
                        Console.ReadLine();
                        break;

                    case "5":
                        if (partita == null || costoCartella <= 0)
                        {
                            Console.WriteLine("\nERRORE: Costo cartelle non impostato! Torna al punto 1.");
                            Console.ReadLine();
                        }
                        else if (party.Giocatori.Count == 0)
                        {
                            Console.WriteLine("\nERRORE: Non ci sono giocatori nel party! Aggiungi giocatori al punto 2.");
                            Console.ReadLine();
                        }
                        else
                        {
                            avviaGioco = true; // Esce dal loop del menu
                        }
                        break;

                    default:
                        Console.WriteLine("\nScelta non valida. Premi Invio.");
                        Console.ReadLine();
                        break;
                }
            }


            // ==========================================
            // FASE DI GIOCO (Post-Menu)
            // ==========================================
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("          INIZIO PARTITA!               ");
            Console.WriteLine("========================================");

            Console.WriteLine("\n--- INIZIALIZZAZIONE PREMI ---");
            partita.InizializzaPremi();

            var premiAttivi = partita.PremiAttivi;

            if (premiAttivi.Count == 0)
            {
                Console.WriteLine("Non ci sono soldi sufficienti nel montepremi per giocare. Partita annullata.");
                Console.ReadLine();
                return;
            }

            int indicePremioAttuale = 0;
            Premio premioInPalio = premiAttivi[indicePremioAttuale];
            Console.WriteLine($"\n-> SI GIOCA PER: {premioInPalio.Tipo.ToString().ToUpper()} ({premioInPalio.Valore:C}) <-");

            Console.WriteLine("\n--- INIZIO ESTRAZIONE ---");
            bool partitaFinita = false;

            while (!partitaFinita)
            {
                Console.WriteLine("\nPremi INVIO per estrarre un numero (o scrivi 'esci' per terminare)...");
                string input = Console.ReadLine();
                if (input?.ToLower() == "esci") break;

                int estratto = partita.Estrai_numero();
                if (estratto == -1) break; 

                Console.WriteLine($"\n==================================================");
                Console.WriteLine($"              NUMERO ESTRATTO: {estratto} !!!              ");
                Console.WriteLine($"==================================================");

                tabellone.SegnaNumero(estratto);
                tabellone.Visualizza(estratto);

                List<Giocatore> vincitoriTurno = new List<Giocatore>();
                bool almenoUnaCartellaColpita = false;

                foreach (var giocatore in party.Giocatori)
                {
                    bool haVintoPremioCorrente = false;
                    foreach (var cartella in giocatore.Cartelle)
                    {
                        // cartella.SegnaNumero() controlla se il numero c'è e gestisce le coperture 
                        bool cartellaColpita = cartella.SegnaNumero(estratto);
                        
                        if (cartellaColpita)
                        {
                            if (!almenoUnaCartellaColpita)
                            {
                                Console.WriteLine("\n--- CARTELLE COLPITE IN QUESTO TURNO ---");
                                almenoUnaCartellaColpita = true;
                            }
                            cartella.Visualizza(giocatore.Nome, estratto);
                        }

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
                        Console.WriteLine($"> {vincitore.Nome} vince {premioInPalio.Tipo} e incassa {premioDaDividere:C}!");
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

            // ==========================================
            // RIEPILOGO FINALE
            // ==========================================
            Console.WriteLine("\n========================================");
            Console.WriteLine("          RIEPILOGO FINALE              ");
            Console.WriteLine("========================================");
            foreach (var giocatore in party.Giocatori)
            {
                string esito = giocatore.Bilancio >= 0 ? "in PROFITTO" : "in PERDITA";
                Console.WriteLine($"- {giocatore.Nome}: Bilancio finale = {giocatore.Bilancio:C} ({esito})");
            }
            Console.WriteLine("========================================");
            Console.WriteLine("Grazie per aver giocato!");
            Console.ReadLine();
        }
    }
}