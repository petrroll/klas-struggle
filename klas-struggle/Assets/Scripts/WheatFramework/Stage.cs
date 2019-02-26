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
        internal Question[] Questions;
        internal Decision Decision;

        public WheatController Controller;
        public Stage NextStage;

        public void ActivateStage()
        {
            Debug.Assert(Questions.Length > 0);

            // activate current stage object
            this.gameObject.SetActive(true);
            
            // Randomly select a question (question) from current stage and activate it's gameObject (answers, ...)
            {
                int i = UnityEngine.Random.Range(0, Questions.Length);
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
            Decision = GetComponent<Decision>();

            Debug.Assert(Decision != null);
            Debug.Assert(Controller != null);

            // assumes the order of retrieved components is the same as the order in Editor
            Questions = GetComponentsInChildren<Question>(true);

            // Initialize questions & subsequently their answers 
            for (int i = 0; i < Questions.Length; i++)
            {
                Questions[i].Init(i, Decision);
            }

            // Initialize decision
            Decision.Init(this, Controller);
        }
    }
}
