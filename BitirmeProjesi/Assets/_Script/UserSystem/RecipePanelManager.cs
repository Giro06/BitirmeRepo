using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Recipe
{
    public string u_tcNum;
    public string protocolNo;
    public string diagnosis;
    public string recipeNo;
    public List<Contain> medicines = new List<Contain>();
}

public class Contain
{
    public string M_ID;
    public string M_name;
    public string Dosage;
    public string Instruction;
}
public  class RecipePanelManager : MonoBehaviour
{    
    public List<string> recipeNos = new List<string>();
    public GameObject targetListObject;
    public GameObject recipeBoxPrefab;

    public GameObject recipeDetailsPanel;
    private void Start()
    {
        StartCoroutine(IAddRecipeBoxToList(UserInformation.userTc));
    }

    IEnumerator IAddRecipeBoxToList(string tc)
    {

        yield return StartCoroutine(IGetRecipeNosFromTc(tc));
        foreach (var x in recipeNos)
        {   if(x=="")
              continue;
            GameObject go = Instantiate(recipeBoxPrefab, transform.position, Quaternion.identity);
            go.GetComponent<RecipeBox>().SetRecipeNo(x);
            go.transform.SetParent(targetListObject.transform);
            go.GetComponent<RectTransform>().localScale= Vector3.one;
            go.GetComponent<RecipeBox>()._panelManager = this;
        }
    }
    IEnumerator IGetRecipeNosFromTc(string U_TcNum)
    {    
        WWWForm form= new WWWForm(); 
        form.AddField("U_TcNum",U_TcNum); 
        UnityWebRequest ww= UnityWebRequest.Post("http://192.168.1.34/GetRecipeNoFromTc.php",form);
        yield return ww.SendWebRequest();
        char[] spearator = {'/'};
        string[] name = ww.downloadHandler.text.Split(spearator);

        foreach (string x in name)
        {
            recipeNos.Add(x);
        }
    }

    public void OpenRecipeDetails(string recipeNo)
    {
        StartCoroutine(IOpenRecipeDetails(recipeNo));
    }

    IEnumerator IOpenRecipeDetails(string recipeNo)
    {
        yield return IGetRecipeFromDatabase(recipeNo);
        recipeDetailsPanel.SetActive(true);
    }
    IEnumerator IGetRecipeFromDatabase(string recipeNo)
    {
        WWWForm form= new WWWForm(); 
        form.AddField("RecipeNo",recipeNo);
        UnityWebRequest ww= UnityWebRequest.Post("http://192.168.1.34/GetRecipeInformationFromRecipeNo.php",form);
        yield return ww.SendWebRequest();
        char[] spearator = {'/'};
        string[] recipeInfo = ww.downloadHandler.text.Split(spearator);
        // 0 is u_tc
        // 1 is protocol 
        // 2 is diagnosis
        Recipe temp = new Recipe();
        
        temp.u_tcNum = recipeInfo[0];
        temp.protocolNo = recipeInfo[1];
        temp.diagnosis = recipeInfo[2];
        temp.recipeNo = recipeNo;
        
        //Lets find all contains with this recipeNo
        
        form= new WWWForm(); 
        form.AddField("RecipeNo",recipeNo);
        ww= UnityWebRequest.Post("http://192.168.1.34/GetAllContainsFromRecipeNo.php",form);
        yield return ww.SendWebRequest();
        char[] containSpearator = {'&'};
        string[] contains = ww.downloadHandler.text.Split(containSpearator);

        foreach (var contain in contains)
        {   // If contain is empty just pass
            if(contain =="")
               continue;
            string[] tempContainHolder = contain.Split(spearator);
            // 0 is M_ID
            // 1 is Dosage 
            // 2 is Instruction
            Contain tempContain = new Contain();

            tempContain.M_ID = tempContainHolder[0];
            tempContain.Dosage = tempContainHolder[1];
            tempContain.Instruction = tempContainHolder[2];
            
            form= new WWWForm(); 
            form.AddField("M_ID",tempContain.M_ID);
            ww= UnityWebRequest.Post("http://192.168.1.34/GetMedicineNameFromID.php",form);
            yield return ww.SendWebRequest();
            tempContain.M_name = ww.downloadHandler.text;
            temp.medicines.Add(tempContain);
        }
        recipeDetailsPanel.GetComponent<RecipeDetailsPanel>().SetRecipeDetails(temp);
    }
    public void CloseRecipeDetails()
    {
        recipeDetailsPanel.SetActive(false);
    }
    
}
