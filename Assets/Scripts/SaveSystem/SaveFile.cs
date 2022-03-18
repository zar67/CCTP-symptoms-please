using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SymptomsPlease.SaveSystem
{
    [Serializable]
    public class SaveFile
    {
        public static SaveFileEvent OnCreated = new SaveFileEvent();
        public static SaveFileEvent OnSave = new SaveFileEvent();
        public static SaveFileEvent OnLoad = new SaveFileEvent();

        public string FilePath;
        public bool IsTextFile;

        protected Dictionary<string, string> m_data = new Dictionary<string, string>();

        public SaveFile(string filePath, bool SaveAsText)
        {
            FilePath = filePath;
            IsTextFile = SaveAsText;

            LoadData();

            OnCreated.Invoke(this);

            LoadData();
        }

        public virtual void Save()
        {
            File.WriteAllText(FilePath, "");

            LoadData();

            OnSave.Invoke(this);

            LoadData();
        }

        public virtual void Load()
        {
            LoadData();

            OnLoad.Invoke(this);
        }

        protected void LoadData()
        {
            string allData = File.ReadAllText(FilePath);
            string[] dataSeparated = allData.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            m_data = new Dictionary<string, string>();

            if (IsTextFile)
            {
                foreach (string item in dataSeparated)
                {
                    string identifier = item.Split('=')[0];
                    string data = item.Split('=')[1];

                    m_data[identifier] = data;
                }
            }
            else
            {
                foreach (string item in dataSeparated)
                {
                    string decrypted = ConvertHexToString(item, Encoding.Unicode);

                    string identifier = decrypted.Split('=')[0];
                    string data = decrypted.Split('=')[1];

                    m_data[identifier] = data;
                }
            }
        }

        public void SaveObject(string identifier, object data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            if (IsTextFile)
            {
                File.AppendAllText(FilePath, identifier + "=" + jsonData + "\n");
            }
            else
            {
                File.AppendAllText(FilePath, ConvertStringToHex(identifier + "=" + jsonData, Encoding.Unicode) + "\n");
            }
        }

        public static string ConvertStringToHex(string input, Encoding encoding)
        {
            byte[] stringBytes = encoding.GetBytes(input);
            var sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }

        public static string ConvertHexToString(string hexInput, Encoding encoding)
        {
            int numberChars = hexInput.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
            }
            return encoding.GetString(bytes);
        }

        public T LoadObject<T>(string identifier)
        {
            if (HasObject(identifier))
            {
                string data = m_data[identifier];
                return JsonConvert.DeserializeObject<T>(data);
            }

            return default;
        }

        public bool HasObject(string identifier)
        {
            return m_data.ContainsKey(identifier);
        }

        public virtual void Clear()
        {
            File.WriteAllText(FilePath, "");
            OnCreated.Invoke(this);
        }
    }
}
