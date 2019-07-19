namespace webapidemo.Model
{
    public class Note
    {
        public string Id { get; set; }
        
        public string Header { get; set; }

        public string Body { get; set; }

        public Position Position {get; set;}

        public string ImagePath {get; set;}

        public string ImageUrl {get; set;}

        public string UserId {get; set;}
    }
}