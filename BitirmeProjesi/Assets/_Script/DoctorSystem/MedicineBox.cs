using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using UnityEngine;
using  TMPro;
using UnityEngine.UI;

public class MedicineBox : MonoBehaviour
{
     public ReceiptListController _receiptListController;
     public TMP_Dropdown medicineNameInput;
     public HorizontalSelector dosageSelectorInput;
     public HorizontalSelector instructionSelectorInput;
     
     public TMP_Text dosageText;
     public TMP_Text instructionText;


     public void DeleteMedicineBox()
     {    
          _receiptListController.DeleteMedicineBox(this);
          Destroy(this.gameObject);
     }

     public Medicine GetMedicine()
     {
          Medicine temp = new Medicine();
          temp.medicineDosage = dosageText.text;
          temp.medicineInstruction = instructionText.text;
          temp.medicineID =  _receiptListController.medicineIDs[medicineNameInput.value];
          return temp;
     }
}
