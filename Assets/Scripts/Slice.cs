using UnityEngine;

public class Slice : MonoBehaviour
{
    public Material greenMaterial; 
    public Material redMaterial; 
    public Material whiteMaterial;
    
    public SliceType sliceType ;

    Rigidbody rb;

    private void Start()
    { 
        rb = gameObject.GetComponent<Rigidbody>();

    }
    public void SetRedSlice()
    {
        sliceType = SliceType.Redslice ;
        gameObject.GetComponent<Renderer>().material = redMaterial;
        gameObject.tag = "redSlice";
    }
    public void SetGap()
    {
        sliceType = SliceType.Gap;
        gameObject.GetComponent<MeshCollider>().convex = true;
        gameObject.GetComponent<MeshCollider>().isTrigger = true;
        gameObject.tag = "gap";
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.localPosition = new Vector3(0, -0.2f, 0); 
    }

    public void ChangeColor(bool combo)
    {
        if (combo)
        {
            GetComponent<Renderer>().material = greenMaterial;

        }
        else
        {
            if (sliceType == SliceType.Redslice)
            {
                GetComponent<Renderer>().material = redMaterial;
            }
            else
            {
                
                GetComponent<Renderer>().material = whiteMaterial;
            }

        }

    }
    public void DestroySlice()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
        float randomForce = Random.Range(5, 10);
        gameObject.GetComponent<MeshCollider>().enabled = false;
        rb.isKinematic = false;
        rb.AddForce(randomDirection * randomForce, ForceMode.Impulse);
    }

}
