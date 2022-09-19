using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmartSphere.CRM.Protos;
using SmartSphere.Database.Raven.Entities;
using Raven.Client.Documents.Session;
using SmartSphere.Database.Raven;
using SmartSphere.Logs;
using System.Reflection;
using Google.Protobuf.WellKnownTypes;
using Google.Protobuf.Collections;
using Raven.Client.Documents.Linq;
using SmartSphere.CRM.Database.Entities;
using Azure.Core;


namespace SmartSphere.CRM.Helpers
{
    internal static class Formulae
    {
        internal static string CustomerCode(IDocumentSession session, string organisationID, string businessID)
        {
            try
            {
                Database.Entities.Organisation _org = session.Load<Database.Entities.Organisation>(organisationID);

                var _query = from s in _org.Businesses
                             where s.BusinessID == businessID
                             select s;

                foreach (var item in _query)
                {
                    string _id = session.Advanced.GetDocumentId(item);

                    if (item.CustomerCode.TypeOfCode == TypeOfCode.Counter)
                    {
                        if (item.CustomerCode.CounterName == "Self")
                            return Counter_Self(session, item);
                        else
                            return Counter_Name(session, item);
                    }
                    else if (item.CustomerCode.TypeOfCode == TypeOfCode.Custom)
                    {

                    }
                    else if (item.CustomerCode.TypeOfCode == TypeOfCode.Erp)
                    {

                    }
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string Counter_Self(IDocumentSession session, Database.Entities.Business business)
        {
            business.CustomerCode.Counter += 1;              
            return business.CustomerCode.Counter.ToString();
        }

        private static string Counter_Name(IDocumentSession session, Database.Entities.Business business)
        {
            Counter _counter = session.Load<Counter>(business.CustomerCode.CounterName);
            _counter.Code += 1;   
    
            return _counter.Code.ToString();
        }

    }
}
