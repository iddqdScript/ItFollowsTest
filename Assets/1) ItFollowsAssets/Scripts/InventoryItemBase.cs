using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//
// Interacting with items, Like the axe on the ground
//



public enum EItemType
{
    Default,
    Consumable,
    Weapon
}

public class InteractableItemBase : MonoBehaviour
{
    public string Name;
    public virtual string _tag { get; protected set; }
    public Sprite Image;
    public string InteractText = "Press F to pickup the item";
    public EItemType ItemType;
    public Vector3 PickPosition; //The chosen position of the item on the character (set in the item inspector)
    public Vector3 PickRotation; //The chosen rotation of the item on the character (set in the item inspector)
    public Vector3 DropRotation;
    public bool UseItemAfterPickup = false;


    void Start()
    {
        
        SetTag();
    }

    public virtual void SetTag()
    {
       //gameObject.tag = "Shadow";
    }

    public virtual void ObjectInteract()
    {

    }


    public virtual void OnInteractAnimation(Animator animator)
    {
        animator.SetTrigger("tr_pickup");
    }

    public virtual void OnInteract()
    {
    }

    public virtual bool CanInteract(Collider other)
    {
        return true;   
    }
}

public class InventoryItemBase : InteractableItemBase
{
    public InventorySlot Slot
    {
        get; set;
    }

    public virtual void OnUse()
    {
        transform.localPosition = PickPosition;
        transform.localEulerAngles = PickRotation;
    }

    public virtual void OnDrop()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            gameObject.SetActive(true);
            gameObject.transform.position = hit.point;
            gameObject.transform.eulerAngles = DropRotation;
        }
    }

    public virtual void OnPickup()
    {
        Destroy(gameObject.GetComponent<Rigidbody>());
        gameObject.SetActive(false);
        
    }




}
