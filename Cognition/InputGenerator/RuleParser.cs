using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Helpers;
using System.IO;
using System.Text.RegularExpressions;

namespace input_generator.Classes
{
    class RuleParser
    {
        public static GenerationRules ParseRules(string rules)
        {
            rules = Regex.Replace(rules, @"r_bracket", "\"");
            var input = new StringReader(rules);
            var yaml = new YamlStream();
            yaml.Load(input);

            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            List<GenerationRules> generationRules = new List<GenerationRules>();
            generationRules.Add(new GenerationRules
            {
                delimiter = mapping.Children[new YamlScalarNode("Delimiter")].ToString(),
                separator = mapping.Children[new YamlScalarNode("Separator")].ToString(),
                matrixAtLine = Int32.Parse(mapping.Children[new YamlScalarNode("StartDrawMatrixAtLine")].ToString()),
                notation = mapping.Children[new YamlScalarNode("NotationRules")],
                type = mapping.Children[new YamlScalarNode("Type")].ToString()
            });

            return generationRules[0];
        }
    }

    public class GenerationRules
    {
        public string delimiter;
        public string separator;
        public int matrixAtLine;
        public string type;
        public YamlNode notation;
    }


}
