using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.WheatFramework
{
    class FinalStage : Stage
    {
        public WheatController Controller;


        public override void ActivateStage()
        {
            // Saves generated wheat into a static state storage & loads next scene
            DataStorage.DS.State = Controller.State;
            SceneManager.LoadScene("field");
        }

        public override void FinishStage()
        {
        }
    }
}
