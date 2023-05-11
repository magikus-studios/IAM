using UnityEngine;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
    public KeyCode primaryKey;
    public KeyCode secondaryKey;
    public UnityEvent OnKeyDown;
    public UnityEvent OnKeyUp;
    public UnityEvent OnKey;

    void Update() { CheckKey(); }

    public void CheckKey()
    {
        if (Input.GetKeyDown(primaryKey) || Input.GetKeyDown(secondaryKey)) { OnKeyDown?.Invoke(); }
        else if (Input.GetKey(primaryKey) || Input.GetKey(secondaryKey)) { OnKey?.Invoke(); }
        else if (Input.GetKeyUp(primaryKey) || Input.GetKeyUp(secondaryKey)) { OnKeyUp?.Invoke(); }
    }
}
