using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Connector.DirectLine;
using System.Net.Http;
using System.Threading;

namespace HealthBot
{
    class Program
    {
        public static async Task Main()
        {
            var secret = "oSja0WXU_wk.eZJsFsUbHf5cpuQ8EqNqLevrRIPqylHJBt7vIlCi1WA";
            // var secret = "kp6yaclbpmofeywzboayqvlhlquykl";
            var client = new DirectLineClient(secret);
            var cnv = client.Conversations.StartConversation();
            int index = 0;
            var old_acts = new ActivitySet();
            var new_acts = new ActivitySet();

            while (true)
            {
                var input = Console.ReadLine();
                var actGreeting = new Activity() { From = new ChannelAccount("sid", "sname"), Text = input, Type = "message" };
                old_acts = await client.Conversations.GetActivitiesAsync(cnv.ConversationId);
                var rr = await client.Conversations.PostActivityAsync(cnv.ConversationId, actGreeting);
                new_acts = await client.Conversations.GetActivitiesAsync(cnv.ConversationId);
                var diff_acts = new ActivitySet();
                diff_acts.Activities = new List<Activity>();
                for(int i = old_acts.Activities.Count; i<new_acts.Activities.Count; i++)
                {
                    diff_acts.Activities.Add(new_acts.Activities[i]);
                }
                

                while (index < diff_acts.Activities.Count)
                {
                    if (index % 2 != 0)
                    {
                        var output = diff_acts.Activities[index].Text;
                        Console.WriteLine("Genzeon Bot: " + output);
                    }
                    index++;
                }

                //var actPurpose = new Activity() { From = new ChannelAccount("sid", "sname"), Text = "Genzeon Provider", Type = "message" };
                //rr = client.Conversations.PostActivity(cnv.ConversationId, actPurpose);
                //acts = client.Conversations.GetActivities(cnv.ConversationId);
            }

            


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
