using Core.Consultas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //creamos las variabes que guardaran la informacion que ingrese el usuario
            string _nomArt, _descripcion, _categoria, _precio, mensaje;
            //cramos un objeto de tipo repositorio para acceder a los metodos del mismo
            ArticulosRepositorio articuloRepositorio = new ArticulosRepositorio();
            int precioEnEnteroyBanderadevalidacion = 0;
            do
            {
                //solicitamos la informacion para crear un articulo al usuario
                Console.WriteLine("Escriba el nombre del articulo:");
                _nomArt = Console.ReadLine();
                Console.WriteLine("Escriba la descripcion del articulo:");
                _descripcion = Console.ReadLine();
                Console.WriteLine("Escriba la categoria del articulo:");
                _categoria = Console.ReadLine();
                Console.WriteLine("Escriba el precio del articulo en numeros:");
                _precio = Console.ReadLine();

                precioEnEnteroyBanderadevalidacion = articuloRepositorio.ValidacionesDatosArticulo(_precio, _nomArt, _descripcion, _categoria, out mensaje);

                if (precioEnEnteroyBanderadevalidacion == 0)
                {
                    Console.WriteLine(mensaje);
                    Console.WriteLine("Por Favor Ingrese nuevamente los datos");
                    Console.ReadKey();
                }

            } while (precioEnEnteroyBanderadevalidacion == 0);

            
            //Guardamos el articulo
            if (articuloRepositorio.AgregarArticulo(_nomArt, _descripcion, _categoria, precioEnEnteroyBanderadevalidacion, out mensaje))
                Console.WriteLine(mensaje);
            else
                Console.WriteLine(mensaje);




            Console.ReadKey();


        }
    }
}
