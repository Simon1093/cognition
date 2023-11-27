using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

class GraphBuilder
{
    public Microsoft.Msagl.GraphViewerGdi.GViewer BuildGraph(List<PrimaryGraph.Verticle> new_graph, int verticle)
    {

        //create a viewer object 
        Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
        //create a graph object 
        Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
        //create the graph content 
        for (int i = 0; i < new_graph.Count; i++)
        {
            for (int j = 0; j < new_graph[i].connections.Count; j++)
            {
                int verticle_out = new_graph[i].verticle_id;
                int verticle_in = new_graph[i].connections[j].connectedTo;
                if (verticle_out == verticle || verticle_in == verticle)
                {
                    graph.AddEdge(verticle_out.ToString(), new_graph[i].connections[j].strength.ToString(), verticle_in.ToString());
                }
            }
        }

        //graph.FindNode(verticle).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
        graph.Attr.LayerDirection = Microsoft.Msagl.Drawing.LayerDirection.LR;
        viewer.Graph = graph;
        viewer.Height = 300;
        viewer.Width = 930;

        return viewer;
    }

    public Microsoft.Msagl.GraphViewerGdi.GViewer BuildGraphBig(List<PrimaryGraph.Verticle> new_graph)
    {
        //create a viewer object 
        Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
        //create a graph object 
        Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
        //create the graph content 
        for (int i = 0; i < new_graph.Count; i++)
        {
            for (int j = 0; j < new_graph[i].connections.Count; j++)
            {
                int verticle_out = new_graph[i].verticle_id;
                int verticle_in = new_graph[i].connections[j].connectedTo;
                {
                    graph.AddEdge(verticle_out.ToString(), new_graph[i].connections[j].strength.ToString(), verticle_in.ToString());
                }
            }
        }

        graph.Attr.LayerDirection = Microsoft.Msagl.Drawing.LayerDirection.LR;
        viewer.Graph = graph;
        viewer.ZoomF = 1;

        return viewer;
    }
}

