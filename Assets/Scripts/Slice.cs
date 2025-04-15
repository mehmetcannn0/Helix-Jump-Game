using UnityEngine;

public class Slice : MonoBehaviour
{
    private Vector3 gapPosition = new Vector3(0, -0.4f, 0);  
    [SerializeField] Material greenMaterial;
    [SerializeField] Material redMaterial;
    [SerializeField] Material whiteMaterial;
    [SerializeField] MeshCollider meshCollider;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] Renderer renderer;

    [SerializeField] Rigidbody rb;


    
    public SliceType sliceType ;
    
    private void Awake()
    { 
        rb = gameObject.GetComponent<Rigidbody>();
        //meshCollider = gameObject.GetComponent<MeshCollider>();
        boxCollider = rb.GetComponent<BoxCollider>();
        renderer = gameObject.GetComponent<Renderer>();

    }
    public void SetRedSlice()
    {
        sliceType = SliceType.Redslice ;
        renderer.material = redMaterial;
        gameObject.tag = Utils.RED_SLICE_TAG;
    }
    public void SetGap()
    {
        sliceType = SliceType.Gap;
        //meshCollider.convex = true;
        //meshCollider.isTrigger = true;
        boxCollider.isTrigger = true;
        gameObject.tag = Utils.GAP_SLICE_TAG;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        //gameObject.GetComponent<MeshRenderer>().material = greenMaterial;   
        gameObject.transform.localPosition = gapPosition;
    }

    public void ChangeColor(bool combo)
    {
        if (combo)
        {
            renderer.material = greenMaterial;

        }
        else
        {
            if (sliceType == SliceType.Redslice)
            {
                renderer.material = redMaterial;
            }
            else
            {
                
                renderer.material = whiteMaterial;
            }

        }

    }
    public void DestroySlice()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
        float randomForce = Random.Range(5, 10);
        //meshCollider.enabled = false;
        boxCollider.enabled = false;
        rb.isKinematic = false;
        rb.AddForce(randomDirection * randomForce, ForceMode.Impulse);
    }

}
