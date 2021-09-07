using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionRock : MonoBehaviour
{
    public float cubeSize = 0.7f;
    public int cubesInRow = 2;

    float cubePDistance;
    Vector3 cubeP;

    [SerializeField] public float ExplosionForce;
    [SerializeField] public float ExplosionRadius;
    [SerializeField] public float ExplosionUpward;

    [SerializeField] public GameObject smallRock;
    [SerializeField] private bool thereIsSmallRock;

    // Start is called before the first frame update
    void Start()
    {
        cubePDistance = cubeSize * cubesInRow / 2;
        cubeP = new Vector3(cubePDistance, cubePDistance, cubePDistance);

        thereIsSmallRock = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (thereIsSmallRock==true)
        {
            Debug.Log("yes");
            Destroy(smallRock);
            thereIsSmallRock = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       
        explode();
        
        
    }
    public void explode()
    {
        gameObject.SetActive(false);
        for(int x = 0; x < cubesInRow; x++)
        {
            for(int y = 0; y < cubesInRow; y++)
            {
                for (int z = 0; z < cubesInRow; z++)
                {
                    createPiece(x, y, z);
                    
                }
            }
        }

        //pos explode
        Vector3 explosionPos = transform.position;
        //add force
        Collider[] colliders = Physics.OverlapSphere(explosionPos, ExplosionRadius);
        foreach(Collider hit in colliders)
        {
            //get rb
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(ExplosionForce, transform.position, ExplosionForce, ExplosionUpward);
            }
        }
        thereIsSmallRock = true;

    }
    void createPiece(int x,int y,int z)
    {
        //create little object
        GameObject piece;
        piece = Instantiate(smallRock, new Vector3(x, y, z), Quaternion.identity);

        //set piece pos
        piece.transform.position = transform.position+new Vector3(cubeSize*x, cubeSize * y, cubeSize * z)-cubeP;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        //add rigidbody
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;

        Destroy(piece,1.5f);
    }
}

