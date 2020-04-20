using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Laboratorio_6.Estructura
{
    public class CifradoCesar
    {
        private static CifradoCesar _instance = null;
        public static CifradoCesar Instance
        {
            get
            {
                if (_instance == null) _instance = new CifradoCesar();
                return _instance;
            }
        }
        static Dictionary<string, int> diccionarioOriginal = new Dictionary<string, int>();
        static Dictionary<string, int> diccionarioCifrado = new Dictionary<string, int>();
        string RutaUsuario = string.Empty;
        static bool diccionarioOriginalVacio = true;

        public void CifrarMensaje(string RutaAchivos, string ArchivoLeido, string clave)
        {
            RutaUsuario = RutaAchivos;
            generarDiccionarioOriginal();
            generarDiccionarioCifrado(clave);
            ObtenerTextoArchivoOriginal(ArchivoLeido);
            diccionarioCifrado.Clear();

        }
        public void DecifrarMensaje(string RutaAchivos, string ArchivoLeido, string clave)
        {
            RutaUsuario = RutaAchivos;
            generarDiccionarioOriginal();
            generarDiccionarioCifrado(clave);
            ObtenerTextoArchivoDecifrado(ArchivoLeido);
            diccionarioCifrado.Clear();
        }
        //se considera que en el Cifrado Cesar unicamente se tienen las letras del alfabeto, mayúsculas y minúsculas, y que no se toman en cuenta las tildes
        public void generarDiccionarioOriginal()
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
        public void generarDiccionarioCifrado(string clave)
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
        public void ObtenerTextoArchivoOriginal(string archivoLeido)
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
        public void ObtenerTextoArchivoDecifrado(string archivoLeido)
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
        public void CifrarTexto(byte[] byteBuffer)
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
            //using (var writeStream = new FileStream(RutaUsuario + "\\..\\Files\\archivoCifradoCesar.cif", FileMode.OpenOrCreate))
            //{
            //    using (var writer = new BinaryWriter(writeStream))
            //    {
            //        writer.Seek(0, SeekOrigin.End);
            //        writer.Write(System.Text.Encoding.Unicode.GetBytes(texto));
            //    }
            //}
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
            //using (var writeStream = new FileStream(RutaUsuario + "\\..\\Files\\archivoDecifradoCesar.txt", FileMode.OpenOrCreate))
            //{
            //    using (var writer = new BinaryWriter(writeStream))
            //    {
            //        writer.Write(System.Text.Encoding.Unicode.GetBytes(texto));
            //    }
            //}
        }
    }
}
