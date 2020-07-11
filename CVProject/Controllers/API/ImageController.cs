using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CVProject.Models;
using CVProject.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using CVProjectCore.Logger;

namespace CVProject.Controllers
{
    public class ImageController : Controller
    {

        private FLogger Logger = FLogger.Get("ImageController");

        private const string  NOIMAGE_FILENAME_PART = @"~/img/noimage.svg";

        private const string JPG_MIME = "image/jpeg";
        private const string PNG_MIME = "image/png";
        private const string SVG_MIME = "image/svg+xml";


        public ImageController()
        {

        }

        public FileResult Get([FromQuery]string Id)
        {
            int imgId = 0;
            if (!int.TryParse(Id, out imgId)) imgId = 0;

            if (imgId < 1) return File(NOIMAGE_FILENAME_PART, SVG_MIME);

            CarImage oneCarImage = DataSource.GetCarImage(imgId);

            if (oneCarImage == null)
            {
                return File(NOIMAGE_FILENAME_PART, SVG_MIME);
            }
            return File(oneCarImage.FileData, oneCarImage.FileType);
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

            string currentContentType = (imgFile.ContentType ?? "").ToLower();

            if ((currentContentType != JPG_MIME) && (currentContentType != PNG_MIME))
            {
                return RequestResult.GetErrorAnswer("Неверный формат файла. Поддерживаются только файлы в формате PNG и JPG");
            }

            try
            {
                bmpStream = new MemoryStream();
                await imgFile.CopyToAsync(bmpStream);
                bmpStream.Flush();
            }
            catch (Exception ee)
            {
                Logger.Error(ee.StackTrace + Environment.NewLine + ee.Message);
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
                Logger.Error(ee.StackTrace + Environment.NewLine + ee.Message);
                return RequestResult.GetErrorAnswer("Ошибка обработки изображения");
            }

            VCarImage oneImage = new VCarImage(bmpStream, currentContentType);

            DataSource.AddCarImage(oneImage);

            RequestResult answer = new RequestResult()
            {
                ImageId = oneImage.Id
            };

            return JsonConvert.SerializeObject(answer);
            
        }



    }
}
