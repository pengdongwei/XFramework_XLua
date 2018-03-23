/// <summary>
/// 打包工具
/// 打包的目录结构
/// StreamingAsserts
///     IOS
///     Android
///     Windows 
///         assetBundle (从这一层开始由自动bundle名字的工具进行设置)
///             model
///             effect
///             anim
///             ui
///   		scene (场景的后缀名为.unity3d)
///         data(csv+xml+json等数据文件,由于这个内容更新比较频繁, 而且不牵涉依赖关系, 最后不采用assetbundle的形式)
///         dll(代码更新的核心dll)
/// </summary>
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class AssetBundlePacker : Editor {

	//[MenuItem("武当剑工具/资源打包/一键美术工程打包(建议) %e", false, 12)]
	public static void Build()
	{
		if(!EditorUtility.DisplayDialog("" , "是否开始打包?",  "确定", "取消")){
			Debug.Log ("取消打包");
			return;
		}
		BuildPrefab();
		BuildPalyer();
		BundleCommonTools.CopyResourceToDevEnvirment();
	}

	//[MenuItem("武当剑工具/资源打包/打包(预制件)", false, 10)]
	public static void BuildPrefab()
	{

		if(EditorUtility.DisplayDialog("" , "是否先设置Assetbundle名字",  "确定", "取消")){
			AutoSetAssetBundleName.Init();
		}
		EditorUtility.DisplayProgressBar("打包中", "打包中...", 0f);
		string outputPath = EditorConfig.GetoutputPath();

		if(Directory.Exists(outputPath)){
			Directory.Delete(outputPath, true);
		}
		Directory.CreateDirectory(outputPath);
		BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
		EditorUtility.ClearProgressBar();
		AssetDatabase.Refresh();
	}

	[MenuItem("XTool/打包/打包(场景)", false, 11)]
	public static void BuildPalyer(){


		string outputPath = EditorConfig.GetoutputPath()+"scene/" ;

		if(Directory.Exists(outputPath)){
			Directory.Delete(outputPath, true);
		}
		Directory.CreateDirectory(outputPath);

		string fullPath = EditorConfig.ABSourceRoot+"scene/";
		string[] scenes = Directory.GetFiles(fullPath, "*.unity", SearchOption.AllDirectories);

		foreach(string sce in scenes){
			string[] splitScenes = sce.Split('/');
			string[] fileS = splitScenes[splitScenes.Length-1].Split('.');
			string outputpath = outputPath+fileS[0]+".unity3d";
			Debug.Log( outputpath );
			BuildPipeline.BuildPlayer(new string[]{sce}, outputpath, EditorUserBuildSettings.activeBuildTarget , BuildOptions.BuildAdditionalStreamedScenes);
		}
		AssetDatabase.Refresh();
	}


	// [MenuItem("武当剑工具/资源打包/打包(图集prefab)")]
	public static void BuildSpritePrefabs()
	{
		string spriteDir = Application.dataPath + "/Resources/UISprite";
		if (!Directory.Exists(spriteDir))
		{
			Directory.CreateDirectory(spriteDir);
		}
		string origDir = Application.dataPath + "/Arts/UI_DynamicLoad";
		DirectoryInfo dirInfo = new DirectoryInfo(origDir);
		var fileinfos = dirInfo.GetFiles("*.png", SearchOption.AllDirectories);
		int i = -1;
		foreach (var fileInfo in fileinfos)
		{
			++i;
			EditorUtility.DisplayProgressBar("打包(图集prefab)", "正在生成sprite预制体...", 1f * i / fileinfos.Length);
			string allPath = fileInfo.FullName;
			string assetPath = allPath.Substring(allPath.IndexOf("Asset"));
			Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
			if (null == sprite)
			{
				continue;
			}
			GameObject gobj = new GameObject();
			gobj.AddComponent<SpriteRenderer>().sprite = sprite;
			allPath = spriteDir + "/" + sprite.name + ".prefab";
			string prefabPath = allPath.Substring(allPath.IndexOf("Asset"));
			PrefabUtility.CreatePrefab(prefabPath, gobj);
			GameObject.DestroyImmediate(gobj);
		}
		EditorUtility.ClearProgressBar();
		AssetDatabase.Refresh();
	}

	/// <summary>
	/// 打包dll文件； 拷贝dll到指定环境下->生成md5文件
	/// </summary>
	//[MenuItem("武当剑工具/资源打包/打包(DLL+MD5)")]
	public static void BuildDll(){
		BuildTarget target =  EditorUserBuildSettings.activeBuildTarget;
		if (target == BuildTarget.Android) {
			/*
			string dllPath = Directory.GetParent (Application.dataPath) + "/Library/ScriptAssemblies/Assembly-CSharp.dll";
			string destPath = EditorConfig.GetoutputPath () + "Assembly-CSharp.dll";
			File.Copy (dllPath, destPath, true);*/
			BundleCommonTools.MakeResourcesMd5FilesAndCopy (bCopy: false);
			Debug.Log ("Dll已经压缩, 当前平台: Android");
		} else {
			BundleCommonTools.MakeResourcesMd5FilesAndCopy (bCopy: false);
			Debug.Log ("Android 平台专用, Dll打包被取消, 当前平台: " + EditorConfig.GetoutputPath(true));
		}
	}

	/// <summary>
	/// Builds the user interface bundle.
	/// </summary>
	//[MenuItem("武当剑工具/资源打包/打包(UI+DLL+MD5)")]
	public static void BuildUIBundle(){
		if(!EditorUtility.DisplayDialog("" , "是否开始打包?",  "确定", "取消")){
			Debug.Log ("取消打包");
			return;
		}

		BuildSpriteBundle ();

		if(EditorUtility.DisplayDialog("" , "是否先设置Assetbundle名字",  "确定", "取消")){
			AutoSetAssetBundleName.Init(true);
		}
		EditorUtility.DisplayProgressBar("打包中", "打包中...", 0f);
		string outputPath = EditorConfig.GetoutputPath()+"/assetbundle/ui/";

		if(Directory.Exists(outputPath)){
			Directory.Delete(outputPath, true);
		}
		Directory.CreateDirectory(outputPath);
		BuildPipeline.BuildAssetBundles(EditorConfig.GetoutputPath(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
		BuildDll ();
		EditorUtility.ClearProgressBar();
		AssetDatabase.Refresh();
	}

	/// <summary>
	/// 打包数据文件，包括策划csv + xml 管卡文件
	/// </summary>
	//[MenuItem("武当剑工具/资源打包/打包(Data+DLL+MD5)")]
	public static void BuildBytesBundle(){
		if(!EditorUtility.DisplayDialog("" , "是否开始打包?",  "确定", "取消")){
			Debug.Log ("取消打包");
			return;
		}

		if(EditorUtility.DisplayDialog("" , "是否先设置Assetbundle名字",  "确定", "取消")){
			AutoSetAssetBundleName.Init(true);
		}
		EditorUtility.DisplayProgressBar("打包中", "打包中...", 0f);
		string outputPath = EditorConfig.GetoutputPath()+"/assetbundle/data/";

		if(Directory.Exists(outputPath)){
			Directory.Delete(outputPath, true);
		}
		Directory.CreateDirectory(outputPath);
		BuildPipeline.BuildAssetBundles(EditorConfig.GetoutputPath(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
		BuildDll ();
		EditorUtility.ClearProgressBar();
		AssetDatabase.Refresh();	
	}

	/// <summary>
	/// 打包数据文件，包括策划csv + xml 管卡文件
	/// </summary>
	//[MenuItem("武当剑工具/资源打包/打包(Sprite)")]
	public static void BuildSpriteBundle(){
		if(!EditorUtility.DisplayDialog("" , "是否开始打包?",  "确定", "取消")){
			Debug.Log ("取消打包");
			return;
		}

		if(EditorUtility.DisplayDialog("" , "是否先设置Assetbundle名字",  "确定", "取消")){
			AutoSetAssetBundleName.Init(true);
		}
		EditorUtility.DisplayProgressBar("打包中", "打包中...", 0f);
		string outputPath = EditorConfig.GetoutputPath()+"/assetbundle/sprite/";

		if(Directory.Exists(outputPath)){
			Directory.Delete(outputPath, true);
		}
		Directory.CreateDirectory(outputPath);
		BuildPipeline.BuildAssetBundles(EditorConfig.GetoutputPath(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
		//BuildDll ();
		EditorUtility.ClearProgressBar();
		AssetDatabase.Refresh();	
	}

	/// <summary>
	/// 一键打包开发目录
	/// </summary>
	[MenuItem("XTool/打包/发布")]
	public static void BuildDev(){
		if(!EditorUtility.DisplayDialog("" , "是否开始打包?",  "确定", "取消")){
			Debug.Log ("取消打包");
			return;
		}

		if(EditorUtility.DisplayDialog("" , "是否先设置Assetbundle名字",  "确定", "取消")){
			AutoSetAssetBundleName.Init(true);
		}

		EditorUtility.DisplayProgressBar("打包中", "打包中...", 0f);
		string[] outputPathArray = new string[]{ 
			"assetbundle/sprite/", 
			"assetbundle/data/",
			"assetbundle/ui/",
		};

		foreach(string outputPath in outputPathArray){
			if(Directory.Exists(EditorConfig.GetoutputPath()+"/"+outputPath)){
				Directory.Delete(outputPath, true);
			}
			Directory.CreateDirectory(outputPath);
		}
		BuildPipeline.BuildAssetBundles(EditorConfig.GetoutputPath(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

		BuildDll ();
		EditorUtility.ClearProgressBar();
		AssetDatabase.Refresh();	
	}
}