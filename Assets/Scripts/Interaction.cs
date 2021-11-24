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
        interactionGo.SetActive(false);
        GameObject.Destroy(interactionGo);
    }
}