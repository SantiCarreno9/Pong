using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlOptionsManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown[] dropdowns = default;
    private Dictionary<Control, bool> _optionsTaken = new Dictionary<Control, bool>();

    private void Start()
    {
        for (int i = 0; i < 5; i++)
            _optionsTaken.Add((Control)i, false);

        _optionsTaken[Control.WSKeys] = true;
        _optionsTaken[Control.Arrows] = true;
    }


    public void UpdateDropdown(TMP_Dropdown dropdown)
    {
        if (!dropdown.IsExpanded)
            return;

        Toggle[] toggles = dropdown.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toggles.Length; i++)
        {
            if (i != (int)Control.AI)
            {
                if (_optionsTaken[(Control)i])
                    toggles[i].interactable = false;
            }
        }
    }

    public void TakeOption(int index)
    {
        _optionsTaken[(Control)index] = true;
    }
}
