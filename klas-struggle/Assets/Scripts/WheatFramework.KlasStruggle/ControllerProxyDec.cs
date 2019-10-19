using Assets.Scripts.KlasStruggle.Wheat;
using System.Threading.Tasks;

namespace Assets.Scripts.WheatFramework.KlasStruggle
{
    class ControllerProxyDec : Decision
    {
        public WheatController Controller;

        public async override Task Decide(Answer answer)
        {
            Controller.ApplyDecision(answer);
            await base.Decide(answer);
        }

        protected override bool Verify(QuestionStage stage) => true;
    }
}
