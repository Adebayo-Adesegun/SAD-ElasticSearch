
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAD_ElasticSearch.Infrastructure
{
    /// <summary>
    /// This class Deserializes a JSON file into a specified generic class T
    /// </summary>
    public class JSONFileReader<T> where T : class
    {
        private readonly string _filePath;
        public JSONFileReader(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Deserializes T : type of class and returns a Lis<T>
        /// </summary>
        /// <returns>T, type of class</returns>
        public List<T> GetData()
        {

            var jsonString = string.Empty;

            using (var str = new StreamReader(_filePath))
            {
                jsonString = str.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<List<T>>(jsonString);
        }
    }
}
