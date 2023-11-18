namespace ManejadorDePresupuestos_MVC.Models
{
    //V# 100 FORMULARIO TIPOS CUENTAS
    //Creando el modelo de Tipos de cuentas

    /// <summary>
    /// Contiene los campos de la tabla Tbl_TiposCuentas_Sys.
    /// </summary>
    public class TipoCuentaViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }       
    }
}
