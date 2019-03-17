﻿using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class PlayerController : MonoBehaviour
    {
        public WheatController Prefab;
        public WheatController GenWheat;

        public Camera Cam;

        public bool SendState = true;
        public bool CreateOtherWheats = true;
        public bool D_ForceDownloadAndWaitForOtherWheats = false;

        private int collisions;
        private bool _rooted = false;

        SpriteRenderer _warningSprite;
        Color _warningSpriteColorVisible;
        Color _warningSpriteColorInvisible;

        public float UnzoomToSize = 10;
        public float UnzoomTime = 2.5f;

        private GameController gameController;

        public void Start()
        {
            this.gameController = GameController.Get;

            // initialize variables for collision warning box
            _warningSprite = GetComponent<SpriteRenderer>();
            _warningSpriteColorVisible = new Color(_warningSprite.color.r, _warningSprite.color.g, _warningSprite.color.b, 0.2f);
            _warningSpriteColorInvisible = new Color(_warningSprite.color.r, _warningSprite.color.g, _warningSprite.color.b, 0.0f);

            // init generated instance
            GenWheat.State = gameController.DataStorage.GeneratedWheatState ?? new WheatState();
            GenWheat.ApplyState();

            // explicitely don't want to await
            if (CreateOtherWheats) { InstantiateOtherWheats(); }
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // we explicitely don't want to await
                if (!_rooted && collisions <= 0) { RootWheat(); _rooted = true; }
            }
        }

        // count colisions 
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collisions <= 0) {  _warningSprite.color = _warningSpriteColorVisible; }
            this.collisions++;
        }
        public void OnTriggerExit2D(Collider2D collision)
        {
            this.collisions--;
            if (collisions <= 0) { _warningSprite.color = _warningSpriteColorInvisible; }
        }

        private async Task RootWheat()
        {
            // freeze generated wheat movement
            var followComp = GenWheat.GetComponent<FollowController>();
            followComp.enabled = false;

            // Unzoom animation
            Cam.DOOrthoSize(UnzoomToSize, UnzoomTime);

            // save current location to state and potentially send state
            GenWheat.SaveLoc();
            if (SendState) { await gameController.FireBaseConnector.PushStateAsync(GenWheat.State); }
        }

        private async Task InstantiateOtherWheats()
        {
            var states = gameController.DataStorage.OtherWheatStatesOnline;

            if(states == null && D_ForceDownloadAndWaitForOtherWheats)
            {
                await gameController.DownloadOtherWheatStates();
                states = gameController.DataStorage.OtherWheatStatesOnline;
            }

            foreach (var state in states)
            {
                InstantiateNewElement(state);
            }
        }

        private void InstantiateNewElement(WheatState state)
        {
            // instantiate one object based on its state
            Vector3 location = new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-3, 3), 0);

            var newInstace = Instantiate(Prefab, location, Quaternion.identity);
            newInstace.State = state;

            // Set prefab copy so it doesn't disable itself automatically on Start & initialize it with state & enable
            newInstace.InitOnStart = true;
            newInstace.InitAndEnable();

            newInstace.gameObject.DOFadeChildrenSprites(0, 1, 5);
        }

    }
}
