using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
public class DataGrid
{
    public DataGrid(string verticle_name, int verticle_id, string weight, bool is_used, int type = 0)
    {
        ID = verticle_id; Name = verticle_name; Weight = weight; Type = type;
    }

    public int _id;
    public int ID { get { return _id; } set { _id = value; } }

    public string _name;
    public string Name { get { return _name; } set { _name = value; } }

    public string _weight;
    public string Weight { get { return _weight; } set { _weight = value; } }

    public int _type;
    public int Type { get { return _type; } set { _type = value; } }
}

