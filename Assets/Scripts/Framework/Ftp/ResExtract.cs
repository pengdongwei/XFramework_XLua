using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
/// <summary>
/// 资源的解压
/// </summary>
public class ResExtract : MonoBehaviour {
	//const string End_Key = "EndExtract2";
	void Awake(){
		UIManager.Intance.ShowWindow(WindowID.AssetCheckUI);
		if(GameManager.Instance.bClearOldExtract){
			if (Directory.Exists (PathTools.DataPath)) {
				Directory.Delete (PathTools.DataPath, true);
			}
		}
		//客户端存在并已经解压过了
		if(Util.HasExtracted()){
			EventsMgr.Instance ().TriigerEvent ("EndExtract", null);
		}else{
			StartCoroutine(OnExtractResource());   
		}
	}

	IEnumerator OnExtractResource()
	{
		string dataPath = PathTools.DataPath;  //数据目录
		Debug.Log("解包存放地址:>" + dataPath);
		string resPath = PathTools.AppContentPath(); //游戏包资源目录

		if (Directory.Exists(dataPath)) Directory.Delete(dataPath, true);
		Directory.CreateDirectory(dataPath);

		string infile = resPath + "files.txt";
		string outfile = dataPath + "files.txt";


		if (File.Exists(outfile)) File.Delete(outfile);

		AssetCheckData data = new AssetCheckData();
		data.msg = "比较资源中";
		data.value = 0f;

		UnityTools.LogMust("正在解包文件:>" + infile);
		UIManager.Intance.SendMsg(WindowID.AssetCheckUI, WindowMsgID.ShowLoadingTips, data);
		UnityTools.LogMust(infile);
		UnityTools.LogMust(outfile);
		if (Application.platform == RuntimePlatform.Android)
		{
			WWW www = new WWW(infile);
			yield return www;

			if (www.isDone)
			{
				File.WriteAllBytes(outfile, www.bytes);
			}
			yield return 0;
		}
		else File.Copy(infile, outfile, true);
		yield return new WaitForEndOfFrame();

		//释放所有文件到数据目录
		string[] files = File.ReadAllLines(outfile);
		int idx = 0;
		foreach (var file in files)
		{
			string[] fs = file.Split('|');
			infile = resPath + fs[0];  //
			outfile = dataPath + fs[0];

			data.msg = "解压文件, 不耗流量" + idx.ToString() + "/" + files.Length.ToString();
			idx++;
			data.value = (float)(idx - 1) / (float)files.Length;

			UnityTools.LogMust("正在拷贝文件:>" + data.value + " ,  " + infile);
			UIManager.Intance.SendMsg(WindowID.AssetCheckUI, WindowMsgID.ShowLoadingTips, data);

			string dir = Path.GetDirectoryName(outfile);
			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

			if (Application.platform == RuntimePlatform.Android)
			{
				WWW www = new WWW(infile);
				yield return www;
				if (www.isDone)
				{
					File.WriteAllBytes(outfile, www.bytes);
				}
				yield return 0;
			}
			else
			{
				if (File.Exists(outfile))
				{
					File.Delete(outfile);
				}
				File.Copy(infile, outfile, true);
			}
			yield return new WaitForEndOfFrame();
		}

		UnityTools.LogMust("拷贝完成!!!");
		data.value = 0.0f;
		data.msg = "版本检测";
		UIManager.Intance.SendMsg(WindowID.AssetCheckUI, WindowMsgID.ShowLoadingTips, data);
		yield return new WaitForSeconds(0.1f);
		//释放完成，开始启动更新资源
		UIManager.Intance.SendMsg(WindowID.AssetCheckUI, WindowMsgID.ShowLoadingTips, data);
		if (GameManager.Instance.bOpenExtract_Zip) {
			yield return StartCoroutine(UnZip());
			data.value = 0f;
			data.msg = "版本检测, 请耐心等待..";
			UnityTools.LogMust("解压完成!!!");
			UIManager.Intance.SendMsg(WindowID.AssetCheckUI, WindowMsgID.ShowLoadingTips, data);
		}

		HandleEndExtract ();
	}


	//解压zip
	IEnumerator UnZip()
	{
		AssetCheckData data = new AssetCheckData();
		data.msg = "解压文件中";
		data.value = 0f;
		string bundlePath = PathTools.DataPath + "assetbundle";
		if (!Directory.Exists(bundlePath))
		{
			Directory.CreateDirectory(bundlePath);
		}
		string scenePath = PathTools.DataPath + "scene";
		if (!Directory.Exists(scenePath))
		{
			Directory.CreateDirectory(scenePath);
		}

		string[] zipFolder = ZipHelper.ZipFolders();
		int tempIndex = 0;
		foreach (string path in zipFolder)
		{
			string zipPath = PathTools.DataPath + path;
			if (!Directory.Exists(zipPath))
			{
				Directory.CreateDirectory(zipPath);
			}
			tempIndex++;
			data.value = (float)(tempIndex - 1) / (float)zipFolder.Length;
			UIManager.Intance.SendMsg(WindowID.AssetCheckUI, WindowMsgID.ShowLoadingTips, data);
			string[] zipName = zipPath.Split('/');
			string zipLocal = PathTools.DataPath + "data_" + zipName[zipName.Length - 1] + ".zip";
			UnityTools.LogMust("正在解压文件 = "+zipLocal);
			ZipHelper.UnZipFolder(zipLocal, zipPath);
			File.Delete(zipLocal);
			yield return new WaitForSeconds(0.2f);
		}
		yield return 0;
	}

	/// <summary>
	/// 处理结束解压
	/// </summary>
	void HandleEndExtract(){
		EventsMgr.Instance ().TriigerEvent ("EndExtract", null);
	}
}
