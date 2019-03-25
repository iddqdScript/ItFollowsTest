using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListButton : MonoBehaviour {

    [SerializeField]
    private Text _mytext;
    [SerializeField]
    private ButtonListController _buttonListController;

    public PlayerController _player;
   // public static GameObject ActivePlayer;

    void Start()
    {
        //    Button _buttonTemplate = gameObject.transform.Find("HUD/RightClickMenu/ButtonListViewport/ButtonListContent/Button").GetComponent<Button>();
        //    //_buttonTemplate = Resources.Load("HUD/RightClickMenu/ButtonListViewport/ButtonListContent/ButtonListContent") as GameObject;
       // ActivePlayer = GameObject.Find("Low Poly Warrior");
        _player = GameObject.FindObjectOfType<PlayerController>();

    }

    private string _myTextString;

    public void SetText(string _textString)
    {

        _myTextString = _textString;
        _mytext.text = _textString; //find theh text attached to the button and set it to be the string passed into this method
    }

    public void OnClick()//pass the information back to the buttonController so it does the work
    {
        _buttonListController.ButtonClicked(_myTextString);
    }

}
