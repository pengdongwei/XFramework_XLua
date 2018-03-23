using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using commdef;
using System;

public class DicDataManager:ISingleton<DicDataManager>
{
	private static DicDataManager Instance;
	public static DicDataManager GetInstance()
	{
		if (Instance == null)
		{
			Instance = new DicDataManager();
		}
		return Instance;
	}


	public void PreLoadCsvData(){
		//LoadGlobeData ();
	}

	public void LoadAllCsvData()
	{
		LoadHeroData ();

	}
		
	void LoadHeroData()
	{
		//herotable = ProtoTool.Load<HeroTable> ("Hero");
		//Hero_Dic = ProtoTool.BuildMap<int, Hero> ("ID", herotable.tlist);
	}

}