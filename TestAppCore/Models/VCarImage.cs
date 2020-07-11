using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CVProject.Data;
using System.Text.RegularExpressions;
using System.IO;

namespace CVProject.Models
{
    public class VCarImage: CarImage
    {

        public VCarImage(MemoryStream fStream, string fileType)
            :base()
        {
            FileType = fileType;
            FileData = fStream.ToArray();
        }

    }



}
