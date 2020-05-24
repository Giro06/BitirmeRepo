using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AvailableMedicineBox : MonoBehaviour
{    
    //UI da gözükücek medicine prefabının textlerini tutuyoruz
    public TMP_Text medicineNameText;
    public TMP_Text medicineCountText;
    
    // Prefabın textlerini updateliyoruz.
    public void SetBox(string medicineName,string medicineCount)
    {
        medicineNameText.text = medicineName;
        medicineCountText.text = medicineCount;
        
    }
}
