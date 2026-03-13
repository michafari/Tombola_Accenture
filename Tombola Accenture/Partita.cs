namespace Tombola_Accenture;

public class Partita
{
    public double CostoCartella { get; set; }
    public List<int> IdCartellePrese { get; set; }
    public List<int> NumeriNelSacchetto { get; set; }
    public double SoldiSpesiTotali => IdCartellePrese.Count * CostoCartella;
    public Dictionary<Premio.TipoPremio, double> PremiAssegnati { get; private set; }

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
public void InizializzaPremi()
    {
        PremiAssegnati.Clear();
        double montepremi = SoldiSpesiTotali;

        if (montepremi == 0)
        {
            Console.WriteLine("Nessuna cartella venduta. Impossibile inizializzare i premi.");
            return;
        }

        // 1. Percentuali classiche della tombola
        double premioAmbo = montepremi * 0.08;
        double premioTerno = montepremi * 0.12;
        double premioQuaterna = montepremi * 0.15;
        double premioCinquina = montepremi * 0.25;
        double premioTombola = montepremi * 0.40;

        double fondiDaRedistribuire = 0;
        
        if (premioAmbo < CostoCartella) { fondiDaRedistribuire += premioAmbo; premioAmbo = 0; }
        if (premioTerno < CostoCartella) { fondiDaRedistribuire += premioTerno; premioTerno = 0; }
        if (premioQuaterna < CostoCartella) { fondiDaRedistribuire += premioQuaterna; premioQuaterna = 0; }
        if (premioCinquina < CostoCartella) { fondiDaRedistribuire += premioCinquina; premioCinquina = 0; }
        
        premioTombola += fondiDaRedistribuire;
        
        PremiAssegnati.Add(Premio.TipoPremio.Ambo, Math.Round(premioAmbo, 2));
        PremiAssegnati.Add(Premio.TipoPremio.Terno, Math.Round(premioTerno, 2));
        PremiAssegnati.Add(Premio.TipoPremio.Quaterna, Math.Round(premioQuaterna, 2));
        PremiAssegnati.Add(Premio.TipoPremio.Cinquina, Math.Round(premioCinquina, 2));
        PremiAssegnati.Add(Premio.TipoPremio.Tombola, Math.Round(premioTombola, 2));
        
        Console.WriteLine($"\n=== MONTEPREMI TOTALE: {montepremi:C} ===");
        foreach (var premio in PremiAssegnati)
        {
            if (premio.Value > 0)
            {
                Console.WriteLine($"- {premio.Key}: {premio.Value:C}");
            }
        }
        Console.WriteLine("===================================\n");
    }}