using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    private GameObject[] status;

    public void UpdatePlayerStatus()
    {
        if (SystemController._instance.PlayerHP < 50)
        {
            Debug.Log("Damaged");
            status[0].SetActive(false);
            status[2].SetActive(true);
        }
        else
        {
            Debug.Log("Alright");
        }
    }

    
}
