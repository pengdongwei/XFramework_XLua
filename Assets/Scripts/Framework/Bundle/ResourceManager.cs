/// <summary>
/// 资源管理, 目前采用同步的方式进行
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public enum BundleType{
	none,
	data,
	sprite,
	ui,
}

public class ResourceManager : MonoBehaviour {
	private string[] m_Variants = { };
	private AssetBundleManifest manifest;
	private AssetBundle shared, assetbundle;
	private Dictionary<string, AssetBundle> bundles;
	private static ResourceManager Instance;
	//清除镜像文件列表
	List<string> unLoadBundleKeys = new List<string>{"assetbundle/data/", "assetbundle/mapconfig/", "assetbundle/skilldata/", "assetbundle/sound/"}; 
	//共享缓存
	List<string> retainBundleKeys = new List<string>{"assetbundle/font/", "assetbundle/shader/"};

	private Dictionary<string, AssetBundle> retainBundles;

	public static ResourceManager GetInstance(){
		if(Instance==null){
			Instance = UnityTools.AddManager<ResourceManager>();
			Instance.Initialize();
		}
		return Instance;
	}

	/// <summary>
	/// 初始化, 加载总的assetbundle
	/// </summary>
	public void Initialize() {
		DontDestroyOnLoad(gameObject);

		byte[] stream = null;
		string uri = string.Empty;
		retainBundles = new  Dictionary<string, AssetBundle>();
		bundles = new Dictionary<string, AssetBundle>();
		uri = PathTools.ManifestPath;
		if (!File.Exists (uri))
			return;
		stream = File.ReadAllBytes (uri);
		assetbundle = AssetBundle.LoadFromMemory (stream);
		manifest = assetbundle.LoadAsset<AssetBundleManifest> ("AssetBundleManifest");
	}


	public static UnityEngine.Object LoadResource(string path)
	{
		UnityEngine.Object obj = Resources.Load(path);
		if (obj == null)
		{
			//  Debug.LogError(path + "不存在");
		}
		return obj;
	}

	static Sprite LoadSpriteInteral(string path, string spriteName)
	{
		GameObject gobj = LoadResource(path + spriteName) as GameObject;
		if (null == gobj)
			return null;
		SpriteRenderer spRenderer = gobj.GetComponent<SpriteRenderer>();
		if (null == spRenderer)
			return null;
		return spRenderer.sprite;
	}

	/// <summary>
	/// 优先加载bundle里面的图片 然后加载Resources下的
	/// </summary>
	/// <returns>The sprite.</returns>
	/// <param name="spriteName">Sprite name.</param>
	public static Sprite LoadSprite(string spriteName)
	{
		if(string.IsNullOrEmpty(spriteName)) return null;
		if (GameManager.Instance.ResFrom != ResourceFrom.Bundle) {
			return Resources.Load<Sprite> ("Sprites/" + spriteName);
		}
		spriteName = spriteName.ToLower ();
		if (Application.isPlaying)
		{
			string abname = "";
			AssetBundle bundle = null;
			abname = "assetbundle/sprite/" + spriteName;
			bundle = GetInstance().LoadAssetBundle(abname);
			if(bundle == null){
				abname = "assetbundle/atlas/"+ ResLoader.GetAtlasName(spriteName);
				bundle = GetInstance().LoadAssetBundle(abname);
			} 
			if (bundle != null)
			{
				Sprite asset = bundle.LoadAsset<Sprite>(spriteName);
				return asset;
			}
		}
		return LoadSpriteInteral("UISprite/", spriteName);
	}   

	AssetBundle GetFromMemAtlas(string spriteName){
		foreach(KeyValuePair <string, AssetBundle> pair in bundles){
			if(pair.Key.Contains("atlas")){
				AssetBundle bundle = GetInstance().LoadAssetBundle(pair.Key);
				if (bundle != null)
				{
					Sprite sprite = bundle.LoadAsset<Sprite>(spriteName);
					if(sprite!=null) return bundle;
				}
			}
		}
		return null;
	}

	/// <summary>
	/// 加载文件夹下的所有内容，暂时只加载resource下的
	/// </summary>
	public Object[] LoadFolderContents(string path){
		Object[] objs = Resources.LoadAll(path);
		return  objs;
	}

	/// <summary>
	/// 载入素材
	/// </summary>
	public RuntimeAnimatorController LoadSmAsset (string abname, string assetname) {
		abname = abname.ToLower();
		AssetBundle bundle = LoadAssetBundle(abname);
		if(bundle==null){
			return (RuntimeAnimatorController)ResourceManager.LoadResource ("StateMachine/"+assetname);
		}
		return bundle.LoadAsset<RuntimeAnimatorController>(assetname);
	}


	public AudioClip LoadAudioClip(string assetname){
		string abname = "assetbundle/sound/"+assetname;
		abname = abname.ToLower();
		AssetBundle bundle = LoadAssetBundle(abname);
		if (bundle == null) {
			return Resources.Load<AudioClip>("Sound/"+assetname);
		}
		AudioClip asset = bundle.LoadAsset<AudioClip>(assetname);
		bundle.Unload(false);
		return asset;
	}

	/// <summary>
	/// 加载文本文件
	/// </summary>
	public TextAsset LoadText(string abname, string assetname) {
		AssetBundle bundle = LoadAssetBundle(abname.ToLower());
		if (bundle == null) {
			return	(TextAsset)LoadResource("ConfigData/"+assetname);
		} else {
			TextAsset asset = bundle.LoadAsset<TextAsset>(assetname);  
			bundle.Unload(false);
			return asset;
		} 
	}

