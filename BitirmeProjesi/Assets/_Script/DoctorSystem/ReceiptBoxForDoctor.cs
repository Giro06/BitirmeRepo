using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class ReceiptBoxForDoctor : MonoBehaviour
{
     
    public TMP_Text receiptNoText;

    public void SetBox(string receiptNo)
    {
         
        receiptNoText.text = receiptNo;
    }

    public void DeleteButton()
    {
        StartCoroutine(DeleteReceipt());
    }

    IEnumerator DeleteReceipt()
    {
        WWWForm form= new WWWForm();
        form.AddField("RecipeNo",receiptNoText.text);
        UnityWebRequest ww= UnityWebRequest.Post("http://localhost/DeleteRecipeAndContain.php",form);
        yield return ww.SendWebRequest();
        Destroy(gameObject);
    }
}
