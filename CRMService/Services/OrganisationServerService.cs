using System;
using System.Threading.Tasks;
using Grpc.Core;
using SmartSphere.Protos;
using SmartSphere.CRM.Protos;
using SmartSphere.CRM.Organisations;

namespace SmartSphere.CRM.Services
{
    internal class OrganisationServerService : OrganisationService.OrganisationServiceBase
    {
        //public override async Task<CommonResponse> Create(Organisation request, ServerCallContext context)
        //{
        //    try
        //    {
        //        IOrganisation _org = new Organisation();
        //        return await Task.FromResult(_org.Create(request));
        //    }
        //    catch (Exception)
        //    {
        //        return await Task.FromResult(new CommonResponse());
        //    }
        //}

        public override async Task<OrganisationList> Search(OrganisationFilter request, ServerCallContext context)
        {
            try
            {
                IOrganisation _org = new OrganisationBase();
                return await Task.FromResult(_org.Search(request));
            }
            catch (Exception)
            {
                return await Task.FromResult(new OrganisationList());
            }
        }

    }
}
