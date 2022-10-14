using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSphere.Protos;
using SmartSphere.CRM.Protos;
using SmartSphere.Financial;

namespace SmartSphere.CRM.Database.Entities
{
    public class Contract
    {
        public string ContractID { get; set; }
        public string ContractCode { get; set; }
        public DateTimeOffset Date1 { get; set; }
        public DateTimeOffset Date2 { get; set; }
        public List<ContractService> ContractServices { get; set; }
    }

    public class ContractService
    {
        public DateTimeOffset Date1 { get; set; }
        public DateTimeOffset Date2 { get; set; }
        public AccountingInterval AccountingInterval { get; set; }
        public string ContractServiceID { get; set; }
        public string ContractServiceType { get; set; }
    }

}
