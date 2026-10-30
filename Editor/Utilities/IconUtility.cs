using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LordBreakerX.EditorUtilities
{
    public static class IconUtility
    {
        public static Texture GetTypeIcon(System.Type type)
        {
            MonoScript script = MonoImporter.GetAllRuntimeMonoScripts().FirstOrDefault(script => script.GetClass() == type);

            if (script != null)
                return AssetPreview.GetMiniThumbnail(script);
            else
                return null;
        }

        public static Texture GetTypeIcon<T> ()
        {
            System.Type type = typeof(T);
            MonoScript script = MonoImporter.GetAllRuntimeMonoScripts().FirstOrDefault(script => script.GetClass() == type);

            if (script != null)
                return AssetPreview.GetMiniThumbnail(script);
            else
                return null;
        }

    }
}
