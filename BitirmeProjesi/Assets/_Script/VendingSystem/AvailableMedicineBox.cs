using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AvailableMedicineBox : MonoBehaviour
{
    public TMP_Text medicineNameText;
    public TMP_Text medicineCountText;

    public void SetBox(string medicineName,string medicineCount)
    {
        medicineNameText.text = medicineName;
        medicineCountText.text = medicineCount;
        
    }
}
