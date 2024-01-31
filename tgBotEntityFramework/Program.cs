using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Threading;
using ConsoleApp5;

class Program
{
    async public static Task Main(string[] args)
    {

        var db = new TelegramContext();
        var asd = db.Clients.ToList();
        var clientsCollection = asd.Where(a => a.username.StartsWith('a')).ToList();
         
        Console.WriteLine();


        string token = "6107397072:AAFiPGcCIla3gg1p7drE63xMNuu1zVQlUHk";
        string baseUrl = $"https://api.telegram.org/bot{token}/";
        long offset = 1;
        long forumId = -1001934929346;

        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(baseUrl);
        
        

        
        //Functions.TransferFromJson();

        //программа обрабатывает клиентов, что пишут боту
        //создает тему клиента, который написал впервые
        //пересылает сообщения клиента в эту тему
        //пересылает клиенту сообщения, написанные в его тему 


        //Дальше программа не работает, потом может быть доделаю реализацию до конца с энтити фреймворком
        while (true)
        {
            Root updates = await Functions.UpdatesAsync(client, offset);

            if (updates.result.Count > 0)
            {
                for (int i = 0; i < updates.result.Count; i++)
                {
 
                    //сообщение получено в тему с клиентом
                    if ((updates.result[i].message.chat.type == "supergroup") && (updates.result[i].message.is_topic_message == true))
                    {
                         var topicName = updates.result[i].message.reply_to_message.forum_topic_created.name;
                      // var chatId = await AdoNet.GetClientId(topicName);
           
                        var messageId = updates.result[i].message.message_id;

                       // await client.GetAsync($"copyMessage?chat_id={chatId}&from_chat_id={forumId}&message_id={messageId}");
                    }
                    //сообщение полученное от клиента в личный чат с ботом
                    else if (updates.result[i].message.chat.type == "private" && updates.result[i].message.text != null)
                    {
                        var clientName = updates.result[i].message.chat.username;
                        var chatId = updates.result[i].message.chat.id;
                        var messageId = updates.result[i].message.message_id;
                        
                        //await client.GetAsync($"sendMessage?chat_id={chatId}&text=Получено сообщение:{updates.result[i].message.text}");
                        int threadId = await Functions.isClientNewNew(chatId, clientName, client, forumId);
                        //int threadId = Functions.GetThreadId(chatId);

                        await client.GetAsync($"forwardMessage?chat_id={forumId}&message_thread_id={threadId}&from_chat_id={chatId}&message_id={messageId}");

                    }
                }
                offset = updates.result[updates.result.Count - 1].update_id + 1;
            }
        }
    }
    
    
}




 
public class ClientBase
{
    public ClientBase(string username, long chatId)
    {
        this.username = username;
        this.chatId = chatId;
        

    }
    
    
    public string username { get; set; }
    public long chatId { get; set; }
    public int threadId { get; set; }
}
