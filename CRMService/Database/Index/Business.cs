using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Google.Protobuf.WellKnownTypes;
using System.Reflection;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using SmartSphere.Database;
using System.Threading;
using Raven.Client.Documents.Indexes;
using SmartSphere.Database.Raven;
using SmartSphere.Logs;
using SmartSphere.CRM.Database.Entities;

namespace SmartSphere.CRM.Database.Index
{
    public class Business_Index : AbstractIndexCreationTask<Entities.Business>
    {
        //public Business_Index()
        //{
        //    Map = values => from dataset in values
        //                    select new
        //                    {
        //                        ContactID = dataset.ContactID,
        //                        OrganisationID = dataset.OrganisationID
        //                    };
        //}
    }

    public class Business_Codes : AbstractIndexCreationTask<Organisation, Business>
    {
        public Business_Codes()
        {
            Map = orgs => from m in orgs                             
                          from mv in m.Businesses                        
                          select new                        
                          {              
                              BusinessID = mv.BusinessID,
                              CustomerCode = mv.CustomerCode
                          };

            Reduce = results => from result in results
                                group result by new { result.BusinessID, result.CustomerCode } into g
                                select new
                                {
                                    BusinessID = g.Key.BusinessID,
                                    CustomerCode = g.Key.CustomerCode
                                };
        }
    }



}
