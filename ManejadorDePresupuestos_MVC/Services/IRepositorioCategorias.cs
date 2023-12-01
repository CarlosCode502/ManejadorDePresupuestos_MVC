using ManejadorDePresupuestos_MVC.Models;

namespace ManejadorDePresupuestos_MVC.Services
{
    public interface IRepositorioCategorias
    {
        //V#136 Editar Categorías (Método Actualizar min 01.50)
        Task Actualizar(CategoriaViewModel categoriaViewModel);

        //V#137 Borrar Categorias (Creando método borrar categoria min 0.55)
        Task Borrar(int id);

        //V#133 Creando Categorias (Agregando a la interfaz)
        Task Crear(CategoriaViewModel categoriaViewModel);

        //V#135 Indice de Categorias (Agregando a la interfaz min 01.10)
        Task<IEnumerable<CategoriaViewModel>> Obtener(int usuarioId);

        //V#136 Editar Categorías (Método ObtenerPorIdCategoria min 00.50)
        Task<CategoriaViewModel> ObtenerPorIdCategoria(int id, int usuarioId);

        //V#141 DropDown Cascada (Método que permita obtener las categorias según usuarioId y tipoOperacion min 04.50)
        Task<IEnumerable<CategoriaViewModel>> ObtenerPorUsuarioIdyTipoOperacion(int usuarioId, TipoOperacionEnum tipoOperacionId);
    }
}
