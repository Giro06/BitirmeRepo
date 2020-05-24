using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
[System.Serializable]
public class MedicineStock
{
    
    public string medicineID;
    public int count;
}
[System.Serializable]
public  class StockScript : MonoBehaviour
{
    List<MedicineStock> MedicineStocks = new List<MedicineStock>();
    
    // Unity ui inputlarının pointerları
    public TMP_InputField slot1MedicineID;
    public TMP_InputField slot1Count;
    public TMP_InputField slot2MedicineID;
    public TMP_InputField slot2Count;
    public TMP_InputField slot3MedicineID;
    public TMP_InputField slot3Count;
    public TMP_InputField slot4MedicineID;
    public TMP_InputField slot4Count;
    public TMP_InputField slot5MedicineID;
    public TMP_InputField slot5Count;
    public TMP_InputField slot6MedicineID;
    public TMP_InputField slot6Count;
    public void SaveAndStartButton()
    {   
        
        //Yeni bir medicineStock listesi oluşturuyoruz
        MedicineStocks= new List<MedicineStock>();
        MedicineStock temp = new MedicineStock();
        //Burda sırayla slotların dolu olup olmadığını check ediyoruz eğer doluysa listeye ekliyoruz.
        if(slot1MedicineID.text.Length>0 && slot1Count.text.Length>0)
        {
            temp.medicineID = slot1MedicineID.text;
            temp.count =int.Parse(slot1Count.text);
            MedicineStocks.Add(temp);
        }
        if(slot2MedicineID.text.Length>0 && slot2Count.text.Length>0)
        {
            temp=new MedicineStock();
            temp.medicineID = slot2MedicineID.text;
            temp.count =int.Parse(slot2Count.text);
            MedicineStocks.Add(temp);
        }
        if(slot3MedicineID.text.Length>0 && slot3Count.text.Length>0)
        {
            temp=new MedicineStock();
            temp.medicineID = slot3MedicineID.text;
            temp.count =int.Parse(slot3Count.text);
            MedicineStocks.Add(temp);
        }
        if(slot4MedicineID.text.Length>0 && slot4Count.text.Length>0)
        {
            temp=new MedicineStock();
            temp.medicineID = slot4MedicineID.text;
            temp.count =int.Parse(slot4Count.text);
            MedicineStocks.Add(temp);
        }
        if(slot5MedicineID.text.Length>0 && slot5Count.text.Length>0)
        {
            temp=new MedicineStock();
            temp.medicineID = slot5MedicineID.text;
            temp.count =int.Parse(slot5Count.text);
            MedicineStocks.Add(temp);
        }
        if(slot6MedicineID.text.Length>0 && slot6Count.text.Length>0)
        {
            temp=new MedicineStock();
            temp.medicineID = slot6MedicineID.text;
            temp.count =int.Parse(slot6Count.text);
            MedicineStocks.Add(temp);
        }
        Stock.stockList = MedicineStocks;
        SceneManager.LoadScene(1);
    }
}

// Static olunca projedeki heryerden ulaşabilirsin
public static class Stock
{
    public static List<MedicineStock> stockList = new List<MedicineStock>();

    public static bool IsEmpty()
    {
        int counter = 0;
        for (int i = 0; i < stockList.Count; i++)
        {
            counter += stockList[i].count;
        }

        if (counter == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}