using Assets.Game.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MainScene : MonoBehaviour
{
	int _allPoints = 0;//счетчик заработанных очков
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
	   GUI.Label(new Rect(Screen.width - 200, Screen.height - 50, 150, 50), pointsText);
	}

    void Update()
    {
		
		if(!IsInvoking("DoWork"))
		{
			Invoke ("DoWork", SphereScript.StartInterval);
		}
		
		// это клик, лопаем
		SphereClickAndDestory();
    }
	
	/// <summary>
	/// Обработка нажатия на экранб если сфера то унитожаем сферу и начисляем очки
	/// </summary>
	void SphereClickAndDestory()
	{
		// строим луч от камеры до точки нажатия, обрабатываем ситуацию пересечения этого луча и какого либо объекта
		if (Physics.Raycast( Camera.main.ScreenPointToRay( Input.mousePosition ), out hitInfo )) 
		{
			GameObject gameObj = hitInfo.collider.gameObject;
			if(gameObj != null)
			{
				SphereScript sphere = gameObj.GetComponent<SphereScript>();
				//взяли сферу, на которую нажали, если она есть (объект является сферой и он существует), то увеличичваем счет в игре и уничтожаем объект сферы
				if(sphere != null)
				{
					sphere.PlayShoot();
					_allPoints += sphere.PointsCount;
					Destroy(gameObj);
					SphereScript.DivValue = _allPoints / 1000;
				}
			}
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
