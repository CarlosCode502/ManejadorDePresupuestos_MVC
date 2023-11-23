using ManejadorDePresupuestos_MVC.Validations;
using System.ComponentModel.DataAnnotations;

namespace ManejadorDePresupuestos_MVC.Models
{
    //V#125 Formulario de Cuentas (Creando el modelo)

    /// <summary>
    /// Esta clase contiene los campos de la tabla Cuentas.
    /// </summary>
    public class CuentaViewModel
    {
        //Id ya que es necesario interactuar con una BD
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(maximumLength: 50)]
        [PrimerLetraMayuscula]        
        public string Nombre { get; set; }

        [Display(Name = "Tipo Cuenta")] //Este texto va a mostrar el DropDownList
        public int TipoCuentaId { get;  set; } 

        public decimal Balance { get; set; }

        [StringLength(maximumLength: 1000)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        //V#127 Indice de Cuentas - Query (Agregando la nueva propiedad del modelo luego de crear el INNER JOIN)
        public string TipoCuenta { get; set; }
    }
}
