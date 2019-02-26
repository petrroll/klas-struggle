using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class Question : MonoBehaviour
    {
        public List<Answer> Answers;

        // Id identifying current question within a stage & its decision script, set by stage.
        internal int QuestionID = -1;
        internal Decision Decision = null;

        public void Init(int questionID, Decision dec)
        {
            QuestionID = questionID;
            Decision = dec;

            InitAnswers();
        }

        private void InitAnswers()
        {
            // Inits ids of all answers, must be called only after QuestionID and Decision has already been itialized.
            for (int i = 0; i < Answers.Count; i++)
            {
                Answers[i].Init(QuestionID, i, Decision);
            }

            Debug.Assert(VerifyAnswersAssignment(), $"Not all answers in Q:{QuestionID} have been assigned.");
        }

        // Verify all children answers have been asigned to Answers array (naively, just checks counts)
        private bool VerifyAnswersAssignment() => Answers.Count == GetComponentsInChildren<Answer>().Length;

    }
}
