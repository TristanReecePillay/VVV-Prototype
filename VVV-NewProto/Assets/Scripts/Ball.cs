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
           transform.position = ballOwner.transform.position + new Vector3(0f, 0.6f, 0f);
        }
    }
    public void SetBallOwner(Player player)
    {
        //ballOwner = player;
    }

    public bool HasBallOwner()
    {
        return ballOwner != null;
    }

    //public Player GetBallOwner()
   // {
       // return ballOwner;
    //}

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
