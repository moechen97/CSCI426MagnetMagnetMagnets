using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum Direction { Up, Down, Left, Right } 
    public enum PullDirection { None, Left, Up, Right, Down, Locked }
    private enum LockType { None, Left, Right, Up, Down }
    private int lockIndex = 0;
    private LockType currLockType;
    private Rigidbody2D rb;
    [HideInInspector] public bool pulling;
    [HideInInspector] public bool resetting;
    [HideInInspector] public Transform startPos;
    private int id;
    [HideInInspector] public PullDirection pull;
    [HideInInspector] public List<Vector3> pullDestHistory;
    [HideInInspector] public Vector3 pullDest;
    private List<PullDirection> pullHistory;
    private bool snapped;
    private ParticleSystem magnetTractorBeam;
    private Magnet magnet;
    private MagnetMove mm;
    private SpriteRenderer sr;
    private Color srOriginalColor;
    private Color srLockColor;
    [HideInInspector] public bool Charged;
    private ElectricStation electricStation;
    private GameObject electricSpark;
    private GameObject spike;
    private List<Vector3> sourceHistory;
    [HideInInspector] public bool unlocking;
    [HideInInspector] public KeyLock currKeyLock;
    void Awake()
    {
        currKeyLock = null;
        electricSpark = transform.GetChild(2).transform.gameObject;
        if (GameObject.FindGameObjectWithTag("ElectricStation")) electricStation = GameObject.FindGameObjectWithTag("ElectricStation").GetComponent<ElectricStation>();
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
        resetting = false;
        pullHistory = new List<PullDirection>();
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.parent.GetChild(0).transform;
        spike = transform.gameObject;
        pullDestHistory = new List<Vector3>();
        pullDest = transform.position;
        sourceHistory = new List<Vector3>();

        string[] spikeInfo = transform.parent.gameObject.name.Split('_');
        id = int.Parse(spikeInfo[1]);
        unlocking = false;
    }
  
    public void SetPull(int magnet, Transform dest, MagnetMove.Quadrant pd)
    {
        pull = PullDirection.None;
        Debug.Log("SET PULL TO MAGNET " + magnet);
        float speed = 2F;
        snapped = false;
        pullDest = dest.position;
        pullDestHistory.Add(dest.position);
        sourceHistory.Add(transform.position);
        if(pd == MagnetMove.Quadrant.Up)
        {
            pull = PullDirection.Up;
        }
        else if (pd == MagnetMove.Quadrant.Down)
        {
            pull = PullDirection.Down;
        }
        if (pd == MagnetMove.Quadrant.Left)
        {
            pull = PullDirection.Left;
        }
        else if(pd == MagnetMove.Quadrant.Right)
        {
            pull = PullDirection.Right;
        }
        pullHistory.Add(pull);
        pulling = true;
        resetting = false;
    }
    public void ResetPull()
    {
        Debug.Log("RESET PULL");
        float speed = 3.33F;
        snapped = false;
        mm.UnhideCurrentMagnetHint();
        pulling = false;
        resetting = true;
        if(pull == PullDirection.Locked)
        {
            pull = PullDirection.None;
            Debug.Log("PULL HISTORY SIZE: " + pullHistory.Count);
            foreach(PullDirection p in pullHistory)
            {
                Debug.Log(p);
            }
            unlocking = true;
            currKeyLock.Unlock();
            StartCoroutine(Unlock());
        }
    }

    private IEnumerator Unlock()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(1.5F);
        unlocking = false;
    }
    public void ResetStage()
    {
        spike.transform.position = startPos.position;
        pullHistory.Clear();
        pull = PullDirection.None;
        pullDest = startPos.position;
        snapped = false;
        Charged = false;
        pullDestHistory.Clear();
        sourceHistory.Clear();
    }

    private IEnumerator UnlockRoutine(LockType l)
    {
        //pull = PullDirection.None;
        yield return new WaitForSeconds(0.025F);
        //if (currLockType == LockType.Left) SetPull(startPos, MagnetMove.Quadrant.Right);
        //else if (currLockType == LockType.Right) SetPull(startPos, MagnetMove.Quadrant.Left);
        //else if (currLockType == LockType.Up) SetPull(startPos, MagnetMove.Quadrant.Down);
        //else if (currLockType == LockType.Down) SetPull(startPos, MagnetMove.Quadrant.Up);
        //magnet.Unlock(lockIndex);
        //currLockType = LockType.None;
    }

    public void RemoveCharge()
    {
        Charged = false;
        electricSpark.SetActive(false);
    }

    public void Update()
    {
        //if (pull == PullDirection.Locked || unlocking) return;
        if (!Charged)
        {
            electricSpark.SetActive(false);
        }
        else
        {
            electricSpark.SetActive(true);
        }

        if (pull != PullDirection.Locked)
        {
            sr.color = srOriginalColor;
        }
        else
        {
            sr.color = srLockColor;
        }

        if (snapped) return;

        if (pull == PullDirection.Left)
        {
            electricSpark.transform.localPosition = new Vector3(-1F, 0F, 0F);
            electricSpark.transform.eulerAngles = new Vector3(0F, 0F, -90F);
        }
        else if (pull == PullDirection.Right)
        {
            electricSpark.transform.localPosition = new Vector3(1F, 0F, 0F);
            electricSpark.transform.eulerAngles = new Vector3(0F, 0F, 90F);
        }
        else if (pull == PullDirection.Down)
        {
            electricSpark.transform.localPosition = new Vector3(0F, -1F, 0F);
            electricSpark.transform.eulerAngles = Vector3.zero;
        }
        else if (pull == PullDirection.Up)
        {
            electricSpark.transform.localPosition = new Vector3(0F, 1F, 0F);
            electricSpark.transform.eulerAngles = new Vector3(0F, 0F, 180F);
        }

        KeyCode thisKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), "Alpha" + id.ToString());
        if (Input.GetKeyDown(thisKeyCode))
        {
            //Debug.Log("PULL DEST: " + sourceHistory[sourceHistory.Count - 1].x + ", " + sourceHistory[sourceHistory.Count - 1].y);
            Debug.Log("RESETTING: " + resetting);
            Debug.Log("PULLING: " + pulling);
        }

        if(resetting)
        {
            if (pullHistory.Count > 0)
            {
                if(id == 2)
                {
                    Debug.Log("HERE");
                }
                if (pullHistory[pullHistory.Count - 1] == PullDirection.Up)
                {
                    if(spike.transform.position.y <= sourceHistory[sourceHistory.Count - 1].y)
                    {
                        Vector3 temp = spike.transform.position;
                        //spike.transform.position = sourceHistory[sourceHistory.Count - 1];
                        pullHistory.Remove(pullHistory[pullHistory.Count - 1]);
                        sourceHistory.Remove(sourceHistory[sourceHistory.Count - 1]);
                    }
                }
                else if (pullHistory[pullHistory.Count - 1] == PullDirection.Down)
                {
                    if (spike.transform.position.y >= sourceHistory[sourceHistory.Count - 1].y)
                    {
                        Vector3 temp = spike.transform.position;
                        //spike.transform.position = sourceHistory[sourceHistory.Count - 1];
                        pullHistory.Remove(pullHistory[pullHistory.Count - 1]);
                        sourceHistory.Remove(sourceHistory[sourceHistory.Count - 1]);
                    }
                }
                if (pullHistory[pullHistory.Count - 1] == PullDirection.Left)
                {
                    if (spike.transform.position.x >= sourceHistory[sourceHistory.Count - 1].x)
                    {
                        Vector3 temp = spike.transform.position;
                        //spike.transform.position = sourceHistory[sourceHistory.Count - 1];
                        pullHistory.Remove(pullHistory[pullHistory.Count - 1]);
                        sourceHistory.Remove(sourceHistory[sourceHistory.Count - 1]);
                    }
                }
                else if (pullHistory[pullHistory.Count - 1] == PullDirection.Right)
                {
                    if (spike.transform.position.x <= sourceHistory[sourceHistory.Count - 1].x)
                    {
                        Vector3 temp = spike.transform.position;
                        //spike.transform.position = sourceHistory[sourceHistory.Count - 1];
                        pullHistory.Remove(pullHistory[pullHistory.Count - 1]);
                        sourceHistory.Remove(sourceHistory[sourceHistory.Count - 1]);
                    }
                }
            }
            else
            {
                Debug.Log("HERE");
            }
        }
        if (pulling)
        {
            if (pull == PullDirection.Up)
            {
                if (spike.transform.position.y >= pullDest.y)
                {
                    Snap();
                }
            }
            else if (pull == PullDirection.Down)
            {
                if (spike.transform.position.y <= pullDest.y)
                {
                    Snap();
                }
            }
            else if (pull == PullDirection.Left)
            {
                if (spike.transform.localPosition.x <= pullDest.x)
                {
                    Snap();
                }
            }
            else if (pull == PullDirection.Right)
            {
                if (spike.transform.localPosition.x >= pullDest.x)
                {
                    Snap();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (pull == PullDirection.Locked) return;
        SetVelocity();
    }

    private void SetVelocity()
    {
        if ((resetting && sourceHistory.Count == 0) || snapped)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (pulling)
        {
            Vector3 dest = pullDestHistory[pullDestHistory.Count - 1];
            Vector3 v = 2F * Vector3.Normalize(new Vector3(dest.x - transform.position.x, dest.y - transform.position.y, 0F));
            rb.velocity = v;
        }
        else if (resetting)
        {
            if (pullHistory.Count > 0)
            {
                Vector3 dest = sourceHistory[sourceHistory.Count - 1];
                Vector3 v = 2F * Vector3.Normalize(new Vector3(dest.x - transform.position.x, dest.y - transform.transform.position.y, 0F));
                if (pullHistory[pullHistory.Count - 1] == PullDirection.Left)
                {
                    v = new Vector3(1.75F, 0F);
                }
                else if (pullHistory[pullHistory.Count - 1] == PullDirection.Right)
                {
                    v = new Vector3(-1.75F, 0F);
                }
                else if (pullHistory[pullHistory.Count - 1] == PullDirection.Up)
                {
                    v = new Vector3(0F, -1.75F);
                }
                else if (pullHistory[pullHistory.Count - 1] == PullDirection.Down)
                {
                    v = new Vector3(0F, 1.75F);
                }
                rb.velocity = v;
            }
            else
            {
                Vector3 dest = startPos.position;
                Vector3 v = 1.75F * Vector3.Normalize(new Vector3(dest.x - transform.position.x, dest.y - transform.position.y, 0F));
                rb.velocity = v;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }


    private void Snap()
    {
        Debug.Log("SNAP");
        Charged = false;
        resetting = false;
        pulling = false;
        rb.velocity = Vector2.zero;
        if (pull == PullDirection.Up || pull == PullDirection.Down)
        {
            spike.transform.position = new Vector3(pullDest.x, pullDest.y, transform.position.z);
        }
        else if (pull == PullDirection.Left || pull == PullDirection.Right)
        {
            spike.transform.position = new Vector3(pullDest.x, pullDest.y, transform.position.z);
        }
        snapped = true;
        magnet.snapBlocked = true;
        mm.HideCurrentMagnetHint();
        if(Mathf.Approximately(spike.transform.position.x, startPos.position.x) && Mathf.Approximately(spike.transform.position.y, startPos.position.y))
        {
            Debug.Log("SNAP COLLIDE start pos");
        }
    }

    public void SetLocked(KeyLock keyLock, int index, Transform dest)
    {
        currKeyLock = keyLock;
        lockIndex = index;
        spike.transform.position = dest.position;
        snapped = false;
        pullDest = dest.position;
        pull = PullDirection.Locked;
        if (rb.velocity.x > 0) currLockType = LockType.Right;
        else if (rb.velocity.x < 0) currLockType = LockType.Left;
        else if (rb.velocity.y > 0) currLockType = LockType.Up;
        else if (rb.velocity.y < 0) currLockType = LockType.Down;
        rb.velocity = Vector2.zero;
    }
}
