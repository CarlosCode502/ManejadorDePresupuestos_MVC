using ManejadorDePresupuestos_MVC.Models;
using ManejadorDePresupuestos_MVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejadorDePresupuestos_MVC.Controllers
{
    //V#125 Formulario de Cuentas (Creando el controlador Cuentas)

    /// <summary>
    /// Clase que contiene los mútliples actions de la tabla Cuentas
    /// </summary>
    public class CuentasController : Controller
    {
        //V#125 Formulario de Cuentas (Luego de inyectar dependencias 2 hasta ahora repoTC y serUs)
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IServicioUsuarios servicioUsuarios;

        //V#125 Formulario de Cuentas (Inyectando dependencias 2 hasta ahora repoTC y serUs)
        /// <summary>
        /// Inyección de dependencias.
        /// </summary>
        /// <param name="repositorioTiposCuentas">Apunta a la interfaz y antes al repo.</param>
        /// <param name="servicioUsuarios">Apunta a la interfaz y luego al sevicion Obtener usuarioId.</param>
        public CuentasController(IRepositorioTiposCuentas repositorioTiposCuentas, IServicioUsuarios servicioUsuarios) 
        {
            //V#125 Formulario de Cuentas (Luego de inyectar dependencias 2 hasta ahora repoTC y serUs)
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.servicioUsuarios = servicioUsuarios;
        }

        //V#125 Formulario de Cuentas (Creando el controlador Cuentas)
        [HttpGet]
        public async Task<IActionResult> Crear() 
        {
            //Obtener el Id del usuario desde el servicio.
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtener los tiposCuentas según el usuarioID obtenido.
            var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);

            //Se asigna el modelo del la clase que hereda de CuentaVM
            var modelo = new DropDownCuentaViewModel();

            //Modelo contiene el DropDLCuentaVM se accede al DPL a través de la propiedad (Nombre de este TiposCuentas)
            //Select hace el mapeo de tipos cuentas y crea un nuevo DropDL con los datos de este
            //(x.Nombre, x.Id.ToString() Primero el texto luego el valor (El texto debe ver el usuario)
            modelo.TiposCuentas = tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));

            //Se manda el modelo a la vista
            return View(modelo); 
        }
    }
}
