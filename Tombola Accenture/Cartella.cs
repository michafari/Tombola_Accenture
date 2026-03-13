namespace DefaultNamespace
{
    public class Cartella
    {
        public int Id { get; private set; }
        public TipoPremio PremioMassimo { get; private set; }
        private List<List<Casella>> _righe;

        // COSTRUTTORE
        public Cartella(int id)
        {
            Id = id;
            PremioMassimo = TipoPremio.Nessuno;
            _righe = new List<List<Casella>>();

            List<int> numeriGenerati;

            if (id < 0 && id >= -6)
            {
                // È una sezione del tabellone
                numeriGenerati = GeneraNumeriTabellone(id);
            }
            else if (id > 0)
            {
                // È una cartella normale (usiamo l'ID come Seed)
                numeriGenerati = GeneraNumeriRealistici(id);
            }
            else
            {
                throw new ArgumentException("L'ID non può essere 0 o minore di -6.");
            }

            CostruisciRighe(numeriGenerati);
        }

        private void CostruisciRighe(List<int> numeri)
        {
            int indiceNumero = 0;
            for (int i = 0; i < 3; i++)
            {
                List<Casella> nuovaRiga = new List<Casella>();
                for (int j = 0; j < 5; j++)
                {
                    nuovaRiga.Add(new Casella(numeri[indiceNumero]));
                    indiceNumero++;
                }
                _righe.Add(nuovaRiga);
            }
        }

        // METODO PER LE SEZIONI DEL TABELLONE (ID da -1 a -6)
        private List<int> GeneraNumeriTabellone(int id)
        {
            List<int> numeri = new List<int>();
            
            // Troviamo il numero di partenza in base all'ID
            int start = 0;
            switch (id)
            {
                case -1: start = 1; break;  // 1..5, 11..15, 21..25
                case -2: start = 6; break;  // 6..10, 16..20, 26..30
                case -3: start = 31; break; // 31..35, 41..45, 51..55
                case -4: start = 36; break; // ecc...
                case -5: start = 61; break;
                case -6: start = 66; break;
            }

            // Creiamo le 3 righe matematicamente
            for (int riga = 0; riga < 3; riga++)
            {
                for (int i = 0; i < 5; i++)
                {
                    // Aggiungiamo 10 per ogni riga successiva
                    numeri.Add(start + (riga * 10) + i); 
                }
            }
            return numeri;
        }

        // METODO PER LE CARTELLE GIOCATORI (Con Seed)
        private List<int> GeneraNumeriRealistici(int seed)
        {
            // ECCO LA MAGIA: Passiamo il seed al Random!
            Random random = new Random(seed); 
            
            int[] conteggioPerColonna = new int[9];
            for (int i = 0; i < 9; i++) conteggioPerColonna[i] = 1;

            int numeriDaAggiungere = 6;
            while (numeriDaAggiungere > 0)
            {
                int colonnaCasuale = random.Next(0, 9);
                if (conteggioPerColonna[colonnaCasuale] < 3)
                {
                    conteggioPerColonna[colonnaCasuale]++;
                    numeriDaAggiungere--;
                }
            }

            List<int> numeriFinali = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                int min = (i == 0) ? 1 : i * 10;
                int max = (i == 8) ? 91 : (i + 1) * 10;
                HashSet<int> estrattiInQuestaColonna = new HashSet<int>();

                while (estrattiInQuestaColonna.Count < conteggioPerColonna[i])
                {
                    estrattiInQuestaColonna.Add(random.Next(min, max));
                }
                numeriFinali.AddRange(estrattiInQuestaColonna);
            }

            numeriFinali.Sort();
            return numeriFinali;
        }

        // CERCA NUMERO
        public void SegnaNumero(int numeroEstratto)
        {
            bool trovato = false;
            
            foreach (var riga in _righe)
            {
                foreach (var casella in riga)
                {
                    if (casella.Numero == numeroEstratto && !casella.Coperto)
                    {
                        casella.copri();
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

            foreach (var riga in _righe)
            {
                int copertiInQuestaRiga = 0;
                foreach (var casella in riga)
                {
                    if (casella.Coperto) copertiInQuestaRiga++;
                }

                totaleCopertiCartella += copertiInQuestaRiga;
                
                if (copertiInQuestaRiga == 5 && PremioMassimo < TipoPremio.Cinquina)
                    PremioMassimo = TipoPremio.Cinquina;
                else if (copertiInQuestaRiga == 4 && PremioMassimo < TipoPremio.Quaterna)
                    PremioMassimo = TipoPremio.Quaterna;
                else if (copertiInQuestaRiga == 3 && PremioMassimo < TipoPremio.Terno)
                    PremioMassimo = TipoPremio.Terno;
                else if (copertiInQuestaRiga == 2 && PremioMassimo < TipoPremio.Ambo)
                    PremioMassimo = TipoPremio.Ambo;
            }
            
            if (totaleCopertiCartella == 15)
            {
                PremioMassimo = TipoPremio.Tombola;
            }
        }

        // VISUAIZZAZIONE
        public void Visualizza()
        {
            string titolo = Id < 0 ? $"SEZIONE TABELLONE ({Id})" : $"CARTELLA N° {Id}";
            Console.WriteLine($"\n--- {titolo} --- (Premio: {PremioMassimo})");

            // Se l'ID è negativo, stampa compatta!
            if (Id < 0) 
            {
                foreach (var riga in _righe)
                {
                    foreach (var casella in riga)
                    {
                        if (casella.Coperto) Console.Write("[XX] ");
                        else Console.Write($"{casella.Numero,3}  ");
                    }
                    Console.WriteLine();
                }
            }
            else // Altrimenti stampa realistica 3x9
            {
                Casella[,] grigliaVisiva = new Casella[3, 9];
                List<Casella> tutteLeCaselle = new List<Casella>();
                
                foreach (var riga in _righe) tutteLeCaselle.AddRange(riga);

                foreach (var casella in tutteLeCaselle)
                {
                    int colonnaDecina = (casella.Numero == 90) ? 8 : casella.Numero / 10;
                    for (int riga = 0; riga < 3; riga++)
                    {
                        if (grigliaVisiva[riga, colonnaDecina] == null)
                        {
                            grigliaVisiva[riga, colonnaDecina] = casella;
                            break; 
                        }
                    }
                }

                for (int riga = 0; riga < 3; riga++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        Casella c = grigliaVisiva[riga, col];
                        if (c == null) Console.Write(" --  "); 
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