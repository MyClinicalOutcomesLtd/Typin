﻿namespace Typin.Tests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Internal;
    using Xunit;
    using Xunit.Abstractions;

    public class ArgumentBindingTests
    {
        private readonly ITestOutputHelper _output;

        public ArgumentBindingTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Property_annotated_as_an_option_can_be_bound_from_multiple_values_even_if_the_inputs_use_mixed_naming()
        {
            // Arrange
            var (console, stdOut, _) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<WithStringArrayOptionCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(new[]
            {
                "cmd", "--opt", "foo", "-o", "bar", "--opt", "baz"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<WithStringArrayOptionCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);;

            commandInstance.Should().BeEquivalentTo(new WithStringArrayOptionCommand
            {
                Opt = new[] { "foo", "bar", "baz" }
            });
        }

        [Fact]
        public async Task Property_annotated_as_a_required_option_must_always_be_set()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<WithSingleRequiredOptionCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(new[]
            {
                "cmd", "--opt-a", "foo"
            });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);;
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Property_annotated_as_a_required_option_must_always_be_bound_to_some_value()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<WithSingleRequiredOptionCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(new[]
            {
                "cmd", "--opt-a"
            });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);;
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Property_annotated_as_a_required_option_must_always_be_bound_to_at_least_one_value_if_it_expects_multiple_values()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<WithRequiredOptionsCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(new[]
            {
                "cmd", "--opt-a", "foo"
            });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);;
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Property_annotated_as_parameter_is_bound_directly_from_argument_value_according_to_the_order()
        {
            // Arrange
            var (console, stdOut, _) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<WithParametersCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(new[]
            {
                "cmd", "foo", "13", "bar", "baz"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<WithParametersCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);;

            commandInstance.Should().BeEquivalentTo(new WithParametersCommand
            {
                ParamA = "foo",
                ParamB = 13,
                ParamC = new[] { "bar", "baz" }
            });
        }

        [Fact]
        public async Task Property_annotated_as_parameter_must_always_be_bound_to_some_value()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<WithSingleParameterCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(new[]
            {
                "cmd"
            });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);;
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task Property_annotated_as_parameter_must_always_be_bound_to_at_least_one_value_if_it_expects_multiple_values()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<WithParametersCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(new[]
            {
                "cmd", "foo", "13"
            });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);;
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task All_provided_option_arguments_must_be_bound_to_corresponding_properties()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(new[]
            {
                "cmd", "--non-existing-option", "13"
            });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);;
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }

        [Fact]
        public async Task All_provided_parameter_arguments_must_be_bound_to_corresponding_properties()
        {
            // Arrange
            var (console, _, stdErr) = VirtualConsole.CreateBuffered();

            var app = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>()
                .UseConsole(console)
                .Build();

            // Act
            int exitCode = await app.RunAsync(new[]
            {
                "cnd", "non-existing-parameter"
            });

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);;
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();

            _output.WriteLine(stdErr.GetString());
        }
    }
}