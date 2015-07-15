using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FindMyFamilies.Web.Extensions {
    public static class ObjectExtensions {
        public static string ToJson(this object obj) {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

//            JsonSerializer js = JsonSerializer.Create(new JsonSerializerSettings());
//            var jw = new StringWriter();
//            js.Serialize(jw, obj);            
//            return jw.ToString();

            string serialized = JsonConvert.SerializeObject(obj, Formatting.None, serializerSettings);
            return serialized;
        }
    }
}

