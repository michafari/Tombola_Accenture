namespace Tombola_Accenture;

public class Party
{
    public string nome;
    public List<string> giocatori;

    public void aggiungi_giocatore()
    {
        Console.WriteLine("Scrivi il tuo nickname per unirti al Party");
        string giocatore =  Console.ReadLine();
        if (giocatori.Contains(giocatore))
        {
            Console.WriteLine("Il nickname scelto è già in uso in questo Party");
            
        }
        else
        {
            giocatori.Add(giocatore);
            Console.WriteLine($"complimenti {giocatore}, sei stato aggiunto al Party {nome}");
            Console.WriteLine($"Ora il Party è composto dai giocatori {giocatori}");
        }

    }

    public void rimuovi_giocatore()
    {
        Console.WriteLine("Scrivi il nickname del giocatore che vuoi rimuovere");  
        string giocatore = Console.ReadLine();
        if (giocatore.Contains(giocatore))
        {
            giocatori.Remove(giocatore);
            Console.WriteLine($"Il giocatore {giocatore} è stato rimosso");
            Console.WriteLine($"Ora il Party è composto dai giocatori {giocatori}");
        }
        else
        {
            Console.WriteLine($"il giocatore {giocatore} non è presente nel Party");
        }
    }
}