namespace webapidemo.Entity
{
    public class NoteEntity
    {
        public string Id { get; set; }
        
        public string Header { get; set; }

        public string Body { get; set; }

        public PositionEntity Position {get; set;}

        public string ImagePath {get; set;}
    }
}