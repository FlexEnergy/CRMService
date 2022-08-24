using System;
using System.Threading.Tasks;
using Grpc.Core;
using SmartSphere.Logs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Reflection;
using SmartSphere.Protos;
using SmartSphere.CRM.Protos;

namespace SmartSphere.CRM.Services
{
    internal class RpcServer : BackgroundService
    {

        #region Members

        private Server GrpcServer;

        #endregion

        public RpcServer()
        {

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                //Log.Message(Severities.INFO, "0001", "Task service start", GetType().Name, MethodBase.GetCurrentMethod().Name);

                GrpcServer = new Server
                {
                    Services = { OrganisationService.BindService(new Organisations.ServerService()),
                    BusinessService.BindService(new Business.ServerService()),
                    CustomerService.BindService(new Customers.ServerService())},
                    Ports = { new ServerPort(Settings.RpcService.Host, Settings.RpcService.Port, ServerCredentials.Insecure) }
                };

                //RemoteConnectors.Directories_Contacts.Contacts.Connect();

            }
            catch (TaskCanceledException)
            {
                Log.Message(Severities.WARN, "0003", "Task service canceled", GetType().Name, MethodBase.GetCurrentMethod().Name);
            }
            catch (RpcException ex)
            {
                Log.Message(Severities.WARN, "0005", "Rpc status changed", GetType().Name, MethodBase.GetCurrentMethod().Name, $"Status:{ex.StatusCode}");
            }
            catch (Exception ex)
            {
                Log.Message(Severities.FATAL, "0004", "Fatal exception", GetType().Name, MethodBase.GetCurrentMethod().Name, text2:ex.Message);
            }

            return base.StartAsync(cancellationToken);

        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await GrpcServer.ShutdownAsync();
            }
            catch (TaskCanceledException)
            {
                Log.Message(Severities.WARN, "0003", "Task service canceled", GetType().Name, MethodBase.GetCurrentMethod().Name);
            }
            catch (RpcException ex)
            {
                Log.Message(Severities.WARN, "0005", "Rpc status changed", GetType().Name, MethodBase.GetCurrentMethod().Name, $"Status:{ex.StatusCode}");
            }
            catch (Exception ex)
            {
                Log.Message(Severities.FATAL, "0004", "Fatal exception", GetType().Name, MethodBase.GetCurrentMethod().Name, text2:ex.Message);
                await base.StopAsync(cancellationToken);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                int _countStart = 2;
                while (!stoppingToken.IsCancellationRequested)
                {
                    //_logger.LogInformation("Service running at: {time}", DateTimeOffset.Now);
                    if (_countStart == 0)
                    {

                        if (!Settings.IndexApplied) //Applica indici database
                        {
                            Database.Index.Manager.Run();
                            Settings.IndexApplied = true;
                        }

                        Log.Message(Severities.INFO, "0001", "Task service start", GetType().Name, MethodBase.GetCurrentMethod().Name);
                        await Task.Run(() => GrpcServer.Start(), stoppingToken);
                    }

                    if (_countStart > -1) _countStart -= 1;

                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (TaskCanceledException)
            {
                Log.Message(Severities.WARN, "0003", "Task service canceled", GetType().Name, MethodBase.GetCurrentMethod().Name);
            }
            catch (RpcException ex)
            {
                Log.Message(Severities.WARN, "0005", "Rpc status changed", GetType().Name, MethodBase.GetCurrentMethod().Name,$"Status:{ex.StatusCode}");
            }
            catch (Exception ex)
            {
                Log.Message(Severities.FATAL, "0004", "Fatal exception", GetType().Name, MethodBase.GetCurrentMethod().Name, text2:ex.Message);
            }

        }

    }
}

