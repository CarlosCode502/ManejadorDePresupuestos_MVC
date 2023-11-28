using ManejadorDePresupuestos_MVC.Models;

namespace ManejadorDePresupuestos_MVC.Services
{
    public interface IRepositorioCategorias
    {
        //V#136 Editar Categorías (Método Actualizar min 01.50)
        Task Actualizar(CategoriaViewModel categoriaViewModel);

        //V#133 Creando Categorias (Agregando a la interfaz)
        Task Crear(CategoriaViewModel categoriaViewModel);

        //V#135 Indice de Categorias (Agregando a la interfaz min 01.10)
        Task<IEnumerable<CategoriaViewModel>> Obtener(int usuarioId);

        //V#136 Editar Categorías (Método ObtenerPorIdCategoria min 00.50)
        Task<CategoriaViewModel> ObtenerPorIdCategoria(int id, int usuarioId);
    }
}
