using System;
using System.Collections;
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
        [SerializeField] private AnimateTransform transformValueToAnimate;
        public Action OnFrameFinished;

        public float FrameTime => frameTime;

        public IEnumerator PlayAnimation(Transform target, bool reverse = false)
        {
            float t = 0.0f;
            WaitForEndOfFrame wait = new();

            while (t < frameTime)
            {
                t += Time.deltaTime;

                AnimateTransform(target, curve.Evaluate(t / frameTime), reverse);

                yield return wait;
            }

            OnFrameFinished?.Invoke();
        }

        private void AnimateTransform(Transform target, float t, bool reverse = false)
        {
            Vector3 fromToAnimate = reverse ? to : from;
            Vector3 toToAnimate = reverse ? from : to;

            switch (transformValueToAnimate)
            {
                case Game.Enums.AnimateTransform.Position:
                {
                    target.position = Vector3.Lerp(fromToAnimate, toToAnimate, t);
                    break;
                }
                case Game.Enums.AnimateTransform.Scale:
                {
                    target.localScale = Vector3.Lerp(fromToAnimate, toToAnimate, t);
                    break;
                }
                case Game.Enums.AnimateTransform.Rotation:
                {
                    target.localEulerAngles = Vector3.Lerp(fromToAnimate, toToAnimate, t);
                    break;
                }
            }
        }
    }
}