using UnityEngine;
using System.Collections;
namespace Assets.Game.Code
{
	public class Utilities 
	{
	    /// <summary>
		/// Стучимся до бандла, пытаемся забрать его
		/// </summary>
		/// <returns>
		/// Нужный бандл или нулл
		/// </returns>
		/// <param name='fullPath'>
		/// Полный путь до бандла
		/// </param>
		public static AssetBundle GetBundle(string fullPath)
		{
			AssetBundle result = null;
			using(WWW sphereWWW = WWW.LoadFromCacheOrDownload(fullPath, 1))
			{
				if(sphereWWW.error == null)
				{
					result = sphereWWW.assetBundle;
				}
			}
			return result;
		}
	}
	
	public class AudioUtilities
	{
		static string mainSoundPath = "file://" + Application.dataPath + "/Game/Bundles/MainSound.unity3d"; //путь до главной музыкальной темы игры
		static string shootSoundPath = "file://" + Application.dataPath + "/Game/Bundles/shoot.unity3d"; // путь до звука выстрела
		
		static  AudioClip shootSound = GetSound(shootSoundPath);
		/// <summary>
		/// Звук выстрела
		/// </summary>
		public static AudioClip ShootSound
		{
			get { return shootSound; }
		}
		
		static  AudioClip mainSound = GetSound(mainSoundPath);
		/// <summary>
		/// Главная музкальная тема игры
		/// </summary>
		public static AudioClip MainSound 
		{
			get { return mainSound; }
		}
		
		/// <summary>
		/// Загружаем из бандла аудио клип по указанному пути
		/// </summary>
		/// <returns>
		/// Ауидо клип
		/// </returns>
		/// <param name='soundPath'>
		/// Путь до бандла с нужным аудио клипом
		/// </param>
		static AudioClip GetSound(string soundPath)
		{
			AudioClip result = null;
			AssetBundle soundBundle = Utilities.GetBundle(soundPath);
			if(soundBundle != null)
			{
				 result = Object.Instantiate(soundBundle.mainAsset) as AudioClip;
				 soundBundle.Unload(false);
			}
			return result;
		}
	}
}

