using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    abstract class Decision : MonoBehaviour
    {
        internal Stage CurrentStage;

        public virtual void Decide(Answer answer)
        {
            Debug.Assert(CurrentStage != null, "Stage not initialized");
            Debug.Log($"Decision: S:{answer.Question.Stage.Id}|Q:{answer.Question.Id}|A:{answer.Id}");

            CurrentStage.FinishStage();
        }


        public void Init(Stage stage)
        {
            this.CurrentStage = stage;

            Debug.Assert(Verify(CurrentStage), "Unable to verify decision.");
        }

        protected virtual bool Verify(Stage stage) => true;
    }
}
