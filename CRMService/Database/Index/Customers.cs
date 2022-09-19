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
using SmartSphere.CRM.Database.Entities;

namespace SmartSphere.CRM.Database.Index
{
    public class Customers_Index : AbstractIndexCreationTask<Customer>
    {
        public Customers_Index()
        {
            Map = values => from dataset in values
                            select new
                            {
                                ContactID = dataset.ContactID,
                                BusinessID = dataset.BusinessID,
                                CustomerCode = dataset.CustomerCode
                            };
        }
    }

    public class Customers_ContractAccounts : AbstractIndexCreationTask<Customer>
    {
        public Customers_ContractAccounts()
        {
            Map = values => from dataset in values
                            from ca in dataset.ContractAccounts 
                            from co in ca.Contracts
                            select new
                            {
                                ContactID = dataset.ContactID,
                                BusinessID = dataset.BusinessID,
                                CustomerCode = dataset.CustomerCode,
                                ContractAccountID = ca.ContractAccountID,
                                ContractAccountCode = ca.ContractAccountCode,
                                ContractID = co.ContractID,
                                ContractCode = co.ContractCode
                            };
        }
    }

}
