using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSphere.Protos;
using SmartSphere.CRM.Protos;

namespace SmartSphere.CRM.Customers.Entities
{
    public class Customer
    {
        public string Code { get; set; }
        public string ContactID { get; set; }
        public string BusinessID { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public string OwnerID { get; set; }
    }
}
