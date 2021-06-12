namespace Typin
{
    /// <summary>
    /// Builds an instance of <see cref="CliApplication"/>.
    /// </summary>
    public sealed class CliApplicationBuilder
    {
        //        private bool _cliApplicationBuilt;

        //        //Directives and commands settings
        //        private readonly List<Type> _commandTypes = new();
        //        private readonly List<Type> _directivesTypes = new();

        //        //Metadata settings
        //        private string? _title;
        //        private string? _executableName;
        //        private string? _versionText;
        //        private string? _description;
        //        private Action<ApplicationMetadata, IConsole>? _startupMessage;

        //        //Console
        //        private IConsole? _console;

        //        //Modes
        //        private readonly List<Type> _modeTypes = new();
        //        private Type? _startupMode;

        //        //Middleware
        //        private readonly LinkedList<Type> _middlewareTypes = new();

        //        /// <summary>
        //        /// Initializes an instance of <see cref="CliApplicationBuilder"/>.
        //        /// </summary>
        //        public CliApplicationBuilder()
        //        {
        //            this.AddBeforeUserMiddlewares();
        //        }

        //        #region Metadata
        //        /// <summary>
        //        ///     Sets application title, which appears in the help text.
        //        /// </summary>
        //        public CliApplicationBuilder UseTitle(string title)
        //        {
        //            _title = title;

        //            return this;
        //        }

        //        /// <summary>
        //        /// Sets application executable name, which appears in the help text.
        //        /// </summary>
        //        public CliApplicationBuilder UseExecutableName(string executableName)
        //        {
        //            _executableName = executableName;

        //            return this;
        //        }

        //        /// <summary>
        //        /// Sets application version text, which appears in the help text and when the user requests version information.
        //        /// </summary>
        //        public CliApplicationBuilder UseVersionText(string versionText)
        //        {
        //            _versionText = versionText;

        //            return this;
        //        }

        //        /// <summary>
        //        /// Sets application description, which appears in the help text.
        //        /// </summary>
        //        public CliApplicationBuilder UseDescription(string? description)
        //        {
        //            _description = description;

        //            return this;
        //        }
        //        #endregion

        //        #region Startup message
        //        /// <summary>
        //        /// Sets application startup message, which appears just after starting the app.
        //        /// </summary>
        //        public CliApplicationBuilder UseStartupMessage(string message, ConsoleColor messageColor = ConsoleColor.DarkYellow)
        //        {
        //            _startupMessage = (metadata, console) =>
        //            {
        //                console.Output.WithForegroundColor(messageColor, (output) => output.WriteLine(message));
        //            };

        //            return this;
        //        }

        //        /// <summary>
        //        /// Sets application startup message, which appears just after starting the app.
        //        /// </summary>
        //        public CliApplicationBuilder UseStartupMessage(Func<ApplicationMetadata, string> message, ConsoleColor messageColor = ConsoleColor.DarkYellow)
        //        {
        //            _startupMessage = (metadata, console) =>
        //            {
        //                string tmp = message(metadata);

        //                console.Output.WithForegroundColor(messageColor, (output) => output.WriteLine(tmp));
        //            };

        //            return this;
        //        }

        //        /// <summary>
        //        /// Sets application startup message, which appears just after starting the app.
        //        /// </summary>
        //        public CliApplicationBuilder UseStartupMessage(Action<ApplicationMetadata, IConsole> message)
        //        {
        //            _startupMessage = message;

        //            return this;
        //        }
        //        #endregion

        //        #region Middleware
        //        /// <summary>
        //        /// Adds a middleware to the command execution pipeline.
        //        /// Middlewares are also registered as scoped services and are executed in registration order.
        //        /// </summary>
        //        public CliApplicationBuilder UseMiddleware(Type middleware)
        //        {
        //            _configureServicesActions.Add(services =>
        //            {
        //                services.AddScoped(typeof(IMiddleware), middleware);
        //                services.AddScoped(middleware);
        //            });

        //            _middlewareTypes.AddLast(middleware);

        //            return this;
        //        }

        //        /// <summary>
        //        /// Adds a middleware to the command execution pipeline.
        //        /// </summary>
        //        public CliApplicationBuilder UseMiddleware<TMiddleware>()
        //            where TMiddleware : class, IMiddleware
        //        {
        //            return UseMiddleware(typeof(TMiddleware));
        //        }
        //        #endregion

        //        /// <summary>
        //        /// Creates an instance of <see cref="CliApplication"/> using configured parameters.
        //        /// Default values are used in place of parameters that were not specified.
        //        /// A scope is defined as a lifetime of a command execution pipeline that includes directives handling.
        //        /// </summary>
        //        public CliApplication Build()
        //        {
        //            if (_cliApplicationBuilt)
        //            {
        //                throw new InvalidOperationException("Build can only be called once.");
        //            }

        //            _cliApplicationBuilt = true;

        //            // Set default values
        //            _title ??= AssemblyUtils.TryGetDefaultTitle() ?? "App";
        //            _executableName ??= AssemblyUtils.TryGetDefaultExecutableName() ?? "app";
        //            _versionText ??= AssemblyUtils.TryGetDefaultVersionText() ?? "v1.0";
        //            _console ??= new SystemConsole();

        //            if (_startupMode is null || _modeTypes.Count == 0)
        //            {
        //                //this.UseDirectMode(true);
        //            }

        //            // Add core middlewares to the end of the pipeline
        //            this.AddAfterUserMiddlewares();

        //            // Create context
        //            ServiceCollection _serviceCollection = new();

        //            ApplicationMetadata metadata = new(_title, _executableName, _versionText, _description);
        //            ApplicationConfiguration configuration = new(_modeTypes,
        //                                                         _commandTypes,
        //                                                         _directivesTypes,
        //                                                         _middlewareTypes,
        //                                                         _startupMode!);

        //            // EnvironmentVariablesAccessor environmentVariablesAccessor = new();

        //            // Add core services
        //            _serviceCollection.AddOptions();
        //            _serviceCollection.AddSingleton(typeof(ApplicationMetadata), metadata);
        //            _serviceCollection.AddSingleton(typeof(ApplicationConfiguration), configuration);
        //            _serviceCollection.AddSingleton(typeof(IConsole), _console);
        //            //_serviceCollection.AddSingleton(typeof(IEnvironmentVariablesAccessor), environmentVariablesAccessor);
        //            _serviceCollection.AddSingleton<IRootSchemaAccessor, RootSchemaAccessor>();
        //            _serviceCollection.AddSingleton<ICliCommandExecutor, CliCommandExecutor>();
        //            _serviceCollection.AddSingleton<ICliApplicationLifetime, CliApplicationLifetime>();

        //            //_serviceCollection.AddScoped(typeof(ICliContext), (provider) =>
        //            //{
        //            //    IRootSchemaAccessor rootSchemaAccessor = provider.GetRequiredService<IRootSchemaAccessor>();

        //            //    return new CliContext(metadata,
        //            //                          configuration,
        //            //                          rootSchemaAccessor.RootSchema,
        //            //                          null!,
        //            //                          _console);
        //            //});

        //            _serviceCollection.AddLogging(cfg =>
        //            {
        //                cfg.ClearProviders();
        //                cfg.AddDebug();
        //                cfg.SetMinimumLevel(LogLevel.Information);
        //            });

        //            IServiceProvider serviceProvider = CreateServiceProvider(_serviceCollection);

        //            return null!;//new CliApplication(serviceProvider, _console, null!, metadata, _startupMessage);
        //        }

        //        private IServiceProvider CreateServiceProvider(ServiceCollection services)
        //        {
        //            foreach (Action<IServiceCollection> configureServicesAction in _configureServicesActions)
        //            {
        //                configureServicesAction(services);
        //            }

        //            services.TryAddSingleton<IOptionFallbackProvider, EnvironmentVariableFallbackProvider>();
        //            services.TryAddScoped<ICliExceptionHandler, DefaultExceptionHandler>();
        //            services.TryAddScoped<IHelpWriter, DefaultHelpWriter>();

        //            object? containerBuilder = _serviceProviderAdapter.CreateBuilder(services);
        //            foreach (IConfigureContainerAdapter containerAction in _configureContainerActions)
        //            {
        //                containerAction.ConfigureContainer(containerBuilder);
        //            }

        //            IServiceProvider? appServices = _serviceProviderAdapter.CreateServiceProvider(containerBuilder);

        //            return appServices ?? throw new InvalidOperationException($"{nameof(IServiceFactoryAdapter)} returned a null instance of object implementing {nameof(IServiceProvider)}.");
        //        }
    }
}