using UnityEditor;
using UnityEngine;

//This script draws lines in editor between waypoints (game objects which have 'waypoint.cs' attached)

[CustomEditor(typeof(WaypointsHolder), true)] 
public class WaypointLines : Editor {

	Transform[] childs;

	void OnSceneGUI () {
		WaypointsHolder wp = target as WaypointsHolder;

		//retrieve all children objects in WaypointsHolder game object
		if(wp.GetComponent<Waypoint>())
		{
			//if selected object is child, than save all children of its parrent in array
			if(wp.transform.parent)
				childs = wp.transform.parent.GetComponentsInChildren<Transform>();
		}
		else
		{
			//save all children in array
			if(wp.transform.childCount > 1)
				childs = wp.GetComponentsInChildren<Transform>();
		}

		Handles.color = Color.green;

		//draw lines between child game objects
		if(wp.drawLines && childs != null && childs.Length > 1) {
			for(var i = 1; i < childs.Length - 1; i++)
				Handles.DrawLine(childs[i].position, childs[i+1].position);
		}
	}
}
