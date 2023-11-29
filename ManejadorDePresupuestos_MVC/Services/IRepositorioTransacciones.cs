using ManejadorDePresupuestos_MVC.Models;

namespace ManejadorDePresupuestos_MVC.Services
{
    //V#138 Creando Transacciones (IRepositorioTrasacciones min 01:20)
    public interface IRepositorioTransacciones
    {
        //V#138 Creando Transacciones (Método para Crear una transacción min 08.40)
        Task Crear(TransaccionesViewModel transaccionesViewModel);
    }
}
