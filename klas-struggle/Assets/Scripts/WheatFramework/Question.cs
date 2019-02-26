using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class Question : MonoBehaviour
    {
        internal Answer[] Answers;

        // Id identifying current question within a stage & its decision script, set by stage.
        internal int QuestionID = -1;
        internal Decision Decision;

        public void Init(int questionID, Decision dec)
        {
            gameObject.SetActive(false);

            QuestionID = questionID;
            Decision = dec;

            InitAnswers();
        }

        private void InitAnswers()
        {
            // assumes the order of retrieved components is the same as the order in Editor
            Answers = GetComponentsInChildren<Answer>(true);

            // Inits ids of all answers, must be called only after QuestionID and Decision has already been itialized.
            for (int i = 0; i < Answers.Length; i++)
            {
                Answers[i].Init(QuestionID, i, Decision);
            }
        }
    }
}
