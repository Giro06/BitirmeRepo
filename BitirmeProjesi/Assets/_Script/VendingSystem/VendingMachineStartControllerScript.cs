using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.SceneManagement;
 

public class VendingMachineStartControllerScript : MonoBehaviour
{    
    //Ardionu port tanımla
    SerialPort sp = new SerialPort("COM4",9600);
  
    // Start is called before the first frame update
    void Start()
    {
        // Port aktif ediliyo
        sp.Open();
        // Frame başına portdan çekilecek input.
        sp.ReadTimeout = 1;
        
        // Makineyi simüle edebilmek için
        //Stock boşmu değilmi diye kontrol ediliyo
        if (Stock.IsEmpty())
        {
            //Open out of order screen
            Debug.Log("Out of order!");
            //Out of order sahnesine git.
            SceneManager.LoadScene(5);
        }
           
        
    }

    // Update is called once per frame
    private void Update()
    {   
        //Port açıkmı kontrol et
        if (sp.IsOpen)
        {    
            try
            {    
                //Sensörden gelen input sayısını kontrol ediyoruz.
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
        // Qr okuma ekranına git.
        SceneManager.LoadScene(2);
    }
   
        
    
               
    
}
