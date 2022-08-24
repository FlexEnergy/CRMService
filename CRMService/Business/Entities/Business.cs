using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSphere.Protos;
using SmartSphere.CRM.Protos;
using SmartSphere.CRM.Database.Models;

namespace SmartSphere.CRM.Business.Entities
{
    public class Business
    {
        public string ContactID { get; set; }
        public string OrganisationID { get; set; }
        public string Description { get; set; }
        public Database.Models.Code CustomerCode { get; set; }
    }
}
