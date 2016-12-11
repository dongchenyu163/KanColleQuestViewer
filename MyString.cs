using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanColleQuestViewer
{
	class MyString
	{
		/// <summary>
		/// 获取src中，在st ed字符串之间的字符串。
		/// </summary>
		/// <param name="src"></param>
		/// <param name="res"></param>
		/// <param name="st"></param>
		/// <param name="ed"></param>
		/// <returns></returns>
		public static bool GetMidString(string src, ref string res, string st, string ed, int startIndex = 0)
		{
			int _IndexSt = -1;
			int _IndexEd = -1;

			_IndexSt = src.IndexOf(st, startIndex);//寻找st在src的位置，返回st第一个字符的位置。
			if (_IndexSt == -1)
			{
				res = "";
				return false;
			}
			_IndexSt += st.Length;//开始位置变更至st的末尾
			_IndexEd = src.IndexOf(ed, _IndexSt);

			if (_IndexEd == -1)
			{
				res = "";
				return false;
			}

			res = src.Substring(_IndexSt, _IndexEd - _IndexSt);

			return true;
		}

		/// <summary>
		/// 删除具有开头结尾标志的字符串。
		/// 详细信息以及例子看实现。
		/// </summary>
		/// <param name="src">原始字符串</param>
		/// <param name="res">结果输出字符串</param>
		/// <param name="st">开头匹配字符串</param>
		/// <param name="ed">接尾匹配字符串</param>
		/// <param name="isDelStEd">是否同时删除匹配字符串</param>
		/// <returns>是否成功删除</returns>
		public static bool DelMidString(string src, ref string res, string st, string ed, bool isDelStEd)
		{
			///例1：原始字符串src："222###412424###1414"
			///		开头匹配字符串st："##"
			///		接尾匹配字符串ed："##"
			///		()内的为删除内容
			///		是否同时删除匹配字符串 为真的结果:222(###412424##)#1414
			///		是否同时删除匹配字符串 为假的结果:222##(#412424)###1414
			///例2：原始字符串src："222###412424###1414"
			///		开头匹配字符串st："##"
			///		接尾匹配字符串ed："#"
			///		()内的为删除内容
			///		是否同时删除匹配字符串 为真的结果:222(###)412424###1414
			///		是否同时删除匹配字符串 为假的结果:222##()#412424###1414	结果为空，返回删除失败。	
			///开头 接尾字符串寻找方式：
			///		原始字符串src："222###412424###1414"
			///		开头匹配字符串st："##"
			///		接尾匹配字符串ed："#"
			///		"222(##)_#412424###1414"
			///		()内为开始字符串匹配上的内容
			///		结尾字符串会在"_"下划线后开始寻找。
			///		——也就是说结尾字符串会在开始字符串结尾之后开始寻找。
			int _IndexSt = -1;
			int _IndexEd = -1;

			//输入参数检查
			if ((src=="")||(st == "") || (ed == ""))
				return false;

			//寻找关键字串
			_IndexSt = src.IndexOf(st, 0);//寻找st在src的位置，返回st第一个字符的位置。
			if (_IndexSt == -1)
				return false;
			if (isDelStEd == false)
			{
				_IndexSt += st.Length;                  //开始位置变更至st的末尾
				_IndexEd = src.IndexOf(ed, _IndexSt);
			}
			else
				_IndexEd = src.IndexOf(ed, _IndexSt + st.Length);

			//没找到则退出
			if (_IndexSt == -1 || _IndexEd == -1)
				return false;

			//执行删除
			if (isDelStEd == false)
				//String类的删除，参数是 起始位置 和 长度。长度可以为0
				res = src.Remove(_IndexSt, _IndexEd - _IndexSt);
			else
				res = src.Remove(_IndexSt, _IndexEd + ed.Length - _IndexSt);

			return true;
		}
	}
}
