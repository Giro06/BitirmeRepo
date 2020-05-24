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
    
    // Uida ilaçları gösterecek kutuların prefabini tutar
    public GameObject availableMedicinePrefab;
    // İlaçların sıralanacağı listeyi tutar
    public GameObject targetList;
    
    //Databaseden reçeteye bağlı ilaç listesi.
    List<MedicineCounter> medicineCounters= new List<MedicineCounter>();
    
    [Header("PopUp")]
    public GameObject popUp;
    public TMP_Text popUpText;
    
    
    //Sistemde uygun olan ilaçların listesini tutar
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
        //Php den gelen ilaç listesini ayırıyoruz.
        char[] spearator = {'/'};
        string[] medicenes = ww.downloadHandler.text.Split(spearator);
       
       
            
        //Tüm ilaçları birer birer kontrol ediyoruz.
        foreach (string medicine in medicenes)
        {     
            
            //Liste boş mu diye kontrol ediyoruz eğer boş değilse ilk olarak listede arıyoruz  eğer boşsa direk listeye atıyoruz.
            if (medicineCounters.Count != 0)
            {  
                bool check = true;
                foreach (var medicineCounter in medicineCounters)
                {    //listede bulunduysa  counterını 1 arttırıyoruz.
                    if (medicineCounter.medicineID == medicine)
                    {
                        medicineCounter.medicineCount++;
                        check = false;
                    }
                }
                //eğer listede bulunmadıysa listeye ekliyoruz
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
            // liste boşssa direk ekliyoruz
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
      
         
        //burdada databaseden çektiğimiz ilaç listesini otomatdaki listeyle kıyaslıyoruz.
        foreach (var medicineCounter in medicineCounters)
        {    
             //Checkstock fonksiyonu ile ilacın otomatda olup olmadıgını varsada kaç adet bulunduğunu kontrol ediyoruz.
            int countforMedicine = CheckStock(medicineCounter.medicineID, medicineCounter.medicineCount);
            // Eğer otomatdaki ilaç sayısı yetmiyorsa sistemde bulunamayan ilaç sayısını arttırıyoruz ki ekranda kullanıcıya bazı ilaçlarının bulunamadıgını söyleyebilelim
            if (countforMedicine < medicineCounter.medicineCount)
            {
                notFoundCounter++;
            }
            // İlac sistemde varsa bunları uide ekranda göstermek için prefab oluşturuyoruz 
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
        //tüm ilaçlar sistemde yoksa burda ekranda uyarı veriyoruz
        if (notFoundCounter == medicineCounters.Count)
        {
              Debug.Log("Not medicine available for your recipe!");
              PopUp("Not medicine available for your recipe!");
        }
        //Bazı ilaçlar yoksa ekranda burda uyarı veriyoruz.
        else if (notFoundCounter >0)
        {
            Debug.Log("Some of your medicine not available!");
            PopUp("Some of your medicine not available!");
        }
        
    }
    //Girilen variablelara göre sistemde varmı yok mu ya da yeterli sayıda varmı diye kontrol ediyoruz.
    int CheckStock(string ID,int counter)
    {
        foreach (var medicine in Stock.stockList)
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
    // eğer kullanıcı onaylarsa reçetesinde olan ve otomatda bulunan ilaçları databaseden ve otomat sisteminden düşüyoruz.
    IEnumerator Approve()
    {
        //reduce from stock and db
        //reçete ve otomatda olan tüm ilaçlar için bir loop döndürüyoruz
        foreach (var ID in availableIdInSystemToGive)
        {
            foreach (var medicineStock in Stock.stockList)
            {
                if (ID == medicineStock.medicineID)
                {   
                    //ilaçları stockdan düşüyoruz
                    medicineStock.count--;  
                  
                    //Delete from db
                    //ilaçları databaseden siliyoruz.
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
        //son olarak databasedeki reçete koduna ait contain kaldımı diye kontrol ediyoruz.
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
            //Eğer contain sayısı 0 ise reçeteyide siliyoruz.
            StartCoroutine(DeleteRecipe());
        }
        else
        {
            //contain sayısı 0 değilse direk thanks ekranına geçiyoruz.
            SceneManager.LoadScene(4);
        }
        yield return null;
    }
    void Reject()
    {    //Kullanıcı eğer reddederse direkman başlangıc ekranına gidiyoruz.
        SceneManager.LoadScene(0);
    }
    //sensörlerden gelen inputları çekiyoruz.
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
        //Sensörlerden gelen inputlara göre sistemi yönetiyoruz.
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
