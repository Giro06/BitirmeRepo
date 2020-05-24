using System;
using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
 

public class AdminPanelScript : MonoBehaviour
{
    // User section inputs
    // Ekrandaki ui objelerinin pointerları
    [Header("User Section")]
    public TMP_InputField u_tcNoInput;
    public TMP_InputField u_firstnameInput;
    public TMP_InputField u_surnameInput;
    public TMP_InputField u_passwordInput;
    public TMP_InputField u_bdate;
    public Toggle u_insurance;

    //Doctor section inputs
    [Header("Doctor Section")]
    public TMP_InputField d_idInput;
    public TMP_InputField d_nameInput;
    public TMP_InputField d_passwordInput;

    //Medicine section inputs
    [Header("Medicine Section")]
    public TMP_InputField m_idInput;
    public TMP_InputField m_nameInput;
    public TMP_InputField m_priceInput;
    public CustomDropdown m_typeInput;
    
    //popUp
    
    [Header("PopUp")]
    public GameObject popUp;
    public TMP_Text popUpText;


    //Button functions
    public void CreateUserButton()
    {
        StartCoroutine(ICreateUser());
    }

    public void DeleteUserButton()
    {
        StartCoroutine(IDeleteUser());
    }
    
    public void ViewUserButton()
    {
        StartCoroutine(IViewUser());
    }

    public void UpdateUserButton()
    {
        StartCoroutine(IUpdateUser());
    }

    public void CreateDoctorButton()
    {
        StartCoroutine(ICreateDoctor());
    }

    public void DeleteDoctorButton()
    {
        StartCoroutine(IDeleteDoctor());
    }

    public void ViewDoctorButton()
    {
        StartCoroutine(IViewDoctor());
    }

    public void UpdateDoctorButton()
    {
        StartCoroutine(IUpdateDoctor());
    }
    public void CreateMedicine()
    {
        StartCoroutine(ICreateMedicine());
    }

    public void DeleteMedicine()
    {
        StartCoroutine(IDeleteMedicine());
    }

    public void UpdateMedicineButton()
    {
        StartCoroutine(IUpdateMedicine());
    }

    public void ViewMedicineButton()
    {
        StartCoroutine(IViewMedicine());
        
    }
    // PHP Activation
    IEnumerator ICreateUser()
    {
        WWWForm form= new WWWForm();
        // Tc no kontrol
        if (u_tcNoInput.text.Length == 0)
        {
            PopUp("Tc cannot be empty !");
            yield break;
        }
        else if (u_tcNoInput.text.Length != 11)
        {
            PopUp("Tc can't be less than 11 digit !");
            yield break;
        }
        // Tc no  add to form for php
        form.AddField("Tc_No",u_tcNoInput.text);
        // Firstname kontrol
        if (u_firstnameInput.text.Length == 0)
        {
            PopUp("First name cannot be empty !");
            yield break;
        }
        // Name add to form for php
        form.AddField("Name",u_firstnameInput.text);
        //Surname  check
        if (u_surnameInput.text.Length == 0)
        {
            PopUp("Surname cannot be empty !");
            yield break;
        }
        // Surname add to form
        form.AddField("Sname",u_surnameInput.text);
        //ınsurance add to form
        form.AddField("Insurance",u_insurance.isOn.ToString());
        //Check password
        if (u_passwordInput.text.Length == 0)
        {
            PopUp("Password cannot be empty !");
            yield break;
        }
        //Pass add to form
        form.AddField("U_Password",u_passwordInput.text);
       
        //Noktaya gördüğü yerde böl
        char[] spearator = {'.'};
        string[] bdate = u_bdate.text.Split(spearator);
        // Bday check
        if (u_bdate.text.Length == 0)
        {
            PopUp("Birthdate cannot be empty !");
            yield break;
        }
        if (bdate.Length != 3)
        {
            PopUp("Birthdate wrong format !");
            yield break;
        }
        // Bday add to form
        form.AddField("BdateDay",bdate[0]);
        form.AddField("BdateMonth",bdate[1]);
        form.AddField("BdateYear",bdate[2]);
        //Create webrequest with form
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/CreateUser.php",form);
        //Php ye formu yollar  ve dönüş bekler. Dönüş gelene kadar routini bekletir.
        yield return ww.SendWebRequest();
        //Phpden dönen stringdi ekranda bastırır.
        PopUp(ww.downloadHandler.text);
    }

    IEnumerator IDeleteUser()
    {
        WWWForm form= new WWWForm();
        form.AddField("Tc_No",u_tcNoInput.text);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/DeleteUser.php",form);
        yield return ww.SendWebRequest();
        PopUp(ww.downloadHandler.text);
    }