	/// <summary>
	/// 加载UI prefab 里面资源
	/// </summary>
	public GameObject LoadUIAsset(string assetname, bool IsInstantiate=false){
		return LoadAsset ("assetbundle/ui/"+assetname, assetname, IsInstantiate);
	}

	/// <summary>
	/// 载入素材
	/// </summary>
	public GameObject LoadAsset(string abname, string assetname, bool IsInstantiate=false) {

		abname = abname.ToLower();
		bool bLoadFromBundle = true;
		string[] assetsArray = ResLoader.GetResourceFolderAssets();
		foreach(string ass in assetsArray){
			if(ass.Equals(assetname.ToLower())){
				bLoadFromBundle = false;
				break;
			}
		}
		AssetBundle bundle =null;
		if(bLoadFromBundle)  bundle = LoadAssetBundle(abname);
		if(bundle==null){
			Debug.Log ("准备在resources 下加载窗口 = "  +assetname);
			GameObject prefab =LoadResource("UIPrefab/" + assetname) as GameObject;
			if (null != prefab)
			{
				if (IsInstantiate)
				{
					prefab = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
				}
				return prefab;
			}
		}
		if (bundle == null)
		{
			Debug.LogError("bundle 资源不存在, 资源名字 = " + assetname);
			return new GameObject(assetname);
		}
		GameObject asset = bundle.LoadAsset<GameObject>(assetname);
		if(IsInstantiate){
			asset = GameObject.Instantiate(asset);
		}
		return asset;
	}



	/// <summary>
	/// 载入AssetBundle
	/// </summary>
	/// <param name="abname"></param>
	/// <returns></returns>
	AssetBundle LoadAssetBundle(string abname) {
		if (!abname.EndsWith(AppConst.ExtName)) {
			abname += AppConst.ExtName;
		}
		AssetBundle bundle = null;
		//加载非缓存bundle
		if(IsUnLoadBundle(abname)){ 
			return GetBundle(abname);
		}else{
			// 加载常驻内存bundle 
			if(IsRetainBundle(abname)){
				bundle = TryLoadAssetBundle(retainBundles, abname);
			}else{
				//加载临时驻内测bundle( 切换场景卸掉 )
				bundle = TryLoadAssetBundle(bundles, abname);
			}
		}
		return bundle;
	}

	AssetBundle TryLoadAssetBundle(Dictionary<string, AssetBundle> bundleDict, string abname){
		AssetBundle bundle = null;
		if(!bundleDict.ContainsKey(abname)) {
			bundle = GetBundle(abname);
			bundleDict.Add(abname, bundle);
		}else{
			bundles.TryGetValue(abname, out bundle);
		}
		return bundle;
	}

	bool IsUnLoadBundle(string abname){
		for(int i=0; i<unLoadBundleKeys.Count; i++){
			if(abname.Contains(unLoadBundleKeys[i])) return true;
		}
		return false;
	}

	bool IsRetainBundle(string abname){
		for(int i=0; i<retainBundleKeys.Count; i++){
			if(abname.Contains(retainBundleKeys[i])) return true;
		}
		return false;
	}


	AssetBundle GetBundle(string abname){
		byte[] stream = null;
		string uri = PathTools.DataPath + abname;
		if(!File.Exists(uri)){
			Debug.Log ("abname = " + uri +" 不存在");

			return null;
		}
		LoadDependencies(abname);
		return AssetBundle.LoadFromFile (uri);	
	}

	/// <summary>
	/// 载入依赖
	/// </summary>
	/// <param name="name"></param>
	void LoadDependencies(string name) {
		if (manifest == null) {
			Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
			Initialize ();
			if(manifest==null)   return;

		}
		// Get dependecies from the AssetBundleManifest object..
		string[] dependencies = manifest.GetAllDependencies(name);
		if (dependencies.Length == 0) return;

		for (int i = 0; i < dependencies.Length; i++)
			dependencies[i] = RemapVariantName(dependencies[i]);

		// Record and load all dependencies.
		for (int i = 0; i < dependencies.Length; i++) {
			LoadAssetBundle(dependencies[i]);
		}
	}

	// Remaps the asset bundle name to the best fitting asset bundle variant.
	string RemapVariantName(string assetBundleName) {
		string[] bundlesWithVariant = manifest.GetAllAssetBundlesWithVariant();

		// If the asset bundle doesn't have variant, simply return.
		if (System.Array.IndexOf(bundlesWithVariant, assetBundleName) < 0)
			return assetBundleName;

		string[] split = assetBundleName.Split('.');

		int bestFit = int.MaxValue;
		int bestFitIndex = -1;
		// Loop all the assetBundles with variant to find the best fit variant assetBundle.
		for (int i = 0; i < bundlesWithVariant.Length; i++) {
			string[] curSplit = bundlesWithVariant[i].Split('.');
			if (curSplit[0] != split[0])
				continue;

			int found = System.Array.IndexOf(m_Variants, curSplit[1]);
			if (found != -1 && found < bestFit) {
				bestFit = found;
				bestFitIndex = i;
			}
		}
		if (bestFitIndex != -1)
			return bundlesWithVariant[bestFitIndex];
		else
			return assetBundleName;
	}

	/// <summary>
	/// 销毁资源
	/// </summary>
	void OnDestroy() {
		if (shared != null) shared.Unload(true);
		if (manifest != null) manifest = null;
		Debug.Log("~ResourceManager was destroy!");
	}

	/// <summary>
	/// 卸载所有的assetbundle
	/// </summary>
	public void UnLoadAllBundle(){
		foreach(var v in bundles){
			if(v.Value!=null)v.Value.Unload (true);			
		}
		bundles.Clear ();
		Util.ClearMemory ();
	}
}