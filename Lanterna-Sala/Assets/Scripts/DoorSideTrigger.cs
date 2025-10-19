using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DoorSideTrigger : MonoBehaviour
{
    public DoorController door;
    public bool isInsideSide = false; // true = lado de dentro

    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
        if (!door) door = GetComponentInParent<DoorController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (door) door.Open(fromInsideSide: isInsideSide);
    }
}
