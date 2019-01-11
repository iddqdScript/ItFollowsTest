using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListController : MonoBehaviour {

    [SerializeField]
    private GameObject _buttonTemplate;
    

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject _button = Instantiate(_buttonTemplate) as GameObject;
            _button.SetActive(true);

            _button.GetComponent<ButtonListButton>().SetText("Button # " + i);

            _button.transform.SetParent(_buttonTemplate.transform.parent, false);
        }
    }

    //The controller handles the click instead of the button, info is being passed back from the ButtonListButton Script
    public void ButtonClicked(string _myTextString)
    {
        Debug.Log(_myTextString);
    }



}
