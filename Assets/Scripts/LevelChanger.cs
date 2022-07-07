using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    public string levelName;
    // Start is called before the first frame update
    public void ChangeLevel()
    {
        Debug.Log(levelName);
        Application.LoadLevel(levelName);
    }
 
}
