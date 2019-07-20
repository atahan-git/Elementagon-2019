using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.Callbacks;

public class GizmoIconUtility_Cards {

    [DidReloadScripts]
    static GizmoIconUtility_Cards () {
        EditorApplication.projectWindowItemOnGUI = ItemOnGUI;
    }

    static void ItemOnGUI (string guid, Rect rect) {
        string assetPath = AssetDatabase.GUIDToAssetPath (guid);

		CardBase obj = AssetDatabase.LoadAssetAtPath (assetPath, typeof (CardBase)) as CardBase;

        if (obj != null) {
            rect.height = 65f;
            rect.width = rect.height;

            if (obj.mySprite != null) {
               
                GUI.DrawTexture (rect, obj.mySprite.texture);
            }

            /*if (obj._Id == 1) {
                GUI.DrawTexture (rect, (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/Gizmos/MyScriptableObject_id-1.png", typeof (Texture2D)));
            } else if (obj._Id == 2) {
                GUI.DrawTexture (rect, (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/Gizmos/MyScriptableObject_id-2.png", typeof (Texture2D)));
            }*/
        }
    }

    /*Texture2D textureFromSprite (Sprite sprite) {
        if (sprite.rect.width != sprite.texture.width) {
            Texture2D newText = new Texture2D ((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels ((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels (newColors);
            newText.Apply ();
            return newText;
        } else
            return sprite.texture;
    }*/
}

