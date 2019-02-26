using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.StageDecisions
{
    class ControllerProxyDec : Decision
    {
        public WheatController Controller;

        public override void Decide(Answer answer)
        {
            Controller.ApplyDecision(answer);
            base.Decide(answer);
        }

        protected override bool Verify(QuestionStage stage) => true;
    }
}
