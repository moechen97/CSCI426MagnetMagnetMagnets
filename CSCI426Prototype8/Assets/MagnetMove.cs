using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetMove : MonoBehaviour
{
    enum Direction { Left, Right }
    public enum Quadrant { None, Up, Down, Left, Right, DiagonalLeft, DiagonalRight } //position of magnet and direction of pull
    private Transform[] magnets;
    private Quadrant[] quadrants;
    private int size;
    public Quadrant currQuad;
    private Magnet magnet;
    public int magnetIndex;
    private int numMagnets;
    private int magnetLimit;
    private Music music;
    void Awake()
    {
        GameObject magnetPositions = GameObject.FindGameObjectWithTag("MagnetPositions");
        magnets = new Transform[magnetPositions.transform.childCount];
        for(int i = 0; i < magnetPositions.transform.childCount; i++)
        {
            Debug.Log(magnetPositions.transform.GetChild(i).gameObject.name);
            magnets[i] = magnetPositions.transform.GetChild(i).transform;
        }
        music = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<Music>();
        numMagnets = 0;
        magnetLimit = 1;
        magnet = GetComponent<Magnet>();
        size = magnets.Length;
        quadrants = new Quadrant[magnets.Length];
        for(int i = 0; i < magnets.Length; i++) 
        {
            Quadrant q;
            if (magnets[i].gameObject.CompareTag("Magnet_None"))
            {
                q = Quadrant.None;
            }
            else if (magnets[i].gameObject.CompareTag("Magnet_Up"))
            {
                q = Quadrant.Up;
            }
            else if (magnets[i].gameObject.CompareTag("Magnet_Down"))
            {
                q = Quadrant.Down;
            }
            else if (magnets[i].gameObject.CompareTag("Magnet_Left"))
            {
                q = Quadrant.Left;
            }
            else if (magnets[i].gameObject.CompareTag("Magnet_Right"))
            {
                q = Quadrant.Right;
            }
            else if (magnets[i].gameObject.CompareTag("Magnet_DiagonalLeft"))
            {
                q = Quadrant.DiagonalLeft;
            }
            else //if (magnets[i].gameObject.CompareTag("Magnet_DiagonalRight"))
            {
                q = Quadrant.DiagonalRight;
            }
            quadrants[i] = q;
        }
        currQuad = quadrants[magnetIndex];
        UpdateMagnetPos();
        UpdateMagnetRotation();
    }


    private void UpdateMagnetPos()
    {
        transform.position = magnets[magnetIndex].position;
        currQuad = quadrants[magnetIndex];
        UpdateMagnetRotation();
        magnet.ResetPulls();
    }

    private void UpdateMagnetRotation()
    {
        if (currQuad == Quadrant.None)
        {
            transform.eulerAngles = new Vector3(0F, 0F, 180F);
            transform.GetChild(1).transform.eulerAngles = new Vector3(0F, 0F, 0F);
        }
        else if(currQuad == Quadrant.Down)
        {
            transform.eulerAngles = new Vector3(0F, 0F, 0F);
            transform.GetChild(1).transform.eulerAngles = new Vector3(0F, 0F, 180F);
        }
        else if (currQuad == Quadrant.Up)
        {
            transform.eulerAngles = new Vector3(0F, 0F, 180F);
            transform.GetChild(1).transform.eulerAngles = new Vector3(0F, 0F, 0F);
        }
        else if(currQuad == Quadrant.Left)
        {
            transform.eulerAngles = new Vector3(0F, 0F, -90F);
            transform.GetChild(1).transform.eulerAngles = new Vector3(0F, 0F, 90F);
        }
        else if(currQuad == Quadrant.Right)
        {
            transform.eulerAngles = new Vector3(0F, 0F, 90F);
            transform.GetChild(1).transform.eulerAngles = new Vector3(0F, 0F, -90F);
        }
        else if(currQuad == Quadrant.DiagonalLeft)
        {
            transform.eulerAngles = new Vector3(0F, 0F, 45F);
            transform.GetChild(1).transform.eulerAngles = new Vector3(0F, 0F, 45F);
        }
    }

    public void SetMagnetPos(int index) 
    {
        if (index >= magnets.Length) return;
        else if (index >= 0)
        {
            music.PlayChangeMagnetPosition();
            transform.position = magnets[index].position;
            magnetIndex = index;
            currQuad = quadrants[magnetIndex];
            UpdateMagnetRotation();
            magnet.ResetPulls();
        }
        else Debug.Log("BUG: SetMagnetPos(index) less than zero");
    }

    public void HideCurrentMagnetHint()
    {
        Debug.Log("HIDE");
        transform.GetChild(2).gameObject.SetActive(false);
    }

    public void UnhideCurrentMagnetHint()
    {
        transform.GetChild(2).gameObject.SetActive(true);
    }
}
