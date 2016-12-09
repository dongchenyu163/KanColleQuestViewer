using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml.Linq;

namespace KanColleQuestViewer
{
	class XML
	{
		public static void SaveQuestsClass_To_XML(Collection<KanColleQuest> _Qs)
		{
			XDocument _xDoc = new XDocument();

			//任务根节点
			XElement _Quests = new XElement("QUESTS");
			//任务链接根节点
			XElement _QuestLink = new XElement("QUEST_LINK");

			int _count = 1;
			foreach (var _Q in _Qs)
			{
				//单个任务节点
				XElement _Quest =
					new XElement("Quest",
						new XElement("Num", _count),
						new XElement("ID", _Q.ID),
						new XElement("Name", _Q.NameOfQuest),
						new XElement("Detail", _Q.DetailOfQuest),
						new XElement("Rewards",
							new XElement("Fuel", _Q.RewardOfQuest.Fuel),
							new XElement("Bullet", _Q.RewardOfQuest.Bullet),
							new XElement("Steel", _Q.RewardOfQuest.Steel),
							new XElement("Bauxite", _Q.RewardOfQuest.Bauxite),
							new XElement("OtherRewards", _Q.RewardOfQuest.OtherRewards)
							),
						new XElement("OtherInfo", _Q.OtherInfoOfQuest)
						);
				_Quests.Add(_Quest);
				_count++;
			}
			_xDoc.Add(_Quests);
			_xDoc.Save("Test.xml");
			
		}

		public static Collection<KanColleQuest> ReadXML_To_QuestsClass()
		{
			XDocument _xDoc = XDocument.Load("Test.xml");

			Collection<KanColleQuest> _Q = new Collection<KanColleQuest>();

			var _xTmp = _xDoc.Element("QUESTS").Elements();

			int _count = 0;
			foreach (var node in _xTmp)
			{
				//var a= node.Element("ID").Value;
				//var b= node.Element("Name").Value;
				//var c=node.Element("Detail").Value;

				//var e=int.Parse((node.Element("Rewards").Element("Fuel").Value));
				//var f=int.Parse(node.Element("Rewards").Element("Bullet").Value);
				//var g=int.Parse(node.Element("Rewards").Element("Steel").Value);
				//var h=int.Parse(node.Element("Rewards").Element("Bauxite").Value);
				//var i=node.Element("Rewards").Element("OtherRewards").Value;

				//var j=node.Element("OtherInfo").Value;
				KanColleQuest _tmp_KNQ =
					new KanColleQuest(
						node.Element("ID").Value,
						node.Element("Name").Value,
						node.Element("Detail").Value,
						new Rewards(
							int.Parse((node.Element("Rewards").Element("Fuel").Value)),
							int.Parse(node.Element("Rewards").Element("Bullet").Value),
							int.Parse(node.Element("Rewards").Element("Steel").Value),
							int.Parse(node.Element("Rewards").Element("Bauxite").Value),
							node.Element("Rewards").Element("OtherRewards").Value
							),
						node.Element("OtherInfo").Value
						);
				_Q.Add(_tmp_KNQ);
				_count++;
			}

			return null;
		}

	}
}
