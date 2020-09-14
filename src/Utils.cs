using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputAxis{
    NULL,
    
    MouseX,MouseY,
    Horizontal,
    Vertical

}

public enum InputButtons{
    NULL,
    
    Space

}


public enum InputMouseButtons{
    
    Left=0,
    Right,
    Middle

}

public enum AxisSelect{
    X,Y,Z
}

public enum WeaponType{
    Indirect,
    Direct,
    CloseCombat
}

public enum MovingType{
    Physic,
    Math,
    Hybird
}

public enum ColliderFunctionType{
    Armor,
    FlueTank,
    Ammo,
    Engine,
    Track,
    Gun,
    Turrent
}


public enum ColliderType{
    BoxCollider,
    MeshCollider
    
}

public enum CollisionMaterial{
    Iron,
    Wood,
    Glass
}

public enum VehicleType{
    Tank
}

namespace Test
{
    public class PCO{
        public float abf;
    }
}