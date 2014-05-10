using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using NuveStack.Core;
using NuveStack.Core.Model;
using NuveStack.Management.Core;
using NuveStack.Management.Web.Api;


namespace NuveStack.Management.Web.Controllers.api
{
    public class DiagnosticsController : ApiController
    {
        private DiagnosticsService _diagnosticsService = new DiagnosticsService();
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            var result = new ApiResult();
            try
            {
                 result.Data = _diagnosticsService.GetPowerShellEnvironment();
            }
            catch (NuveStackApplicationException nEx)
            {

            }
            catch (Exception ex)
            {

            }
            return ApiHelper.CreateResponse(Request, result);
        }
    }
}