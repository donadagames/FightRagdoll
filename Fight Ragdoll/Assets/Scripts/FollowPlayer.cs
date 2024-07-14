using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Transform target;
    public float hight;

    private void Start()
    {
        target = FindObjectOfType<Player>().transform;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z - hight);
    }
}
