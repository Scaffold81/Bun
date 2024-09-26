using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private float startingCountdownTime = 15;
    private float startUpTimer = 15f;
    
    private void Start()
    {
        StartCoroutine(StartGame());
    }

   private IEnumerator StartGame()
   {
        startUpTimer = startingCountdownTime;
        while (startUpTimer>0)
        {
            yield return new WaitForSeconds(1);
            startUpTimer--;
            
            if(startUpTimer == 0)
            {
                print(startUpTimer);
            }
            
        }

   }
}
