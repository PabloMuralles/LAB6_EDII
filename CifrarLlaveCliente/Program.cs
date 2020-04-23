using System.Numerics;
using System;

namespace CifrarLlaveCliente
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Igrese el primer numoer de su llave");
            string n_ = Console.ReadLine();
            BigInteger n = BigInteger.Parse(n_);
            Console.WriteLine("Igrese el segundo numero de su llave");
            string d_e = Console.ReadLine();
            BigInteger de = BigInteger.Parse(d_e);
            Console.WriteLine("Ingrese la llave a cifrar");
            string Llave = Console.ReadLine();

            Cifrar Cifrar = new Cifrar();
            var LlaveBytes = Cifrar.LlaveEnBytes(Llave);
            Console.WriteLine( "Su clave cifrada es: "+"("+Cifrar.Cifrar2(n,de,LlaveBytes)+")");
            Console.WriteLine("Tomar unicamente lo que esta dentro de los parentesis");
            Console.ReadKey();

        }


      

    }
    
}
