using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CurrentTime : MonoBehaviour
{
    public TMP_Text timeText;
    void Update()
    {
        timeText.text = System.DateTime.Now.ToString();
    }
}
