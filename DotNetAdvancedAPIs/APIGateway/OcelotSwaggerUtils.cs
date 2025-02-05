using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace OcelotAPIGateway
{
    public class OcelotSwaggerUtils
    {
        public static string AlterUpstreamSwaggerJson(HttpContext context, string swaggerJson)  
        {
            var swagger = JObject.Parse(swaggerJson);
            // ... alter upstream json
            return swagger.ToString(Formatting.Indented);
        }
    }
}
