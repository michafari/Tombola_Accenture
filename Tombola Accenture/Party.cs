namespace Tombola_Accenture;

public class Party
{
    public List<Giocatore> Giocatori { get; set; }

    public Party()
    {
        Giocatori = new List<Giocatore>();
    }

    // METODO
    public void Aggiungi_giocatore(Giocatore giocatore)
    {
        if (!Giocatori.Contains(giocatore))
        {
            Giocatori.Add(giocatore);
            Console.WriteLine($"{giocatore.Nome} si è unito alla partita.");
        }
        else
        {
            Console.WriteLine($"{giocatore.Nome} è già presente nella partita.");
        }
    }

    // METODO
    public void Rimuovi_giocatore(Giocatore giocatore)
    {
        Giocatori.Remove(giocatore);
        Console.WriteLine($"{giocatore.Nome} ha lasciato la partita.");
    }
}