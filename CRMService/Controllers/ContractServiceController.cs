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
using FlexEnergy.Protos;
using Grpc.Core;
using Azure.Core;
using SmartSphere.CRM.Database.Entities;


namespace SmartSphere.CRM.Controllers
{
    internal class ContractServiceController
    {
        #region Members  

        private static ElectricalSvc.ElectricalSvcClient Client;
        private static Channel Channel;

        #endregion

        public static void Start()
        {
            string _Server = "10.45.5.22";
            int _port = 12002;

            Channel = new Channel(_Server, _port, ChannelCredentials.Insecure);
            Client = new ElectricalSvc.ElectricalSvcClient(Channel);         
        }

        public static async Task<FlexEnergy.Protos.ContractServiceElectrical> GetContractService(CSeFilter filter)
        {
            try
            {
                FlexEnergy.Protos.ContractServiceElectrical _response = await Client.GetContractServiceAsync(filter);
                return _response;
            }
            catch (RpcException ex) // when (ex.StatusCode == StatusCode.Cancelled)
            {
                Log.Message(Severities.ERROR, "0005", "Rpc status changed", string.Empty, MethodBase.GetCurrentMethod().Name, text1: $"Status:{ex.StatusCode}");
                return new FlexEnergy.Protos.ContractServiceElectrical();
            }
            catch (Exception ex)
            {
                Log.Message(Severities.FATAL, "0004", "Fatal exception", string.Empty, MethodBase.GetCurrentMethod().Name, text2: ex.Message);
                return new FlexEnergy.Protos.ContractServiceElectrical();
            }
        }

        public static async Task<RepeatedField<FlexEnergy.Protos.ContractServiceElectrical>> GetContractServices(RepeatedField<string> items, CSeFilter filter)
        {
            //var cts = new CancellationTokenSource();
            //cts.CancelAfter(TimeSpan.FromSeconds(120.0));

            filter.ContractServiceIDs.AddRange(items);

            using var call = Client.GetContractServices(filter);
            try
            {
                RepeatedField<FlexEnergy.Protos.ContractServiceElectrical> _electricalContracts = new();
                await foreach (var message in call.ResponseStream.ReadAllAsync())
                {
                    _electricalContracts.Add(message);
                }

                return _electricalContracts;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                return new RepeatedField<FlexEnergy.Protos.ContractServiceElectrical>();
            }
            catch (Exception)
            {
                return new RepeatedField<FlexEnergy.Protos.ContractServiceElectrical>();
            }


        }



    }
}
