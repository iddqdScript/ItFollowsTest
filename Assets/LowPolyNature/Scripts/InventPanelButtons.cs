using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventPanelButtons : MonoBehaviour
{

    public Button _inventButton,_skillsButton;
    public GameObject _inventPanel,_skillsPanel;

    bool inSkills = false;
    bool inInvent = true;

	// Use this for initialization
	void Start ()
    {
       
        _inventButton.onClick.AddListener(InventButtonOnClick);
        _skillsButton.onClick.AddListener(SkillsButtonOnClick);
        

    }

    // Update is called once per frame
    void Update ()
    {

    }

    void InventButtonOnClick()
    {
        inInvent = true;
        inSkills = false;
        Debug.Log("Invent Button Clicked");
        Debug.Log("inInvent = " + inInvent);
        Debug.Log("inSkills = " + inSkills);

        if (_inventPanel.gameObject.active == false)
        {
            _inventPanel.gameObject.SetActive(true);
            _skillsPanel.gameObject.SetActive(false);
        }
        else
        {

        }
        
    }

    void SkillsButtonOnClick()
    {
        inSkills = true;
        inInvent = false;
        Debug.Log("Skills Button Clicked");
        Debug.Log("inInvent = " + inInvent);
        Debug.Log("inSkills = " + inSkills);

        if (_skillsPanel.gameObject.active == false)
        {
            _skillsPanel.gameObject.SetActive(true);
            _inventPanel.gameObject.SetActive(false);
        }
        else
        {

        }

    }

}
