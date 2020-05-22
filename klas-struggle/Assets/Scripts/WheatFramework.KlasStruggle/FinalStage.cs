using Assets.Scripts.AnimsEtc;
using Assets.Scripts.KlasStruggle.Persistent;
using Assets.Scripts.KlasStruggle.Wheat;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.WheatFramework.KlasStruggle
{
    class FinalStage : Stage
    {
        public WheatController Controller = null;
        public StopMotionController StopMotionController = null;
        public string NextScene = "field";

        public float NextSceneDelay = 4f;

        public override void ActivateStage()
        {
            // Saves generated wheat into a static state storage & loads next scene
            GameController.Get.DataStorage.GeneratedWheatState = Controller.State;

            // TODO: This could be done better
            StopMotionController.RunStopMotion();
            Invoke(nameof(LoadSceneMethod), NextSceneDelay);
        }

        private void LoadSceneMethod()
        {
            SceneManager.LoadScene(NextScene);
        }

        public override Task FinishStageAsync()
        {
            return Task.CompletedTask;
        }
    }
}
