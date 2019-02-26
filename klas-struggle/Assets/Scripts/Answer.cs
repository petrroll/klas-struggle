using UnityEngine;

namespace Assets.Scripts
{
    class Answer : MonoBehaviour
    {
        // decision script assigned to this answer (inherited from stage -> question)
        internal Decision Decision;

        // Ids that identifies the answer within its decision script, set by Question
        internal int QuestionId = -1;
        internal int AnswerId = -1;

        internal KeyCode AnswerKey;

        public void OnCollisionEnter2D(Collision2D collision) => SelectCurrentAnswer();
        public void Update()
        {
            if (Input.GetKeyDown(AnswerKey))
            {
                SelectCurrentAnswer();
            }
        }

        public void Init(int questionID, int answerID, Decision dec)
        {
            Debug.Assert(dec != null);

            QuestionId = questionID;
            AnswerId = answerID;

            Decision = dec;
            InitAnswerKey(answerID);
        }


        private void SelectCurrentAnswer()
        {
            Debug.Assert(QuestionId != -1 && AnswerId != -1, $"Answers selected but not initialized Q:{QuestionId}, A:{AnswerId}.");
            Decision.Decide(QuestionId, AnswerId);
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
