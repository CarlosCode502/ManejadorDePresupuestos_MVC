namespace ManejadorDePresupuestos_MVC.Services
{
    //V#116 Evitando repetir código (Creando un servicio que contenga el UsuarioId para cambiarlo cualquier momento)
    public class ServicioUsuarios : IServicioUsuarios
    {
        public int ObtenerUsuarioID() //C+. para implementar en la interfaz (Pull up)
        {
            return 1;
        }
    }
}
