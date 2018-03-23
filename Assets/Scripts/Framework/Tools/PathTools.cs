using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTools  {
	/// <summary>
	/// 取得数据存放目录 持久化目录，可读写目录
	/// </summary>
	public static string DataPath
	{
		get
		{
			string game = AppConst.AppName.ToLower();
			if (Application.isMobilePlatform)
			{

				if(Application.platform == RuntimePlatform.IPhonePlayer){
					Debug.Log ("Application.persistentDataPath = " + Application.persistentDataPath);
					return Application.persistentDataPath + "/" + game + "/" + GetPlatformFolder() + "/";
				}else {
					return Application.persistentDataPath + "/" + game + "/" + GetPlatformFolder() + "/";
					//return "/data/data/"+Application.buildGUID+"/files/"+ game + "/" + GetPlatformFolder() + "/";
				}
			}
			else
			{
				if (GameManager.Instance.ResFrom!= ResourceFrom.Origin) {
					return Application.persistentDataPath + "/" + game + "/" + GetPlatformFolder () + "/";
				} else {
					return Application.dataPath + "/" + AppConst.AssetDirname + "/" + GetPlatformFolder() + "/";
				}

			}
			return "c:/" + game + "/";
		}
	}

	/// <summary>
	/// 获取总的manifest的路径,  注意： 这类的Andoird 是对应的Manifest的名字而不是路径了Android
	/// </summary>
	public static string ManifestPath
	{
		get
		{
			if (Application.platform == RuntimePlatform.Android) {
				//return "jar:file://" + Application.dataPath + "!/assets/" + GetPlatformFolder() + "/" + "Android";
				return DataPath + "Android";
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				return DataPath + "IOS";
			} else {
				if(GameManager.Instance.ResFrom != ResourceFrom.Origin){
					return DataPath + GetPlatformFolder();
				}
				return DataPath + "Windows";
			} 
			return DataPath + "Windows";
		}
	}

	public static string GetRelativePath()
	{
		if (Application.isEditor)
			return "file://" + System.Environment.CurrentDirectory.Replace("\\", "/") + "/Assets/" + AppConst.AssetDirname + "/";
		else if (Application.isMobilePlatform || Application.isConsolePlatform)
			return "file:///" + DataPath;
		else // For standalone player.
			return "file://" + Application.streamingAssetsPath + "/";
	}

	/// <summary>
	/// 应用程序内容路径 = StreamingAsserts的路径
	/// </summary>
	public static string AppContentPath()
	{
		string path = string.Empty;
		switch (Application.platform)
		{
		case RuntimePlatform.Android:
			path = "jar:file://" + Application.dataPath + "!/assets/" + GetPlatformFolder() + "/";
			break;
		case RuntimePlatform.IPhonePlayer:
			path = Application.dataPath + "/Raw/" + GetPlatformFolder() + "/";
			break;
		default:
			if (GameManager.Instance.ResFrom != ResourceFrom.Origin) {
				return (Application.dataPath + "/" + AppConst.AssetDirname + "/" + GetPlatformFolder () + "/");
			} else {
				path = Application.dataPath + "/" + AppConst.AssetDirname + "/" + GetPlatformFolder() + "/";
			}
			break;
		}
		return path;
	}

	/// <summary>
	/// 平台文件夹名字
	/// </summary>
	/// <returns>The platform folder.</returns>
	public static string GetPlatformFolder()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			return AppConst.prefix_android;
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return AppConst.prefix_ios;
		}
		else
		{
			return GameManager.Instance.plat.ToString ();
			return AppConst.prefix_pc;
		}
	}

	/// <summary>
	/// 获取更新地址
	/// </summary>
	public static string GetWebURL(){
		return "http://"+AppConst.outIp+":"+AppConst.httpPort+"/"+GetPlatformFolder()+"/"; 
	}
}
