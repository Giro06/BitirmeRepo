using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MedicineStock
{
    
    public string medicineID;
    public int count;
}
[System.Serializable]
public  class StockScript : MonoBehaviour
{
    public  List<MedicineStock> MedicineStocks = new List<MedicineStock>();

    private void Awake()
    {
        Stock.stock = MedicineStocks;
    }
}

public static class Stock
{
    public static List<MedicineStock> stock = new List<MedicineStock>();

    public static bool IsEmpty()
    {
        int counter = 0;
        for (int i = 0; i < stock.Count; i++)
        {
            counter += stock[i].count;
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