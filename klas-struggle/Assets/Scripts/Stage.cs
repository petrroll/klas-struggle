using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class Stage : MonoBehaviour
    {
        public bool ActiveOnStart = false;

        public List<Question> Questions;
        public WheatController Controller;
        public Decision Dec;
        public Stage NextStage;

        public void ActivateStage()
        {
            Debug.Assert(Questions.Count > 0);

            // activate current stage object
            this.gameObject.SetActive(true);
            
            // Randomly select a question (question) from current stage and activate it's gameObject (answers, ...)
            {
                int i = UnityEngine.Random.Range(0, Questions.Count - 1);
                var selectedDecisions = Questions[i];

                selectedDecisions.gameObject.SetActive(true);
            }

        }

        public void FinishStage()
        {
            // disable current stage's gameObject ane activate next stage 
            this.gameObject.SetActive(false);
            NextStage?.ActivateStage();
        }

        public void Start()
        {
            Init();

            // Potentially activate a question from current stage
            if (!ActiveOnStart) { gameObject.SetActive(false); }
            else { ActivateStage(); }
        }

        private void Init()
        {
            Debug.Assert(Dec != null);
            Debug.Assert(Controller != null);

            // Initialize questions & subsequently their answers 
            for (int i = 0; i < Questions.Count; i++)
            {
                Questions[i].Init(i, Dec);
            }

            // Initialize decision
            Dec.Init(this, Controller);

            Debug.Assert(VerifyQuestionsAssignment(), $"Not all questions in current stage have been assigned.");
        }

        // Verify all children questions have been asigned to Questions array (naively, just checks counts)
        private bool VerifyQuestionsAssignment() => Questions.Count == GetComponentsInChildren<Question>().Length;

    }
}
