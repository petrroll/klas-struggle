using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;

public class FadeOutTMP : MonoBehaviour
{
    
    
    
    // Start is called before the first frame upda q        te
    void Update()
    {
        if (Input.anyKeyDown)
        {
        GetComponent<TextMeshProUGUI>().DOFade(0f,3f);
        }
    }

}
