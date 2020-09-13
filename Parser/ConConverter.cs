using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;



using UnityEngine;
using UnityEditor;

public class ConConverter : EditorWindow{

    private string filePath,writeFilePath;
    private GameObject target;
    [MenuItem("CONFILE/File")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ConConverter));
    }

    IEnumerator<string> file;
    private void OnGUI()
    {
        filePath = EditorGUILayout.TextField("Path For File", filePath);
        target = (GameObject)EditorGUILayout.ObjectField("Target",target,typeof(GameObject));

        
        if (GUILayout.Button("Convert"))
        {   
           file =File.ReadLines(filePath).GetEnumerator(); 
           c=0;       
        }
        
        // if (GUILayout.Button("DE"))
        // {   
        //    Debug.Log(file);  
        //    Debug.Log(file.Current);  
        //    Debug.Log(c);       
        // }
        if(target!=null){
            
            writeFilePath = EditorGUILayout.TextField("Path For File", writeFilePath);
            if (GUILayout.Button("Convert Target to File"))
            {   
                Debug.Log(typeof(Engine));
                writeTofile();
            }
        }
       
    }

    void writeTofile (){
        // FileStream fs=  File.OpenWrite(writeFilePath);

        List<Component> comList = new List<Component>();
        foreach (var component in target.GetComponents<Component>())
        {
            comList.Add(component);
            Debug.Log(component.GetType());

            SerializedObject so = new SerializedObject (component);
            SerializedProperty sp = so.GetIterator ();
            do 
            {
                Debug.Log (sp.name);
            } while(sp.Next(true));

            
        }
      
    }



    int c=0;
    SerializedObject so;
    GameObject curObj;
    GameObject addObj;
    MonoBehaviour curComp;
    void Update(){
        if(file!=null && file.MoveNext() ){
            string line = file.Current;
            c++;

            // jump away from the comments
            if(line.Contains("rem")) return;

            line.Trim();

            if(line.Contains("ObjectTemplate") || line.Contains("objectTemplate") ){
                string[] args = line.Split('.',' ');
                
                switch (args[1])
                {
                    case "activeSafe":
                        break;
                    case "createComponent":
                        
                        break;

                    case "create":
                        if(target==null){
                            target= new GameObject(args[3]);
                            target.AddComponent(getType(args[2]));
                            curObj=target;
                        }else{
                            curObj=GameObject.Find(args[3]);
                            curObj.AddComponent(getType(args[2]));
                        }

                        so = new SerializedObject(curObj);
                        SerializedProperty sp = so.GetIterator();
                        foreach (var item in sp)
                        {
                            Debug.Log(sp.name);
                        }
                        break;
                    default:
                        break;
                }

                 
            }

        }
    }

    public Type getType(string name){

        switch (name)
        { 
            case "PlayerControlObject":
                return typeof(MonoBehaviour);
            default:
                return null;
        }
    }

    // public IEnumerator convert(){
    //     foreach (string line in )
    //     {   
    //         //jump away from the comments
    //         if(line.Contains("rem")) continue;

    //         line.Trim();

    //         if(line.Contains("ObjectTemplate") || line.Contains("objectTemplate") ){
    //             Debug.Log(line);
    //         }


    //         yield return 0;
    //     }

    //     start=false;
    // }
}