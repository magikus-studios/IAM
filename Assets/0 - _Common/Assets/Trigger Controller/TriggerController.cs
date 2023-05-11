using UnityEngine;
using UnityEngine.Events;

public class TriggerController : MonoBehaviour
{
    [SerializeField] private string _name;

    [SerializeField] private UnityEvent OnTriggerEnter;
    [SerializeField] private UnityEvent OnTriggerStay;
    [SerializeField] private UnityEvent OnTriggerExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (OnTriggerEnter == null) { return; }
        if (collision.gameObject.name == _name) { OnTriggerEnter?.Invoke(); }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (OnTriggerStay == null) { return; }
        if (collision.gameObject.name == _name) { OnTriggerStay?.Invoke(); }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (OnTriggerExit == null) { return; }
        if (collision.gameObject.name == _name) { OnTriggerExit?.Invoke(); }
    }
}
