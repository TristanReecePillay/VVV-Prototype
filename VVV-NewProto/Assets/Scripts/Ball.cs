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
        transform.position = new Vector3(2, 0.5f, 1);
    }

    // Update is called once per frame
    void Update()
    {

       
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
