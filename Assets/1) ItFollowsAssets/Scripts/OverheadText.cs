using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OverheadText : MonoBehaviour
{

    public TextMeshProUGUI _overheadText;
    public HUD _HUD;

    // Start is called before the first frame update
    void Start()
    {

        _HUD = GameObject.FindObjectOfType<HUD>();
        Transform _hud = _HUD.transform;
        //Debug.Log("HUD " + _hud);

        TextMeshProUGUI _generatedTextGameObject = _HUD.CreateText(_hud, 0, 200, "Hi!", 20, TextAlignmentOptions.Center);
        Debug.Log("_generatedTextGameObject " + _generatedTextGameObject);
        _overheadText = _generatedTextGameObject.GetComponent<TextMeshProUGUI>();
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





    //create new overheadtext TMP for character
    //Attach generated TMP object to sphere
   

}
