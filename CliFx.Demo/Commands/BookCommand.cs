﻿using System.Threading.Tasks;
using CliFx.Attributes;
using CliFx.Demo.Internal;
using CliFx.Demo.Services;
using CliFx.Exceptions;

namespace CliFx.Demo.Commands
{
    [Command("book", Description = "View, list, add or remove books.",
             Manual = "Curabitur eros nisl, mollis et iaculis eget, dictum in massa. Aenean eu velit sit amet leo ultricies lobortis vel vel velit. Proin ultricies augue fringilla, ullamcorper nisi sed, tincidunt erat. Aenean facilisis elit in diam mollis pellentesque. Fusce tristique porta est non pulvinar. In aliquam lectus tellus, sit amet tempus lacus mollis a. Duis mattis, lorem et placerat molestie, ex enim lobortis odio, ultrices rutrum nulla dui id dui. Integer imperdiet justo ullamcorper odio rhoncus, blandit accumsan est varius. In suscipit magna metus, eget hendrerit metus sodales in.\n\nInteger volutpat mauris velit, et consequat mauris hendrerit at. Sed porta ullamcorper sodales. Phasellus quis tortor magna. Quisque ultricies hendrerit purus eget ullamcorper. Nunc condimentum dolor et lectus faucibus, pulvinar vulputate eros dapibus.\n\n\t\t\tNullam iaculis pulvinar arcu vulputate pretium. Nunc finibus risus vel leo facilisis pretium. Duis pulvinar sapien lacinia, vulputate dolor nec, ultricies lectus. Mauris ex dolor, feugiat eu enim eu, pretium dignissim lacus. Suspendisse fermentum sapien vitae luctus viverra. Donec et bibendum tortor. Nam eget mollis nisi. Sed ornare ornare semper. Aliquam tempus ante eu sodales viverra. Praesent elementum ipsum nec erat commodo, eu convallis lorem malesuada. Sed ligula lectus, pretium in massa nec, viverra semper nunc.\n\n\t\t\tIn dapibus metus at mi sodales, vel faucibus quam semper. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Curabitur finibus vel mi vitae placerat.")]
    public class BookCommand : ICommand
    {
        private readonly LibraryService _libraryService;

        [CommandParameter(0, Name = "title", Description = "Book title.")]
        public string Title { get; set; } = "";

        public BookCommand(LibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            var book = _libraryService.GetBook(Title);

            if (book == null)
                throw new CommandException("Book not found.", 1);

            console.RenderBook(book);

            return default;
        }
    }
}