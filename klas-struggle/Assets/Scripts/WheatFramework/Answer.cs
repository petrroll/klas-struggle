using UnityEngine;

namespace Assets.Scripts
{
    class Answer : MonoBehaviour
    {
        // decision script assigned to this answer (inherited from stage -> question)
        internal Decision Decision;

        // Id that identifies the answer within current question and question.
        internal int Id = -1;
        internal Question Question;

        internal KeyCode AnswerKey;

        public void OnCollisionEnter2D(Collision2D collision) => SelectCurrentAnswer();
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
            Decision.Decide(this);
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
