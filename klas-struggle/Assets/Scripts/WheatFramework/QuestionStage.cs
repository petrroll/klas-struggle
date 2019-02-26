﻿using Assets.Scripts.StageDecisions;
using Assets.Scripts.WheatFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class QuestionStage : Stage
    {
        public bool ActiveOnStart = false;
        public Stage NextStage;

        internal Question[] Questions;

        internal Decision Decision;


        public override void ActivateStage()
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

        public override void FinishStage()
        {
            // disable current stage's gameObject ane activate next stage 
            this.gameObject.SetActive(false);
            NextStage?.ActivateStage();
        }


        internal override void Init(int stageId)
        {
            base.Init(stageId);

            Decision = GetComponent<Decision>();
            Debug.Assert(Decision != null);

            // assumes the order of retrieved components is the same as the order in Editor
            Questions = GetComponentsInChildren<Question>(true);

            // Initialize questions & subsequently their answers 
            for (int i = 0; i < Questions.Length; i++)
            {
                Questions[i].Init(this, i, Decision);
            }

            // Initialize decision
            Decision.Init(this);

            // Potentially activate a question from current stage
            if (!ActiveOnStart) { gameObject.SetActive(false); }
            else { ActivateStage(); }
        }
    }
}
