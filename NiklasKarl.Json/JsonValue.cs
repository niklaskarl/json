//-----------------------------------------------------------------------
// <copyright file="JsonValue.cs" company="Niklas Karl">
//     Copyright © Niklas Karl 2015
// </copyright>
//-----------------------------------------------------------------------

namespace NiklasKarl.Json
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// The abstract base type for any <see cref="NiklasKarl.Json.JsonValue"/>.
    /// </summary>
    public abstract class JsonValue
    {
        /// <summary>
        /// The <see cref="NiklasKarl.Json.JsonNull"/> singleton.
        /// </summary>
        private static JsonNull nullSingleton = new JsonNull();

        /// <summary>
        /// The <see cref="NiklasKarl.Json.JsonBoolean"/> singleton with value <c>false</c>.
        /// </summary>
        private static JsonBoolean falseSingleton = new JsonBoolean(false);

        /// <summary>
        /// The <see cref="NiklasKarl.Json.JsonBoolean"/> singleton with value <c>true</c>.
        /// </summary>
        private static JsonBoolean trueSingleton = new JsonBoolean(true);

        /// <summary>
        /// Gets the <see cref="NiklasKarl.Json.JsonNull"/> singleton.
        /// </summary>
        public static JsonNull Null
        {
            get { return JsonValue.nullSingleton; }
        }

        /// <summary>
        /// Gets the <see cref="NiklasKarl.Json.JsonBoolean"/> singleton with value <c>false</c>.
        /// </summary>
        public static JsonBoolean False
        {
            get { return JsonValue.falseSingleton; }
        }

        /// <summary>
        /// Gets the <see cref="NiklasKarl.Json.JsonBoolean"/> singleton with value <c>true</c>.
        /// </summary>
        public static JsonBoolean True
        {
            get { return JsonValue.trueSingleton; }
        }

        /// <summary>
        /// Gets the <see cref="NiklasKarl.Json.JsonValueType"/> of the current instance.
        /// </summary>
        public abstract JsonValueType ValueType { get; }

        /// <summary>
        /// Parses a JSON string to its corresponding <see cref="NiklasKarl.Json.JsonValue"/>.
        /// </summary>
        /// <param name="json">The reader from which to read the data.</param>
        /// <returns>The resulting <see cref="NiklasKarl.Json.JsonValue"/>.</returns>
        public static JsonValue Parse(TextReader reader)
        {
            return JsonParser.ParseValue(reader);
        }

        /// <summary>
        /// Converts the <see cref="NiklasKarl.Json.JsonValue"/> to its string representation. It is formatted to be human-readable.
        /// </summary>
        /// <returns>The string representation of the <see cref="NiklasKarl.Json.JsonValue"/>.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            this.ToString(builder, 0);

            return builder.ToString();
        }

        /// <summary>
        /// Converts the <see cref="NiklasKarl.Json.JsonValue"/> to its string representation.
        /// </summary>
        /// <param name="compact">A value indicating whether to make the string as short as possible or to make the result human-readable.</param>
        /// <returns>The string representation of the <see cref="NiklasKarl.Json.JsonValue"/>.</returns>
        public string ToString(bool compact)
        {
            StringBuilder builder = new StringBuilder();
            this.ToString(builder, compact ? -1 : 0);

            return builder.ToString();
        }

        /// <summary>
        /// Writes the string representation of the <see cref="NiklasKarl.Json.JsonValue"/> to the <see cref="System.Text.StringBuilder"/> using the specified indent.
        /// The first line is not affected by the indent.
        /// </summary>
        /// <param name="destination">The <see cref="System.Text.StringBuilder"/> to write to.</param>
        /// <param name="indent"> The indent to use when writing the string.</param>
        internal abstract void ToString(StringBuilder destination, int indent);
    }
}
