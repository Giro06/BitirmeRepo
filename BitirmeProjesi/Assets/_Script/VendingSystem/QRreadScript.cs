using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;


public class QRreadScript : MonoBehaviour
{    
    
    [Header("PopUp")]
    public GameObject popUp;
    public TMP_Text popUpText;
    
    private WebCamTexture camTexture;
    
    public RawImage rawImage;

    private string currentString;
    void Start() {
         Debug.Log("Start");
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = 1000; 
        camTexture.requestedWidth = 1000;
        rawImage.texture = camTexture;
        rawImage.material.mainTexture = camTexture;
        if (camTexture != null) {
            Debug.Log("Cam start");
            camTexture.Play();
        }
        
    }

    void Update()
    {
       string recipeNo= ReadQrCode();
       if (recipeNo == "0")
       {   
           camTexture.Stop();
           SceneManager.LoadScene(0); 
       }
       if (recipeNo != null)
       {
           if (currentString != recipeNo)
           {
               currentString = recipeNo;
               StartCoroutine(CheckRecipe(recipeNo));
           }
       }
    }

    string ReadQrCode()
    {   
        try {
            IBarcodeReader barcodeReader = new BarcodeReader ();
            // decode the current frame
            var result = barcodeReader.Decode(camTexture.GetPixels32(),
                camTexture.width, camTexture.height);
            if (result != null) {
                Debug.Log("DECODED TEXT FROM QR: " + result.Text);
                return result.Text;
            }
        } catch(Exception ex) { Debug.LogWarning (ex.Message); }

        return null;
    }

    IEnumerator CheckRecipe(string recipeNo)
    {
        WWWForm form= new WWWForm();
        form.AddField("RecipeNo", recipeNo);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/CheckRecipeFromNo.php",form);
        yield return ww.SendWebRequest();
       
        if (ww.downloadHandler.text == "True")
        {   
            PopUp("Recipe Found Succesfully!");
            RecipeInformation.RecipeNo = recipeNo;
            yield return new WaitForSeconds(2);
            camTexture.Stop();
            SceneManager.LoadScene(3);
         
        }
        else
        {
            PopUp("Recipe Not Found !");
        }

    }
    void PopUp(string message)
    {
        popUpText.text = message;
        popUp.SetActive(false);
        popUp.SetActive(true);
    }
    
}
