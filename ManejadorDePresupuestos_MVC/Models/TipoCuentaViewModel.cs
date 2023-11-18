using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Timers;

namespace ManejadorDePresupuestos_MVC.Models
{
    //V# 100 FORMULARIO TIPOS CUENTAS
    //Creando el modelo de Tipos de cuentas

    /// <summary>
    /// Contiene los campos de la tabla Tbl_TiposCuentas_Sys.
    /// </summary>
    public class TipoCuentaViewModel
    {
        //V#101 VALIDANDO EL FORMULARIO( Nunca confiar en la data que envía el usuario)

        public int Id { get; set; }

        //V#102 Visualizando los Errores de validación
        [Required(ErrorMessage = "* El campo {0} es obligatorio.")]
        [StringLength(maximumLength:50, MinimumLength = 3, ErrorMessage = "La longitud del campo {0} debe ser mayor a {2} y menor a {1}")]
        public string Nombre { get; set; }
        [Required]
        public int UsuarioId { get; set; }
        [Required]
        public int Orden { get; set; }

        /*PRUEBAS DE VALIDACIONES POR DEGECTO*/
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [EmailAddress(ErrorMessage = "El campo debe ser un {0} válido")]
        [DisplayName("Correo electrónico")]
        public string Email { get; set; }

        [Range(minimum: 18, maximum: 100, ErrorMessage = "El valor debe estar entre {1} y {2}")]
        public int Edad { get; set; }

        [Url(ErrorMessage = "Debe ser una {0} válida")]
        public string URL { get; set; }

        [CreditCard(ErrorMessage = "La Tarjeta de crédito no es válida")]
        [DisplayName("Tarjeta de Crédito")]
        public string TarjetaDeCredito { get; set; }
    }
}
