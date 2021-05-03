using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xLiAd.DapperEx.Repository;

namespace xLiAdProjectTemplate.Services.Assets
{
    public class AspectTransactionAttribute : AbstractInterceptorAttribute
    {
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var conn = context.ServiceProvider.GetService(typeof(IConnectionHolder)) as IConnectionHolder;
            conn?.BeginTransaction();
            try
            {
                var result = context.Invoke(next);
                if (result.Exception == null)
                {
                    conn.Transaction.Commit();
                }
                else
                {
                    conn.Transaction.Rollback();
                }
                return result;
            }
            catch
            {
                conn.Transaction.Rollback();
                throw;
            }
        }
    }
}
