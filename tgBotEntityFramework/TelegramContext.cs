using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using static System.Net.WebRequestMethods;


namespace ConsoleApp5
{
    internal class TelegramContext : DbContext
    {
        public DbSet<ClientEntity> Clients { get; set; }
        public TelegramContext()
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
            //var collection = DBFiller.FillWithClients();
            //this.Clients.AddRange(collection);
           // this.SaveChanges();
           
             
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("DataSource=clientsBase.db;mode=readwritecreate");
        }

    }

}
public class ClientEntity : ClientBase
{
    public ClientEntity(string username, long chatId, int threadId) : base(username, chatId)
    {
        this.username = username;
        this.chatId = chatId;
        this.threadId = threadId;
    }
    
    public int Id { get; set; }
}
public static class DBFiller
{
    public static List<ClientEntity> FillWithClients()
    {

        string html = "https://jimpix.co.uk/words/random-username-list.asp";
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = web.Load(html);
        HtmlNodeCollection usernameNodes = doc.DocumentNode.SelectNodes("//li[@id]");
        List<ClientEntity> clientDBCollection = new List<ClientEntity>();
        Random random = new Random();
         

        foreach (HtmlNode node in usernameNodes)
        {
            string username = node.InnerText;
            clientDBCollection.Add(new ClientEntity(username, (long)random.Next(100000000, 999999999), random.Next(100,999)));
        }
        



        return clientDBCollection;


    }

}
