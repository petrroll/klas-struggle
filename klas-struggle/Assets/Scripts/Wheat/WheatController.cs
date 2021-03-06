﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utils;
using Assets.Scripts.WheatFramework;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.KlasStruggle.Wheat
{
    public class WheatController : MonoBehaviour
    {
        private const float SizeChangeDuration = 1f;
        private List<List<GameObject>> StagesObjects = new List<List<GameObject>>();

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

            InitStagesAndStageObjects();

            if (InitDebugState) { this.State.InitDebugState(); }
            ApplyState();

            gameObject.SetActive(true);
        }

        private void InitStagesAndStageObjects()
        {
            // Inits gameObjects for stages: 
            // - all children with `WheatStage` as stage & under each of them `WheatStageObject` as an option.
            // assumes the order of retrieved components is the same as the order in Editor
            foreach (var stage in GetComponentsInChildren<WheatStage>(includeInactive: true).Where(st => st.Enabled))
            {
                // assumes the order of retrieved components is the same as the order in Editor
                var stageObjects = stage.gameObject.GetComponentsInChildren<WheatStageObject>(includeInactive: true)
                    .Where(stObj => stObj.Enabled)
                    .Select(stageObj => stageObj.gameObject).ToList();
                StagesObjects.Add(stageObjects);
            }
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
                    float newSize = 1 + (answer.Id + answer.Question.Id) / 8f;
                    State.Size = newSize;
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
            // allign activated stage
            if (activatedObjIndex >= 0)
            {
                var activatedObject = objects[activatedObjIndex];
                AllignObjectsPlugWithPreviouslyActiveSocket(activatedObject);
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
                        objects[i].gameObject.SetFadeChildrenTextsAndSprites(0, includeInactive: false);
                        objects[i].gameObject.DOFadeChildrenTextsAndSprites(1, 3, includeInactive: false);

                        // need to activate animation after object has been activated
                        foreach (var animation in objects[i].GetComponentsInChildren<AnimationPoint>(includeInactive: false))
                        {
                            animation.StartAnimation();
                        }
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
            var connectionPlugs = gameObject.GetComponentsInChildren<ConnectPointPlug>(false);
            foreach(var connectionPlug in connectionPlugs)
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
                    connectionPlug.transform.parent.position -= transformDifference;
                }
            }
        }

        void ApplySize()
        {
            Vector3 newLocalScale = new Vector3(State.Size, State.Size);
            if (GenerateAsPlayer)
            {
                transform.DOScale(newLocalScale, SizeChangeDuration);
            }
            else
            {
                transform.localScale = newLocalScale;
            }
        }

        void ApplyLoc() => gameObject.transform.localPosition = State.Loc;

        void ApplyStageResult(int stageIndex)
        {
            var stage = GetStageObjects(stageIndex);
            var stageAnswer = GetStateSageAnswer(stageIndex);

            SetActiveObject(stage, stageAnswer >= 0 ? stageAnswer % stage.Count : -1);
        }

        void SetStateStageAnswer(int stageIndex, int answerId) 
            => GetStateSageAnswer(stageIndex) = answerId;

        /// <summary>
        /// Offset can be used e.g. when a DOMove Tween has been but we want to save the location before it has finished.
        /// </summary>
        public void SaveLoc(Vector3 offset = new Vector3()) 
            => State.Loc = gameObject.transform.localPosition + offset;

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
                case 2:
                case 3:
                case 4:
                    return StagesObjects[stageIndex - 1];
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
