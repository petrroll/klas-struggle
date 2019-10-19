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

        public async override Task Decide(Answer answer)
        {
            Controller.ApplyDecision(answer);
            await base.Decide(answer);
        }

        protected override bool Verify(QuestionStage stage) => true;
    }
}
