using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MedicineCounter
{
    public string medicineName;
    public string medicineID;
    public int medicineCount;
}
public class AvailableMedicinePanel : MonoBehaviour
{   
    SerialPort sp = new SerialPort("COM4",9600);
    
    
    public GameObject availableMedicinePrefab;
    public GameObject targetList;
    List<MedicineCounter> medicineCounters= new List<MedicineCounter>();
    
    [Header("PopUp")]
    public GameObject popUp;
    public TMP_Text popUpText;
    
    List<string> availableIdInSystemToGive = new List<string>();
    private bool flag = false;
    private void Start()
    {    
        StartCoroutine(GetMedicineNameAndCount(RecipeInformation.RecipeNo));
         sp.Open();
         sp.ReadTimeout = 1;
    }

    IEnumerator GetMedicineNameAndCount(string RecipeNo)
    {    
        WWWForm form= new WWWForm();
        form.AddField("RecipeNo", RecipeNo);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/GetRecipeForVendingFromRecipeNo.php",form);
        yield return ww.SendWebRequest();

        int notFoundCounter=0;
        Debug.Log(ww.downloadHandler.text);
        char[] spearator = {'/'};
        string[] medicenes = ww.downloadHandler.text.Split(spearator);
        Debug.Log("Medicine Count:"+ medicenes.Length);
        foreach (string medicine in medicenes)
        {     
            
            //Firstly check list to do we this medicine in list
            if (medicineCounters.Count != 0)
            {
                bool check = true;
                foreach (var medicineCounter in medicineCounters)
                {
                    if (medicineCounter.medicineID == medicine)
                    {
                        medicineCounter.medicineCount++;
                        check = false;
                    }
                }

                if (check)
                {
                        if (medicine != "")
                        {   
                            MedicineCounter temp= new MedicineCounter();
                            WWWForm medicineForm = new WWWForm();
                            medicineForm.AddField("M_ID",medicine);
                            UnityWebRequest mww= UnityWebRequest.Post("http://localhost/GetMedicineNameFromID.php",medicineForm);
                            yield return mww.SendWebRequest();
                            temp.medicineName=(mww.downloadHandler.text);
                            temp.medicineID = medicine;
                            temp.medicineCount = 1;
                            medicineCounters.Add(temp);
                        }
                    
                }
            }
            else
            {
                if (medicine!="")
                {   
                    MedicineCounter temp= new MedicineCounter();
                    WWWForm medicineForm = new WWWForm();
                    medicineForm.AddField("M_ID",medicine);
                    UnityWebRequest mww= UnityWebRequest.Post("http://localhost/GetMedicineNameFromID.php",medicineForm);
                    yield return mww.SendWebRequest();
                    temp.medicineName=(mww.downloadHandler.text);
                    temp.medicineID = medicine;
                    temp.medicineCount = 1;
                    medicineCounters.Add(temp);
                }
            } 
        }
        Debug.Log("After split:"+medicineCounters.Count);
        //Check we have in automat
       
        foreach (var medicineCounter in medicineCounters)
        {    
            Debug.Log("Medicine ID:"+medicineCounter.medicineID);
            int countforMedicine = CheckStock(medicineCounter.medicineID, medicineCounter.medicineCount);
            if (countforMedicine < medicineCounter.medicineCount)
            {
                notFoundCounter++;
            }
            if (countforMedicine !=0)
            {  
                GameObject temp = Instantiate(availableMedicinePrefab, transform.position, Quaternion.identity);
                temp.GetComponent<AvailableMedicineBox>().SetBox(medicineCounter.medicineName,countforMedicine+"x");
                temp.transform.SetParent(targetList.transform);
                temp.GetComponent<RectTransform>().localScale=Vector3.one;
                for (int i = 0; i < countforMedicine; i++)
                {
                    availableIdInSystemToGive.Add(medicineCounter.medicineID);
                    //Use this for delete from db.
                    Debug.Log(medicineCounter.medicineID+" added to list");
                }
              
            }
            else
            {
                notFoundCounter++;
            }
        }

        if (notFoundCounter == medicineCounters.Count)
        {
              Debug.Log("Not medicine available for your recipe!");
              PopUp("Not medicine available for your recipe!");
        }
        else if (notFoundCounter >0)
        {
            Debug.Log("Some of your medicine not available!");
            PopUp("Some of your medicine not available!");
        }
        
    }

    int CheckStock(string ID,int counter)
    {
        foreach (var medicine in Stock.stock)
        {
            if (medicine.medicineID == ID)
            {
                if (medicine.count >= counter)
                {
                    return counter;
                }
                else
                {
                    return medicine.count;
                }
            }
        }

        return 0;
    }

    IEnumerator Approve()
    {
        //reduce from stock and db
        Debug.Log(availableIdInSystemToGive.Count);
        foreach (var ID in availableIdInSystemToGive)
        {
            foreach (var medicineStock in Stock.stock)
            {
                if (ID == medicineStock.medicineID)
                {
                    medicineStock.count--;  
                    Debug.Log(ID+" reduced.");
                    //Delete from db
                    WWWForm deleteForm = new WWWForm();
                    deleteForm.AddField("M_ID",ID);
                    deleteForm.AddField("RecipeNo",RecipeInformation.RecipeNo);
                    UnityWebRequest mww= UnityWebRequest.Post("http://localhost/DeleteContainFromDB.php",deleteForm); 
                    mww.SendWebRequest();
                }
            }
        }
        Debug.Log("Approve Check");
        //Check there is left any medicine on receipt
        yield return new WaitForSeconds(1);
        StartCoroutine(CheckContainCount());
        
    }

    IEnumerator DeleteRecipe()
    {
        //delete recipe
        WWWForm recipeDelete = new WWWForm();
        recipeDelete.AddField("RecipeNo",RecipeInformation.RecipeNo);
        UnityWebRequest rww= UnityWebRequest.Post("http://localhost/DeleteRecipeFromDB.php",recipeDelete);
        yield return rww.SendWebRequest();
        Debug.Log("Delete Check");
        SceneManager.LoadScene(4);
    }
    IEnumerator CheckContainCount()
    {
        WWWForm checkCount = new WWWForm();
        checkCount.AddField("RecipeNo",RecipeInformation.RecipeNo);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/GetContainCountOnMedicine.php",checkCount); 
        ww.SendWebRequest();
        yield return new WaitForSeconds(1);
        Debug.Log("Contain Check");
        if (ww.downloadHandler.text == 0.ToString() )
        {
            StartCoroutine(DeleteRecipe());
        }
        else
        {
            SceneManager.LoadScene(4);
        }
        yield return null;
    }
    void Reject()
    {
        SceneManager.LoadScene(0);
    }
    int InputFromSensors()
    {   
        if (Input.GetKeyDown(KeyCode.A))
        {
            return 2;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            return 1;
        }
        if (sp.IsOpen)
        {
            try
            {
                if (sp.BytesToRead > 0)
                {
                   return sp.ReadByte();
                }
             }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

     
        return 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        int x = InputFromSensors();
        if (x == 1 && !flag)
        {
            StartCoroutine(Approve());
            flag = true;
          
        }
        else if (x == 2 && !flag)
        {
            SceneManager.LoadScene(1);
        }
    }
    void PopUp(string message)
    {
        popUpText.text = message;
        popUp.SetActive(false);
        popUp.SetActive(true);
    }
}
