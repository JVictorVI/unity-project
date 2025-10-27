using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DoorSideTrigger : MonoBehaviour
{
    public DoorController door;
    public bool isInsideSide = false;

    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
        if (!door) door = GetComponentInParent<DoorController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        door?.Open(fromInsideSide: isInsideSide);
        door?.NotifyEnteredSideTrigger(); // mantém aberta enquanto entra
    }
}
