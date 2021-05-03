using AspectCore.DynamicProxy;
using Polly;
using Polly.Timeout;
using System;
using System.Threading.Tasks;

namespace xLiAdProjectTemplate.Services.Assets
{
    /// <summary>
    /// 这个类用 ISyncPolicy 同步的策略，在处理单独的重试时有点问题，不知怎么回事；处理超时加重试是好的。
    /// </summary>
    public class AspectTimeoutRetry : AbstractInterceptorAttribute
    {
        /// <summary>
        /// 最大重试次数，默认为0，不重试。
        /// </summary>
        public int MaxRetryTimes { get; set; } = 0;
        /// <summary>
        /// 超时时间（毫秒），默认为0，不限制。
        /// </summary>
        public int TimeoutByMillisecond { get; set; } = 0;
        /// <summary>
        /// 重试操作是否只针对超时
        /// </summary>
        public bool HandleTimeoutOnly { get; set; } = false;
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            ISyncPolicy policy = null;
            if (MaxRetryTimes > 0)
            {
                if (HandleTimeoutOnly)
                    policy = Policy.Handle<TimeoutRejectedException>().Retry(MaxRetryTimes);
                else
                    policy = Policy.Handle<Exception>().Retry(MaxRetryTimes);
            }
            if (TimeoutByMillisecond > 0)
            {
                var timeoutPolicy = Policy.Timeout(TimeSpan.FromMilliseconds(TimeoutByMillisecond), Polly.Timeout.TimeoutStrategy.Pessimistic);
                if (policy == null)
                    policy = timeoutPolicy;
                else
                    policy = policy.Wrap(timeoutPolicy);
            }
            if (policy == null)
                return context.Invoke(next);
            else
                return policy.Execute(() => context.Invoke(next));
        }
    }

    public class AspectRetry : AbstractInterceptorAttribute
    {
        /// <summary>
        /// 最大重试次数，默认为0，不重试。
        /// </summary>
        public int MaxRetryTimes { get; set; } = 0;
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            IAsyncPolicy policy = null;
            if (MaxRetryTimes > 0)
            {
                policy = Policy.Handle<Exception>().RetryAsync(MaxRetryTimes);
            }
            if (policy == null)
                return context.Invoke(next);
            else
                return policy.ExecuteAsync(() => context.Invoke(next));
        }
    }
}
