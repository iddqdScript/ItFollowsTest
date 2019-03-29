using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : InventoryItemBase
{

    public int Damage = 10;

    public override void SetTag()
    {
        if (gameObject.tag != "UsableObject" && gameObject.tag != null)
        {
            gameObject.tag = "UsableObject";
        }
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
