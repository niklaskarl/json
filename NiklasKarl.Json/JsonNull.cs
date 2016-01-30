//-----------------------------------------------------------------------
// <copyright file="JsonNull.cs" company="Niklas Karl">
//     Copyright © Niklas Karl 2016
// </copyright>
//-----------------------------------------------------------------------

namespace NiklasKarl.Json
{
    using System;
    using System.Text;

    /// <summary>
    /// Represents a <see cref="NiklasKarl.Json.JsonValue"/> with <see cref="NiklasKarl.Json.JsonValueType"/> of <c>Null</c>
    /// </summary>
    public class JsonNull : JsonValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NiklasKarl.Json.JsonNull"/> class.
        /// </summary>
        internal JsonNull()
        {
        }

        /// <summary>
        /// Gets the <see cref="NiklasKarl.Json.JsonValueType"/> of the current instance.
        /// This is always <c>Null</c> for instances of type <see cref="NiklasKarl.Json.JsonNull"/>.
        /// </summary>
        public override JsonValueType ValueType
        {
            get { return JsonValueType.Null; }
        }

        /// <summary>
        /// Writes the string representation of the <see cref="NiklasKarl.Json.JsonNull"/> to the <see cref="System.Text.StringBuilder"/> using the specified indent.
        /// The first line is not affected by the indent.
        /// </summary>
        /// <param name="destination">The <see cref="System.Text.StringBuilder"/> to write to.</param>
        /// <param name="indent">
        /// The indent to use when writing the string.
        /// As the first line is not indented and <see cref="NiklasKarl.Json.JsonNull"/> always results in a single line, this has no effect.
        /// </param>
        internal override void ToString(StringBuilder destination, int indent)
        {
            destination.Append("null");
        }
    }
}
