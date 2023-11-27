using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using MoreLinq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Helpers;

public class InputMatrixParser
{
    public static FullMatrixData ParseSquareMatrix(string inputMatrix, string fileName)
    {
        List<char> separators = SquareMatrixHelper.FindNumberSeparators(inputMatrix);
        char separator = SquareMatrixHelper.SelectPopularSeparator(separators);
        string delimiter = SquareMatrixHelper.GetDelimiter(separator, inputMatrix);
        MatrixData data = SquareMatrixHelper.GetMatrixData(inputMatrix, delimiter);
        List<string> matrixHat = data.matrixHat;
        if (matrixHat != null)
        {
            data.matrixData = SquareMatrixHelper.CutMatrixHat(data.matrixData, matrixHat);
        }
        //int startLine = SquareMatrixHelper.GetStartLine(matrixData, delimiter, inputMatrix);
        int startLine = 0;
        List<ConnectionsJson> connections = SquareMatrixHelper.MatrixConnections(data.matrixData);

        List<PrimaryGraph.Verticle> graph = new List<PrimaryGraph.Verticle>();
        var numberGroups = connections.GroupBy(i => i.from);

        foreach (var grp in numberGroups)
        {
            //if (matrixHat.Find(o => o == (grp.Key + 1).ToString()) == null) continue; //skip if row is out of matrixHat range
            PrimaryGraph.Verticle v = new PrimaryGraph.Verticle();
            v.verticle_id = grp.Key + 1;
            if (matrixHat != null)
            {
                v.verticle = matrixHat[grp.Key];
            }
            else
            {
                v.verticle = (grp.Key + 1).ToString();
            }
            List<PrimaryGraph.Connections> cons = new List<PrimaryGraph.Connections>();
            foreach (ConnectionsJson connection in connections)
            {
                if (connection.value != 0 && grp.Key == connection.from)
                {
                    PrimaryGraph.Connections c = new PrimaryGraph.Connections();
                    c.connectedTo = connection.to + 1;
                    c.strength = connection.value;
                    cons.Add(c);
                }
            }
            v.connections = cons;
            graph.Add(v);

        }

        List<GenerationRules> generationRules = new List<GenerationRules>();
        generationRules.Add(new GenerationRules { delimiter = delimiter, matrixAtLine = startLine, separator = separator.ToString(), type = "square", notation = "" });
        string yamlRules = Helper.CollectGenerationRulesToYAML(generationRules);

        FullMatrixData fullMatrixData = new FullMatrixData {
            delimiter = delimiter,
            separator = separator,
            startLine = startLine,
            graph = graph,
            generationRules = generationRules,
            yamlRules = yamlRules
        };

        return fullMatrixData;
    }

    public static List<FullMatrixData> ParseLineMatrix(string inputMatrix, string fileName)
    {
        List<char> separators = LineMatrixHelper.FindNumberSeparators(inputMatrix);
        char separator = LineMatrixHelper.SelectPopularSeparator(separators);
        string delimiter = LineMatrixHelper.GetDelimiter(separator, inputMatrix);
        List<string[]> matrixData = LineMatrixHelper.GetMatrixData(inputMatrix, delimiter);
        int startLine = LineMatrixHelper.GetStartLine(matrixData, delimiter, inputMatrix);

        List<FullMatrixData> fullMatrixData = new List<FullMatrixData>();
        MatrixJson jsonMatrix = LineMatrixHelper.MatrixToJson(matrixData, fileName);

        fullMatrixData.Add(new FullMatrixData { delimiter = delimiter, separator = separator, matrixData = matrixData, matrixJson = jsonMatrix, startLine = startLine });

        return fullMatrixData;
    }

    public static List<FullMatrixData> ParseColumnMatrix(string inputMatrix, string fileName)
    {
        List<char> separators = ColumnMatrixHelper.FindNumberSeparators(inputMatrix);
        char separator = ColumnMatrixHelper.SelectPopularSeparator(separators);
        string delimiter = ColumnMatrixHelper.GetDelimiter(separator, inputMatrix);
        List<string[]> matrixData = ColumnMatrixHelper.GetMatrixData(inputMatrix, delimiter);
        int startLine = ColumnMatrixHelper.GetStartLine(matrixData, delimiter, inputMatrix);

        List<FullMatrixData> fullMatrixData = new List<FullMatrixData>();
        MatrixJson jsonMatrix = ColumnMatrixHelper.MatrixToJson(matrixData, fileName);

        fullMatrixData.Add(new FullMatrixData { delimiter = delimiter, separator = separator, matrixData = matrixData, matrixJson = jsonMatrix, startLine = startLine });

        return fullMatrixData;
    }

    public static List<FullMatrixData> ParseFortranMatrix(string inputMatrix, string fileName)
    {
        List<char> separators = ColumnMatrixHelper.FindNumberSeparators(inputMatrix);
        char separator = ColumnMatrixHelper.SelectPopularSeparator(separators);
        string delimiter = ColumnMatrixHelper.GetDelimiter(separator, inputMatrix);
        List<string[]> matrixData = ColumnMatrixHelper.GetMatrixData(inputMatrix, delimiter);
        int startLine = ColumnMatrixHelper.GetStartLine(matrixData, delimiter, inputMatrix);

        List<FullMatrixData> fullMatrixData = new List<FullMatrixData>();
        MatrixJson jsonMatrix = ColumnMatrixHelper.MatrixToJson(matrixData, fileName);

        fullMatrixData.Add(new FullMatrixData { delimiter = delimiter, separator = separator, matrixData = matrixData, matrixJson = jsonMatrix, startLine = startLine });

        return fullMatrixData;
    }
}

