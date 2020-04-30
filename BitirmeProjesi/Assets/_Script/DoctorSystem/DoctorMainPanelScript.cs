using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DoctorMainPanelScript : MonoBehaviour
{   
    [Header("PopUp")]
    public GameObject popUp;
    public TMP_Text popUpText;
    
    [Header("Doctor Information")] 
    public DoctorInformation DoctorInformation;

    [Header("Doctor Panel Section")] 
    public TMP_Text doctorIDText;
    public TMP_Text doctorNameText;

    [Header("Searc Patient Section")]
    public TMP_InputField userTcNoInput;
    
    [Header("User Info Section")]
    public TMP_Text u_tcNOtext;
    public TMP_Text u_firstNameText;
    public TMP_Text u_surnameText;
    public TMP_Text u_birthdateText;
    public TMP_Text u_ınsuranceText;
    
    [Header("Receipt Section")]
    public GameObject  lockObject;
    public TMP_InputField protocolInput;
    public TMP_InputField diagnosisInput;
    public ReceiptListController _ReceiptListController;
    
    private string lastRecipeNO; 
    private int recipeNO;
    private void Start()
    {
        GetDoctorInfoToDoctorPanel();
        lockObject.SetActive(true);
    }
    
    //Search Section
    public void SearchUserButton()
    {
        if (userTcNoInput.text.Length == 0)
        {
            PopUp("Patient TC can't be empty !");
            return;
        }

        if (userTcNoInput.text.Length < 11)
        {
            PopUp("Patient TC can't be less than 11 digit !");
            return;
        }
        StartCoroutine(ISearchUserByTc(userTcNoInput.text));
    }

  
    IEnumerator ISearchUserByTc(string Tc_No)
    {   
        //Check User exits
        WWWForm form= new WWWForm();
        form.AddField("Tc_No",Tc_No);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/CheckUserFromTc.php",form);
        yield return ww.SendWebRequest();
        if (ww.downloadHandler.text == "False")
        {   
            PopUp("Patient not found !");
            lockObject.SetActive(true);
            yield break;
        }
        lockObject.SetActive(false);
        //NAME
        u_tcNOtext.text = Tc_No;
        form= new WWWForm();
        form.AddField("Tc_No",Tc_No);
        ww= UnityWebRequest.Post("http://localhost/GetUserNameFromTc.php",form);
        yield return ww.SendWebRequest();
        u_firstNameText.text= ww.downloadHandler.text;
        //Sname
        form= new WWWForm();
        form.AddField("Tc_No",Tc_No);
        ww= UnityWebRequest.Post("http://localhost/GetUserSnameFromTc.php",form);
        yield return ww.SendWebRequest();
        u_surnameText.text= ww.downloadHandler.text;
        //Bdate
        form= new WWWForm();
        form.AddField("Tc_No",Tc_No);
        ww= UnityWebRequest.Post("http://localhost/GetUserBdateFromTc.php",form);
        yield return ww.SendWebRequest();
        u_birthdateText.text= ww.downloadHandler.text;
        //Insurance
        form= new WWWForm();
        form.AddField("Tc_No",Tc_No);
        ww= UnityWebRequest.Post("http://localhost/GetUserInsuranceFromTc.php",form);
        yield return ww.SendWebRequest();
        if (ww.downloadHandler.text == "1")
        {
            u_ınsuranceText.text = "True";
        }
        else
        {
            u_ınsuranceText.text = "False";
        }
     
    }
    //DOCTOR SECTION
    //Functions
    void GetDoctorInfoToDoctorPanel()
    {
        doctorIDText.text = DoctorInformation.doctorID;
        StartCoroutine(IGetDoctorNameFromServer(DoctorInformation.doctorID));
    }
    
    //PHP CO
    IEnumerator IGetDoctorNameFromServer(string D_ID)
    {
        WWWForm form= new WWWForm();
        form.AddField("D_ID",D_ID);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/GetDoctorNameFromID.php",form);
        yield return ww.SendWebRequest();
        doctorNameText.text = ww.downloadHandler.text;
    }
    // Recipe Section
    public void CreateRecipe()
    {
        if (u_tcNOtext.text.Length == 0)
        {
            PopUp("Tc Error !");
            return;
        }

        if (protocolInput.text.Length == 0)
        {
            PopUp("ProtocolNo Error !");
            return;
        }
        if (diagnosisInput.text.Length == 0)
        {
            PopUp("Diagnosis Error !");
            return;
        }
        if (_ReceiptListController.GetAllMedicines().Count == 0)
        {
            PopUp("Medicine Error !");
            return;
        }
        StartCoroutine(ICreateRecipeDB(u_tcNOtext.text,protocolInput.text,diagnosisInput.text,_ReceiptListController.GetAllMedicines()));
    }

    IEnumerator ICreateRecipeDB(string U_TcNum,string Protocol_No,string Diagnosis,List<Medicine> medicines)
    {  
        yield return StartCoroutine(IGetLastRecipeNO());
        WWWForm form = new WWWForm();
        recipeNO = Int32.Parse(lastRecipeNO) + 1;
        form.AddField("U_TcNum",U_TcNum);
        form.AddField("Protocol_No",Protocol_No);
        form.AddField("Diagnosis",Diagnosis);
        form.AddField("RecipeNo",recipeNO.ToString());
        UnityWebRequest ww = UnityWebRequest.Post("http://localhost/CreateRecipe.php", form);
        yield return ww.SendWebRequest();
        for (int i = 0; i < medicines.Count; i++)
        {
            form = new WWWForm();
            recipeNO = Int32.Parse(lastRecipeNO) + 1;
            form.AddField("RecipeNo",recipeNO.ToString());
            form.AddField("M_ID",medicines[i].medicineID);
            form.AddField("Dosage",medicines[i].medicineDosage);
            form.AddField("Instruction",medicines[i].medicineInstruction); 
            ww = UnityWebRequest.Post("http://localhost/CreateContain.php", form);
            yield return ww.SendWebRequest();
        }
        PopUp("Recipe create succesfully");
        
      
     }
    IEnumerator IGetLastRecipeNO( )
    {
        WWWForm form = new WWWForm(); 
        UnityWebRequest ww = UnityWebRequest.Post("http://localhost/GetLastRecipeCount.php", form);
        yield return ww.SendWebRequest();
        lastRecipeNO = ww.downloadHandler.text;
        Debug.Log(lastRecipeNO);
       
    }

     
    void PopUp(string message)
    {
        popUpText.text = message;
        popUp.SetActive(false);
        popUp.SetActive(true);
    }
}
