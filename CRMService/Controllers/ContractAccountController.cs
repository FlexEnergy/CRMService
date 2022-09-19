using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSphere.Protos;
using SmartSphere.CRM.Protos;
using SmartSphere.Database.Raven.Entities;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using SmartSphere.Database.Raven;
using SmartSphere.Logs;
using System.Reflection;
using Google.Protobuf.WellKnownTypes;
using Google.Protobuf.Collections;
using System.Runtime.CompilerServices;
using SmartSphere.Directory.Protos;
using Azure.Core;
using FlexEnergy.Protos;
using Newtonsoft.Json.Linq;

namespace SmartSphere.CRM.Controllers
{
    internal class ContractAccountController
    {
        internal static Response Add(ContractAccount request)
        {
            Response _response = new() { ResponseState = ResponseState.Failed };
            try
            {    
                //if(request.HasCustomerID)
                //{
                //    using IDocumentSession _session = DocumentStoreHolder.Store.OpenSession();
                //    _session.Advanced.WaitForIndexesAfterSaveChanges();

                //    Database.Entities.Customer _customer = CustomerController.Search(_session,request.CustomerID);

                //    if (_customer != null)
                //    {
                //        string _contractAccountID = Guid.NewGuid().ToString();
                //        Database.Entities.ContractAccount _contractAccount = new()
                //        {
                //            ContractAccountID = _contractAccountID,
                //            ContractAccountCode = "",
                //            Date1 = request.Date1.ToDateTimeOffset(),
                //            Date2 = request.Date2.ToDateTimeOffset(),
                //            Contracts = new List<Database.Entities.Contract>()
                //        };
                //        _customer.ContractAccounts.Add(_contractAccount);

                //        _session.SaveChanges();
                //        _response.ResponseState = ResponseState.Ok;
                //        _response.ID = _contractAccountID;

                //        Log.Message(Severities.INFO, "0102", "ContractAccount create", string.Empty, MethodBase.GetCurrentMethod().Name, text1: $"ContractAccountID = {_contractAccountID}");
                //    }
                //    else
                //    {
                //        Log.Message(Severities.ERROR, "0102", "ContractAccount create", string.Empty, MethodBase.GetCurrentMethod().Name, text1: $"Data not found");
                //    }
                //}
                //else
                //{
                //    Log.Message(Severities.ERROR, "0102", "ContractAccount create", string.Empty, MethodBase.GetCurrentMethod().Name, text1: "Missing mandatory data");  
                //}

                return _response;
            }
            catch (Exception ex)
            {
                Log.Message(Severities.FATAL, "0004", "Fatal exception", string.Empty, MethodBase.GetCurrentMethod().Name, text2: ex.Message);
                _response.ResponseState = ResponseState.Failed;
                return _response;
            }
        }

        internal static Database.Entities.ContractAccount Search(IDocumentSession session, string customerID, string contractAccountID)
        {
            Database.Entities.Customer _customer = CustomerController.Search(session, customerID);
            var _query = from s in _customer.ContractAccounts
                         where s.ContractAccountID == contractAccountID
                         select s;

            foreach (var item in _query)
                return item;      
               
            return null;
        }

        internal static void ContractAccountDTO(Database.Entities.Customer item, Customer customer)
        {
            foreach (var obj in item.ContractAccounts)
            {

                ContractAccount _contractAccount = new();
                if (IsEmpty(obj.ContractAccountID))
                    _contractAccount.ContractAccountID = obj.ContractAccountID;
                if (IsEmpty(obj.ContractAccountCode))
                    _contractAccount.ContractAccountCode = obj.ContractAccountCode;


                _contractAccount.Date1 = obj.Date1.ToUniversalTime().ToTimestamp();
                _contractAccount.Date2 = obj.Date2.ToUniversalTime().ToTimestamp();

                foreach (var ob in obj.Contracts)
                    ContractDTO(ob, _contractAccount);             

                customer.ContractAccounts.Add(_contractAccount);
            }
        }

        private static void ContractDTO(Database.Entities.Contract item, ContractAccount contractAccount)
        {

            Contract _contract = new();
            if (IsEmpty(item.ContractID))
                _contract.ContractID = item.ContractID;
            if (IsEmpty(item.ContractCode))
                _contract.ContractCode = item.ContractCode;

            _contract.Date1 = item.Date1.ToUniversalTime().ToTimestamp();
            _contract.Date2 = item.Date2.ToUniversalTime().ToTimestamp();

            RepeatedField<string> _contractServiceIDs = new();
            CSeFilter _reqFilter = new()
            {
                Additionals = new CSeFilterAdditonal() { OnlyActive = false}
            };

            foreach (var ob1 in item.ContractServices)
            {
                _contractServiceIDs.Add(ob1.ContractServiceID);
                _contract.ContractServices.Add(new ContractService
                {
                    ContractServiceID = ob1.ContractServiceID,
                    ContractServiceType = new SmartSphere.Protos.KeyValues { Key = ob1.ContractServiceType }
                });
            };

            var _contractServices =  Task.Run(() => ContractServiceController.GetContractServices(_contractServiceIDs,_reqFilter));

            foreach (var ob1 in _contract.ContractServices)
            {
                var _query = from s in _contractServices.Result
                             where s.ContractServiceID == ob1.ContractServiceID
                             select s;

                foreach (var ob2 in _query)
                {
                    ContractServiceDTO(ob1, ob2);
                }
            }

            contractAccount.Contracts.Add(_contract);


        }



        private static void ContractServiceDTO(ContractService item, FlexEnergy.Protos.ContractServiceElectrical contractService)
        {
            if (contractService.HasContractServiceName)
                item.ContractServiceName = contractService.ContractServiceName;
            if (contractService.HasDescription)
                item.Description = contractService.Description;
            if (contractService.HasZPB)
                item.ZPB = contractService.ZPB;

            item.Date1 = contractService.Date1;
            item.Date2 = contractService.Date2;
        }

        private static bool IsEmpty(string value)
        {
            if (value == null)
                return false;
            else if (value.Length == 0)
                return false;
            else if (value == string.Empty)
                return false;
            else
                return true;
        }
    }
}
