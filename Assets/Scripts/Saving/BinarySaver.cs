using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Assets.Scripts
{
    public static class BinarySaver
    {
        public static void Save(object data, string savePath, string saveName)
        {
            if (Tools.CheckFileName(saveName))
            {
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(savePath + saveName + ".bin", FileMode.Create))
                {
                    try
                    {
                        formatter.Serialize(stream, data);
                    }
                    catch
                    {
                    }
                }
            }
        }
        public static T Load<T>(string loadPath)
        {
            if (File.Exists(loadPath + ".bin"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(loadPath + ".bin", FileMode.Open))
                {
                    try
                    {
                        T data = (T)formatter.Deserialize(stream);
                        return data;
                    }
                    catch
                    {
                    }
                }
            }
            return default;
        }
    }
}
