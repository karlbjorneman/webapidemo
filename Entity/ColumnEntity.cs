using System.Collections.Generic;

namespace webapidemo.Entity
{
    public class ColumnEntity
    {
        public string Id {get; set;}
        public string ColumnId {get; set;}
        public List<string> NoteIds {get; set;}
    }
}