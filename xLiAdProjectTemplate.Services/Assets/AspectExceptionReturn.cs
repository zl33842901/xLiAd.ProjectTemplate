using AspectCore.DynamicProxy;
using xLiAdProjectTemplate.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xLiAdProjectTemplate.Services.Assets
{
    /// <summary>
    /// 把服务的异常，处理掉，返回异常消息。注：要保证日志处理切片在此切片的内层（后边），不然日志切片不认为发生了异常。
    /// </summary>
    public class AspectExceptionReturn : AbstractInterceptorAttribute
    {
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            Task t = null;
            Exception ex = null;
            try
            {
                t = context.Invoke(next);
            }
            catch (Exception e) { ex = e; }
            if (t != null && t.Exception != null)
                ex = t.Exception;
            if (ex != null && context.ProxyMethod.ReturnType == typeof(IApiResultModel))
            {
                Exception exception = ex.GetInnerException();
                if (exception is ServiceException)
                {
                    context.ReturnValue = new ApiResultModelNoData()
                    {
                        code = 1001,
                        enumMsgType = null,
                        message = exception.Message,
                        result = false,
                        time = DateTime.Now.ToTimeStamp(true).ToString()
                    };
                }
                else if (exception is System.Threading.Tasks.TaskCanceledException)
                {
                    context.ReturnValue = ApiResultModel.FromError("第三方服务暂时不可用，请稍候再试。");
                }
                else if (exception is SqlException)
                {
                    if (exception.Message.StartsWith("timeout", StringComparison.OrdinalIgnoreCase))
                        context.ReturnValue = ApiResultModel.FromError("数据库连接超时，请稍候再试。");
                    else
                        context.ReturnValue = ApiResultModel.FromError("数据库执行错误，请稍候再试。");
                }
                else
                {
                    context.ReturnValue = ApiResultModel.FromError(exception.Message);
                }
                return Task.CompletedTask;
            }
            else
            {
                return t;
            }
        }
    }
}
