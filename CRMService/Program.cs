using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using SmartSphere.Database.Raven.Entities;
using SmartSphere.Protos;
using SmartSphere.Directory.Protos;
using SmartSphere.Database.Raven;
using SmartSphere.CRM.Services;
using SmartSphere.CRM.Database.Services;
using SmartSphere.Logs;

namespace SmartSphere.CRM
{
    public class Program
    {

        public static void Main(string[] args)
        {
            if (AppSettings())
            {
                Log.Start(Settings.Log.Path, Settings.Log.FileName, Settings.Log.Level, Settings.Name, Settings.ID);
                //Database.Index.Test.AddressType(); //DISABILITATO PER CONGUAGLI TEMPORANEI

                //Database.Index.Test.Directory_Person();
                //Database.Index.Test.Directory_Corporate();
                //Database.Index.Manager.Run();
                //Database.Index.Test.RunDbTest();
                //Database.Models.Manager.Run();

                //Controllers.CustomerController.Create(new Protos.Customer()
                //{
                //    OrganisationID = "c3f6a102-5dfe-4d58-9151-dbb019e41cbc",
                //    BusinessID = "7a47fbc4-fbf3-4dab-8903-a6dc8adfaecb",
                //    ContactID = "0da89c2f-2a50-45e2-a027-c04aaa3e84ec"

                //});


                Settings.IndexApplied = false;
                CreateHostBuilder(args).Build().Run();
            }
        }

        private static bool AppSettings()
        {
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json").Build();

                var section = config.GetSection(nameof(FlexEnergy.Model.Settings));
                FlexEnergy.Model.Settings _appSettings = section.Get<FlexEnergy.Model.Settings>();

                //Self Settings
                Settings.Token = _appSettings.Token;
                Settings.Group = _appSettings.Group;

                //Broker Settings
                BrokerStoreHolder.Host = FlexEnergy.Helpers.Converter.GetIP(_appSettings.Broker);
                BrokerStoreHolder.Port = FlexEnergy.Helpers.Converter.GetPort(_appSettings.Broker);
                BrokerStoreHolder.DatabaseName = _appSettings.BrokerDB;

                //Self Rpc Service    
                Settings.ID = SmartSphere.Database.Raven.Services.Token.GetID(Settings.Token);
                Settings.Name = AppDomain.CurrentDomain.FriendlyName;
                Settings.RpcService = SmartSphere.Database.Raven.Services.Microservices.GetRpc(Settings.ID, Settings.Group);

                //Self Raven connection
                RavenDB _ravenDB = SmartSphere.Database.Raven.Services.Microservices.GetRavenDB(Settings.ID, Settings.Group);
                DocumentStoreHolder.Host = _ravenDB.Host;
                DocumentStoreHolder.Port = _ravenDB.Port;
                DocumentStoreHolder.DatabaseName = _ravenDB.Name;

                //Logs
                Settings.Log = SmartSphere.Database.Raven.Services.Logs.Get("Log");
                Settings.RemoteLog = SmartSphere.Database.Raven.Services.Logs.Get("RemoteLog");

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                //services.AddHostedService<MessageService>();
                   services.AddHostedService<RpcServer>();
                }).UseWindowsService();

    }
}










//using SmartSphere.CRM;

//IHost host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices(services =>
//    {
//        services.AddHostedService<Worker>();
//    })
//    .Build();

//await host.RunAsync();
