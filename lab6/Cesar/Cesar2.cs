using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace lab6.Cesar
{
    public class Cesar2
    {
        private static Cesar2 _instance = null;
        public static Cesar2 Instance
        {
            get
            {
                if (_instance == null) _instance = new Cesar2();
                return _instance;
            }
        }
        static Dictionary<int, int> diccionarioOriginal = new Dictionary<int, int>();
        static Dictionary<int, int> diccionarioCifrado = new Dictionary<int, int>();
        string RutaUsuario = string.Empty;
        static bool diccionarioOriginalVacio = true;
        /// <summary>
        /// Metodo para cifrar el archivo
        /// </summary>
        /// <param name="RutaAchivos">Es donde se va guardar el archivo cifrado</param>
        /// <param name="ArchivoLeido">Es la ruta del archivo a cifrar</param>
        /// <param name="clave">Es la clave para mover el diccionario</param>
        public void CifrarMensaje(string RutaAchivos, string ArchivoLeido, string clave)
        {
            RutaUsuario = RutaAchivos;
            generarDiccionarioOriginal();
            generarDiccionarioCifrado(clave);
            ObtenerTextoArchivoOriginal(ArchivoLeido);
            diccionarioCifrado.Clear();

        }
        /// <summary>
        /// Metodo para descifrar el archivo
        /// </summary>
        /// <param name="RutaAchivos">Es donde se va guardar el archivo cifrado</param>
        /// <param name="ArchivoLeido">Es la ruta del archivo a cifrar</param>
        /// <param name="clave">Es la clave para mover el diccionario</param>
        public void DecifrarMensaje(string RutaAchivos, string ArchivoLeido, string clave)
        {
            RutaUsuario = RutaAchivos;
            generarDiccionarioOriginal();
            generarDiccionarioCifrado(clave);
            ObtenerTextoArchivoDecifrado(ArchivoLeido);
            diccionarioCifrado.Clear();
        }


        public void generarDiccionarioOriginal()
        {
            if (diccionarioOriginalVacio)
            {

                var contadorDiccionario = 1;

                for (int i = 65; i < 91; i++)
                {
                     
                    diccionarioOriginal.Add(i, contadorDiccionario);
                    contadorDiccionario++;

                }
                for (int i = 97; i < 123; i++)
                {
                     
                    diccionarioOriginal.Add(i, contadorDiccionario);
                    contadorDiccionario++;

                }
                 
                diccionarioOriginalVacio = false;
            }


        }
        public void generarDiccionarioCifrado(string clave)
        {
            var contadorDiccionario = 1;
            //se añade la clave al diccionario
            if (clave != null)
            {
                foreach (var letra in clave)
                {
                     
                    if (!diccionarioCifrado.ContainsKey((letra)))
                    {
                         
                        diccionarioCifrado.Add((letra), contadorDiccionario);
                        contadorDiccionario++;
                    }
                }
            }
            //se realiza la compracion de las letras del diccionario original que entraran en diferente orden al diccionario cifrado
            foreach (var item in diccionarioOriginal.Keys)
            {
                if (!diccionarioCifrado.ContainsKey(item))
                {
                    diccionarioCifrado.Add(item, contadorDiccionario);
                    contadorDiccionario++;
                }
            }
        }


        public void ObtenerTextoArchivoOriginal(string archivoLeido)
        {
            var bufferLength = 10000;

            string CarpetaCompress = Environment.CurrentDirectory;

            if (!Directory.Exists(Path.Combine(CarpetaCompress, "CipherCesar2")))
            {
                Directory.CreateDirectory(Path.Combine(CarpetaCompress, "CipherCesar2"));
            }

            using (var writeStream = new FileStream(Path.Combine(CarpetaCompress, "CipherCesar2", $"{RutaUsuario}.txt"), FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    using (var stream = new FileStream(archivoLeido, FileMode.Open))
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            var byteBuffer = new byte[bufferLength];
                            //el buffer de lectura de archivo se utiliza indirectamente para la escritura del nuevo archivo tambien
                            while (reader.BaseStream.Position != reader.BaseStream.Length)
                            {
                                byteBuffer = reader.ReadBytes(bufferLength);

                                var texto = new List<int>();
                                foreach (var letra in byteBuffer)
                                {
                                    int receptorValorOriginal;
                                    int receptorValorCifrado;

                                    if (diccionarioOriginal.ContainsKey(letra))
                                    {
                                        receptorValorOriginal = diccionarioOriginal.LastOrDefault(x => x.Key == (letra)).Value;
                                        receptorValorCifrado = diccionarioCifrado.LastOrDefault(x => x.Value == receptorValorOriginal).Key;
                                        if (receptorValorOriginal == 0) { receptorValorCifrado = (letra); }

                                    }
                                    else
                                    {
                                        receptorValorCifrado = letra;
                                    }


                                    texto.Add(receptorValorCifrado);
                                }
                                foreach (var item in texto)
                                {
                                    writer.Write(Convert.ToByte(Convert.ToChar(item)));
                                }
                            }
                        }
                    }

                }
            }



        }




        public void ObtenerTextoArchivoDecifrado(string archivoLeido)
        {
            var bufferLength = 10000;

            string CarpetaCompress = Environment.CurrentDirectory;

            if (!Directory.Exists(Path.Combine(CarpetaCompress, "DecipherCesar2")))
            {
                Directory.CreateDirectory(Path.Combine(CarpetaCompress, "DecipherCesar2"));
            }




            using (var writeStream = new FileStream(Path.Combine(CarpetaCompress, "DecipherCesar2", $"{RutaUsuario}.txt  "), FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    using (var stream = new FileStream(archivoLeido, FileMode.Open))
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            var byteBuffer = new byte[bufferLength];
                            //el buffer de lectura de archivo se utiliza indirectamente para la escritura del nuevo archivo tambien
                            while (reader.BaseStream.Position != reader.BaseStream.Length)
                            {
                                byteBuffer = reader.ReadBytes(bufferLength);

                                var texto = new List<int>();
                                var textoverificacion = string.Empty;

                                int receptorValorCifrado;
                                int receptorValorDecifrado;
                                foreach (var letra in byteBuffer)
                                {
                                    if (diccionarioCifrado.ContainsKey(letra))
                                    {
                                        receptorValorCifrado = diccionarioCifrado.LastOrDefault(x => x.Key == (letra)).Value;
                                        receptorValorDecifrado = diccionarioOriginal.LastOrDefault(x => x.Value == receptorValorCifrado).Key;

                                    }
                                    else
                                    {
                                        receptorValorDecifrado = letra;
                                    }

                                    textoverificacion += Convert.ToString(Convert.ToChar(receptorValorDecifrado));
                                    texto.Add(receptorValorDecifrado);
                                }
                                foreach (var item in texto)
                                {
                                    writer.Write(Convert.ToByte(Convert.ToChar(item)));
                                }

                            }
                        }
                    }
                }
            }


        }
         
    }
}
