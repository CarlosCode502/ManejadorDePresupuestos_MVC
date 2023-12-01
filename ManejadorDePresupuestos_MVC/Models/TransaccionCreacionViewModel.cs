using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ManejadorDePresupuestos_MVC.Models
{
    //V#138 Creando Transacciones (Modelo que contien 2 SelectListItem de Transacciones 09.20)
    public class TransaccionCreacionViewModel : TransaccionesViewModel //Había olvidado el transacción viewModel
    {
        //Va a contener las cuentas del usuario en un SelectListItem
        public IEnumerable<SelectListItem> Cuentas { get; set; }

        //Va a contener las Categorias del usuario en un SelectListItem
        public IEnumerable<SelectListItem> Categorias { get; set; }

        //V#140 Agregando los demás Campos (Agregando la propiedad TiposCuentas min 01.26) 
        //Es importante ya que las cat se van a mostrar los tipos operación (Gastos o Ingresos)
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DisplayName("Tipo de Operación")]
        public TipoOperacionEnum TipoOperacionId { get; set; } = TipoOperacionEnum.Gastos; //Simular que se elije uno
    }
}
