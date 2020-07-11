using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CVProject.Models
{
    public class CarImage
    {
        [JsonProperty]
        public int Id { set; get; }

        [Required]
        public string FileType { set; get; }

        [Required]
        public byte[] FileData { set; get; }

        public CarImage()
        {

        }

    }
}
