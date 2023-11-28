using ManejadorDePresupuestos_MVC.Models;

namespace ManejadorDePresupuestos_MVC.Services
{
    public interface IRepositorioCategorias
    {
        //V#133 Creando Categorias (Agregando a la interfaz)
        Task Crear(CategoriaViewModel categoriaViewModel);
    }
}
