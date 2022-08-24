using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSphere.Protos;
using SmartSphere.CRM.Protos;

namespace SmartSphere.CRM.Organisations.Entities
{
    public class Organisation
    {
        public string ContactID { get; set; }
        public string Description { get; set; }
    }
}
