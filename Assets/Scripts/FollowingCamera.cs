using UnityEngine;
public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private Vector3 _offset = new Vector3(0,0,0);
    [SerializeField] private Transform target;
    void Start()
    {
        if (target == null) target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        transform.position = target.position + _offset;
    }
}
