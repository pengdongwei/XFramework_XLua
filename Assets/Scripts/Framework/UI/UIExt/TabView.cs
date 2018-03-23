using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabView : MonoBehaviour
{
	public string TITLE_NAME = "TabTitle";
	public string PANEL_NAME = "TabPanel";
	public List<Toggle> tabTitleList;
	public List<GameObject> tabPanelList;
	public ToggleGroup toggleGroup;

	void Start()
	{
		int i;
		//自动查找
		if (toggleGroup == null)
		{
			this.transform.gameObject.AddComponent<ToggleGroup>();
			toggleGroup=this.transform.GetComponent<ToggleGroup>();
		}
		if (tabTitleList == null||tabTitleList.Count==0)
		{
			foreach (Transform child in this.transform)
			{
				GameObject gameObject = child.gameObject;
				if (gameObject.name == TITLE_NAME)
				{
					tabTitleList.Add(gameObject.transform.GetComponent<Toggle>()); 
				}
			}
		}
		if (tabPanelList == null || tabPanelList.Count == 0)
		{
			foreach (Transform child in this.transform)
			{
				GameObject gameObject = child.gameObject;
				if (gameObject.name == PANEL_NAME)
				{
					tabPanelList.Add(gameObject);
				}
			}
		}
		//判断列表是否为空
		if (tabTitleList == null || tabPanelList == null|| tabTitleList.Count==0||tabPanelList.Count==0)
		{
			print("list is null.");
			return;
		}
		else if (tabTitleList.Count != tabPanelList.Count)
		{
			print("list is not match." + tabTitleList.Count + ":" + tabPanelList.Count);
			return;
		}
		else
		{
			//初始化，挂载toggleGroup，挂载监听器
			for (i = 0; i < tabTitleList.Count; i++)
			{
				if (i == 0)
				{
					tabTitleList[i].isOn = true;
					tabPanelList[i].SetActive(true);
				}
				else
				{
					tabTitleList[i].isOn = false;
					tabPanelList[i].SetActive(false);
				}
				if(toggleGroup!=null)tabTitleList[i].group = toggleGroup;
				tabTitleList[i].onValueChanged.AddListener(OnVauelChanged);
			}
		}
	}

	private void OnVauelChanged(bool isOn)
	{
		int i;
		if (!isOn)
			return;
		else
		{
			for (i = 0; i < tabTitleList.Count; i++)
			{
				tabPanelList[i].SetActive(tabTitleList[i].isOn);
			}
		}
	}
}
