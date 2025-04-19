using System;
using Base;
using Game;
using Game.Enums;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Manager.Enviroment
{
    /// <summary>
    /// This class manages the day time it hold s events for when certain daytimes hit to which you can subscribe
    /// You can finetune how daytimes look how intense and in which colors the light will shine
    /// </summary>
    public class DayNightCycle : MonoBehaviour
    {
        [Header("Daytime clock")]
        [SerializeField] DayTimeClock dayTimeClock;
        
        [Header("Daytime settings")]
        [SerializeField, Min(5)] int dayLengthInSeconds;
        [SerializeField, Range(0f, 1f)] private float percentageOfMorningOfOneDay = 0.25f;
        [SerializeField, Range(0f, 1f)] private float percentageOfAfternoonOfOneDay = 0.25f;
        [SerializeField, Range(0f, 1f)] private float percentageOfEveningOfOneDay = 0.25f;
        [SerializeField, Range(0f, 1f)] private float percentageOfNightOfOneDay = 0.25f;
        
        [Header("Color settings")]
        [SerializeField] private Light2D sunLight;
        [SerializeField] private DayPart morning;
        [SerializeField] private DayPart afternoon;
        [SerializeField] private DayPart evening;
        [SerializeField] private DayPart night;

        private DayPart _currentDayTimePart;
        private float _passedDayTime = 0.0f;
        
        private void Start()
        {
            dayTimeClock.Init(percentageOfMorningOfOneDay, percentageOfAfternoonOfOneDay, percentageOfEveningOfOneDay);
            CaluclateLengthOfDayTimes();
        }

        private void Update()
        {
            //Increment the time of the day 
            //interpolate the color of the light based on the curve
            UpdateCurrentDayTime();
            InterpolateSunColor();
            dayTimeClock.UpdateDayClock(_passedDayTime / dayLengthInSeconds);
        }

        private void CaluclateLengthOfDayTimes()
        {
            
            float lengthOfMorningInSeconds = dayLengthInSeconds * percentageOfMorningOfOneDay;
            float lengthOfAfternoonInSeconds = dayLengthInSeconds * percentageOfAfternoonOfOneDay;
            float lengthOfEveningInSeconds = dayLengthInSeconds * percentageOfEveningOfOneDay;
            float lengthOfNightInSeconds = dayLengthInSeconds * percentageOfNightOfOneDay;

            morning.InitDayPart(0, lengthOfMorningInSeconds, DayTime.Morning);
            afternoon.InitDayPart(morning.EndTime, morning.EndTime + lengthOfAfternoonInSeconds, DayTime.Afternoon);
            evening.InitDayPart(afternoon.EndTime, afternoon.EndTime + lengthOfEveningInSeconds, DayTime.Evening);
            night.InitDayPart(evening.EndTime, evening.EndTime + lengthOfNightInSeconds, DayTime.Night);

            _currentDayTimePart = morning;
        }

        private void UpdateCurrentDayTime()
        {
            _passedDayTime += Time.deltaTime;

            //Switch the daytime
            if (_passedDayTime >= _currentDayTimePart.EndTime)
            {
                SwitchDaytime();
                Debug.Log($"Its now {_currentDayTimePart.DayTime}");
            }
            
            //Switch the day
            if (_passedDayTime >= dayLengthInSeconds)
            {
                _passedDayTime = 0.0f;
                _currentDayTimePart = morning;
                Debug.Log($"Next day started");
            }
        }

        private void InterpolateSunColor()
        {
            sunLight.color = _currentDayTimePart.GetDayPartColor(_passedDayTime);
        }
        
        private void SwitchDaytime()
        {
            switch (_currentDayTimePart.DayTime)
            {
                case DayTime.Morning:
                {
                    _currentDayTimePart = afternoon;
                    break;
                }
                case DayTime.Afternoon:
                {
                    _currentDayTimePart = evening;
                    break;
                }
                case DayTime.Evening:
                {
                    _currentDayTimePart = night;
                    break;
                }
                case DayTime.Night:
                {
                    _currentDayTimePart = morning;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void SetCurrentDayTime(DayTime dayTime)
        {
            switch (dayTime)
            {
                case DayTime.Morning:
                {
                    _currentDayTimePart = morning;
                    break;
                }

                case DayTime.Afternoon:
                {
                    _currentDayTimePart = afternoon;
                    break;
                }
                case DayTime.Evening:
                {
                    _currentDayTimePart = evening;
                    break;
                }

                case DayTime.Night:
                {
                    _currentDayTimePart = night;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(dayTime), dayTime, null);
            }
            
            _passedDayTime = _currentDayTimePart.StartTime;
        }
    }
}
