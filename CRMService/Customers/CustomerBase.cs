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
using SmartSphere.CRM.Customers.Entities;

namespace SmartSphere.CRM.Customers
{
    internal class CustomerBase : ICustomer
    {
        CommonResponse ICustomer.Create(CustomerMessage request)
        {
            CommonResponse _response = new() ;
           
            try
            {             
                using IDocumentSession _session = DocumentStoreHolder.Store.OpenSession();
                _session.Advanced.WaitForIndexesAfterSaveChanges();

                if(!Exists(_session, request.ContactID, request.BusinessID))
                {
                    string _customerID = Guid.NewGuid().ToString();
                    string _code = Helpers.Formulae.CustomerCode(_session, request.BusinessID);
                    if (_code != string.Empty)
                    {
                        Customer _customer = new()
                        {
                            ContactID = request.ContactID,
                            Code = _code,
                            BusinessID = request.BusinessID,
                            Description = request.Description,
                            Created = DateTimeOffset.UtcNow,
                            OwnerID = request.OwnerID
                        };

                        _session.Store(_customer, _customerID);
                        _session.SaveChanges();

                        //_response.Responses.Add(new Response() { ResponseState = ResponseState.Ok, ResponseCode = "C000" });
                        _response.ID = _customerID;
                        Log.Message(Severities.INFO, "C000", "Customer create", GetType().Name, MethodBase.GetCurrentMethod().Name, $"CustomerID = {_customerID}");
                    }
                    else
                    {
                        Log.Message(Severities.ERROR, "C000", "Customer create", GetType().Name, MethodBase.GetCurrentMethod().Name, "Customer code is empty");
                        //_response.Responses.Add(new Response() { ResponseState = ResponseState.Failed, ResponseCode = "C000", Message = "Customer code is empty" });
                    }
                }
                else
                {
                    Log.Message(Severities.ERROR, "C000", "Customer create", GetType().Name, MethodBase.GetCurrentMethod().Name, "Customer is existing");
                    //_response.Responses.Add(new Response() { ResponseState = ResponseState.Failed, ResponseCode = "C000", Message = "Customer is existing" });
                }

                return _response;
            }
            catch (Exception ex)
            {
                Log.Message(Severities.FATAL, "C000", "Fatal exception", GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
                _response.ResponseState = ResponseState.Failed;
                return _response;
            }
        }

        private bool Exists(IDocumentSession session,string contactID, string businessID)
        {
            IRavenQueryable<Customer> _query = session.Query<Customer>("Customers/Index")
                .Where(x => x.ContactID == contactID && x.BusinessID == businessID);

            foreach (var item in _query)
                return true;

            return false;
        }

        CommonResponse ICustomer.Remove(CustomerMessage request)
        {
            throw new NotImplementedException();
        }

        CustomerListMessage ICustomer.Search(CustomerFilterMessage request)
        {
            throw new NotImplementedException();
        }

        CommonResponse ICustomer.Update(CustomerMessage request)
        {
            throw new NotImplementedException();
        }
    }
}
