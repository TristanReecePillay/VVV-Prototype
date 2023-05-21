using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameObject owner;

    public void SetOwner(GameObject player)
    {
        owner = player;
    }

    public GameObject GetOwner()
    {
        return owner;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
