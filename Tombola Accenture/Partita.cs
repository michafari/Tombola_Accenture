namespace Tombola_Accenture;

public class Partita
{
    public double costo_cartelle;
    public string Party;
    public string tabellone;
    public List<int> numeri_usciti = new List<int>(); 
    public List<string> premi = new List<string>();

    public void inizio_partita()
    {
        Console.WriteLine("Via all'inizio della partita!");
    }

    public void estrazione_numero(out int numero_estratto)
    {
        Random rnd = new Random();
        
        do
        {
            numero_estratto = rnd.Next(1, 91);
        } while (numeri_usciti.Contains(numero_estratto));
        
        Console.WriteLine($"!! è uscito il numero {numero_estratto}");
        numeri_usciti.Add(numero_estratto);
        Console.WriteLine("RECAP numeri usciti: ");
        foreach (var num in numeri_usciti)
        {
            Console.Write(num + " ");
        }
        Console.WriteLine();
        
        
    }
}