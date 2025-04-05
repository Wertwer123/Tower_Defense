using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteAnimation
{
    /// <summary>
    /// this scriptable object needs to be instanced to be used
    /// </summary>
    [CreateAssetMenu(fileName = "New Sprite Animation", menuName = "Sprite Animation")]
    public class SpriteAnimTemplate : ScriptableObject
    {
        [SerializeField] SpriteAnimationFrame[] frames;
        [SerializeField] bool loop;
        [SerializeField, Tooltip("-1 means it loops endlessly"), Min(-1)] int loopTimes;

        public event Action OnAnimationFinished;
        
        private MonoBehaviour _owningComponent;
        private Coroutine _currentAnimationCoroutine;
        private int _currentFrame = 0;
        private int loopCount = 0;
        
        
        private void OnValidate()
        {
            loopCount = 0;
            _currentFrame = 0;
            
            foreach (var animationFrame in frames)
            {
                animationFrame.OnFrameFinished = null;
            }
        }

        private void OnEnable()
        {
            InitAnimation();
        }

        public void PlayAnimation(MonoBehaviour target)
        {
            _owningComponent = target;
            
            if (_currentAnimationCoroutine != null)
            {
                return;
            }
            Debug.Log(_currentFrame);
            _currentAnimationCoroutine = target.StartCoroutine(frames[_currentFrame].PlayAnimation(target.transform));
        }

        public void StopAnimation()
        {
            _owningComponent.StopCoroutine(_currentAnimationCoroutine);
        }
        
        void InitAnimation()
        {
            foreach (var animationFrame in frames)
            {
                animationFrame.OnFrameFinished += ContinueFrame;
            }
        }

        void ContinueFrame()
        {
            _currentFrame++;

            if (_currentFrame > frames.Length - 1 && loop && (loopCount < loopTimes || loopTimes == -1))
            {
                _currentFrame = 0;
                _currentAnimationCoroutine = null;
                PlayAnimation(_owningComponent);
                loopCount++;
            }
            else if (_currentFrame <= frames.Length - 1)
            {
                _currentAnimationCoroutine = null;
                PlayAnimation(_owningComponent);
            }
            else
            {
                OnAnimationFinished?.Invoke();
                StopAnimation();
            }
        }
    }
}
