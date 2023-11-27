using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Helpers;

namespace input_generator.Classes
{
    class Generator
    {
        public static string generateSquareMatrix(GenerationRules ruleData, MatrixJson matrixData)
        {
            string matrixString = "";
            for (int i = 0; i < ruleData.matrixAtLine; i++)
            {
                matrixString = matrixString + "\r\n";
            }
            for (int i = 0; i < matrixData.connections.Count; i++)
            {
                if (i % matrixData.n == 0 && i > 0)
                {
                    matrixString = matrixString + "\r\n";
                }

                matrixString = matrixString + Helper.CorrectSeparatorValue(matrixData.connections[i].value, ruleData.separator) + ruleData.delimiter;
            }

            return matrixString;
        }
        public static string generateLineMatrix(GenerationRules ruleData, MatrixJson matrixData)
        {
            string matrixString = "";
            for (int i = 0; i < ruleData.matrixAtLine; i++)
            {
                matrixString = matrixString + "\r\n";
            }
            for (int i = 0; i < matrixData.connections.Count; i++)
            {
                matrixString = matrixString + Helper.CorrectSeparatorValue(matrixData.connections[i].value, ruleData.separator) + ruleData.delimiter;
            }

            return matrixString;
        }
        public static string generateColumnMatrix(GenerationRules ruleData, MatrixJson matrixData)
        {
            string matrixString = "";
            for (int i = 0; i < ruleData.matrixAtLine; i++)
            {
                matrixString = matrixString + "\r\n";
            }
            for (int i = 0; i < matrixData.connections.Count; i++)
            {
                if (Helper.ConvertToDouble(matrixData.connections[i].value) > 0)
                {
                    matrixString = matrixString + matrixData.connections[i].from + ruleData.delimiter + matrixData.connections[i].to + ruleData.delimiter + Helper.CorrectSeparatorValue(matrixData.connections[i].value, ruleData.separator) + "\r\n";
                }
            }

            return matrixString;
        }

        public static string AddNotationRules(string matrix, GenerationRules ruleData, MatrixJson matrixData)
        {
            var notation = (YamlMappingNode)ruleData.notation;
            string newMatrix = "";
            for (int i = 0; i < notation.Children.Count; i++)
            {
                if (newMatrix != "")
                {
                    matrix = newMatrix;
                }

                YamlMappingNode notationData = (YamlMappingNode)notation.Children[new YamlScalarNode((i + 1).ToString())];
                string element = notationData.Children[new YamlScalarNode("el")].ToString();
                string formula = notationData.Children[new YamlScalarNode("f")].ToString();
                string value = notationData.Children[new YamlScalarNode("v")].ToString();
                newMatrix = DoNotationRule(matrix, element, formula, value, matrixData);
            }

            return newMatrix;
        }

        private static string DoNotationRule(string matrix, string element, string formula, string value, MatrixJson matrixData)
        {
            string[] elementArray = element.Split(';');
            int x = Int32.Parse(elementArray[0]);
            int y = Int32.Parse(elementArray[1]);
            List<string> lines = GetLines(matrix);

            string line = lines[y];
            string newLine = "";
            if (formula != "NULL")
            {
                char[] formulaChar = formula.ToCharArray();
                if (Array.IndexOf(formulaChar, 'n') >= 0)
                {
                    value = matrixData.n.ToString();
                }

            }

            if (value != "NULL")
            {
                while (line.Length < x + value.Length)
                {
                    {
                        line = line + " ";
                    }
                }

                char[] lineChar = line.ToCharArray();
                for (int i = 0; i < value.Length; i++)
                {
                    lineChar[x + i] = value[i];
                }

                newLine = new string(lineChar);
            }

            string newMatrix = "";

            for (int i = 0; i < lines.Count; i++)
            {
                if (i != y)
                {
                    newMatrix = newMatrix + lines[i] + "\r\n";
                }
                else
                {
                    newMatrix = newMatrix + newLine + "\r\n";
                }

            }

            return newMatrix;

        }

        private static List<string> GetLines(string matrix)
        {
            List<string> matrixLines = new List<string>();
            string matrixLine = "";
            for (int i = 0; i < matrix.Length; i++)
            {
                if (matrix[i] == '\n')
                {
                    matrixLines.Add(matrixLine);
                    matrixLine = "";

                }
                else if (matrix[i] != '\r' && matrix[i] != '\n')
                {
                    matrixLine = matrixLine + matrix[i];
                }
            }

            return matrixLines;
        }

    }

}
