using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSphere.Protos;
using SmartSphere.CRM.Protos;

namespace SmartSphere.CRM.Business
{
    internal interface IBusiness
    {
        internal CommonResponse Create(BusinessMessage request);
        internal CommonResponse Update(BusinessMessage request);
        internal CommonResponse Remove(BusinessMessage request);
        internal BusinessListMessage Search(BusinessFilterMessage request);
    }
}
