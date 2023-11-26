using AutoMapper;
using ManejadorDePresupuestos_MVC.Models;

namespace ManejadorDePresupuestos_MVC.Services
{
    //V#131 Utilizando AutoMapper (Indicando clase1 a clase 2 que pueda mapear clase profile)
    public class AutoMapperProfiles : Profile //Hereda de profile que es de AutoMapper
    {
        public AutoMapperProfiles() 
        {
            //Va mapear de CuentaViewModel a DropDownCuentaViewModel
            CreateMap<CuentaViewModel, DropDownCuentaViewModel>();            
        }
    }
}
