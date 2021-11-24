using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public Stack<Interaction> interactionStack = new Stack<Interaction>();


    public void UndoInteraction()
    {
        if (interactionStack.Count > 0)
        {
            Interaction itn = interactionStack.Pop();
            itn.Undo();
        }
    }

    public void StoreInteraction(GameObject obj)
    {
        Interaction itn = new Interaction(obj);
        interactionStack.Push(itn);
    }
}
