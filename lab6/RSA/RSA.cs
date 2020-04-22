using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab6.RSA
{
    public class RSA
    {
        int p = 13;

        int q = 17;

        //var  n = p * q;

        //var fee = (p - 1) * (q - 1);

        //var e = Coprimos(fee, p, q);

        //var d = CalcularD(fee, e);

        //Console.WriteLine($"Llave Publica: {n}  , {d} ");

        //    Console.WriteLine($"Llave Privada: {n}  , {e} ");

     
        private  int CalcularD(int phe_, int e_)
        {
            var Matriz = new int[2, 2]
            {
                   { phe_,phe_ },
                   { e_,1 }
            };

            while (Matriz[1, 0] != 1)
            {
                var Division = Matriz[0, 0] / Matriz[1, 0];

                var NuevaPosicionA = Matriz[0, 0] - (Division * Matriz[1, 0]);

                var NuevaPosicionB = Matriz[0, 1] - (Division * Matriz[1, 1]);

                if (NuevaPosicionB < 0)
                {
                    while (NuevaPosicionB < 0)
                    {
                        NuevaPosicionB += phe_;
                    }
                }

                var AuxiliarA = Matriz[1, 0];
                var AuxiliarB = Matriz[1, 1];

                Matriz[0, 0] = AuxiliarA;
                Matriz[0, 1] = AuxiliarB;
                Matriz[1, 0] = NuevaPosicionA;
                Matriz[1, 1] = NuevaPosicionB;

            }

            return Matriz[1, 1];

        }


        private int Coprimos(int phe, int p_, int q_)
        {
            int contador = 2;
            bool CoprimoEncontrado = false;
            int Coprimo = 0;

            var MultiposPhe = new List<int>();

            var NuevoNumero = phe;

            while (contador < phe)
            {
                while (NuevoNumero % contador == 0)
                {

                    if (!MultiposPhe.Contains(contador))
                    {
                        MultiposPhe.Add(contador);
                    }
                    NuevoNumero = NuevoNumero / contador;
                }
                contador++;
            }
            if (!MultiposPhe.Contains(p))
            {
                MultiposPhe.Add(p);
            }
            if (!MultiposPhe.Contains(q))
            {
                MultiposPhe.Add(q);
            }

            var contador2 = 2;
            while (CoprimoEncontrado == false && contador2 < phe)
            {
                var ContadorCasos = 0;
                foreach (var item in MultiposPhe)
                {
                    if (contador2 % item != 0)
                    {
                        ContadorCasos++;
                    }
                }
                if (ContadorCasos == MultiposPhe.Count)
                {
                    CoprimoEncontrado = true;
                    Coprimo = contador2;

                }


                contador2++;
            }
            return Coprimo;


        }





    }
}
