namespace Tombola_Accenture;

public class Partita
{
    public double CostoCartella { get; set; }
    public List<int> IdCartellePrese { get; set; }
    public List<int> NumeriNelSacchetto { get; set; }

    private Random _random = new Random();

    public Partita(double costoCartella)
    {
        CostoCartella = costoCartella;
        IdCartellePrese = new List<int>();
        NumeriNelSacchetto = new List<int>();
        
        for (int i = 1; i <= 90; i++)
        {
            NumeriNelSacchetto.Add(i);
        }
    }
    
    public int Estrai_numero()
    {
        int indice = _random.Next(NumeriNelSacchetto.Count);
        int numeroEstratto = NumeriNelSacchetto[indice];
        
        NumeriNelSacchetto.RemoveAt(indice);

        return numeroEstratto;
    }
}