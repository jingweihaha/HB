using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Connector.DirectLine;
using System.Net.Http;
using System.Threading;
using System.Text.RegularExpressions;
using Microsoft.CognitiveServices.Speech;

namespace HealthBot
{
    class Program
    {
        private static int prevMsgs = 0;

        public static void Main()
        {
            var exit = false;
            do
            {
                Console.WriteLine("Genzeon Healthbot Research");
                Console.Write(" enter 1 for TextSession, enter 2 for SpeechSession: ");
                var option = Console.ReadLine();
                if(!String.IsNullOrWhiteSpace(option))
                {
                    if(option == "1")
                    {
                        var cnv = StartHBConversation();
                        exit=StartTextSession(cnv.Client, cnv.Conversation);
                        
                    }
                    else if(option == "2")
                    {
                        var cnv = StartHBConversation();
                        exit = StartSpeechSession(cnv.Client, cnv.Conversation);
                    }

                }

            } while (!exit);


        }

        private static bool StartTextSession(DirectLineClient client, Conversation cnv)
        {
            //var secret = "oSja0WXU_wk.eZJsFsUbHf5cpuQ8EqNqLevrRIPqylHJBt7vIlCi1WA";
            //Healthcare Bot APP secret:
            bool exit = false;
            //Azure SpeechtoText Configuration:
            //var config = SpeechConfig.FromSubscription("2223424bc1bd4cc0991f8813917277a6", "westus");
            do
            {
                Console.Write("Type your message or bye or exit or quit to end this session: ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;
                if (Regex.IsMatch(input, "bye|exit|quit"))
                    break;
                else
                {
                    PostToHealthBot(input, client, cnv);
                }
            } while (!exit);
            return true;
        }

        private static void PostToHealthBot(string input, DirectLineClient client, Conversation cnv)
        {
            //var botName = "gznhbdev";
            var botName = "myhbot";
            
            var usrMsg = new Activity() { From = new ChannelAccount("sid", "sname"), Text = input, Type = "message" };
            var rr = client.Conversations.PostActivity(cnv.ConversationId, usrMsg);
            Thread.Sleep(1000);
            var acts = client.Conversations.GetActivities(cnv.ConversationId);
            var botMsgs = acts.Activities.Where(a => string.Equals(a.From.Name, botName,StringComparison.OrdinalIgnoreCase));
            foreach (var bm in botMsgs.Skip(prevMsgs))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Bot Message: {bm.Text}");
            }
            Console.ResetColor();
            prevMsgs = botMsgs.Count();
        }

        private static(DirectLineClient Client,  Conversation Conversation) StartHBConversation()
        {
            //var secret = "c2Nb5-ENwqg.O7fE_fx56mxGdewzFq0JiJ561pO3PesjfLuPKVGYJJI";
            var secret = "C7qnGmJ_Uo8.cGdO53PW5MmAT7xq707cOTBfNSebO6n8waNOM1Eqc0Y";
            var client = new DirectLineClient(secret);
            return (client, client.Conversations.StartConversation());
        }

        private static bool StartSpeechSession(DirectLineClient client, Conversation cnv)
        {
            var exit = false;

            do
            {
                Console.WriteLine("Say something or bye or exit or quit to end this session...");
                var input = GetSpeechFromUser().Result;
                if (string.IsNullOrWhiteSpace(input)) continue;

                if (string.Equals(input,"bye.",StringComparison.OrdinalIgnoreCase) || string.Equals(input, "exit.", StringComparison.OrdinalIgnoreCase) || string.Equals(input, "quit.", StringComparison.OrdinalIgnoreCase))
                {
                    exit = true;
                }
                else
                {
                    PostToHealthBot(input, client, cnv);
                }
                
            } while (!exit);
            return true;
        }

        public async static Task<string> GetSpeechFromUser()
        {
            var config = SpeechConfig.FromSubscription("2223424bc1bd4cc0991f8813917277a6", "westus");
            var text = String.Empty;
            using (var recognizer = new SpeechRecognizer(config))
            {
                var result = await recognizer.RecognizeOnceAsync();
                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Captured speech: {result.Text}{Environment.NewLine}");
                    text = result.Text;
                    Console.ResetColor();
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("NOMATCH: Speech could not be recognized.");
                    Console.ResetColor();
                }
            }
            return text;
        }
    }
}
