namespace Assets.Scripts.Decisions
{
    class Size : Decision
    {
        public override void Decide(SelectedAnswer answer)
        {
            Controller.Size = (answer == SelectedAnswer.A) ? 10 : 5;
            base.Decide(answer);
        }

    }
}
