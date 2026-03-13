namespace Tombola_Accenture;

public class Casella
{
    public int Numero;
    public bool Coperto;

    public Casella(int numero, bool coperto = false)
    {
        Numero = numero;
        Coperto = coperto;
    }

    public void copri()
    {
        Coperto = true;
    }
}