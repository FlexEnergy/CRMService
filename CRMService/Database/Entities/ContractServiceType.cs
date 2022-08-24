using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSphere.Protos;
using SmartSphere.CRM.Protos;

namespace SmartSphere.CRM.Database.Entities
{

    public class ContractServiceType
    {
        public string Type { get; set; }
        public string Language { get; set; }
        public string Name { get; set; } 
    }

}
