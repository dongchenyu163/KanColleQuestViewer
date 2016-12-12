#define _READ_FROM_FILE_

using System.Text;
using System.Windows;
using System.Windows.Input;
using MindFusion.Diagramming.Wpf;
using System.Collections.ObjectModel;
using System.IO;

namespace KanColleQuestViewer
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		public static string gs_url = @"https://zh.moegirl.org/%E8%88%B0%E9%98%9FCollection/%E4%BB%BB%E5%8A%A1#.E6.94.B9.E8.A3.85";
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

#if _READ_FROM_FILE_ == false
			gs_HTTP_Res = HTTP.GetHtmlContent(gs_url, Encoding.UTF8);//正常读取
#else
			//从文件读取
			FileStream _fS = File.OpenRead(gs_url_tmp);
			byte[] _tmp = new byte[_fS.Length];

			_fS.Read(_tmp, 0, (int)_fS.Length);
			gs_HTTP_Res = Encoding.UTF8.GetString(_tmp);
#endif
			KanColleQuest.UpdateQuests(gs_HTTP_Res);

			Collection<KanColleQuest> _res = new Collection<KanColleQuest>();
			_res=KanColleQuest.FindQuestChain("F36");

			Graph.DisplayALL(QuestGraph,_res);
			
		}

		private static void DisplayToListBox(Collection<KanColleQuest> Data)
		{

		}
	}
}
