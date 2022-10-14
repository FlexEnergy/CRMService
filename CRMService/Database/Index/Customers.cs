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
using Raven.Client.Documents.Linq.Indexing;

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
        public class Result
        {
            public string ContactID { get; set; }
            public string BusinessID { get; set; }
            public string CustomerCode { get; set; }
            public string ContractAccountID { get; set; }
            public string ContractAccountCode { get; set; }
            public string ContractID { get; set; }
            public string ContractCode { get; set; }
            public string ContractServiceID { get; set; }
            public string ContractServiceType { get; set; }
        }

        public Customers_ContractAccounts()
        {
            Map = values => from dataset in values
                            from ca in dataset.ContractAccounts 
                            from co in ca.Contracts
                            from cos in co.ContractServices
                            select new
                            {
                                ContactID = dataset.ContactID,
                                BusinessID = dataset.BusinessID,
                                CustomerCode = dataset.CustomerCode,
                                ContractAccountID = ca.ContractAccountID,
                                ContractAccountCode = ca.ContractAccountCode,
                                ContractID = co.ContractID,
                                ContractCode = co.ContractCode,
                                ContractServiceID = cos.ContractServiceID,
                                ContractServiceType = cos.ContractServiceType
                            };
        }
    }

}
