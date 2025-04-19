using System;
using UnityEngine;

namespace SpriteAnimation
{
    /// <summary>
    ///     this scriptable object needs to be instanced to be used
    /// </summary>
    [CreateAssetMenu(fileName = "New Sprite Animation", menuName = "Sprite Animation")]
    public class SpriteAnimTemplate : ScriptableObject
    {
        [SerializeField] private SpriteAnimationFrame[] frames = new SpriteAnimationFrame[0];
        [SerializeField] private bool loop;

        [SerializeField] [Tooltip("-1 means it loops endlessly")] [Min(-1)]
        private int loopTimes;

        private Coroutine _currentAnimationCoroutine;
        private int _currentFrame;

        private MonoBehaviour _owningComponent;
        private int loopCount;

        private void OnEnable()
        {
            InitAnimation();
        }


        private void OnValidate()
        {
            loopCount = 0;
            _currentFrame = 0;

            foreach (SpriteAnimationFrame animationFrame in frames) animationFrame.OnFrameFinished = null;
        }

        public event Action OnAnimationFinished;

        public void PlayAnimation(MonoBehaviour target, bool reverse = false)
        {
            _owningComponent = target;

            if (_currentAnimationCoroutine != null) return;

            _currentAnimationCoroutine =
                target.StartCoroutine(frames[_currentFrame].PlayAnimation(target.transform, reverse));
        }

        public void StopAnimation()
        {
            _owningComponent.StopCoroutine(_currentAnimationCoroutine);
        }

        private void InitAnimation()
        {
            foreach (SpriteAnimationFrame animationFrame in frames) animationFrame.OnFrameFinished += ContinueFrame;
        }

        private void ContinueFrame()
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
                StopAnimation();
                OnAnimationFinished?.Invoke();
            }
        }
    }
}