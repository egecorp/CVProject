using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System;

namespace CVProject.Models
{
    [JsonObject]
    public class DBConfig
    {
        public string ConnectionString { set; get; }

        public List<BodyType> DefaultBodyType { set; get; }

        public List<Brand> DefaultBrand { set; get; }


        private const string DEFALUT_JSON_FILENAME = "dbsettings.json";

        private static object mLock = new object();

        private static DBConfig myConfig = null;

        public static DBConfig Get(string jsonFileName = null)
        {
            lock (mLock)
            {
                if (myConfig != null) return myConfig;

                string settingsFileName = (jsonFileName != null) ? jsonFileName : null;

                if (settingsFileName == null)
                {
                    settingsFileName = Path.Combine(Directory.GetCurrentDirectory(), DEFALUT_JSON_FILENAME);
                }


                string fBody = File.ReadAllText(settingsFileName, System.Text.Encoding.UTF8);
                DBConfig newConfig = JsonConvert.DeserializeObject<DBConfig>(fBody);

                return myConfig = newConfig;
            }
        }
    }
}