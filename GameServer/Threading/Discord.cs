using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace COServer
{
    public class Discord
    {
        string API = "";
        Queue<string> Msgs;
        Uri webhook;

        public Discord(string API)
        {
            this.API = API;
            Msgs = new Queue<string>();
            webhook = new Uri(API);
            var thread = new Thread(Dequeue);
            thread.Start();
        }
        private void Dequeue()
        {
            while (true)
            {
                try
                {
                    while (Msgs.Count != 0)
                    {
                        var msg = Msgs.Dequeue();
                        postToDiscord(msg);
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        public void Enqueue(string str)
        {
            Msgs.Enqueue("" + str + "");
        }
        private void postToDiscord(string Text)
        {
            HttpClient client = new HttpClient();

            Dictionary<string, string> discordToPost = new Dictionary<string, string>();
            discordToPost.Add("content", Text);

            var content = new FormUrlEncodedContent(discordToPost);

            var res = client.PostAsync(webhook, content).Result;
            if (res.IsSuccessStatusCode)
            {
            }
        }
    }
}