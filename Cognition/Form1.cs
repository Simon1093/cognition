using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using AutoMapper;
using System.Text.RegularExpressions;

namespace Cognition
{

    public partial class Form1 : Form
    {
        FullMatrixData matrixData = new FullMatrixData
        {
            graph = new List<PrimaryGraph.Verticle>()
        };
        
        string base_dir;
        int sort = 1;

        input_generator.Classes.GenerationRules ruleDataGlobal = new input_generator.Classes.GenerationRules();
        MatrixJson matrixJsonGlobal = new MatrixJson();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AppDomain domain = AppDomain.CreateDomain("/");
            base_dir = domain.BaseDirectory;
        }

        //Add verticle
        private void button5_Click(object sender, EventArgs e)
        {
            enableAddVerticle(false);
            List<PrimaryGraph.Connections> VerticleConnection = new List<PrimaryGraph.Connections>();

            int maxVerticleID = 0;
            for (int i = 0; i < matrixData.graph.Count; i++)
            {
                if(maxVerticleID < matrixData.graph[i].verticle_id)
                {
                    maxVerticleID = matrixData.graph[i].verticle_id;
                }
            }

            matrixData.graph.Add(new PrimaryGraph.Verticle { verticle_id = maxVerticleID + 1, verticle = textBox1.Text, connections = VerticleConnection });

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            renderVerticlesList();

            textBox1.Clear();
        }

