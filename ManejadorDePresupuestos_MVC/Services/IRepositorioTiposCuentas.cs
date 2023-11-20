using ManejadorDePresupuestos_MVC.Models;

namespace ManejadorDePresupuestos_MVC.Services
{
    //V#110 Insertando un Tipo de Cuenta en la Base de Datos (Creando el IRepositorio)
    public interface IRepositorioTiposCuentas
    {
        void Crear(TipoCuentaViewModel tipoCuentaViewModel);
    }
}
