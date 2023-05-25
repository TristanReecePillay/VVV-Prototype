using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public BoardMaker boardMaker;
    

    public GameObject ballOwner;


    public void SetOwner(GameObject player)
    {
        ballOwner = player;
        
    }

    public GameObject GetOwner()
    {
        return ballOwner;
    }
    // Start is called before the first frame update
    void Start()
    {
        boardMaker = GameObject.Find("BoardMaker").GetComponent<BoardMaker>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (ballOwner != null ) 
        {
            transform.position = ballOwner.transform.position + new Vector3(2, 0.6f, 1);
        }
    }
    public void SetBallOwner(GameObject player)
    {
        ballOwner = player;
    }

    public bool HasBallOwner()
    {
        return ballOwner != null;
    }

    public GameObject GetBallOwner()
    {
        return ballOwner;
    }

    public void ClearBallOwner()
    {
        ballOwner = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            ballOwner = other.gameObject;
        }
    }
}
