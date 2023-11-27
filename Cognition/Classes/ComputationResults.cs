using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
public class ComputationResults
{
    public ComputationResults(string verticle_name, int verticle_id, double x = 0, double y = 0,  double x1 = 0, double y1 = 0)
    {
        ID = verticle_id; Name = verticle_name; X = x; Y = y; X1 = x1; Y1 = y1;
    }

    public int _id;
    public int ID { get { return _id; } set { _id = value; } }

    public string _name;
    public string Name { get { return _name; } set { _name = value; } }

    public double _x;
    public double X { get { return _x; } set { _x = value; } }

    public double _y;
    public double Y { get { return _y; } set { _y = value; } }

    public double _x1;
    public double X1 { get { return _x1; } set { _x1 = value; } }

    public double _y1;
    public double Y1 { get { return _y1; } set { _y1 = value; } }
}

