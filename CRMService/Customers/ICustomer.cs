using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSphere.Protos;
using SmartSphere.CRM.Protos;

namespace SmartSphere.CRM.Customers
{
    internal interface ICustomer
    {
        internal CommonResponse Create(CustomerMessage request);
        internal CommonResponse Update(CustomerMessage request);
        internal CommonResponse Remove(CustomerMessage request);
        internal CustomerListMessage Search(CustomerFilterMessage request);
    }
}
