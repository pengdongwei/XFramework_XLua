/// <summary>
///	自动设置AB 名字工具。 打包路径请在  EditorConfig 配置
/// v2.0 修改assetbundle名字的设置方式，目前场景由于使用的是buildplayer, 所以场景的排除在外
///      打包会进行两个步骤
///      1：一个是在美术工程目录下进行的，这个一般打包后不在进行改动，
///      2：一个是程序工程目录主要打包UI csv  图片等等，然后和美术的打包进行合并操作
/// 
/// </summary>
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class AutoSetAssetBundleName : MonoBehaviour {

	//	[MenuItem("武当剑工具/资源打包/通用工具/自动命名Assetbundle名字", false, 1)]
	public static void Init(bool bUIOnly = false)
	{
		string dicPath = "";
		/*
		//设置开发目录
		//UI 图片
		dicPath = Application.dataPath + "/Resources/Sprites/";
		SetNames (dicPath, new string[]{ "*.png", "*.jpg" }, "assetbundle/sprite/");
		//font
		dicPath = Application.dataPath + "/Arts/";
		SetNames (dicPath, new string[]{ "*.ttf" }, "assetbundle/font/");
		//UI prefab
		dicPath = Application.dataPath + "/Resources/UIPrefab/";
		SetNames (dicPath, new string[]{ "*.prefab" }, "assetbundle/ui/", excludeArray: new string[] {
			"assetcheckui",
			"loadingui"	});
		//模型
		dicPath = Application.dataPath + "/Resources/Model";
		SetNames(dicPath, new string[]{"*.prefab"}, "assetbundle/model/");
		//声音
		dicPath = Application.dataPath + "/Resources/Sound";
		SetNames(dicPath, new string[]{"*.mp3"}, "assetbundle/sound/");
		//特效
		dicPath = Application.dataPath + "/Resources/Effect";
		SetNames(dicPath, new string[]{"*.prefab"}, "assetbundle/effect/");
		*/


		//CSV
		dicPath = Application.dataPath + "/Resources/ConfigData/";
		SetNames (dicPath, new string[]{ "*.bytes" }, "assetbundle/data/");
	}

	/// <summary>
	///设置bundle的名字
	/// </summary>
	/// <param name="dicPath">源文件，文件夹名字.</param>
	/// <param name="extArray">扩展名 数组</param>
	/// <param name="assetRootName">资源根的名字，如果需要手动指定的可以不填这个</param>
	/// <param name="manualName">手动自定名字</param>
	/// <param name="excludeArray">排除集合, 不在这个里面的不进行打包操作</param>
	public static void SetNames(string dicPath, string[] extArray, string assetRootName, string manualName = "", string[] excludeArray = null){
		if(Directory.Exists(dicPath)){
			var dir = new DirectoryInfo (dicPath);
			foreach(string ext in extArray){
				var files = dir.GetFiles(ext, SearchOption.AllDirectories);
				for(int i=0; i<files.Length; i++){
					var fileInfo = files[i];
					EditorUtility.DisplayProgressBar("设置Assetbundle名称", "正在设置Assetbundle名称中...", 1*i/files.Length);
					if(!fileInfo.Name.EndsWith(".meta")){
						//处理特殊情况不打包的资源
						bool bexclude = false;
						if (excludeArray != null) {
							string filename = fileInfo.Name.Split ('.') [0].ToLower ();
							foreach(string str in excludeArray){
								if (filename.Equals (str)) {
									bexclude = true;
									continue;
								}
							}
						}
						if (bexclude) continue;

						//设置名字操作
						int index = EditorConfig.ToolsProjectDirRoot.Length + 1;
						int length = fileInfo.FullName.Length - index;
						string importerName = fileInfo.FullName.Substring (index, length);
						var importer = AssetImporter.GetAtPath(importerName.Replace("\\", "/"));
						string bundleName = assetRootName + fileInfo.Name.Split ('.') [0];
						if(importer!=null && importer.assetBundleName!=bundleName){
							if (string.IsNullOrEmpty (manualName)) {
								importer.assetBundleName = bundleName + ".assetbundle";
							} else {
								importer.assetBundleName = manualName+ ".assetbundle";
							}

						}
					}
				}
			}
			EditorUtility.ClearProgressBar();
			AssetDatabase.Refresh();
		}
	}

	//	[MenuItem("武当剑工具/资源打包/通用工具/检查资源", false, 1)]
	public static void CheckRes(){
		string dicPath = Application.dataPath;
		var dir = new DirectoryInfo (dicPath);
		var files = dir.GetFiles("*.*", SearchOption.AllDirectories);
		for(int i=0; i<files.Length; i++){
			var fileInfo = files[i];
			EditorUtility.DisplayProgressBar("设置Assetbundle名称", "正在设置Assetbundle名称中...", 1*i/files.Length);
			if(!fileInfo.Name.EndsWith(".meta")){
				//设置名字操作
				int index = EditorConfig.ToolsProjectDirRoot.Length + 1;
				int length = fileInfo.FullName.Length - index;
				string importerName = fileInfo.FullName.Substring (index, length);
				var importer = AssetImporter.GetAtPath(importerName.Replace("\\", "/"));

				if(importer!=null && importer.assetBundleName.Contains("assets")){
					Debug.Log ("name = "+ fileInfo.FullName);

				}
			}
		}
		EditorUtility.ClearProgressBar();
		AssetDatabase.Refresh();
	}

}