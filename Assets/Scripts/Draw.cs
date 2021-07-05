using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;


public class Draw : MonoBehaviour
{
    [SerializeField] float distanceFromCamera;
    [SerializeField] Camera arCam;
    [SerializeField] GameObject lineRendererPrefab;
    [SerializeField] ARAnchorManager anchorManager;
    [SerializeField] BrushSetting brushSetting;

    public LineRenderer line;
    HashSet<Vector3> points = new HashSet<Vector3>();

    //For Undo Function
    Stack<LineRenderer> lineStack = new Stack<LineRenderer>();
    private void Update()
    {
#if UNITY_EDITOR
        DrawOnMouse();
#endif
#if UNITY_ANDROID || UNITY_IPHONE
        DrawOnTouch();
#endif
    }

    private void DrawOnMouse()
    {
        if (Input.GetMouseButton(0) && !IsOverUI())
        {
            Vector3 mousePosition = arCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera));
            //check mouse position not on top of UI

            //Identify first click OR continous dragging.
            if (points.Count > 0)
            {
                UpdateLineRenderer(mousePosition);
            }
            else
            {
                InitializeLineRenderer(mousePosition);
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            points.Clear();
        }

    }

    private void DrawOnTouch()
    {
        if (Input.touchCount > 0 && !IsOverUI())
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = arCam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, distanceFromCamera));

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    InitializeLineRenderer(touchPosition);
                    break;
                case TouchPhase.Moved:
                    UpdateLineRenderer(touchPosition);
                    break;
                case TouchPhase.Stationary:
                    UpdateLineRenderer(touchPosition);
                    break;
                case TouchPhase.Ended:
                    break;

            }
        }
    }

    void InitializeLineRenderer(Vector3 pos)
    {
        GameObject lineGO = new GameObject($"Line");
       // GameObject lineGO= Instantiate(lineRendererPrefab, pos,Quaternion.identity);
        lineGO.transform.position = pos;

        lineGO.AddComponent<ARAnchor>();
        line = lineGO.AddComponent<LineRenderer>();
        lineStack.Push(line);


        brushSetting.DefaultLineSetting(line);

        line.SetPosition(0, pos);
        line.SetPosition(1, pos);
        points.Add(pos);
    }
    void UpdateLineRenderer(Vector3 pos)
    {
        line.positionCount++;
        line.SetPosition(line.positionCount-1, pos);
        points.Add(pos);
    }

    bool IsOverUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void UndoDrawing()
    {
        if (lineStack.Count > 0)
        {
            LineRenderer lineToDelete = lineStack.Peek();
            lineStack.Pop();
            Destroy(lineToDelete.gameObject);
        }
    }

    public void SetDistanceFromCamera(float dist)
    {
        distanceFromCamera = dist;
    }




}
