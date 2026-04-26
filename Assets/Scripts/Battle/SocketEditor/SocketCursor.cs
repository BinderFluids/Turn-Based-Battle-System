using TransformHandles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Battle.SocketEditor
{
    public class SocketCursor : Singleton<SocketCursor>
    {
    
        private TransformHandleManager transformHandleManager;

        [SerializeField] private Camera socketOverlayCamera; 
        [SerializeField] private Handle handle; 
        [SerializeField] private UnityEvent<SocketHandle> onSocketSelected;
        [SerializeField] private SocketEditorInputReader input;
        [SerializeField] private SocketHandle currentlyHoveredSocketHandle; 
    
        [SerializeField] private bool handleIsBeingInteractedWith;
        [SerializeField] private LayerMask socketLayerMask; 
        private UnityAction<Handle> onSocketSelectedAction;
        private UnityAction<Handle> onSocketDeselectedAction;
    
        private void Start()
        {
            transformHandleManager = TransformHandleManager.Instance;
        
            handle = transformHandleManager.CreateHandle(transform); 
        
            onSocketSelectedAction = _ => handleIsBeingInteractedWith = true;
            onSocketDeselectedAction = _ => handleIsBeingInteractedWith = false;
        
            handle.OnInteractionStartUnityEvent.AddListener(onSocketSelectedAction);
            handle.OnInteractionEndUnityEvent.AddListener(onSocketDeselectedAction);
        
            DisableHandle();
        }

        private void Update()
        {
            SocketHandle previouslyHoveredSocketHandle = currentlyHoveredSocketHandle;
            Ray ray = socketOverlayCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hover, ~socketLayerMask))
            {
                if (hover.collider.TryGetComponent(out currentlyHoveredSocketHandle))
                {
                    currentlyHoveredSocketHandle.EnableHoverName(true);
                }
                else
                {
                    previouslyHoveredSocketHandle?.EnableHoverName(false);
                    currentlyHoveredSocketHandle = null;
                }
            }
            else
            {
                previouslyHoveredSocketHandle?.EnableHoverName(false);
                currentlyHoveredSocketHandle = null;
            }
        
        
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (handleIsBeingInteractedWith) return; 
        
            if (input.Select.WasPressedThisFrame)
            {
                if (!Physics.Raycast(ray, out var hit/*, socketLayerMask*/))
                {
                    SelectSocket(null); 
                    return;
                }
                if (!hit.collider.TryGetComponent(out SocketHandle socketHandle))
                {
                    SelectSocket(null); 
                    return;
                }

                SelectSocket(socketHandle); 
            }
        }
        public void SelectSocket(SocketHandle socketHandle)
        {
            if (socketHandle == null)
            {
                DisableHandle(); 
                return;
            }

            EnableHandle(socketHandle.transform);
            onSocketSelected?.Invoke(socketHandle);
        }

        void EnableHandle(Transform target)
        {
            handle.Enable(target); 
            //handle.target.gameObject.SetActive(true); 
        }
        void DisableHandle()
        {
            //handle.target.gameObject.SetActive(false); 
            handle.Disable(); 
        }
    
    
        protected override void OnDestroy()
        {
            base.OnDestroy();
            handle.OnInteractionStartUnityEvent.RemoveListener(onSocketSelectedAction);
            handle.OnInteractionEndUnityEvent.RemoveListener(onSocketDeselectedAction);
        }


    }
}