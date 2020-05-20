using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMotionScript : MonoBehaviour
{
    public Vector3 Step1;
    public Vector3 Step2;
    public Vector3 Step3;
    public Vector3 Step4;
    public float StepLength;
    public bool continueCoroutine;
    private int CoroutineStep = 0;
    // Start is called before the first frame update
    // Update is called once per frame
    void MotionStepMake(Vector3 Step)
    {
        transform.position = Step;    
    
    }

    public IEnumerator MakeMotion()
    {
        while (continueCoroutine)
        {
            CoroutineStep++;
            if (CoroutineStep == 1) { MotionStepMake(Step1); }
            else if (CoroutineStep == 2) { MotionStepMake(Step2); }
            else if (CoroutineStep == 3) { MotionStepMake(Step3); }
            else if (CoroutineStep == 4) { MotionStepMake(Step4); }
            else if (CoroutineStep == 5) { continueCoroutine = false; }
            
            yield return new WaitForSeconds(StepLength);
        }
    }

}
