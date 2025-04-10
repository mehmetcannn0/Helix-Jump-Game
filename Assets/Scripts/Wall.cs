using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.CompareTag("gap"))
        {
            Destroy(gameObject);
        }
    }
}
