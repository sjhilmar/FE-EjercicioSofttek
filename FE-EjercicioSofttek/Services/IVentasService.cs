using FE_EjercicioSofttek.Models;

namespace FE_EjercicioSofttek.Services
{
    public interface IVentasService
    {
        Task<List<Ventas>> Listar();
        Task<Ventas> Obtener(int id);
        Task<bool> Guardar(Ventas ventas);
        Task<bool> Editar(Ventas ventas);
        Task<bool> Eliminar(int id);
    }
}
