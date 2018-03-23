using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;

public class MD5Tools{
	/// <summary>
	/// HashToMD5Hex
	/// </summary>
	public static string HashToMD5Hex(string sourceStr)
	{
		byte[] Bytes = Encoding.UTF8.GetBytes(sourceStr);
		using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
		{
			byte[] result = md5.ComputeHash(Bytes);
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < result.Length; i++)
				builder.Append(result[i].ToString("x2"));
			return builder.ToString();
		}
	}

	/// <summary>
	/// 计算字符串的MD5值
	/// </summary>
	public static string md5(string source)
	{
		MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
		byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
		byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
		md5.Clear();

		string destString = "";
		for (int i = 0; i < md5Data.Length; i++)
		{
			destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
		}
		destString = destString.PadLeft(32, '0');
		return destString;
	}

	/// <summary>
	/// 计算文件的MD5值
	/// </summary>
	public static string md5file(string file)
	{
		try
		{
			FileStream fs = new FileStream(file, FileMode.Open);
			System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] retVal = md5.ComputeHash(fs);
			fs.Close();

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < retVal.Length; i++)
			{
				sb.Append(retVal[i].ToString("x2"));
			}
			return sb.ToString();
		}
		catch (Exception ex)
		{
			throw new Exception("md5file() fail, error:" + ex.Message);
		}
	}

}
