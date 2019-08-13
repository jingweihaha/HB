using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Connector.DirectLine;
using System.Net.Http;
using System.Threading;
using System.Text.RegularExpressions;

namespace HealthBot
{
    class Program
    {
        public static async Task Main()
        {
            var secret = "oSja0WXU_wk.eZJsFsUbHf5cpuQ8EqNqLevrRIPqylHJBt7vIlCi1WA";
            var botName = "HB-Genzeon";
            var client = new DirectLineClient(secret);
            var cnv = client.Conversations.StartConversation();
            var stringList = new List<String>();
            bool exit = false;
            int prevMsgs = 0;
            do
            {
                Console.Write("Type your message: ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;
                if (Regex.IsMatch(input, "bye|exit|quit"))
                {
                    exit = true;
                }
                else
                {
                    var usrMsg = new Activity() { From = new ChannelAccount("sid", "sname"), Text = input, Type = "message" };
                    var rr = client.Conversations.PostActivity(cnv.ConversationId, usrMsg);
                    Thread.Sleep(850);
                    var acts = client.Conversations.GetActivities(cnv.ConversationId);
                    var botMsgs = acts.Activities.Where(a => a.From.Name == botName);
                    //var botMsgs = acts.Activities;

                    foreach (var bm in botMsgs.Skip(prevMsgs))
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"Bot Message: {bm.Text}");
                    }
                    Console.ResetColor();
                    prevMsgs = botMsgs.Count();
                }
            } while (!exit);

            //while (true)
            //{
            //    int index = 0;

            //    var input = Console.ReadLine();
            //    if (input == "bye" || input == "exit")
            //    {
            //        break;
            //    }
            //    var actGreeting = new Activity() { From = new ChannelAccount("sid", "sname"), Text = input, Type = "message" };
            //    var rr = client.Conversations.PostActivity(cnv.ConversationId, actGreeting);
            //    Thread.Sleep(850);
            //    stringList.Add(rr.Id);
            //    var acts = client.Conversations.GetActivities(cnv.ConversationId);


            //    while (index < acts.Activities.Count)
            //    {
            //        if (!stringList.Contains(acts.Activities[index].Id))
            //        {
            //            var output = acts.Activities[index].Text;
            //            Console.WriteLine("Genzeon Bot: " + output);
            //            stringList.Add(acts.Activities[index].Id);
            //        }
            //        index++;
            //    }
            //    //var actPurpose = new Activity() { From = new ChannelAccount("sid", "sname"), Text = "Genzeon Provider", Type = "message" };
            //    //rr = client.Conversations.PostActivity(cnv.ConversationId, actPurpose);
            //    //acts = client.Conversations.GetActivities(cnv.ConversationId);
            //}

            //var cc = new DirectLineClientCredentials(secret);
            //cc.InitializeServiceClient(client);

            //var req = new HttpRequestMessage(HttpMethod.Post, "https://directline.botframework.com/v3/directline/tokens/generate");
            //var ct = new CancellationToken();

            //await cc.ProcessHttpRequestAsync(req, ct);
            //var cnv = client.Tokens.GenerateTokenForNewConversation();
            //var s = cnv.ConversationId;
        }
    }
}
