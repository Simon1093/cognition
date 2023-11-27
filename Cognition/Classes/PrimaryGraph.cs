using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
public class PrimaryGraph
{
    public PrimaryGraph(string verticle_name, int verticle_id, double weight, int type = 0)
    {
        ID = verticle_id; Name = verticle_name; Weight = weight; Type = type;
    }

    public int _id;
    public int ID { get { return _id; } set { _id = value; } }

    public string _name;
    public string Name { get { return _name; } set { _name = value; } }

    public double _weight;
    public double Weight { get { return _weight; } set { _weight = value; } }

    public int _type;
    public int Type { get { return _type; } set { _type = value; } }

    public class Verticle
    {
        [JsonProperty("verticle_id")]
        public int verticle_id { get; set; }

        [JsonProperty("verticle")]
        public string verticle { get; set; }

        [JsonProperty("connections")]
        public List<Connections> connections { get; set; }

        [JsonProperty("type")]
        public int type { get; set; }
    }
    public class Connections
    {
        [JsonProperty("connectedTo")]
        public int connectedTo { get; set; }

        [JsonProperty("strength")]
        public double strength { get; set; }
    }
}

