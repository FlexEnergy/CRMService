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
    public class Contract
    {
        public string Code { get; set; }
        public DateTimeOffset Date1 { get; set; }
        public DateTimeOffset Date2 { get; set; }
        public List<ContractService> ContractServices { get; set; }
    }

    public class ContractService
    {
        public string Type { get; set; }
        public string ID { get; set; }
    }

}
