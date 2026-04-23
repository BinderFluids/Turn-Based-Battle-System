using UnityEngine;
using UnityEngine.Events;

namespace Battle.SocketEditor
{
    public class SocketCreator : MonoBehaviour
    {
        private string defaultname; 
    
        [SerializeField] private Camera cam; 
        [SerializeField] private SocketHandle socketPrefab;
        [SerializeField] private SocketEditorInputReader input;
        [SerializeField] public UnityEvent<SocketHandle> onSocketCreated;
    
        private void Update()
        {
            if (input.CreateSocket.WasPressedThisFrame)
            {
                if (!Physics.Raycast(input.MouseRay, out var hit)) return;
                CreateSocket(hit.point);
            }
        }

        public SocketHandle CreateSocket(Vector3 position, string name = "")
        {
            SocketHandle newSocketHandle = Instantiate(socketPrefab, position, Quaternion.identity);  
            GameObject socketHandleGameObject = newSocketHandle.gameObject;
        
            newSocketHandle.Initialize(name == "" ? defaultname : name);
        
            onSocketCreated?.Invoke(newSocketHandle);

            return newSocketHandle; 
        }
    }
}
