//-----------------------------------------------------------------------
// <copyright file="JsonNumber.cs" company="Niklas Karl">
//     Copyright © Niklas Karl 2015
// </copyright>
//-----------------------------------------------------------------------

namespace NiklasKarl.Json
{
    using System;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Represents a <see cref="NiklasKarl.Json.JsonValue"/> with <see cref="NiklasKarl.Json.JsonValueType"/> of <c>Number</c>.
    /// </summary>
    public class JsonNumber : JsonValue
    {
        /// <summary>
        /// The value of the <see cref="NiklasKarl.Json.JsonNumber"/>.
        /// </summary>
        private double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="NiklasKarl.Json.JsonNumber"/> class with a specified value.
        /// </summary>
        /// <param name="value">The value of the <see cref="NiklasKarl.Json.JsonNumber"/>.</param>
        public JsonNumber(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets the <see cref="NiklasKarl.Json.JsonValueType"/> of the current instance.
        /// This is always <c>Number</c> for instances of type <see cref="NiklasKarl.Json.JsonNumber"/>.
        /// </summary>
        public override JsonValueType ValueType
        {
            get { return JsonValueType.Number; }
        }

        /// <summary>
        /// Gets the value of the <see cref="NiklasKarl.Json.JsonNumber"/>.
        /// </summary>
        public double Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Writes the string representation of the <see cref="NiklasKarl.Json.JsonNumber"/> to the <see cref="System.Text.StringBuilder"/> using the specified indent.
        /// The first line is not affected by the indent.
        /// </summary>
        /// <param name="destination">The <see cref="System.Text.StringBuilder"/> to write to.</param>
        /// <param name="indent">
        /// The indent to use when writing the string.
        /// As the first line is not indented and <see cref="NiklasKarl.Json.JsonNumber"/> always results in a single line, this has no effect.
        /// </param>
        internal override void ToString(StringBuilder destination, int indent)
        {
            destination.Append(this.value.ToString(NumberFormatInfo.InvariantInfo));
        }
    }
}
