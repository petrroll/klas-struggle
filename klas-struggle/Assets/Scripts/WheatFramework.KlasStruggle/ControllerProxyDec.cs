using Assets.Scripts.KlasStruggle.Wheat;
using System.Threading.Tasks;

namespace Assets.Scripts.WheatFramework.KlasStruggle
{
    class ControllerProxyDec : Decision
    {
        public WheatController Controller = null;

        public async override Task DecideAsync(Answer answer)
        {
            Controller.ApplyDecision(answer);
            await base.DecideAsync(answer);
        }

        protected override bool Verify(QuestionStage stage) => true;
    }
}
