using System;
using System.Collections;
using System.Collections.Generic;
using Game.Enums;
using UnityEngine;

namespace SpriteAnimation
{
    [Serializable]
    public class SpriteAnimationFrame
    {
        [SerializeField] private float frameTime;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private Vector3 from = Vector3.one;
        [SerializeField] private Vector3 to;
        [SerializeField] private List<AnimateTransform> transformValuesToAnimate;
        
        public float FrameTime => frameTime;
        public Action OnFrameFinished;

        public IEnumerator PlayAnimation(Transform target)
        {
            float t = 0.0f;
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            
            while (t < frameTime)
            {
                t += Time.deltaTime;
                
                AnimateTransform(target, curve.Evaluate(t / frameTime));
                
                yield return wait;
            }
            
            OnFrameFinished?.Invoke();
        }

        void AnimateTransform(Transform target, float t)
        {
            foreach (var animatedTransformValue in transformValuesToAnimate)
            {
                switch (animatedTransformValue)
                {
                    case Game.Enums.AnimateTransform.Position:
                    {
                        target.position = Vector3.Lerp(from, to, t);
                        break;
                    }
                    case Game.Enums.AnimateTransform.Scale:
                    {
                        target.localScale = Vector3.Lerp(from, to, t);
                        break;
                    }
                    case Game.Enums.AnimateTransform.Rotation:
                    {
                        target.localEulerAngles = Vector3.Lerp(from, to, t);
                        break;
                    }
                }
            }
        }
    }
    
    
}