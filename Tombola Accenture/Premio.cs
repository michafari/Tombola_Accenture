namespace Tombola_Accenture;

public class Premio
{
    public enum TipoPremio
    {
        Ambo,
        Terno,
        Quaterna,
        Cinquina,
        Tombola
    }
    
    // COSTRUTTORE
    public class Premio
    {
        public TipoPremio Tipo { get; set; }
        public double Valore { get; set; }

        public Premio(TipoPremio tipo, double valore)
        {
            Tipo = tipo;
            Valore = valore;
        }
    }
}