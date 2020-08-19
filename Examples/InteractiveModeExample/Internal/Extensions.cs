﻿namespace InteractiveModeExample.Internal
{
    using System;
    using InteractiveModeExample.Models;
    using Typin.Console;

    internal static class Extensions
    {
        public static void RenderBook(this IConsole console, Book book)
        {
            // Title
            console.WithForegroundColor(ConsoleColor.White, () => console.Output.WriteLine(book.Title));

            // Author
            console.Output.Write("  ");
            console.Output.Write("Author: ");
            console.WithForegroundColor(ConsoleColor.White, () => console.Output.WriteLine(book.Author));

            // Published
            console.Output.Write("  ");
            console.Output.Write("Published: ");
            console.WithForegroundColor(ConsoleColor.White, () => console.Output.WriteLine($"{book.Published:d}"));

            // ISBN
            console.Output.Write("  ");
            console.Output.Write("ISBN: ");
            console.WithForegroundColor(ConsoleColor.White, () => console.Output.WriteLine(book.Isbn));
        }
    }
}