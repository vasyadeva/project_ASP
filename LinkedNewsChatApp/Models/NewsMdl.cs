namespace LinkedNewsChatApp.Models
{
    public class NewsMdl
    {

            public int id { get; set; }
            public string title { get; set; }
            public string text { get; set; }
            public byte[] photo { get; set; }
            public DateTime Time { get; set; }
    }
}
