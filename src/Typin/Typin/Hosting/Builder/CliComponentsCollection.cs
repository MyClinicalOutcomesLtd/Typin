namespace Typin.Hosting.Builder
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Typin.Modes;
    using Typin.Schemas;

    internal class CliComponentsCollection : ICliComponentsCollection
    {
        private readonly List<Type> _commands = new();
        private readonly List<Type> _dynamicCommands = new();
        private readonly List<Type> _directives = new();
        private readonly List<Type> _modes = new();
        private Type? _startupMode;

        /// <inheritdoc/>
        public IReadOnlyList<Type> Commands => _commands;

        /// <inheritdoc/>
        public IReadOnlyList<Type> DynamicCommands => _dynamicCommands;

        /// <inheritdoc/>
        public IReadOnlyList<Type> Directives => _directives;

        /// <inheritdoc/>
        public IReadOnlyList<Type> Modes => _modes;

        /// <inheritdoc/>
        public Type? StartupMode => _startupMode;

        /// <inheritdoc/>
        public IServiceCollection Services { get; }

        public CliComponentsCollection(IServiceCollection services)
        {
            Services = services;
        }

        /// <inheritdoc/>
        public ICliComponentsCollection AddDirective(Type directiveType)
        {
            if (!KnownTypesHelpers.IsDirectiveType(directiveType))
            {
                throw new ArgumentException($"Invalid directive type '{directiveType}'.", nameof(directiveType));
            }

            _directives.Add(directiveType);

            Services.TryAddTransient(directiveType);
            Services.AddTransient(typeof(IDirective), directiveType);

            return this;
        }

        /// <inheritdoc/>
        public ICliComponentsCollection AddCommand(Type commandType)
        {
            if (!KnownTypesHelpers.IsCommandType(commandType))
            {
                throw new ArgumentException($"Invalid command type '{commandType}'.", nameof(commandType));
            }

            _commands.Add(commandType);

            Services.TryAddTransient(commandType);
            Services.AddTransient(typeof(ICommand), commandType);

            return this;
        }

        /// <summary>
        /// Adds a dynamic command of specified type to the application.
        /// </summary>
        public ICliComponentsCollection AddDynamicCommand(Type dynamicCommandType)
        {
            if (!KnownTypesHelpers.IsDynamicCommandType(dynamicCommandType))
            {
                throw new ArgumentException($"Invalid dynamic command type '{dynamicCommandType}'.", nameof(dynamicCommandType));
            }

            _dynamicCommands.Add(dynamicCommandType);

            Services.TryAddTransient(dynamicCommandType);
            Services.AddTransient(typeof(IDynamicCommand), dynamicCommandType);

            return this;
        }

        /// <summary>
        /// Registers a CLI mode. Only one mode can be registered as startup mode.
        /// If no mode was registered or none of the registered modes was marked as startup, <see cref="DirectMode"/> will be registered.
        ///
        /// Do not call RegisterMode directly from builder, instead call UseXMode method, e.g. UseDirectMode().
        /// </summary>
        public ICliComponentsCollection RegisterMode(Type modeType, bool asStartup = false)
        {
            Type cliMode = modeType;
            _modes.Add(cliMode);

            if (!KnownTypesHelpers.IsCliModeType(modeType))
            {
                throw new ArgumentException($"Invalid CLI mode type '{modeType}'.", nameof(modeType));
            }

            Services.TryAddSingleton(cliMode);
            Services.AddSingleton(typeof(ICliMode), (IServiceProvider sp) => sp.GetRequiredService(cliMode));

            if (asStartup)
            {
                _startupMode = _startupMode is null ? cliMode : throw new ArgumentException($"Only one mode can be registered as startup mode.", nameof(asStartup));
            }

            return this;
        }
    }
}
