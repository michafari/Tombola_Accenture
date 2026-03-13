using System;

namespace Tombola_Accenture
{
    class Program
    {
        static void Main(string[] args)
        {
            Partita miaPartita = new Partita();
            while (true)
            {
                miaPartita.estrazione_numero(out int num);
            }
        }
    }
}
