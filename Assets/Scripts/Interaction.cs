using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction
{
    public GameObject interactionGo;

    public Interaction(GameObject obj)
    {
        interactionGo = obj;
    }
    public void Undo()
    {
        Object.Destroy(interactionGo);
    }
}