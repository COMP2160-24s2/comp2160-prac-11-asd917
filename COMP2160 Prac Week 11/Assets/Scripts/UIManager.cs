/**
 * A singleton class to allow point-and-click movement of the marble.
 * 
 * It publishes a TargetSelected event which is invoked whenever a new target is selected.
 * 
 * Author: Malcolm Ryan
 * Version: 1.0
 * For Unity Version: 2022.3
 */

using UnityEngine;
using UnityEngine.InputSystem;

// note this has to run earlier than other classes which subscribe to the TargetSelected event
[DefaultExecutionOrder(-100)]
public class UIManager : MonoBehaviour
{
#region UI Elements
    [SerializeField] private Transform crosshair;
    [SerializeField] private Transform target;
#endregion 

#region Singleton
    static private UIManager instance;
    static public UIManager Instance
    {
        get { return instance; }
    }
#endregion 

#region Actions
    private Actions actions;
    private InputAction mouseAction;
    private InputAction deltaAction;
    private InputAction selectAction;
#endregion

#region Events
    public delegate void TargetSelectedEventHandler(Vector3 worldPosition);
    public event TargetSelectedEventHandler TargetSelected;
    [SerializeField] private bool followMouseCursor = true;
#endregion

#region Init & Destroy
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one UIManager in the scene.");
        }

        instance = this;

        actions = new Actions();
        mouseAction = actions.mouse.position;
        deltaAction = actions.mouse.delta;
        selectAction = actions.mouse.select;

        Cursor.visible = false;
        target.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        actions.mouse.Enable();
    }

    void OnDisable()
    {
        actions.mouse.Disable();
    }
#endregion Init

#region Update
    void Update()
    {
        MoveCrosshair();
        SelectTarget();
    }

    private void MoveCrosshair()
    {
        if (followMouseCursor)
        {
            Vector2 mousePos = mouseAction.ReadValue<Vector2>();
            
            Debug.Log(mousePos);

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));

            Plane plane = new Plane(Vector3.up, new Vector3(0, 1, 0));

            if (plane.Raycast(ray, out float enter))
            {
                Vector3 worldPosition = ray.GetPoint(enter);

                crosshair.position = worldPosition;
            }
        }
        else
        {
            Vector2 delta = deltaAction.ReadValue<Vector2>();
            
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(crosshair.position);

            screenPosition += new Vector3(delta.x, delta.y, 0);

            Vector3 newWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
            
            screenPosition.x = Mathf.Clamp(screenPosition.x, screenRect.xMin, screenRect.xMax);

            screenPosition.y = Mathf.Clamp(screenPosition.y, screenRect.yMin, screenRect.yMax);

            newWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            crosshair.position = newWorldPosition;
        }
        /*

        Vector3 screenPosition = new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y);
    
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        worldPosition.y = 1;

        crosshair.position = worldPosition;

        // FIXME: Move the crosshair position to the mouse position (in world coordinates)
        // crosshair.position = ...;*/
    }


    private void SelectTarget()
    {
        if (selectAction.WasPerformedThisFrame())
        {
            // set the target position and invoke 
            target.gameObject.SetActive(true);
            target.position = crosshair.position;     
            TargetSelected?.Invoke(target.position);       
        }
    }

#endregion Update

}






