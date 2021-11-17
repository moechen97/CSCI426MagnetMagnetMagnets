using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum Direction { Up, Down, Left, Right } 
    public enum PullDirection { None, Up, Down, Left, Right, DiagonalLeftUp, Locked }
    private enum LockType { None, Left, Right, Up, Down }
    private int lockIndex = 0;
    private LockType currLockType;
    private Rigidbody2D rb;
    [HideInInspector] public bool pulling;
    [HideInInspector] public Transform startPos;
    private int id;
    [HideInInspector] public PullDirection pull;
    [HideInInspector] public Transform pullDest;
    private List<PullDirection> pullHistory;
    private bool snapped;
    private ParticleSystem magnetTractorBeam;
    private Magnet magnet;
    private MagnetMove mm;
    private SpriteRenderer sr;
    private Color srOriginalColor;
    private Color srLockColor;
    public bool Charged;
    private ElectricStation electricStation;
    private GameObject electricSpark;
    public void SetLocked(int index, Transform dest)
    {
        lockIndex = index;
        transform.position = dest.position;
        snapped = false;
        pullDest = dest;
        pull = PullDirection.Locked;
        currLockType = LockType.None;
        if (rb.velocity.x > 0) currLockType = LockType.Right;
        else if (rb.velocity.x < 0) currLockType = LockType.Left;
        else if (rb.velocity.y > 0) currLockType = LockType.Up;
        else if (rb.velocity.y < 0) currLockType = LockType.Down;
        rb.velocity = Vector2.zero;

        string[] spikeInfo = gameObject.name.Split('_');
        id = int.Parse(spikeInfo[1]);
    }
    public void SetPull(Transform dest, MagnetMove.Quadrant pd)
    {
        snapped = false;
        pullDest = dest;   
        if(pd == MagnetMove.Quadrant.Up)
        {
            pull = PullDirection.Up;
            transform.position = new Vector3(pullDest.position.x, transform.position.y, transform.position.z);
            pullHistory.Add(pull);
        }
        else if(pd == MagnetMove.Quadrant.Down)
        {
            pull = PullDirection.Down;
            transform.position = new Vector3(pullDest.position.x, transform.position.y, transform.position.z);
            pullHistory.Add(pull);
        }
        else if (pd == MagnetMove.Quadrant.Left)
        {
            pull = PullDirection.Left;
            transform.position = new Vector3(transform.position.x, pullDest.position.y, transform.position.z);
            pullHistory.Add(pull);
        }
        else if (pd == MagnetMove.Quadrant.Right)
        {
            pull = PullDirection.Right;
            transform.position = new Vector3(transform.position.x, pullDest.position.y, transform.position.z);
            pullHistory.Add(pull);
        }
        else if(pd == MagnetMove.Quadrant.DiagonalLeft)
        {
            pull = PullDirection.DiagonalLeftUp;
            pullHistory.Add(pull);
        }
    }
    void Awake()
    {
        electricSpark = transform.GetChild(2).gameObject;
        if(GameObject.FindGameObjectWithTag("ElectricStation")) electricStation = GameObject.FindGameObjectWithTag("ElectricStation").GetComponent<ElectricStation>();
        currLockType = LockType.None;
        Charged = false;
        sr = transform.GetChild(1).GetComponent<SpriteRenderer>();
        srOriginalColor = sr.color;
        srLockColor = new Color(88F / 256F, 183F / 256F, 66F / 256F);
        magnet = GameObject.FindGameObjectWithTag("Magnet").GetComponent<Magnet>();
        mm = GameObject.FindGameObjectWithTag("Magnet").GetComponent<MagnetMove>();
        magnetTractorBeam = magnet.transform.GetChild(1).GetComponent<ParticleSystem>();
        snapped = false;
        pull = PullDirection.None;
        pulling = false;
        pullHistory = new List<PullDirection>();
        rb = GetComponent<Rigidbody2D>();
        startPos = GameObject.FindGameObjectWithTag("Rest" + id).transform;
    }

    public void ResetStage()
    {
        transform.position = startPos.position;
        pullHistory.Clear();
        pull = PullDirection.None;
        snapped = false;
        Charged = false;
    }

    private IEnumerator UnlockRoutine(LockType l)
    {
        pull = PullDirection.None;
        yield return new WaitForSeconds(0.025F);
        if (currLockType == LockType.Left) SetPull(startPos, MagnetMove.Quadrant.Right);
        else if (currLockType == LockType.Right) SetPull(startPos, MagnetMove.Quadrant.Left);
        else if (currLockType == LockType.Up) SetPull(startPos, MagnetMove.Quadrant.Down);
        else if (currLockType == LockType.Down) SetPull(startPos, MagnetMove.Quadrant.Up);
        magnet.Unlock(lockIndex);
        currLockType = LockType.None;
    }
    public void ResetPull()
    {
        Debug.Log("RESET PULL");
        float speed = 3.33F;
        snapped = false;
        mm.UnhideCurrentMagnetHint();
        if (pull == PullDirection.Locked)
        {
            StartCoroutine(UnlockRoutine(currLockType));
        }
        else if (pull == PullDirection.Up)
        {
            rb.velocity = new Vector2(startPos.position.x - transform.position.x, -1.0F * speed);
            SetPull(startPos, MagnetMove.Quadrant.Down);
        }
        else if(pull == PullDirection.Down)
        {
            rb.velocity = new Vector2(startPos.position.x - transform.position.x, speed);
            SetPull(startPos, MagnetMove.Quadrant.Up);
        }
        else if(pull == PullDirection.Left)
        {
            rb.velocity = new Vector2(speed, startPos.position.y - transform.position.y);
            SetPull(startPos, MagnetMove.Quadrant.Right);
        }
        else if(pull == PullDirection.Right)
        {
            rb.velocity = new Vector2(-1.0F * speed, startPos.position.y - transform.position.y);
            SetPull(startPos, MagnetMove.Quadrant.Left);
        }
        else if(pull == PullDirection.DiagonalLeftUp)
        {
            rb.velocity = new Vector2(-1.0F * speed, speed);
            SetPull(startPos, MagnetMove.Quadrant.DiagonalLeft);
        }
    }

    public void RemoveCharge()
    {
        Charged = false;
        electricSpark.SetActive(false);
    }

    public void Update()
    {
        if(!Charged)
        {
            electricSpark.SetActive(false);
        }
        else
        {
            electricSpark.SetActive(true);
            if (pull == PullDirection.Left)
            {
                electricSpark.transform.localPosition = new Vector3(-1F, 0F, 0F);
                electricSpark.transform.eulerAngles = new Vector3(0F, 0F, -90F);
            }
            else if(pull == PullDirection.Right)
            {
                electricSpark.transform.localPosition = new Vector3(1F, 0F, 0F);
                electricSpark.transform.eulerAngles = new Vector3(0F, 0F, 90F);
            }
            else if(pull == PullDirection.Down)
            {
                electricSpark.transform.localPosition = new Vector3(0F, -1F, 0F);
                electricSpark.transform.eulerAngles = Vector3.zero;
            }
            else if(pull == PullDirection.Up)
            {
                electricSpark.transform.localPosition = new Vector3(0F, 1F, 0F);
                electricSpark.transform.eulerAngles = new Vector3(0F, 0F, 180F);
            }
        }

        if (pull != PullDirection.Locked)
        {
            sr.color = srOriginalColor;
        }
        else
        {
            sr.color = srLockColor;
        }
    }
    private void FixedUpdate()
    {
        if (!snapped)
        {
            SetVelocity();
            if (pull == PullDirection.None || (pull == PullDirection.Locked))
            {
                rb.velocity = Vector2.zero;
            }
            else if (pull == PullDirection.Up)
            {
                if (transform.position.y >= pullDest.position.y)
                {
                    Snap();
                }
            }
            else if (pull == PullDirection.Down)
            {
                if (transform.position.y <= pullDest.position.y)
                {
                    Snap();
                }
            }
            else if (pull == PullDirection.Left)
            {
                if (transform.position.x <= pullDest.position.x)
                {
                    Snap();
                }
            }
            else if (pull == PullDirection.Right)
            {
                if (transform.position.x >= pullDest.position.x)
                {
                    Snap();
                }
            }
            else if(pull == PullDirection.DiagonalLeftUp)
            {
                if((transform.position.x <= pullDest.position.x && transform.position.y >= pullDest.position.y)) {
                    Snap();
                }
            }
        }
    }

    private void Snap()
    {
        Charged = false;
        rb.velocity = Vector2.zero;
        if (pull == PullDirection.Up || pull == PullDirection.Down)
        {
            transform.position = new Vector3(transform.position.x, pullDest.position.y, transform.position.z);
        }
        else if (pull == PullDirection.Left || pull == PullDirection.Right)
        {
            transform.position = new Vector3(pullDest.position.x, transform.position.y, transform.position.z);
        }
        if (pullDest != startPos)
        {
            snapped = true;
            magnet.snapBlocked = true;
            mm.HideCurrentMagnetHint();
        }
        else
        {
            pull = PullDirection.None;
        }
    }
    private void SetVelocity()
    {
        if (snapped || pull == PullDirection.None)
        {
            rb.velocity = Vector2.zero;
        }
        else if (pull == PullDirection.Up)
        {
            rb.velocity = new Vector2(pullDest.position.x - transform.position.x, 2F);
        }
        else if (pull == PullDirection.Down)
        {
            rb.velocity = new Vector2(pullDest.position.x - transform.position.x, -2F);
        }
        else if(pull == PullDirection.Left)
        {
            rb.velocity = new Vector2(-2F, pullDest.position.y - transform.position.y);
        }
        else if(pull == PullDirection.Right)
        {
            rb.velocity = new Vector2(2F, pullDest.position.y - transform.position.y);
        }
        else if(pull == PullDirection.DiagonalLeftUp)
        {
            rb.velocity = new Vector2(2F, -2F);
        }
    }

}
