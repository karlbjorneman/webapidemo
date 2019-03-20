using System.Collections.Generic;

namespace webapidemo.Model
{
    public class Column
    {
        public string Id {get; set;}
        public string ColumnId {get; set;}
        public List<string> NoteIds {get; set;}
    }
}