using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContainBox : MonoBehaviour
{
    public TMP_Text medicineNameText;
    public TMP_Text dosageText;
    public TMP_Text instructionText;

    public void SetContainBox(Contain contain)
    {
        medicineNameText.text = contain.M_name;
        dosageText.text = contain.Dosage;
        instructionText.text = contain.Instruction;
    }
}
