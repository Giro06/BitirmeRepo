using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class DoctorLoginScript : MonoBehaviour
{
    //INPUTS
    [Header("Inputs")]
    public TMP_InputField idInput;
    public TMP_InputField passwordInput;
    
    [Header("PopUp")]
    public GameObject popUp;
    public TMP_Text popUpText;

    [Header("DoctorInfoHolder")] 
    public DoctorInformation DoctorInformation;
    public void LoginButton()
    {
        StartCoroutine(ICheckLogin());
    }
    
    IEnumerator ICheckLogin()
    {
        WWWForm form= new WWWForm();
        form.AddField("D_ID",idInput.text);
        form.AddField("D_Pass",passwordInput.text);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/DoctorLoginCheck.php",form);
        yield return ww.SendWebRequest();
        PopUp(ww.downloadHandler.text);

        if (ww.downloadHandler.text == "Login Succesfull !")
        {
            DoctorInformation.doctorID = idInput.text;
            yield return new WaitForSeconds(1);
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
