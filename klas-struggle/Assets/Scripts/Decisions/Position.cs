using UnityEngine;

namespace Assets.Scripts.Decisions
{
    class Position : Decision
    {
        public override void Decide(SelectedAnswer answer)
        {
            Controller.Position = (answer == SelectedAnswer.A) ? new Vector3(0, 5) : new Vector3(0, -5);
            base.Decide(answer);
        }

    }
}
