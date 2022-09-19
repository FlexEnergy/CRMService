using System;
using System.Threading.Tasks;
using Grpc.Core;
using SmartSphere.Protos;
using SmartSphere.CRM.Protos;
using SmartSphere.CRM.Controllers;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Raven.Client.Documents;
using SmartSphere.Database.Raven;
using SmartSphere.Directory.Protos;

namespace SmartSphere.CRM.Services
{
    internal class CustomerServerService : CustomerService.CustomerServiceBase
    {
        public override async Task<Response> Create(Customer request, ServerCallContext context)
        {
            return await Task.FromResult(CustomerController.Create(request));
        }

        public override async Task<Response> AddContractAccount(ContractAccount request, ServerCallContext context)
        {
            return await Task.FromResult(ContractAccountController.Add(request));
        }

        public override async Task<Response> AddContract(Contract request, ServerCallContext context)
        {
            return await Task.FromResult(ContractController.Add(request));
        }

        public override async Task<Response> AddContractService(ContractService request, ServerCallContext context)
        {
            return await Task.FromResult(ContractController.AddService(request));
        }

        public override async Task GetCustomers(CustomerFilter request, IServerStreamWriter<Customer> responseStream, ServerCallContext context)
        {
            using IDocumentSession _session = DocumentStoreHolder.Store.OpenSession();
            _session.Advanced.WaitForIndexesAfterSaveChanges();

            List<Customer> _customers = new();
            _customers.AddRange(CustomerController.GetCustomers(_session, request));
            var _query = from c in _customers
                         orderby c.CustomerID
                         select c;

            foreach (var item in _query)
            {
                await responseStream.WriteAsync(item);
            }
        }

        public override async Task<Customer> GetCustomer(CustomerFilter request, ServerCallContext context)
        {
            return await Task.FromResult(CustomerController.GetCustomer(request));
        }

    }
}
