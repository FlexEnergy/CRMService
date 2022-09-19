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
    public class Customer
    {
        public string CustomerCode { get; set; }
        public string ContactID { get; set; }
        public string BusinessID { get; set; }
        public List<ContractAccount> ContractAccounts { get; set; }
    }
}
