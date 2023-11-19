using System.ComponentModel.DataAnnotations;

namespace ManejadorDePresupuestos_MVC.Validations
{
    //V#107 Validaciones personalizadas por atributos
    /// <summary>
    /// Contiene una validación personalizada para que la primera letra deba ser mayuscula.
    /// </summary>
    public class PrimerLetraMayusculaAttribute : ValidationAttribute
    {
        /// <summary>
        /// A través de override podemos sobrescribir una validación y asignarle otro comportamiento.
        /// </summary>
        /// <param name="value">Valor o el Campo a validar</param>
        /// <param name="validationContext"></param>
        /// <returns>Un msj o texto de error</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ////Verifica si el campo es igual a nulo o contiene un string vacio
            //if(value == null || string.IsNullOrEmpty(value.ToString()))
            //{
            //    //Devuelve una validación exitosa, dado que no contiene ningun valor a verificar
            //    return ValidationResult.Success;
            //}

            //Obtener el primer elemento de un string y se almacena en una variable
            var primeraLetra = value.ToString()[0].ToString();  

            //Verifica si la primera letra es distinta a primeraletra pero en mayuscula
            if(primeraLetra != primeraLetra.ToUpper())
            {
                //Se procede a mostrar el mensaje de error
                return new ValidationResult("La primera letra debe ser Mayúscula");
            }

            //Al final devuelve una validación exitosa si no se ejecuta ninguna condición
            return ValidationResult.Success;
        }
    }
}
