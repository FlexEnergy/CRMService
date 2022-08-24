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

namespace SmartSphere.CRM.Organisations
{
    internal class OrganisationBase : IOrganisation
    {
        CommonResponse IOrganisation.Create(Organisation request)
        {
            CommonResponse _response = new() ;
            
            try
            {
                //using IDocumentSession _session = DocumentStoreHolder.Store.OpenSession();
                //_session.Advanced.WaitForIndexesAfterSaveChanges();

                //if (!Exists(_session, request.ContactID))
                //{


                //    string _orgID = Guid.NewGuid().ToString();                 
                //     Entities.Organisation _org = new()
                //    {
                //        ContactID = request.ContactID,
                //        Description = request.Description
                //    };

                //    _session.Store(_org, _orgID);
                //    _session.SaveChanges();

                //    _response.ResponseState = ResponseState.Ok;
                //    _response.ID = _orgID;

                //    Log.Message(Severities.INFO, "O000", "Organisation create", GetType().Name, MethodBase.GetCurrentMethod().Name, $"OrganisationID = {_orgID}");
                //}
                //else
                //{
                //    Log.Message(Severities.ERROR, "0010", "Organisation create", GetType().Name, MethodBase.GetCurrentMethod().Name, "Organisation is existing");
                //    _response.ResponseState = ResponseState.Failed;              
                //    _response.ResponseMessage = "Organisation is existing";       
                //}

                return _response;
            }
            catch (Exception ex)
            {
                Log.Message(Severities.FATAL, "0004", "Fatal exception", GetType().Name, MethodBase.GetCurrentMethod().Name, text2:ex.Message);
                _response.ResponseState = ResponseState.Failed;            
                return _response;
            }
        }

        private bool Exists(IDocumentSession session, string contactID)
        {
            IRavenQueryable<Entities.Organisation> _query = session.Query<Entities.Organisation>("Organisations/Index")
                .Where(x => x.ContactID == contactID);

            foreach (var item in _query)
                return true;

            return false;
        }

        CommonResponse IOrganisation.Remove(Organisation request)
        {
            throw new NotImplementedException();
        }

        OrganisationList IOrganisation.Search(OrganisationFilter request)
        {
            OrganisationList _response = new();
            try
            {
                RepeatedField<Organisation> _orgs = Database.GetOrganisations();

                if(_orgs != null)
                {
                    _response.Organisations.AddRange(_orgs);
                    _response.ResponseState = ResponseState.Ok;

                    return _response;
                }

                _response.ResponseState = ResponseState.Failed;
                return _response;
            }
            catch (Exception ex)
            {
                Log.Message(Severities.FATAL, "O000", "Fatal exception", GetType().Name, MethodBase.GetCurrentMethod().Name,text2:ex.Message);
                _response.ResponseState = ResponseState.Failed;             
                return _response;
            }
        }
   

        CommonResponse IOrganisation.Update(Organisation request)
        {
            throw new NotImplementedException();
        }
    }
}
