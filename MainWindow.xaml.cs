using System;
using System.Collections.Generic;
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

namespace KanColleQuestViewer
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		public static string gs_url= @"https://zh.moegirl.org/%E8%88%B0%E9%98%9FCollection/%E4%BB%BB%E5%8A%A1#.E6.94.B9.E8.A3.85";
		public static string gs_HTTP_Res;

		public static string gs_Find_UpdateTime = "<b><u>";
		public static string gs_Find_UpdateTime_end = "</u>";
		string gs_Find_Quest = "<td id=\"";
		string gs_Find_Quest_end = "</tr>";

		public static string gs_UpdateTime = "";
		
		public MainWindow()
		{
			InitializeComponent();

			gs_HTTP_Res = HTTP.GetHtmlContent(gs_url,Encoding.UTF8);
			
			HTML_StringProcess();

			Tx_Res.Text = "";

			for (int i = 0; i < KanColleQuest.Quests.Count; i++)
			{
				Tx_Res.Text += i.ToString()+
					"["+ KanColleQuest.Quests[i].ID+ "]" + ".\t" + 
					KanColleQuest.Quests[i].OtherInfoOfQuest + "\n";
			}
		}

		/// <summary>
		/// 获取src中，在st ed字符串之间的字符串。
		/// </summary>
		/// <param name="src"></param>
		/// <param name="res"></param>
		/// <param name="st"></param>
		/// <param name="ed"></param>
		/// <returns></returns>
		private bool GetMidString(string src,ref string res,string st,string ed,int startIndex=0)
		{
			int _IndexSt = -1;
			int _IndexEd = -1;

			_IndexSt = src.IndexOf(st, startIndex);//寻找st在src的位置，返回st第一个字符的位置。
			_IndexSt += st.Length;//开始位置变更至st的末尾
			_IndexEd = src.IndexOf(ed, _IndexSt);

			if (_IndexSt == -1 || _IndexEd == -1)
			{
				res = "";
				return false;
			}
			
			res = src.Substring(_IndexSt, _IndexEd - _IndexSt);

			return true;
		}

		private void HTML_StringProcess()
		{
			int _IndexSt = 0;
			int _IndexEd = 0;
			int _count = 0;
			string _quest = "";
			string _copyOf_gs_HTTP_Res = gs_HTTP_Res;
			
			// 获取更新日期
			GetMidString(_copyOf_gs_HTTP_Res, 
				ref gs_UpdateTime, 
				gs_Find_UpdateTime, 
				gs_Find_UpdateTime_end);
			#region 将更新时间字符串转化为DateTime形变量
			System.Globalization.CultureInfo zhCN = new System.Globalization.CultureInfo("zh-CN");
			//TODO:编码转换
			//string s = "这里是测试字符串";Unicode UTF8
			//string utf8_string = Encoding.UTF8.GetString(Encoding.Default.GetBytes(s));
			
			string utf8_string = Encoding.Unicode.GetString(Encoding.Convert(Encoding.UTF8 ,Encoding.Unicode,Encoding.UTF8.GetBytes(gs_UpdateTime)));
			DateTime.TryParseExact(utf8_string, "yyyy年MM月dd日hh:mm", 
				zhCN, 
				System.Globalization.DateTimeStyles.None,
				out KanColleQuest.UpdateTime);
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
