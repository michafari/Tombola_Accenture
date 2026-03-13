namespace Tombola_Accenture;

public class Tabellone
{
    private Casella[,] _griglia = new Casella[9, 10];
    
    // COSTRUTTORE: matrice 9x10 di Casella()
    public Tabellone()
    {
        InizializzaTabellone();
    }

    private void InizializzaTabellone()
    {
        int contatore = 1;
        for (int riga = 0; riga < 9; riga++)
        {
            for (int col = 0; col < 10; col++)
            {
                _griglia[riga, col] = new Casella(contatore);
                contatore++;
            }
        }
    }


    // METODO: ESTRAI NUMERO
    public void SegnaNumero(int numero)
    {
        int riga = (numero - 1) / 10;
        int col = (numero - 1) % 10;
        
        _griglia[riga, col].copri();
    }
    
    public void Visualizza(int ultimoEstratto = -1)
    {
        Console.WriteLine("\n=============== TABELLONE  TOMBOLA ===============");
        for (int riga = 0; riga < 9; riga++)
        {
            for (int col = 0; col < 10; col++)
            {
                Casella casellaAttuale = _griglia[riga, col];
                
                if (casellaAttuale.Coperto)
                {
                    // Colora di Giallo se appena estratto, di Rosso se estratto in passato
                    if (casellaAttuale.Numero == ultimoEstratto) Console.ForegroundColor = ConsoleColor.Yellow;
                    else Console.ForegroundColor = ConsoleColor.Red;
                    
                    Console.Write($"{casellaAttuale.Numero,3}  "); 
                    Console.ResetColor(); // Ripristina il colore di default
                }
                else
                {
                    // Se non è coperta, stampiamo il numero. 
                    Console.Write($"{casellaAttuale.Numero,3}  "); 
                }
            }
            Console.WriteLine(); // Va a capo alla fine di ogni riga (ogni 10 numeri)
        }
        Console.WriteLine("==================================================\n");
    }
}
