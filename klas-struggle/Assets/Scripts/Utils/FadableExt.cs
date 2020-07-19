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

        public static void SetFadeChildrenSprites(this GameObject go, float value, bool includeInactive)
        {
            var sprites = go.GetComponentsInChildren<SpriteRenderer>(includeInactive);

            foreach (var sprite in sprites)
            {
                sprite.SetFade(value);
            }
        }

        public static void DOFadeChildrenSprites(this GameObject go, float endValue, float duration, bool includeInactive)
        {
            var sprites = go.GetComponentsInChildren<SpriteRenderer>(includeInactive);

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

        public static void DOFadeChildrenTexts(this GameObject go, float endValue, float duration, bool includeInactive)
        {
            var objs = go.GetComponentsInChildren<TextMeshPro>(includeInactive);

            foreach (var obj in objs)
            {
                obj.DOFade(endValue, duration);
            }
        }

        public static void SetFadeChildrenTexts(this GameObject go, float value, bool includeInactive)
        {
            var objs = go.GetComponentsInChildren<TextMeshPro>(includeInactive);
            foreach (var obj in objs)
            {
                obj.SetFade(value);
            }
        }

        public static void DOFadeChildrenTextsAndSprites(this GameObject go, float endValue, float duration, bool includeInactive = true)
        {
            DOFadeChildrenTexts(go, endValue, duration, includeInactive);
            DOFadeChildrenSprites(go, endValue, duration, includeInactive);
        }

        public static void SetFadeChildrenTextsAndSprites(this GameObject go, float value, bool includeInactive = true)
        {
            SetFadeChildrenTexts(go, value, includeInactive);
            SetFadeChildrenSprites(go, value, includeInactive);
        }

        public static int DOKillInChildrenTextAndSprites(this GameObject go, bool includeInactive = true)
        {
            int killedTweens = 0;

            var tmpObjs = go.GetComponentsInChildren<TextMeshPro>(includeInactive);
            foreach (var tmp in tmpObjs)
            {
                killedTweens += DOTween.Kill(tmp);
            }

            var spriteObjs = go.GetComponentsInChildren<SpriteRenderer>(includeInactive);
            foreach (var sprite in spriteObjs)
            {
                killedTweens += DOTween.Kill(sprite);
            }

            return killedTweens;
        }
    }
}