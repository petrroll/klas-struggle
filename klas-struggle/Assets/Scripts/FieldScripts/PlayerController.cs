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
        public bool D_ForceDownloadAndWaitForOtherWheats = false;

        private bool _inited = false;

        private bool _rooted = false;
        BoxCollider2D _boxCollider2D = null;

        SpriteRenderer _warningSprite;
        Color _warningSpriteColorVisible;
        Color _warningSpriteColorInvisible;

        public float UnzoomToSizeRoot = 10;
        public float UnzoomTimeRoot = 2.5f;

        public float UnzoomCoefInitial = 10;
        public float UnzoomTimeInit = 5f;

        private GameController gameController;
        private MoveController moveController;

        public void Start()
        {
            gameController = GameController.Get;
            moveController = this.GetComponent<MoveController>();

            // initialize variables for collision warning box
            _boxCollider2D = this.GetComponent<BoxCollider2D>();

            _warningSprite = GetComponent<SpriteRenderer>();
            _warningSpriteColorVisible = new Color(_warningSprite.color.r, _warningSprite.color.g, _warningSprite.color.b, 0.2f);
            _warningSpriteColorInvisible = new Color(_warningSprite.color.r, _warningSprite.color.g, _warningSprite.color.b, 0.0f);
            _warningSprite.color = IsInCollision() ? _warningSpriteColorVisible : _warningSpriteColorInvisible;

            // init generated instance
            GenWheat.State = gameController.DataStorage.GeneratedWheatState ?? new WheatState();
            GenWheat.ApplyState();

            // explicitely don't want to await
            if (CreateOtherWheats) { _ = InstantiateOtherWheatsAsync(); }

            // fire initial unzoom transition to field & set _inited & enable movement after its done
            var origOrthoSize = Cam.orthographicSize;
            Cam.orthographicSize = origOrthoSize / UnzoomCoefInitial;
            Cam.DOOrthoSize(origOrthoSize, UnzoomTimeInit);

            var origScale = GenWheat.transform.localScale;
            GenWheat.transform.DOScale(origScale/10, 0);    // `GenWheat.transform.localScale = origScale / 10;` doesn't register for _some_ reason
            var tweener =  GenWheat.transform.DOScale(origScale, UnzoomTimeInit);

            tweener.OnComplete(InitComplete); 
        }

        private void InitComplete()
        {
            _inited = true; 
            moveController.enableMovement = true;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // we explicitely don't want to await
                if (!_rooted && _inited && !IsInCollision()) { _ = RootWheatAsync(); _rooted = true; }
            }
        }

        // count colisions 
        public void OnTriggerEnter2D(Collider2D _)
        {
            if (IsInCollision() && !_rooted) {  _warningSprite.color = _warningSpriteColorVisible; }
        }
        public void OnTriggerExit2D(Collider2D _)
        {
            if (!IsInCollision()) { _warningSprite.color = _warningSpriteColorInvisible; }
        }

        bool IsInCollision() => _boxCollider2D.IsTouchingLayers();

        private async Task RootWheatAsync()
        {
            // freeze generated wheat movement
            var followComp = GenWheat.GetComponent<FollowController>();
            followComp.enabled = false;

            // Unzoom animation
            Cam.DOOrthoSize(UnzoomToSizeRoot, UnzoomTimeRoot);

            // save current location to state and potentially send state
            GenWheat.SaveLoc();
            if (SendState) { await gameController.FireBaseConnector.PushStateAsync(GenWheat.State); }
        }

        private async Task InstantiateOtherWheatsAsync()
        {
            var states = gameController.DataStorage.OtherWheatStatesOnline;

            if(states == null && D_ForceDownloadAndWaitForOtherWheats)
            {
                await gameController.DownloadOtherWheatStatesAsync();
                states = gameController.DataStorage.OtherWheatStatesOnline;
            }

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
            newInstace.gameObject.SetFadeChildrenSprites(0);
            newInstace.gameObject.DOFadeChildrenSprites(1, 5);
        }

    }
}
