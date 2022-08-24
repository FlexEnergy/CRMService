using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSphere.Database.Raven;
using Raven.Client.Documents.Session;

namespace SmartSphere.CRM.Database.Services
{
    internal class Settings : SmartSphere.Database.Raven.Services.Settings
    {
        internal static Entities.Setting GetSetting()
        {
            using IDocumentSession _session = DocumentStoreHolder.Store.OpenSession();
            return _session.Load<Entities.Setting>("Setting");

        }


    }
}

