using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OutOfOrder : MonoBehaviour
{
   

    // Update is called once per frame
    void Update()
    {    
        //Out of order ekranında eğer space tuşuna basılırsa stock doldurma ekranına dön
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }
}
