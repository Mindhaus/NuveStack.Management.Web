using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuveStack.Management.Web.Api
{
    public class ApiResult
    {
        public string Message { get; set; }
        public string Reference { get; set; }
        public ApiResultStatus StatusCode { get; set; }
        public object Data { get; set; }
    }
}