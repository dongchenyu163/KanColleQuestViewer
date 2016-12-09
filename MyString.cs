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

		public static bool DelMidString(string src, ref string res, string st, string ed, bool isDelStEd)
		{
			int _IndexSt = -1;
			int _IndexEd = -1;

			_IndexSt = src.IndexOf(st, 0);//寻找st在src的位置，返回st第一个字符的位置。
			if(isDelStEd==false)
			{
				_IndexSt += st.Length;					//开始位置变更至st的末尾
				_IndexEd = src.IndexOf(ed, _IndexSt);
			}
			else
			{
				_IndexEd = src.IndexOf(ed, _IndexSt+st.Length);
			}

			if (_IndexSt == -1 || _IndexEd == -1)
			{
				res = "";
				return false;
			}

			if (isDelStEd == false)
			{
				res = src.Remove(_IndexSt, _IndexEd - _IndexSt);
			}
			else
			{
				res = src.Remove(_IndexSt, _IndexEd + ed.Length - _IndexSt);
			}

			return true;
		}
	}
}
