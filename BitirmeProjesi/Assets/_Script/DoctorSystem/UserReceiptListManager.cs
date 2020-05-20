using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class UserReceiptListManager : MonoBehaviour
{
     public GameObject listPanel;
     public GameObject targetList;
     public GameObject boxPrefab;

     public TMP_Text userTcText;
     
     public void OpenPanel()
     {
          if (userTcText.text == "")
          {
               return;
          }
          listPanel.SetActive(true);
          StartCoroutine(FillList(userTcText.text));
     }

     public void ClosePanel()
     {
          for (int i = 0; i < targetList.transform.GetChildCount(); i++)
          {
                
              Destroy(targetList.transform.GetChild(i).gameObject);
               
          }
          listPanel.SetActive(false);
     }
     IEnumerator FillList(string Tc_No)
     {
          WWWForm form= new WWWForm();
          form.AddField("U_TcNum",Tc_No);
          UnityWebRequest ww= UnityWebRequest.Post("http://localhost/GetRecipeNoFromTc.php",form);
          yield return ww.SendWebRequest();
          char[] spearator = {'/'};
          string[] name = ww.downloadHandler.text.Split(spearator);

          foreach (var receiptNo in name)
          {
               if (receiptNo != "")
               {
                    CreateBox(receiptNo);
               }
          }
     }

     void CreateBox(string receiptNo)
     {
          GameObject go = Instantiate(boxPrefab, transform.position, Quaternion.identity);
          go.GetComponent<ReceiptBoxForDoctor>().SetBox(receiptNo);
          go.transform.SetParent(targetList.transform);
          go.GetComponent<RectTransform>().localScale=Vector3.one;
     }
}
