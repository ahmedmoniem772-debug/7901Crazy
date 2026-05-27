using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConquerOnline
{
    class TranslationSystem
    {

        public static Dictionary<string, Dictionary<string, TranslateItem>> SaveTranslate = new Dictionary<string, Dictionary<string, TranslateItem>>();


        public class TranslateItem
        {
            public string language;

            public string[] text = new string[2];
           
            public override string ToString()
            {
                var writer = new ConquerOnline.Database.DBActions.WriteLine('/');
                writer.Add(language);
                for (uint i = 0; i < text.Length; i++)
                {
                    writer.Add(text[i]);
                }
                return writer.Close();
            }
        }

        public class Translation
        {
            public int code { get; set; }
            public string lang { get; set; }
            public List<string> text { get; set; }
        }
        public static string Translate(string ClientLanguage = "", string description = "")
        {
            return "";
            string fromLanguage = "";
            string toLanguage = "";

            if (ClientLanguage == "En" || string.IsNullOrEmpty(ClientLanguage) || string.IsNullOrEmpty(description))
                return description;
            if (description.Contains("@@"))
                return description;
            else
            {
                fromLanguage = "en";
                toLanguage = ClientLanguage;
            }
            var url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=" + fromLanguage + "&tl=" + toLanguage + "&dt=t&q=" + HttpUtility.UrlEncode(description);
            var webclient = new WebClient
            {
                Encoding = System.Text.Encoding.UTF8
            };
            var result = webclient.DownloadString(url);
            try
            {
                return result.Substring(4, result.IndexOf("\"", 4, StringComparison.Ordinal) - 4);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "error";
            }

        }
        public static void Save()
        {
            using (Database.DBActions.Write _wr = new Database.DBActions.Write("TranslationSystem.txt"))
            {
                foreach (var Info in SaveTranslate.Values)
                {
                    foreach (var Words in Info.Values.ToArray())
                    {
                        _wr.Add(Words.ToString());
                    }
                }
                _wr.Execute(ConquerOnline.Database.DBActions.Mode.Open);
            }
        }
        public static void Load()
        {
            using (Database.DBActions.Read r = new Database.DBActions.Read("TranslationSystem.txt"))
            {
                if (r.Reader())
                {
                    int count = r.Count;
                    for (uint x = 0; x < count; x++)
                    {
                        Database.DBActions.ReadLine reader = new ConquerOnline.Database.DBActions.ReadLine(r.ReadString(""), '/');
                        TranslateItem Words = new TranslateItem();

                        Words.language = reader.Read("");
                        Words.text[0] = reader.Read("");
                        Words.text[1] = reader.Read("");

                        if (Words.text[0] != "" && Words.text[1] != "")
                        {
                            if (!SaveTranslate.ContainsKey(Words.language))
                                SaveTranslate.Add(Words.language, new Dictionary<string, TranslateItem>());
                            SaveTranslate[Words.language].Add(Words.text[0], Words);
                        }
                    }
                }
            }

        }
    }
}
