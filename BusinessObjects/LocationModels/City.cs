using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.LocationModels
{
    public class District
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public List<Ward> Wards { get; set; } = new List<Ward>();
    }
}
