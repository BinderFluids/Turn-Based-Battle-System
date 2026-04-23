using TMPro;
using UnityEngine;

namespace Battle.SocketEditor
{
    public class SocketWindow : MonoBehaviour
    {
        [SerializeField] private GameObject windowContainer;
        [SerializeField] private TMP_InputField socketNameField; 

        private void Start()
        {
            socketNameField.onValueChanged.AddListener(UpdateCurrentSocketName);
        }
        private void OnDestroy()
        {
            socketNameField.onValueChanged.RemoveListener(UpdateCurrentSocketName);
        }

        public void SnapToGameObject(GameObject gameObject) => SocketEditorManager.Instance.CurrentSocket.SnapToGameObject(gameObject);
        public void SnapSocketToGround() => SocketEditorManager.Instance.CurrentSocket.SnapToGround();

        void UpdateCurrentSocketName(string newName)
        {
            if (SocketEditorManager.Instance.CurrentSocket == null) return; 
            SocketEditorManager.Instance.CurrentSocket.SetName(newName);
        }

        public void Open(SocketHandle socketHandle)
        {
            if (socketHandle == null)
            {
                Close();
                return; 
            }
        
            windowContainer.SetActive(true);
            socketNameField.text = SocketEditorManager.Instance.CurrentSocket.name;
        }

        public void Close()
        {
            windowContainer.SetActive(false);
        }

    }
}