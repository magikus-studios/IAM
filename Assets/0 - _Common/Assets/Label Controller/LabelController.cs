using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace IAM
{
    [RequireComponent(typeof(TMP_Text))]
    public class LabelController : MonoBehaviour
    {
        [SerializeField] private string _genericText = "#";
        [SerializeField] private string _replaceChar = "#";
        [SerializeField] private float _numberMultiplier = 1f;
        [SerializeField] private string _numberFormat = "F0";

        private TMP_Text _label;

        private void Awake()
        {
            _label = GetComponent<TMP_Text>();
        }
        public void SetLabel(int amount) 
        {
            amount *= (int)_numberMultiplier;
            _label.text = _genericText.Replace(_replaceChar, amount.ToString(_numberFormat));
        }
        public void SetLabel(float amount)
        {
            amount *= _numberMultiplier;
            _label.text = _genericText.Replace(_replaceChar, amount.ToString(_numberFormat));
        }
    }
}
