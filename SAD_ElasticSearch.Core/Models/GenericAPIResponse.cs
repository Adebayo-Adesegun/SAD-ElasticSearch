using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAD_ElasticSearch.Core.Models
{
    public class GenericAPIResponse<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
