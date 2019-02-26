using UnityEngine;

namespace Assets.Scripts.Decisions
{
    class Position : Decision
    {
        public override void Decide(int questionID, int selectedAnswerID)
        {
            Controller.Position = (selectedAnswerID == 0) ? new Vector3(0, 5) : new Vector3(0, -5);
            base.Decide(questionID, selectedAnswerID);
        }

        protected override bool Verify(Stage stage) => true;

    }
}
