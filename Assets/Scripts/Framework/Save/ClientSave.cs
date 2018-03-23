using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonFx.Json;

public class ClientSave  {

	public static int GetInt(PrefsInfo info){
		return PlayerPrefs.GetInt (info.ToString(), 0);
	}

	public static bool GetBool(PrefsInfo info){
		int value = PlayerPrefs.GetInt (info.ToString(), 0);
		return (value>0);
	}

	public static string GetString(PrefsInfo info){
		return PlayerPrefs.GetString (info.ToString(), string.Empty);
	}

	public static void Save(PrefsInfo info, int value){
		PlayerPrefs.SetInt (info.ToString(), value);
		PlayerPrefs.Save ();
	}

	public static void Save(PrefsInfo info, bool value){
		if (value) {
			PlayerPrefs.SetInt (info.ToString(), 1);
		} else {
			PlayerPrefs.SetInt (info.ToString(), 0);
		}
		PlayerPrefs.Save ();
	}

	public static void Save(PrefsInfo info, string value){
		PlayerPrefs.SetString (info.ToString(), value);
		PlayerPrefs.Save ();
	}

	/// <summary>
	/// 清除所有的保存数据
	/// </summary>
	public static void ClearAll(){

		Debug.Log ("ClearAll keykeykeykeykeykeykeykeykeykeykeykey");
		PlayerPrefs.DeleteAll ();
		PlayerPrefs.Save ();
	}
}