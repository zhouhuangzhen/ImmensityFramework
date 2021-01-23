using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Xavier.Framework.Db.Commons
{
	public class ExtList<T> : IEqualityComparer<T> where T : class, new()
	{
		private string[] comparintFiledName = new string[0];

		public ExtList()
		{
		}

		public ExtList(params string[] comparintFiledName)
		{
			this.comparintFiledName = comparintFiledName;
		}

		bool IEqualityComparer<T>.Equals(T x, T y)
		{
			if (x == null && y == null)
			{
				return false;
			}
			if (comparintFiledName.Length == 0)
			{
				return x.Equals(y);
			}
			bool flag = true;
			Type type = x.GetType();
			Type type2 = y.GetType();
			string[] array = comparintFiledName;
			foreach (string filedName in array)
			{
				PropertyInfo propertyInfo = (from p in type.GetProperties()
					where p.Name.Equals(filedName)
					select p).FirstOrDefault();
				PropertyInfo propertyInfo2 = (from p in type2.GetProperties()
					where p.Name.Equals(filedName)
					select p).FirstOrDefault();
				flag = (flag && propertyInfo != null && propertyInfo2 != null && propertyInfo.GetValue(x, null).ToString().Equals(propertyInfo2.GetValue(y, null)));
			}
			return flag;
		}

		int IEqualityComparer<T>.GetHashCode(T obj)
		{
			return obj.ToString().GetHashCode();
		}
	}
}
