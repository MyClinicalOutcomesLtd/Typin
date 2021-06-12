namespace InteractiveModeExample.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using InteractiveModeExample.Internal;
    using InteractiveModeExample.Services;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("book", Description = "List all books in the library.")]
    public class BookCommand : ICommand
    {
        private readonly IConsole _console;
        private readonly LibraryService _libraryService;

        public BookCommand(IConsole console, LibraryService libraryService)
        {
            _console = console;
            _libraryService = libraryService;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            var library = _libraryService.GetLibrary();

            var isFirst = true;
            foreach (var book in library.Books)
            {
                // Margin
                if (!isFirst)
                {
                    _console.Output.WriteLine();
                }

                isFirst = false;

                // Render book
                _console.RenderBook(book);
            }

            if (isFirst)
            {
                _console.Error.WithForegroundColor(ConsoleColor.Red, (error) => error.WriteLine("No books"));
            }

            return default;
        }
    }

    [Command("BOOK", Description = "List all books in the library.")]
    public class Book2Command : ICommand
    {
        private readonly IConsole _console;
        private readonly LibraryService _libraryService;

        public Book2Command(IConsole console, LibraryService libraryService)
        {
            _console = console;
            _libraryService = libraryService;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            var library = _libraryService.GetLibrary();

            var isFirst = true;
            foreach (var book in library.Books)
            {
                // Margin
                if (!isFirst)
                {
                    _console.Output.WriteLine();
                }

                isFirst = false;

                // Render book
                _console.RenderBook(book);
            }

            if (isFirst)
            {
                _console.Error.WithForegroundColor(ConsoleColor.Red, (error) => error.WriteLine("No BOOKS"));
            }

            return default;
        }
    }
}