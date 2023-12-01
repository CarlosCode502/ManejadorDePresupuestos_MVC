using ManejadorDePresupuestos_MVC.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ManejadorDePresupuestos_MVC.Models
{
    //V#133 Creando Categorias (Creando el Modelo Categoria)
    public class CategoriaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [PrimerLetraMayuscula]
        [StringLength(maximumLength:50, ErrorMessage = "No puede ser mayor a {1} caracteres")]
        [DisplayName("Nombre de Categoría")]
        public string NombreCategoria { get; set; }

        //Debe poder elegir el tipodeOperación
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public TipoOperacionEnum TipoOperacionId { get; set; } //Col TipoOperacionId

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int UsuarioId { get; set; }
    }
}
