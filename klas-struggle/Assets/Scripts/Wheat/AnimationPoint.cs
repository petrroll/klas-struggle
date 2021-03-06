﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.KlasStruggle.Wheat
{
    public class AnimationPoint : MonoBehaviour
    {
        public float AnimTime;
        public Sprite[] SpriteList;

        public void StartAnimation()
        {
            StartCoroutine(nameof(Animate));
        }

        IEnumerator Animate()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            int currState = 0;

            while(currState < SpriteList.Length)
            {
                spriteRenderer.sprite = SpriteList[currState];
                yield return new WaitForSeconds(AnimTime);

                currState++;
            }
        }
    }
}
