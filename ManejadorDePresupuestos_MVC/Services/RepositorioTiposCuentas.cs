﻿using Dapper;
using ManejadorDePresupuestos_MVC.Models;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ManejadorDePresupuestos_MVC.Services
{
    //V#110 Insertando un Tipo de Cuenta en la Base de Datos (Creando el Repositorio)
    public class RepositorioTiposCuentas : IRepositorioTiposCuentas
    {
        //NO ES CORRECTO MANEJAR LA LOGICA DENTRO DEL CONSTRUCTOR YA QUE ESTAMOS VIOLENTANDO
        //EL PRINCIPIO DE RESPONSABILIDAD UNICA 

        //A este campo se le va a poder asignar la cadena de conexión
        private readonly string connectionString;

        /// <summary>
        /// Obtiene la cadena de conexión del appsettings (perfiles de configuración)
        /// </summary>
        /// <param name="configuration"></param>
        public RepositorioTiposCuentas(IConfiguration configuration)
        {
            //Cadena de conexión obtenida a través de la IConfig desde Perfiles de Config.
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        #region //Antes de V#112 Aplicando la programación Asíncrona
        //V#110 Insertando un Tipo de Cuenta en la Base de Datos (Creando el Repositorio)
        ///// <summary>
        ///// Permite crear un TipoCuenta en la BD.
        ///// </summary>
        ///// <param name="tipoCuentaViewModel">Recibe el modelo de TipoCuentaViewModel</param>
        //public void Crear(TipoCuentaViewModel tipoCuentaViewModel) //Ctrl + . para agregar al IRepositorio
        //{
        //    //Abre una nueva conexión
        //    using var connection = new SqlConnection(connectionString);

        //    //Consulta que obtiene el id(int) del registro
        //    //Dapper -> QuerySingle (Realiza un query que va a obtener un solo resultado). 
        //    var id = connection.QuerySingle<int>($@"INSERT INTO Tbl_TiposCuentas_Sys (Nombre, UsuarioId, Orden)
        //                                            Values (@Nombre, @UsuarioId, 0);
        //                                            SELECT SCOPE_IDENTITY();", tipoCuentaViewModel);
        //    //SCOPE_IDENTITY() -> Trae el id del registro creado

        //    //Asigna el id obtenido al campo id del modelo tipo cuenta 
        //    tipoCuentaViewModel.Id = id;
        //}
        #endregion

        //V#112 Aplicando la programación Asíncrona(Es útil cuando existe una comunicación con externos)
        //Task (es como un void asincrono)
        //V#110 Insertando un Tipo de Cuenta en la Base de Datos (Creando el Repositorio)
        /// <summary>
        /// Permite crear un TipoCuenta en la BD.
        /// </summary>
        /// <param name="tipoCuentaViewModel">Recibe el modelo de TipoCuentaViewModel</param>
        public async Task Crear(TipoCuentaViewModel tipoCuentaViewModel) //Ctrl + . para agregar al IRepositorio
        {
            //Abre una nueva conexión
            using var connection = new SqlConnection(connectionString);

            //Consulta que obtiene el id(int) del registro
            //Dapper -> QuerySingle (Realiza un query que va a obtener un solo resultado). 
            var id = await connection.QuerySingleAsync<int>
                (@"INSERT INTO Tbl_TiposCuentas_Sys (Nombre, UsuarioId, Orden)
                    Values (@Nombre, @UsuarioId, 0);
                    SELECT SCOPE_IDENTITY();", tipoCuentaViewModel); //SCOPE_IDENTITY() -> Trae el id del registro creado

            //Asigna el id obtenido al campo id del modelo tipo cuenta 
            tipoCuentaViewModel.Id = id;
        }

        //V# 113 Validaciones personalizadas a Nivel del Controlador 
        //Creando un método que valide si existe un TipoCuenta para determinado usuario
        /// <summary>
        /// Método asíncrono que valida si un usario ya tiene 1 TipoCuenta
        /// </summary>
        /// <param name="nombre">Se asigna el nombre de la BD</param>
        /// <param name="usuarioId">Se asigna el usuarioId de la BD</param>
        /// <returns>Un booleano  (true o false), 1 si existe y 0 si no</returns>
        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            //Obtenemos y abre la cadena de conexión
            using var connection2 = new SqlConnection(connectionString);

            //Se crea la consulta para traer al PrimerElementoOPordefecto que se encuentre (Dapper)
            //QueryFirstOrDefaultAsync Obtiene el primero que encuentre o un valor por defecto(igual a 0)
            var existe = await connection2.QueryFirstOrDefaultAsync<int>
                //Select 1 obtiene el primer registro de la tabla cuando nombre y usuarioId correspondan
                (@"SELECT 1 
                FROM Tbl_TiposCuentas_Sys
                WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId;", new {nombre, usuarioId});
            //Funciona muy bien para ver la existencia de un registro sin traer el registro(para mostrarlo)

            //Como el método recibe un boleano se le envia si es (true -> 1 o false -> 0)
            return existe == 1;
        }
    }
}