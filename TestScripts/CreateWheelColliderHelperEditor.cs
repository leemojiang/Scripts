using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CreateWheelColliderHelper))]
public class CreateWheelColliderHelperEditor:Editor{

    SerializedProperty NumProp;
    SerializedProperty SpacingProp;
    
    SerializedProperty Sus_SpringProp;
    SerializedProperty Sus_DamperProp;
    SerializedProperty Sus_LengthProp;
    SerializedProperty Sus_TargetProp;
    SerializedProperty Wheel_DistanceProp;
    SerializedProperty Wheel_MassProp;
    SerializedProperty Wheel_RadiusProp;
    SerializedProperty Collider_MaterialProp;
    SerializedProperty Wheel_MeshProp;
    SerializedProperty Wheel_Materials_NumProp;
    SerializedProperty Wheel_MaterialsProp;
    SerializedProperty Wheel_MaterialProp;
    SerializedProperty RealTime_FlagProp;
    
    Transform thisTransform;
    void OnEnable(){
        NumProp = serializedObject.FindProperty ("Num");
        SpacingProp = serializedObject.FindProperty ("Spacing");
        
        Sus_SpringProp = serializedObject.FindProperty ("Sus_Spring");
        Sus_DamperProp = serializedObject.FindProperty ("Sus_Damper");
        Sus_LengthProp = serializedObject.FindProperty ("Sus_Length");  
        Sus_TargetProp = serializedObject.FindProperty("Sus_Target");

        Wheel_DistanceProp=serializedObject.FindProperty("Wheel_Distance");
        Wheel_MassProp = serializedObject.FindProperty ("Wheel_Mass");		
        Wheel_RadiusProp = serializedObject.FindProperty ("Wheel_Radius");
        Collider_MaterialProp = serializedObject.FindProperty ("Collider_Material");
        Wheel_MeshProp = serializedObject.FindProperty ("Wheel_Mesh");
        Wheel_Materials_NumProp = serializedObject.FindProperty ("Wheel_Materials_Num");
        Wheel_MaterialsProp = serializedObject.FindProperty ("Wheel_Materials");
        Wheel_MaterialProp = serializedObject.FindProperty ("Wheel_Material");

        RealTime_FlagProp = serializedObject.FindProperty ("RealTime_Flag");
        if (Selection.activeGameObject) {
            thisTransform = Selection.activeGameObject.transform;
        }
    }


    public override void OnInspectorGUI ()
    {
        bool isPrepared;
        if (Application.isPlaying || thisTransform.parent == null || thisTransform.parent.gameObject.GetComponent<Rigidbody> () == null) {
            isPrepared = false;
        } else {
            isPrepared = true;
        }

        if (isPrepared) {
				// Keep rotation.
            Vector3 ang = thisTransform.localEulerAngles;
            if (ang.z != 90.0f) {
                ang.z = 90.0f;
                thisTransform.localEulerAngles = ang;
            }
            // Set Inspector window.
            Set_Inspector ();
            // Update (Recreate) the parts.
            if (GUI.changed && RealTime_FlagProp.boolValue) {
                Create ();
            }
            if (Event.current.commandName == "UndoRedoPerformed") {
                Create ();
            }
        }
    }

    void Set_Inspector ()
    {
        serializedObject.Update ();

        GUI.backgroundColor = new Color (1.0f, 1.0f, 0.7f, 1.0f);

        //wheel collider settings
        EditorGUILayout.Space ();
        EditorGUILayout.HelpBox ("wheel collider settings", MessageType.None, true);
        
        EditorGUILayout.IntSlider (NumProp, 0, 30, "Number");
        EditorGUILayout.Slider (SpacingProp, 0.1f, 10.0f, "Spacing");

        EditorGUILayout.Slider (Wheel_DistanceProp, 0.1f, 10.0f, "Distance");
        EditorGUILayout.Slider (Wheel_MassProp, 0.1f, 300.0f, "Mass");
        EditorGUILayout.Space ();
		EditorGUILayout.Space ();
        GUI.backgroundColor = new Color (1.0f, 0.5f, 0.5f, 1.0f);
        EditorGUILayout.Slider (Wheel_RadiusProp, 0.01f, 1.0f, "wheelCollider Radius");
        GUI.backgroundColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);
        Collider_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ("Physic Material", Collider_MaterialProp.objectReferenceValue, typeof(PhysicMaterial), false);
        EditorGUILayout.Space ();	
        //spring settings
        EditorGUILayout.Space ();
        EditorGUILayout.HelpBox ("Sus settings fot wheel collider", MessageType.None, true);

        EditorGUILayout.Slider (Sus_SpringProp, 0.0f, 100000.0f, "Sus Spring Force");
        if (Sus_SpringProp.floatValue == 100000.0f) {
            Sus_SpringProp.floatValue = Mathf.Infinity;
        }
        EditorGUILayout.Slider (Sus_DamperProp, 0.0f, 10000.0f, "Sus Damper Force");
		EditorGUILayout.Slider (Sus_LengthProp, 0.0f, 1.0f, "Length");
        EditorGUILayout.Slider (Sus_TargetProp, 0.0f, 1.0f, "Sus Spring Target Position");
        //mesh settings
        Collider_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ("Physic Material", Collider_MaterialProp.objectReferenceValue, typeof(PhysicMaterial), false);
        EditorGUILayout.Space ();
        Wheel_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ("Mesh", Wheel_MeshProp.objectReferenceValue, typeof(Mesh), false);

