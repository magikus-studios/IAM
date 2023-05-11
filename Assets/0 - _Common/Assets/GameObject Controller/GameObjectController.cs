using UnityEngine;

public class GameObjectController : MonoBehaviour
{
    public void SetActive() { gameObject.SetActive(true); }
    public void SetInactive() { gameObject.SetActive(false); }
    public void ToggleActive() { gameObject.SetActive(!gameObject.activeSelf); }
}
