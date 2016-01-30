//-----------------------------------------------------------------------
// <copyright file="JsonObject.cs" company="Niklas Karl">
//     Copyright © Niklas Karl 2015
// </copyright>
//-----------------------------------------------------------------------

namespace NiklasKarl.Json
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Represents a <see cref="NiklasKarl.Json.JsonValue"/> with <see cref="NiklasKarl.Json.JsonValueType"/> of <c>Object</c>.
    /// </summary>
    public class JsonObject : JsonValue, IDictionary<string, JsonValue>, IReadOnlyDictionary<string, JsonValue>, IDictionary,
        ICollection<KeyValuePair<string, JsonValue>>, IReadOnlyCollection<KeyValuePair<string, JsonValue>>, ICollection,
        IEnumerable<KeyValuePair<string, JsonValue>>, IEnumerable
    {
        /// <summary>
        /// The values of the <see cref="NiklasKarl.Json.JsonObject"/>.
        /// </summary>
        private SortedDictionary<string, JsonValue> values;

        /// <summary>
        /// Initializes a new instance of the <see cref="NiklasKarl.Json.JsonObject"/> class.
        /// </summary>
        public JsonObject()
        {
            this.values = new SortedDictionary<string, JsonValue>();
        }

        /// <summary>
        /// Gets the <see cref="NiklasKarl.Json.JsonValueType"/> of the current instance.
        /// This is always <c>Object</c> for instances of type <see cref="NiklasKarl.Json.JsonObject"/>.
        /// </summary>
        public override JsonValueType ValueType
        {
            get { return JsonValueType.Object; }
        }

        /// <summary>
        /// Gets the number of key/value pairs contained in the <see cref="NiklasKarl.Json.JsonObject"/>.
        /// </summary>
        public int Count
        {
            get { return this.values.Count; }
        }

        /// <summary>
        /// Gets a collection containing the keys in the <see cref="NiklasKarl.Json.JsonObject"/>.
        /// </summary>
        public ICollection<string> Keys
        {
            get { return this.values.Keys; }
        }

        /// <summary>
        /// Gets a collection containing the values in the <see cref="NiklasKarl.Json.JsonObject"/>.
        /// </summary>
        public ICollection<JsonValue> Values
        {
            get { return this.values.Values; }
        }

        /// <summary>
        /// Gets a collection containing the keys of the <see cref="System.Collections.Generic.IReadOnlyDictionary{string, NiklasKarl.Json.JsonValue}"/>.
        /// </summary>
        IEnumerable<string> IReadOnlyDictionary<string, JsonValue>.Keys
        {
            get { return this.values.Keys; }
        }

        /// <summary>
        /// Gets a collection containing the values of the <see cref="System.Collections.Generic.IReadOnlyDictionary{string, NiklasKarl.Json.JsonValue}"/>.
        /// </summary>
        IEnumerable<JsonValue> IReadOnlyDictionary<string, JsonValue>.Values
        {
            get { return this.values.Values; }
        }

        /// <summary>
        /// Gets a value indicating whether the dictionary is read-only.
        /// </summary>
        bool ICollection<KeyValuePair<string, JsonValue>>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets an <see cref="System.Collections.ICollection"/> containing the keys of the <see cref="System.Collections.IDictionary"/>.
        /// </summary>
        ICollection IDictionary.Keys
        {
            get { return this.values.Keys; }
        }

        /// <summary>
        /// Gets an <see cref="System.Collections.ICollection"/> containing the values in the <see cref="System.Collections.IDictionary"/>.
        /// </summary>
        ICollection IDictionary.Values
        {
            get { return this.values.Values; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="System.Collections.IDictionary"/> is read-only.
        /// </summary>
        bool IDictionary.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="System.Collections.IDictionary"/> has a fixed size.
        /// </summary>
        bool IDictionary.IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="System.Collections.ICollection"/> is synchronized (thread safe).
        /// </summary>
        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="System.Collections.ICollection"/>.
        /// </summary>
        object ICollection.SyncRoot
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>
        /// The value associated with the specified key.
        /// If the specified key is not found, a get operation throws a <see cref="System.Collections.Generic.KeyNotFoundException"/>,
        /// and a set operation creates a new element with the specified key.
        /// </returns>
        public JsonValue this[string key]
        {
            get
            {
                JsonValue value;
                if (!this.values.TryGetValue(key, out value))
                {
                    throw new KeyNotFoundException("No value with the specified key exists.");
                }

                return value;
            }

            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException("The key must not be null.", "key");
                }

                if (value == null)
                {
                    throw new ArgumentNullException("The value must not be null.", "value");
                }

                this.values[key] = value;
            }
        }

        /// <summary>
        /// Gets or sets the value with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key, or <c>null</c> if <paramref name="key"/> is not in the dictionary or <paramref name="key"/> is of a type that is not assignable to the key type <see cref="NiklasKarl.Json.JsonValue"/> of the <see cref="NiklasKarl.Json.JsonObject"/>.</returns>
        object IDictionary.this[object key]
        {
            get
            {
                if (key != null && !(key is string))
                {
                    return null;
                }

                return (this.values as IDictionary)[key];
            }

            set
            {
                if (key != null && !(key is string))
                {
                    throw new ArgumentException("The key must be of type string.", "key");
                }

                if (value != null && !(value is JsonValue))
                {
                    throw new ArgumentException("The value must be of type JsonValue.", "value");
                }

                this[key as string] = value as JsonValue;
            }
        }

        public bool TryGetValue(string key, out JsonValue value)
        {
            return this.TryGetValue(key, out value);
        }

        public bool ContainsKey(string key)
        {
            return this.values.ContainsKey(key);
        }

        public bool Contains(KeyValuePair<string, JsonValue> item)
        {
            return this.Contains(item);
        }

        public void Add(KeyValuePair<string, JsonValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void Add(string key, JsonValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("The key must not be null.", "key");
            }

            if (value == null)
            {
                throw new ArgumentNullException("The value must not be null.", "value");
            }

            this.values.Add(key, value);
        }

        public bool Remove(KeyValuePair<string, JsonValue> item)
        {
            return (this.values as IDictionary<string, JsonValue>).Remove(item);
        }

        public bool Remove(string key)
        {
            return this.values.Remove(key);
        }

        public void Clear()
        {
            this.values.Clear();
        }

        public void CopyTo(KeyValuePair<string, JsonValue>[] array, int arrayIndex)
        {
            this.values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, JsonValue>> GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        bool IDictionary.Contains(object key)
        {
            if (key != null && !(key is string))
            {
                throw new ArgumentException("The key must be of type string.", "key");
            }

            return this.ContainsKey(key as string);
        }

        void IDictionary.Add(object key, object value)
        {
            if (key != null && !(key is string))
            {
                throw new ArgumentException("The key must be of type string.", "key");
            }

            if (value != null && !(value is JsonValue))
            {
                throw new ArgumentException("The value must be of type JsonValue.", "value");
            }

            this.Add(key as string, value as JsonValue);
        }

        void IDictionary.Remove(object key)
        {
            if (key != null && !(key is string))
            {
                throw new ArgumentException("The key must be of type string.", "key");
            }

            this.Remove(key as string);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (!(array is KeyValuePair<string, JsonValue>[]))
            {
                throw new ArgumentException("The array must be of type array of KeyValuePair<string, JsonValue>.", "array");
            }

            this.CopyTo(array as KeyValuePair<string, JsonValue>[], index);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        internal override void ToString(StringBuilder builder, int tabs)
        {
            if (this.values.Count == 0)
            {
                builder.Append("{}");
            }
            else if (tabs < 0)
            {
                builder.Append('{');

                bool first = true;
                foreach (KeyValuePair<string, JsonValue> value in this.values)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        builder.Append(',');
                    }

                    JsonString.ToString(value.Key, builder);
                    builder.Append(':');
                    value.Value.ToString(builder, -1);
                }

                builder.Append('}');
            }
            else
            {
                builder.Append('{');
                builder.AppendLine();

                bool first = true;
                foreach (KeyValuePair<string, JsonValue> value in this.values)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        builder.Append(',');
                        builder.AppendLine();
                    }

                    builder.Append('\t', tabs + 1);
                    JsonString.ToString(value.Key, builder);
                    builder.Append(" : ");
                    value.Value.ToString(builder, tabs + 1);
                }

                builder.AppendLine();

                builder.Append('\t', tabs);
                builder.Append('}');
            }
        }
    }
}
