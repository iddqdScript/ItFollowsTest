using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : InventoryItemBase {

    public int Damage = 10;

   void start()
    {
       // gameObject.tag = "Player";
    }

    public override void OnUse()
    {
        base.OnUse();
    }

    public override void ObjectInteract()
    {
        // Specific Interaction code for Axes...

        // If also want to call generic stuff then add
        base.ObjectInteract();
    }



    }
