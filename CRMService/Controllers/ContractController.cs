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

namespace SmartSphere.CRM.Controllers
{
    internal class ContractController
    {
        internal static Response Add(Contract request)
        {
            Response _response = new() { ResponseState = ResponseState.Failed};
            try
            {
                //if (request.HasCustomerID && request.HasContractAccountID)
                //{
                //    using IDocumentSession _session = DocumentStoreHolder.Store.OpenSession();
                //    _session.Advanced.WaitForIndexesAfterSaveChanges();

                //    var _contractAccount = ContractAccountController.Search(_session, request.CustomerID, request.ContractAccountID);
                //    if (_contractAccount != null)
                //    {
                //        string _contractID = Guid.NewGuid().ToString();
                //        Database.Entities.Contract _contract = new ()
                //        {
                //            ContractID = _contractID,
                //            ContractCode = "",
                //            Date1 = request.Date1.ToDateTimeOffset(),
                //            Date2 = request.Date1.ToDateTimeOffset(),
                //            ContractServices = new List<Database.Entities.ContractService>()
                //        };

                //        //if(request.ContractServices.Count > 0)
                //        //{
                //        //    foreach (var item in request.ContractServices)
                //        //    {
                //        //        if(item.HasContractServiceID && item.HasContractServiceType )
                //        //            _contract.ContractServices.Add(new Database.Entities.ContractService
                //        //            {
                //        //                ContractServiceID = item.ContractServiceID,
                //        //                ContractServiceType = item.ContractServiceType
                //        //            });
                //        //    }                            
                //        //}

                //        _session.SaveChanges();
                //        _response.ResponseState = ResponseState.Ok;
                //        _response.ID = _contractID;

                //        Log.Message(Severities.INFO, "0102", "Contract create", string.Empty, MethodBase.GetCurrentMethod().Name, text1: $"ContractID = {_contractID}");                        
                //    }
                //    else
                //    {
                //        Log.Message(Severities.ERROR, "0102", "Contract create", string.Empty, MethodBase.GetCurrentMethod().Name, text1: $"CustomerID:{request.CustomerID} not found");    
                //    }
                //}
                //else
                //{
                //    Log.Message(Severities.ERROR, "0102", "Contract create", string.Empty, MethodBase.GetCurrentMethod().Name, text1: "Missing mandatory data");
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

        internal static Database.Entities.Contract Search(IDocumentSession session, string customerID, string contractAccountID, string contractID)
        {
            var _contractAccount = ContractAccountController.Search(session, customerID, contractAccountID);
            var _query = from s in _contractAccount.Contracts
                         where s.ContractID == contractID
                         select s;

            foreach (var item in _query)
                return item;

            return null;
        }

        internal static Response AddService(ContractService request)
        {
            Response _response = new() { ResponseState = ResponseState.Failed };
            try
            {
                //if (request.HasCustomerID && request.HasContractAccountID && request.HasContractID)
                //{
                //    using IDocumentSession _session = DocumentStoreHolder.Store.OpenSession();
                //    _session.Advanced.WaitForIndexesAfterSaveChanges();

                //    var _contract = Search(_session, request.CustomerID, request.ContractAccountID,request.ContractID);
                //    if (_contract != null)                    {
                                   
                //        Database.Entities.ContractService _contractService = new()
                //        {
                //            ContractServiceID = request.ContractServiceID,
                //            ContractServiceType = request.ContractServiceType  
                //        };
                //        _contract.ContractServices.Add(_contractService);
                //        _session.SaveChanges();
                //        _response.ResponseState = ResponseState.Ok;
                //        _response.ID = request.ContractServiceID;

                //        Log.Message(Severities.INFO, "0102", "ContractService create", string.Empty, MethodBase.GetCurrentMethod().Name, text1: $"ContractServiceID = {request.ContractServiceID}");
                //    }
                //    else
                //    {
                //        Log.Message(Severities.ERROR, "0102", "ContractService create", string.Empty, MethodBase.GetCurrentMethod().Name, text1: $"Data not found");
                //    }
                //}
                //else
                //{
                //    Log.Message(Severities.ERROR, "0102", "ContractService create", string.Empty, MethodBase.GetCurrentMethod().Name, text1: "Missing mandatory data");
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


    }
}
