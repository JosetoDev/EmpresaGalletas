using Common.DTOs;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Consultas
{
    public class ExistenciasRepositorio
    {

        public List<ArticuloExistenciaDTO> ListaExistencias()
        {
            using (GalletasDBEntities db = new GalletasDBEntities())
            {
                var res = from e in db.tblExistencias
                          group e by e.IdArticulo into g
                          join a in db.tblArticulos on g.Key equals a.IdArticulo
                          select new ArticuloExistenciaDTO
                          {
                              IdArticulo = g.Key,
                              CantidadExistencias = g.Sum(x => x.Cantidad),
                              NombreArticulo = a.NomArticulo,
                              DescripcionArticulo = a.Descripcion,
                              CategoriaArticulo = a.Categoria
                          };

                return res.ToList();
            }
        }

        public int ExistenciasPorArticulo(string _idArt)
        {
            using (GalletasDBEntities db = new GalletasDBEntities())
            {
                var res = from e in db.tblExistencias
                          where e.IdArticulo == _idArt
                          select e.Cantidad;

                return res.Sum();
            }
        }

        public bool AgregarExistencia(string _idArt, DateTime _fechaVence, int _cantidad, string _lote)
        {
            tblExistencias nuevaExistencia = new tblExistencias()
            {
                IdArticulo = _idArt,
                IdExistencias = Guid.NewGuid().ToString(),
                Cantidad = _cantidad,
                FechaVence = _fechaVence,
                Lote = _lote
            };

            using (GalletasDBEntities db = new GalletasDBEntities())
            {
                db.tblExistencias.Add(nuevaExistencia);
                return db.SaveChanges() > 0;
            }
        }
    }
}
