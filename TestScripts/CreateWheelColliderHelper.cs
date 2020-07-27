using UnityEngine;
using System.Collections;

public class CreateWheelColliderHelper:MonoBehaviour{
    // public bool Fit_ST_Flag = false;
        public float Wheel_Distance = 2.7f;
        public float Wheel_Mass = 30.0f;
        public float Wheel_Radius = 0.3f;
        public PhysicMaterial Collider_Material;
		public Mesh Wheel_Mesh;
		public int Wheel_Materials_Num = 1;
		public Material[] Wheel_Materials;
		public Material Wheel_Material; 
		public int Num = 6;
		public float Spacing = 0.88f;

		
		public float Sus_Spring = 900.0f;
		public float Sus_Damper = 2000.0f;
		public float Sus_Distance = 2.06f;
        public float Sus_Target=0;
        public float Sus_Length=0.88f;
        public bool RealTime_Flag=true;
} 
