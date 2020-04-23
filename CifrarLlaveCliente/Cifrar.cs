using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.IO;

namespace CifrarLlaveCliente
{
    class Cifrar
    {

        public string Cifrar2(BigInteger N, BigInteger D_E, byte[] llave)
        { 
            var ClaveCifrada = string.Empty;

            foreach (var item in llave)
            {
                BigInteger NumeroCaracter = new BigInteger(item);
                var CaracterCifrado = BigInteger.ModPow(NumeroCaracter, D_E, N);
                var Cambio = CaracterCifrado.ToByteArray();
                var INT = 0;
                foreach (var item2 in Cambio)
                {
                    INT += Convert.ToInt32(item2);
                }
                var NuevoCaracter= Convert.ToString(Convert.ToChar(Convert.ToByte(INT)));
                ClaveCifrada += NuevoCaracter;
            }
            return ClaveCifrada;
        }

        public byte[] LlaveEnBytes(string cadena)
        {
            var Codificador = new ASCIIEncoding();

            return Codificador.GetBytes(cadena);
        }



    }
}
