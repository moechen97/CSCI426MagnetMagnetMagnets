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
       if(vs.research[0] && vs.research[1] && vs.research[2])
        {
            Vector3 magnetImageShift = magnetImage.transform.position;
            magnetImageShift.y -= 25F;
            magnetImage.transform.position = magnetImageShift;
            magnetImage.color = new Color(195F / 255F, 133F / 255F, 133F / 255F);
        }
       else if(vs.research[0] && vs.research[1])
        {
            research3.text = "4. ?????";
            research3.fontSize = research3.fontSize * 1.125F;
            research3.fontStyle = FontStyles.Bold;
        }
       else if(vs.research[0])
        {
            research2.text = "3. ?????";
            research2.fontSize = research2.fontSize * 1.125F;
            research2.fontStyle = FontStyles.Bold;
            research3.text = "4. ?????";
            research3.fontSize = research3.fontSize * 1.125F;
            research3.fontStyle = FontStyles.Bold;
        }
       else
        {
            research1.text = "2. ?????";
            research1.fontSize = research1.fontSize * 1.125F;
            research1.fontStyle = FontStyles.Bold;
            research2.text = "3. ?????";
            research2.fontSize = research2.fontSize * 1.125F;
            research2.fontStyle = FontStyles.Bold;
            research3.text = "4. ?????";
            research3.fontSize = research3.fontSize * 1.125F;
            research3.fontStyle = FontStyles.Bold;
        }
    }
}
