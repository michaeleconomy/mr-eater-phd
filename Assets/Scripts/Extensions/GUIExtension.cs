#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class GUIExtension {

    public static UnityEngine.Object[] DropZone(string title) {
        GUILayout.Box(title);

        EventType eventType = Event.current.type;
        bool isAccepted = false;

        if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform) {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (eventType == EventType.DragPerform) {
                DragAndDrop.AcceptDrag();
                isAccepted = true;
            }
            Event.current.Use();
        }

        return isAccepted ? DragAndDrop.objectReferences : null;
    }
}
#endif