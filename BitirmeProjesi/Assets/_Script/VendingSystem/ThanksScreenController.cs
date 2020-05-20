using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThanksScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TurnStart());
    }

    IEnumerator TurnStart()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }
}
