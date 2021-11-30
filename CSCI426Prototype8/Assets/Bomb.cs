using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Interactable.PullDirection pull;
    private bool pulling;
    private Vector3 startPos;
    private Rigidbody2D rb;
    private Vector3 dest;
    private Player player;
    private Music music;
    public GameObject bombLock;
    // Start is called before the first frame update
    void Start()
    {
        pull = Interactable.PullDirection.None;
        pulling = false;
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        music = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<Music>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(pull == Interactable.PullDirection.None)
        {
            return;
        }
        if (pulling)
        {
            rb.velocity = 4.5F * Vector3.Normalize(new Vector3(dest.x - transform.position.x, dest.y - transform.position.y, 0F));
        }
        
    }

    public void Reset()
    {
        bombLock.gameObject.SetActive(true);
        foreach (Transform child in bombLock.transform)
        {
            child.gameObject.SetActive(true);
        }
        foreach (Transform child in bombLock.transform.GetChild(0))
        {
            child.gameObject.SetActive(true);
        }   
        gameObject.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(true);
        ResetPull();
        transform.position = startPos;
        dest = transform.position;
    }

    public void ResetPull()
    {
        rb.velocity = Vector3.zero;
        pull = Interactable.PullDirection.None;
        pulling = false;
    }

    public void SetPull(int magnet, Transform dest, MagnetMove.Quadrant pd)
    {
        pull = Interactable.PullDirection.None;
        Debug.Log("SET *BOMB* PULL TO MAGNET " + magnet);
        this.dest = dest.position;
        if (pd == MagnetMove.Quadrant.Up)
        {
            pull = Interactable.PullDirection.Up;
        }
        else if (pd == MagnetMove.Quadrant.Down)
        {
            pull = Interactable.PullDirection.Down;
        }
        if (pd == MagnetMove.Quadrant.Left)
        {
            pull = Interactable.PullDirection.Left;
        }
        else if (pd == MagnetMove.Quadrant.Right)
        {
            pull = Interactable.PullDirection.Right;
        }
        pulling = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("BombLock"))
        {
            Debug.Log("PLAY BOMB DEFUSE");
            music.PlayBombDefuse();
            Debug.Log("BOMB DEFUSE PLAYED");
            bombLock = collision.transform.parent.gameObject;
            foreach (Transform child in collision.transform.parent)
            {
                child.gameObject.SetActive(false);
            }
            collision.transform.parent.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        else if(collision.gameObject.CompareTag("Trail"))
        {
            bool dead = player.Die();
            if(dead)
            {
                transform.position = startPos;
            }
            music.PlayBombExplode();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("HII");
    }
}