        //Edit verticle
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null || textBox1.Text.Length == 0) return;

            string selectedVerticle = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None)[0];

            for (int i = 0; i < matrixData.graph.Count; i++)
            {
                if(matrixData.graph[i].verticle_id.ToString() == selectedVerticle)
                {
                    matrixData.graph[i].verticle = textBox1.Text;
                    renderPrimaryGraphBinding();
                    renderVerticlesList(i);
                }
            }
        }

        //Delete verticle
        private void button21_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null) return;

            primaryGraphBindingSource.Clear();

            string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
            int verticleNumber = Int32.Parse(verticleSplit[0]);

            for (int i = 0; i < matrixData.graph.Count; i++)
            {
                if (matrixData.graph[i].verticle_id == verticleNumber)
                {
                    matrixData.graph.Remove(matrixData.graph[i]);
                }
            }

            for (int i = 0; i < matrixData.graph.Count; i++)
            {
                for (int j = 0; j < matrixData.graph[i].connections.Count; j++)
                {
                    if (matrixData.graph[i].connections[j].connectedTo == verticleNumber)
                    {
                        matrixData.graph[i].connections.Remove(matrixData.graph[i].connections[j]);
                    }
                }
            }

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            textBox1.Clear();

            renderVerticlesList();
        }

        // Clear Attribute Verticle
        private void button14_Click_1(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null) return;

            string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
            int verticleNumber = Int32.Parse(verticleSplit[0]);

            for (int i = 0; i < matrixData.graph.Count; i++)
            {
                if (matrixData.graph[i].verticle_id == verticleNumber)
                {
                    matrixData.graph[i].type = 0;
                }
            }

            renderVerticlesList(listBox2.SelectedIndex);
        }

        // =0 Verticle
        private void button11_Click_1(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null) return;

            string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
            int verticleNumber = Int32.Parse(verticleSplit[0]);

            for (int i = 0; i < matrixData.graph.Count; i++)
            {
                if (matrixData.graph[i].verticle_id == verticleNumber)
                {
                    matrixData.graph[i].type = 1;
                }
            }

            renderVerticlesList(listBox2.SelectedIndex);
        }

        // >=0 Verticle
        private void button12_Click_1(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null) return;

            string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
            int verticleNumber = Int32.Parse(verticleSplit[0]);

            for (int i = 0; i < matrixData.graph.Count; i++)
            {
                if (matrixData.graph[i].verticle_id == verticleNumber)
                {
                    matrixData.graph[i].type = 2;
                }
            }

            renderVerticlesList(listBox2.SelectedIndex);
        }

        // <0 Verticle
        private void button13_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null) return;

            string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
            int verticleNumber = Int32.Parse(verticleSplit[0]);

            for (int i = 0; i < matrixData.graph.Count; i++)
            {
                if (matrixData.graph[i].verticle_id == verticleNumber)
                {
                    matrixData.graph[i].type = 3;
                }
            }

            renderVerticlesList(listBox2.SelectedIndex);
        }

        //Save...
        private void button6_Click(object sender, EventArgs e)
        {
            string matrixText = "";

            for (int i = 0; i < matrixData.graph.Count; i++)
            {
                matrixText += matrixData.delimiter + matrixData.graph[i].verticle;
            }

            for (int i = 0; i < matrixData.graph.Count; i++)
            {
                matrixText += '\n' + matrixData.graph[i].verticle;

                for (int j = 0; j < matrixData.graph.Count; j++)
                {
                    int searchVerticleID = matrixData.graph[j].verticle_id;
                    PrimaryGraph.Connections foundVerticle = matrixData.graph[i].connections.Find(s => s.connectedTo == searchVerticleID);

                    if (foundVerticle != null)
                    {
                        matrixText += matrixData.delimiter + (foundVerticle.strength.ToString()).Replace(',', matrixData.separator);
                    }
                    else
                    {
                        matrixText += matrixData.delimiter + "0";
                    }
                }
            }

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "matrix.txt";
            savefile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            try
            {
                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter sw = new StreamWriter(savefile.FileName);
                    sw.WriteLine(matrixText);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Add influence factor
        private void button7_Click_1(object sender, EventArgs e)
        {
            primaryGraphBindingSource.Clear();
            addFactor("influence");
            renderPrimaryGraphBinding();
        }

        //Add impact factor
        private void button8_Click(object sender, EventArgs e)
        {
            primaryGraphBindingSource1.Clear();
            addFactor("impact");
            renderPrimaryGraphBinding();
        }

        //Load matrix file
        private void button9_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            textBox1.Clear();
            button7.Enabled = false;
            button8.Enabled = false;
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        string filename = openFileDialog1.FileName;
                        string fileText = System.IO.File.ReadAllText(filename);
                        MatrixParser parser = new MatrixParser();
                        matrixData = InputMatrixParser.ParseSquareMatrix(fileText, filename);
                        renderVerticlesList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //Show full graph
        private void button10_Click(object sender, EventArgs e)
        {
            var form2 = new Form2(matrixData.graph);
            form2.Show();
        }

        //Run math algorithm
        private void button11_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog2 = new OpenFileDialog();
            openFileDialog2.Filter = "All Files|*.*";
            openFileDialog2.FilterIndex = 2;
            openFileDialog2.RestoreDirectory = true;
            Stream myStream = null;
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog2.OpenFile()) != null)
                    {
                        string fileName = openFileDialog2.SafeFileName;
                        string filePath = Path.GetDirectoryName(openFileDialog2.FileName);
                        string data = Classes.MathModuleController.rumModule(fileName, filePath);
                        //richTextBox1.Text = data;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        //Load input data sample
        private void button12_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        string filename = openFileDialog1.FileName;
                        string fileText = System.IO.File.ReadAllText(filename);
                        MatrixParser parser = new MatrixParser();
                        FullMatrixData parseData = InputMatrixParser.ParseSquareMatrix(fileText, filename);
                        // richTextBox2.Text = Regex.Replace(parseData[0].yamlRules, @"r_bracket", "\"");

                        ruleDataGlobal = input_generator.Classes.RuleParser.ParseRules(parseData.yamlRules);
                        //input_generator.Classes.Generator.generateSquareMatrix(ruleData, parseData[0].matrixJson);
                        //richTextBox4.Text = 
                        //System.IO.File.WriteAllText("generated-matrix.txt", matrixJson);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //Load matrix to convert with rules
        private void button14_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        string filename = openFileDialog1.FileName;
                        string fileText = System.IO.File.ReadAllText(filename);
                        MatrixParser parser = new MatrixParser();
                        FullMatrixData parseData = InputMatrixParser.ParseSquareMatrix(fileText, filename);
                        matrixJsonGlobal = parseData.matrixJson;
                        //  richTextBox3.Text = JsonConvert.SerializeObject(matrixJsonGlobal);

                        //GenerationRules ruleData = input_generator.Classes.RuleParser.ParseRules(parseData[0].yamlRules);
                        //input_generator.Classes.Generator.generateSquareMatrix(ruleData, parseData[0].matrixJson);
                        //richTextBox4.Text = 
                        //System.IO.File.WriteAllText("generated-matrix.txt", matrixJson);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //Do matrix convert
        private void button15_Click(object sender, EventArgs e)
        {

            Mapper.Initialize(cfg => { cfg.CreateMap<MatrixJson, input_generator.Classes.MatrixJson>(); });
            var matrixJson = Mapper.Map<input_generator.Classes.MatrixJson>(matrixJsonGlobal);
            string generatedMatrix = input_generator.Classes.Generator.generateSquareMatrix(ruleDataGlobal, matrixJson);
            Mapper.Reset();
            // richTextBox4.Text = generatedMatrix;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            checkEnableAddVerticle();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != null)
            {
                button7.Enabled = true;
            }
            else
            {
                button7.Enabled = false;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != null)
            {
                button8.Enabled = true;
            }
            else
            {
                button8.Enabled = false;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null) return;

            enableAddVerticle(true);

            string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
            textBox1.Text = verticleSplit[1];

            renderPrimaryGraphBinding();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Delete influence verticle
            if (e.ColumnIndex == 4 && listBox2.Items.Count > 0)
            {
                string verticle_delete;
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                verticle_delete = row.Cells[0].Value.ToString();

                string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);

                for (int i = 0; i < matrixData.graph.Count; i++)
                {
                    if (matrixData.graph[i].verticle_id.ToString() == verticleSplit[0])
                    {
                        for (int j = 0; j < matrixData.graph[i].connections.Count; j++)
                        {
                            if (matrixData.graph[i].connections[j].connectedTo.ToString() == verticle_delete)
                            {
                                matrixData.graph[i].connections.Remove(matrixData.graph[i].connections[j]);
                                renderPrimaryGraphBinding();
                            }
                        }
                    }
                }
            }

            if (e.ColumnIndex == 3 && listBox2.Items.Count > 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                string clicked_verticle_id = row.Cells[0].Value.ToString();
                string clicked_verticle_name = row.Cells[1].Value.ToString();
                string clicked_verticle_value = row.Cells[2].Value.ToString();
                string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
                string action_verticle = "";

                if (Double.Parse(clicked_verticle_value) > 0)
                {
                    action_verticle = "increases ";
                }
                else
                {
                    action_verticle = "decreases ";
                }

                MessageBox.Show(@"Increasing  attribute """ + verticleSplit[1] + @""" by 1 " + action_verticle + @"""" + clicked_verticle_name + @"""" + " by " + clicked_verticle_value);

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Delete impact verticle
            if (e.ColumnIndex == 4)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string verticle_delete = row.Cells[0].Value.ToString();

                string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);

                for (int i = 0; i < matrixData.graph.Count; i++)
                {
                    if (matrixData.graph[i].verticle_id.ToString() == verticle_delete)
                    {
                        for (int j = 0; j < matrixData.graph[i].connections.Count; j++)
                        {
                            if (matrixData.graph[i].connections[j].connectedTo.ToString() == verticleSplit[0])
                            {
                                matrixData.graph[i].connections.Remove(matrixData.graph[i].connections[j]);
                                renderPrimaryGraphBinding();
                            }
                        }
                    }
                }
            }

            if (e.ColumnIndex == 3 && listBox2.Items.Count > 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string clicked_verticle_id = row.Cells[0].Value.ToString();
                string clicked_verticle_name = row.Cells[1].Value.ToString();
                string clicked_verticle_value = row.Cells[2].Value.ToString();
                string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
                string action_verticle = "";

                if (Double.Parse(clicked_verticle_value) > 0)
                {
                    action_verticle = "increases ";
                }
                else
                {
                    action_verticle = "decreases ";
                }

                MessageBox.Show(@"Increasing  attribute """ + clicked_verticle_name + @""" by 1 " + action_verticle + @"""" + verticleSplit[1] + @"""" + " by " + clicked_verticle_value);

            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            string verticleEdit = row.Cells[0].Value.ToString();
            //Edit impact
            if (e.ColumnIndex == 1)
            {
                string selectedVerticle = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None)[0];
                string newValue = row.Cells[1].Value.ToString();
                for (int i = 0; i < matrixData.graph.Count; i++)
                {
                    if (matrixData.graph[i].verticle_id.ToString() == verticleEdit)
                    {
                        matrixData.graph[i].verticle = newValue;
                        renderPrimaryGraphBinding();
                        renderVerticlesList(listBox2.SelectedIndex);
                    }
                }
            }
            else if (e.ColumnIndex == 2)
            {
                string selectedVerticle = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None)[0];
                string newValue = row.Cells[2].Value.ToString().Replace('.', ',');
                for (int i = 0; i < matrixData.graph.Count; i++)
                {
                    if (matrixData.graph[i].verticle_id.ToString() == verticleEdit)
                    {
                        for (int j = 0; j < matrixData.graph[i].connections.Count; j++)
                        {
                            if (matrixData.graph[i].connections[j].connectedTo.ToString() == selectedVerticle)
                            {
                                matrixData.graph[i].connections[j].strength = Double.Parse(newValue);
                                renderPrimaryGraphBinding();
                            }
                        }
                    }
                }
            }
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
            string verticleEdit = row.Cells[0].Value.ToString();
            //Edit influence
            if (e.ColumnIndex == 1)
            {
                string newValue = row.Cells[1].Value.ToString();
                for (int i = 0; i < matrixData.graph.Count; i++)
                {
                    if (matrixData.graph[i].verticle_id.ToString() == verticleEdit)
                    {
                        matrixData.graph[i].verticle = newValue;
                        renderVerticlesList(listBox2.SelectedIndex);
                        renderPrimaryGraphBinding();
                    }
                }
            }
            else if (e.ColumnIndex == 2)
            {
                string selectedVerticle = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None)[0];
                string newValue = row.Cells[2].Value.ToString().Replace('.', ',');
                for (int i = 0; i < matrixData.graph.Count; i++)
                {
                    if (matrixData.graph[i].verticle_id.ToString() == selectedVerticle)
                    {
                        for (int j = 0; j < matrixData.graph[i].connections.Count; j++)
                        {
                            if (matrixData.graph[i].connections[j].connectedTo.ToString() == verticleEdit)
                            {
                                matrixData.graph[i].connections[j].strength = Double.Parse(newValue);
                                renderPrimaryGraphBinding();
                            }
                        }
                    }
                }
            }
        }

        private void dataGridView3_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch(e.ColumnIndex)
            {
                case 0:
                    matrixData.computationResults.Sort((a, b) => Convert.ToDouble(a.ID) > Convert.ToDouble(b.ID) ? -1 * sort : 1 * sort);
                    break;
                case 1:
                    matrixData.computationResults.Sort((a, b) => Convert.ToDouble(a.X) > Convert.ToDouble(b.X) ? -1 * sort : 1 * sort);
                    break;
                case 2:
                    matrixData.computationResults.Sort((a, b) => Convert.ToDouble(a.Y) > Convert.ToDouble(b.Y) ? -1 * sort : 1 * sort);
                    break;
                case 3:
                    matrixData.computationResults.Sort((a, b) => Convert.ToDouble(a.X1) > Convert.ToDouble(b.X1) ? -1 * sort : 1 * sort);
                    break;
                case 4:
                    matrixData.computationResults.Sort((a, b) => Convert.ToDouble(a.Y1) > Convert.ToDouble(b.Y1) ? -1 * sort : 1 * sort);
                    break;
            }

            //sort = sort * -1;

            computationResultsBindingSource.Clear();

            foreach (ComputationResults result in matrixData.computationResults)
            {
                computationResultsBindingSource.Add(result);
            }
        }

        //Do math
        private void button1_Click(object sender, EventArgs e)
        {
            string matrixText = matrixData.graph.Count + "\t" + this.textBox2.Text + "\t" + this.textBox3.Text + "\n";

            matrixData.graph.Sort((a, b) => a.verticle_id > b.verticle_id ? 1 : -1);

            string type1 = "";
            int type1Amount = 0;
            string type2 = "";
            int type2Amount = 0;
            string type3 = "";
            int type3Amount = 0;

            for (int i = 0; i < matrixData.graph.Count; i++)
            {
                switch(matrixData.graph[i].type)
                {
                    case 1:
                        type1 +=(i + 1) + "\t";
                        type1Amount++;
                        break;
                    case 2:
                        type2 += (i + 1) + "\t";
                        type2Amount++;
                        break;
                    case 3:
                        type3 += (i + 1) + "\t";
                        type3Amount++;
                        break;
                }
            }

            matrixText += type1Amount + "\t" + (type1Amount + type2Amount) + "\t" + (type1Amount + type2Amount + type3Amount) + "\n";

            if(type1Amount + type2Amount + type3Amount > 0)
            {
                matrixText += type1 + type2 + type3 + "\n";
            }

            matrixText += "source" + "\t " + "receiver" + "\t" + "value" + "\t" + "weight" + "\n";

            for (int i = 0; i < matrixData.graph.Count; i++)
            {
                for(int j = 0; j < matrixData.graph[i].connections.Count; j++)
                {
                    matrixText += matrixData.graph[i].verticle_id + "\t" + matrixData.graph[i].connections[j].connectedTo + "\t" + '0' + "\t" + matrixData.graph[i].connections[j].strength.ToString().Replace(',', '.') + "\n";
                }           
            }

            try
            {
                if (File.Exists(@"temp/a_matrix.txt"))
                {
                    File.Delete(@"temp/a_matrix.txt");
                }

                if (File.Exists(@"temp/report.txt"))
                {
                    File.Delete(@"temp/report.txt");
                }

                StreamWriter sw = new StreamWriter("temp/a_matrix.txt");
                sw.WriteLine(matrixText);
                sw.Close();

                string filename = Path.Combine("temp", "a.exe");
                Process myProcess = new Process();
                string parameters = "";
                ProcessStartInfo processInfo = new ProcessStartInfo("temp/a.exe", "/c " + filename);

                processInfo.UseShellExecute = false;
                processInfo.WorkingDirectory = "temp";

                myProcess.StartInfo = processInfo;
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.RedirectStandardOutput = true;
                myProcess.StartInfo.RedirectStandardInput = true;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
                myProcess.StandardInput.WriteLine("/c exit");
                string output = myProcess.StandardOutput.ReadToEnd();

                matrixData.impacts = new List<Factor>(); ;
                matrixData.influences = new List<Factor>(); ;

                string fileReportName = "temp/report.txt";
                int lineNum = 0;
                using (var reader = File.OpenText(fileReportName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if(lineNum == 0)
                        {
                            int factorNum = 0;
                            Regex r = new Regex(@"(-?0[Xx][A-Fa-f0-9]+|-?\d+\.\d+|-?\d+)");
                            var m = r.Matches(line);
                            foreach (Match h in m)
                            {
                                if (factorNum == 1)
                                {
                                    label2.Text = h.Value.Replace('.', ',');
                                }
                                factorNum++;
                            }
                        }
                        if (lineNum > 0)
                        {
                            int factorNum = 0;
                            Regex r = new Regex(@"(-?0[Xx][A-Fa-f0-9]+|-?\d+\.\d+|-?\d+)");
                            var m = r.Matches(line);
                            foreach (Match h in m)
                            {
                                Factor factor = new Factor();
                                factor.value = double.Parse(h.Value.Replace('.', ','));
                                factor.verticle_id = matrixData.graph[lineNum - 1].verticle_id;
                                if (factorNum == 0)
                                {
                                    matrixData.impacts.Add(factor);
                                } else
                                {
                                    matrixData.influences.Add(factor);
                                }
                                factorNum++;
                            }
                        }
                        lineNum++;
                    }
                }

                computationResultsBindingSource.Clear();

                List<ComputationResults> results = new List<ComputationResults>();
                matrixData.computationResults = new List<ComputationResults>();

                matrixData.influencesNormalized = new List<Factor>();
                double influencesSquaresSum = 0;
                for (int i = 0; i < matrixData.influences.Count; i++)
                {
                    influencesSquaresSum += Math.Pow(matrixData.influences[i].value, 2);
                    ComputationResults result = new ComputationResults(
                            "",
                            matrixData.influences[i].verticle_id,
                            Math.Round(matrixData.impacts[i].value, 8), 
                            Math.Round(matrixData.influences[i].value, 8)
                        );
                    results.Add(result);
                }

                matrixData.impactsNormalized = new List<Factor>();
                double impactsSquaresSum = 0;
                for (int i = 0; i < matrixData.impacts.Count; i++)
                {
                    impactsSquaresSum += Math.Pow(matrixData.impacts[i].value, 2);
                }

                List<double> influencesNormalized = new List<double>();
                double influencesNormalizedSum = 0;
                for (int i = 0; i < matrixData.influences.Count; i++)
                {
                    Factor influence = new Factor();
                    influence.value = Math.Pow(matrixData.influences[i].value, 2) / influencesSquaresSum;
                    influence.verticle_id = matrixData.influences[i].verticle_id;
                    matrixData.influencesNormalized.Add(influence);
                    influencesNormalizedSum += influence.value;
                    results[i].Y1 = Math.Round(influence.value, 8);
                }

                List<double> impactsNormalized = new List<double>();
                double impactsNormalizedSum = 0;
                for (int i = 0; i < matrixData.impacts.Count; i++)
                {
                    Factor impact = new Factor();
                    impact.value = Math.Pow(matrixData.impacts[i].value, 2) / impactsSquaresSum;
                    impact.verticle_id = matrixData.impacts[i].verticle_id;
                    matrixData.impactsNormalized.Add(impact);
                    impactsNormalizedSum += impact.value;
                    results[i].X1 = Math.Round(impact.value, 8);

                    matrixData.computationResults.Add(results[i]);
                    computationResultsBindingSource.Add(results[i]);
                }

                this.label6.Text = impactsNormalizedSum.ToString();
                this.label7.Text = influencesNormalizedSum.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void copyAlltoClipboard()
        {
            dataGridView3.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        //csv export
        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text|*.csv";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {      
                    string filename = saveFileDialog1.FileName;

                    var sb = new StringBuilder();
                    var headers = dataGridView3.Columns.Cast<DataGridViewColumn>();
                    sb.AppendLine(string.Join(",", headers.Select(column => "\"" + column.HeaderText + "\"").ToArray()));
                    foreach (DataGridViewRow row in dataGridView3.Rows)
                    {
                        var cells = row.Cells.Cast<DataGridViewCell>();
                        sb.AppendLine(string.Join(",", cells.Select(cell => "\"" + cell.Value + "\"").ToArray()));
                    }

                    File.WriteAllText(filename, sb.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void addFactor(string type)
        {
            if (type == "influence")
            {
                string[] verticleSplitAdd = comboBox1.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
                int verticleNumberAdd = Int32.Parse(verticleSplitAdd[0]);

                string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
                int verticleNumber = Int32.Parse(verticleSplit[0]);
                int verticlesCount = matrixData.graph.Count;

                for (int i = 0; i < verticlesCount; i++)
                {
                    if (matrixData.graph[i].verticle_id.ToString() == verticleSplit[0])
                    {
                        matrixData.graph[i].connections.Add(new PrimaryGraph.Connections { connectedTo = verticleNumberAdd, strength = 0 });
                    }
                }
            }
            else if (type == "impact")
            {
                string[] verticleSplitAdd = comboBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
                int verticleNumberAdd = Int32.Parse(verticleSplitAdd[0]);

                string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
                int verticleNumber = Int32.Parse(verticleSplit[0]);
                int verticlesCount = matrixData.graph.Count;

                for (int i = 0; i < verticlesCount; i++)
                {
                    if (matrixData.graph[i].verticle_id.ToString() == verticleSplitAdd[0])
                    {
                        matrixData.graph[i].connections.Add(new PrimaryGraph.Connections { connectedTo = verticleNumber, strength = 0 });
                    }
                }
            }
        }

        private void renderVerticlesList(Nullable<int> selectedIndex = null)
        {
            listBox2.Items.Clear();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            for (int i = 0; i < matrixData.graph.Count; i++)
            {
                string type = "";

                switch(matrixData.graph[i].type)
                {
                    case 1:
                        type = " N(0)";
                        break;
                    case 2:
                        type = " N(+)";
                        break;
                    case 3:
                        type = " N(-)";
                        break;
                }

                listBox2.Items.Add(matrixData.graph[i].verticle_id + ") " + matrixData.graph[i].verticle + type);
                comboBox1.Items.Add(matrixData.graph[i].verticle_id + ") " + matrixData.graph[i].verticle);
                comboBox2.Items.Add(matrixData.graph[i].verticle_id + ") " + matrixData.graph[i].verticle);
            }

            if (selectedIndex != null)
            {
                listBox2.SelectedIndex = selectedIndex.Value;
            }
        }

        private void renderGraph()
        {
            panel1.Controls.Clear();
            string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
            GraphBuilder graphBuilder = new GraphBuilder();
            Microsoft.Msagl.GraphViewerGdi.GViewer graphView = graphBuilder.BuildGraph(matrixData.graph, int.Parse(verticleSplit[0]));
            panel1.SuspendLayout();
            graphView.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Controls.Add(graphView);
            panel1.ResumeLayout();
        }

        private void renderPrimaryGraphBinding()
        {
            //Show influence
            if (listBox2.SelectedItem == null) return;

            primaryGraphBindingSource.Clear();

            string[] verticleSplit = listBox2.SelectedItem.ToString().Split(new[] { ") " }, StringSplitOptions.None);
            int verticleNumber = Int32.Parse(verticleSplit[0]);

            for (int j = 0; j < matrixData.graph.Count; j++)
            {
                if (verticleSplit[0] == matrixData.graph[j].verticle_id.ToString())
                    for (int i = 0; i < matrixData.graph[j].connections.Count; i++)
                    {
                        int connected_to = matrixData.graph[j].connections[i].connectedTo;
                        for (int k = 0; k < matrixData.graph.Count; k++)
                        {
                            if (matrixData.graph[k].verticle_id == connected_to && matrixData.graph[j].verticle != null)
                                primaryGraphBindingSource.Add(new DataGrid(
                                    matrixData.graph[k].verticle,
                                    connected_to,
                                    matrixData.graph[j].connections[i].strength.ToString(),
                                    true
                                    ));
                        }
                    }
            }

            //Show impact
            primaryGraphBindingSource1.Clear();
            for (int i = 0; i < matrixData.graph.Count; i++)
            {
                for (int j = 0; j < matrixData.graph[i].connections.Count; j++)
                {
                    if (matrixData.graph[i].connections[j].connectedTo == verticleNumber)
                    {
                        int verticle_id = matrixData.graph[i].verticle_id;
                        string verticle = matrixData.graph[i].verticle;
                        if (verticle != null)
                            primaryGraphBindingSource1.Add(new DataGrid(
                                verticle,
                                verticle_id,
                                matrixData.graph[i].connections[j].strength.ToString(),
                                true
                                ));
                    }
                }
            }
            renderGraph();
        }

        private void enableAddVerticle(bool enable)
        {
            comboBox1.Enabled = enable;
            comboBox2.Enabled = enable;
        }

        private void checkEnableAddVerticle()
        {
            if (textBox1.Text != "")
            {
                button5.Enabled = true;
            }
            else
            {
                button5.Enabled = false;
            }
        }
    }
}
