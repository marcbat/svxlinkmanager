using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Infrastructure.Services;
using SvxlinkManager.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SvxlinkManager.Domain.Entities;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Installer.Commands;
using LanguageExt;
using LanguageExt.Common;
using System.Reflection;
using System.Runtime.CompilerServices;
using NSubstitute.Core;

namespace SvxlinkManager.Application.Integration.Tests
{
    public class BaseTest
    {
        protected Guid configGuid = Guid.Parse("555a4521-15a1-4e02-a540-91ee600452ac");

        protected string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        protected IMediator mediatr;
        protected ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        protected ISa818Service sa818Service;
        protected ISvxlinkServiceBase svxlinkService;
        protected IAuthentificationService authentificationService;
        protected string db;

        protected List<Guid> installChannels = [
                Guid.Parse("235a4521-15a1-4e02-a540-91ee600452ac"),
                Guid.Parse("1f2e87b8-d984-4c05-8a4a-ffad65c829a9"),
                Guid.Parse("0f669a03-dcf1-4277-9b07-54f6a0fd3037"),
                Guid.Parse("a749ffe5-16c7-45da-809d-c048908f115c"),
                Guid.Parse("dd03fd9e-aeed-457e-97bf-973837a5fcec"),
                Guid.Parse("d4c59d86-947c-4b1d-831a-807c1877d426"),
                Guid.Parse("9f99b18b-96ea-453d-b07a-7923c09c939f"),
                Guid.Parse("dcc5afa2-790f-40ca-b24d-bf91e90b1ac7")
            ];

        protected VerifySettings settings;

        [ModuleInitializer]
        public static void Init() =>
            VerifyNSubstitute.Initialize();

        [SetUp]
        public void Setup()
        {
            settings = new VerifySettings();
            settings.UseDirectory(Path.Combine("snapshots"));
            //settings.AutoVerify();

            IServiceCollection services = new ServiceCollection();

            services.AddLogging(cfg => cfg.AddConsole());

            services.AddApplication();
            services.AddInfrastructure();

            if (!Directory.Exists("db"))
                Directory.CreateDirectory("db");

            // Mockup services
            db = Path.Combine("db", $"{TestContext.CurrentContext.Test.ClassName}.{TestContext.CurrentContext.Test.MethodName}.db");
            services.AddSingleton<IOptions<SvxlinkManagerOptions>>(x => new FakeOptions(db));
            
            services.AddSingleton(Substitute.For<ISa818Service>());
            services.AddSingleton(Substitute.For<ISvxlinkServiceBase>());
            services.AddScoped(x => Substitute.For<IAuthentificationService>());

            // build service provider
            var serviceProvider = services.BuildServiceProvider();

            // get services
            mediatr = serviceProvider.GetRequiredService<IMediator>();
            svxlinkManagerConfigRepository = serviceProvider.GetRequiredService<ISvxlinkManagerConfigRepository>();
            sa818Service = serviceProvider.GetRequiredService<ISa818Service>();
            svxlinkService = serviceProvider.GetRequiredService<ISvxlinkServiceBase>();
            authentificationService = serviceProvider.GetRequiredService<IAuthentificationService>();

            // configure services
            sa818Service.WriteRadioProfile(Arg.Any<RadioProfil>()).Returns(LanguageExt.Unit.Default);
            svxlinkService.StopSvxlink().Returns(LanguageExt.Unit.Default);
            svxlinkService.StartSvxlink(Arg.Any<Domain.Entities.SvxlinkChannel>(), false, null, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).ReturnsForAnyArgs(LanguageExt.Unit.Default);
            authentificationService.SeedUser(Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
        }

        protected async Task<Validation<Error, Guid>> CreateDefaultConfigAsync()
        {
            var installSvxlinkCommand = new InstallCommand("marcbat79@gmail.com",
                                                            "Pa$$w0rd",
                                                            configGuid,
                                                            installChannels,
                                                            "Fake CallSign",
                                                            "Fake AnnonceCallSign",
                                                            installChannels.First(),
                                                            "Fake Name",
                                                            "123.450",
                                                            "543.210",
                                                            "5",
                                                            "67",
                                                            "88.5",
                                                            "10",
                                                            "0.5",
                                                            "100",
                                                            "10000",
                                                            "true");

            return await mediatr.Send(installSvxlinkCommand);
        }

        protected static byte[] FileToByteArray(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
            }

            return File.ReadAllBytes(filePath);
        }

        protected IEnumerable<ICall> GetReceivedCalls()
        {
            return authentificationService.ReceivedCalls()
                                .Concat(sa818Service.ReceivedCalls())
                                .Concat(svxlinkService.ReceivedCalls())
                                .Where(x => !x.GetMethodInfo().IsSpecialName)
                                .OrderBy(x => x.GetSequenceNumber());
                                
        }

        protected void ClearReceivedCalls()
        {
            authentificationService.ClearReceivedCalls();
            sa818Service.ClearReceivedCalls();
            svxlinkService.ClearReceivedCalls();
        }
    }

    public class FakeOptions(string file) : IOptions<SvxlinkManagerOptions>
    {
        private readonly string file = file;

        public SvxlinkManagerOptions Value => new(file);
    }


}
