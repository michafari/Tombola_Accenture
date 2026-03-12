namespace Tombola_Accenture;

public class Giocatore
{
    public string nome;
    public List<int> cartelle;
    public double portafogli;

    public void acquista_cartella(List<int> cartelle_disponibili, double costo_cartella)
    {
        Console.WriteLine("Scegli il numero della cartella");
        Console.WriteLine("Questa è la lista delle cartelle disponibili");
        int cartella = int.Parse(Console.ReadLine());
        if (cartelle_disponibili.Contains(cartella))
        {
            
            cartelle.Add(cartella);
            portafogli -= costo_cartella;
            Console.WriteLine($"{nome}, hai acquistato la cartella numero {cartella}");
            Console.WriteLine($"Ora il tuo saldo è {portafogli}");
            Console.WriteLine($"Le tue cartelle sono {cartelle}");
        }
        else
        {
            Console.WriteLine("La cartella che hai scelto non è disponibile");
            Console.WriteLine("Scegli un'altra cartella");
        }
    }

    public void incassa_premi(double premio)
    {
        Console.WriteLine($"complimenti {nome}, hai ottenuto una vincita di {premio} da una delle tue cartelle");
        portafogli += premio;
        Console.WriteLine($"Ora il tuo saldo è {portafogli}");
    }
    
}