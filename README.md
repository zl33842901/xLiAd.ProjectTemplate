# xLiAd.ProjectTemplate

xLiAd 模板项目

### 介绍
xLiAd 模板项目是一个简单的快速开发平台，基于 AspNetCore 3.1，可实现在数据库设计完成的情况下，几分钟搭建好内容管理平台。

集成了以下的功能：

1，基本的增删改查。数据库连接使用基于 Dapper 的 xLiAd.DapperEx（https://github.com/zl33842901/DapperEx），支持SqlServer、MySql，有计划支持 Pg。

2，日志功能。日志使用 xLiAd.DiagnosticLogCenter（https://github.com/zl33842901/DiagnosticLogCenter），切片方式自动记录接口日志。需要搭建收集端和UI端，不接入不会影响其他功能。

3，切片缓存。切片缓存使用 xLiAd.AspectCache（https://github.com/zl33842901/xLiAd.AspectCache），支持内存缓存和 Redis 缓存。

4，切片事务。基于 xLiAd.DapperEx 和 AspectCore 的支持，使数据库事务的使用大大简化，项目模板里的 AspectTransactionAttribute 类只不到40行代码，即实现切片事务，使开发人员能把精力集中在业务逻辑上。

5，超时与重试。基于 Polly 实现，代码在 AspectTimeoutRetry 类中。

6，行为次数限制。基于 xLiAd.Limiter（https://github.com/zl33842901/Limiter）。

7，代码生成。根据数据库的表结构，快速生成增删改查代码。前端采用 Vue2.0 不分离模式、Element-UI。代码生成器使用 xLiAd.Gaia （暂未开源）。

### 项目地址
模板项目开源地址：https://github.com/zl33842901/xLiAd.ProjectTemplate

官方部署地址：http://code.xliad.com/

### 使用方法：
一，打开 http://code.xliad.com/ ，输入解决方案名称，如 XLI.SampleProject ，选择数据库类型，输入数据库连接串（取得代码后再改也可），点击[生成解决方案]，下载代码。

二，把下载下来的 zip 包解压，得到项目文件，使用 Vs2019 打开解方案。

三，在解决方案中，确认 appsettings.*.json 的三个文件中的数据库连接串 SqlConnectionString 配置的数据库地址可以访问；把 WebApp 项目设为启动项目；把调试方式由 IIS Express 改为项目调试（不必须）。

四，按 F5 以调试模式运行项目；请确认跑起来的首页显示的各项配置正确（主要是编码，别出现乱码）。

五，在浏览器地址栏中输入 http://localhost:5000/Gaia 回车，稍等 20 秒（SqlServer 较慢，MySql 较快，可 F12 关注一下接口返回）。

六，选择要生成的表及功能，并在模块名称中输入合适的名称，顶部的一些字段如果输入正确可以生成正确的逻辑，建议所有表采用统一的创建时间创建人等字段；输入好之后点“确定生成”。

七，稍等片刻，系统会提示：代码已生成，请停止后重新调试。  这时，请停止调试，然后再次按 F5 运行起项目，这时，如果你的表都有主键或标识，代码就直接可以跑起来。

八，点击栏目对应菜单，进去后，增删改查功能都可以用了。
