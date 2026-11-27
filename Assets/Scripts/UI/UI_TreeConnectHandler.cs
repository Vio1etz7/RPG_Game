using System;
using UnityEngine;

[Serializable] 
class TreeConnectDetails
{
    public UI_TreeConnectHandler childNode;
    public NodeDirectionType direction;
    [Range(100f,350f)] public float length; 
}

public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private TreeConnectDetails[] connectionDetails;
    [SerializeField] private UI_TreeConnection[] connections;

    private void OnValidate()
    {
        if(rect == null)
           rect = GetComponent<RectTransform>();

        if(connectionDetails.Length != connections.Length)
        {
            Debug.Log("Amounts of connectionDetails should be same as connections! - "+ gameObject.name);
            return;
        }

        UpdateConnections();
    }

    public void UpdateConnections()
    {
        for(int i = 0; i < connectionDetails.Length; i++)
        {
            var detail = connectionDetails[i];
            var connection = connections[i];
            Vector2 targetPosition = connection.GetConnectionPoint(rect);

            connection.DirectionConnect(detail.direction,detail.length);
            detail.childNode?.setPosition(targetPosition);
        }
    }

    public void setPosition(Vector2 position) => rect.anchoredPosition = position;
}
