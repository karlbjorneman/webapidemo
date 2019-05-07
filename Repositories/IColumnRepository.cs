using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using webapidemo.DTO;

namespace webapidemo.Repositories
{
    public interface IColumnRepository
    {
         Task<IList<ColumnDto>> Get(string userId);

         Task Update(string userId, string sourceColumnId, List<ObjectId> sourceNoteIds);
    }
}