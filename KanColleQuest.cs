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

		public LinkInfo LinkInfoOfQuest;

		private string _test;
		public string test {
			get { return _test; }
			set { _test = value; }
		}

		/// <summary>
		/// 构造函数
		/// </summary>
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
			LinkInfoOfQuest = new LinkInfo();
		}
		public KanColleQuest() { }

		#region 从HTTP中解析任务信息集 函数区域

		private static string gs_Find_UpdateTime = "><u>";
		private static string gs_Find_UpdateTime_end = "</u>";
		private static string gs_Find_Quest = "<td id=\"";
		private static string gs_Find_Quest_end = "</tr>";
		private static string gs_UpdateTime = "";
		public static bool UpdateQuests(string str_HTTP)
		{
			int _IndexSt = 0;
			int _IndexEd = 0;
			int _count = 0;
			string _quest = "";

			// 获取更新日期
			MyString.GetMidString(str_HTTP,
				ref gs_UpdateTime,
				gs_Find_UpdateTime,
				gs_Find_UpdateTime_end);
			#region 将更新时间字符串转化为DateTime形变量
			System.Globalization.CultureInfo zhCN = new System.Globalization.CultureInfo("zh-CN");
			//string utf8_string =	Encoding.Unicode.GetString(Encoding.Convert(
			//						Encoding.UTF8, 
			//						Encoding.Unicode, Encoding.UTF8.GetBytes(gs_UpdateTime)));
			//string sss = "2016年12月03日21:33";
			DateTime.TryParseExact(gs_UpdateTime, "yyyy年MM月dd日HH:mm（UTC+8）",
				zhCN,
				System.Globalization.DateTimeStyles.None,
				out KanColleQuest.UpdateTime);
			#endregion

			while (true)
			{
				_IndexSt = str_HTTP.IndexOf(gs_Find_Quest, 0);//寻找st在src的位置，返回st第一个字符的位置。
				_IndexSt += gs_Find_Quest.Length;//开始位置变更至末尾
				_IndexEd = str_HTTP.IndexOf(gs_Find_Quest_end, _IndexSt);

				if (_IndexSt == -1 || _IndexEd == -1)
				{
					break;
				}

				_count++;

				_quest = str_HTTP.Substring(_IndexSt, _IndexEd - _IndexSt);

				CreateStringToKanColleQuestClass(_quest);
				//删除前面的字符。
				str_HTTP = str_HTTP.Substring(_IndexEd);
			}

			#region 创建任务链
			MakeLink();
			#endregion

			#region 将OtherInfoOfQuest里面的标签去掉
			foreach (var item in Quests)
			{
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
					item.OtherInfoOfQuest = item.OtherInfoOfQuest.Replace(_Del[i], "");
				//删除超链接，直到没有为止。
				while (MyString.DelMidString(item.OtherInfoOfQuest, ref item.OtherInfoOfQuest, "<a href=\"", ">", true)) ;
				while (MyString.DelMidString(item.OtherInfoOfQuest, ref item.OtherInfoOfQuest, "<span class=\"heimu\" title=\"你知道的太多了\">", "</span>", true)) ;
				//由于删除“你知道的太多了”须要"</span>"来进行定位，故删除"</span>"放到最后执行。
				item.OtherInfoOfQuest = item.OtherInfoOfQuest.Replace("</span>", "");

				#endregion
				#endregion

				#region 2、再替换字符

				//寻找_Replace_Scr中的字符串，替换成_Replace_Res的对应字符串
				string[] _Replace_Scr = new string[] { "<br />", "<br>", "\n\n\n", "\n\n" };
				string[] _Replace_Res = new string[] { "\n", "\n", "\n", "\n" };
				for (int i = 0; i < _Replace_Scr.Length; i++)
					item.OtherInfoOfQuest = item.OtherInfoOfQuest.Replace(_Replace_Scr[i], _Replace_Res[i]);

				#endregion
			}
			#endregion
			return true;
		}
		#endregion

		#region 解析任务信息 函数区域

		static bool _isMulRow = false;
		static string _tmpMulRow_str = "";
		/// <summary>
		/// 从小段任务字串解析出任务信息，并加至Quests中
		/// </summary>
		/// <param name="quest_str"></param>
		public static void CreateStringToKanColleQuestClass(string quest_str)
		{
			string _split_1_st = "<td>", _split_1_ed = "</td>";
			string _split_2_st = "<span lang=\"ja\">", _split_2_ed = "</span>";
			int _IndexSt = 0;
			int _IndexEd = 0;

			#region 1、取出 ID  下一个"为止。
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
				_Detail = _Detail.Replace(_Del[i], "");
			//删除超链接，直到没有为止。
			while (MyString.DelMidString(_Detail, ref _Detail, "<a href=\"", ">", true)) ;
			while (MyString.DelMidString(_Detail, ref _Detail, "<span class=\"heimu\" title=\"你知道的太多了\">", "</span>", true)) ;
			//由于删除“你知道的太多了”须要"</span>"来进行定位，故删除"</span>"放到最后执行。
			_Detail = _Detail.Replace("</span>", "");

			#endregion
			#endregion

			#region 2、再替换字符

			//寻找_Replace_Scr中的字符串，替换成_Replace_Res的对应字符串
			string[] _Replace_Scr = new string[] { "<br />", "<br>", "\n\n\n", "\n\n" };
			string[] _Replace_Res = new string[] { "\n", "\n", "\n", "\n" };
			for (int i = 0; i < _Replace_Scr.Length; i++)
				_Detail = _Detail.Replace(_Replace_Scr[i], _Replace_Res[i]);

			#endregion
			#endregion

			#endregion

			#region 4、取出报酬字串
			_IndexSt = quest_str.IndexOf(_split_1_st, 0);
			_IndexSt += _split_1_st.Length;
			_IndexEd = quest_str.IndexOf(_split_1_ed, _IndexSt);

			int _Fu = 0;
			try
			{
				Convert.ToInt32(quest_str.Substring(_IndexSt, _IndexEd - _IndexSt));
			}
			catch (FormatException e)
			{
				_Fu = -1;
			}
			quest_str = quest_str.Substring(_IndexEd);
			/*****************************************************************/
			_IndexSt = quest_str.IndexOf(_split_1_st, 0);
			_IndexSt += _split_1_st.Length;
			_IndexEd = quest_str.IndexOf(_split_1_ed, _IndexSt);

			int _Bu = 0;
			try
			{
				Convert.ToInt32(quest_str.Substring(_IndexSt, _IndexEd - _IndexSt));
			}
			catch (FormatException e)
			{
				_Bu = -1;
			}
			quest_str = quest_str.Substring(_IndexEd);
			/*****************************************************************/
			_IndexSt = quest_str.IndexOf(_split_1_st, 0);
			_IndexSt += _split_1_st.Length;
			_IndexEd = quest_str.IndexOf(_split_1_ed, _IndexSt);

			int _Steel = 0;
			try
			{
				Convert.ToInt32(quest_str.Substring(_IndexSt, _IndexEd - _IndexSt));
			}
			catch (FormatException e)
			{
				_Steel = -1;
			}
			quest_str = quest_str.Substring(_IndexEd);
			/*****************************************************************/
			_IndexSt = quest_str.IndexOf(_split_1_st, 0);
			_IndexSt += _split_1_st.Length;
			_IndexEd = quest_str.IndexOf(_split_1_ed, _IndexSt);

			int _Ba = 0;
			try
			{
				Convert.ToInt32(quest_str.Substring(_IndexSt, _IndexEd - _IndexSt));
			}
			catch (FormatException e)
			{
				_Ba = -1;
			}
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

			#region 额外的字符处理  移至bool MakeLink() 函数内部处理
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
		#endregion

		/// <summary>
		/// 生成任务链信息
		/// </summary>
		/// <returns></returns>
		private static bool MakeLink()
		{
			foreach (var item in Quests)
			{
				/// 任务类-额外信息原始字符串
				string _tmp = item.OtherInfoOfQuest;

				/// 连接信息集合
				Collection<Link> _links = new Collection<Link>();

				#region 1、字符处理

				#region 情况1：有指明具体任务的
				string _HrefFlag = "<a href=\"https://zh.moegirl.org/%E8%88%B0%E9%98%9FCollection/%E4%BB%BB%E5%8A%A1";
				_tmp = _tmp.Replace(_HrefFlag, "");

				string _res = "";
				int _tmpindex = 0;
				while (MyString.GetMidString(_tmp, ref _res, "#", "\"", _tmpindex))
				{
					_tmpindex = _tmp.IndexOf("#", _tmpindex) + 1;
					//ID的长度不可能大于10
					if (_res.Length < 10)
						_links.Add(new Link(_res, true));
					_res = "";
				}
				#endregion
				#region 情况2：具有 指向上一个任务的(似乎不需要这个情况)

				#endregion
				#region 情况3：纯文字说明
				if (_links.Count == 0)
				{
					//TODO:纯文字实现
				}
				#endregion
				#endregion

				#region 2、不确定项的处理

				string[] _UnSures = new string[] { "要確認","要确认", "待確認", "需要驗證",
					"可能含有其他前置","達成後?","待确认","待验证", "达成后?" };
				#region 找到不确定字符串
				List<int> _UnSure_res = new List<int>();
				foreach (var _UnSure in _UnSures)
				{
					for (int _index = 0; ;)
					{
						_index = _tmp.IndexOf(_UnSure, _index);
						if (_index == -1)
							break;
						else
						{
							_UnSure_res.Add(_index);
							_index += _UnSure.Length;
						}
					}
				}
				//默认排序是升序排序
				//降序排序：_UnSure_res.Sort((x, y) => -x.CompareTo(y));
				_UnSure_res.Sort();
				#endregion

				if (_UnSure_res.Count == 1 && _links.Count == 1)
				{
					#region 单个前置任务的情况特殊处理
					_links[0].isBelivable = false;
					#endregion
				}
				else if (_UnSure_res.Count == 0) ;
				else
				{
					#region 多个前置任务的情况

					#region 找到任务ID（超链接）字符串

					List<int> _LinkIndex_res = new List<int>();
					foreach (var _link in _links)
					{
						for (int _index = 0; ;)
						{
							_index = _tmp.IndexOf("#" + _link.ID, _index);
							if (_index == -1)
								break;
							else
							{
								_LinkIndex_res.Add(_index);
								_index += ("#" + _link.ID).Length;
							}
						}
					}
					_LinkIndex_res.Sort();
					#endregion

					#region 通过 不确定 任务 两个位置List 来进行不确定连接的判断
					int _count_un = 0;
					int _count_linkindex = 0;
					foreach (var _UnSure_res_item in _UnSure_res)
					{
						for (; _count_linkindex < _LinkIndex_res.Count;)
						{
							if (_LinkIndex_res[_count_linkindex] > _UnSure_res_item)
							{
								#region 不确定字串在开头，则全部链接都不确定。
								if (_count_linkindex == 0)
								{
									foreach (var _link in _links)
										_link.isBelivable = false;
									_count_linkindex = 0;
									break;
								}
								#endregion
								#region 剩下的，不确定字串向前结合
								else
								{
									_links[_count_linkindex - 1].isBelivable = false;
								}
								#endregion
								_count_linkindex++;
								break;
							}
							else
								_count_linkindex++;
						}
						if (_count_linkindex == 0)
							break;
						_count_un++;
					}
					#endregion

					#endregion
				}

				#endregion

				#region 3、生成链接
				foreach (var _link in _links)
				{
					//设置当前任务的前置任务
					item.LinkInfoOfQuest.preLinkInfo.Add(_link);
					//设置当前任务的前置任务的后置任务
					KanColleQuest hehe = FindByID(_link.ID);
					hehe.LinkInfoOfQuest.nextLinkInfo.Add(new Link(item.ID, _link.isBelivable));
				}
				#endregion
			}
			return true;
		}

		/// <summary>
		/// 以ID寻找任务。
		/// </summary>
		/// <param name="ID">任务ID字符串(不分大小写)</param>
		/// <returns>KanColleQuest类型引用，没找到则为null。</returns>
		public static KanColleQuest FindByID(string ID)
		{
			foreach (var item in Quests)
			{
				if (ID.ToUpper() == item.ID.ToUpper())
					return item;
			}
			return null;
		}

		/// <summary>
		/// 以关键字寻找对应的所有任务
		/// </summary>
		/// <param name="KeyWord"></param>
		/// <returns></returns>
		public static Collection<KanColleQuest> FindByKeyWord(string KeyWord)
		{
			Collection<KanColleQuest> _res = new Collection<KanColleQuest>();
			foreach (var item in Quests)
			{
				if (KeyWord.ToUpper() == item.ID.ToUpper())
					_res.Add(item);
				else if (item.NameOfQuest.IndexOf(KeyWord) != -1)
					_res.Add(item);
				else if (item.DetailOfQuest.IndexOf(KeyWord) != -1)
					_res.Add(item);
				else if (item.RewardOfQuest.OtherRewards.IndexOf(KeyWord) != -1)
					_res.Add(item);
				else if (item.OtherInfoOfQuest.IndexOf(KeyWord) != -1)
					_res.Add(item);
				else
					continue;
			}
			return _res;
		}

		/// <summary>
		/// 递归寻找前置任务链
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public static Collection<KanColleQuest> FindQuestChain(string ID)
		{
			Collection<KanColleQuest> _res = new Collection<KanColleQuest>();
			if (FindByID(ID) == null)
				return null;
			else if (FindByID(ID).LinkInfoOfQuest.preLinkInfo.Count == 0)
			{
				_res.Add(FindByID(ID));
				return _res;
			}
			else if (FindByID(ID).LinkInfoOfQuest.preLinkInfo.Count != 0)
			{
				Collection<KanColleQuest> _tmp_res = new Collection<KanColleQuest>();
				foreach (var item in FindByID(ID).LinkInfoOfQuest.preLinkInfo)
				{
					_tmp_res = FindQuestChain(item.ID);
					_res = Union(_res, _tmp_res);
				}
				_res.Add(FindByID(ID));
				return _res;
			}
			else
			{
				return null;//以上的三个情况应该包含了全部。我也不知道这个是什么情况
			}
		}

		/// <summary>
		/// 合并两个Collection对象。
		/// 以src为基础，把adder里面的元素加至src里面。
		/// </summary>
		/// <param name="src"></param>
		/// <param name="adder"></param>
		/// <param name="allowDuplicate">允许重复项；默认不允许</param>
		/// <returns></returns>
		private static Collection<KanColleQuest> Union(Collection<KanColleQuest> src,
			Collection<KanColleQuest> adder,
			bool allowDuplicate = false)
		{

			if (allowDuplicate == true)
				foreach (var item in adder)
					src.Add(item);
			else
				foreach (var item in adder)
				{
					if (src.Contains(item))
						continue;
					else
						src.Add(item);
				}
			return src;
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
		public bool isRoot;

		public LinkInfo()
		{
			preLinkInfo = new Collection<Link>();
			nextLinkInfo = new Collection<Link>();
			isRoot = false;
		}
	}

	class Link
	{
		public string ID;
		public bool isBelivable;

		public Link(string _ID, bool _isBelivable)
		{
			ID = _ID;
			isBelivable = _isBelivable;
		}
	}
}
