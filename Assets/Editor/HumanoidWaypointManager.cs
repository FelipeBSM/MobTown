using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class HumanoidWaypointManager : EditorWindow
{
    [MenuItem("Tools/Humanoid Waypoint Editor")]
    public static void Open()
    {
        GetWindow<HumanoidWaypointManager>();
    }
    public Transform waypointRoot;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);
        EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

        if(waypointRoot == null)
        {
            EditorGUILayout.HelpBox("Root transform must be selected.",MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }
        obj.ApplyModifiedProperties();
    }

    private void DrawButtons()
    {
        if(GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
            //GameObject wayPointObject
        }
        if(Selection.activeGameObject !=null && Selection.activeGameObject.GetComponent<WaypointHumanoid>())
        {
            if (GUILayout.Button("Create Waypoint Before"))
            {
                CreateWaypointBefore();
                //GameObject wayPointObject
            }
            if (GUILayout.Button("Create Waypoint After"))
            {
                CreateWaypointAfter();
                //GameObject wayPointObject
            }
            if (GUILayout.Button("Remove Waypoint"))
            {
                RemoveWaypoint();
                //GameObject wayPointObject
            }
        }
    }

   

    private void CreateWaypoint()
    {
        GameObject wayPointObject = new GameObject("Waypoint " + waypointRoot.childCount,typeof(WaypointHumanoid));
        wayPointObject.transform.SetParent(waypointRoot, false);

        WaypointHumanoid waypoint = wayPointObject.GetComponent<WaypointHumanoid>();
        if(waypointRoot.childCount > 1)
        {
            waypoint.previousWaypoint = waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<WaypointHumanoid>();
            waypoint.previousWaypoint.nextWaypoint = waypoint;

            waypoint.transform.position = waypoint.previousWaypoint.transform.position;
            waypoint.transform.forward = waypoint.previousWaypoint.transform.forward;
        }
        Selection.activeGameObject = waypoint.gameObject;

    }
    private void RemoveWaypoint()
    {
        WaypointHumanoid selectedWaypoint = Selection.activeGameObject.GetComponent<WaypointHumanoid>();
        if(selectedWaypoint.nextWaypoint != null)
        {
            selectedWaypoint.nextWaypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
            
        }
        if (selectedWaypoint.previousWaypoint != null)
        {
            selectedWaypoint.previousWaypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
            Selection.activeGameObject = selectedWaypoint.previousWaypoint.gameObject;
        }

        DestroyImmediate(selectedWaypoint.gameObject);
    }

    private void CreateWaypointAfter()
    {
        GameObject wayPointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(WaypointHumanoid));
        wayPointObject.transform.SetParent(waypointRoot, false);

        WaypointHumanoid newWaypoint = wayPointObject.GetComponent<WaypointHumanoid>();
        WaypointHumanoid selectedWaypoint = Selection.activeGameObject.GetComponent<WaypointHumanoid>();
        wayPointObject.transform.position = selectedWaypoint.transform.position;
        wayPointObject.transform.forward = selectedWaypoint.transform.forward;

        newWaypoint.previousWaypoint = selectedWaypoint;
        if (selectedWaypoint.nextWaypoint != null)
        {
            selectedWaypoint.nextWaypoint.previousWaypoint = newWaypoint;
            newWaypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
        }

        selectedWaypoint.nextWaypoint = newWaypoint;
        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());
        Selection.activeGameObject = newWaypoint.gameObject;
    }

    private void CreateWaypointBefore()
    {
        GameObject wayPointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(WaypointHumanoid));
        wayPointObject.transform.SetParent(waypointRoot, false);

        WaypointHumanoid newWaypoint = wayPointObject.GetComponent<WaypointHumanoid>();
        WaypointHumanoid selectedWaypoint = Selection.activeGameObject.GetComponent<WaypointHumanoid>();
        wayPointObject.transform.position = selectedWaypoint.transform.position;
        wayPointObject.transform.forward = selectedWaypoint.transform.forward;
        if(selectedWaypoint.previousWaypoint != null)
        {
            newWaypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
            selectedWaypoint.previousWaypoint.nextWaypoint = newWaypoint;
        }
        newWaypoint.nextWaypoint = selectedWaypoint;
        selectedWaypoint.previousWaypoint = newWaypoint;
        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());
        Selection.activeGameObject = newWaypoint.gameObject;
    }
}
