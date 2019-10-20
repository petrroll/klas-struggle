using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.WheatFramework
{
    abstract class Stage : MonoBehaviour
    {
        internal int Id { get; private set; }

        internal virtual void Init(int stageId)
        {
            Id = stageId;
        }

        public abstract void ActivateStage();
        public abstract Task FinishStageAsync();
    }
}
