using UnityEngine;
using System.Collections;

public class CreateRoadWheelHelper:MonoBehaviour{
    public bool Fit_ST_Flag = false;

		public float Sus_Distance = 2.06f;
		public int Num = 6;
		public float Spacing = 0.88f;
		public float Sus_Length = 0.5f;
		public bool Set_Individually = false;
		public float Sus_Angle = 0.0f;
		public float [] Sus_Angles;
		public float Sus_Anchor = 0.0f;
		public float Sus_Mass = 30.0f;
		public float Sus_Spring = 900.0f;
		public float Sus_Damper = 2000.0f;
		public float Sus_Target = 30.0f;
		public float Sus_Forward_Limit = 30.0f;
		public float Sus_Backward_Limit = 30.0f;
		public Mesh Sus_L_Mesh;
		public Mesh Sus_R_Mesh;
		public int Sus_Materials_Num = 1;
		public Material[] Sus_Materials;
		public Material Sus_L_Material; // for old versions.
		public Material Sus_R_Material; // for old versions.
		public float Reinforce_Radius = 0.5f;

		public float Wheel_Distance = 2.7f;
		public float Wheel_Mass = 30.0f;
		public float Wheel_Radius = 0.3f;
		public PhysicMaterial Collider_Material;
		public Mesh Wheel_Mesh;
		public int Wheel_Materials_Num = 1;
		public Material[] Wheel_Materials;
		public Material Wheel_Material; // for old versions.

		public bool Drive_Wheel = true;
		public bool Use_BrakeTurn = true;
		public bool Wheel_Resize = false;
		public float ScaleDown_Size = 0.5f;
		public float Return_Speed = 0.05f;

		public bool RealTime_Flag = true;

} 
