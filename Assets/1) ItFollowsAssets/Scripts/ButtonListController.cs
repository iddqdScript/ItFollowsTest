using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListController : MonoBehaviour {

    [SerializeField]
    private GameObject _buttonTemplate;
    private static List<string> _menuitemlist = new List<string>();
    private HUD _Hud;
    private GameObject _ActivePlayer;
    private PlayerController _playerController;
    public InteractableItemBase _item = null;
    

   

    public List<string> _Menuitemlist 
    {
        get { return _menuitemlist; }
        set { _menuitemlist = value; } 


    }

    void Start()
    {
        _ActivePlayer = GameObject.Find("Low Poly Warrior");
        _Hud = GameObject.FindObjectOfType<HUD>();
        _playerController = GameObject.FindObjectOfType<PlayerController>();
        //    Button _buttonTemplate = gameObject.transform.Find("HUD/RightClickMenu/ButtonListViewport/ButtonListContent/Button").GetComponent<Button>();
        //    //_buttonTemplate = Resources.Load("HUD/RightClickMenu/ButtonListViewport/ButtonListContent/ButtonListContent") as GameObject;
    }
    
    public void GenerateList()
    {
        //Destroying all the buttons in the list before making new ones
        DestroyAllButtonsInList();
        CreateButtons();
    }

    //The controller handles the click instead of the button, info is being passed back from the ButtonListButton Script
    public void ButtonClicked(string _myTextString)
    {
       // Debug.Log(_myTextString);
        if (_myTextString == "Pick Up")
        {
            _playerController.RightClickInteractWithAnItem(_item);
            _Hud.CloseRightClickMenu();
        }
        else if(_myTextString == "Examine")
            {
            _Hud.CloseRightClickMenu();
            Debug.Log(_item._examineText);
        }
        else if (_myTextString == "Walk Here")
        {
            _Hud.CloseRightClickMenu();

        }
        else if (_myTextString == "Cancel")
        {
            _Hud.CloseRightClickMenu();

        }
    }

        public void ClearList()
    {
        _menuitemlist.Clear();
    }

    public void DestroyAllButtonsInList()
    {
        
        if (GetComponentInChildren<ButtonListButton>() != null)
        {
            ButtonListButton[] BLB = GetComponentsInChildren<ButtonListButton>();
            foreach (ButtonListButton i in BLB)
            {
                Destroy(i.gameObject);
            }
            Debug.Log("Buttons Deleted");
        }
    }

    public void CreateButtons()
    {
        for (int i = 0; i < _menuitemlist.Count; i++)
        {
            GameObject _button = Instantiate(_buttonTemplate) as GameObject;
            _button.SetActive(true);
            _button.GetComponent<ButtonListButton>().SetText(_menuitemlist[i]);
            _button.transform.SetParent(_buttonTemplate.transform.parent, false);
        }
    }

}


//if (_menuitemlist.Count == 0)
//{
//    //_menuitemlist.Clear();
//    _menuitemlist.Add("Walk Here");
//    _menuitemlist.Add("Examine");
//    _menuitemlist.Add("Cancel");

//    for (int i = 0; i < _menuitemlist.Count; i++)
//    {
//        GameObject _button = Instantiate(_buttonTemplate) as GameObject;
//        _button.SetActive(true);

//        _button.GetComponent<ButtonListButton>().SetText(_menuitemlist[i]);

//        _button.transform.SetParent(_buttonTemplate.transform.parent, false);
//        //_button.GetComponent<ButtonListButton>().on;
//        //_button.transform.
//    }
////}
////else
////{
//    //_menuitemlist.Clear();
//    for (int i = 0; i < _menuitemlist.Count; i++)
//    {
//        GameObject _button = Instantiate(_buttonTemplate) as GameObject;
//        _button.SetActive(true);

//        _button.GetComponent<ButtonListButton>().SetText(_menuitemlist[i]);

//        _button.transform.SetParent(_buttonTemplate.transform.parent, false);
//        //_button.GetComponent<ButtonListButton>().on;
//        //_button.transform.

//    }
//}