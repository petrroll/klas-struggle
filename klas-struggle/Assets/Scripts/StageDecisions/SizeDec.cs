namespace Assets.Scripts.Decisions
{
    class SizeDec : Decision
    {
        public override void Decide(int questionID, int selectedAnswerID)
        {
            Controller.Size = (selectedAnswerID == 0) ? 10 : 5;
            base.Decide(questionID, selectedAnswerID);
        }

        protected override bool Verify(Stage stage) => true;
    }
}
