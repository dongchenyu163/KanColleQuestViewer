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

		public static string gs_Find_UpdateTime = ">此页面的内容及资料需要长期更新，现存条目中资料未必是最新。<li>本条目的<b>最后</b>更新时间为<b><u>";
		public static string gs_Find_UpdateTime_end = "</u>";
		string gs_Find_Quest = "<td id=\"";
		string gs_Find_Quest_end = "</tr>";

		public static string gs_UpdateTime = "";
		
		public MainWindow()
		{
			InitializeComponent();

			gs_HTTP_Res = HTTP.GetHtmlContent(gs_url,Encoding.UTF8);

			GetMidString(gs_HTTP_Res, ref gs_UpdateTime, gs_Find_Quest, gs_Find_Quest_end);
			Tx_Res.Text = gs_UpdateTime;

			HTML_StringProcess();
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
