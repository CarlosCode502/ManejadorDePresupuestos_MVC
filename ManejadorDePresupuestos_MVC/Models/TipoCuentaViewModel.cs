using System.ComponentModel.DataAnnotations;
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
        public int UsuarioId { get; set; }
        public int Orden { get; set; }       
    }
}
