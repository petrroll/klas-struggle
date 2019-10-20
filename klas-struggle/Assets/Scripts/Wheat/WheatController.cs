using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utils;
using Assets.Scripts.WheatFramework;
using UnityEngine;

namespace Assets.Scripts.KlasStruggle.Wheat
{
    public class WheatController : MonoBehaviour
    {
        public List<GameObject> Stage1;
        public List<GameObject> Stage2;
        public List<GameObject> Stage3;
        public List<GameObject> Stage4;

        public bool InitDebugState = false;
        public bool GenerateAsPlayer = false;

        public WheatState State = new WheatState();
        public bool InitOnStart = true;
        private bool _inited = false;

        internal void ApplyDecision(Answer answer)
        {
            switch (answer.Question.Stage.Id)
            {
                case 0:
                    State.Stage1Answer = answer.Id + answer.Question.Id * 2;
                    ApplyStage1State();
                    break;
                case 1:
                    State.Stage2Answer = answer.Id + answer.Question.Id * 2;
                    ApplyStage2State();
                    break;
                case 2:
                    State.Stage3Answer = answer.Id + answer.Question.Id * 2;
                    ApplyStage3State();
                    break;
                case 3:
                    State.Stage4Answer = answer.Id + answer.Question.Id * 2;
                    ApplyStage4State();
                    break;
                case 4:
                    State.Size = answer.Id == 0 ? 1 : 1.2f;
                    ApplySize();
                    break;
                default:
                    Debug.Assert(false, "Unreachable.");
                    break;

            }
        }

        public void ApplyState()
        {
            ApplySize();

            ApplyStage1State();
            ApplyStage2State();
            ApplyStage3State();
            ApplyStage4State();

            ApplyLoc();
        }

        void SetActiveObject(List<GameObject> objects, int activeObject)
        {
            if (activeObject >= 0)
            {
                var newActiveObject = objects[activeObject];
                AllignObjectsPlugWithPreviouslyActiveSocket(newActiveObject);
            }

            // enable activeObject from `objects` (current stage objects) and disable all other ones
            for (int i = 0; i < objects.Count; i++)
            {
                bool activeNow = (i == activeObject);
                bool activePreviously = objects[i].activeSelf;

                objects[i].SetActive(activeNow);

                if (GenerateAsPlayer)
                {
                    if (activeNow && !activePreviously)
                    {
                        objects[i].gameObject.SetFadeChildrenSprites(0);
                        objects[i].gameObject.DOFadeChildrenSprites(1, 3);             // fade new parts in -> set alpha to 0 & slowly move to 1
                    }
                    else if (!activeNow && activePreviously)
                    {
                        objects[i].gameObject.DOFadeChildrenSprites(0, 3); // fade out, should already be visible -> just decrease alpha
                    }

                }

            }
        }

        /// <summary>
        /// If gameObject has ConnectPointPlug, finds associated ConnectPointSocket and aligns them.
        /// </summary>
        private void AllignObjectsPlugWithPreviouslyActiveSocket(GameObject gameObject)
        {
            var connectionPlug = gameObject.GetComponentInChildren<ConnectPointPlug>(false);
            if (connectionPlug != null)
            {

                // Corresponding socket must be in a correct stage, on an active gameObject, must itself be active, and have the 
                // same `.ConnectIndex` as `gameObject`'s plug. Keep the invariant there's always only one such socket.
                var stageWithSocket = GetStageObjects(connectionPlug.SocketStageIndex);
                var activeGameObjsInCorrectStage = stageWithSocket.Where(gobj => gobj.activeSelf);

                foreach (var activeGameObjectInCorrectStage in activeGameObjsInCorrectStage)
                {
                    var potentialSockets = activeGameObjectInCorrectStage.GetComponentsInChildren<ConnectPointSocket>(false);
                    var desiredSocket = potentialSockets.FirstOrDefault(socket => socket.ConnectIndex == connectionPlug.ConnectIndex);

                    if (desiredSocket == null) { continue; }

                    var transformDifference = connectionPlug.transform.position - desiredSocket.transform.position;
                    gameObject.transform.position -= transformDifference;
                }
            }
        }


        void Start()
        {
            if (InitOnStart) { InitAndEnable(); }
            else { gameObject.SetActive(false); }
        }

        public void InitAndEnable()
        {
            if (_inited) { return; }
            _inited = true;

            if (InitDebugState) { this.State.InitDebugState(); }

            ApplyState();
            gameObject.SetActive(true);
        }

        void ApplySize() => transform.localScale = new Vector3(State.Size, State.Size);

        private void ApplyLoc() => gameObject.transform.localPosition = State.Loc;

        private void ApplyStage1State() => SetActiveObject(Stage1, State.Stage1Answer >= 0 ? State.Stage1Answer % Stage1.Count : -1);
        private void ApplyStage2State() => SetActiveObject(Stage2, State.Stage2Answer >= 0 ? State.Stage2Answer % Stage2.Count : -1);
        private void ApplyStage3State() => SetActiveObject(Stage3, State.Stage3Answer >= 0 ? State.Stage3Answer % Stage3.Count : -1);
        private void ApplyStage4State() => SetActiveObject(Stage4, State.Stage4Answer >= 0 ? State.Stage4Answer % Stage4.Count : -1);

        private List<GameObject> GetStageObjects(int i)
        {
            switch (i)
            {
                case 1:
                    return Stage1;
                case 2:
                    return Stage2;
                case 3:
                    return Stage3;
                case 4:
                    return Stage4;
                default:
                    return null;
            }
        }

        public void SaveLoc()
        {
            State.Loc = gameObject.transform.localPosition;
        }
    }
}
