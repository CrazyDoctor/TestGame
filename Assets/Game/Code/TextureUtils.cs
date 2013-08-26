using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Assets.Game.Code
{
	public class TextureUtils : MonoBehaviour
	{
		/*
		 сначала создаем коллекции с текстурами, потом из этих коллекций текстуры раздаются объектам. При уничтожении объекто текстуры удаляются.
		 При достижении определенного количества уничтоженных сфер начинаем чистить память.
		*/
		static int _maxDisabledCount = 10; // максимальное количество текстур удаленных сфер
		static int _deletedCounter = 0;
		public static int DeletedCounter
		{
			get { return _deletedCounter; }
			set { _deletedCounter = value; }
		}
		
		static int maxTexturesCount = 10; // кол-во хранимых текстур в коллекции
		static List<Texture2D> generatedTextures32 = null; // 32*32 текстуры
		static List<Texture2D> generatedTextures64 = null; //64*64 текстуры
		static List<Texture2D> generatedTextures128 = null; //128*128 текстуры
		static List<Texture2D> generatedTextures256 = null; //256*256 текстуры
		
		
		void Start()
		{
			
		}
		
		void Update()
		{
			ClearDisabledTextures();
		}
		
		/// <summary>
		/// Генерируем текстуры
		/// </summary>
		static void FillTextures(ref List<Texture2D> texList, int size)
		{
			if(texList.Count == 0)
			{
				for(int i = 0; i < maxTexturesCount; i++)
				{
					texList.Add(GenerateTexture(size,new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value)));
				}
			}
		}
		
		/// <summary>
		/// Заполняем коллекции текстур
		/// </summary>
		public static void FillTextures()
		{
			if(generatedTextures32 == null)
				generatedTextures32 = new List<Texture2D>();
			if(generatedTextures64 == null)
				generatedTextures64 = new List<Texture2D>();
			if(generatedTextures128 == null)
				generatedTextures128 = new List<Texture2D>();
			if(generatedTextures256 == null)
				generatedTextures256 = new List<Texture2D>();
			
			FillTextures(ref generatedTextures32, 32);
			FillTextures(ref generatedTextures64, 64);
			FillTextures(ref generatedTextures128, 128);
			FillTextures(ref generatedTextures256, 256);
		}
		
		/// <summary>
		/// Генерируем текстуру
		/// </summary>
		static Texture2D GenerateTexture(int size, Color color)
		{
			Texture2D result = new Texture2D(size, size,TextureFormat.ARGB32, false); //задаем размер текстуры
			// заливка текстуры
			for(int i = 0; i < result.width; i++) 
			{
				for(int j = 0; j < result.height; j++)
				{
					result.SetPixel(i, j, new Color(GetNeededDigit(color.r, i, j, result.width, result.height), GetNeededDigit(color.g, i, j, result.width, result.height), GetNeededDigit(color.b, i, j, result.width, result.height)));
				}
			}
			
			result.Apply();
			return result;
		}
		
		/// <summary>
		/// Задаем цвет пикселя (вернее одной составляющей цвета)
		/// </summary>
		static float GetNeededDigit(float colorVal, int i, int j, int width, int height)
		{
			return (float)(colorVal * Mathf.Abs(width*i + height*j)) / (Mathf.Pow(width, 2) + Mathf.Pow(height, 2));
		}
		
		/// <summary>
		/// Отдаем текстуру объекту и удаляем из коллекции
		/// </summary>
		public static Texture2D GetTexture(float figureSize)
		{
			Texture2D result = null;
			
			if(figureSize == 0.5f)
			{
				if(generatedTextures32 != null && generatedTextures32.Count > 0)
				{
					result = generatedTextures32.FirstOrDefault();
					generatedTextures32.Remove(result);
				}
			}
			else if(figureSize == 1.0f)
			{
				if(generatedTextures64 != null && generatedTextures64.Count > 0)
				{
					result = generatedTextures64.FirstOrDefault();
					generatedTextures64.Remove(result);
				}
			}
			else if(figureSize == 1.5f)
			{
				
				if(generatedTextures128 != null && generatedTextures128.Count > 0)
				{
					result = generatedTextures128.FirstOrDefault();
					generatedTextures128.Remove(result);
				}
			}
			else
			{
				if(generatedTextures256 != null && generatedTextures256.Count > 0)
				{
					result = generatedTextures256.FirstOrDefault();
					generatedTextures256.Remove(result);
				}
			}
			
			return result;
		}
		
		/// <summary>
		/// Чистим память
		/// </summary>
		static void ClearDisabledTextures()
		{
			if(_deletedCounter > _maxDisabledCount)
			{
				Resources.UnloadUnusedAssets();
				_deletedCounter = 0;
			}
		}
	}
}

