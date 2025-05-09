using UnityEngine;

public class OffsetMouvement : MonoBehaviour
{
    public GameObject target;

    void Update()
    {
        transform.position = new Vector3(target.transform.localPosition.x, transform.position.y, transform.position.z);
    }
}
