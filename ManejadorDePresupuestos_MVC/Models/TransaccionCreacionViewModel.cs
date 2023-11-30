using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejadorDePresupuestos_MVC.Models
{
    //V#138 Creando Transacciones (Modelo que contien 2 SelectListItem de Transacciones 09.20)
    public class TransaccionCreacionViewModel : TransaccionesViewModel //Había olvidado el transacción viewModel
    {
        //Va a contener las cuentas del usuario en un SelectListItem
        public IEnumerable<SelectListItem> Cuentas { get; set; }

        //Va a contener las Categorias del usuario en un SelectListItem
        public IEnumerable<SelectListItem> Categorias { get; set; }
    }
}
