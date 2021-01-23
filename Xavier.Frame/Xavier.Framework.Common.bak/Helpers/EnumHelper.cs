using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Xavier.Framework.Entity;

namespace Xavier.Framework.Common.Helpers
{
	public static class EnumHelper
	{
        /// <summary>
        /// ����KeyתValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
		public static string Key2Value<T>(int key)
		{
			Type typeFromHandle = typeof(T);
			return Enum.GetName(typeFromHandle, key);
		}

        /// <summary>
        /// Valueת����Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
		public static int Value2Key<T>(string value)
		{
			return (int)Enum.Parse(typeof(T), value);
		}

        /// <summary>
        /// ö��תValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
		public static string Item2Value<T>(T item)
		{
			return item.ToString();
		}

        /// <summary>
        /// ö��תDescription
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
		public static string Item2Description<T>(T item)
		{
			string result = string.Empty;
			FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public);
			FieldInfo[] array = fields;
			foreach (FieldInfo fieldInfo in array)
			{
				if (fieldInfo.Name.Equals(item.ToString()))
				{
					DescriptionAttribute customAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
					result = ((customAttribute != null) ? customAttribute.Description : string.Empty);
					break;
				}
			}
			return result;
		}

        /// <summary>
        /// ö��ת����Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
		public static int Item2Key<T>(T item)
		{
			return (int)Enum.Parse(typeof(T), item.ToString());
		}

        /// <summary>
        /// ValueתDescription
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
		public static string Value2Desc<T>(string value)
		{
			Type typeFromHandle = typeof(T);
			FieldInfo[] fields = typeFromHandle.GetFields(BindingFlags.Static | BindingFlags.Public);
			FieldInfo[] array = fields;
			foreach (FieldInfo fieldInfo in array)
			{
				string name = fieldInfo.Name;
				if (name.Equals(value))
				{
					DescriptionAttribute customAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
					if (customAttribute != null)
					{
						return customAttribute.Description;
					}
				}
			}
			return string.Empty;
		}

        /// <summary>
        /// KeyתDescription
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
		public static string Key2Desc<T>(int key)
		{
			Type typeFromHandle = typeof(T);
			FieldInfo[] fields = typeFromHandle.GetFields(BindingFlags.Static | BindingFlags.Public);
			string text = Key2Value<T>(key);
			FieldInfo[] array = fields;
			foreach (FieldInfo fieldInfo in array)
			{
				string name = fieldInfo.Name;
				if (text.ToString().Equals(name))
				{
					DescriptionAttribute customAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
					if (customAttribute != null)
					{
						return customAttribute.Description;
					}
				}
			}
			return string.Empty;
		}

        /// <summary>
        /// ö��ת�б�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
		public static List<KeyValue> GetList<T>()
		{
			List<KeyValue> list = new List<KeyValue>();
			FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public);
			FieldInfo[] array = fields;
			foreach (FieldInfo fieldInfo in array)
			{
				DescriptionAttribute customAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
				string text = string.Empty;
				if (customAttribute != null)
				{
					text = customAttribute.Description;
				}
				list.Add(new KeyValue
				{
					Key = Value2Key<T>(fieldInfo.Name.ToString()).ToString(),
					Value = fieldInfo.Name.ToString(),
					Text = text
				});
			}
			return list;
		}
	}
}
