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

		private static Collection<KanColleQuest> gKCQ_Display;

		public MainWindow()
		{
			InitializeComponent();

			gKCQ_Display = KanColleQuest.Quests;

			#region 事件处理函数关联 区域
			LB_Result.MouseDoubleClick += LB_Result_MouseDoubleClick;
			BT_Search.Click += BT_Search_Click;
			BT_DisplayChain.Click += BT_DisplayChain_Click;
			#endregion

			#region 获取网页HTML代码

#if _READ_FROM_FILE_ == false
			gs_HTTP_Res = HTTP.GetHtmlContent(gs_url, Encoding.UTF8);//正常读取
#else
			//从文件读取
			FileStream _fS = File.OpenRead(gs_url_tmp);
			byte[] _tmp = new byte[_fS.Length];

			_fS.Read(_tmp, 0, (int)_fS.Length);
			gs_HTTP_Res = Encoding.UTF8.GetString(_tmp);
#endif
			#endregion

			KanColleQuest.UpdateQuests(gs_HTTP_Res);
			
			Graph.DisplayALL(QuestGraph, KanColleQuest.FindQuestChain("F36"));

			DisplayToListBox(LB_Result, gKCQ_Display);
		}


		private void DisplayToListBox(System.Windows.Controls.ListBox listbox,Collection<KanColleQuest> Data)
		{
			listbox.Items.Clear();
			foreach (var item in Data)
			{
				listbox.Items.Add(item.ID+"\n"+item.NameOfQuest);
			}
		}

		/// <summary>
		/// 显示一个任务信息到对应文本框上。
		/// </summary>
		/// <param name="Data"></param>
		private void DisplayTheQuestToTextBox(KanColleQuest Data)
		{
			TX_ID.Text = Data.ID;
			TX_Name.Text = Data.NameOfQuest;
			TX_Rewards.Text =	"燃油:"+Data.RewardOfQuest.Fuel.ToString()+"\t"+
								"弹药:" + Data.RewardOfQuest.Bullet.ToString() + "\n"+
								"钢铁:" + Data.RewardOfQuest.Steel.ToString() + "\t"+
								"铝土:" + Data.RewardOfQuest.Bauxite.ToString() + "\n"+
								Data.RewardOfQuest.OtherRewards;
			TX_Detial.Text = Data.DetailOfQuest;
			TX_Other.Text = Data.OtherInfoOfQuest;
		}

		#region 事件处理函数 区域
		/// <summary>
		/// 双击ListBox，在5个文本框显示任务的详细信息。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LB_Result_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			System.Windows.Controls.ListBox _Sender = (System.Windows.Controls.ListBox)sender;
			DisplayTheQuestToTextBox(
				KanColleQuest.FindByID(
					((gKCQ_Display[_Sender.SelectedIndex]).ID)));
		}

		private void BT_Search_Click(object sender, RoutedEventArgs e)
		{
			Collection<KanColleQuest> _res = KanColleQuest.FindByKeyWord(TX_KeyWord.Text);
			gKCQ_Display = _res;
			if (_res.Count == 0)
				return;
			else
				DisplayToListBox(LB_Result, gKCQ_Display);
		}
		private void BT_DisplayChain_Click(object sender, RoutedEventArgs e)
		{
			Graph.DisplayALL(QuestGraph, KanColleQuest.FindQuestChain((gKCQ_Display[LB_Result.SelectedIndex]).ID));
		}

		#endregion
	}
}
