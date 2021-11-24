using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Research : MonoBehaviour
{
    private VariablesSaver vs;
    [SerializeField] TMPro.TextMeshProUGUI research1;
    [SerializeField] TMPro.TextMeshProUGUI research2;
    //[SerializeField] Text research2;
    // Start is called before the first frame update
    void Start()
    {
        vs = GameObject.FindGameObjectWithTag("VariablesSaver").GetComponent<VariablesSaver>();
        if(!vs.research[0])
        {
            research1.text = "2. ??????";
            research1.fontSize = 52.7F;
            Vector3 shift = research1.gameObject.transform.position;
            if (!vs.research[1])
            {
                shift.y -= 18F;
            }
            else
            {
                shift.y += 3.75F;
            }
            research1.gameObject.transform.position = shift;
        }
        else if(vs.research[1])
        {
            Vector3 shift = research1.gameObject.transform.position;
            shift.y += 26.75F;
            research1.gameObject.transform.position = shift;
        }
        else
        {
            Vector3 shift = research1.gameObject.transform.position;
            shift.y += 16.75F;
            research1.gameObject.transform.position = shift;
        }

        if(!vs.research[1])
        {
            research2.text = "3. ??????";
            research2.fontSize = 52.7F;
        }
        else
        {
            Vector3 shift = research1.gameObject.transform.position;
            shift.y -= 20.125F;
            research1.gameObject.transform.position = shift;
        }
    }
}
