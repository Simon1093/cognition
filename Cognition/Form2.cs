using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cognition
{
    public partial class Form2 : Form
    {
        List<PrimaryGraph.Verticle> graph;
        public Form2(List<PrimaryGraph.Verticle> loaded_graph)
        {
            InitializeComponent();
            this.graph = loaded_graph;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            GraphBuilder graphBuilder = new GraphBuilder();
            Microsoft.Msagl.GraphViewerGdi.GViewer graphView = graphBuilder.BuildGraphBig(this.graph);
            this.SuspendLayout();
            graphView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(graphView);
            this.ResumeLayout();
        }
    }
}
