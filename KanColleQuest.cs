using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanColleQuestViewer
{
	class KanColleQuest
	{
		public static Collection<KanColleQuest> Quests = new Collection<KanColleQuest>();
		public static DateTime UpdateTime = new DateTime();
		/// <summary>
		/// 任务编号
		/// </summary>
		public string ID;
		/// <summary>
		/// 任务名称
		/// </summary>
		public string NameOfQuest;
		/// <summary>
		/// 任务的详细信息
		/// </summary>
		public string DetailOfQuest;
		/// <summary>
		/// 获得的奖励
		/// </summary>
		public Rewards RewardOfQuest;
		/// <summary>
		/// 任务的追加信息
		/// </summary>
		public string OtherInfoOfQuest;

		/// <summary>
		/// 开放条件是否确定
		/// </summary>
		public bool isSure = true;

		///// <summary>
		///// 前置任务
		///// </summary>
		//Collection<KanColleQuest> preQuests;
		///// <summary>
		///// 后续会打开的任务
		///// </summary>
		//Collection<KanColleQuest> nextQuests;

		public KanColleQuest(string ID,
					string NameOfQuest,
					string DetailOfQuest,
					Rewards Rewards,
					string OtherInfoOfQuest)
		{
			this.ID = ID;
			this.NameOfQuest = NameOfQuest;
			this.DetailOfQuest = DetailOfQuest;
			this.RewardOfQuest = Rewards;
			this.OtherInfoOfQuest = OtherInfoOfQuest;
		}

		//
		static bool _isMulRow = false;
		static string _tmpMulRow_str = "";
		public static void CreateStringProcess(string quest_str)
		{
			string _split_1_st = "<td>", _split_1_ed = "</td>";
			string _split_2_st = "<span lang=\"ja\">", _split_2_ed = "</span>";
			int _IndexSt = 0;
			int _IndexEd = 0;

			#region 1、取出 ID
			_IndexEd = quest_str.IndexOf("\"", 0);
			string _ID = quest_str.Substring(0, _IndexEd);
			quest_str = quest_str.Substring(_IndexEd);

			#endregion

			#region 2、取出 任务名称
			_IndexSt = quest_str.IndexOf(_split_2_st, 0);//寻找st在src的位置，返回st第一个字符的位置。
			_IndexSt += _split_2_st.Length;//开始位置变更至末尾
			_IndexEd = quest_str.IndexOf(_split_2_ed, _IndexSt);

			string _Name = quest_str.Substring(_IndexSt, _IndexEd - _IndexSt);
			quest_str = quest_str.Substring(_IndexEd);

			#endregion

			#region 3、取出 任务的详细信息。需要额外的字符处理
			//取出原始 详细信息 字串
			_IndexSt = quest_str.IndexOf(_split_1_st, 0);
			_IndexSt += _split_1_st.Length;
			_IndexEd = quest_str.IndexOf(_split_1_ed, _IndexSt);

			string _Detail = quest_str.Substring(_IndexSt, _IndexEd - _IndexSt);
			quest_str = quest_str.Substring(_IndexEd);
			#region 额外的字符处理
			//TODO: 详细信息 字串 额外的字符处理

			#region 1、先删除字符

			string[] _Del = new string[]{   "<span lang=\"ja\">" ,
											"<span style=\"color:blue;\">",
											"<span style=\"color:pink;\">",
											"<span style=\"color:red;\">",
											"<span style=\"color:red; \">",
											"<span style=\"color:orange;\">",
											"<span style=\"color:green;\">",
											"&lt;1&gt;",
											"</a>",
											"<b>",
											"</b>",
											"\n<p>任務達成的注意事項見表格下方\n</p>",
											"<p>",
											"</p>"
										};
			#region 删除功能代码
			for (int i = 0; i < _Del.Length; i++)
				_Detail=_Detail.Replace(_Del[i], "");
			//删除超链接，直到没有为止。
			while (MyString.DelMidString(_Detail, ref _Detail, "<a href=\"", ">", true)) ;
			while (MyString.DelMidString(_Detail, ref _Detail, "<span class=\"heimu\" title=\"你知道的太多了\">", "</span>", true)) ;
			//由于删除“你知道的太多了”须要"</span>"来进行定位，故删除"</span>"放到最后执行。
			_Detail = _Detail.Replace("</span>", "");
			
			#endregion
			#endregion

			#region 2、再替换字符

			//寻找_Replace_Scr中的字符串，替换成_Replace_Res的对应字符串
			string[] _Replace_Scr = new string[] { "<br />", "<br>","\n\n\n","\n\n" };
			string[] _Replace_Res = new string[] { "\n","\n","\n","\n" };
			for (int i = 0; i < _Replace_Scr.Length; i++)
				_Detail=_Detail.Replace(_Replace_Scr[i], _Replace_Res[i]);

			#endregion
			#endregion
			#endregion

			#region 4、取出报酬字串
			_IndexSt = quest_str.IndexOf(_split_1_st, 0);
			_IndexSt += _split_1_st.Length;
			_IndexEd = quest_str.IndexOf(_split_1_ed, _IndexSt);

			int _Fu = Convert.ToInt32(quest_str.Substring(_IndexSt, _IndexEd - _IndexSt));
			quest_str = quest_str.Substring(_IndexEd);
			/*****************************************************************/
			_IndexSt = quest_str.IndexOf(_split_1_st, 0);
			_IndexSt += _split_1_st.Length;
			_IndexEd = quest_str.IndexOf(_split_1_ed, _IndexSt);

			int _Bu = Convert.ToInt32(quest_str.Substring(_IndexSt, _IndexEd - _IndexSt));
			quest_str = quest_str.Substring(_IndexEd);
			/*****************************************************************/
			_IndexSt = quest_str.IndexOf(_split_1_st, 0);
			_IndexSt += _split_1_st.Length;
			_IndexEd = quest_str.IndexOf(_split_1_ed, _IndexSt);

			int _Steel = Convert.ToInt32(quest_str.Substring(_IndexSt, _IndexEd - _IndexSt));
			quest_str = quest_str.Substring(_IndexEd);
			/*****************************************************************/
			_IndexSt = quest_str.IndexOf(_split_1_st, 0);
			_IndexSt += _split_1_st.Length;
			_IndexEd = quest_str.IndexOf(_split_1_ed, _IndexSt);

			int _Ba = Convert.ToInt32(quest_str.Substring(_IndexSt, _IndexEd - _IndexSt));
			quest_str = quest_str.Substring(_IndexEd);
			/*****************************************************************/
			_IndexSt = quest_str.IndexOf(_split_1_st, 0);
			_IndexSt += _split_1_st.Length;
			_IndexEd = quest_str.IndexOf(_split_1_ed, _IndexSt);

			string _OtherReward = quest_str.Substring(_IndexSt, _IndexEd - _IndexSt);
			quest_str = quest_str.Substring(_IndexEd);
			#endregion

			#region 5、取出额外信息：任务链信息。需要额外的字符处理
			//取出原始 额外信息 字串
			string _Find_MulRow = "<td rowspan=\"2\">";//Find "<td rowspan="2">"
			string _OtherInfo = "";

			/// 取出原始 额外信息 字串；并处理被合并的单元格；被标/*ST*/的是标准流程步骤
			/// 剩下的是处理合并单元格所用的语句。
			/// 处理合并单元格所用的全局变量：_isMulRow；_tmpMulRow_str。
			/// <summary>
			/// 识别;
			/// 置位；
			/// 保存；
			/// 读标志位；
			/// 取数据；
			/// 清除标志位
			/// </summary>
			if (_isMulRow == false)
			{
				_IndexSt = quest_str.IndexOf(_Find_MulRow, 0);

				//没找到 合并单元格 则走正常流程
				if (_IndexSt == -1)
				{
					_IndexSt = quest_str.IndexOf(_split_1_st, 0);/*ST*/
					_IndexSt += _split_1_st.Length;/*ST*/
					_isMulRow = false;
				}
				else
				{
					_IndexSt += _Find_MulRow.Length;
					_isMulRow = true;
				}

				_IndexEd = quest_str.IndexOf(_split_1_ed, _IndexSt);/*ST*/

				_OtherInfo = quest_str.Substring(_IndexSt, _IndexEd - _IndexSt);/*ST*/
				if (_isMulRow == true)
					_tmpMulRow_str = _OtherInfo;
			}
			else
			{
				_OtherInfo = _tmpMulRow_str;
				_tmpMulRow_str = "";
				_isMulRow = false;
			}
			//quest_str = quest_str.Substring(_IndexEd);/*ST*/
			#region 额外的字符处理
			//TODO: 额外信息 字串 额外的字符处理
			#endregion
			#endregion

			//添加新的任务
			Quests.Add(new KanColleQuest(
										_ID,
										_Name,
										_Detail,
										new Rewards(_Fu, _Bu, _Steel, _Ba, _OtherReward),
										_OtherInfo));

		}

		/// <summary>
		/// 以ID寻找任务。
		/// </summary>
		/// <param name="ID">任务ID字符串(不分大小写)</param>
		/// <returns>KanColleQuest类型引用，没找到则为null。</returns>
		public static KanColleQuest Find(string ID)
		{
			return null;
		}
	}

	class QuestID
	{
		/// <summary>
		/// 任务类型的字母代号
		/// </summary>
		string Alphabet = "A";
		/// <summary>
		/// 任务序号
		/// </summary>
		int Num = 0;

		QuestID(string data)
		{

		}
	}

	class Rewards
	{
		public int Fuel;
		public int Bullet;
		public int Steel;
		public int Bauxite;
		/// <summary>
		/// 其他的报酬
		/// </summary>
		public string OtherRewards;

		public Rewards(int Fu, int Bu, int Steel, int Ba, string Other)
		{
			Fuel = Fu;
			Bullet = Bu;
			this.Steel = Steel;
			Bauxite = Ba;
			OtherRewards = Other;
		}
	}

	class LinkInfo
	{
		public Collection<Link> preLinkInfo;
		public Collection<Link> nextLinkInfo;
	}

	class Link
	{
		public string ID;
		public bool isBelivable;

		public Link(string _ID,bool _isBelivable)
		{
			ID = _ID;
			isBelivable = _isBelivable;
		}
	}
}
