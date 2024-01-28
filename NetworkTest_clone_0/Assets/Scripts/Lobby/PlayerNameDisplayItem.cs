using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameDisplayItem : MonoBehaviour
{
    public string nameToDisplay = "player";
    [SerializeField] TextMeshProUGUI nameText;
    
    private void Update() {
        if(nameText.text != nameToDisplay)
            nameText.text = nameToDisplay;
    }
}
