using System;
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

        /// <summary>
        /// Applies state to actual representation: enables children defined by the state, ... .
        /// </summary>
        public void ApplyState()
        {
            ApplySize();

            ApplyStageResult(1);
            ApplyStageResult(2);
            ApplyStageResult(3);
            ApplyStageResult(4);

            ApplyLoc();
        }

        /// <summary>
        /// Applies decisions from WheatFramework's answers -> advances current wheat generation.
        /// </summary>
        internal void ApplyDecision(Answer answer)
        {
            Debug.Assert(GenerateAsPlayer);

            int stageIndex = answer.Question.Stage.Id + 1;
            switch (stageIndex)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    SetStateStageAnswer(stageIndex, answer.Id + answer.Question.Id * 2);
                    ApplyStageResult(stageIndex);
                    break;
                case 5:
                    State.Size = answer.Id == 0 ? 1 : 1.2f;
                    ApplySize();
                    break;
                default:
                    Debug.Assert(false, "Unreachable.");
                    break;

            }
        }
        
        /// <summary>
        /// Activates and potentially fades-in desired object and inactivates & fades-out all other.
        /// Potentially aligns ConnectPoints & starts animation.
        /// </summary>
        void SetActiveObject(List<GameObject> objects, int activatedObjIndex)
        {
            if (activatedObjIndex >= 0)
            {
                var activatedObject = objects[activatedObjIndex];

                AllignObjectsPlugWithPreviouslyActiveSocket(activatedObject);
                var animationObj = activatedObject.GetComponentInChildren<Animation>(false);
                animationObj?.StartAnimation();
            }

            // enable activeObject from `objects` (current stage objects) and hide all other ones
            for (int i = 0; i < objects.Count; i++)
            {
                bool activeNow = (i == activatedObjIndex);
                bool activePreviously = objects[i].activeSelf;

                objects[i].SetActive(activeNow);

                if (GenerateAsPlayer)
                {
                    if (activeNow && !activePreviously) // fade new parts in -> set alpha to 0 & slowly move to 1
                    {
                        objects[i].gameObject.SetFadeChildrenTextsAndSprites(0);
                        objects[i].gameObject.DOFadeChildrenTextsAndSprites(1, 3);
                    }
                    else if (!activeNow && activePreviously)
                    {
                        objects[i].gameObject.DOFadeChildrenTextsAndSprites(0, 3); // fade out, should already be visible -> just decrease alpha
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


        void ApplySize() => transform.localScale = new Vector3(State.Size, State.Size);

        void ApplyLoc() => gameObject.transform.localPosition = State.Loc;

        void ApplyStageResult(int stageIndex)
        {
            var stage = GetStageObjects(stageIndex);
            var stageAnswer = GetStateSageAnswer(stageIndex);

            SetActiveObject(stage, stageAnswer >= 0 ? stageAnswer % stage.Count : -1);
        }

        void SetStateStageAnswer(int stageIndex, int answerId) 
            => GetStateSageAnswer(stageIndex) = answerId;

        public void SaveLoc() 
            => State.Loc = gameObject.transform.localPosition;

        private ref int GetStateSageAnswer(int stageIndex)
        {
            switch (stageIndex)
            {
                case 1:
                    return ref State.Stage1Answer;
                case 2:
                    return ref State.Stage2Answer;
                case 3:
                    return ref State.Stage3Answer;
                case 4:
                    return ref State.Stage4Answer;
                default:
                    throw new InvalidOperationException();
            }

        }

        private List<GameObject> GetStageObjects(int stageIndex)
        {
            switch (stageIndex)
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
                    throw new InvalidOperationException();
            }
        }
    }
}
