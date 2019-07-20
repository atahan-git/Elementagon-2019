using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.Callbacks;

public class GizmoIconUtility_Items {

    [DidReloadScripts]
    static GizmoIconUtility_Items () {
        EditorApplication.projectWindowItemOnGUI = ItemOnGUI;
    }

    static void ItemOnGUI (string guid, Rect rect) {
        string assetPath = AssetDatabase.GUIDToAssetPath (guid);

        ItemBase obj = AssetDatabase.LoadAssetAtPath (assetPath, typeof (ItemBase)) as ItemBase;

        if (obj != null) {
            rect.height = 65f;
            rect.width = rect.height;

            GUI.DrawTexture (rect, (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/Items/Icons/bg.psd", typeof (Texture2D)));
            GUI.DrawTexture (rect, (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/Items/Icons/bg.psd", typeof (Texture2D)));

            if (obj.sprite != null) {
               
                GUI.DrawTexture (rect, obj.sprite.texture);
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

