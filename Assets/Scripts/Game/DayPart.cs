using System;
using Game.Enums;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class DayPart
    {
        [SerializeField] private Color dayPartStartColor;
        [SerializeField] private Color dayPartEndColor;
        [SerializeField] AnimationCurve dayPartAnimationCurve;
        
        public void InitDayPart(float startTime, float endTime, DayTime dayTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            DayTime = dayTime;
            Length = endTime - startTime;
        }

        public Color GetDayPartColor(float currentlyPassedTime)
        {
            float interpolationValue = dayPartAnimationCurve.Evaluate((currentlyPassedTime - StartTime) / Length);
            return Color.Lerp(dayPartStartColor, dayPartEndColor, interpolationValue);
        }
        
        public DayTime DayTime { get; private set; }
        public float StartTime { get; private set;}
        public float Length { get; private set;}
        public float EndTime { get; private set;}
    }
}