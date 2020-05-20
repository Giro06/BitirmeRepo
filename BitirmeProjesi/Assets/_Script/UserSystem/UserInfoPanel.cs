using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
public class UserInfoPanel : MonoBehaviour
{
    public TMP_Text u_tcNOtext;
    public TMP_Text u_firstNameText;
    public TMP_Text u_surnameText;
    public TMP_Text u_birthdateText;
    public TMP_Text u_ınsuranceText;

    public GameObject settingPanel;
    void Start()
    {
        StartCoroutine(GetUserInfoWithTC(UserInformation.userTc));
    }

    IEnumerator GetUserInfoWithTC(string Tc_No)
    {
        //Check User exits
        WWWForm form= new WWWForm();
        form.AddField("Tc_No",Tc_No);
        UnityWebRequest ww= UnityWebRequest.Post("http://192.168.1.34/CheckUserFromTc.php",form);
        yield return ww.SendWebRequest();
        //NAME
        u_tcNOtext.text = Tc_No;
        form= new WWWForm();
        form.AddField("Tc_No",Tc_No);
        ww= UnityWebRequest.Post("http://192.168.1.34/GetUserNameFromTc.php",form);
        yield return ww.SendWebRequest();
        u_firstNameText.text= ww.downloadHandler.text;
        //Sname
        form= new WWWForm();
        form.AddField("Tc_No",Tc_No);
        ww= UnityWebRequest.Post("http://192.168.1.34/GetUserSnameFromTc.php",form);
        yield return ww.SendWebRequest();
        u_surnameText.text= ww.downloadHandler.text;
        //Bdate
        form= new WWWForm();
        form.AddField("Tc_No",Tc_No);
        ww= UnityWebRequest.Post("http://192.168.1.34/GetUserBdateFromTc.php",form);
        yield return ww.SendWebRequest();
        u_birthdateText.text= ww.downloadHandler.text;
        //Insurance
        form= new WWWForm();
        form.AddField("Tc_No",Tc_No);
        ww= UnityWebRequest.Post("http://192.168.1.34/GetUserInsuranceFromTc.php",form);
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

    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
    }
    
}
