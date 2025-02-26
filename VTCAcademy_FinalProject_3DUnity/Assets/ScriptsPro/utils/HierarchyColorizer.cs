using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
public static class HierarchyColorizer
{
    static HierarchyColorizer()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (gameObject != null)
        {
            string name = gameObject.name;

            // Quy tắc cho [M] (Game Manager)
            if (name.StartsWith("[M]"))
            {
                DrawColor(selectionRect, Color.white, Color.black, name, TextAnchor.MiddleCenter);
            }
            // Quy tắc cho các chữ cái đầu của 7 sắc cầu vồng
            else if (name.StartsWith("[R]")) // Đỏ
            {
                DrawColor(selectionRect, Color.red, Color.white, name, TextAnchor.MiddleCenter);
            }
            else if (name.StartsWith("[O]")) // Cam
            {
                DrawColor(selectionRect, new Color(1f, 0.5f, 0f), Color.white, name, TextAnchor.MiddleCenter); // Màu cam
            }
            else if (name.StartsWith("[Y]")) // Vàng
            {
                DrawColor(selectionRect, Color.yellow, Color.white, name, TextAnchor.MiddleCenter);
            }
            else if (name.StartsWith("[G]")) // Xanh lá
            {
                DrawColor(selectionRect, Color.green, Color.white, name, TextAnchor.MiddleCenter);
            }
            else if (name.StartsWith("[B]")) // Xanh dương
            {
                DrawColor(selectionRect, Color.blue, Color.white, name, TextAnchor.MiddleCenter);
            }
            else if (name.StartsWith("[I]")) // Chàm (Indigo)
            {
                DrawColor(selectionRect, new Color(0.29f, 0f, 0.51f), Color.white, name, TextAnchor.MiddleCenter); // Màu chàm
            }
            else if (name.StartsWith("[V]")) // Tím (Violet)
            {
                DrawColor(selectionRect, new Color(0.58f, 0f, 0.83f), Color.white, name, TextAnchor.MiddleCenter); // Màu tím
            }
        }
    }

    private static void DrawColor(Rect rect, Color backgroundColor, Color textColor, string name, TextAnchor alignment)
    {
        // Vẽ màu nền
        Rect backgroundRect = new Rect(rect.x, rect.y, rect.width, rect.height);
        EditorGUI.DrawRect(backgroundRect, backgroundColor);

        // Đặt màu chữ và căn chỉnh
        GUIStyle style = new GUIStyle();
        style.normal.textColor = textColor;
        style.alignment = alignment;

        // Vẽ tên GameObject
        EditorGUI.LabelField(rect, name, style);
    }
}
#endif