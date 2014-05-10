using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using NuveStack.Core;
using NuveStack.Management.Core;
using NuveStack.Core.Model;
using NuveStack.Management.Web.Api;

namespace NuveStack.Management.Web.Controllers.api
{
    public class UserController : ApiController
    {
        private StackUserService _userService = new StackUserService();
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            var result = new ApiResult();
            try
            {
                string tenantId = "123"; //TODO: how do we establish the tenant id? by login? require two args? dont' care?
                result.Data = _userService.GetUsers(tenantId);
            }
            catch (NuveStackApplicationException nEx)
            {
                result.Message = nEx.FriendlyMessage;
                result.StatusCode = ApiResultStatus.InternalError;
                this.LogException(nEx);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ApiResultStatus.InternalError;
                this.LogException(ex);
            }
            return ApiHelper.CreateResponse(Request, result);
        }

        // GET api/<controller>/
        public HttpResponseMessage Get(string id)
        {
            var result = new ApiResult();
            try
            {
                string tenantId = "123"; //TODO: how do we establish the tenant id? by login? require two args? dont' care?
                var data = _userService.GetUser(id, tenantId);
                result.Data = data;
            }
            catch (NuveStackApplicationException nEx)
            {
                result.Message = nEx.FriendlyMessage;
                result.StatusCode = ApiResultStatus.InternalError;
                this.LogException(nEx);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ApiResultStatus.InternalError;
                this.LogException(ex);
            }
            return ApiHelper.CreateResponse(Request, result);
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]StackUser newUser)
        {
            var result = new ApiResult();
            try
            {
                //provision the user in AD
                _userService.AddUser(newUser);
                result.Reference = ApiHelper.GetReference(newUser);
                result.StatusCode = ApiResultStatus.Created;
            }
            catch (NuveStackApplicationException nEx)
            {
                result.Message = nEx.FriendlyMessage;
                result.StatusCode = ApiResultStatus.InternalError;
                this.LogException(nEx);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ApiResultStatus.InternalError;
                this.LogException(ex);
            }
            return ApiHelper.CreateResponse(Request, result);
        }

        public HttpResponseMessage Put(string id, [FromBody]Dictionary<string, object> values)
        {
            var result = new ApiResult();
            var resultList = new Dictionary<string, string>();
            result.Data = resultList;
            try
            {
                var current = _userService.GetUser(id, "");

                foreach (var pair in values)
                {
                    var property = pair.Key;
                    if(property == "NewPassword")
                    {
                        //_userService.ChangePassword(id, values);
                        resultList.Add("Password", "*****");
                    }
                    else if(property == "DisplayName" && string.Compare((string)pair.Value, current.DisplayName, true) != 0)
                    {
                        //_userService.ChangeProperty()
                        resultList.Add("DisplayName", (string)pair.Value);
                    }
                    else if (property == "PackageType"  && string.Compare((string)pair.Value, current.PackageType, true) != 0)
                    {
                        _userService.ChangePackage(id, (string)pair.Value, current.PackageType);
                        resultList.Add("PackageType", (string)pair.Value);
                    }
                }
            }
            catch (NuveStackApplicationException nEx)
            {
                result.Message = nEx.FriendlyMessage;
                result.StatusCode = ApiResultStatus.InternalError;
                this.LogException(nEx);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = ApiResultStatus.InternalError;
                this.LogException(ex);
            }
            return ApiHelper.CreateResponse(Request, result);
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}