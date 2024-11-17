using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInGame : MonoBehaviour
{
    public string itemMessage;
    public bool isHeld = false;

    public void PickUP ()
    {
        isHeld = true;

    }
    public void Drop () 
    {
        isHeld=false;
    }
}
