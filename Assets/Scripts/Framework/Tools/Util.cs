using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text.RegularExpressions;

public class Util {
	/// 清理内存
	/// </summary>
	public static void ClearMemory()
	{
		GC.Collect();
		Resources.UnloadUnusedAssets();
	}

	/// <summary>
	/// 是否为数字
	/// </summary>
	public static bool IsNumber(string strNumber)
	{
		Regex regex = new Regex("[^0-9]");
		return !regex.IsMatch(strNumber);
	}

	/// <summary>
	/// 网络可用
	/// </summary>
	public static bool NetAvailable
	{
		get{
			return Application.internetReachability != NetworkReachability.NotReachable;
		}
	}

	/// <summary>
	/// 是否是无线
	/// </summary>
	public static bool IsWifi
	{
		get{
			return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
		}
	}

	/// <summary>
	/// 是否已经解压过了
	/// </summary>
	public static bool HasExtracted(){
		return false;
	}

	/// <summary>
	/// 计算字符串文字个数. 不计颜色串
	/// </summary>
	/// <param name="text"></param>
	/// <returns></returns>
	public static int CalculateStringLength(string text)
	{
		int length = 0;
		for (int i = 0; i < text.Length; )
		{
			if (text[i] == '<')
			{
				if (text.Substring(i, 8) == "<color=#")
				{
					i += 14;
					continue;
				}
				if (text.Substring(i, 8) == "</color>")
				{
					i += 8;
					continue;
				}
			}
			i++;
			length++;
		}
		return length;
	}
}


