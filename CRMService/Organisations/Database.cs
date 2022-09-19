using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSphere.Protos;
using SmartSphere.CRM.Protos;
using SmartSphere.Directory.Protos;
using SmartSphere.Database.Raven.Entities;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using SmartSphere.Database.Raven;
using SmartSphere.Logs;
using System.Reflection;
using Google.Protobuf.WellKnownTypes;
using Google.Protobuf.Collections;
//using SmartSphere.CRM.Database.Entities;

namespace SmartSphere.CRM.Organisations
{
    internal static class Database
    {
        internal static RepeatedField<Organisation> GetOrganisations()
        {
            try
            {
                using IDocumentSession _session = DocumentStoreHolder.Store.OpenSession();
                _session.Advanced.WaitForIndexesAfterSaveChanges();

                IRavenQueryable<CRM.Database.Entities.Organisation> _query = _session.Query<CRM.Database.Entities.Organisation>("Organisations/Index");

                RepeatedField<Organisation> _reply = new();     
                
                //ContactFilter _filter = new(){ };

                //foreach (var obj in _query)
                //    _filter.ContactIDs.Add(obj.ContactID);

                //var _contacts = RemoteConnectors.Directories_Contacts.Contacts.Search(_filter);

                foreach (var item in _query)
                {
                    Organisation _org = new()
                    {
                        ContactID = item.ContactID,
                        Description = item.Description,
                        OrganisationID = _session.Advanced.GetDocumentId(item)
                    };

                    //var _contactQuery = from c in _contacts.AsEnumerable()
                    //                    where c.ContactID == item.ContactID
                    //                    select c;
                    //foreach (var _contact in _contactQuery)
                    //{                
                    //    string _name = string.Empty;
                    //    if (_contact.TypeOfContact == TypeOfContact.Person)
                    //        _name = $"{_contact.Person.LastName} {_contact.Person.FirstName}";
                    //    else if (_contact.TypeOfContact == TypeOfContact.Company)
                    //        _name = $"{_contact.Corporate.CorporateName}";

                    //    if (_name != string.Empty)
                    //        _org.Name = _name.Trim();
                    //}
                    _reply.Add(_org);
                }

                return _reply;
            }
            catch (Exception ex)
            {
                Log.Message(Severities.FATAL, "0004", "Fatal exception", "SmartSphere.Directory.Contacts", MethodBase.GetCurrentMethod().Name, text2:ex.Message);
                return null;
            }
        }




    }
}
