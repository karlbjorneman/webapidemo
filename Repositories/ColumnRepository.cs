using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using webapidemo.DTO;

namespace webapidemo.Repositories
{
    public class ColumnRepository : IColumnRepository
    {

        private IMongoCollection<ColumnDto> _columnsCollection;

        public ColumnRepository(IMongoDatabase database)
        {            
            _columnsCollection = database.GetCollection<ColumnDto>("Column");

            // Text index
            var indexModel = new CreateIndexModel<ColumnDto>(Builders<ColumnDto>.IndexKeys.Combine(
                Builders<ColumnDto>.IndexKeys.Text(p => p.ColumnId)));
            _columnsCollection.Indexes.CreateOne(indexModel);
        }

        public async Task<IList<ColumnDto>> Get(string userId)
        {
            List<ColumnDto> columnDtos = await _columnsCollection.Find(column => column.UserId == userId).ToListAsync();         
            return columnDtos;
        }

        public async Task Update(string userId, string sourceColumnId, List<ObjectId> sourceNoteIds)
        {
            var updateDef = new UpdateDefinitionBuilder<ColumnDto>().Set(x => x.NoteIds, sourceNoteIds.ToArray());
            await _columnsCollection.UpdateOneAsync(col => col.ColumnId == sourceColumnId && col.UserId == userId, updateDef);
        }
    }
}