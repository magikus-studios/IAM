using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListController : MonoBehaviour
{
    [SerializeField] private List<InputControllerObject> _inputList;

    void Update() { foreach (InputControllerObject input in _inputList) { input.CheckKey(); } }
}
