using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{

    private SpriteRenderer sr;
    // public static Sprite currentSprite;
    public static bool isPointer = false;
    [SerializeField] private Texture2D hand;
    [SerializeField] private Texture2D pointerfinger;
    [SerializeField] private Texture2D grab;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        //sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;
        if (Input.GetMouseButton(0)) {
            //sr.sprite = pointerfinger;
            Cursor.SetCursor(grab, Vector2.zero, CursorMode.Auto);
        }
        else if (isPointer)
        {
            Cursor.SetCursor(pointerfinger, Vector2.zero, CursorMode.Auto);

        }
        else {
            //sr.sprite = hand;
            Cursor.SetCursor(hand, Vector2.zero, CursorMode.Auto);
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
