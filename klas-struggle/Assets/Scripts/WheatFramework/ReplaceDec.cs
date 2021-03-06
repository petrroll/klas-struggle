﻿using Assets.Scripts.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.WheatFramework
{
    class ReplaceDec : Decision
    {

        public GameObject ObjectToReplace = null;
        public List<GameObjList> Replacements = new List<GameObjList>();

        public override async Task DecideAsync(Answer answer)
        {
            int questionID = answer.Question.Id;
            int selectedAnswerID = answer.Id;

            ObjectToReplace?.SetActive(false);
            Replacements[questionID][selectedAnswerID].SetActive(true);

            await base.DecideAsync(answer);
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
