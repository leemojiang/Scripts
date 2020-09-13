import UnityEngine
import UnityEditor
import 

obj = UnityEngine.GameObject.FindGameObjectsWithTag("Player")

comps = obj[0].GetComponents(UnityEngine.Component)

for comp in comps:
    print comp.GetType()