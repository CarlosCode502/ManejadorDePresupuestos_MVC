using ManejadorDePresupuestos_MVC.Models;

namespace ManejadorDePresupuestos_MVC.Services
{
    //V#126 Insertar Cuenta (Interfaz)
    public interface IRepositorioCuentas
    {
        //V#130 Editando Cuentas - Agregando Íconos a la Aplicación (Creando el método actualizar Cuenta)
        Task Actualizar(DropDownCuentaViewModel dropDownCuentaViewModel);

        //V#127 Indice de Cuentas - Query (Agregando a la Interfaz)
        Task<IEnumerable<CuentaViewModel>> Buscar(int usuarioId);

        //V#126 Insertar Cuenta (Enviando a la Interfaz)
        Task Crear(CuentaViewModel cuentaViewModel);

        //V#130 Editando Cuentas - Agregando Íconos a la Aplicación (Agregando a la Interfaz)
        Task<CuentaViewModel> ObtenerPorIdCuenta(int id, int usuarioId);



    }
}
