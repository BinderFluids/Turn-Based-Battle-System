using TMPro;
using UnityEngine;

public class SocketHandle : MonoBehaviour
{
    [SerializeField] private GameObject hoverName;
    [SerializeField] private TMP_Text nameText; 
    
    public void Initialize(string name)
    {
        SetName(name); 
    }

    public void SnapToGround()
    {
        SetPosition(new Vector3(transform.position.x, 0, transform.position.z));
    }

    void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetName(string name)
    {
        gameObject.name = name;
        nameText.text = name;
    }

    void SelectThisHandle()
    {
        Debug.Log("selc");
        SocketCursor.Instance.SelectSocket(this); 
    }

    public void EnableHoverName(bool enabled) => hoverName.SetActive(enabled);
}
