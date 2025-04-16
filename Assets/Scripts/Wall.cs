using UnityEngine;

public class Wall : MonoBehaviour, IDestroyable, IColorChangeable
{
    private BoxCollider boxCollider;
    [SerializeField] Rigidbody rb;
    [SerializeField] Material greenMaterial;
    [SerializeField] Material redMaterial;
    [SerializeField] Renderer objectRenderer;

    public Pizza pizza;

    public DestroyableType objectType { get; set; } = DestroyableType.Wall;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        objectRenderer = gameObject.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Slice slice))
        {
            if (slice.objectType == DestroyableType.Gap)
            {
                pizza.wall = null;
                Destroy(gameObject.GetComponentInParent<Transform>().gameObject);
            }

        }
    }

    public void ChangeColor(bool combo)
    {
        if (combo)
        {
            objectRenderer.material = greenMaterial;

        }
        else
        {
            objectRenderer.material = redMaterial;
        }
    }

    public void DestroyObject()
    {
        DestroyWall();
    }

    public void DestroyWall()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
        float randomForce = Random.Range(5, 10);
        boxCollider.enabled = false;
        rb.isKinematic = false;
        rb.AddForce(randomDirection * randomForce, ForceMode.Impulse);

        //Invoke(nameof(SetDefaultValues), 2f);

    }
    public void MakeDisabled()
    {
        gameObject.GetComponentInParent<Transform>().gameObject.SetActive(false);
        pizza.wall = null;
    }

    private void SetDefaultValues()
    { 

        rb.isKinematic = true;
        boxCollider.enabled = true;
        boxCollider.isTrigger = false;
        objectRenderer.material = redMaterial; 
        rb.velocity = Vector3.zero;
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, 0));
    }
}
