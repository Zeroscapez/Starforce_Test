using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemePlayer : MonoBehaviour
{
    // Start is called before the first frame update

   
    void Start()
    {
        AudioManager.Instance.PlaySound("CombatThemeBase");
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
