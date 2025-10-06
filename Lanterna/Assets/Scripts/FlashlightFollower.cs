using UnityEngine;

public class FlashlightFollower : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speedRotation;

    private Transform target;

    void Start()
    {
        target = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, speedRotation * Time.deltaTime);
    }
}
