﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    public static void DOFadeChildrenSprites(this GameObject go, float startValue, float endValue, float duration)
    {
        var sprites = go.GetComponentsInChildren<SpriteRenderer>(true);
        
        foreach(var sprite in sprites)
        {
            sprite.SetFade(startValue);
            sprite.DOFade(endValue, duration);
        }
    }

    public static Tweener DOFade(this TextMeshPro target, float endValue, float duration)
    {
        return DOTween.To(() => target.alpha, x => target.alpha = x, endValue, duration).SetTarget(target);
    }

    public static void SetFade(this TextMeshPro target, float value) => target.alpha = value;

    public static void DOFadeChildrenTexts(this GameObject go, float startValue, float endValue, float duration)
    {
        var objs = go.GetComponentsInChildren<TextMeshPro>(true);

        foreach (var obj in objs)
        {
            obj.SetFade(startValue);
            obj.DOFade(endValue, duration);
        }
    }
}