using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSphere.Protos;
using SmartSphere.CRM.Protos;

namespace SmartSphere.CRM.Database.Models
{
    public class Code
    {
        public TypeOfCode TypeOfCode { get; set; }
        public string Formulae { get; set; }
        public string CounterName { get; set; }
        public int Counter { get; set; }

    }
}
