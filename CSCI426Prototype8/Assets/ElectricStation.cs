using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricStation : MonoBehaviour
{
    public GameObject electricPrefab;
    private Music music;
    // Start is called before the first frame update
    void Awake()
    {
        music = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<Music>();
    }

    // Update is called once per frame
    void Update() { 
    //{
    //    foreach(Pair<Interactable, GameObject> c in charges)
    //    {
    //        if (!c.First.Charged)
    //        {
    //            Destroy(c.Second);
    //            charges.Remove(c);
    //            Debug.Log("REMOVE CHARGE");
    //        }
    //    }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Interactable"))
        {
            Interactable i = collision.gameObject.GetComponent<Interactable>();
            music.PlayChargeUp();
            i.Charged = true;
        }
    }
}
