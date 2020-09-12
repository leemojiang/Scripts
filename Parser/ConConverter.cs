using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;



using UnityEngine;
using UnityEditor;

public class ConConverter : MonoBehaviour{

    public string filePath;
    [ContextMenu("CONFILE/File")]
    public void start()
    {
       StartCoroutine(convert());
    }

    // private bool start=false;
    // IEnumerator<string> file;
    // private void OnGUI()
    // {
    //     filePath = EditorGUILayout.TextField("Path For File", filePath);
        

        
    //     if (GUILayout.Button("Convert"))
    //     {   
    //        file =File.ReadLines(filePath).GetEnumerator();         
    //     }

    //     if(file!=null && file.MoveNext() ){
    //         string line = file.Current;
    //         // Debug.Log(line);



    //         file.MoveNext();
    //     }
       
    // }


    public IEnumerator convert(){
        foreach (string line in File.ReadLines(filePath) )
        {      
            Debug.Log(line);
            //jump away from the comments
            if(line.Contains("rem")) continue;

            line.Trim();

            if(line.Contains("ObjectTemplate") || line.Contains("objectTemplate") ){
                Debug.Log(line);
            }


            yield return 0;
        }

    }
}