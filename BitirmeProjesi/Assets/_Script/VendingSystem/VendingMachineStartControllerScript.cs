using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.SceneManagement;
 

public class VendingMachineStartControllerScript : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM4",9600);
  
    // Start is called before the first frame update
    void Start()
    {
       
        sp.Open();
        sp.ReadTimeout = 1;

        if (Stock.IsEmpty())
        {
            //Open out of order screen
            Debug.Log("Out of order!");
            SceneManager.LoadScene(5);
        }
           
        
    }

    // Update is called once per frame
  

    private void Update()
    {
        if (sp.IsOpen)
        {
            try
            {    
                if(sp.BytesToRead > 0)
                    ChangeScene();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(2);
    }
    IEnumerator ICheckRecipeID(string qrCode)
    {   
        
        yield return null;
    }
        
    
               
    
}
