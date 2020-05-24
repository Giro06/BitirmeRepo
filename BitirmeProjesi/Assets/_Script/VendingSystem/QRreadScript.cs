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
    
    //Cameranın pointerı
    private WebCamTexture camTexture;
    
    //Camera outputunu gösterilecek ımage
    public RawImage rawImage;

    private string currentString;
    void Start() {
        
        // sistemdeki ilk camerayı tanımla
        camTexture = new WebCamTexture();
        // Kameranın çözünürlük ayarları
        camTexture.requestedHeight = 1000; 
        camTexture.requestedWidth = 1000;
        //Raw image 'e  kameradan gelen görüntüyü aktarıyoruz.
        rawImage.texture = camTexture;
        rawImage.material.mainTexture = camTexture;
        
        //Eğer kamera varsa görüntü almayı başlatıyoruz.
        if (camTexture != null) {
            camTexture.Play();
        }
        
    }

    void Update()
    {
       string recipeNo= ReadQrCode();
       //Stock doldurmak için 0
       if (recipeNo == "0")
       {   
           //Cam durdur ve  stock doldurma ekranına geç
           camTexture.Stop();
           SceneManager.LoadScene(0); 
       }
       // eğer gerçekden bi qr okunduysa 
       if (recipeNo != null)
       {      
           
           if (currentString != recipeNo)
           {   
               currentString = recipeNo;
               //Reçete kontrol fonksiyonunu kontrol et.
               StartCoroutine(CheckRecipe(recipeNo));
           }
       }
    }

    string ReadQrCode()
    {   
        try {
            //Barcode reader yaratıyoruz.
            IBarcodeReader barcodeReader = new BarcodeReader ();
            // decode the current frame
            // Kamerada qr code varmı yok mu diye bakıp varsa şifresini çözüyoruz.
            var result = barcodeReader.Decode(camTexture.GetPixels32(),
                camTexture.width, camTexture.height);
            if (result != null) {
                Debug.Log("DECODED TEXT FROM QR: " + result.Text);
                return result.Text;
            }
        } catch(Exception ex) { Debug.LogWarning (ex.Message); }

        return null;
    }
    
    //Reçetenin databasede var olup olmadığını kontrol ediyoruz.
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
            // reçete detay ekranına geç
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
