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
    [SerializeField] float updateDrawingFrames;
    float frameTimer;
    public CustomLineRenderer line;
    HashSet<Vector3> points = new HashSet<Vector3>();

    //public InteractionManager interactionManager;
    Stack<CustomLineRenderer> lineStack = new Stack<CustomLineRenderer>();

    private void Update()
    {
        if (frameTimer > 0)
        {
            frameTimer -= 1;
        }
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
            if (brushSetting.settingIsOn)
            {
                brushSetting.ToggleColorPicker();

            }
            Vector3 mousePosition = arCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera));
            //check mouse position not on top of UI

            //Identify first click OR continous dragging.
            if (points.Count > 0)
            {
                if (frameTimer < 1)
                {
                    UpdateLineRenderer(mousePosition);
                    frameTimer = updateDrawingFrames;
                }
                else
                {
                    UpdateLineRenderer();
                }
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
                    if (frameTimer < 1)
                    {
                        UpdateLineRenderer(touchPosition);
                        frameTimer = updateDrawingFrames;
                    }
                    else
                    {
                        UpdateLineRenderer();
                    }
                    break;
                case TouchPhase.Stationary:
                    UpdateLineRenderer();
                    break;
                case TouchPhase.Ended:
                    break;

            }
        }
    }

    void InitializeLineRenderer(Vector3 pos)
    {

        GameObject lineGO = new GameObject($"Line");
        lineGO.transform.position = Vector3.zero;//pos;

        lineGO.AddComponent<ARAnchor>();
        line = lineGO.AddComponent<CustomLineRenderer>();
        //interactionManager.StoreInteraction(lineGO);
        lineStack.Push(line);

        brushSetting.LineSetting(line);

        line.InitializeRenderer(pos);
        //line.SetPosition(1, pos);
        points.Add(pos);
    }
    void UpdateLineRenderer(Vector3 pos)
    {

        line.SetPosition(pos);
        points.Add(pos);
    }

    void UpdateLineRenderer()
    {
        line.RenderPosition();
    }

    bool IsOverUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }


    public void SetDistanceFromCamera(float dist)
    {
        distanceFromCamera = dist;
    }

    public void Undo()
    {
        if (lineStack.Count > 0)
        {
            
            CustomLineRenderer line = lineStack.Pop();
            line.GetComponent<MeshRenderer>().enabled = false;
            Destroy(line.gameObject);
        }
    }




}
