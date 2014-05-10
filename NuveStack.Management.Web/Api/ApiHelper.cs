using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;

using NuveStack.Core.Extensions;

namespace NuveStack.Management.Web.Api
{
    public class ApiHelper
    {
        public static string GetReference<T>(T item) where T : NuveStack.Core.Model.IApiReferenceObject
        {
            var basePath = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/api";
            var path = GetPathForType<T>();
            return string.Format("{0}/{1}/{2}", basePath, path, item.Id);
        }

        private static string GetPathForType<T>()
        {
            var path = "user";
            if (typeof(T) == typeof(NuveStack.Core.Model.StackUser))
            {
                path = "user";
            }
            return path;
        }

        public static HttpResponseMessage CreateResponse(HttpRequestMessage request,  ApiResult result)
        {
            //map our status to a public http status
            System.Net.HttpStatusCode httpStatusCode;
            switch (result.StatusCode)
            {
                case ApiResultStatus.Created:
                    httpStatusCode = System.Net.HttpStatusCode.Created;
                    break;
                case ApiResultStatus.InternalError:
                    httpStatusCode = System.Net.HttpStatusCode.InternalServerError;
                    break;
                case ApiResultStatus.BadRequest:
                    httpStatusCode = System.Net.HttpStatusCode.BadRequest;
                    break;
                default:
                    httpStatusCode = System.Net.HttpStatusCode.OK;
                    break;
            }

            var clientResult = new ApiPublicResult();

            if(result.Reference.IsNotEmpty())
            {
                clientResult.Reference = result.Reference;
            }
            if(result.Message.IsNotEmpty())
            {
                clientResult.Message = result.Message;
            }
            if (result.StatusCode == ApiResultStatus.Ok)
            {
                return request.CreateResponse(httpStatusCode, result.Data);
            }
            else
            {
                return request.CreateResponse(httpStatusCode, clientResult);
            }
        }

        internal static object GetPropertiesFromRequest(dynamic value)
        {
            throw new NotImplementedException();
        }
    }
}