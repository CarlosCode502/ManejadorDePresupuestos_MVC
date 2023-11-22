using ManejadorDePresupuestos_MVC.Models;

namespace ManejadorDePresupuestos_MVC.Services
{
    //V#110 Insertando un Tipo de Cuenta en la Base de Datos (Creando el IRepositorio)
    public interface IRepositorioTiposCuentas
    {
        //V#117 Actualizando Tipos Cuentas (Método simple para actualizar un registro TipoCuenta no retorna nada)
        Task Actualizar(TipoCuentaViewModel tipoCuentaViewModel);
        //V#118 Borrando tipos de cuentas (Registrando en la interfaz)
        Task Borrar(int id);
        //V#110
        //void Crear(TipoCuentaViewModel tipoCuentaViewModel);
        //V#112 Aplicando la programación Asíncrona(Es útil cuando existe una comunicación con externos)
        //Debe ser acorde al método asyncrono
        Task Crear(TipoCuentaViewModel tipoCuentaViewModel);
        //V#113 Validaciones personalizadas a Nivel del Controlador
        Task<bool> Existe(string nombre, int usuarioId);
        //V# 115 Listado Tipos Cuentas (Pasando a la interfaz)
        Task<IEnumerable<TipoCuentaViewModel>> Obtener(int usuarioId);
        //V#117 Actualizando Tipos Cuentas (Para que el usuario pueda consultar por id solo su id) (Este si retorna)
        Task<TipoCuentaViewModel> ObtenerPorId(int id, int usuarioId);
    }
}
