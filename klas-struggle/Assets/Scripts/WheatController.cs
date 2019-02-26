using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

[System.Serializable]
public class WheatState
{
    public float Size = 2.5f;
}

public class WheatController : MonoBehaviour
{
    public WheatState State = new WheatState();

    private void ApplySize()
    {
        transform.localScale = new Vector3(State.Size, State.Size);
    }

    internal void ApplyDecision(Answer answer)
    {
        if(answer.Question.Stage.Id == 0)
        {
            State.Size = answer.Id == 0 ? 5 : 10;
            ApplySize();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ApplySize();
    }
}
