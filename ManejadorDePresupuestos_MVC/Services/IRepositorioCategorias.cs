using ManejadorDePresupuestos_MVC.Models;

namespace ManejadorDePresupuestos_MVC.Services
{
    public interface IRepositorioCategorias
    {
        //V#133 Creando Categorias (Agregando a la interfaz)
        Task Crear(CategoriaViewModel categoriaViewModel);

        //V#135 Indice de Categorias (Agregando a la interfaz min 01.10)
        Task<IEnumerable<CategoriaViewModel>> Obtener(int usuarioId);
    }
}
