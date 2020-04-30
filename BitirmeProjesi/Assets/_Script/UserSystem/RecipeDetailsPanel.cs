using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class RecipeDetailsPanel : MonoBehaviour
{   
    public TMP_Text u_TcText;
    public TMP_Text protocolNoText;
    public TMP_Text diagnosisText;
    public TMP_Text recipeNoText;

    public GameObject containBoxPrefab;
    public GameObject targetListObject;

    public GameObject qrBox;
    public Image myQr;
    public void SetRecipeDetails(Recipe recipe)
    {
        //Clean list before new contains
        for (int i = 0; i < targetListObject.transform.childCount; i++)
        {
            Transform go = targetListObject.transform.GetChild(i);
            go.SetParent(null);
            Destroy(go.gameObject);
        }

        //------------------------------------
        u_TcText.text = recipe.u_tcNum;
        protocolNoText.text = recipe.protocolNo;
        diagnosisText.text = recipe.diagnosis;
        recipeNoText.text = recipe.recipeNo;

        foreach (var contain in recipe.medicines)
        {
            GameObject go = Instantiate(containBoxPrefab, transform.position, Quaternion.identity);
            go.GetComponent<ContainBox>().SetContainBox(contain);
            go.transform.SetParent(targetListObject.transform);
            go.GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }

    public void GenerateQr()
    {    
        
        qrBox.SetActive(true);
        Texture2D texture = GenerateQr(recipeNoText.text, 256, 256);
        Debug.Log(texture.GetPixels32().Length);
        myQr.sprite.texture.SetPixels32(texture.GetPixels32());
        myQr.sprite.texture.Apply();
    }

    public void CloseQr()
    {
        qrBox.SetActive(false);
    }
    public static Texture2D GenerateQr(string text, int width, int height)
    {
        Texture2D encoded = new Texture2D(width,height);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }
    private static Color32[] Encode(string textForEncoding, 
        int width, int height) {
        var writer = new BarcodeWriter {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }
}
