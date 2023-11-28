using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ManejadorDePresupuestos_MVC.Models
{
    //V#138 Creando Transacciones (Creando el Modelo de Transacciones min 02:30)
    public class TransaccionesViewModel
    {
        //Necesitamos realizar validaciones
        public int Id { get; set; }

        public int UsuarioId { get; set; }  

        public DateTime FechaTransaccion { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public decimal Monto { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una {0}")]
        [DisplayName("Categoria")]
        public int CategoriaId { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una {0}")]
        [DisplayName("Cuenta")]
        public int CuentaId { get; set; }

        //public int TipoOperacionId { get; set; }

        [StringLength(maximumLength: 1000, ErrorMessage = "No debe exeder de los {1} caracteres.")]
        public string Observaciones { get; set; }
    }
}
