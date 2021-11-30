using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{

    private SpriteRenderer sr;
    // public static Sprite currentSprite;
    public static bool isPointer = false;
    [SerializeField] private Sprite hand;
    [SerializeField] private Sprite pointerfinger;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;
        if (isPointer) {
            sr.sprite = pointerfinger;

        }
        else {
            sr.sprite = hand;
        }
    }

    // public static void SetMouseCursor(Sprite cursor) {
    //     sr.sprite = cursor;
    // }

    // void OnTriggerEnter2D(Collider2D other) {
    //     if (other.tag == "Magnet_Up" || other.tag == "Magnet_Down" || other.tag == "Magnet_Left" || other.tag == "Magnet_Right") {
    //         sr.sprite = pointer;
    //     }

    // }
    // void OnTriggerExit2D(Collider2D other) {
    //     sr.sprite = def;
    // }
}
