using SymptomsPlease.Debugging.Logging;
using SymptomsPlease.Utilities.ExtensionMethods;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace SymptomsPlease.Utilities
{
    public class GoogleSheetsDownload
    {
        public UnityWebRequest WebRequest
        {
            get; protected set;
        }
        private string m_docID;

        public GoogleSheetsDownload(string docID)
        {
            m_docID = docID;
        }

        public async Task<string> GetCSVDataFromGoogleSheet()
        {
            string url = "https://docs.google.com/spreadsheets/d/" + m_docID + "/export?format=csv";

            WebRequest = UnityWebRequest.Get(url);
            await WebRequest.SendWebRequest();

            if (WebRequest.result != UnityWebRequest.Result.Success)
            {
                CustomLogger.Error(LoggingChannels.Utilities, $"Error downloading {m_docID}: \n {WebRequest.error}");
            }

            return WebRequest.downloadHandler.text;
        }
    }
}