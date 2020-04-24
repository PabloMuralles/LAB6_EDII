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
        static Dictionary<string, int> diccionarioOriginal = new Dictionary<string, int>();
        static Dictionary<string, int> diccionarioCifrado = new Dictionary<string, int>();
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
        //se considera que en el Cifrado Cesar unicamente se tienen las letras del alfabeto, mayúsculas y minúsculas, y que no se toman en cuenta las tildes
        private void generarDiccionarioOriginal()
        {
            if (diccionarioOriginalVacio)
            {
                var valorMayusculas = 32;
                var contadorDiccionario = 1;

                for (int i = 0; i < 256; i++)
                {
                    diccionarioOriginal.Add(Convert.ToString((char)valorMayusculas), contadorDiccionario);
                    contadorDiccionario++;
                    valorMayusculas++;
                }
                diccionarioOriginal.Add("\r\n", contadorDiccionario + 1);
                diccionarioOriginalVacio = false;
            }


        }
        private void generarDiccionarioCifrado(string clave)
        {
            var contadorDiccionario = 1;
            //se añade la clave al diccionario
            if (clave != null)
            {
                foreach (var letra in clave)
                {
                    if (!diccionarioCifrado.ContainsKey(Convert.ToString(letra)))
                    {
                        diccionarioCifrado.Add(Convert.ToString(letra), contadorDiccionario);
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
        private void ObtenerTextoArchivoOriginal(string archivoLeido)
        {
            var bufferLength = 10000;
            using (var stream = new FileStream(archivoLeido, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var byteBuffer = new byte[bufferLength];
                    //el buffer de lectura de archivo se utiliza indirectamente para la escritura del nuevo archivo tambien
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(bufferLength);
                        CifrarTexto(byteBuffer);
                    }
                }
            }
        }
        private void ObtenerTextoArchivoDecifrado(string archivoLeido)
        {
            var bufferLength = 10000;
            var texto = string.Empty;
            using (var stream = new FileStream(archivoLeido, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var byteBuffer = new byte[bufferLength];
                    //el buffer de lectura de archivo se utiliza indirectamente para la escritura del nuevo archivo tambien
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(bufferLength);

                        DecifrarTexto(byteBuffer);
                    }
                }
            }
        }
        private void CifrarTexto(byte[] byteBuffer)
        {
            var texto = string.Empty;
            foreach (char letra in byteBuffer)
            {
                //se realiza la conversión de los caracteres 
                var receptorValorOriginal = diccionarioOriginal.LastOrDefault(x => x.Key == Convert.ToString(letra)).Value;
                var receptorValorCifrado = diccionarioCifrado.LastOrDefault(x => x.Value == receptorValorOriginal).Key;
                if (receptorValorOriginal == 0) { receptorValorCifrado = Convert.ToString(letra); }

                texto += receptorValorCifrado;
            }


            string CarpetaCompress = Environment.CurrentDirectory;

            if (!Directory.Exists(Path.Combine(CarpetaCompress, "CipherCesar2")))
            {
                Directory.CreateDirectory(Path.Combine(CarpetaCompress, "CipherCesar2"));
            }
             

            using (var writeStream = new FileStream(Path.Combine(CarpetaCompress, "CipherCesar2", $"{RutaUsuario}.text  "), FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    writer.Seek(0, SeekOrigin.End);
                    writer.Write(System.Text.Encoding.Unicode.GetBytes(texto));
                }
            }
        }
        private void DecifrarTexto(byte[] byteBuffer)
        {
            var texto = string.Empty;
            foreach (char letra in byteBuffer)
            {
                //se realiza la conversión de los caracteres 
                var receptorValorCifrado = diccionarioCifrado.LastOrDefault(x => x.Key == Convert.ToString(letra)).Value;
                var receptorValorDecifrado = diccionarioOriginal.LastOrDefault(x => x.Value == receptorValorCifrado).Key;
                if (receptorValorDecifrado == "\0")
                {
                    receptorValorDecifrado = Convert.ToString(letra);
                }
                if (receptorValorDecifrado == "\r")
                {
                    receptorValorDecifrado = diccionarioOriginal.LastOrDefault(x => x.Value == receptorValorCifrado).Key;
                }

                texto += receptorValorDecifrado;
            }


            string CarpetaCompress = Environment.CurrentDirectory;

            if (!Directory.Exists(Path.Combine(CarpetaCompress, "DecipherCesar2")))
            {
                Directory.CreateDirectory(Path.Combine(CarpetaCompress, "DecipherCesar2"));
            }


            using (var writeStream = new FileStream(Path.Combine(CarpetaCompress, "DecipherCesar2", $"{RutaUsuario}.text  "), FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    foreach (var item in texto)
                    {
                        writer.Write(item);
                    }
                }
            }
        }



    }
}
