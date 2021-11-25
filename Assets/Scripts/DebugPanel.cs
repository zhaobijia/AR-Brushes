using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DebugPanel : MonoBehaviour
{
    public TMP_Text lineCountsText;
    Draw mainDrawing;

    private void Start()
    {
        mainDrawing = FindObjectOfType<Draw>();

    }
    private void Update()
    {
        ShowLineCount();
    }
    public void ShowLineCount()
    {
        lineCountsText.text = mainDrawing.lineStack.Count.ToString();
    }

}
