namespace Assets.Scripts.Decisions
{
    class Size : Decision
    {
        public override void Decide(SelectedAnswer answer)
        {
            Wheat.Size = (answer == SelectedAnswer.A) ? 10 : 5;
        }

    }
}
