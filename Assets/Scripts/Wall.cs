using UnityEngine;

public class Wall : MonoBehaviour
{
    public Rigidbody rb;
    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.CompareTag("gap"))
        {
            other.gameObject.GetComponentInParent<Pizza>().wall = null;
            Destroy(gameObject);
        }
    }
    public void DestroyWall()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
        float randomForce = Random.Range(5, 10);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        rb.isKinematic = false;
        rb.AddForce(randomDirection * randomForce, ForceMode.Impulse);
    }
    public void MakeDisabled() {
        gameObject.SetActive(false);
    }

}
