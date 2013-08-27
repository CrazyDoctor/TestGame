using Assets.Game.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MainScene : MonoBehaviour
{
	static int _allPoints = 0;//счетчик заработанных очков
	public static int AllPoints 
	{
		get { return _allPoints; }
		set { _allPoints = value; }
	}
	String pointsText = string.Empty;//строка вывода информации о заработанных очках
	AssetBundle sphereBundle;//бандл сфер
	RaycastHit hitInfo; //хранилище информации о нажатом объекте
	//AudioClip sound;
	//AudioClip soundShoot;
	
    void Start()
    {
		sphereBundle = Utilities.GetBundle(SphereScript.SphereBundlePath);
		audio.clip = AudioUtilities.MainSound;
		if(audio.clip != null)
		{
			audio.Play();
		}
		TextureUtils.FillTextures();
		DoWork();
    }
	
	void OnGUI()
	{
	   pointsText = "Заработано " + _allPoints + " очков";	
	   GUI.Label(new Rect(10, Screen.height - 50, 150, 50), pointsText);
	}

    void Update()
    {
		
		if(!IsInvoking("DoWork"))
		{
			Invoke ("DoWork", SphereScript.StartInterval);
		}
    }
	
	void DoWork()
	{
		if (sphereBundle != null)
        {
			TextureUtils.FillTextures();
			GameObject gameObj = UnityEngine.Object.Instantiate(sphereBundle.mainAsset)as GameObject;
			SphereScript sphereScript = gameObj.GetComponent<SphereScript>();
			if(sphereScript != null)
			{
				sphereScript.InitializeSphere();
			}
        }
	}
	
	/// <summary>
	/// Зачистка памяти
	/// </summary>
	void OnApplicationQuit()
	{
		sphereBundle.Unload(true);
		Resources.UnloadUnusedAssets();
	}
}
