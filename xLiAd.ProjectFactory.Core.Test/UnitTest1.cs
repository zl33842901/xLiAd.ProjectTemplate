using NUnit.Framework;
using System.IO;
using xLiAd.ProjectFactory.Core.Options;

namespace xLiAd.ProjectFactory.Core.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            CodeLoader codeLoader = new CodeLoader("./", "xLiAdProjectTemplate");
            IConvertService convertService = new ConvertService(codeLoader);
            var fileItems = convertService.Convert("SampCorp.Abc");
            Assert.Pass();
        }

        [Test]
        public void Test2()
        {
            var coding = TextEncoder.GetEncoding(new System.IO.FileStream(@"D:\MyGithub\ProjectTemplate\xLiAdProjectTemplate.WebApp\Startup.cs", FileMode.Open));

            coding = TextEncoder.GetEncoding(new System.IO.FileStream(@"D:\MyGithub\ProjectTemplate\xLiAdProjectTemplate.WebApp\appsettings.json", FileMode.Open));
        }

        [Test]
        public void TestOptions()
        {
            OptionsModel model = new OptionsModel()
            {
                Items = new OptionsModelItem[]
                {
                    new OptionsModelItem()
                    {
                        OptionCode = "SqlType",
                        OptionDescription = "请选择数据库类型",
                        Type = OptionType.Radio,
                        Options = new OptionChangeItem[]
                        {
                            new OptionChangeItem()
                            {
                                IsDefault = true,
                                ItemName = "SqlServer"
                            },
                            new OptionChangeItem()
                            {
                                IsDefault = false,
                                ItemName = "MySql",
                                ChangeDetails = new OptionChangeDetail[]
                                {
                                    new OptionChangeDetail()
                                    {
                                        FileName = "xLiAdProjectTemplate.WebApp.csproj",
                                        OldeValue = "xLiAd.Gaia.SqlServer",
                                        NewValue = "xLiAd.Gaia.MySql"
                                    },
                                    new OptionChangeDetail()
                                    {
                                        FileName = "xLiAdProjectTemplate.Infrastructure.csproj",
                                        OldeValue = "xLiAd.DapperEx.Repository",
                                        NewValue = "xLiAd.DapperEx.RepositoryMysql"
                                    },
                                    new OptionChangeDetail()
                                    {
                                        FileName = "Startup.cs",
                                        OldeValue = "AddGaiaWithSqlServer",
                                        NewValue = "AddGaiaWithMySql"
                                    },
                                    new OptionChangeDetail()
                                    {
                                        FileName = "Startup.cs",
                                        OldeValue = "new SqlConnection",
                                        NewValue = "new MySql.Data.MySqlClient.MySqlConnection"
                                    },
                                    new OptionChangeDetail()
                                    {
                                        FileName = "AuthRoleRepository.cs",
                                        OldeValue = " Repository<AuthRole>",
                                        NewValue = " RepositoryMysql<AuthRole>"
                                    },
                                    new OptionChangeDetail()
                                    {
                                        FileName = "AuthRoleRepository.cs",
                                        OldeValue = "using xLiAd.DapperEx.Repository;",
                                        NewValue = "using xLiAd.DapperEx.Repository;\r\nusing xLiAd.DapperEx.RepositoryMysql;"
                                    }
                                }
                            }
                        }
                    },
                    new OptionsModelItem()
                    {
                        OptionCode = "SqlConn",
                        OptionDescription = "请输入数据库连接串",
                        Type = OptionType.StringInput,
                        Options = new OptionChangeItem[]
                        {
                            new OptionChangeItem()
                            {
                                IsDefault = true,
                                ItemName = "server=127.0.0.1;user id=sa;password=zhanglei;database=OKR;Max Pool Size=300;"
                            },
                            new OptionChangeItem()
                            {
                                IsDefault = false,
                                ItemName = "server=127.0.0.1;user id=root;password=zhanglei;database=okr;CharSet=utf8mb4;Convert Zero Datetime=true;Allow Zero Datetime=true",
                                ChangeDetails = new OptionChangeDetail[]
                                {
                                    new OptionChangeDetail()
                                    {
                                        FileName ="appsettings.Development.json",
                                        OldeValue = "server=127.0.0.1;user id=sa;password=zhanglei;database=OKR;Max Pool Size=300;",
                                        NewValue = "$"
                                    },
                                    new OptionChangeDetail()
                                    {
                                        FileName ="appsettings.Production.json",
                                        OldeValue = "server=127.0.0.1;user id=sa;password=zhanglei;database=OKR;Max Pool Size=300;",
                                        NewValue = "$"
                                    },
                                    new OptionChangeDetail()
                                    {
                                        FileName ="appsettings.Staging.json",
                                        OldeValue = "server=127.0.0.1;user id=sa;password=zhanglei;database=OKR;Max Pool Size=300;",
                                        NewValue = "$"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(model, Newtonsoft.Json.Formatting.Indented);
        }
    }
}