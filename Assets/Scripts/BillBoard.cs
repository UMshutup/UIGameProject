using UnityEngine;

public class BillBoard : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
