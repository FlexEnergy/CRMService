using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSphere.Protos;
using SmartSphere.CRM.Protos;
using SmartSphere.Database.Raven.Entities;
using Raven.Client.Documents.Session;
using SmartSphere.Database.Raven;
using SmartSphere.Logs;
using System.Reflection;
using Google.Protobuf.WellKnownTypes;
using Google.Protobuf.Collections;

namespace SmartSphere.CRM.Helpers
{
    internal static class Formulae
    {
        internal static string CustomerCode(IDocumentSession session, string businessID)
        {
            try
            {
                Business.Entities.Business _business = session.Load<Business.Entities.Business>(businessID);
                if (_business != null)
                {
                    if (_business.CustomerCode.TypeOfCode == TypeOfCode.Counter)
                    {
                        if (_business.CustomerCode.CounterName == "Self")
                            return Counter_Self(session, _business);
                        else
                            return Counter_Name(session, _business);
                    }
                    else if (_business.CustomerCode.TypeOfCode == TypeOfCode.Custom)
                    {

                    }
                    else if (_business.CustomerCode.TypeOfCode == TypeOfCode.Erp)
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

        private static string Counter_Self(IDocumentSession session, Business.Entities.Business business)
        {
            business.CustomerCode.Counter += 1;
            session.SaveChanges();

            return business.CustomerCode.ToString();
        }

        private static string Counter_Name(IDocumentSession session, Business.Entities.Business business)
        {
            Database.Entities.Counter _counter = session.Load<Database.Entities.Counter>(business.CustomerCode.CounterName);
            _counter.Code += 1;   
            session.SaveChanges();

            return _counter.Code.ToString();
        }

    }
}
