namespace ManejadorDePresupuestos_MVC.Services
{
    //V#133 Creando Categorias (Creando el RepositorioCategorias)
    public class RepositorioCategorias : IRepositorioCategorias //Va heredar de la IRepoCategorias
    {
        //Campo para sacar o obtener la cadena de conexion de AppSettings.json
        private readonly string connectionString;

        //Constructor para inicializar IConfiguration
        public RepositorioCategorias(IConfiguration configuration)
        {
            //Obtiene la cadena de conexion y la almacena aquí
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

    }
}
