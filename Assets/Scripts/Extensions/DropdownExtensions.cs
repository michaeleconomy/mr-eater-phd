using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class DropdownExtensions {

    public static string StringValue(this Dropdown dropdown) {
        return dropdown.options[dropdown.value].text;
    }

}
