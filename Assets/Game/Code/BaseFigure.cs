using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Game.Code
{
    public abstract class BaseFigure : MonoBehaviour
    {
        /// <summary>
        /// Начальные координаты объекта
        /// </summary>
        protected Vector3 _startCoordinates;
		
		protected float _startTime = 0;

        /// <summary>
        /// Размер фигуры (для сфер это радиус)
        /// </summary>
        protected float _figureSize;

        /// <summary>
        /// Кол-во очков за лопанье объекта
        /// </summary>
        protected int _pointsCount;
        public int PointsCount
        {
            get { return _pointsCount; }
        }

        /// <summary>
        /// Скорость движения
        /// </summary>
        protected float _figureSpeed;

        /// <summary>
        /// Коэффициент скорости, будет увеличиваться (для повышения сложности). Зависит от общего кол-ва набранных очков.
        /// </summary>
        protected abstract float BaseSpeedCoefficient
        {
            get;
        }

        /// <summary>
        /// Генерация скорости объекта (зависит от размера)
        /// </summary>
        protected virtual void GenerateSpeed(float distance)
        {
            if (distance > 0 && this._figureSize > 0 && this.BaseSpeedCoefficient >= 1)
            {
                this._figureSpeed = ((Time.time - this._startTime) * this.BaseSpeedCoefficient) / (this._figureSize * 2 * distance); //чем больше фигура, тем меньше скорость
            }
            else
            {
                this._figureSpeed = 0; //здесь предполагается обработка ошибки (неправильные входные данные)
            }
        }

        /// <summary>
        /// Генерируем кол-во очков (зависит от размера)
        /// </summary>
        protected virtual void GeneratePoints()
        {
            if (this._figureSize > 0)
                this._pointsCount = (int)(150 / this._figureSize);
            else
            {
                //безразмерная фигура, обработка ошибки
            }
        }

        /// <summary>
        /// Рандомно выбираем сторону (для сфер это радиус)
        /// </summary>
        protected virtual void GenerateMainSize()
        {
            this._figureSize = (float)((UnityEngine.Random.Range(10, 50) / 10) * 0.5);
        }

        /// <summary>
        /// Назначаем нашей фигуре рандомный цвет
        /// </summary>
        protected virtual void GenerateColor()
        {
            
        }

        /// <summary>
        /// Устанавливаем начальные координаты объекта
        /// </summary>
        protected virtual void SetStartCoordinates()
        {
 
        }

        /// <summary>
        /// Двигаем объект вниз
        /// </summary>
        public virtual void MoveDown()
        {
            
        }
		
    }
}
