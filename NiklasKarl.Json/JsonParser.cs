//-----------------------------------------------------------------------
// <copyright file="JsonParser.cs" company="Niklas Karl">
//     Copyright © Niklas Karl 2015
// </copyright>
//-----------------------------------------------------------------------

namespace NiklasKarl.Json
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Parses a string to a <see cref="NiklasKarl.Json.JsonValue"/>.
    /// </summary>
    internal class JsonParser : IDisposable
    {
        /// <summary>
        /// The string representation of a <see cref="NiklasKarl.Json.JsonNull"/>.
        /// </summary>
        private const string NullString = "null";

        /// <summary>
        /// The string representation of a <see cref="NiklasKarl.Json.JsonBoolean"/> with value <c>false</c>.
        /// </summary>
        private const string FalseString = "false";

        /// <summary>
        /// The string representation of a <see cref="NiklasKarl.Json.JsonBoolean"/> with value <c>true</c>.
        /// </summary>
        private const string TrueString = "true";

        private TextReader reader;

        private char[] buffer;

        private int bufferStart;

        private int bufferLength;

        private int bufferPosition;

        /// <summary>
        /// The number of the line the <see cref="NiklasKarl.Json.JsonParser"/> is currently at in the string to parse.
        /// </summary>
        private int line;

        /// <summary>
        /// The position of the first character of the line the <see cref="NiklasKarl.Json.JsonParser"/> is currently at in the string to parse.
        /// </summary>
        private int lineStart;

        /// <summary>
        /// The <see cref="System.Text.StringBuilder"/> used to compose a <see cref="NiklasKarl.Json.JsonString"/>.
        /// </summary>
        private StringBuilder builder;

        /// <summary>
        /// The <see cref="System.IFormatProvider"/> used to parse a <see cref="NiklasKarl.Json.JsonNumber"/>.
        /// </summary>
        private IFormatProvider numberFormat;

        /// <summary>
        /// The <see cref="System.Globalization.NumberStyles"/> used to parse a <see cref="NiklasKarl.Json.JsonNumber"/>.
        /// </summary>
        private NumberStyles numberStyle;

        /// <summary>
        /// Initializes a new instance of the <see cref="NiklasKarl.Json.JsonParser"/> class with a specified string to parse.
        /// </summary>
        /// <param name="data">The string to parse.</param>
        private JsonParser(TextReader reader)
        {
            this.reader = reader;
            
            this.line = 1;
            this.lineStart = 0;
            
            this.buffer = new char[512];
            this.bufferStart = 0;
            this.bufferPosition = 0;
            this.bufferLength = 0;

            this.builder = new StringBuilder(64);

            this.numberFormat = NumberFormatInfo.InvariantInfo;
            this.numberStyle = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent;
        }

        /// <summary>
        /// Parses a string to a <see cref="NiklasKarl.Json.JsonValue"/>.
        /// </summary>
        /// <param name="data">The reader from which to read the data.</param>
        /// <returns>The resulting <see cref="NiklasKarl.Json.JsonValue"/>.</returns>
        internal static JsonValue ParseValue(TextReader reader)
        {
            JsonValue value = null;
            using (JsonParser parser = new JsonParser(reader))
            {
                value = parser.ParseValue();
                parser.EnsureEndOfData();
            }

            return value;
        }

        /// <summary>
        /// Parses a string to a <see cref="NiklasKarl.Json.JsonObject"/>.
        /// </summary>
        /// <param name="data">The reader from which to read the data.</param>
        /// <returns>The resulting <see cref="NiklasKarl.Json.JsonObject"/>.</returns>
        internal static JsonObject ParseObject(TextReader reader)
        {
            JsonObject obj = null;
            using (JsonParser parser = new JsonParser(reader))
            {
                obj = parser.ParseObject();
                parser.EnsureEndOfData();
            }

            return obj;
        }

        /// <summary>
        /// Parses a string to a <see cref="NiklasKarl.Json.JsonArray"/>.
        /// </summary>
        /// <param name="reader">The reader from which to read the data.</param>
        /// <returns>The resulting <see cref="NiklasKarl.Json.JsonArray"/>.</returns>
        internal static JsonArray ParseArray(TextReader reader)
        {
            JsonArray array = null;
            using (JsonParser parser = new JsonParser(reader))
            {
                array = parser.ParseArray();
                parser.EnsureEndOfData();
            }

            return array;
        }

        /// <summary>
        /// Parses a string to a <see cref="NiklasKarl.Json.JsonString"/>.
        /// </summary>
        /// <param name="data">The reader from which to read the data.</param>
        /// <returns>The resulting <see cref="NiklasKarl.Json.JsonString"/>.</returns>
        internal static JsonString ParseString(TextReader reader)
        {
            JsonString str = null;
            using (JsonParser parser = new JsonParser(reader))
            {
                str = new JsonString(parser.ParseString());
                parser.EnsureEndOfData();
            }

            return str;
        }

        /// <summary>
        /// Parses a string to a <see cref="NiklasKarl.Json.JsonNumber"/>.
        /// </summary>
        /// <param name="data">The reader from which to read the data.</param>
        /// <returns>The resulting <see cref="NiklasKarl.Json.JsonNumber"/>.</returns>
        internal static JsonNumber ParseNumber(TextReader reader)
        {
            JsonNumber number = null;
            using (JsonParser parser = new JsonParser(reader))
            {
                number = new JsonNumber(parser.ParseNumber());
                parser.EnsureEndOfData();
            }

            return number;
        }

        /// <summary>
        /// Parses a <see cref="NiklasKarl.Json.JsonValue"/> at the current position in the input string.
        /// After the method completes, position points to the last character of the parsed <see cref="NiklasKarl.Json.JsonValue"/>.
        /// </summary>
        /// <returns>The parsed <see cref="NiklasKarl.Json.JsonValue"/>.</returns>
        private JsonValue ParseValue()
        {
            int state = 0;
            int index = 1;
            for (char c = this.GetNextCharacter(); c != '\0'; c = this.GetNextCharacter())
            {
                switch (state)
                {
                    case 0:
                        if (!this.IsWhitespace(c))
                        {
                            if (c == '{')
                            {
                                this.bufferPosition--;
                                return this.ParseObject();
                            }
                            else if (c == '[')
                            {
                                this.bufferPosition--;
                                return this.ParseArray();
                            }
                            else if (c == '"')
                            {
                                this.bufferPosition--;
                                return new JsonString(this.ParseString());
                            }
                            else if ((c >= '0' && c <= '9') || c == '-')
                            {
                                this.bufferPosition--;
                                return new JsonNumber(this.ParseNumber());
                            }
                            else if (c == 'f')
                            {
                                state = 1;
                            }
                            else if (c == 't')
                            {
                                state = 2;
                            }
                            else if (c == 'n')
                            {
                                state = 3;
                            }
                            else
                            {
                                throw this.CreateException("Expected a json value but found an invalid token.");
                            }
                        }

                        break;
                    case 1:
                        if (c != JsonParser.FalseString[index])
                        {
                            throw this.CreateException("Expected a json value but found an invalid token.");
                        }

                        if ((++index) == JsonParser.FalseString.Length)
                        {
                            return JsonValue.False;
                        }

                        break;
                    case 2:
                        if (c != JsonParser.TrueString[index])
                        {
                            throw this.CreateException("Expected a json value but found an invalid token.");
                        }

                        if ((++index) == JsonParser.TrueString.Length)
                        {
                            return JsonValue.True;
                        }

                        break;
                    case 3:
                        if (c != JsonParser.NullString[index])
                        {
                            throw this.CreateException("Expected a json value but found an invalid token.");
                        }

                        if ((++index) == JsonParser.NullString.Length)
                        {
                            return JsonValue.Null;
                        }

                        break;
                }
            }

            throw this.CreateException("Expected a json value but found end of data.");
        }

        /// <summary>
        /// Parses a <see cref="NiklasKarl.Json.JsonObject"/> at the current position in the input string.
        /// After the method completes, position points to the last character of the parsed <see cref="NiklasKarl.Json.JsonObject"/>.
        /// </summary>
        /// <returns>The parsed <see cref="NiklasKarl.Json.JsonObject"/>.</returns>
        private JsonObject ParseObject()
        {
            JsonObject obj = new JsonObject();

            string name = null;

            int state = 0;
            for (char c = this.GetNextCharacter(); c != '\0'; c = this.GetNextCharacter())
            {
                switch (state)
                {
                    case 0:
                        if (!this.IsWhitespace(c))
                        {
                            if (c == '{')
                            {
                                state = 1;
                            }
                            else
                            {
                                throw this.CreateException("Expected opening curly bracket ('{{') but found '{0}'.", c);
                            }
                        }

                        break;
                    case 1:
                        if (!this.IsWhitespace(c))
                        {
                            if (c == '"')
                            {
                                this.bufferPosition--;
                                name = this.ParseString();
                                state = 2;
                            }
                            else if (c == '}')
                            {
                                return obj;
                            }
                            else
                            {
                                throw this.CreateException("Expected string indicator ('\"') or closing curly bracket ('}}') but found '{0}'.", c);
                            }
                        }

                        break;
                    case 2:
                        if (!this.IsWhitespace(c))
                        {
                            if (c == ':')
                            {
                                state = 3;
                            }
                            else
                            {
                                throw this.CreateException("Expected name/value seperator (':') but found '{0}'.", c);
                            }
                        }

                        break;
                    case 3:
                        this.bufferPosition--;
                        obj.Add(name, this.ParseValue());
                        state = 4;
                        break;
                    case 4:
                        if (!this.IsWhitespace(c))
                        {
                            if (c == ',')
                            {
                                state = 1;
                            }
                            else if (c == '}')
                            {
                                return obj;
                            }
                            else
                            {
                                throw this.CreateException("Expected name/value pair delimiter (',') or closing curly bracket ('}}') but found '{0}'.", c);
                            }
                        }

                        break;
                }
            }

            throw this.CreateException("Expected closing curly bracket ('}}') but found end of data.");
        }

        /// <summary>
        /// Parses a <see cref="NiklasKarl.Json.JsonArray"/> at the current position in the input string.
        /// After the method completes, position points to the last character of the parsed <see cref="NiklasKarl.Json.JsonArray"/>.
        /// </summary>
        /// <returns>The parsed <see cref="NiklasKarl.Json.JsonArray"/>.</returns>
        private JsonArray ParseArray()
        {
            JsonArray array = new JsonArray();

            int state = 0;
            for (char c = this.GetNextCharacter(); c != '\0'; c = this.GetNextCharacter())
            {
                switch (state)
                {
                    case 0:
                        if (!this.IsWhitespace(c))
                        {
                            if (c == '[')
                            {
                                state = 1;
                            }
                            else
                            {
                                throw this.CreateException("Expected opening hard bracket ('[') but found '{0}'.", c);
                            }
                        }

                        break;
                    case 1:
                        if (!this.IsWhitespace(c))
                        {
                            if (c == ']')
                            {
                                return array;
                            }
                            else
                            {
                                this.bufferPosition--;
                                array.Add(this.ParseValue());
                                state = 2;
                            }
                        }

                        break;
                    case 2:
                        if (!this.IsWhitespace(c))
                        {
                            if (c == ',')
                            {
                                state = 1;
                            }
                            else if (c == ']')
                            {
                                return array;
                            }
                            else
                            {
                                throw this.CreateException("Expected value delimiter (',') or closing hard bracket (']') but found '{0}'.", c);
                            }
                        }

                        break;
                }
            }

            throw this.CreateException("Expected closing hard bracket (']') but found end of data.");
        }

        /// <summary>
        /// Parses a <see cref="NiklasKarl.Json.JsonString"/> at the current position in the input string.
        /// After the method completes, position points to the last character of the parsed <see cref="NiklasKarl.Json.JsonString"/>.
        /// </summary>
        /// <returns>The unescaped string.</returns>
        private string ParseString()
        {
            this.builder.Clear();

            ushort unicode = 0;

            int index = 0;
            int state = 0;
            for (char c = this.GetNextCharacter(); c != '\0'; c = this.GetNextCharacter())
            {
                switch (state)
                {
                    case 0:
                        if (!this.IsWhitespace(c))
                        {
                            if (c == '"')
                            {
                                state = 1;
                            }
                            else
                            {
                                throw this.CreateException("Expected string indicator ('\"') but found '{0}'.", c);
                            }
                        }

                        break;
                    case 1:
                        if (c == '"')
                        {
                            return this.builder.ToString();
                        }
                        else if (c == '\\')
                        {
                            state = 2;
                        }
                        else
                        {
                            this.builder.Append(c);
                        }

                        break;
                    case 2:
                        if (c == '"' || c == '\\' || c == '/')
                        {
                            this.builder.Append(c);
                            state = 1;
                        }
                        else if (c == 'b')
                        {
                            this.builder.Append('\b');
                            state = 1;
                        }
                        else if (c == 'f')
                        {
                            this.builder.Append('\f');
                            state = 1;
                        }
                        else if (c == 'n')
                        {
                            this.builder.Append('\n');
                            state = 1;
                        }
                        else if (c == 'r')
                        {
                            this.builder.Append('\r');
                            state = 1;
                        }
                        else if (c == 't')
                        {
                            this.builder.Append('\t');
                            state = 1;
                        }
                        else if (c == 'u')
                        {
                            unicode = 0;
                            state = 3;
                        }
                        else
                        {
                            throw this.CreateException("Expected escape character but found '{0}'.", c);
                        }

                        break;
                    case 3:
                        if (c >= '0' && c <= '9')
                        {
                            unicode |= (ushort)((c - '0') << (4 * (3 - index)));
                        }
                        else if (c >= 'A' && c <= 'F')
                        {
                            unicode |= (ushort)((c - 'A' + 10) << 12);
                        }
                        else if (c >= 'a' && c <= 'f')
                        {
                            unicode |= (ushort)((c - 'a' + 10) << 12);
                        }
                        else
                        {
                            throw this.CreateException("Expected hexadecimal digit but found '{0}'.", c);
                        }

                        if ((++index) == 4)
                        {
                            this.builder.Append((char)unicode);
                            index = 0;
                            state = 1;
                        }

                        break;
                }
            }

            throw this.CreateException("Expected string terminator ('\"') but found end of data.");
        }

        /// <summary>
        /// Parses a <see cref="NiklasKarl.Json.JsonNumber"/> at the current position in the input string.
        /// After the method completes, position points to the last character of the parsed <see cref="NiklasKarl.Json.JsonNumber"/>.
        /// </summary>
        /// <returns>The parsed number.</returns>
        private double ParseNumber()
        {
            this.builder.Clear();

            int state = 0;
            for (char c = this.GetNextCharacter(); c != '\0'; c = this.GetNextCharacter())
            {
                switch (state)
                {
                    case 0:
                        if (!this.IsWhitespace(c))
                        {
                            if (c == '-')
                            {
                                state = 1;
                            }
                            else if (c >= '1' && c <= '9')
                            {
                                state = 2;
                            }
                            else if (c == '0')
                            {
                                state = 3;
                            }
                            else
                            {
                                throw this.CreateException("Expected minus sign ('-') or digit ('0' to '9') but found '{0}'.", c);
                            }
                        }

                        break;
                    case 1:
                        if (c >= '1' && c <= '9')
                        {
                            state = 2;
                        }
                        else if (c == '0')
                        {
                            state = 3;
                        }
                        else
                        {
                            throw this.CreateException("Expected digit ('0' to '9') but found '{0}'.", c);
                        }

                        break;
                    case 2:
                        if (c >= '0' && c <= '9')
                        {
                        }
                        else if (c == '.')
                        {
                            state = 4;
                        }
                        else if (c == 'e' || c == 'E')
                        {
                            state = 6;
                        }
                        else
                        {
                            this.bufferPosition--;

                            try
                            {
                                return double.Parse(this.builder.ToString(), this.numberStyle, this.numberFormat);
                            }
                            catch (OverflowException)
                            {
                                throw this.CreateException("The number is too large to be stored.");
                            }
                        }

                        break;
                    case 3:
                        if (c == '.')
                        {
                            state = 4;
                        }
                        else if (c == 'e' || c == 'E')
                        {
                            state = 6;
                        }
                        else
                        {
                            this.bufferPosition--;

                            try
                            {
                                return double.Parse(this.builder.ToString(), this.numberStyle, this.numberFormat);
                            }
                            catch (OverflowException)
                            {
                                throw this.CreateException("The number is too large to be stored.");
                            }
                        }

                        break;
                    case 4:
                        if (c >= '0' && c <= '9')
                        {
                            state = 5;
                        }
                        else
                        {
                            throw this.CreateException("Expected digit ('0' to '9') but found {0}.", c);
                        }

                        break;
                    case 5:
                        if (c >= '0' && c <= '9')
                        {
                        }
                        else if (c == 'e' || c == 'E')
                        {
                            state = 6;
                        }
                        else
                        {
                            this.bufferPosition--;

                            try
                            {
                                return double.Parse(this.builder.ToString(), this.numberStyle, this.numberFormat);
                            }
                            catch (OverflowException)
                            {
                                throw this.CreateException("The number is too large to be stored.");
                            }
                        }

                        break;
                    case 6:
                        if (c == '-' || c == '+')
                        {
                            state = 7;
                        }
                        else if (c >= '0' && c <= '9')
                        {
                            state = 8;
                        }
                        else
                        {
                            throw this.CreateException("Expected sign ('-' or '+') or digit ('0' to '9') but found {0}.", c);
                        }

                        break;
                    case 7:
                        if (c >= '0' && c <= '9')
                        {
                            state = 8;
                        }
                        else
                        {
                            throw this.CreateException("Expected digit ('0' to '9') but found {0}.", c);
                        }

                        break;
                    case 8:
                        if (c >= '0' && c <= '9')
                        {
                        }
                        else
                        {
                            this.bufferPosition--;

                            try
                            {
                                return double.Parse(this.builder.ToString(), this.numberStyle, this.numberFormat);
                            }
                            catch (OverflowException)
                            {
                                throw this.CreateException("The number is too large to be stored.");
                            }
                        }

                        break;
                }

                this.builder.Append(c);
            }

            switch (state)
            {
                case 0:
                    throw this.CreateException("Expected minus sign ('-') or digit ('0' to '9') but found end of data.");
                case 1:
                    throw this.CreateException("Expected digit ('0' to '9') but found end of data.");
                case 4:
                    throw this.CreateException("Expected digit ('0' to '9') but found end of data.");
                case 6:
                    throw this.CreateException("Expected sign ('-' or '+') or digit ('0' to '9') but found end of data.");
                case 7:
                    throw this.CreateException("Expected digit ('0' to '9') but found end of data.");
                default:
                    this.bufferPosition--;

                    try
                    {
                        return double.Parse(this.builder.ToString(), this.numberStyle, this.numberFormat);
                    }
                    catch (OverflowException)
                    {
                        throw this.CreateException("The number is too large to be stored.");
                    }
            }
        }

        private char GetNextCharacter()
        {
            if (this.bufferPosition >= this.bufferLength)
            {
                this.bufferStart += this.bufferLength;
                this.bufferLength = this.reader.Read(this.buffer, 0, this.buffer.Length);
                this.bufferPosition = 0;
            }

            char c = this.bufferPosition < this.bufferLength ? this.buffer[this.bufferPosition++] : '\0';
            if (c == '\n' || c == '\r')
            {
                this.line++;
                this.lineStart = this.bufferStart + this.bufferPosition + 1;
            }

            return c;
        }

        /// <summary>
        /// Ensures that all characters following the current position of the <see cref="NiklasKarl.Json.JsonParser"/>are whitespace.
        /// Otherwise a <see cref="NiklasKarl.Json.JsonParseException"/> is thrown.
        /// </summary>
        private void EnsureEndOfData()
        {
            for (char c = this.GetNextCharacter(); c != '\0'; c = this.GetNextCharacter())
            {
                if (!this.IsWhitespace(c))
                {
                    throw this.CreateException("Expected end of data but found '{0}'.", c);
                }
            }
        }

        private bool IsWhitespace(char c)
        {
            return c == '\n' || c == '\r' || c == ' ' || c == '\t';
        }

        /// <summary>
        /// Constructs a <see cref="NiklasKarl.Json.JsonParseException"/> with a specified error message and automatically adds the line number and the character position.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <returns>The constructed <see cref="NiklasKarl.Json.JsonParseException"/>.</returns>
        private JsonParseException CreateException(string message)
        {
            return new JsonParseException(this.line, this.bufferStart + this.bufferPosition - this.lineStart, message);
        }

        /// <summary>
        /// Constructs a <see cref="NiklasKarl.Json.JsonParseException"/> with a specified error message format and an invalid character and automatically adds the line number and the character position.
        /// </summary>
        /// <param name="format">The error message format string.</param>
        /// <param name="c">The invalid character found in the input string.</param>
        /// <returns>The constructed <see cref="NiklasKarl.Json.JsonParseException"/>.</returns>
        private JsonParseException CreateException(string format, char c)
        {
            return new JsonParseException(this.line, this.bufferStart + this.bufferPosition - this.lineStart, string.Format(format, c));
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.reader != null)
                    {
                        this.reader.Dispose();
                    }
                }

                this.buffer = null;
                this.builder = null;

                disposedValue = true;
            }
        }
        
        ~JsonParser()
        {
            Dispose(false);
        }
        
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
