using ManejadorDePresupuestos_MVC.Models;

namespace ManejadorDePresupuestos_MVC.Services
{
    public interface IRepositorioCuentas
    {
        //V#126 Insertar Cuenta (Enviando a la Interfaz)
        Task Crear(CuentaViewModel cuentaViewModel);
    }
}
