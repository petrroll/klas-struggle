using UnityEngine;

namespace Assets.Scripts
{
    class Answer : MonoBehaviour
    {
        public Decision Decision;
        public SelectedAnswer SelectedAnswer;

        public void OnCollisionEnter2D(Collision2D collision)
        {
            Decision.Decide(SelectedAnswer);
        }
    }
}
