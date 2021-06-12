namespace InteractiveModeExample.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using InteractiveModeExample.Services;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Exceptions;

    [Command("book remove", Description = "Remove a book from the library.")]
    public class BookRemoveCommand : ICommand
    {
        private readonly IConsole _console;
        private readonly LibraryService _libraryService;

        [CommandParameter(0, Name = "title", Description = "Book title.")]
        public string Title { get; init; } = string.Empty;

        public BookRemoveCommand(IConsole console, LibraryService libraryService)
        {
            _console = console;
            _libraryService = libraryService;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            var book = _libraryService.GetBook(Title);

            if (book == null)
            {
                throw new CommandException("Book not found.", 1);
            }

            _libraryService.RemoveBook(book);

            _console.Output.WriteLine($"Book {Title} removed.");

            return default;
        }
    }
}