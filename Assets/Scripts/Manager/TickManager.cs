using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using Interfaces;
using Debug = UnityEngine.Debug;
using Task = System.Threading.Tasks.Task;

namespace Manager
{
    public class TickManager
    {
        private readonly List<ITickable> _registeredTickables = new List<ITickable>();
        private readonly object _locker = new object();
        private bool _processTicks = true;
        private readonly ConcurrentQueue<Action> _onTickCompletedMainThread = new ConcurrentQueue<Action>();
        
        private double _deltaTime;
        private double currentTime;
        private double lastTickTime;
        private Stopwatch _stopwatch = new Stopwatch();
        
        public void Update()
        {
            while (_onTickCompletedMainThread.TryDequeue(out Action finishedTick))
            {
                finishedTick.Invoke();
            }
        }

        public void AddTickFinishedCallback(Action action)
        {
            _onTickCompletedMainThread.Enqueue(action);
        }
        
        public void StartUpdatingTickables()
        {
            _stopwatch.Start();
            Task.Run(UpdateTickables);
        }

        void UpdateTickables()
        {
            while (_processTicks)
            {
                currentTime = _stopwatch.Elapsed.TotalSeconds;
                _deltaTime = currentTime - lastTickTime;
                lastTickTime = currentTime;
                
                foreach (var tickable in _registeredTickables)
                {
                    if (!tickable.IsReadyForRemoval)
                    {
                        tickable.Tick((float)_deltaTime, this);
                    }
                    else
                    {
                        DeregisterTickable(tickable);
                    }
                }

                System.Threading.Thread.Sleep(1);
            }
        }

        public void StopTicking()
        {
            _processTicks = false;
            _stopwatch.Reset();
        }

        /// <summary>
        /// locks the tickables list and removes the tickable from it
        /// </summary>
        /// <param name="tickable"></param>
        public void DeregisterTickable(ITickable tickable)
        {
            lock (_locker)
            {
                _registeredTickables.Remove(tickable);
            }
        }
        
        /// <summary>
        /// locks the tickables list and adds the tickable to it
        /// </summary>
        /// <param name="tickable"></param>
        public void RegisterTickable(ITickable tickable)
        {
            lock(_locker)
            {
                _registeredTickables.Add(tickable);
            }
        }
    }
}