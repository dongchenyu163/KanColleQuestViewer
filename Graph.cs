using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindFusion.Diagramming.Wpf;
using System.Collections.ObjectModel;

namespace KanColleQuestViewer
{
	class Graph
	{
		static Collection<ShapeNode> gs_GraphData = new Collection<ShapeNode>();
		public static void DisplayALL(Diagram Graph,Collection<KanColleQuest> Data)
		{
			#region 画图控件例程

			//// load node data
			//var nodes = xml.Descendants("Node");
			//foreach (var node in nodes)
			//{
			//	var diagramNode = MainWindow.graphss.Factory.CreateShapeNode(bounds);
			//	nodeMap[node.Attribute("id").Value] = diagramNode;
			//	diagramNode.Text = node.Attribute("name").Value;
			//	diagramNode.MouseDown += DiagramNode_MouseDown;
			//}

			//// load link data
			//var links = xml.Descendants("Link");
			//foreach (var link in links)
			//{
			//	DiagramLink __ = graphss.Factory.CreateDiagramLink(
			//		nodeMap[link.Attribute("origin").Value],
			//		nodeMap[link.Attribute("target").Value]);

			//}

			//// arrange the graph
			//var layout = new LayeredLayout();
			//layout.Arrange(graphss);
			#endregion
			System.Windows.Rect _bound=new System.Windows.Rect(new System.Windows.Point(100,0),
				new System.Windows.Size(50,24));

			foreach (var item in Data)
			{
				var _newNode = Graph.Factory.CreateShapeNode(_bound);
				_newNode.Text = item.ID;
				gs_GraphData.Add(_newNode);
			}

			foreach (var item in Data)
			{
				foreach (var preLinkInfo_item in item.LinkInfoOfQuest.preLinkInfo)
				{
					DiagramLink _tmp= Graph.Factory.CreateDiagramLink(
						FindGraphDataByQuestID(preLinkInfo_item.ID),
						FindGraphDataByQuestID(item.ID)
						);

				}
			}

			var layout = new MindFusion.Diagramming.Wpf.Layout.LayeredLayout();
			layout.Arrange(Graph);
		}

		private static DiagramNode FindGraphDataByQuestID(string ID)
		{
			foreach (var item in gs_GraphData)
			{
				if (item.Text.ToUpper() == ID.ToUpper())
					return item;
			}
			return null;
		}
	}
}
