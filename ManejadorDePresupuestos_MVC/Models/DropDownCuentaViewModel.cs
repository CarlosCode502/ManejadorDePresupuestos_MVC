using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejadorDePresupuestos_MVC.Models
{
    //V#125 Formulario de Cuentas (Creando el SelectListItem de los tiposCuentas)

    /// <summary>
    /// Esta clase contiene un IEnum de tipo SelectListItem para crear un listado 
    /// de dropdowns, que hereda de la clase CuentaViewModel que contiene un modelo.
    /// </summary>
    public class DropDownCuentaViewModel : CuentaViewModel
    {
        //SelectListItem es una clase especial de ASP.Net que nos permite crear selects
        //de una manera muy sencilla (DropDownList).
        public IEnumerable<SelectListItem> TiposCuentas { get; set; }
    }
}
