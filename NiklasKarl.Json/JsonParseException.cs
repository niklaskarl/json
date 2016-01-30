//-----------------------------------------------------------------------
// <copyright file="JsonParseException.cs" company="Niklas Karl">
//     Copyright © Niklas Karl 2015
// </copyright>
//-----------------------------------------------------------------------

namespace NiklasKarl.Json
{
    using System;

    /// <summary>
    /// The exception that is thrown when a string is not in valid JSON format.
    /// </summary>
    public class JsonParseException : Exception
    {
        /// <summary>
        /// The line number at which the <see cref="NiklasKarl.Json.JsonParseException"/> occurred.
        /// </summary>
        private int line;

        /// <summary>
        /// The character position in the line at which the <see cref="NiklasKarl.Json.JsonParseException"/> occurred.
        /// </summary>
        private int character;

        /// <summary>
        /// Initializes a new instance of the <see cref="NiklasKarl.Json.JsonParseException"/> class with a specified line number and a character position.
        /// </summary>
        /// <param name="line">The line number at which the <see cref="NiklasKarl.Json.JsonParseException"/> occurred.</param>
        /// <param name="character">The character position in the line at which the <see cref="NiklasKarl.Json.JsonParseException"/> occurred.</param>
        public JsonParseException(int line, int character)
        {
            this.line = line;
            this.character = character;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NiklasKarl.Json.JsonParseException"/> class with a specified line number, a character position and an error message.
        /// </summary>
        /// <param name="line">The line number at which the <see cref="NiklasKarl.Json.JsonParseException"/> occurred.</param>
        /// <param name="character">The character position in the line at which the <see cref="NiklasKarl.Json.JsonParseException"/> occurred.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public JsonParseException(int line, int character, string message) :
            base(message)
        {
            this.line = line;
            this.character = character;
        }

        /// <summary>
        /// Gets the line at which the <see cref="NiklasKarl.Json.JsonParseException"/> occurred.
        /// </summary>
        public int Line
        {
            get { return this.line; }
        }

        /// <summary>
        /// Gets the character position in the line at which the <see cref="NiklasKarl.Json.JsonParseException"/> occurred.
        /// </summary>
        public int Character
        {
            get { return this.character; }
        }
    }
}
