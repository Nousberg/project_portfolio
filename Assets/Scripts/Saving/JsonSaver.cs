using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public static class JsonSaver
    {
        public static void Save(object data, string savePath, string saveName)
        {
            if (Tools.CheckFileName(saveName))
            {
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                File.WriteAllText(savePath + saveName + ".json", JsonUtility.ToJson(data));
            }
        }
        public static T Load<T>(string loadPath)
        {
            if (File.Exists(loadPath))
            {
                return JsonUtility.FromJson<T>(File.ReadAllText(loadPath + ".json"));
            }
            return default;
        }
    }
}
