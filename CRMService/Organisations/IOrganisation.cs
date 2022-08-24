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
        internal CommonResponse Create(Organisation request);
        internal CommonResponse Update(Organisation request);
        internal CommonResponse Remove(Organisation request);
        internal OrganisationList Search(OrganisationFilter request);
    }
}
