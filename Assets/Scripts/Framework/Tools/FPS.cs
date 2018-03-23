using UnityEngine;

//FPS 显示工具类
public class FPS : MonoBehaviour
{
	public float updateInterval = 0.5f;
	private float lastInterval;
	private int frames;
	private float fps;
	private string sFps;
	private GUIStyle style;

	private void Start()
	{
		lastInterval = Time.realtimeSinceStartup;
		frames = 0;

		style = new GUIStyle();
		style.fontSize = 50;
		style.normal.textColor = Color.white;
	}

	private void Update()
	{
		frames++;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (realtimeSinceStartup > lastInterval + updateInterval)
		{
			fps = frames / (realtimeSinceStartup - lastInterval);
			sFps = fps.ToString("0.0");
			frames = 0;
			lastInterval = realtimeSinceStartup;
		}
	}

	private void OnGUI()
	{
		GUI.color = Color.red;
		GUI.Label(new Rect(10, 10, 200, 100), sFps, style);
	}

	/// <summary>
	/// 获取当前帧数
	/// </summary>
	/// <returns></returns>
	public float GetCurrentFPS()
	{
		return fps;
	}

}