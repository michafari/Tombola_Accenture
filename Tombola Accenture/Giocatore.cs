namespace Tombola_Accenture;


public class Giocatore
{
    public string Nome { get; set; }
    public double Portafogli { get; set; }
    public List<Cartella> Cartelle { get; set; }

    public Giocatore(string nome, double portafogliIniziale)
    {
        Nome = nome;
        Portafogli = portafogliIniziale;
        Cartelle = new List<Cartella>();
    }

    // METODO
    public void Incassa_premio(double valorePremio)
    {
        Portafogli += valorePremio;
    }

    // METODO
    public bool Compra_cartella(int id, Partita partita)
    {
        if (Portafogli < partita.CostoCartella)
        {
            Console.WriteLine($"{Nome} non ha abbastanza soldi per comprare la cartella {id}.");
            return false;
        }
        
        if (partita.IdCartellePrese.Contains(id))
        {
            Console.WriteLine($"La cartella {id} è già stata acquistata da un altro giocatore.");
            return false;
        }
        
        Portafogli -= partita.CostoCartella;
        Cartelle.Add(cartella = new Cartella(id));
        partita.IdCartellePrese.Add(id);
            
        Console.WriteLine($"{Nome} ha acquistato la cartella {id}. Saldo residuo: {Portafogli:C}");
        cartella.Visualizza()
        return true;
    }
    
}