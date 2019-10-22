using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class FadableExt
    {
        public static Tweener DOFade(this SpriteRenderer target, float endValue, float duration)
        {
            return DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration).SetTarget(target);
        }

        public static void SetFade(this SpriteRenderer target, float value)
        {
            var color = target.color;
            color.a = value;
            target.color = color;
        }

        public static void SetFadeChildrenSprites(this GameObject go, float value)
        {
            var sprites = go.GetComponentsInChildren<SpriteRenderer>(true);

            foreach (var sprite in sprites)
            {
                sprite.SetFade(value);
            }
        }

        public static void DOFadeChildrenSprites(this GameObject go, float endValue, float duration)
        {
            var sprites = go.GetComponentsInChildren<SpriteRenderer>(true);

            foreach (var sprite in sprites)
            {
                sprite.DOFade(endValue, duration);
            }
        }

        public static Tweener DOFade(this TextMeshPro target, float endValue, float duration)
        {
            return DOTween.To(() => target.alpha, x => target.alpha = x, endValue, duration).SetTarget(target);
        }

        public static void SetFade(this TextMeshPro target, float value) => target.alpha = value;

        public static void DOFadeChildrenTexts(this GameObject go, float endValue, float duration)
        {
            var objs = go.GetComponentsInChildren<TextMeshPro>(true);

            foreach (var obj in objs)
            {
                obj.DOFade(endValue, duration);
            }
        }

        public static void SetFadeChildrenTexts(this GameObject go, float value)
        {
            var objs = go.GetComponentsInChildren<TextMeshPro>(true);
            foreach (var obj in objs)
            {
                obj.SetFade(value);
            }
        }

        public static void DOFadeChildrenTextsAndSprites(this GameObject go, float endValue, float duration)
        {
            DOFadeChildrenTexts(go, endValue, duration);
            DOFadeChildrenSprites(go, endValue, duration);
        }

        public static void SetFadeChildrenTextsAndSprites(this GameObject go, float value)
        {
            SetFadeChildrenTexts(go, value);
            SetFadeChildrenSprites(go, value);
        }
    }
}