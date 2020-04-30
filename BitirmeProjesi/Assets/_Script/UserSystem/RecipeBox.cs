using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RecipeBox : MonoBehaviour
{
    public RecipePanelManager _panelManager;
    public TMP_Text recipeNoText;
    public string RecipeNo;
    
    public void SetRecipeNo(string x)
    {
        RecipeNo = x;
        recipeNoText.text = x;
    }

    public string GetRecipeNo()
    {
        return RecipeNo;
    }

    public void OpenIt()
    {
      _panelManager.OpenRecipeDetails(GetRecipeNo());   
    }
}
