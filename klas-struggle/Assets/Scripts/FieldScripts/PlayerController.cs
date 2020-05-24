using Assets.Scripts.KlasStruggle.Persistent;
using Assets.Scripts.KlasStruggle.Wheat;
using Assets.Scripts.Movement;
using Assets.Scripts.Utils;
using DG.Tweening;
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
        private bool? _collisionIndicatorWasVisible = null;

      
        public float UnzoomToSizeRoot = 10;
        public float UnzoomTimeRoot = 2.5f;
        public float UnoomWait = 1f;
        public float UnzoomCoefInitial = 10;
        public float UnzoomTimeInit = 5f;

        private GameController gameController;
        private MoveController moveController;
        private FollowController wheatFollowController;

        private SpriteRenderer[] wheatSpriteRenderers;

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

            // update collision indicator -> show it if we can't root on intial location
            // TODO: Have different system for showing the initial can't root to `can't root due to collision with other wheat`?
            wheatSpriteRenderers = GenWheat.GetComponentsInChildren<SpriteRenderer>();
            UpdateCollisionIndicator();

            // explicitly don't want to await
            if (CreateOtherWheats) { _ = InstantiateOtherWheatsAsync(); }

            Invoke(nameof(UnZoomCamera), 0.5f);
        }


        private void UnZoomCamera()
        {
            // fire unzoom transition to field & incrase scale of generated wheat to create illusion of it growing
            var newOrthoSize = Cam.orthographicSize * UnzoomCoefInitial;
            Cam.DOOrthoSize(newOrthoSize, UnzoomTimeInit);

            Invoke(nameof(ScaleWheat), 0.3f);
        }

        private void ScaleWheat()
        {
            var newScale = GenWheat.transform.localScale * UnzoomCoefInitial;
            var tweener = GenWheat.transform.DOScale(newScale, UnzoomTimeInit);

            // set _inited & enable movement after the transitions above are done
            tweener.OnComplete(InitComplete);
        }
        private void InitComplete()
        {
            _inited = true; 
            moveController.enableMovement = true;
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

            // need to update scale of other wheats to match final scale of generated wheat (after it grows to final size)
            newInstace.gameObject.transform.localScale *= UnzoomCoefInitial;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // we explicitely don't want to await
                if (!_rooted && _inited && !IsInCollision()) { _ = RootWheatAsync(); _rooted = true; }
            }
        }

        private void UpdateCollisionIndicator()
        {
            bool isInCollision = IsInCollision();

            if (isInCollision && _collisionIndicatorWasVisible != true && !_rooted) { _collisionIndicatorWasVisible = true; PaintRed(); }
            else if (!isInCollision && _collisionIndicatorWasVisible != false) { _collisionIndicatorWasVisible = false; PaintWhite(); }
        }

        private void PaintRed()
        {
            foreach (SpriteRenderer RendererRef in wheatSpriteRenderers) { RendererRef.color = Color.red;}
        }

        private void PaintWhite()
        {
            foreach (SpriteRenderer RendererRef in wheatSpriteRenderers) { RendererRef.color = Color.white;}
        }

        public void OnTriggerEnter2D(Collider2D _)
        {
            UpdateCollisionIndicator();
        }

        public void OnTriggerExit2D(Collider2D _)
        {
            UpdateCollisionIndicator();
        }

        private bool IsInCollision() => _boxCollider2D.IsTouchingLayers();
    }
}
