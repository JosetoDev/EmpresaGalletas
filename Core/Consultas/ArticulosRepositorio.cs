using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Consultas
{
    public class ArticulosRepositorio
    {
        /// <summary>
        /// este metodo se encarga de consultar todos los registros de la tabla articulos
        /// </summary>
        /// <returns></returns>
        public List<tblArticulos> ConsultaTodosArticulos()
        {
            //creamos un objeto de tipo contex de la base de datos para accedela
            using (GalletasDBEntities db = new GalletasDBEntities())
            {
                //realizamos la consulta a la base de datos
                var resultado = db.tblArticulos;
                //retornamos el resultado de la consulta
                return resultado.ToList();
            }

        }

        /// <summary>
        /// este metodo se encarga de crear un nuevo registro en la tabla articulos y guardarlo en la base de datos
        /// </summary>
        /// <param name="_nomArticulo"></param>
        /// <param name="_descripcion"></param>
        /// <param name="_categoria"></param>
        /// <param name="_precio"></param>
        /// <returns></returns>
        public bool AgregarArticulo(string _nomArticulo, string _descripcion, string _categoria, int  _precio, out string Mensaje)
        {            
            //validamos si el artculo ya esta creado            
            if (ValidarArticulo(_nomArticulo))
            {
                Mensaje = "el articulo ya se encuentra registrado";
                return false;
            }
            //creamos un objeto de tipo tblArticulos y lo cargamos con toda la informacion que me llega
            tblArticulos nuevoArticulo = new tblArticulos();
            nuevoArticulo.IdArticulo = Guid.NewGuid().ToString();
            nuevoArticulo.NomArticulo = _nomArticulo.Trim().ToUpper();
            nuevoArticulo.Descripcion = _descripcion.Trim();
            nuevoArticulo.Categoria = _categoria.Trim();
            nuevoArticulo.Precio = _precio;
            nuevoArticulo.Eliminado = false;

            //creamos un objeto de tipo contex de la base de datos para accedela
            using (GalletasDBEntities db = new GalletasDBEntities())
            {                
                //agregamos el nuevo registro la tabla de la base de datos
                db.tblArticulos.Add(nuevoArticulo);
                                
                //validamos el resultado de insertar el nuevo registro y retornamos el resultado
                if (db.SaveChanges() > 0)
                {
                    Mensaje = "el articulo se agrego correctamente";
                    return true;
                }
                else
                {
                    Mensaje = "Error: el articulo no se agrego, verifique la informacion";
                    return false;
                }
            }
        }

        /// <summary>
        /// este metodo se encarga de consultar un registro y de modificarlo
        /// </summary>
        /// <param name="_idArt"></param>
        /// <param name="_nomArt"></param>
        /// <param name="_descripcion"></param>
        /// <param name="_precio"></param>
        /// <param name="_categoria"></param>
        /// <returns></returns>
        public bool ActualizarArticulo(string _idArt, string _nomArt, string _descripcion, int _precio, string _categoria, bool _eliminado)
        {
            //creamos un objeto de tipo contex de la base de datos para accedela
            using (GalletasDBEntities db = new GalletasDBEntities())
            {
                //consultamos la bd para e registro a modificar
                var registro = (from a in db.tblArticulos
                                where a.IdArticulo == _idArt
                                select a).FirstOrDefault();

                //validamos si tenemos un registro para modificar
                if (registro == null)
                    return false;

                //reslizamos las modificaciones necesarias
                registro.NomArticulo = _nomArt;
                registro.Descripcion = _descripcion;
                registro.Precio = _precio;
                registro.Categoria = _categoria;
                registro.Eliminado = _eliminado;

                //guardamos el numero de registros modificados
                int resgitrosModificados = db.SaveChanges();
                //validamos si se realizaron los cambios
                if (resgitrosModificados == 0)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// este metodo me permite activar o DesActivar un articulo
        /// </summary>
        /// <param name="_idArt"></param>
        /// <param name="_eliminado"></param>
        /// <returns></returns>
        public bool ActivarDesActivarArticulo(string _idArt, bool _eliminado)
        {
            //creamos un objeto de tipo contex de la base de datos para accedela
            using (GalletasDBEntities db = new GalletasDBEntities())
            {
                //consultamos la bd para e registro a modificar
                var registro = (from a in db.tblArticulos
                                where a.IdArticulo == _idArt
                                select a).FirstOrDefault();

                //validamos si tenemos un registro para modificar
                if (registro == null)
                    return false;

                //reslizamos las modificaciones necesarias           
                registro.Eliminado = _eliminado;

                //guardamos el numero de registros modificados
                int resgitrosModificados = db.SaveChanges();
                //validamos si se realizaron los cambios
                if (resgitrosModificados == 0)
                    return false;

                return true;
            }            
        }

        /// <summary>
        /// metodo que se encarga de validar si un articulo ya existe, retorna verdadero cuando el articulo existe
        /// </summary>
        /// <param name="_nomArt"></param>
        /// <returns></returns>
        private bool ValidarArticulo(string _nomArt)
        {
            using (GalletasDBEntities db = new GalletasDBEntities())
            {
                var validacion = from a in db.tblArticulos
                                 where a.NomArticulo.Trim().ToUpper().Contains(_nomArt.Trim().ToUpper())
                                 select a;

                return validacion.Count() > 0;

                //if (validacion.Count() == 0 )
                //{
                //    return true;
                //}
                //return false;
            }

        }

        /// <summary>
        /// valida si todos los datos son correctos, el entero de retorna 0 cuando no paso alguna validacion,
        /// retorna otro numero diferente a cero que hace refencia a que paso todas las validaciones y es el precio de venta,
        /// ademas retorna un parametro de salida que contiene los mensajes de validacion
        /// </summary>
        /// <param name="_precio"></param>
        /// <param name="_nomArt"></param>
        /// <param name="_descip"></param>
        /// <param name="_cate"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        public int ValidacionesDatosArticulo(string _precio, string _nomArt,string _descip, string _cate, out string mensaje)
        {
            StringBuilder MesajeDeSalida = new StringBuilder();
            int enteroDeSalida = 0;

            //validamos si el precio se puede convertir
            bool sePudoConvertir = int.TryParse(_precio, out enteroDeSalida);
            if (!sePudoConvertir)
            {
                MesajeDeSalida.Append("el precio debe ser ingresado con numeros \n");
            }
            else
            {
                if (enteroDeSalida == 0)
                {
                    MesajeDeSalida.Append("el precio debe ser mayor a cero \n");
                }
            }
            if (string.IsNullOrWhiteSpace(_nomArt))
            {
                MesajeDeSalida.Append("debe ingresar nombre de articulo \n");
                enteroDeSalida = 0;
            }
            if (string.IsNullOrWhiteSpace(_descip))
            {
                MesajeDeSalida.Append("debe ingresar descripcion de articulo \n");
                enteroDeSalida = 0;
            }
            if (string.IsNullOrWhiteSpace(_cate))
            {
                MesajeDeSalida.Append("debe ingresar categoria de articulo \n");
                enteroDeSalida = 0;
            }

            mensaje = MesajeDeSalida.ToString();

            return enteroDeSalida;

        }
    }
}
