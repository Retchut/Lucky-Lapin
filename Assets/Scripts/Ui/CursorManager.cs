using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;

    public bool hideOnFocus = true;

    [SerializeField]
    public Texture2D defaultCursor;
    [SerializeField]
    public Texture2D clickCursor;
    public Vector2 hotspot;

    bool showing;
    bool clickMode;

    private void Awake()
    {
        instance = this;
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.ForceSoftware);
    }

    private void Start()
    {
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.ForceSoftware);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && hideOnFocus)
        {
            HideCursor();
        }
    }

    void Update()
    {
        if (!showing) return;

        if (Input.anyKeyDown)
        {
            clickMode = true;
            Cursor.SetCursor(clickCursor, hotspot, CursorMode.ForceSoftware);
        }
        else if (!Input.anyKey && clickMode)
        {
            clickMode = false;
            Cursor.SetCursor(defaultCursor, hotspot, CursorMode.ForceSoftware);
        }

    }
    [ContextMenu("Show Cursor")]
    public void ShowCursor()
    {
        showing = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    [ContextMenu("Hide Cursor")]
    public void HideCursor()
    {
        showing = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }




}
