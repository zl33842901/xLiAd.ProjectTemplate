using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xLiAdProjectTemplate.Entities.Dtos
{
    /// <summary>
    /// 配置实体类
    /// </summary>
    public class ConfigEntity : IConfigEntity
    {
        public string SqlConnectionString { get; set; }
        public string ExceptionLogCenterUrl { get; set; }
        public bool? AspectLog { get; set; }
        public string ExceptionLogCenterClient { get; set; }
        public string ExceptionLogCenterEnv { get; set; }
        public string LoginUrl { get; set; }
        public bool EnableCache { get; set; }
        public string CacheType { get; set; }
        public string RedisUrl { get; set; }
        public int CacheTimeoutMinute { get; set; }
        public string RedisPrefix { get; set; }
        public string ExcelExportUrl { get; set; }
    }

    public interface IConfigEntity
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        string SqlConnectionString { get; }
        /// <summary>
        /// 日志记录服务地址
        /// </summary>
        string ExceptionLogCenterUrl { get; }
        /// <summary>
        /// 是否开启日志记录服务
        /// </summary>
        bool? AspectLog { get; }
        /// <summary>
        /// 日志记录服务客户端名称
        /// </summary>
        string ExceptionLogCenterClient { get; }
        /// <summary>
        /// 日志记录服务客户端环境
        /// </summary>
        string ExceptionLogCenterEnv { get; }
        /// <summary>
        /// 登录页地址
        /// </summary>
        string LoginUrl { get; }
        /// <summary>
        /// 是否开启缓存
        /// </summary>
        bool EnableCache { get; }
        /// <summary>
        /// 缓存类型 Redis Memory
        /// </summary>
        string CacheType { get; }
        /// <summary>
        /// Redis 地址
        /// </summary>
        string RedisUrl { get; }
        /// <summary>
        /// 缓存生命周期（分钟）
        /// </summary>
        int CacheTimeoutMinute { get; }
        /// <summary>
        /// Redis 键前辍
        /// </summary>
        string RedisPrefix { get; }
        /// <summary>
        /// EXCEL导出地址
        /// </summary>
        string ExcelExportUrl { get; }
    }
}
