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
using Grpc.Core;
using SmartSphere.Directory.Protos;

namespace SmartSphere.CRM.Controllers
{
    internal static class CustomerController
    {
        internal static Response Create(Customer request)
        {
            Response _response = new() { ResponseState = ResponseState.Failed };

            try
            {
                using IDocumentSession _session = DocumentStoreHolder.Store.OpenSession();
                _session.Advanced.WaitForIndexesAfterSaveChanges();
                string _exists = Exists(_session, request.ContactID, request.BusinessID);
            
                if (_exists == string.Empty)
                {
                    string _customerID = Guid.NewGuid().ToString();
                    string _code = Helpers.Formulae.CustomerCode(_session, request.OrganisationID, request.BusinessID);
                   
                    if (_code != string.Empty)
                    {
                        Database.Entities.Customer _customer = new()
                        {
                            ContactID = request.ContactID,
                            CustomerCode = _code,
                            BusinessID = request.BusinessID,
                            ContractAccounts = new List<Database.Entities.ContractAccount>()
                        };

                        _session.Store(_customer, _customerID);
                        _session.SaveChanges();

                        _response.ResponseState = ResponseState.Ok;
                        _response.ID = _customerID;
                        Log.Message(Severities.INFO, "0102", "Customer create", string.Empty, MethodBase.GetCurrentMethod().Name, $"CustomerID = {_customerID}");
                    }
                    else
                    {
                        Log.Message(Severities.ERROR, "0102", "Customer create", string.Empty, MethodBase.GetCurrentMethod().Name, "Missing mandatory data");                                         
                    }
                }
                else
                {
                    Log.Message(Severities.WARN, "0102", "Customer create", string.Empty, MethodBase.GetCurrentMethod().Name, "Customer already exists");
                    _response.ResponseState = ResponseState.Warning;
                    _response.ID = _exists;
                }

                return _response;
            }
            catch (Exception ex)
            {
                Log.Message(Severities.FATAL, "0004", "Fatal exception", string.Empty, MethodBase.GetCurrentMethod().Name, ex.Message);
                _response.ResponseState = ResponseState.Failed;
                return _response;
            }
        }


        internal static Database.Entities.Customer Search(IDocumentSession session, string customerID)
        {
            return session.Load<Database.Entities.Customer>(customerID);       
        }

        private static string Exists(IDocumentSession session, string contactID, string businessID)
        {
            IRavenQueryable<Customer> _query = session.Query<Customer>("Customers/Index")
                .Where(x => x.ContactID == contactID && x.BusinessID == businessID);

            foreach (var item in _query)           
                return session.Advanced.GetDocumentId(item).ToString();
         
            return string.Empty;
        }

        internal static List<Customer> GetCustomers(IDocumentSession session, CustomerFilter request)
        {
            try
            {
                IRavenQueryable<Database.Entities.Customer> _query = session.Query<Database.Entities.Customer>("Customers/Index")    
                    .Where(x => x.BusinessID == request.BusinessID);

                List<Customer> _customers = new();

                foreach (var item in _query)
                {
                    Customer _customer = new()                    
                    {
                        CustomerID = session.Advanced.GetDocumentId(item)                
                    };
                    CustomerDTO(item, _customer, request);
                    _customers.Add(_customer);
                }
                return _customers;
            }
            catch (Exception ex)
            {
                Log.Message(Severities.FATAL, "0004", "Fatal exception", string.Empty, MethodBase.GetCurrentMethod().Name, ex.Message);
                return new List<Customer>();
            }
        }

        internal static Customer GetCustomer(CustomerFilter request)
        {
            try
            {
                Customer _customer = new();
                using IDocumentSession _session = DocumentStoreHolder.Store.OpenSession();
                _session.Advanced.WaitForIndexesAfterSaveChanges();
                Database.Entities.Customer _result = _session.Load<Database.Entities.Customer>(request.CustomerID);

                if (IsEmpty(request.CustomerID))
                    _customer.CustomerID = request.CustomerID;

                if(_result != null)                
                    CustomerDTO(_result, _customer, request);

                return _customer;
            }
            catch (Exception ex)
            {
                Log.Message(Severities.FATAL, "0004", "Fatal exception", string.Empty, MethodBase.GetCurrentMethod().Name, ex.Message);           
                return new Customer() { Successful = false};
            }
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

        private static void CustomerDTO(Database.Entities.Customer item, Customer customer, CustomerFilter request)
        {
            if (IsEmpty(item.CustomerCode))
                customer.CustomerCode = item.CustomerCode;
            if (IsEmpty(item.ContactID))
                customer.ContactID = item.ContactID;
            if (IsEmpty(item.BusinessID))
                customer.BusinessID = item.BusinessID;

            if(request.Additionals != null)            
                if(request.Additionals.HasContracts)               
                    if (request.Additionals.Contracts)                    
                        ContractAccountController.ContractAccountDTO(item, customer);

            customer.Successful = true;

        }

    }
}
