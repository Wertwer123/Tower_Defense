using System;
using Manager;
using UnityEngine;

namespace Interfaces
{
    /// <summary>
    /// An interface that can be applied to everything that works in ticks and will be handled by the ticking manager
    /// something like a building that produces resources or a tower that emits waves of fire at a certain intervall
    /// or a status effect that needs to tick basically everything working in intervalls
    /// </summary>
    public interface ITickable
    {
        public float TickInterval { get; }
        public bool IsReadyForRemoval { get; } 
        public Action OnTickFinishedCallback{get; set; }
        float CurrentTickTime { get; set; }
        
        public bool IsTickFinished();
        public void Tick(float deltaTime, TickManager tickManager);
        public void OnTickFinished();
    }
}