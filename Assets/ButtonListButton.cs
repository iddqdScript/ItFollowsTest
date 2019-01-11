using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListButton : MonoBehaviour {

    [SerializeField]
    private Text _mytext;
    [SerializeField]
    private ButtonListController _buttonListController;

    private string _myTextString;

    public void SetText(string _textString)
    {
        _mytext.text = _textString; //find theh text attached to the button and set it to be the string passed into this method
    }

    public void OnClick()//pass the information back to the buttonController so it does the work
    {
        _buttonListController.ButtonClicked(_myTextString);
    }

}