    IEnumerator IViewUser()
    {
        if (u_tcNoInput.text.Length != 11)
        {
            PopUp("Tc can't be less than 11 digit !");
            yield break;
        }
        WWWForm form= new WWWForm();
        form.AddField("Tc_No",u_tcNoInput.text);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/CheckUserFromTc.php",form);
        yield return ww.SendWebRequest();
        
        if (ww.downloadHandler.text == "False")
        {
             PopUp("User not found!");
             yield break;
        }
        
        ww= UnityWebRequest.Post("http://localhost/GetUserNameFromTc.php",form);
        yield return ww.SendWebRequest();
        u_firstnameInput.text = ww.downloadHandler.text;
         
        ww= UnityWebRequest.Post("http://localhost/GetUserSnameFromTc.php",form);
        yield return ww.SendWebRequest();
        u_surnameInput.text = ww.downloadHandler.text;
        ww= UnityWebRequest.Post("http://localhost/GetUserPasswordFromTc.php",form);
        yield return ww.SendWebRequest();
        u_passwordInput.text = ww.downloadHandler.text;
        ww= UnityWebRequest.Post("http://localhost/GetUserBdateFromTc.php",form);
        yield return ww.SendWebRequest();
        u_bdate.text = ww.downloadHandler.text;
        
    }

    IEnumerator IUpdateUser()
    {
        WWWForm form= new WWWForm();
        if (u_tcNoInput.text.Length == 0)
        {
            PopUp("Tc cannot be empty !");
            yield break;
        }
        else if (u_tcNoInput.text.Length != 11)
        {
            PopUp("Tc can't be less than 11 digit !");
            yield break;
        }
        form.AddField("Tc_No",u_tcNoInput.text);
        if (u_firstnameInput.text.Length == 0)
        {
            PopUp("First name cannot be empty !");
            yield break;
        }
        form.AddField("Name",u_firstnameInput.text);
        if (u_surnameInput.text.Length == 0)
        {
            PopUp("Surname cannot be empty !");
            yield break;
        }
        form.AddField("Sname",u_surnameInput.text); 
        form.AddField("Insurance",u_insurance.isOn.ToString());
        if (u_passwordInput.text.Length == 0)
        {
            PopUp("Password cannot be empty !");
            yield break;
        }
        form.AddField("U_Password",u_passwordInput.text);
        char[] spearator = {'.'};
        string[] bdate = u_bdate.text.Split(spearator);
        if (u_bdate.text.Length == 0)
        {
            PopUp("Birthdate cannot be empty !");
            yield break;
        }
        if (bdate.Length != 3)
        {
            PopUp("Birthdate wrong format !");
            yield break;
        }
        
        form.AddField("BdateDay",bdate[0]);
        form.AddField("BdateMonth",bdate[1]);
        form.AddField("BdateYear",bdate[2]);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/UpdateUserWithTc.php",form);
        yield return ww.SendWebRequest();
        PopUp(ww.downloadHandler.text);
    }
    IEnumerator ICreateDoctor()
    {
        WWWForm form= new WWWForm();
        if (d_idInput.text.Length == 0)
        {
            PopUp("Doctor id cant be empty !");
            yield break;
        }
        if (d_idInput.text.Length < 9)
        {
            PopUp("Doctor id cant be less than 9 digit !");
            yield break;
        }
        form.AddField("D_ID",d_idInput.text);
        if (d_nameInput.text.Length == 0)
        {
            PopUp("Doctor name cant be empty !");
            yield break;
        }
        form.AddField("D_Name",d_nameInput.text);
        if (d_passwordInput.text.Length == 0)
        {
            PopUp("Password cant be empty !");
            yield break;
        }
        form.AddField("D_Pass",d_passwordInput.text);
    
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/CreateDoctor.php",form);
        yield return ww.SendWebRequest();
        PopUp(ww.downloadHandler.text);
    }
    IEnumerator IDeleteDoctor()
    {
        WWWForm form= new WWWForm();
        form.AddField("D_ID",d_idInput.text);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/DeleteDoctor.php",form);
        yield return ww.SendWebRequest();
        PopUp(ww.downloadHandler.text);
    }

    IEnumerator IViewDoctor()
    {
        if (d_idInput.text.Length != 9)
        {
            PopUp("Doctor Id can't be less than 9 digit !");
            yield break;
        }
        WWWForm form= new WWWForm();
        form.AddField("D_ID",d_idInput.text);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/CheckDoctorFromID.php",form);
        yield return ww.SendWebRequest();
        if (ww.downloadHandler.text == "False")
        {
            PopUp("Doctor not found!");
            yield break;
        }
        ww= UnityWebRequest.Post("http://localhost/GetDoctorNameFromID.php",form);
        yield return ww.SendWebRequest();
        d_nameInput.text = ww.downloadHandler.text;
        ww= UnityWebRequest.Post("http://localhost/GetDoctorPasswordFromID.php",form);
        yield return ww.SendWebRequest();
        d_passwordInput.text = ww.downloadHandler.text;
    }

