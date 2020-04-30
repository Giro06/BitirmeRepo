using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class UserLogin : MonoBehaviour
{
    //INPUTS
    [Header("Inputs")]
    public TMP_InputField tcInput;
    public TMP_InputField passwordInput;
    
    [Header("PopUp")]
    public GameObject popUp;
    public TMP_Text popUpText;

    [Header("UserInfoHolder")] 
    public UserInformation userInformation;
    public void LoginButton()
    {
        StartCoroutine(ICheckLogin());
    }
    
    IEnumerator ICheckLogin()
    {
        WWWForm form= new WWWForm();
        form.AddField("Tc_No",tcInput.text);
        form.AddField("U_Password",passwordInput.text);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/UserLoginCheck.php",form);
        yield return ww.SendWebRequest();
        PopUp(ww.downloadHandler.text);

        if (ww.downloadHandler.text == "Login Succesfull !")
        {
            UserInformation.userTc = tcInput.text;
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(3);
        }
    }
    
    void PopUp(string message)
    {
        popUpText.text = message;
        popUp.SetActive(false);
        popUp.SetActive(true);
    }
}
