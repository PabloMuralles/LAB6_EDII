using System;
using System.Collections.Generic;
using System.Numerics;

namespace lab6.RSA
{
    public class RSA
    {

        private BigInteger p;
        private BigInteger q;

        public string Llaves = string.Empty;
        public RSA()
        {

            p = GenerarNumeroPrimoRandom();

            q = GenerarNumeroPrimoRandom();



            while (p == q || p * q > 255)
            {
                p = GenerarNumeroPrimoRandom();
                q = GenerarNumeroPrimoRandom();
            }

            //var p = new BigInteger(31);
            //var q = new BigInteger(3);

            var n = p * q;

            var Phe = (p - 1) * (q - 1);

            var e = Coprimos(Phe, p, q);



            var d = CalcularD(Phe, e);


            var Prueva = ValidarNumeros(e, d, Phe);

            while (Prueva == false || e == d)
            {
                p = GenerarNumeroPrimoRandom();
                q = GenerarNumeroPrimoRandom();
                while (p == q || p * q > 255)
                {
                    p = GenerarNumeroPrimoRandom();
                    q = GenerarNumeroPrimoRandom();
                }

                n = p * q;

                Phe = (p - 1) * (q - 1);



                e = Coprimos(Phe, p, q);
                d = CalcularD(Phe, e);
            }

            Llaves = $"RSA: (Llave Publica: {n}, {e}) y (Llave Privada: {n}, {d}) , {p}, {q} ";

            //Llaves = $"RSA: Llave Publica: {n}, {e}  ";

            Descifrar.Instance.RecibirLlavePrivada(n, d);


        }






        private bool ValidarNumeros(BigInteger P, BigInteger D, BigInteger PHE)
        {
            var Multiplicacion = BigInteger.Multiply(D, P);
            var Mod = BigInteger.ModPow(Multiplicacion, 1, PHE);
            if (Mod == 1)
            {
                return true;
            }
            return false;
        }

        private BigInteger GenerarNumeroPrimoRandom()
        {
            var Random = new Random();

            var Number = new BigInteger(Random.Next(3, 50));

            while (!EsPrimo(Number))
            {
                Number = new BigInteger(Random.Next(3, 50));

            }

            return Number;

        }
        private BigInteger GenerarPQ()
        {
            var NumeroRandom = string.Empty;
            var Random = new Random();
            var Numero = new BigInteger();

            for (int i = 0; i < 6; i++)
            {
                NumeroRandom += Convert.ToString(Random.Next(0, 6));
            }
            Numero = BigInteger.Parse(NumeroRandom);

            while (!EsPrimo(Numero))
            {
                NumeroRandom = string.Empty;
                for (int i = 0; i < 6; i++)
                {
                    NumeroRandom += Convert.ToString(Random.Next(0, 9));
                }
                Numero = BigInteger.Parse(NumeroRandom);
            }



            return Numero;

        }






        private bool EsPrimoGrande(BigInteger Number)
        {
            var Raiz = Math.Exp(BigInteger.Log(Number) / 2);

            for (int i = 2; i < Raiz; i++)
            {
                if (Number % i == 0)
                {
                    return false;

                }

            }
            return true;
        }


        private bool EsPrimo(BigInteger Number)
        {
            for (int i = 2; i < Number; i++)
            {
                if (Number % i == 0)
                {
                    return false;

                }

            }
            return true;
        }



        private BigInteger CalcularD(BigInteger phe_, int e_)
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
            //if (Matriz[1, 1] == e_ && Matriz[1, 0] == 1)
            //{
            //    var Posicion = Matriz[1, 1] + phe_;
            //    Matriz[1, 1] = Posicion;

            //}


            return Matriz[1, 1];

        }


        private int Coprimos(BigInteger phe, BigInteger p_, BigInteger q_)
        {
            int contador = 2;
            bool CoprimoEncontrado = false;
            var Coprimo = 0;

            var MultiposPhe = new List<BigInteger>();

            var NuevoNumero = phe;



            while (contador < phe && MultiposPhe.Count < 10)
            {
                while (NuevoNumero % contador == 0)
                {
                    if (!MultiposPhe.Contains(contador))
                    {
                        MultiposPhe.Add(contador);
                    }
                    NuevoNumero = NuevoNumero / contador;

                }
                if (NuevoNumero == 1)
                {
                    break;
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
