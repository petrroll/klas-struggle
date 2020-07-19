using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.WheatFramework
{
    class Answer : MonoBehaviour
    {
        // decision script assigned to this answer (inherited from stage -> question)
        internal Decision Decision;

        // Id that identifies the answer within current question and question.
        internal int Id = -1;
        internal Question Question;

        internal KeyCode AnswerKey;

        public void OnMouseDown() => SelectCurrentAnswer();
        public void OnCollisionEnter2D(Collision2D _) => SelectCurrentAnswer();

        [Tooltip("Overrides the fade-out animation started by QuestionStage.FinishStageAsync that is shared among all (even non-selected) answers.")]
        public bool CustomSelectedAnimation = false;
        
        public void Update()
        {
            if (Input.GetKeyDown(AnswerKey))
            {
                SelectCurrentAnswer();
            }
        }

        public void Init(Question question, int answerID, Decision dec)
        {
            Debug.Assert(dec != null);

            Question = question;
            Id = answerID;

            Decision = dec;
            InitAnswerKey(answerID);
        }


        private void SelectCurrentAnswer()
        {
            Debug.Assert(Question.Id != -1 && Id != -1, $"Answers selected but not initialized Q:{Question.Id}, A:{Id}.");
            if (Question.Stage.ReadyForAnswers) { _ = Decision.DecideAsync(this); } // Selects answer only when current stage is ready for answers (e.g. is not finishing).

            if (CustomSelectedAnimation)
            {
                // FIX (ugly hack): The Decision.DecideAsync->CurrentStage.FinishStageAsync->gameObject.DOFadeChildrenTextsAndSprites(0, ..) starts DOFade for all children
                // ..including the selected answer. If we want to override that and provide shorter/longer/different animation for the text of the selected answer than what
                // ..we generally have for the not-selected ones we need to kill all of its tweens first & then start new ones.
                int killedTweens = this.gameObject.DOKillInChildrenTextAndSprites();
                Debug.Assert(killedTweens != 1, $"Selected answer animation is replacing way too many/zero previously running tweens: {killedTweens}.");
                // ... implement custom animation (e.g. slower fadout, ...)
            }
        }

        private void InitAnswerKey(int answerID)
        {
            switch (answerID)
            {
                case 0:
                    AnswerKey = KeyCode.LeftArrow;
                    break;
                case 1:
                    AnswerKey = KeyCode.RightArrow;
                    break;
                case 3:
                    AnswerKey = KeyCode.UpArrow;
                    break;
                case 4:
                    AnswerKey = KeyCode.PageDown;
                    break;
            }
        }

    }
}
