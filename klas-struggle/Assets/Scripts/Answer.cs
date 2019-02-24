using UnityEngine;

namespace Assets.Scripts
{
    class Answer : MonoBehaviour
    {
        public Decision Decision;
        public SelectedAnswer SelectedAnswer;

        public KeyCode AnswerKey = KeyCode.LeftArrow;

        public void OnCollisionEnter2D(Collision2D collision) => Decision.Decide(SelectedAnswer);
        public void Update()
        {
            if (Input.GetKeyDown(AnswerKey))
            {
                Decision.Decide(SelectedAnswer);
            }
        }
    }
}
