using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private MagnetMove mm;
    private Transform pullSource;
    private List<GameObject> pulls;
    private float pullSpeed = 3.0F;
    public enum Quadrant { Upper, Lower }
    private Quadrant currQuad;
    private LayerMask maskInteractable;
    private LayerMask maskBomb;
    private ParticleSystem[] ps;
    [HideInInspector] public bool snapBlocked;
    private float snapBlockedTimer;
    private Music music;
    private bool playingClink;
    public KeyLock[] KeyLocks;
    private Player player;
    public enum InteractableType { Interactable, Bomb } 
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playingClink = false;
        snapBlocked = false;
        snapBlockedTimer = 0.0F;
        mm = GetComponent<MagnetMove>();
        pullSource = transform.GetChild(0);
        pulls = new List<GameObject>();
        currQuad = Quadrant.Upper;
        maskInteractable = LayerUtility.Only("Magnet", "Interactable");
        maskBomb = LayerUtility.Only("Magnet", "Bomb");
        ps = new ParticleSystem[transform.GetChild(1).childCount];
        for(int i = 0; i < ps.Length; i++)
        {
            ps[i] = transform.GetChild(1).GetChild(i).GetComponent<ParticleSystem>();
        }
        music = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<Music>();
    }

    public void AddPull(GameObject i, InteractableType type = InteractableType.Interactable)
    {
        if (mm.currQuad == MagnetMove.Quadrant.None) return;
        ResetPulls();
        if (type == InteractableType.Interactable)
        {
            i.GetComponent<Interactable>().SetPull(mm.magnetIndex, pullSource, mm.currQuad);
        }
        else if(type == InteractableType.Bomb)
        {
            i.GetComponent<Bomb>().SetPull(mm.magnetIndex, pullSource, mm.currQuad);
        }
        pulls.Add(i);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    public void RemovePulls(GameObject i)
    {
        foreach(GameObject p in pulls)
        {
            if (p.CompareTag("Interactable"))
            {
                p.GetComponent<Interactable>().ResetPull();
            }
            else if(p.CompareTag("Bomb"))
            {
                p.GetComponent<Bomb>().ResetPull();
            }
        }
        snapBlocked = false;
        pulls.Clear();
    }

    public void ResetPulls()
    {
        foreach(GameObject p in pulls)
        {
            if (p.CompareTag("Interactable"))
            {
                p.GetComponent<Interactable>().ResetPull();
            }
            else if(p.CompareTag("Bomb"))
            {
                p.GetComponent<Bomb>().ResetPull();
            }
        }
        snapBlocked = false;
        pulls.Clear();
    }

    public void Update()
    {
        if(player.dying)
        {
            mm.SetMagnetPos(0);
            return;
        }
        ParticleSystem currBeam;
        if (mm.currQuad == MagnetMove.Quadrant.Up)
        {
            currBeam = ps[0];
            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i] != currBeam)
                {
                    ps[i].Stop();
                    ps[i].gameObject.SetActive(false);
                }
            }
        }
        else if (mm.currQuad == MagnetMove.Quadrant.Down)
        {
            currBeam = ps[1];
            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i] != currBeam)
                {
                    ps[i].Stop();
                    ps[i].gameObject.SetActive(false);
                }
            }
        }
        else if (mm.currQuad == MagnetMove.Quadrant.Left)
        {
            currBeam = ps[2];
            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i] != currBeam)
                {
                    ps[i].Stop();
                    ps[i].gameObject.SetActive(false);
                }
            }
        }
        else if (mm.currQuad == MagnetMove.Quadrant.Right)
        {
            currBeam = ps[3];
            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i] != currBeam)
                {
                    ps[i].Stop();
                    ps[i].gameObject.SetActive(false);
                }
            }
        }
        else if (mm.currQuad == MagnetMove.Quadrant.None)
        {
            foreach (ParticleSystem beam in ps)
            {
                beam.Stop();
                beam.gameObject.SetActive(false);
            }
            return;
        }
        else
        {
            return;
        }

        if (currBeam.gameObject.activeSelf)
        {
            if (snapBlocked && !playingClink)
            {
                if (currBeam.isPlaying) { playingClink = true; StartCoroutine(TurnOffTractorBeam()); }
            }
        }
        else
        {
            if (!snapBlocked)
            {
                //Turn on tractor beam
                //music.PlayUnSnapClink();
                currBeam.gameObject.SetActive(true);
                currBeam.Play();
            }
        }

        if (snapBlocked || mm.currQuad == MagnetMove.Quadrant.None) return;
    }

    private void FixedUpdate()
    {
        //USED TO BE IN UPDATE
        Vector2 forward = Vector2.zero;
        if (mm.currQuad == MagnetMove.Quadrant.Up)
        {
            forward = Vector2.down;
        }
        else if (mm.currQuad == MagnetMove.Quadrant.Down)
        {
            forward = Vector2.up;
        }
        else if (mm.currQuad == MagnetMove.Quadrant.Left)
        {
            forward = Vector2.right;
        }
        else if (mm.currQuad == MagnetMove.Quadrant.Right)
        {
            forward = Vector2.left;
        }
        else if (mm.currQuad == MagnetMove.Quadrant.DiagonalLeft)
        {
            forward = new Vector2(-1F, 1F);
        }
        else if (mm.currQuad == MagnetMove.Quadrant.DiagonalRight)
        {
            forward = new Vector2(1F, 1F);
        }

        //raycast for interactables
        RaycastHit2D[] raycasts = Physics2D.RaycastAll(transform.position, forward, 20.5F, maskInteractable);
        int numHits = ObserveRaycasts(raycasts);
        //increase range of magnet if nothing detected from center of magnet
        if (numHits == 0)
        {
            if (mm.currQuad == MagnetMove.Quadrant.Left || mm.currQuad == MagnetMove.Quadrant.Right)
            {
                Vector3 bottomOfMagnet = transform.position;
                bottomOfMagnet.y -= 0.625F;
                raycasts = Physics2D.RaycastAll(bottomOfMagnet, forward, 20.5F, maskInteractable);
                numHits = ObserveRaycasts(raycasts);
                if (numHits == 0)
                {
                    Vector3 topOfMagnet = transform.position;
                    topOfMagnet.y += 0.625F;
                    raycasts = Physics2D.RaycastAll(topOfMagnet, forward, 20.5F, maskInteractable);
                    numHits = ObserveRaycasts(raycasts);
                }
            }
            else if (mm.currQuad == MagnetMove.Quadrant.Up || mm.currQuad == MagnetMove.Quadrant.Down)
            {
                Vector3 leftOfMagnet = transform.position;
                leftOfMagnet.x -= 0.625F;
                raycasts = Physics2D.RaycastAll(leftOfMagnet, forward, 20.5F, maskInteractable);
                numHits = ObserveRaycasts(raycasts);
                if (numHits == 0)
                {
                    Vector3 rightOfMagnet = transform.position;
                    rightOfMagnet.x += 0.625F;
                    raycasts = Physics2D.RaycastAll(rightOfMagnet, forward, 20.5F, maskInteractable);
                    numHits = ObserveRaycasts(raycasts);
                }
            }
        }
    }

    private int ObserveRaycasts(RaycastHit2D[] raycasts)
    {
        int numHits = 0;
        for (int r = 0; r < raycasts.Length; r++)
        {
            if (raycasts[r].collider != null)
            {
                if (raycasts[r].collider.gameObject.CompareTag("Interactable"))
                {
                    numHits++;
                    Interactable i = raycasts[r].collider.gameObject.GetComponent<Interactable>();
                    if(i.pull == Interactable.PullDirection.Locked)
                    {
                        continue;
                    }
                    if (!pulls.Contains(raycasts[r].collider.gameObject))
                    {
                        AddPull(raycasts[r].collider.gameObject, InteractableType.Interactable);
                    }
                    break;
                }
                else if (raycasts[r].collider.gameObject.CompareTag("Bomb"))
                {
                    numHits++;
                    Bomb b = raycasts[r].collider.gameObject.GetComponent<Bomb>();
                    if (!pulls.Contains(raycasts[r].collider.gameObject))
                    {
                        AddPull(raycasts[r].collider.gameObject, InteractableType.Bomb);
                    }
                    break;
                }
            }
        }
        return numHits;
    }
    private IEnumerator TurnOffTractorBeam()
    {
        Debug.Log("?");
        if (pulls.Count == 0) { playingClink = false; yield break; }
        //music.PlaySnapClink();
        foreach(ParticleSystem beam in ps)
        {
            beam.Stop();
        }
        yield return new WaitForSeconds(0.35F);
        playingClink = false;
        if (snapBlocked)
        {
            foreach (ParticleSystem beam in ps)
            {
                beam.gameObject.SetActive(false);
            }
        }
    }

    public void TurnOffBeams()
    {
        foreach(ParticleSystem beam in ps)
        {
            beam.Stop();
            beam.gameObject.SetActive(false);
        }
    }

    //Key Locks
    public void Unlock(int index)
    {
        KeyLocks[index].Unlock();
    }
}
