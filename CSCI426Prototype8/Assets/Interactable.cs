using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum ForceFieldState { None, Weak, Strong }
    public enum Direction { Up, Down, Left, Right } 
    public enum PullDirection { None, Left, Up, Right, Down, Locked }
    public enum LockType { None, Left, Right, Up, Down }
    private int lockIndex = 0;
    [HideInInspector] public LockType currLockType;
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
    private ForceFieldState forceFieldState;
    private ForceFieldState prevForceFieldState;
    private ParticleSystem forceFieldNone;
    private ParticleSystem forceFieldWeak;
    private ParticleSystem forceFieldStrong;
    private ParticleSystem.VelocityOverLifetimeModule forceFieldStrongVelocity;
    private Vector3 ffHide;
    private Vector3 ffVisible;
    [SerializeField] public bool isAttractedToStart;
    void Awake()
    {
        ffHide = new Vector3(500F, 500F, 500F);
        ffVisible = new Vector3(0, -0.06F, 0F);
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
        GameObject start = transform.parent.GetChild(0).gameObject;
        startPos = start.transform;
        forceFieldNone = start.transform.GetChild(0).GetComponent<ParticleSystem>();
        forceFieldNone.transform.localPosition = ffVisible;
        forceFieldWeak = start.transform.GetChild(1).GetComponent<ParticleSystem>();
        forceFieldWeak.transform.localPosition = ffHide;
        forceFieldStrong = start.transform.GetChild(2).GetComponent<ParticleSystem>();
        forceFieldStrong.transform.localPosition = ffHide;
        spike = transform.gameObject;
        pullDestHistory = new List<Vector3>();
        pullDest = transform.position;
        sourceHistory = new List<Vector3>();

        string[] spikeInfo = transform.parent.gameObject.name.Split('_');
        id = int.Parse(spikeInfo[1]);
        unlocking = false;

        forceFieldState = ForceFieldState.None;
        forceFieldStrongVelocity = forceFieldStrong.velocityOverLifetime;
        forceFieldStrongVelocity.zMultiplier = forceFieldStrongVelocity.zMultiplier / 2F;
        if(!isAttractedToStart)
        {
            forceFieldNone.gameObject.SetActive(false);
            forceFieldWeak.gameObject.SetActive(false);
            forceFieldStrong.gameObject.SetActive(false);
            transform.parent.GetChild(0).gameObject.SetActive(false);
        }
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
        if (isAttractedToStart)
        {
            resetting = true;
        }
        if(pull == PullDirection.Locked)
        {

            Debug.Log("LOCKED");
        }
    }

    private IEnumerator Unlock()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.5F);
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
        prevForceFieldState = forceFieldState;
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
            //sr.color = srLockColor;
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

        if(resetting)
        {
            if(Mathf.Abs(transform.position.x - startPos.position.x) <= 0.015F && Mathf.Abs(transform.position.y - startPos.position.y) <= 0.015F)
            {
                forceFieldState = ForceFieldState.None;
                resetting = false;
                transform.position = startPos.position;
                rb.velocity = Vector2.zero;
                pull = PullDirection.None;
            }
            else {
                forceFieldState = ForceFieldState.Strong;
            }
        }

        if (pulling)
        {
            if (isAttractedToStart)
            {
                forceFieldState = ForceFieldState.Weak;
            }
        }

        if (isAttractedToStart)
        {
            if(!resetting && !pulling)
            {
                forceFieldState = ForceFieldState.None;
            }
            //Force field visual updates
            if (forceFieldState == ForceFieldState.None)
            {
                if (forceFieldWeak.transform.localPosition == ffVisible)
                {
                    forceFieldWeak.transform.localPosition = ffHide;
                    //ParticleSystem.VelocityOverLifetimeModule velocityModule = forceFieldWeak.velocityOverLifetime;
                    //velocityModule.zMultiplier = velocityModule.zMultiplier / 2F;
                }
                if (forceFieldStrong.transform.localPosition == ffVisible)
                {
                    forceFieldStrong.transform.localPosition = ffHide;
                }
                forceFieldNone.transform.localPosition = ffVisible;
            }
            else if (forceFieldState == ForceFieldState.Weak && prevForceFieldState != ForceFieldState.Weak)
            {
                StartCoroutine(ForceFieldTransition(forceFieldWeak));
            }
            else if (forceFieldState == ForceFieldState.Strong && prevForceFieldState != ForceFieldState.Strong)
            {
                StartCoroutine(ForceFieldTransition(forceFieldStrong));
            }
        }
    }

    private IEnumerator ForceFieldTransition(ParticleSystem ff)
    {
        if (ff == forceFieldStrong)
        {
            yield return new WaitForSeconds(0.1125F);
        }
        ff.transform.localPosition = ffVisible;
        yield return new WaitForSeconds(0.225F);
        if(ff == forceFieldWeak)
        {
            if(forceFieldState == ForceFieldState.Weak)
            {
                forceFieldStrong.transform.localPosition = ffHide;
                forceFieldNone.transform.localPosition = ffHide;
            }
        }
        else if(ff == forceFieldStrong)
        {
            if(forceFieldState == ForceFieldState.Strong)
            {
                forceFieldWeak.transform.localPosition = ffHide;
                forceFieldNone.transform.localPosition = ffHide;
            }
        }
    }

    private void FixedUpdate()
    {
        if (pull == PullDirection.Locked) return;
        SetVelocity();
        if(pulling && !resetting)
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
                if (spike.transform.position.x <= pullDest.x)
                {
                    Snap();
                }
            }
            else if (pull == PullDirection.Right)
            {
                if (spike.transform.position.x >= pullDest.x)
                {
                    Snap();
                }
            }
        }
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
            Vector3 dest = startPos.position;
            Vector3 v = 1.5F * Vector3.Normalize(new Vector3(dest.x - transform.position.x, dest.y - transform.position.y, 0F));
            rb.velocity = v;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }


    private void Snap()
    {
        Charged = false;
        resetting = false;
        pulling = false;
        rb.velocity = Vector2.zero;
        if (pull == PullDirection.Up || pull == PullDirection.Down)
        {
            spike.transform.position = new Vector3(pullDest.x, pullDest.y, transform.position.z);
        }
        else if (pull == PullDirection.Left)
        {
            spike.transform.position = new Vector3(pullDest.x, pullDest.y, transform.position.z);
        }
        else if (pull == PullDirection.Right)
        {
            spike.transform.position = new Vector3(pullDest.x, pullDest.y, transform.position.z);
        }
        snapped = true;
        Debug.Log("SNAPPED");
        //magnet.snapBlocked = true;
        mm.HideCurrentMagnetHint();
    }

    public void SetLocked(KeyLock keyLock, int index, Transform dest)
    {
        currKeyLock = keyLock;
        lockIndex = index;
        spike.transform.position = dest.position;
        snapped = false;
        pullDest = dest.position;
        pull = PullDirection.Locked;
        if (rb.velocity.x > 1F) currLockType = LockType.Right;
        else if (rb.velocity.x < 1F) currLockType = LockType.Left;
        else if (rb.velocity.y > 1F) currLockType = LockType.Up;
        else if (rb.velocity.y < 1F) currLockType = LockType.Down;
        rb.velocity = Vector2.zero;
    }
}
