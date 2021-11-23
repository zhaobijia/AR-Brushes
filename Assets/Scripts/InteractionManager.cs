using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public Stack<Interaction> interactionStack;
    public void UndoInteraction()
    {
        Interaction itn = interactionStack.Pop();
        itn.Undo();
    }

    public void StoreInteraction(GameObject obj)
    {
        Interaction itn = new Interaction(obj);
        interactionStack.Push(itn);
    }
}
