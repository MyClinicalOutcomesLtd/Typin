﻿namespace Typin.Console
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Implementation of <see cref="IConsole"/> that wraps the default system console.
    /// </summary>
    public partial class SystemConsole : IConsole
    {
        private CancellationTokenSource? _cancellationTokenSource;

        /// <inheritdoc />
        public StreamReader Input { get; }

        /// <inheritdoc />
        public bool IsInputRedirected => Console.IsInputRedirected;

        /// <inheritdoc />
        public StreamWriter Output { get; }

        /// <inheritdoc />
        public bool IsOutputRedirected => Console.IsOutputRedirected;

        /// <inheritdoc />
        public StreamWriter Error { get; }

        /// <inheritdoc />
        public bool IsErrorRedirected => Console.IsErrorRedirected;

        /// <inheritdoc />
        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        /// <inheritdoc />
        public ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        /// <summary>
        /// Initializes an instance of <see cref="SystemConsole"/>.
        /// </summary>
        public SystemConsole()
        {
            Input = WrapInput(Console.OpenStandardInput());
            Output = WrapOutput(Console.OpenStandardOutput());
            Error = WrapOutput(Console.OpenStandardError());
        }

        /// <inheritdoc />
        public void Clear()
        {
            Console.Clear();
        }

        /// <inheritdoc />
        public void ResetColor()
        {
            Console.ResetColor();
        }

        /// <inheritdoc />
        public int CursorLeft
        {
            get => Console.CursorLeft;
            set => Console.CursorLeft = value;
        }

        /// <inheritdoc />
        public int CursorTop
        {
            get => Console.CursorTop;
            set => Console.CursorTop = value;
        }

        /// <inheritdoc />
        public int WindowWidth
        {
            get => Console.WindowWidth;
            set => Console.WindowWidth = value;
        }

        /// <inheritdoc />
        public int WindowHeight
        {
            get => Console.WindowHeight;
            set => Console.WindowHeight = value;
        }

        /// <inheritdoc />
        public int BufferWidth
        {
            get => Console.BufferWidth;
            set => Console.BufferWidth = value;
        }

        /// <inheritdoc />
        public int BufferHeight
        {
            get => Console.BufferHeight;
            set => Console.BufferHeight = value;
        }

        /// <inheritdoc />
        public CancellationToken GetCancellationToken()
        {
            if (_cancellationTokenSource != null)
                return _cancellationTokenSource.Token;

            _cancellationTokenSource = new CancellationTokenSource();

            Console.CancelKeyPress += (_, args) =>
            {
                // If cancellation hasn't been requested yet - cancel shutdown and fire the token
                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    args.Cancel = true;
                    _cancellationTokenSource.Cancel();
                }
            };

            return _cancellationTokenSource.Token;
        }

        /// <inheritdoc/>
        public void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }

        /// <inheritdoc/>
        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            return Console.ReadKey(intercept);
            //return Task.Run(() => Console.ReadKey(intercept)).Result;
        }
    }

    public partial class SystemConsole
    {
        private static StreamReader WrapInput(Stream? stream)
        {
            return stream != null
                    ? new StreamReader(Stream.Synchronized(stream), Console.InputEncoding, false)
                    : StreamReader.Null;
        }

        private static StreamWriter WrapOutput(Stream? stream)
        {
            return stream != null
                    ? new StreamWriter(Stream.Synchronized(stream), Console.OutputEncoding) { AutoFlush = true }
                    : StreamWriter.Null;
        }
    }
}