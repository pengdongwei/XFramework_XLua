using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour {
	private AudioSource audio = null;
	private AudioSource once_audio = null;
	private static MusicManager instance;
	private Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();

	public static MusicManager Instance
	{
		get
		{ 
			if (instance == null) 
			{
				GameObject music  = new GameObject("MusicRoot");
				instance = music.AddComponent<MusicManager> ();
			}
			return instance; 
		}
	}

	void Awake() {
		GameObject music_audio_go = new GameObject("music_audio_go");
		audio = UnityTools.TryAddComponent<AudioSource> (music_audio_go);

		GameObject once_audio_go = new GameObject("once_audio_go");
		once_audio = UnityTools.TryAddComponent<AudioSource> (once_audio_go);

		UnityTools.TryAddComponent<AudioListener> (gameObject);
	}

	/// <summary>
	/// 添加一个声音
	/// </summary>
	void Add(string key, AudioClip value) {
		if (!audioDict.ContainsKey (key)) {
			audioDict.Add (key, value);
		};
	}

	/// <summary>
	/// 获取一个声筇
	/// </summary>
	AudioClip Get(string key) {
		if (audioDict.ContainsKey(key))
		{
			return audioDict[key];
		}
		return null;
	}

	public void PlayMusic(string audioFileName){
		if (ClientSave.GetBool(PrefsInfo.IsCloseMusic))	return;
		audio.loop = true;
		AudioClip clip = Resources.Load<AudioClip> ("Sound/"+audioFileName); 
		audio.clip = clip;
		audio.Play ();

	}

	public void PlayOneShot(string audioFileName){
		if (ClientSave.GetBool(PrefsInfo.IsCloseMusic))	return;
		once_audio.loop = false;
		AudioClip clip = Resources.Load<AudioClip> ("Sound/"+audioFileName); 
		once_audio.clip = clip;
		once_audio.Play ();
	}

	public void PauseMusic(bool bPause){
		if (bPause) {
			audio.Pause ();
		} else {
			audio.Play ();
		}
	}

	public void StopMusic(){
		audio.Stop ();
	}
	/// <summary>
	/// 播放音频剪辑
	/// </summary>
	/// <param name="clip"></param>
	/// <param name="position"></param>
	public void Play(string audioFileName, Vector3 postion = default(Vector3)) {
		if (ClientSave.GetBool(PrefsInfo.IsCloseMusic))	return;
		AudioClip clip = null;
		if (Get (audioFileName) == null) {
			clip = Resources.Load<AudioClip> ("Sound/"+audioFileName);  // ResourceManager.GetInstance ().LoadAudioClip (audioFileName);
			if(clip==null)return;
			Add (audioFileName, clip);
		} else {
			clip = Get (audioFileName);
		}

		AudioSource.PlayClipAtPoint(clip, Vector3.zero);
	}
}
