using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThanksScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {    
        // ilk ekrana geri dönüş fonksiyonu
        StartCoroutine(TurnStart());
    }

    IEnumerator TurnStart()
    {    
        //3 saniye bekle ve ilk ekrana dön
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }
}
