using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    abstract class Decision : MonoBehaviour
    {
        internal WheatController Controller;
        internal Stage CurrentStage;

        public virtual void Decide(int questionID, int selectedAnswerID)
        {
            Debug.Assert(CurrentStage != null && Controller != null, "Stage not initialized");
            CurrentStage.FinishStage();
        }


        public void Init(Stage stage, WheatController controller)
        {
            this.CurrentStage = stage;
            this.Controller = controller;

            Debug.Assert(Verify(CurrentStage), "Unable to verify decision.");
        }

        protected abstract bool Verify(Stage stage);
    }
}
