using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace input_generator.Classes
{
    class MatrixJSONParser
    {
        //public static dynamic ParseMatrix(string MatrixJSON)
        //{
        //    //dynamic elements = JsonConvert.DeserializeObject(MatrixJSON);
        //    //string matrix = JsonConvert.SerializeObject(elements[0]);
        //    //var dynObj = JsonConvert.DeserializeObject<MatrixJson>(matrix);
        //    //return dynObj;
        //}

    }

    public class MatrixJson
    {
        [JsonProperty("name")]
        public string name;
        [JsonProperty("n")]
        public int n;
        [JsonProperty("connections")]
        public List<ConnectionsJson> connections;
    }

    public class ConnectionsJson
    {
        public int from;
        public int to;
        public string value;
    }
}
