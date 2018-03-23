/// <summary>
/// 打包通用工具
/// </summary>
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using System;

public class BundleCommonTools : Editor {

	//[MenuItem("武当剑工具/资源打包/通用工具/重置美术资源路径", false, 2)]
	public static void CreateDefaultAssetBundle(){
		foreach (string path in EditorConfig.SourcePaths) {
			if (!Directory.Exists (EditorConfig.ABSourceRoot + path +"/")) {
				Directory.CreateDirectory (path);
			}	
		}
		AssetDatabase.Refresh();
	}

	//[MenuItem("武当剑工具/资源打包/拷贝/ 一键拷贝 到开发环境(程序编程环境)", false, 1)]
	public static void CopyResourceToDevEnvirment(){
		//拷贝	AB 目录
		CopyABResourceToDevEnvirment();
		//生成MD5码并拷贝到开发目录
		MakeResourcesMd5FilesAndCopy();
	}

	//[MenuItem("武当剑工具/资源打包/拷贝/ Assetbundle 到开发环境(程序编程环境)", false, 3)]
	public static void CopyABResourceToDevEnvirment(){
		foreach (string folder in EditorConfig.FolderNames) {
			string DestPath = EditorConfig.DevProjectDirRoot + "/StreamingAssets/"+EditorConfig.GetoutputPath(bFolderOnly: true)+"/"+folder+"/";
			string ResPath = EditorConfig.ABExportPath+EditorConfig.GetoutputPath(bFolderOnly: true) +"/"+ folder;
			EditorConfig.CopyDir(ResPath, DestPath);
		}
		CopyManifestToDevEnvirment();
	}

	//拷贝manifest文件到指定的目录中
	public static void CopyManifestToDevEnvirment(){
		string DestPath = EditorConfig.DevProjectDirRoot + "/StreamingAssets/"+EditorConfig.GetoutputPath(bFolderOnly: true)+"/"+EditorConfig.GetoutputPath(bFolderOnly: true);
		File.Copy(EditorConfig.GetoutputPath()+"/"+EditorConfig.GetoutputPath(bFolderOnly: true), DestPath, true);

		DestPath = EditorConfig.DevProjectDirRoot + "/StreamingAssets/"+EditorConfig.GetoutputPath(bFolderOnly: true)+"/"+EditorConfig.GetoutputPath(bFolderOnly: true)+".manifest";
		File.Copy(EditorConfig.GetoutputPath()+"/"+EditorConfig.GetoutputPath(bFolderOnly: true)+".manifest", DestPath, true);
	}

	/// <summary>
	/// 创建文件的MD5 码
	/// </summary>
	static List<string> paths = new List<string>();
	static List<string> files = new List<string>();
	//[MenuItem("武当剑工具/资源打包/生成MD5", false, 2)]
	public static void MakeResourcesMd5FilesAndCopy(bool bCopy = true){
		paths.Clear(); files.Clear();

		string resPath = EditorConfig.GetoutputPath();
		string newFilePath = resPath + "/files.txt";
		if (File.Exists(newFilePath)) File.Delete(newFilePath);

		paths.Clear(); files.Clear();
		Recursive(resPath);

		FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
		StreamWriter sw = new StreamWriter(fs);
		for (int i = 0; i < files.Count; i++) {
			string file = files[i];
			string ext = Path.GetExtension(file);
			if (file.EndsWith(".meta") || file.Contains(".DS_Store")) continue;

			string md5 = md5file(file);
			string value = file.Replace(resPath, string.Empty);
			sw.WriteLine(value + "|" + md5);
		}
		sw.Close(); fs.Close();
		AssetDatabase.Refresh();

		if (bCopy) {
			string DestPath = EditorConfig.DevProjectDirRoot + "/StreamingAssets/"+EditorConfig.GetoutputPath(bFolderOnly: true)+"/files.txt";
			File.Copy(EditorConfig.GetoutputPath()+"files.txt", DestPath, true);
		}
	}

	/// <summary>
	/// 遍历目录和子目录
	/// </summary>
	static void Recursive(string path){
		string[] names = Directory.GetFiles(path);
		string[] dirs = Directory.GetDirectories(path);
		foreach(string filename in names){
			string ext = Path.GetExtension(filename);
			if(ext.Equals(".meta"))continue;
			files.Add(filename.Replace('\\', '/'));
		}

		foreach(string dir in dirs){
			paths.Add(dir.Replace('\\', '/'));
			Recursive(dir);
		}
	}


	/// <summary>
	/// 计算字符串的MD5值
	/// </summary>
	public static string md5(string source) {
		MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
		byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
		byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
		md5.Clear();

		string destString = "";
		for (int i = 0; i < md5Data.Length; i++) {
			destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
		}
		destString = destString.PadLeft(32, '0');
		return destString;
	}

	/// <summary>
	/// 计算文件的MD5值
	/// </summary>
	public static string md5file(string file) {
		try {
			FileStream fs = new FileStream(file, FileMode.Open);
			System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] retVal = md5.ComputeHash(fs);
			fs.Close();

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < retVal.Length; i++) {
				sb.Append(retVal[i].ToString("x2"));
			}
			return sb.ToString();
		} catch (Exception ex) {
			throw new Exception("md5file() fail, error:" + ex.Message);
		}
	}

}
