using System;
using Interfaces;
using Manager;
using UnityEngine;

namespace Base
{
    /// <summary>
    /// A wrapper class for the I tickable interface for a generic ticker
    /// </summary>
    [Serializable]
    public class Ticker : ITickable
    {
        [SerializeField, Min(0.0f)] float _tickInterval = 1.0f;
        [SerializeField] bool isReadyForRemoval = false;
        
        public float TickInterval => _tickInterval;
        public bool IsReadyForRemoval => isReadyForRemoval;
        public Action OnTickFinishedCallback { get; set; }
        public float CurrentTickTime { get; set; } = 0.0f;

        public Ticker(float tickInterval, Action onTickFinishedCallback)
        {
            _tickInterval = tickInterval;
            OnTickFinishedCallback = onTickFinishedCallback;
        }
        
        public bool IsTickFinished()
        {
            return CurrentTickTime >= TickInterval;
        }

        public void Tick(float deltaTime, TickManager tickManager)
        {
            CurrentTickTime += deltaTime;
            
            if (IsTickFinished())
            {
                CurrentTickTime = 0;
                tickManager.AddTickFinishedCallback(OnTickFinished);
            }
        }
        
        public void OnTickFinished()
        {
            OnTickFinishedCallback?.Invoke();
        }
    }
}