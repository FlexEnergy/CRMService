using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSphere.Protos;
using SmartSphere.CRM.Protos;

namespace SmartSphere.CRM.Organisations
{
    internal interface IOrganisation
    {
        internal Response Create(Organisation request);
        internal Response Update(Organisation request);
        internal Response Remove(Organisation request);
        internal OrganisationList Search(OrganisationFilter request);
    }
}
