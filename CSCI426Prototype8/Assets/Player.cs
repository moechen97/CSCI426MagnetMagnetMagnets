using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject DieParticle;
    [SerializeField] private GameObject WinParticle;
    private SpriteRenderer _spriteRenderer;
    private Vector3 startPos;
    private SWS.splineMove move;
    public Battery battery;
    private float timeElapsed;
    private Color spriteOriginColor;
    private ScreenShake _screenShake;
    private MagnetMove mm;

    private bool WinParticleCreated = false;
    private VariablesSaver vs;
    private Interactable[] interactables;
    private KeyLock[] keyLocks;
    private Battery[] batteries;
    public bool dead;
    private Music music;
    void Awake()
    {
        music = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<Music>();
        GameObject[] interactableObjects = GameObject.FindGameObjectsWithTag("Interactable");
        interactables = new Interactable[interactableObjects.Length];
        for(int i = 0; i < interactableObjects.Length; i++)
        {
            interactables[i] = interactableObjects[i].GetComponent<Interactable>();
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

        mm = GameObject.FindGameObjectWithTag("Magnet").GetComponent<MagnetMove>();
        vs = GameObject.FindGameObjectWithTag("VariablesSaver").GetComponent<VariablesSaver>();
        startPos = transform.position;
        move = GetComponent<SWS.splineMove>();
        timeElapsed = 0.0F;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        spriteOriginColor = _spriteRenderer.color;

        _screenShake = FindObjectOfType<ScreenShake>();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            vs.ReloadLevel();
        }
        else if (Input.GetKeyDown(KeyCode.Minus)) vs.PreviousLevel();
        else if (Input.GetKeyDown(KeyCode.Equals)) vs.NextLevel();
        //MUSIC TEST
        else if(Input.GetKeyDown(KeyCode.M))
        {
            music.SwapBackgroundMusic();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Interactable") && collision.gameObject.GetComponent<Interactable>().pull != Interactable.PullDirection.Locked)
        {
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
                Die();
                //music.PlayElectricDie();
            }
        }
    }

    public void Die()
    {
        Instantiate(DieParticle, transform.position, quaternion.identity);
        _spriteRenderer.color = new Color(0, 0, 0, 0);
        _screenShake.ShakeScreen();
        move.Stop();
        StartCoroutine(RestartLevel());
    }

    IEnumerator RestartLevel()
    {
        mm.SetMagnetPos(0);
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
        yield return new WaitForSeconds(1f);
        move.ResetToStart();
        move.StartMove();
        timeElapsed = 0.0F;
        _spriteRenderer.color = spriteOriginColor;
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
