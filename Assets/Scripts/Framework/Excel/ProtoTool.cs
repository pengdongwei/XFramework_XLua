using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using ProtoBuf;
//using for BuildMap
using System.Collections.Generic;
using System.Reflection;
using ProtoBuf.Meta;

public class ProtoTool {

	public static byte[] Serialize<T>(T t)
	{
		using (MemoryStream ms = new MemoryStream()) {
			Serializer.Serialize<T>(ms, t);
			return ms.ToArray();
		}
	}

	public static T DeSerialize<T>(byte[] content)
	{
		using (MemoryStream ms = new MemoryStream(content))
		{
			T t = Serializer.Deserialize<T>(ms);
			return t;
		}        
	}

	public static T Load<T>(string fileName)where T:ProtoBuf.IExtensible
	{
		TextAsset textAsset = null;
		if(GameManager.Instance.ResFrom == ResourceFrom.Origin){
			string protoPath = "ConfigData/"+fileName;
			textAsset = Resources.Load(protoPath) as TextAsset;	
		}else{
			textAsset = ResourceManager.GetInstance ().LoadText ("assetbundle/data/"+fileName, fileName);
			if (null == textAsset) {
				Debug.LogError("prototool bundle 加载失败, 没有对应的资源: " + fileName);
			}
		}
		T t = default(T);
		try
		{
			t = ProtoTool.DeSerialize<T>(textAsset.bytes);
		}
		catch (ProtoException)
		{
			Debug.LogError(string.Format("解析配表：<color=red>{0}</color>出错,请相关人员检查配表!", fileName));
		}
		catch (Exception)
		{
			Debug.LogError(string.Format("解析配表：<color=red>{0}</color>出错,未知异常!", fileName));
		}
		return t;
	}

	public static Dictionary<Tkey, T> BuildMap<Tkey,T>(string keyName, List<T> tlist)
	{
		System.Type protoType = typeof(T);
		PropertyInfo properInfo = protoType.GetProperty(keyName, typeof(Tkey));
		if (null == properInfo) {
			Debug.LogError ("不存在的Key名:" + keyName);
		}
		if (!(properInfo.PropertyType == typeof(Tkey))) {
			Debug.LogError ("KeyName类型不匹配!");
		}
		Dictionary<Tkey, T> buildmap = new Dictionary<Tkey, T> ();
		foreach (var entry in tlist) {
			Tkey mapkey = (Tkey)properInfo.GetValue (entry, null);
			if (buildmap.ContainsKey(mapkey))
				Debug.LogError(string.Format("字段名:{0},有重复值：{1}", properInfo.Name, mapkey));
			buildmap.Add (mapkey, entry);
		}

		return buildmap;
	}
}