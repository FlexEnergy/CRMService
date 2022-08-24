using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSphere.Protos;
using SmartSphere.CRM.Protos;
using SmartSphere.CRM.Database.Models;

namespace SmartSphere.CRM.Database.Entities
{
    public class Counter
    {
        public string Description { get; set; }
        public int Code { get; set; }
    }
}
