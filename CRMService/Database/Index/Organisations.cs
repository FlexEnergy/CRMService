using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using SmartSphere.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Reflection;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using SmartSphere.Database;
using System.Threading;

using Raven.Client.Documents.Indexes;
using Google.Protobuf.Collections;
using SmartSphere.Database.Raven;
using SmartSphere.Logs;
using SmartSphere.CRM.Organisations.Entities;

namespace SmartSphere.CRM.Database.Index
{
    public class Organisations_Index : AbstractIndexCreationTask<Organisation>
    {
        public Organisations_Index()
        {
            Map = values => from dataset in values
                            select new
                            {
                                ContactID = dataset.ContactID                   
                            };
        }
    }
  
}
