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


namespace SmartSphere.CRM.Database.Index
{
    internal static class Manager
    {
        internal static void Run() {
            try
            {
                Log.Message(Severities.INFO, "0006", "Index manager", "IndexManager", MethodBase.GetCurrentMethod().Name);
           
                using IDocumentSession _session = DocumentStoreHolder.Store.OpenSession();

                new Customers_Index().Execute(DocumentStoreHolder.Store, DocumentStoreHolder.Store.Conventions);
                new Organisations_Index().Execute(DocumentStoreHolder.Store, DocumentStoreHolder.Store.Conventions);
                new Business_Index().Execute(DocumentStoreHolder.Store, DocumentStoreHolder.Store.Conventions);
            }

            catch (Exception ex)
            {
                Log.Message(Severities.FATAL, "0004", "Fatal exception", "IndexManager", MethodBase.GetCurrentMethod().Name, text2:ex.Message);
            }      
        
        }
    }
}
