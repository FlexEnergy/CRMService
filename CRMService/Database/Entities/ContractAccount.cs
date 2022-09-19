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
    public class ContractAccount
    {
        public string ContractAccountID { get; set; }
        public string ContractAccountCode { get; set; }
        public DateTimeOffset Date1 { get; set; }
        public DateTimeOffset Date2 { get; set; }
        public List<Contract> Contracts { get; set; }
    }
}
