using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestAppCore.Models;
using TestAppCore.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;

namespace TestAppCore.Controllers
{
    public class ImageController : Controller
    {
        private const string  NOIMAGE_FILENAME_PART = @"~/img/noimage.svg";

        private readonly ILogger<ImageController> _logger;

        public ImageController(ILogger<ImageController> logger)
        {
            _logger = logger;
        }

        public FileResult Get([FromQuery]string Id)
        {
            //string noImagePath = Path.Combine(Directory.GetCurrentDirectory(), NOIMAGE_FILENAME_PART);


            int imgId = 0;
            if (!int.TryParse(Id, out imgId)) imgId = 0;

            if (imgId < 1) return File(NOIMAGE_FILENAME_PART, "image/svg+xml");

            CarImage oneCarImage = DataSource.GetCarImage(imgId);

            if (oneCarImage == null)
            {
                return File(NOIMAGE_FILENAME_PART, "image/svg+xml");
            }

            return File(oneCarImage.FileData, oneCarImage.FileType);

            /*
            if (fs.Extension.ToLower() == "png") return File(fName, "image/png");
            if ((fs.Extension.ToLower() == "jpeg") || (fs.Extension.ToLower() == "jpg")) return File(fName, "image/jpeg");

            return File(NOIMAGE_FILENAME_PART, "image/svg+xml");*/
        }


        [HttpPost]
        public async Task<string> Upload(IList<IFormFile> upload)
        {
            MemoryStream bmpStream = null;

            if ((upload == null) || (upload.Count < 1))
            {
                return RequestResult.GetErrorAnswer("Не переданы файлы");
            }

            IFormFile imgFile = upload[0];

            Console.WriteLine(imgFile.ContentType);

            try
            {
                bmpStream = new MemoryStream();
                await imgFile.CopyToAsync(bmpStream);
                bmpStream.Flush();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.StackTrace + Environment.NewLine + ee.Message);
                return RequestResult.GetErrorAnswer("Ошибка обработки файла");
            }


            try
            {
                if (bmpStream == null) return "Error";
                Bitmap bmp = new Bitmap(bmpStream);

                if (bmp == null) return RequestResult.GetErrorAnswer("Ошибка обработки изображения");
                if ((bmp.Width < 1) || (bmp.Height < 1)) return RequestResult.GetErrorAnswer("Некорректное изображение");
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.StackTrace + Environment.NewLine + ee.Message);
                return RequestResult.GetErrorAnswer("Ошибка обработки изображения");
            }

            CarImage oneImage = new CarImage(bmpStream, imgFile.ContentType);

            DataSource.AddCarImage(oneImage);

            RequestResult answer = new RequestResult()
            {
                ImageId = oneImage.Id
            };

            return JsonConvert.SerializeObject(answer);
            
        }



    }
}
