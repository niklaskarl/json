//-----------------------------------------------------------------------
// <copyright file="JsonValueType.cs" company="Niklas Karl">
//     Copyright © Niklas Karl 2015
// </copyright>
//-----------------------------------------------------------------------

namespace NiklasKarl.Json
{
    using System;

    /// <summary>
    /// The type of a <see cref="NiklasKarl.Json.JsonValue"/>.
    /// </summary>
    public enum JsonValueType
    {
        /// <summary>
        /// Indicates that a <see cref="NiklasKarl.Json.JsonValue"/> is of type <see cref="NiklasKarl.Json.JsonString"/>.
        /// </summary>
        String,

        /// <summary>
        /// Indicates that a <see cref="NiklasKarl.Json.JsonValue"/> is of type <see cref="NiklasKarl.Json.JsonNumber"/>.
        /// </summary>
        Number,

        /// <summary>
        /// Indicates that a <see cref="NiklasKarl.Json.JsonValue"/> is of type <see cref="NiklasKarl.Json.JsonObject"/>.
        /// </summary>
        Object,

        /// <summary>
        /// Indicates that a <see cref="NiklasKarl.Json.JsonValue"/> is of type <see cref="NiklasKarl.Json.JsonArray"/>.
        /// </summary>
        Array,

        /// <summary>
        /// Indicates that a <see cref="NiklasKarl.Json.JsonValue"/> is of type <see cref="NiklasKarl.Json.JsonBoolean"/>.
        /// </summary>
        Boolean,

        /// <summary>
        /// Indicates that a <see cref="NiklasKarl.Json.JsonValue"/> is of type <see cref="NiklasKarl.Json.JsonNull"/>.
        /// </summary>
        Null
    }
}
