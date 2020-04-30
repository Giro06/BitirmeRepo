using System;
using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Medicine
{
    public string medicineID;
    public string medicineDosage;
    public string medicineInstruction;
    
}
public class ReceiptListController : MonoBehaviour
{
    public List<MedicineBox> medicineBoxList= new List<MedicineBox>();
    public List<String>  medicineIDs= new List<string>();
    public List<String> medicineNames= new List<string>();
    
    [Header("Medicine Section")]
    public GameObject targetList;
    public GameObject medicineBox;
    public GameObject addButton;

    void Awake()
    {
        StartCoroutine(GetMedicineIDFromDb());
        StartCoroutine(GetMedicineNameFromDB());
    }

    void CreateRecipe()
    {
        
    }
    public void AddMedicineBox()
    {
        GameObject go = Instantiate(medicineBox, transform.position, Quaternion.identity);
        go.transform.SetParent(targetList.transform);
        go.GetComponent<RectTransform>().localScale=new Vector3(1,1,1);
        go.GetComponent<MedicineBox>()._receiptListController = this;
        medicineBoxList.Add(go.GetComponent<MedicineBox>());
        Debug.Log(medicineIDs.Count);
        for (int i = 0; i < medicineIDs.Count; i++)
        {    
            go.GetComponent<MedicineBox>().medicineNameInput.options.Add(new TMP_Dropdown.OptionData(medicineNames[i]));
        }
        addButton.transform.SetAsLastSibling();
    }

    public void DeleteMedicineBox(MedicineBox medicineBox)
    {
        medicineBoxList.Remove(medicineBox);
    }

    public List<Medicine> GetAllMedicines()
    {   
        List<Medicine> temp = new List<Medicine>();
        foreach (var medicineBox in medicineBoxList)
        {
              temp.Add(medicineBox.GetMedicine());   
        }

        return temp;
    }
    
    public IEnumerator GetMedicineNameFromDB()
    {
        WWWForm form= new WWWForm(); 
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/GetMedicineNames.php",form);
        yield return ww.SendWebRequest();
        char[] spearator = {'/'};
        string[] name = ww.downloadHandler.text.Split(spearator);

        foreach (string x in name)
        {  
            Debug.Log(x);
            medicineNames.Add(x);
        }
    }
    IEnumerator GetMedicineIDFromDb()
    {
        WWWForm form= new WWWForm(); 
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/GetMedicineIDs.php",form);
        yield return ww.SendWebRequest();
        char[] spearator = {'/'};
        string[] name = ww.downloadHandler.text.Split(spearator);

        foreach (string x in name)
        {  
            Debug.Log(x);
            medicineIDs.Add(x);
        }
    }
}
