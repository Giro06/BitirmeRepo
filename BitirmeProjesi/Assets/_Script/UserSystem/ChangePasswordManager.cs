using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class ChangePasswordManager : MonoBehaviour
{
    public TMP_InputField currentPasswordInput;
    public TMP_InputField newPasswordInput;
    public TMP_InputField confirmPasswordInput;

    public TMP_Text bugText;
    public void SaveButton()
    {
        if (currentPasswordInput.text.Length != 0)
        {
            if (newPasswordInput.text == confirmPasswordInput.text)
            {
                StartCoroutine(ChangePassword());
            }
            else
            {
                bugText.text = "New password not match!";
            }
        }
        else
        {
            bugText.text = "Enter current password!";
        }
    }

    public void CloseSettingPanel()
    {
        gameObject.SetActive(false);
        bugText.text = "";
    }
    IEnumerator ChangePassword()
    {
        WWWForm form= new WWWForm();
        form.AddField("Tc_No",UserInformation.userTc);
        form.AddField("U_Password",currentPasswordInput.text);
        UnityWebRequest ww= UnityWebRequest.Post("http://192.168.1.34/UserLoginCheck.php",form);
        yield return ww.SendWebRequest();
        
        if (ww.downloadHandler.text == "Login Succesfull !")
        {
            form.AddField("Tc_No",UserInformation.userTc);
            form.AddField("U_Password",newPasswordInput.text);
            ww= UnityWebRequest.Post("http://192.168.1.34/UpdatePasswordWithTc.php",form);
            yield return ww.SendWebRequest();
            bugText.text = ww.downloadHandler.text;
        }
        else
        {
            bugText.text = "Wrong password !";
        }
    }
}
