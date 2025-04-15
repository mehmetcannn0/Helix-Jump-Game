using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    private BoxCollider boxCollider;
    public Pizza pizza;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Slice slice))
        {
            if (slice.sliceType  == SliceType.Gap)
            {
                
            }
            pizza.wall = null;
                Destroy(gameObject.GetComponentInParent<Transform>().gameObject); 
        }
    }


    public void DestroyWall()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
        float randomForce = Random.Range(5, 10);
        boxCollider.enabled = false;
        rb.isKinematic = false;
        rb.AddForce(randomDirection * randomForce, ForceMode.Impulse);
    }
    public void MakeDisabled() {
        gameObject.GetComponentInParent<Transform>().gameObject.SetActive(false);
        pizza.wall = null;
    }

}