    IEnumerator IUpdateDoctor()
    {
        WWWForm form= new WWWForm();
        form.AddField("D_ID",d_idInput.text);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/CheckDoctorFromID.php",form);
        yield return ww.SendWebRequest();
        if (ww.downloadHandler.text == "False")
        {
            PopUp("Doctor not found!");
            yield break;
        }
         form= new WWWForm();
        if (d_idInput.text.Length == 0)
        {
            PopUp("Doctor id cant be empty !");
            yield break;
        }
        if (d_idInput.text.Length < 9)
        {
            PopUp("Doctor id cant be less than 9 digit !");
            yield break;
        }
        form.AddField("D_ID",d_idInput.text);
        if (d_nameInput.text.Length == 0)
        {
            PopUp("Doctor name cant be empty !");
            yield break;
        }
        form.AddField("D_Name",d_nameInput.text);
        if (d_passwordInput.text.Length == 0)
        {
            PopUp("Password cant be empty !");
            yield break;
        }
        form.AddField("D_Pass",d_passwordInput.text);
    
        ww= UnityWebRequest.Post("http://localhost/UpdateDoctorWithID.php",form);
        yield return ww.SendWebRequest();
        PopUp(ww.downloadHandler.text);
    }
    IEnumerator ICreateMedicine()
    {
        WWWForm form= new WWWForm();
        if (m_idInput.text.Length == 0)
        {
            PopUp("Medicine id cant be empty !");
            yield break;
        }
        if (m_idInput.text.Length < 5)
        {
            PopUp("Medicine id cant be less than 5 digit !");
            yield break;
        }
        form.AddField("M_ID",m_idInput.text);
        if (m_nameInput.text.Length == 0)
        {
            PopUp("Medicine name cant be empty !");
            yield break;
        }
        form.AddField("M_Name",m_nameInput.text);
        if (m_priceInput.text.Length == 0)
        {
            PopUp("Medicine price cant be empty !");
            yield break;
        }
        form.AddField("Price", m_priceInput.text);
        form.AddField("Type",m_typeInput.dropdownItems[m_typeInput.selectedItemIndex].itemName);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/CreateMedicine.php",form);
        yield return ww.SendWebRequest();
        PopUp(ww.downloadHandler.text);
    }
    IEnumerator IDeleteMedicine()
    {
        WWWForm form= new WWWForm();
        form.AddField("M_ID",m_idInput.text);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/DeleteMedicine.php",form);
        yield return ww.SendWebRequest();
        PopUp(ww.downloadHandler.text);
    }

    IEnumerator IViewMedicine()
    {
        if (m_idInput.text.Length != 5)
        {
            PopUp("Medicine Id can't be less than 5 digit !");
            yield break;
        }
        WWWForm form= new WWWForm();
        form.AddField("M_ID",m_idInput.text);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/CheckMedicineFromID.php",form);
        yield return ww.SendWebRequest();
        if (ww.downloadHandler.text == "False")
        {
            PopUp("Medicine not found!");
            yield break;
        }
        ww= UnityWebRequest.Post("http://localhost/GetMedicineNameFromID.php",form);
        yield return ww.SendWebRequest();
        m_nameInput.text = ww.downloadHandler.text;
        ww= UnityWebRequest.Post("http://localhost/GetMedicinePriceFromID.php",form);
        yield return ww.SendWebRequest();
        m_priceInput.text = ww.downloadHandler.text;
       
    }

    IEnumerator IUpdateMedicine()
    {
        WWWForm form= new WWWForm();
        if (m_idInput.text.Length == 0)
        {
            PopUp("Medicine id cant be empty !");
            yield break;
        }
        if (m_idInput.text.Length < 5)
        {
            PopUp("Medicine id cant be less than 5 digit !");
            yield break;
        }
        form.AddField("M_ID",m_idInput.text);
        if (m_nameInput.text.Length == 0)
        {
            PopUp("Medicine name cant be empty !");
            yield break;
        }
        form.AddField("M_Name",m_nameInput.text);
        if (m_priceInput.text.Length == 0)
        {
            PopUp("Medicine price cant be empty !");
            yield break;
        }
        form.AddField("Price", m_priceInput.text);
        form.AddField("Type",m_typeInput.dropdownItems[m_typeInput.selectedItemIndex].itemName);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/UpdateMedicine.php",form);
        yield return ww.SendWebRequest();
        PopUp(ww.downloadHandler.text);
    }
    //PopUp functions
    void PopUp(string message)
    {
        popUpText.text = message;
        popUp.SetActive(false);
        popUp.SetActive(true);
    }
}
