using Assets.Scripts.KlasStruggle.Persistent;
using Assets.Scripts.KlasStruggle.Wheat;
using Assets.Scripts.Movement;
using Assets.Scripts.Utils;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.KlasStruggle.Field
{
    class PlayerController : MonoBehaviour
    {
        public WheatController Prefab = null;
        public WheatController GenWheat = null;

        public Camera Cam = null;

        public bool SendState = true;
        public bool CreateOtherWheats = true;
        public bool D_ForceWaitUntilDownloadOtherWheatsFromFirebase = false;

        private bool _otherWheatsDownloaded = false;
        private bool _inited = false;

        private bool _rooted = false;
        BoxCollider2D _boxCollider2D = null;

        public float UnzoomToSizeRoot = 10;
        public float UnzoomTimeRoot = 2.5f;

        public float UnzoomCoefInitial = 10;
        public float UnzoomTimeInit = 5f;

        private GameController gameController;
        private MoveController moveController;
        private FollowController wheatFollowController;


        private Component[] wheatSpriteRenderers;

        public void Start()
        {
            gameController = GameController.Get;
            moveController = this.GetComponent<MoveController>();
            wheatFollowController = GenWheat.gameObject.GetComponent<FollowController>();

            // init generated instance
            GenWheat.State = gameController.DataStorage.GeneratedWheatState ?? new WheatState(initDebugState: true);
            GenWheat.InitAndEnable();

            // initialize variables for collision warning
            _boxCollider2D = this.GetComponent<BoxCollider2D>();


            // explicitely don't want to await
            if (CreateOtherWheats) { _ = InstantiateOtherWheatsAsync(); }

            // fire initial unzoom transition to field & set _inited & enable movement after its done
            var origOrthoSize = Cam.orthographicSize;
            Cam.orthographicSize = origOrthoSize / UnzoomCoefInitial;
            Cam.DOOrthoSize(origOrthoSize, UnzoomTimeInit);

            var origScale = GenWheat.transform.localScale;
            GenWheat.transform.localScale = origScale / UnzoomCoefInitial;
            var tweener =  GenWheat.transform.DOScale(origScale, UnzoomTimeInit);

            tweener.OnComplete(InitComplete); 
        }

        private void InitComplete()
        {
            _inited = true; 
            moveController.enableMovement = true;

            wheatSpriteRenderers = GenWheat.GetComponentsInChildren<SpriteRenderer>();
        }

        private async Task RootWheatAsync()
        {
            // freeze generated wheat movement
            wheatFollowController.enabled = false;

            // Unzoom animation
            Cam.DOOrthoSize(UnzoomToSizeRoot, UnzoomTimeRoot);

            // save current location to state and potentially send state
            // send state only when the other wheats came from firebase (-> reasonably sure there're no conflicts)
            GenWheat.SaveLoc();
            if (SendState && _otherWheatsDownloaded) { await gameController.FireBaseConnector.PushStateToFirebaseAsync(GenWheat.State); }
        }

        private async Task InstantiateOtherWheatsAsync()
        {
            var states = gameController.DataStorage.OtherWheatStatesOnline;
            if (states == null)
            {
                // (Most probably from firebase) other wheat states not ready yet
                // -> get offline data (quick) unless D_ForceWaitUntilDownloadOtherWheatsFromFirebase
                await gameController.GetOtherWheatStatesAsync(forceStatesFromOffline: !D_ForceWaitUntilDownloadOtherWheatsFromFirebase);
                states = gameController.DataStorage.OtherWheatStatesOnline;
            }
            _otherWheatsDownloaded = gameController.StatesFromOnline;

            foreach (var state in states)
            {
                InstantiateNewElement(state);
            }
        }

        private void InstantiateNewElement(WheatState state)
        {
            var newInstace = Instantiate(Prefab, new Vector3(0, 0, 0), Quaternion.identity); //location doesn't matter, gets overwritten by state's loc
            newInstace.State = state;

            // Set prefab copy so it doesn't disable itself automatically on Start & initialize it with state & enable
            newInstace.InitOnStart = true;
            newInstace.InitAndEnable();

            // fade new instance in -> set alpha to 0 & slowly move to 1
            newInstace.gameObject.SetFadeChildrenTextsAndSprites(0);
            newInstace.gameObject.DOFadeChildrenTextsAndSprites(1, 5, includeInactive: false);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // we explicitely don't want to await
                if (!_rooted && _inited && !IsInCollision()) { _ = RootWheatAsync(); _rooted = true; }
            }
        }

        public void OnTriggerEnter2D(Collider2D _)
        {
            if (IsInCollision() && !_rooted) { PaintRed(); }
        }
        public void OnTriggerExit2D(Collider2D _)
        {
            if (!IsInCollision()) { PaintWhite(); }
        }

        bool IsInCollision() => _boxCollider2D.IsTouchingLayers();

        public void PaintRed()
        {
            foreach (SpriteRenderer RendererRef in wheatSpriteRenderers) { RendererRef.color = Color.red;}
        }

        public void PaintWhite()
        {
            foreach (SpriteRenderer RendererRef in wheatSpriteRenderers) { RendererRef.color = Color.white;}
        }
    }
}
