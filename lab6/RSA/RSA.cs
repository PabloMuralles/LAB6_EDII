using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;

namespace lab6.RSA
{
    public class RSA
    {
        //int p = 13;

        //int q = 17;

        //var  n = p * q;

        //var fee = (p - 1) * (q - 1);

        //var e = Coprimos(fee, p, q);

        //var d = CalcularD(fee, e);

        //Console.WriteLine($"Llave Publica: {n}  , {d} ");

        //    Console.WriteLine($"Llave Privada: {n}  , {e} ");

        

         
        public RSA(BigInteger p, BigInteger q)
        {
            var n = p * q;

            var Phe= (p - 1) * (q - 1);

            var e = Coprimos(Phe, p, q);

            var d = CalcularD(Phe, e);



        }

     
        private  BigInteger CalcularD(BigInteger phe_, int e_)
        {
            var Matriz = new BigInteger[2, 2]
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


        private int Coprimos(BigInteger phe, BigInteger p_, BigInteger q_)
        {
            int contador = 2;
            bool CoprimoEncontrado = false;
            var Coprimo = 0;

            var MultiposPhe = new List<BigInteger>();

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
            if (!MultiposPhe.Contains(p_))
            {
                MultiposPhe.Add(p_);
            }
            if (!MultiposPhe.Contains(q_))
            {
                MultiposPhe.Add(q_);
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
