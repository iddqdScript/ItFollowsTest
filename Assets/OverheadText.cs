using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OverheadText : MonoBehaviour
{

    public TextMeshProUGUI _overheadText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClampOverheadText();
    }


    void ClampOverheadText()
    {
        Vector3 namePose = Camera.main.WorldToScreenPoint(this.transform.position);
        _overheadText.transform.position = namePose;
    }
}
