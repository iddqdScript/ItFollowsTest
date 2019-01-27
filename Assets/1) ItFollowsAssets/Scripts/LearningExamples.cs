using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningExamples
{


    void getcomponentinparent()
    {
       /* 
        In a nutshell..
        FindComponentUpwards == GetComponentInParent
        FindComponentDownward == GetComponentInChildren

        If you just want to access the transform of the direct parent object, use:
        Character = transform.parent;

        If you want to access the top most parent, use:
        Character = transform.root;
        */
    }

    void AccessingVariablesEtcFromOtherClasses()
    {
        /* 
         If I add multiple scripts to one game object, I can find the script and get whatever I want off it with GetComponent<ScriptName>().variable/Method;

        I can use properties, if a variable is private (cant be accessed outside the class even when instanciated) I can set the value of the private variable or get
        it's value instead without making it public


         */
    }





}
