//-----------------------------------------------------------------------
// <copyright file="JsonString.cs" company="Niklas Karl">
//     Copyright © Niklas Karl 2016
// </copyright>
//-----------------------------------------------------------------------

namespace NiklasKarl.Json
{
    using System;
    using System.Text;

    /// <summary>
    /// Represents a <see cref="NiklasKarl.Json.JsonValue"/> with <see cref="NiklasKarl.Json.JsonValueType"/> of <c>String</c>.
    /// </summary>
    public class JsonString : JsonValue
    {
        /// <summary>
        /// The characters to escape in a <see cref="NiklasKarl.Json.JsonString"/>.
        /// </summary>
        private static char[] escapeCharacter = { '"', '\\', '/', '\b', '\f', '\n', '\r', '\t' };

        /// <summary>
        /// The value of the <see cref="NiklasKarl.Json.JsonString"/>.
        /// </summary>
        private string value;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonString"/> class with a specified value.
        /// </summary>
        /// <param name="value">The value of the <see cref="JsonString"/>.</param>
        public JsonString(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", "The 'value' argument must not be null.");
            }

            this.value = value;
        }

        /// <summary>
        /// Gets the <see cref="NiklasKarl.Json.JsonValueType"/> of the current instance.
        /// This is always <c>String</c> for instances of type <see cref="NiklasKarl.Json.JsonString"/>.
        /// </summary>
        public override JsonValueType ValueType
        {
            get { return JsonValueType.String; }
        }

        /// <summary>
        /// Gets the value of the <see cref="NiklasKarl.Json.JsonString"/>.
        /// </summary>
        public string Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Writes the escaped string to the <see cref="System.Text.StringBuilder"/>.
        /// </summary>
        /// <param name="value">The string to write.</param>
        /// <param name="destination">The <see cref="System.Text.StringBuilder"/> to write to.</param>
        internal static void ToString(string value, StringBuilder destination)
        {
            int start = 0;
            int index = value.IndexOfAny(JsonString.escapeCharacter);

            destination.Append('"');

            while (index >= 0)
            {
                destination.Append(value, start, index - start);
                switch (value[index])
                {
                    case '"':
                        destination.Append("\\\"");
                        break;
                    case '\\':
                        destination.Append("\\\\");
                        break;
                    case '/':
                        destination.Append("\\/");
                        break;
                    case '\b':
                        destination.Append("\\b");
                        break;
                    case '\f':
                        destination.Append("\\f");
                        break;
                    case '\n':
                        destination.Append("\\n");
                        break;
                    case '\r':
                        destination.Append("\\r");
                        break;
                    case '\t':
                        destination.Append("\\t");
                        break;
                }

                start = index + 1;
                index = value.IndexOfAny(JsonString.escapeCharacter, start);
            }

            destination.Append(value, start, value.Length - start);
            destination.Append('"');
        }

        /// <summary>
        /// Writes the string representation of the <see cref="NiklasKarl.Json.JsonString"/> to the <see cref="System.Text.StringBuilder"/> using the specified indent.
        /// The first line is not affected by the indent.
        /// </summary>
        /// <param name="destination">The <see cref="System.Text.StringBuilder"/> to write to.</param>
        /// <param name="indent">
        /// The indent to use when writing the string.
        /// As the first line is not indented and <see cref="NiklasKarl.Json.JsonString"/> always results in a single line, this has no effect.
        /// </param>
        internal override void ToString(StringBuilder destination, int indent)
        {
            JsonString.ToString(this.value, destination);
        }
    }
}
