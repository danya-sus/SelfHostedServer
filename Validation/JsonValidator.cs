using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;

namespace SelfHostedServer.Validation
{
    public class JsonValidator : IJsonValidator
    {
        public bool IsValid(string json, string address)
        {
            var stringSchema = "";
            using (StreamReader r = new StreamReader($"C:/Users/danya/source/repos/SelfHostedServer/Schemas/{address}.json"))
            {
                stringSchema = r.ReadToEnd();
            }
            var schema = JSchema.Parse(stringSchema);

            return JObject.Parse(json).IsValid(schema);
        }
    }
}
