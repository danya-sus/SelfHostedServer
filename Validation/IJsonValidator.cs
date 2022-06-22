using Newtonsoft.Json.Linq;

namespace SelfHostedServer.Validation
{
    public interface IJsonValidator
    {
        bool IsValid(string json, string schema);
    }
}
