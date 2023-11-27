using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

class MatrixParser
{
    public List<PrimaryGraph.Verticle> ParseMatrix(string data)
    {
        Console.WriteLine("Converting matrix to JSON file...");
        List<rowsList> matrixRowsList = new List<rowsList>();
        string[] matrixMainVerticles = null;
        string[] matrixRows = data.Split(new[] { "\r\n" }, StringSplitOptions.None);

        for (int i = 0; i < matrixRows.Length; i++)
        {
            string[] matrixVerticles = matrixRows[i].Split('\t'); ;
            if (i == 0)
            {
                matrixMainVerticles = matrixVerticles;
            }
            else
            {
                matrixRowsList.Add(new rowsList { verticles = matrixVerticles });
            }

        }

        List<PrimaryGraph.Verticle> AllVerticles = new List<PrimaryGraph.Verticle>();

        for (int i = 0; i < matrixRowsList.Count; i++)
        {
            string currentVerticle = matrixRowsList[i].verticles[0];

            List<PrimaryGraph.Connections> VerticleConnection = new List<PrimaryGraph.Connections>();

            for (int j = 0; j < matrixRowsList[i].verticles.Length; j++)
            {
                if (matrixRowsList[i].verticles[j] != "" && j != 0)
                    VerticleConnection.Add(new PrimaryGraph.Connections { connectedTo = j, strength = double.Parse(matrixRowsList[i].verticles[j]) });
            }

            if (currentVerticle != "")
            AllVerticles.Add(new PrimaryGraph.Verticle { verticle_id = i + 1, verticle = currentVerticle, connections = VerticleConnection });
        }

        return AllVerticles;
    }
}

public class rowsList
{
    public string[] verticles { get; set; }
}
