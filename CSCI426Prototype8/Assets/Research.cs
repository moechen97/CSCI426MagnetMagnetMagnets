using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Research : MonoBehaviour
{
    private VariablesSaver vs;
    [SerializeField] TMPro.TextMeshProUGUI research1;
    [SerializeField] TMPro.TextMeshProUGUI research2;
    [SerializeField] TMPro.TextMeshProUGUI research3;
    [SerializeField] Image magnetImage;
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
            Vector3 shift = research2.gameObject.transform.position;
            shift.y -= 20.125F;
            research2.gameObject.transform.position = shift;
        }

        if(!vs.research[2])
        {
            research3.text = "4. ??????";
            research3.fontSize = 52.7F;
            Vector3 shift = research3.gameObject.transform.position;
            shift.y = 245F;
            research3.gameObject.transform.position = shift;
        }
        else
        {
            Vector3 shift = research3.gameObject.transform.position;
            shift.y = 213.375F;
            research3.gameObject.transform.position = shift;
        }

        if(!vs.research[0] && !vs.research[1] && !vs.research[2])
        {

        }
        else if(vs.research[0] && !vs.research[1] && !vs.research[2])
        {

        }
        else if(vs.research[0] && vs.research[1] && !vs.research[2])
        {
            Vector3 shift = research3.gameObject.transform.position;
            shift.y -= 43F;
            research3.gameObject.transform.position = shift;

            shift = research2.gameObject.transform.position;
            shift.y += 33F;
            research2.gameObject.transform.position = shift;
        }
       else if(vs.research[0] && vs.research[1] && vs.research[2])
        {
            Vector3 shift = research3.gameObject.transform.position;
            shift.y += 46F;
            research3.gameObject.transform.position = shift;

            shift = research2.gameObject.transform.position;
            shift.y += 57F;
            research2.gameObject.transform.position = shift;

            Vector3 magnetImageShift = magnetImage.transform.position;
            magnetImageShift.y -= 25F;
            magnetImage.transform.position = magnetImageShift;
            magnetImage.color = new Color(195F / 255F, 133F / 255F, 133F / 255F);
        }
    }
}
