namespace Tombola_Accenture;

public class Premio
{
    public enum TipoPremio
    {
        Nessuno,
        Ambo,
        Terno,
        Quaterna,
        Cinquina,
        Tombola
    }
    
    public TipoPremio Tipo { get; set; }
    public double Valore { get; set; }
    
    public Premio(TipoPremio tipo, double valore)
    {
        Tipo = tipo;
        Valore = valore;
    }
}