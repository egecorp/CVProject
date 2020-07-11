using Newtonsoft.Json;

namespace CVProject.Models
{
    [JsonObject]
    public class BodyType
    {
        [JsonProperty]
        public int Id { set; get; }
        
        [JsonProperty]
        public string Name { set; get; }

    }
}
