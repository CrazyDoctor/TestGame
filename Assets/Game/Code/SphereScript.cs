using UnityEngine;
using System.Collections;
using Assets.Game.Code;

public class SphereScript : BaseFigure
{
	static double MinSize = 0.5; // минималный размер сферы
	const float _minStartInterval = 0.1f; //минимальный промежуток времени между генерацией сферы
	SphereCollider colliderSphere = null; // коллайдер
	static string _sphereBundlePath = "file://" + Application.dataPath + "/Game/Bundles/SphereBundleWithScriptSound.unity3d"; // путь до бандла сферы
	static Vector2 _tileScaleVector = new Vector2(0, 1); //
	public static string SphereBundlePath
	{
		get { return _sphereBundlePath; }
	}
	
	// общмй коэффициент прироста
	static int _divValue = 0;
	public static int DivValue
	{
		set { _divValue = value; }
	}
	const float _growCoefficient = 0.1f;
	
	// интервал между появлениями сфер
	static float _startInterval = 2;
	public static float StartInterval
	{
		get { return SetStartInterval(); }
	}
	
	//коэффициент для роста скорости
	protected override float BaseSpeedCoefficient
    {
        get { return 1 + _divValue * _growCoefficient; }
    }
       
    public SphereScript()
    {
		this._startTime = Time.time;
    }
	
	public void InitializeSphere()
    {
		this.GenerateMainSize();
		this.SetStartCoordinates();
		this.SetRadius();
		//this.GenerateColor(); //для сложности 1, генерация цвета сферы
		this.SetTexture();
        this.GeneratePoints();
		this.MoveDown();
    }
	
	static float SetStartInterval()
	{
		float growInterval = _startInterval - _divValue * _growCoefficient;
		if(growInterval > _minStartInterval)
		{
			return growInterval;
		}
		else
		{
			return _minStartInterval;
		}
	}
	
	/// <summary>
	/// Задаем радиус сферы
	/// </summary>
	void SetRadius()
	{
		if(colliderSphere == null)
		{
			colliderSphere = this.GetComponent("SphereCollider") as SphereCollider;
		}
		if(colliderSphere != null)
		{
			//раобтаем с localScale (масштабируем сферу, чтобу разница в радиусах была видна)
			double scaleValue = this._figureSize / SphereScript.MinSize + 2;
			this.transform.localScale = new Vector3((float)scaleValue, (float)scaleValue, (float)scaleValue);
		}
	}
	
	/// <summary>
    /// Генерация скорости объекта (зависит от размера)
    /// </summary>
    protected override void GenerateSpeed(float distance)
    {
        base.GenerateSpeed(distance);
    }

    protected override void GeneratePoints()
    {
        base.GeneratePoints();
    }
	
	/// <summary>
    /// Рандомно выбираем сторону (для сфер это радиус)
    /// </summary>
    protected override void GenerateMainSize()
    {
        base.GenerateMainSize();
    }
	
	/// <summary>
    /// Назначаем нашей фигуре рандомный цвет
    /// </summary>
    protected override void GenerateColor()
    {
		if(colliderSphere == null)
		{
			colliderSphere = this.GetComponent("SphereCollider") as SphereCollider;
		}
        if (colliderSphere != null)
        {
            colliderSphere.renderer.material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        }
        else
        {
            //обработка отсутсвия колайдера
        }
    }
	
	/// <summary>
	/// устанавливаем текстуру 
	/// </summary>
	void SetTexture()
	{
		if(colliderSphere == null)
		{
			colliderSphere = this.GetComponent("SphereCollider") as SphereCollider;
		}
        if (colliderSphere != null)
        {
			colliderSphere.renderer.material.mainTexture = TextureUtils.GetTexture(this._figureSize); //TextureUtils.GenerateTexture(this._figureSize, new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value));
			colliderSphere.renderer.material.mainTextureScale = _tileScaleVector;
        }
        else
        {
            //обработка отсутсвия колайдера
        }
	}
	
	/// <summary>
    /// Устанавливаем начальные координаты объекта
    /// </summary>
    protected override void SetStartCoordinates()
    {
        if (this._figureSize > 0)
		{
            this._startCoordinates =  new Vector3(UnityEngine.Random.Range(- Camera.main.orthographicSize + this._figureSize, Camera.main.orthographicSize - this._figureSize), Camera.main.orthographicSize - this._figureSize, 0);
			this.transform.position = this._startCoordinates;
		}
        else
        {
            //обрабатываем ошибку (безразмерный объект)
        }
    }
	
	/// <summary>
	/// Движение сферы вниз
	/// </summary>
    public override void MoveDown()
    {
        float endY = (Camera.main.orthographicSize * 2) - this._figureSize * 2;
        this.GenerateSpeed(endY);
        this.transform.position = Vector3.Lerp(this._startCoordinates, new Vector3(this._startCoordinates.x,  -1 * Camera.main.orthographicSize + this._figureSize, this._startCoordinates.z), this._figureSpeed * 10);
    } 	
	
	/// <summary>
	/// Воспроизведение звука "лопанья"
	/// </summary>
	public void PlayShoot()
	{
		if(audio != null && AudioUtilities.ShootSound != null)
		{
			AudioSource.PlayClipAtPoint(AudioUtilities.ShootSound, Camera.main.transform.position, 0.3f);
			//audio.PlayOneShot(clipItem, 5);
		}
	}
	
	void Update()
	{
		if(this.transform.position.y > -Camera.main.orthographicSize + this.transform.localScale.y - this._figureSize)
		{
			this.MoveDown();
		}
		else
		{
			DestroyObject(gameObject);
		}
	}
	
	void OnMouseDown()
	{
		if(gameObject != null)
		{
			SphereScript sphere = gameObject.GetComponent<SphereScript>();
			//взяли сферу, на которую нажали, если она есть (объект является сферой и он существует), то увеличичваем счет в игре и уничтожаем объект сферы
			if(sphere != null)
			{
				sphere.PlayShoot();
				MainScene.AllPoints += sphere.PointsCount;
				Destroy(gameObject);
				_divValue = MainScene.AllPoints / 1000;
			}
		}
	}
	
	void OnDestroy()
	{
		//переносим текстуру наложенную на сферу в коллекцию для удаления
		DestroyImmediate(this.renderer.material.mainTexture);
		TextureUtils.DeletedCounter++;
	}
}

