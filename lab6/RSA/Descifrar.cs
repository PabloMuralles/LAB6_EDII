using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;
using System.Text;
using System.IO;

namespace lab6.RSA
{

    public class Descifrar
    {
        private static Descifrar _instance = null;
        public static Descifrar Instance
        {
            get
            {
                if (_instance == null) _instance = new Descifrar();
                return _instance;
            }
        }

        private BigInteger N;

        private BigInteger D;


        public void RecibirLlavePrivada(BigInteger N, BigInteger D)
        {
            this.N = new BigInteger();
            this.N = N;
            this.D = new BigInteger();
            this.D = D;
             
        }

        public string DecifrarContraseña( )
        {

            var contraseñacifrada = new List<byte>();
             
            string Ruta = @"c:\Temp";
            
            using (var streamwriter = new FileStream(Path.Combine(Ruta, $"Contraseña.txt"), FileMode.Open))
            {
                using (var reader = new  BinaryReader(streamwriter))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        var nuevocaracter = reader.ReadBytes(1);
                        foreach (var item in nuevocaracter )
                        {
                            contraseñacifrada.Add(item);
                        }
                    }

                }

            }



            var ClaveDescifrada = string.Empty;
            foreach (var item in contraseñacifrada)
            {
                BigInteger NumeroCaracter = new BigInteger(item);
                var CaracterCifrado = BigInteger.ModPow(NumeroCaracter,D, N);
                var Cambio = CaracterCifrado.ToByteArray();
                var INT = 0;
                foreach (var item2 in Cambio)
                {
                    INT += Convert.ToInt32(item2);
                }
                var NuevoCaracter = Convert.ToString(Convert.ToChar(Convert.ToByte(INT)));
                ClaveDescifrada += NuevoCaracter;
            }

            return ClaveDescifrada;

        }

         
    }
}
