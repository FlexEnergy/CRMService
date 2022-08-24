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
using SmartSphere.CRM.Business.Entities;

namespace SmartSphere.CRM.Business
{
    internal class BusinessBase : IBusiness
    {
        CommonResponse IBusiness.Create(BusinessMessage request)
        {
            CommonResponse _response = new();

            try
            {
                using IDocumentSession _session = DocumentStoreHolder.Store.OpenSession();
                _session.Advanced.WaitForIndexesAfterSaveChanges();

                if (!Exists(_session, request.ContactID, request.OrganisationID))
                {
                    string _businessID = Guid.NewGuid().ToString();
                    Entities.Business _business = new()
                    {
                        ContactID = request.ContactID,
                        OrganisationID = request.OrganisationID,
                        Description = request.Description,
                        CustomerCode = new Database.Models.Code
                        {
                            Formulae = request.CustomerCode.Formulae,
                            Counter = request.CustomerCode.Counter,
                            CounterName = request.CustomerCode.CounterName,
                            TypeOfCode = request.CustomerCode.TypeOfCode
                        }
                    };

                    _session.Store(_business, _businessID);
                    _session.SaveChanges();

                    //_response.Responses.Add(new Response() { ResponseState = ResponseState.Ok, ResponseCode = "B000" });
                    //_response.ID = _businessID;
                    Log.Message(Severities.INFO, "B000", "Business create", GetType().Name, MethodBase.GetCurrentMethod().Name, $"BusinessID = {_businessID}");
                }
                else
                {
                    Log.Message(Severities.ERROR, "B000", "Business create", GetType().Name, MethodBase.GetCurrentMethod().Name, "Business is existing");
                    //_response.Responses.Add(new Response() { ResponseState = ResponseState.Failed, ResponseCode = "B000", Message = "Business is existing" });
                }

                return _response;
            }
            catch (Exception ex)
            {
                Log.Message(Severities.FATAL, "B000", "Fatal exception", GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
                _response.ResponseState = ResponseState.Failed;
                return _response;
            }
        }

        private bool Exists(IDocumentSession session, string contactID, string organisationID)
        {
            IRavenQueryable<Entities.Business> _query = session.Query<Entities.Business>("Organisations/Index")
                .Where(x => x.ContactID == contactID && x.OrganisationID == organisationID);

            foreach (var item in _query)
                return true;

            return false;
        }

        CommonResponse IBusiness.Remove(BusinessMessage request)
        {
            throw new NotImplementedException();
        }

        BusinessListMessage IBusiness.Search(BusinessFilterMessage request)
        {
            throw new NotImplementedException();
        }

        CommonResponse IBusiness.Update(BusinessMessage request)
        {
            throw new NotImplementedException();
        }
    }
}
