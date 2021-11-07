using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBeam : MonoBehaviour
{
    public Battery battery;
    public GameObject attached; //interactable
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float s = Mathf.Abs(transform.parent.position.y - attached.transform.position.y) / 3.5F;
        transform.localScale = new Vector3(s, s, 1F);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Battery"))
        {
            battery.charged = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Battery"))
        {
            battery.charged = false;
        }
    }
}
