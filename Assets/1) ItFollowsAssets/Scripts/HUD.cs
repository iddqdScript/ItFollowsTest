using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HUD : MonoBehaviour {

    public Inventory Inventory;

    public GameObject MessagePanel;
    

	// Use this for initialization
	void Start () {
        Inventory.ItemAdded += InventoryScript_ItemAdded;
        Inventory.ItemRemoved += Inventory_ItemRemoved;
	}

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("InventPanelButtons/InventoryPanel");
        int index = -1;
        foreach (Transform slot in inventoryPanel)
        {
            index++;

            // Border... Image
            Transform imageTransform = slot.GetChild(0).GetChild(0);
            Transform textTransform = slot.GetChild(0).GetChild(1);
            Image image = imageTransform.GetComponent<Image>();
            Text txtCount = textTransform.GetComponent<Text>();
            ItemDragHandler itemDragHandler = imageTransform.GetComponent<ItemDragHandler>();

            if(index == e.Item.Slot.Id)
            {
                image.enabled = true;
                image.sprite = e.Item.Image;

                int itemCount = e.Item.Slot.Count;
                if (itemCount > 1)
                    txtCount.text = itemCount.ToString();
                else
                    txtCount.text = "";
                         

                // Store a reference to the item
                itemDragHandler.Item = e.Item;

                break;
            }
        }
    }

    private void Inventory_ItemRemoved(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("InventPanelButtons/InventoryPanel");

        int index = -1;
        foreach (Transform slot in inventoryPanel)
        {
            index++;

            Transform imageTransform = slot.GetChild(0).GetChild(0);
            Transform textTransform = slot.GetChild(0).GetChild(1);

            Image image = imageTransform.GetComponent<Image>();
            Text txtCount = textTransform.GetComponent<Text>();

            ItemDragHandler itemDragHandler = imageTransform.GetComponent<ItemDragHandler>();

            // We found the item in the UI
            if (itemDragHandler.Item == null)
                continue;

            // Found the slot to remove from
            if(e.Item.Slot.Id == index)
            {
                int itemCount = e.Item.Slot.Count;
                itemDragHandler.Item = e.Item.Slot.FirstItem;

                if(itemCount < 2)
                {
                    txtCount.text = "";
                }
                else
                {
                    txtCount.text = itemCount.ToString();
                }

                if(itemCount == 0)
                {
                    image.enabled = false;
                    image.sprite = null;
                }
                break;
            }
           
        }
    }

    private bool mIsMessagePanelOpened = false;

    public bool IsMessagePanelOpened
    {
        get { return mIsMessagePanelOpened; }
    }

    public void OpenMessagePanel(InteractableItemBase item)
    {
        MessagePanel.SetActive(true);

        Text mpText = MessagePanel.transform.Find("Text").GetComponent<Text>();
        mpText.text = item.InteractText;
        

        mIsMessagePanelOpened = true;


    }

    public void OpenMessagePanel(string text)
    {
        MessagePanel.SetActive(true);

        Text mpText = MessagePanel.transform.Find("Text").GetComponent<Text>();
        mpText.text = text;


        mIsMessagePanelOpened = true;
    }

    public void CloseMessagePanel()
    {
        MessagePanel.SetActive(false);

        mIsMessagePanelOpened = false;
    }

    public void RightClickMenu()
    {
        //Vector3 mouseposition
        float _mouseposX = Input.mousePosition.x;
        float _mouseposY = Input.mousePosition.y;
        float _mouseposZ = Input.mousePosition.z;
        
        var mp = Input.mousePosition;

       // Debug.Log("Pressed right click.");
        Transform _findRightClickMenu = gameObject.transform.Find("RightClickMenu");
        _findRightClickMenu.gameObject.SetActive(true);
        // Debug.Log("Position = " + g.position);
        _findRightClickMenu.position = new Vector3(_mouseposX+20, _mouseposY-55, 0);
        Debug.Log("In RightClickMenu (HUD)");


    }

    public void CloseRightClickMenu()
    {
        Transform g = gameObject.transform.Find("RightClickMenu");
        g.gameObject.SetActive(false);
    }

    public void SetSelectedText(string _selectedText)
    {
        Text _tag = gameObject.transform.Find("SelectedPanel/Tag").GetComponent<Text>();
        _tag.text = _selectedText;
        
       
    }


    public bool _isMouseOverRightClickMenu()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

}
