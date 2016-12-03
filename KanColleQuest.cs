using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanColleQuestViewer
{
	class KanColleQuest
	{
		/// <summary>
		/// 任务编号
		/// </summary>
		QuestID ID;
		/// <summary>
		/// 任务名称
		/// </summary>
		string NameOfQuest;
		/// <summary>
		/// 任务的详细信息
		/// </summary>
		string DetailOfQuest;
		/// <summary>
		/// 获得的奖励
		/// </summary>
		Rewards RewardOfQuest;
		/// <summary>
		/// 任务的追加信息
		/// </summary>
		string OtherInfoOfQuest;

		/// <summary>
		/// 开放条件是否确定
		/// </summary>
		bool isSure = true;
		/// <summary>
		/// 前置任务
		/// </summary>
		KanColleQuest[] preQuests;
		/// <summary>
		/// 后续会打开的任务
		/// </summary>
		KanColleQuest[] nextQuests;
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
	}

	class Rewards
	{
		int Fuel = 0;
		int Bullet = 0;
		int Steel = 0;
		int Bauxite = 0;
		/// <summary>
		/// 其他的报酬
		/// </summary>
		string OtherRewards = "";
	}
}
