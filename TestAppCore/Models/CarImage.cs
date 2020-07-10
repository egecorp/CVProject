using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TestAppCore.Data;
using System.Text.RegularExpressions;
using System.IO;

namespace TestAppCore.Models
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

        public CarImage(MemoryStream fStream, string fileType)
        {
            FileType = fileType;
            FileData = fStream.ToArray();

        }

    }



}
