using UnityEditor;

public class Unparenting : Editor {
	[MenuItem("GameObject/Unparent", false, 10)]
	static void CreateCustomGameObject(MenuCommand menuCommand) {
//		Get selected object
		var selectedObject = Selection.activeGameObject;
//		Unparent selected object and register it in the undo system
		Undo.SetTransformParent(selectedObject.transform, null, selectedObject.name + " Unparenting");
	}
}
