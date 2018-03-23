using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathTools {
	/// <summary>
	/// 随机获取不重复的数字range 大范围， count 小范围
	/// </summary>
	public static int[] RandomNotSame(int range, int count){
		int[] index = new int[range]; 
		for (int i = 0; i < range; i++) index[i] = i; 
		System.Random r = new System.Random(); 
		int[] result = new int[count]; 
		int site = range;//设置上限 
		int id; 
		for (int j = 0; j < count; j++) 
		{ 
			//id = r.Next(1, site - 1); 
			id = r.Next(0, site); 
			//在随机位置取出一个数，保存到结果数组 
			result[j] = index[id]; 
			//最后一个数复制到当前位置 
			index[id] = index[site - 1]; 
			//位置的上限减少一 
			site--; 
		}
		return result;
	}

	/// <summary>
	/// 格式化长数字
	/// </summary>
	public static string FormatNumber(ulong num)
	{
		if (num >= 100000)
		{
			return (num / 10000).ToString() + "万";
		}
		return num.ToString();
	}
}
