using Assets.Scripts.KlasStruggle.Persistent;
using Assets.Scripts.KlasStruggle.Wheat;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.WheatFramework.KlasStruggle
{
    class FinalStage : Stage
    {
        public WheatController Controller = null;
        public string NextScene = "field";

        public override void ActivateStage()
        {
            // Saves generated wheat into a static state storage & loads next scene
            GameController.Get.DataStorage.GeneratedWheatState = Controller.State;
            SceneManager.LoadScene(NextScene);
        }

        public override Task FinishStageAsync()
        {
            return Task.CompletedTask;
        }
    }
}
