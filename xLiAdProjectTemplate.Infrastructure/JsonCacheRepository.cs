using xLiAdProjectTemplate.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xLiAd.DapperEx.Repository;

namespace xLiAdProjectTemplate.Infrastructure
{
    public class JsonCacheRepository : Repository<JsonCache>, IJsonCacheRepository
    {
        public JsonCacheRepository(IConnectionHolder conn) : base(conn) { }
    }

    public interface IJsonCacheRepository : IRepository<JsonCache>
    {

    }
}
