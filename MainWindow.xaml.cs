using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MindFusion.Diagramming.Wpf;
using MindFusion.Diagramming.Wpf.Layout;
using System.Xml.Linq;
using System.IO;

namespace KanColleQuestViewer
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		public static string gs_url= @"https://zh.moegirl.org/%E8%88%B0%E9%98%9FCollection/%E4%BB%BB%E5%8A%A1#.E6.94.B9.E8.A3.85";
		public static string gs_url_tmp = @"C:\Users\Dong Chenyu\Documents\Visual Studio 2015\Projects\KanColleQuestViewer\KanColleQuestViewer\tmp.html";
		public static string gs_HTTP_Res;

		public static string gs_Find_UpdateTime = " < b><u>";
		public static string gs_Find_UpdateTime_end = "</u>";
		string gs_Find_Quest = "<td id=\"";
		string gs_Find_Quest_end = "</tr>";

		public static string gs_UpdateTime = "";
		
		public MainWindow()
		{
			InitializeComponent();

			//gs_HTTP_Res = HTTP.GetHtmlContent(gs_url, Encoding.UTF8);//正常读取

			//从文件读取
			FileStream _fS=File.OpenRead(gs_url_tmp);
			byte[] _tmp=new byte[_fS.Length];

			_fS.Read(_tmp, 0, (int)_fS.Length);
			gs_HTTP_Res = Encoding.UTF8.GetString(_tmp);

			HTML_StringProcess();
			//XML.SaveQuestsClass_To_XML(KanColleQuest.Quests);

			//XML.ReadXML_To_QuestsClass();

			#region 画图控件例程

			var nodeMap = new Dictionary<string, DiagramNode>();
			var bounds = new Rect(0, 0, 60, 22);

			// load the graph xml
			var xml = XDocument.Load(@"C:\Users\Dong Chenyu\Documents\Visual Studio 2015\Projects\KanColleQuestViewer\KanColleQuestViewer\QuestsForGraph.xml");
			var graph = xml.Element("Graph");

			// load node data
			var nodes = xml.Descendants("Node");
			foreach (var node in nodes)
			{
				var diagramNode = graphss.Factory.CreateShapeNode(bounds);
				nodeMap[node.Attribute("id").Value] = diagramNode;
				diagramNode.Text = node.Attribute("name").Value;
				diagramNode.MouseDown += DiagramNode_MouseDown;
			}

			// load link data
			var links = xml.Descendants("Link");
			foreach (var link in links)
			{
				DiagramLink __ = graphss.Factory.CreateDiagramLink(
					nodeMap[link.Attribute("origin").Value],
					nodeMap[link.Attribute("target").Value]);
				__.MouseDown += ___MouseDown;
			}

			// arrange the graph
			var layout = new LayeredLayout();
			layout.Arrange(graphss);
			#endregion
		}

		private void ___MouseDown(object sender, MouseButtonEventArgs e)
		{
			Tx_Res.Text = ((DiagramLink)sender).StartPoint.ToString();
		}

		private void DiagramNode_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Tx_Res.Text=((ShapeNode)sender).Text;
		}

		private void HTML_StringProcess()
		{
			int _IndexSt = 0;
			int _IndexEd = 0;
			int _count = 0;
			string _quest = "";
			string _copyOf_gs_HTTP_Res = gs_HTTP_Res;
			
			// 获取更新日期
			MyString.GetMidString(_copyOf_gs_HTTP_Res, 
				ref gs_UpdateTime, 
				gs_Find_UpdateTime, 
				gs_Find_UpdateTime_end);
			#region 将更新时间字符串转化为DateTime形变量
			//System.Globalization.CultureInfo zhCN = new System.Globalization.CultureInfo("zh-CN");
			////TODO:编码转换
			////string s = "这里是测试字符串";Unicode UTF8
			////string utf8_string = Encoding.UTF8.GetString(Encoding.Default.GetBytes(s));
			
			//string utf8_string = Encoding.Unicode.GetString(Encoding.Convert(Encoding.UTF8 ,Encoding.Unicode,Encoding.UTF8.GetBytes(gs_UpdateTime)));
			//DateTime.TryParseExact(utf8_string, "yyyy年MM月dd日hh:mm", 
			//	zhCN, 
			//	System.Globalization.DateTimeStyles.None,
			//	out KanColleQuest.UpdateTime);
			//KanColleQuest.UpdateTime = DateTime.Parse(gs_UpdateTime);
			#endregion

			while (true)
			{
				_IndexSt = _copyOf_gs_HTTP_Res.IndexOf(gs_Find_Quest, 0);//寻找st在src的位置，返回st第一个字符的位置。
				_IndexSt += gs_Find_Quest.Length;//开始位置变更至末尾
				_IndexEd = _copyOf_gs_HTTP_Res.IndexOf(gs_Find_Quest_end, _IndexSt);

				if(_IndexSt==-1|| _IndexEd==-1)
				{
					break;
				}

				_count++;

				_quest= _copyOf_gs_HTTP_Res.Substring(_IndexSt, _IndexEd - _IndexSt);

				KanColleQuest.CreateStringProcess(_quest);
				//删除前面的字符。
				_copyOf_gs_HTTP_Res=_copyOf_gs_HTTP_Res.Substring(_IndexEd);
			}
		}
	}
}
