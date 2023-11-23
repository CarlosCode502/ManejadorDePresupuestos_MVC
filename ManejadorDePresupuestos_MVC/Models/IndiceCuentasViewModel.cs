namespace ManejadorDePresupuestos_MVC.Models
{
    //V#127 Indice de Cuentas - Query (Creando modelo que contendra los indices y sumatoria de balances )
    //Representa el modelo de la vista index que se va a crear
    public class IndiceCuentasViewModel
    {
        //
        public string TipoCuenta { get; set; }

        //
        public IEnumerable<CuentaViewModel> CuentasIndice { get; set;}
        
        //Sumatoria de los balances segun indice (Balance va a ser igual a los distintos balances por tipoCuenta)
        public decimal Balance => CuentasIndice.Sum(x => x.Balance);
    }
}
