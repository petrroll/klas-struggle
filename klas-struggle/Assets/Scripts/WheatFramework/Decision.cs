using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    abstract class Decision : MonoBehaviour
    {
        internal QuestionStage CurrentStage;

        public virtual async Task Decide(Answer answer)
        {
            Debug.Assert(CurrentStage != null, "Stage not initialized");
            Debug.Log($"Decision: S:{answer.Question.Stage.Id}|Q:{answer.Question.Id}|A:{answer.Id}");

            await CurrentStage.FinishStage();
        }

        public void Init(QuestionStage stage)
        {
            this.CurrentStage = stage;

            Debug.Assert(Verify(CurrentStage), "Unable to verify decision.");
        }

        protected virtual bool Verify(QuestionStage stage) => true;
    }
}
