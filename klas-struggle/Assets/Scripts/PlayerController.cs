using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class PlayerController : MonoBehaviour
    {
        public WheatController Prefab;
        public WheatController GenWheat;

        public FireBaseConnector Connector;
        public Camera Camera;

        private bool _rooted = false;

        public void Start()
        {
            // init generated instance
            GenWheat.State = DataStorage.DS.State ?? new WheatState();
            GenWheat.ApplyState();

            // explicitely don't want to await
            GetAndInstantiateOtherWheats();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // we explicitely don't want to await
                if (!_rooted) { RootWheat(); _rooted = true; }
            }
        }

        private async Task RootWheat()
        {
            var followComp = GenWheat.GetComponent<FollowController>();
            followComp.enabled = false;
            Camera.orthographicSize = 10;

            GenWheat.SaveLoc();
            await Connector.PushStateAsync(GenWheat.State);
        }

        private async Task GetAndInstantiateOtherWheats()
        {
            var states = await Connector.GetStatesAsync();
            Debug.Log($"Retrieved & instantiated {states.Count} states.");
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
        }

    }
}