        EditorGUILayout.IntSlider (Wheel_Materials_NumProp, 1, 10, "Number of Materials");
        Wheel_MaterialsProp.arraySize = Wheel_Materials_NumProp.intValue;
        if (Wheel_Materials_NumProp.intValue == 1 && Wheel_MaterialProp.objectReferenceValue != null) {
            if (Wheel_MaterialsProp.GetArrayElementAtIndex (0).objectReferenceValue == null) {
                Wheel_MaterialsProp.GetArrayElementAtIndex (0).objectReferenceValue = Wheel_MaterialProp.objectReferenceValue;
            }
            Wheel_MaterialProp.objectReferenceValue = null;
        }
        EditorGUI.indentLevel++;
        for (int i = 0; i < Wheel_Materials_NumProp.intValue; i++) {
            Wheel_MaterialsProp.GetArrayElementAtIndex (i).objectReferenceValue = EditorGUILayout.ObjectField ("Material " + "(" + i + ")", Wheel_MaterialsProp.GetArrayElementAtIndex (i).objectReferenceValue, typeof(Material), false);
        }
        EditorGUI.indentLevel--;
        
        //updates
        EditorGUILayout.Space ();
        EditorGUILayout.Space ();
        RealTime_FlagProp.boolValue = EditorGUILayout.Toggle ("Real Time Update", RealTime_FlagProp.boolValue);
        if (GUILayout.Button ("Update Values")) {
            if (RealTime_FlagProp.boolValue == false) {
                Create ();
            }
        }
        EditorGUILayout.Space ();
        EditorGUILayout.Space ();

        serializedObject.ApplyModifiedProperties ();
    }

    void Create(){
        int childCount = thisTransform.childCount;
        for (int i = 0; i < childCount; i++) {
            DestroyImmediate (thisTransform.GetChild (0).gameObject);
        }
        // Create Wheel
        for (int i = 0; i < NumProp.intValue; i++) {
            Create_Wheel ("L", i + 1);
        }
        for (int i = 0; i < NumProp.intValue; i++) {
            Create_Wheel ("R", i + 1);
        }
    }

    void Create_Wheel(string direction, int number){
        // Create gameobject & Set parent.
        GameObject wheelObject = new GameObject ("RoadWheel_" + direction + "_" + number);
        wheelObject.transform.parent = thisTransform;

        // Set position.
        Vector3 pos;
        // pos.x = Mathf.Sin (Mathf.Deg2Rad * (180.0f + Sus_AngleProp.floatValue)) * Sus_LengthProp.floatValue;
        // pos.z = Mathf.Cos (Mathf.Deg2Rad * (180.0f + Sus_AngleProp.floatValue)) * Sus_LengthProp.floatValue;
        pos.x=0;
        pos.z=0;

        pos.z -= SpacingProp.floatValue * (number -1);
        pos.y = Wheel_DistanceProp.floatValue / 2.0f;
        if (direction == "R") {
            pos.y *= -1.0f;
        }
        wheelObject.transform.localPosition = pos;

        // Set rotation.
        if (direction == "L") { // Left
            wheelObject.transform.localRotation = Quaternion.Euler (Vector3.zero);
        } else { // Right
            wheelObject.transform.localRotation = Quaternion.Euler (0.0f, 0.0f, 180);
        }

        // Mesh
        if (Wheel_MeshProp.objectReferenceValue) {
            MeshFilter meshFilter = wheelObject.AddComponent < MeshFilter > ();
            meshFilter.mesh = Wheel_MeshProp.objectReferenceValue as Mesh;
            MeshRenderer meshRenderer = wheelObject.AddComponent < MeshRenderer > ();
            Material[] materials = new Material [ Wheel_Materials_NumProp.intValue ];
            for (int i = 0; i < materials.Length; i++) {
                materials [i] = Wheel_MaterialsProp.GetArrayElementAtIndex (i).objectReferenceValue as Material;
            }
            meshRenderer.materials = materials;
        }

         // WheelCollider
			WheelCollider wheelCollider = wheelObject.AddComponent < WheelCollider > ();
			wheelCollider.radius = Wheel_RadiusProp.floatValue;
			wheelCollider.center = Vector3.zero;
			wheelCollider.mass=Wheel_MassProp.floatValue;
            // wheelCollider.material= Collider_MaterialProp.objectReferenceValue as PhysicMaterial;
            wheelCollider.suspensionDistance=Sus_LengthProp.floatValue;
            
            JointSpring jointSpring = wheelCollider.suspensionSpring;
			jointSpring.spring = Sus_SpringProp.floatValue;
			jointSpring.damper = Sus_DamperProp.floatValue;
            jointSpring.targetPosition=Sus_TargetProp.floatValue;
			wheelCollider.suspensionSpring = jointSpring;
			
            Wheel wheel = wheelObject.AddComponent<Wheel>();
            wheel.controlScript=thisTransform.parent.gameObject.GetComponent< Engine > ();
    
            
    }
}