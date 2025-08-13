using BusinessObjects.LocationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ILocationService
    {
        Task<List<Province>> GetProvinces();
        Task<List<District>> GetCities(string proCode);
        Task<List<Ward>> GetWards(string cityCode);


    }
}
