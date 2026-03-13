namespace Tombola_Accenture
{
    public class Cartella
    {
        public int Id { get; private set; }
        
        // Usiamo nullable (?) per gestire il caso in cui non c'è ancora nessun premio
        public Premio.TipoPremio? PremioMassimo { get; private set; } 
        
        // Usiamo una matrice 3x9 per garantire la forma esatta e fissa
        private Casella[,] _griglia;

        // COSTRUTTORE
        public Cartella(int id)
        {
            Id = id;
            PremioMassimo = null; 
            _griglia = new Casella[3, 9];

            if (id < 0 && id >= -6)
            {
                // È una sezione del tabellone
                GeneraTabellone(id);
            }
            else if (id > 0)
            {
                // È una cartella normale (usiamo l'ID come Seed)
                GeneraNumeriRealistici(id);
            }
            else
            {
                throw new ArgumentException("L'ID non può essere 0 o minore di -6.");
            }
        }

        // METODO PER LE SEZIONI DEL TABELLONE (ID da -1 a -6)
        private void GeneraTabellone(int id)
        {
            int start = 0;
            switch (id)
            {
                case -1: start = 1; break;  
                case -2: start = 6; break;  
                case -3: start = 31; break; 
                case -4: start = 36; break; 
                case -5: start = 61; break;
                case -6: start = 66; break;
            }

            // Posizioniamo i numeri compatti nelle prime 5 colonne
            for (int riga = 0; riga < 3; riga++)
            {
                for (int col = 0; col < 5; col++)
                {
                    _griglia[riga, col] = new Casella(start + (riga * 10) + col);
                }
            }
        }

        // METODO PER LE CARTELLE GIOCATORI (Con Seed)
        private void GeneraNumeriRealistici(int seed)
        {
            Random random = new Random(seed);
            bool[,] layout = new bool[3, 9];
            bool valido = false;

            // 1. Generiamo il layout visivo: 
            // ESATTAMENTE 5 caselle piene per riga.
            // Almeno 1 numero per colonna (che di fatto forza un MAX di 3 per colonna essendo le righe solo 3).
            while (!valido)
            {
                layout = new bool[3, 9];
                int[] conteggioColonna = new int[9];

                for (int r = 0; r < 3; r++)
                {
                    int aggiunti = 0;
                    while (aggiunti < 5)
                    {
                        int c = random.Next(0, 9);
                        if (!layout[r, c]) // Se la cella è vuota, la occupiamo
                        {
                            layout[r, c] = true;
                            conteggioColonna[c]++;
                            aggiunti++;
                        }
                    }
                }

                valido = true;
                // Assicuriamoci che non ci siano colonne vuote (Regola d'oro della tombola realistica)
                for (int c = 0; c < 9; c++)
                {
                    if (conteggioColonna[c] == 0)
                    {
                        valido = false;
                        break;
                    }
                }
            }

            // 2. Popoliamo il layout con i numeri corretti per ciascuna decina
            for (int c = 0; c < 9; c++)
            {
                int numeriInColonna = 0;
                for (int r = 0; r < 3; r++) if (layout[r, c]) numeriInColonna++;

                if (numeriInColonna > 0)
                {
                    int min = (c == 0) ? 1 : c * 10;
                    int max = (c == 8) ? 91 : (c + 1) * 10;
                    
                    List<int> estratti = new List<int>();
                    while (estratti.Count < numeriInColonna)
                    {
                        int n = random.Next(min, max);
                        if (!estratti.Contains(n)) estratti.Add(n);
                    }
                    estratti.Sort(); // I numeri scendono sempre in ordine crescente nella colonna

                    int indiceEstratto = 0;
                    for (int r = 0; r < 3; r++)
                    {
                        if (layout[r, c])
                        {
                            _griglia[r, c] = new Casella(estratti[indiceEstratto]);
                            indiceEstratto++;
                        }
                    }
                }
            }
        }

        // CERCA NUMERO E COPRI
        public void SegnaNumero(int numeroEstratto)
        {
            bool trovato = false;
            
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (_griglia[r, c] != null && _griglia[r, c].Numero == numeroEstratto && !_griglia[r, c].Coperto)
                    {
                        _griglia[r, c].copri();
                        trovato = true;
                    }
                }
            }
            
            if (trovato)
            {
                AggiornaPremio();
            }
        }

        // CALCOLO PREMIO
        private void AggiornaPremio()
        {
            int totaleCopertiCartella = 0;

            for (int r = 0; r < 3; r++)
            {
                int copertiInQuestaRiga = 0;
                for (int c = 0; c < 9; c++)
                {
                    if (_griglia[r, c] != null && _griglia[r, c].Coperto)
                    {
                        copertiInQuestaRiga++;
                    }
                }

                totaleCopertiCartella += copertiInQuestaRiga;
                
                if (copertiInQuestaRiga == 5 && (!PremioMassimo.HasValue || PremioMassimo.Value < Premio.TipoPremio.Cinquina))
                    PremioMassimo = Premio.TipoPremio.Cinquina;
                else if (copertiInQuestaRiga == 4 && (!PremioMassimo.HasValue || PremioMassimo.Value < Premio.TipoPremio.Quaterna))
                    PremioMassimo = Premio.TipoPremio.Quaterna;
                else if (copertiInQuestaRiga == 3 && (!PremioMassimo.HasValue || PremioMassimo.Value < Premio.TipoPremio.Terno))
                    PremioMassimo = Premio.TipoPremio.Terno;
                else if (copertiInQuestaRiga == 2 && (!PremioMassimo.HasValue || PremioMassimo.Value < Premio.TipoPremio.Ambo))
                    PremioMassimo = Premio.TipoPremio.Ambo;
            }
            
            if (totaleCopertiCartella == 15)
            {
                PremioMassimo = Premio.TipoPremio.Tombola;
            }
        }

        // VISUALIZZAZIONE
        public void Visualizza()
        {
            string premioStr = PremioMassimo.HasValue ? PremioMassimo.Value.ToString() : "Nessuno";
            string titolo = Id < 0 ? $"SEZIONE TABELLONE ({Id})" : $"CARTELLA N° {Id}";
            Console.WriteLine($"\n--- {titolo} --- (Premio: {premioStr})");

            // Stampa compatta per le sezioni del tabellone
            if (Id < 0) 
            {
                for (int riga = 0; riga < 3; riga++)
                {
                    for (int col = 0; col < 5; col++)
                    {
                        Casella casella = _griglia[riga, col];
                        if (casella.Coperto) Console.Write("[XX] ");
                        else Console.Write($"{casella.Numero,3}  ");
                    }
                    Console.WriteLine();
                }
            }
            else // Stampa realistica 3x9 per le cartelle
            {
                for (int riga = 0; riga < 3; riga++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        Casella c = _griglia[riga, col];
                        if (c == null) Console.Write(" --  "); // Stampa spazi vuoti
                        else if (c.Coperto) Console.Write("[XX] ");
                        else Console.Write($"{c.Numero,3}  ");
                    }
                    Console.WriteLine(); 
                }
            }
            Console.WriteLine("----------------------------------------------\n");
        }
    }
}