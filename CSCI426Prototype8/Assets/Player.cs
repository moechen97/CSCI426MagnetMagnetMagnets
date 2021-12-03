using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject DieParticle;
    [SerializeField] private GameObject WinParticle;
    [SerializeField] public int GameLevel;
    [SerializeField] public float RateOfAcceleration;
    [SerializeField] public float DelayBeforeAcceleration;
    [SerializeField] public float startSpeed = 2F;
    [SerializeField] public float maxSpeed = 10F;
    private SpriteRenderer _spriteRenderer;
    private Vector3 startPos;
    private SWS.splineMove move;
    private float timeElapsed;
    private Color spriteOriginColor;
    private ScreenShake _screenShake;
    private MagnetMove mm;

    private bool WinParticleCreated = false;
    private VariablesSaver vs;
    private Interactable[] interactables;
    private KeyLock[] keyLocks;
    private Battery[] batteries;
    private Bomb[] bombs;
    public bool dead;
    private Music music;
    [HideInInspector] public bool dying = false;
    private TMPro.TextMeshProUGUI countdown;
    private bool countingDown = false;
    private Coroutine cd;
    void Awake()
    {
        music = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<Music>();
        GameObject[] interactableObjects = GameObject.FindGameObjectsWithTag("Interactable");
        interactables = new Interactable[interactableObjects.Length];
        for(int i = 0; i < interactableObjects.Length; i++)
        {
            interactables[i] = interactableObjects[i].GetComponentInParent<Interactable>();
        }

        GameObject[] keyLockObjects = GameObject.FindGameObjectsWithTag("KeyLock");
        keyLocks = new KeyLock[keyLockObjects.Length];
        for(int i = 0; i < keyLockObjects.Length; i++)
        {
            keyLocks[i] = keyLockObjects[i].GetComponent<KeyLock>();
        }

        GameObject[] batteryObjects = GameObject.FindGameObjectsWithTag("Battery");
        batteries = new Battery[batteryObjects.Length];
        for(int i = 0; i < batteryObjects.Length; i++)
        {
            batteries[i] = batteryObjects[i].GetComponent<Battery>();
        }

        GameObject[] bombObjects = GameObject.FindGameObjectsWithTag("Bomb");
        bombs = new Bomb[bombObjects.Length];
        for(int i = 0; i < bombObjects.Length; i++)
        {
            bombs[i] = bombObjects[i].GetComponent<Bomb>();
        }

        mm = GameObject.FindGameObjectWithTag("Magnet").GetComponent<MagnetMove>();
        vs = GameObject.FindGameObjectWithTag("VariablesSaver").GetComponent<VariablesSaver>();
        startPos = transform.position;
        move = GetComponent<SWS.splineMove>();
        timeElapsed = 0.0F;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        spriteOriginColor = _spriteRenderer.color;

        _screenShake = FindObjectOfType<ScreenShake>();
        GameObject vsc = GameObject.FindGameObjectWithTag("VolumeSliderCanvas");
        foreach(Transform child in vsc.transform) 
        {
            if(child.gameObject.name.Equals("Countdown_Text"))
            {
                countdown = child.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                cd = StartCoroutine(Count());
            }
        }
    }

    private IEnumerator Count()
    {
        countingDown = true;
        countdown.gameObject.SetActive(true);
        countdown.text = "3~";
        _spriteRenderer.enabled = false;
        yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(1F);
        countdown.text = "2~";
        yield return new WaitForSeconds(1F);
        countdown.text = "1!";
        yield return new WaitForSeconds(1F);
        countingDown = false;
        _spriteRenderer.enabled = true;
        countdown.gameObject.SetActive(false);
        countdown.text = "3~";
        move.StartMove();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            vs.ReloadLevel();
        }
        else if(Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.R))
        {
            vs.LoadLevel(0);
        }
        else if (Input.GetKeyDown(KeyCode.Minus)) vs.PreviousLevel();
        else if (Input.GetKeyDown(KeyCode.Equals)) vs.NextLevel();

        if(countingDown)
        {
            if(Input.GetMouseButtonDown(0))
            {
                countingDown = false;
                StopCoroutine(cd);
                _spriteRenderer.enabled = true;
                countdown.gameObject.SetActive(false);
                vs.deathCountTitleText.gameObject.SetActive(false);
                vs.deathCountText.gameObject.SetActive(false);
                countdown.text = "3~";
                move.StartMove();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Interactable") && collision.gameObject.GetComponentInParent<Interactable>().pull != Interactable.PullDirection.Locked)
        {
            music.PlayHit();
            Die();
        }
        else if(collision.gameObject.CompareTag("KeyLock") || collision.gameObject.CompareTag("Block"))
        {
            if(collision.gameObject.CompareTag("KeyLock") && !collision.GetComponent<KeyLock>().isLocked)
            {
                Die();
            }
            else if(collision.gameObject.CompareTag("Block") && !collision.GetComponentInParent<KeyLock>().isLocked)
            {
                Die();
            }
            
        }
        else if(collision.gameObject.CompareTag("Battery"))
        {
            if(!collision.gameObject.GetComponent<Battery>().charged)
            {
                Die();
                music.PlayElectricDie();
            }
        }
        else if(collision.gameObject.CompareTag("BombLock"))
        {
            Die();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Check for gates
        if (collision.gameObject.CompareTag("Block") && !collision.GetComponentInParent<KeyLock>().isLocked)
        {
            Die();
        }
        else if (collision.gameObject.CompareTag("Battery"))
        {
            if (!collision.gameObject.GetComponent<Battery>().charged)
            {
                //Die();
                //music.PlayElectricDie();
            }
        }
        else if(collision.gameObject.CompareTag("BombLock"))
        {
            Die();
            music.PlayBombExplode();
        }
    }

    public bool Die()
    {
        if(dying)
        {
            return true;
        }
        transform.position = startPos;
        vs.RecordDeath();
        vs.deathCountTitleText.gameObject.SetActive(true);
        vs.deathCountText.gameObject.SetActive(true);
        StartCoroutine(FailTimer());
        dying = true;
        mm.SetMagnetPos(0);
        Instantiate(DieParticle, transform.position, quaternion.identity);
        _spriteRenderer.color = new Color(0, 0, 0, 0);
        _screenShake.ShakeScreen();
        move.Stop();
        StartCoroutine(RestartLevel());
        return false;
    }

    private IEnumerator FailTimer()
    {
        yield return new WaitForSeconds(1.5F);
        vs.deathCountTitleText.gameObject.SetActive(false);
        vs.deathCountText.gameObject.SetActive(false);
    }

    IEnumerator RestartLevel()
    {
        foreach (Interactable i in interactables)
        {
            i.ResetStage();
        }
        foreach(KeyLock k in keyLocks)
        {
            k.ResetLock();
        }
        foreach(Battery b in batteries)
        {
            b.UnCharge();
        }
        foreach(Bomb b in bombs)
        {
            b.Reset();
        }
        countingDown = true;
        cd = StartCoroutine(Count());
        yield return new WaitUntil(() => !countingDown);
        move.ResetToStart();
        move.StartMove();
        timeElapsed = 0.0F;
        _spriteRenderer.color = spriteOriginColor;
        dying = false;
    }

    public void createWinParticle()
    {
        if (!WinParticleCreated)
        {
            Instantiate(WinParticle, transform.position, quaternion.identity);
            WinParticleCreated = true;
            music.PlayWin();
            StartCoroutine(NextLevelDelay());
        }
    }

    private IEnumerator NextLevelDelay()
    {
        yield return new WaitForSeconds(1.5F);
        vs.NextLevel();
    }
}
