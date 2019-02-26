﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class Question : MonoBehaviour
    {
        internal Answer[] Answers;

        // Id identifying current question within a stage & its stage.
        internal int Id = -1;
        internal Stage Stage;

        public void Init(Stage stage, int questionID, Decision dec)
        {
            gameObject.SetActive(false);

            Id = questionID;
            Stage = stage;

            InitAnswers(dec);
        }

        private void InitAnswers(Decision dec)
        {
            // assumes the order of retrieved components is the same as the order in Editor
            Answers = GetComponentsInChildren<Answer>(true);

            // Inits ids of all answers, must be called only after QuestionID and Decision has already been itialized.
            for (int i = 0; i < Answers.Length; i++)
            {
                Answers[i].Init(this, i, dec);
            }
        }
    }
}
