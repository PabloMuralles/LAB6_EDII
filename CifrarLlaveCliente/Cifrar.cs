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

            var LlaveCifrada = new List<byte>();

            foreach (var item in llave)
            {
                BigInteger NumeroCaracter = new BigInteger(item);
                var CaracterCifrado = BigInteger.ModPow(NumeroCaracter, D_E, N);
                var Cambio = CaracterCifrado.ToByteArray();
                var INT = 0;
                foreach (var item2 in Cambio)
                {
                    LlaveCifrada.Add(item2);
                    INT += Convert.ToInt32(item2);
                }
                 
                var NuevoCaracter= Convert.ToString(Convert.ToChar(Convert.ToByte(INT)));
                ClaveCifrada += NuevoCaracter;
            }
            EscribirLlave(LlaveCifrada);
            return ClaveCifrada;
        }

        public byte[] LlaveEnBytes(string cadena)
        {
            var lista = new List<byte>();
            foreach (var item in cadena)
            {
                lista.Add(Convert.ToByte(Convert.ToByte(item)));
            }

            return lista.ToArray();
        }

        public void EscribirLlave(List<byte> ContraseñaCifrada)
        {
            var ruta = Path.Combine(@"c:\Temp", $"Contraseña.txt");
            if (File.Exists(ruta))
            {
                File.Delete(ruta);
            }
            
             using (var streamwriter = new FileStream(Path.Combine( @"c:\Temp", $"Contraseña.txt"), FileMode.OpenOrCreate))
             {
                using (var write = new BinaryWriter(streamwriter))
                {
                    foreach (var item in ContraseñaCifrada)
                    {
                        write.Write(item);
                    }
                    
                }

             }

        }



    }
}
