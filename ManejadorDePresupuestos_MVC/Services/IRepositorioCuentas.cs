using ManejadorDePresupuestos_MVC.Models;

namespace ManejadorDePresupuestos_MVC.Services
{
    //V#126 Insertar Cuenta (Interfaz)
    public interface IRepositorioCuentas
    {
        //V#127 Indice de Cuentas - Query (Agregando a la Interfaz)
        Task<IEnumerable<CuentaViewModel>> Buscar(int usuarioId);

        //V#126 Insertar Cuenta (Enviando a la Interfaz)
        Task Crear(CuentaViewModel cuentaViewModel);
    }
}
