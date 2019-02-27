using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

[System.Serializable]
public class WheatState
{
    public int Checkmarks = -1;
    public int DropDowns = 1;

    public float Size = 2.5f;
}

public class WheatController : MonoBehaviour
{
    public List<GameObject> Checkmarks;
    public List<GameObject> DropDowns;

    public WheatState State = new WheatState();
    public bool InitOnStart = true;
    private bool _inited = false;

    internal void ApplyDecision(Answer answer)
    {
        switch (answer.Question.Stage.Id)
        {
            case 0:
                State.Size = answer.Id == 0 ? 5 : 10;
                ApplySize();
                break;
            case 1:
                State.Checkmarks = answer.Id + answer.Question.Id * 2;
                ApplyCheckMarks();
                break;
            case 2:
                State.DropDowns = answer.Id;
                ApplyDropDowns();
                break;
            default:
                Debug.Assert(false, "Unreachable.");
                break;

        }
    }

    public void ApplyState()
    {
        ApplySize();
        ApplyCheckMarks();
        ApplyDropDowns();
    }


    void SetActiveObject(List<GameObject> objects, int activeObject)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(i == activeObject);
        }
    }

    void Start()
    {
        if (InitOnStart) { InitAndEnable(); }
        else { gameObject.SetActive(false); }
    }

    public void InitAndEnable()
    {
        if(_inited) { return; }
        _inited = true;

        ApplyState();
        gameObject.SetActive(true);
    }

    void ApplySize()
    {
        transform.localScale = new Vector3(State.Size, State.Size);
    }

    void ApplyCheckMarks()
    {
        SetActiveObject(Checkmarks, State.Checkmarks);
    }

    void ApplyDropDowns()
    {
        SetActiveObject(DropDowns, State.DropDowns);
    }

}
