using ManejadorDePresupuestos_MVC.Validations;
using Microsoft.AspNetCore.Mvc;
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
    public class TipoCuentaViewModel : IValidatableObject //V#108 Validaciones personalizadas por modelos (Ctl+. para la interfaz)
    {
        //V#101 VALIDANDO EL FORMULARIO( Nunca confiar en la data que envía el usuario)

        //Id ya que sera necesario interactuar con la bd
        public int Id { get; set; }

        //V#102 Visualizando los Errores de validación
        [Required(ErrorMessage = "* El campo {0} es requerido.")]
        [StringLength(maximumLength:50, MinimumLength = 3, ErrorMessage = "La longitud del campo {0} debe ser mayor a {2} y menor a {1}")]
        //[PrimerLetraMayuscula]    //V#107 Validaciones personalizadas por atributos (no es necesario poner el Attribute)

        //Remote se comunica desde el nav hasta el servidor (Action Httpget, Controller)
        [Remote(action: "VerificarExisteTipoCuenta", controller: "TiposCuentas")] //V#114 Validaciones personalizadas con JavaScript utilizando Remote (Aplicando el DA)
        public string Nombre { get; set; }

        [Required(ErrorMessage = "* El campo {0} es requerido.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "* El campo {0} es requerido.")]
        public int Orden { get; set; }


        #region ////V#104 OTRAS VALIDACIONES POR DEFECTO
        ///*Pruebas de validaciones por degecto*/
        //[Required(ErrorMessage = "* El campo {0} es requerido.")]
        //[EmailAddress(ErrorMessage = "El campo debe ser un {0} válido")]
        //[DisplayName("Correo electrónico")]
        //public string Email { get; set; }

        //[Range(minimum: 18, maximum: 100, ErrorMessage = " El valor debe estar entre {1} y {2}")]
        //public int Edad { get; set; }

        //[Url(ErrorMessage = "Debe ser una {0} válida")]
        //public string URL { get; set; }

        //[CreditCard(ErrorMessage = "La Tarjeta de crédito no es válida")]
        //[DisplayName("Tarjeta de Crédito")]
        //public string TarjetaDeCredito { get; set; } 
        #endregion

        //V#108 Validaciones personalizadas por modelos (quitar V#107)
        //Permiten realizarse dentro de las clases
        //Permite validar varios campos a la vez
        //Pero obligatoriamente tiende a estar ligada al modelo
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Es posible realizar una validación compleja que involucre todos los campos del modelo
            //Verifica si el campo nombre es distinto a nulo o no esta vacío
            if(Nombre != null && Nombre.Length > 0)
            {
                //Contiene la primera letra del campo 
                var primeraLetra = Nombre[0].ToString();

                //Si primeraLetra es distinto a primeraLetra en mayúscula 
                if(primeraLetra != primeraLetra.ToUpper())
                {
                    //Devuelve el mensaje de error para el campo especifico
                    //Se especifica de esta manera , new[] { nameof(Nombre) } error a nivel de campo
                    //En caso de no especificarlo se toma como a nivel general (se tomo como error a nivel del modelo)
                    yield return new ValidationResult("La primera letra debe ser mayúscula.", new[] { nameof(Nombre) });
                }
            }
        }
    }
}


