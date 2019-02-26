using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.StageDecisions
{
    class ReplaceDec : Decision
    {

        public GameObject ObjectToReplace = null;
        public List<GameObjList> Replacements;

        public override void Decide(Answer answer)
        {
            int questionID = answer.Question.Id;
            int selectedAnswerID = answer.Id;

            ObjectToReplace?.SetActive(false);
            Replacements[questionID][selectedAnswerID].SetActive(true);

            base.Decide(answer);
        }

        protected override bool Verify(QuestionStage stage)
        {
            // verifies the number of replacements equals to number of all potential answers
            if (stage.Questions.Length != Replacements.Count) { return false; }
            for (int i = 0; i < stage.Questions.Length; i++)
            {
                if(stage.Questions[i].Answers.Length != Replacements[i].Count) { return false; }
            }

            return true;
        }
    }
}
