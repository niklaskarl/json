//-----------------------------------------------------------------------
// <copyright file="JsonArray.cs" company="Niklas Karl">
//     Copyright © Niklas Karl 2016
// </copyright>
//-----------------------------------------------------------------------

namespace NiklasKarl.Json
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Represents a <see cref="NiklasKarl.Json.JsonValue"/> with <see cref="NiklasKarl.Json.JsonValueType"/> of <c>Array</c>
    /// </summary>
    public class JsonArray : JsonValue, IList<JsonValue>, IReadOnlyList<JsonValue>, IList,
        ICollection<JsonValue>, IReadOnlyCollection<JsonValue>, ICollection, IEnumerable<JsonValue>, IEnumerable
    {
        /// <summary>
        /// The buffer of the <see cref="NiklasKarl.Json.JsonArray"/>.
        /// </summary>
        private JsonValue[] values;

        /// <summary>
        /// The number of elements in the <see cref="NiklasKarl.Json.JsonArray"/>.
        /// </summary>
        private int count;

        /// <summary>
        /// Initializes a new instance of the <see cref="NiklasKarl.Json.JsonArray"/> class.
        /// </summary>
        public JsonArray()
        {
            this.values = new JsonValue[0];
            this.count = 0;
        }

        /// <summary>
        /// Gets the <see cref="NiklasKarl.Json.JsonValueType"/> of the current instance.
        /// This is always <c>Array</c> for instances of type <see cref="NiklasKarl.Json.JsonArray"/>.
        /// </summary>
        public override JsonValueType ValueType
        {
            get { return JsonValueType.Array; }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="NiklasKarl.Json.JsonArray"/>.
        /// </summary>
        public int Count
        {
            get { return this.count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="NiklasKarl.Json.JsonArray"/> is read-only.
        /// </summary>
        bool ICollection<JsonValue>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="System.Collections.IList"/> is read-only.
        /// </summary>
        bool IList.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="System.Collection.IList"/> has a fixed size.
        /// </summary>
        bool IList.IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="System.Collection.ICollection"/> is synchronized (thread safe).
        /// </summary>
        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="System.Collection.ICollection"/>.
        /// </summary>
        object ICollection.SyncRoot
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is less than <c>0</c>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is equal to or greater than <see cref="Count"/>.</exception>
        public JsonValue this[int index]
        {
            get
            {
                if (index < 0 || index > this.count - 1)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return this.values[index];
            }

            set
            {
                if (index < 0 || index > this.count - 1)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (value == null)
                {
                    throw new ArgumentNullException("The value must not be null. Use JsonValue.Null instead.", "value");
                }

                this.values[index] = value;
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="System.Collection.IList"/>.</exception>
        /// <exception cref="System.ArgumentException">The property is set and <paramref name="value"/> is of a type that is not assignable to the <see cref="System.Collection.IList"/>.</exception>
        object IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                if (value != null && !(value is JsonValue))
                {
                    throw new ArgumentException("The value must be of type JsonValue.", "value");
                }

                this[index] = value as JsonValue;
            }
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="NiklasKarl.Json.JsonArray"/>.
        /// </summary>
        /// <param name="value">The object to locate in the <see cref="NiklasKarl.Json.JsonArray"/>.</param>
        /// <returns><c>true</c> if item is found in the <see cref="NiklasKarl.Json.JsonArray"/>; otherwise, <c>false</c>.</returns>
        public bool Contains(JsonValue value)
        {
            for (int i = 0; i < this.count; i++)
            {
                if (this.values[i] == value)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire <see cref="NiklasKarl.Json.JsonArray"/>.
        /// </summary>
        /// <param name="value">The object to locate in the <see cref="NiklasKarl.Json.JsonArray"/>.</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire <see cref="NiklasKarl.Json.JsonArray"/>, if found; otherwise, <c>–1</c>.</returns>
        public int IndexOf(JsonValue value)
        {
            for (int i = 0; i < this.count; i++)
            {
                if (this.values[i] == value)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Adds an object to the end of the <see cref="NiklasKarl.Json.JsonArray"/>.
        /// </summary>
        /// <param name="value">The object to be added to the end of the <see cref="NiklasKarl.Json.JsonArray"/>.</param>
        public void Add(JsonValue value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("The value must not be null. Use JsonValue.Null instead.", "value");
            }

            if (this.count == this.values.Length)
            {
                this.EnsureCapacity(this.count + 1);
            }

            this.values[this.count] = value;
            this.count++;
        }

        /// <summary>
        /// Inserts an element into the <see cref="NiklasKarl.Json.JsonArray"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="value">The object to insert.</param>
        public void Insert(int index, JsonValue value)
        {
            if (index < 0 || index > this.count)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (value == null)
            {
                throw new ArgumentNullException("The value must not be null. Use JsonValue.Null instead.", "value");
            }

            if (this.count == this.values.Length)
            {
                this.EnsureCapacity(this.count + 1);
            }

            for (int i = this.count; i > index; i--)
            {
                this.values[i] = this.values[i - 1];
            }

            this.values[index] = value;
            this.count++;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="NiklasKarl.Json.JsonArray"/>.
        /// </summary>
        /// <param name="value">The object to remove from the <see cref="NiklasKarl.Json.JsonArray"/>.</param>
        /// <returns><c>true</c> if item is successfully removed; otherwise, <c>false</c>. This method also returns <c>false</c> if item was not found in the <see cref="NiklasKarl.Json.JsonArray"/>.</returns>
        public bool Remove(JsonValue value)
        {
            int index = this.IndexOf(value);
            if (index >= 0)
            {
                this.RemoveAt(index);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="NiklasKarl.Json.JsonArray"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index > this.count - 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            for (int i = index; i < this.count - 1; i++)
            {
                this.values[i] = this.values[i + 1];
            }

            this.values[this.count - 1] = null;
            this.count--;
        }

        /// <summary>
        /// Removes all elements from the <see cref="NiklasKarl.Json.JsonArray"/>.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < this.count; i++)
            {
                this.values[i] = null;
            }

            this.count = 0;
        }

        /// <summary>
        /// Copies the entire <see cref="NiklasKarl.Json.JsonArray"/> to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="NiklasKarl.Json.JsonArray"/>.
        /// The <see cref="System.Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        public void CopyTo(JsonValue[] array, int arrayIndex)
        {
            if (array.Length - arrayIndex > this.count)
            {
                throw new ArgumentException("The destination array is too small.", "array");
            }

            for (int i = 0; i < this.count; i++)
            {
                array[arrayIndex + i] = this.values[i];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="NiklasKarl.Json.JsonArray"/>.
        /// </summary>
        /// <returns>A <see cref="NiklasKarl.Json.JsonArray.Enumerator"/> for the <see cref="NiklasKarl.Json.JsonArray"/>.</returns>
        public IEnumerator<JsonValue> GetEnumerator()
        {
            return new JsonArray.Enumerator(this);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collection.IList"/> contains a specific value.
        /// </summary>
        /// <param name="item">The <see cref="System.Object"/> to locate in the <see cref="System.Collection.IList"/>.</param>
        /// <returns><c>true</c> if <paramref name="item"/> is found in the <see cref="System.Collection.IList"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="item"/> is of a type that is not assignable to the <see cref="System.Collection.IList"/>.</exception>
        bool IList.Contains(object item)
        {
            if (item != null && !(item is JsonValue))
            {
                throw new ArgumentException("The value must be of type JsonValue.", "value");
            }

            return this.Contains(item as JsonValue);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="System.Collection.IList"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="System.Collection.IList"/>.</param>
        /// <returns>The index of <paramref name="item"/> if found in the <see cref="System.Collection.IList"/>; otherwise, <c>–1</c>.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="item"/> is of a type that is not assignable to the <see cref="System.Collection.IList"/>.</exception>
        int IList.IndexOf(object item)
        {
            if (item != null && !(item is JsonValue))
            {
                throw new ArgumentException("The value must be of type JsonValue.", "value");
            }

            return this.IndexOf(item as JsonValue);
        }

        /// <summary>
        /// Adds an item to the <see cref="System.Collection.IList"/>.
        /// </summary>
        /// <param name="item">The <see cref="System.Object"/> to add to the <see cref="System.Collection.IList"/>.</param>
        /// <returns>The position into which the new element was inserted.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="item"/> is of a type that is not assignable to the <see cref="System.Collection.IList"/>.</exception>
        int IList.Add(object item)
        {
            if (item != null && !(item is JsonValue))
            {
                throw new ArgumentException("The value must be of type JsonValue.", "value");
            }

            this.Add(item as JsonValue);
            return this.count - 1;
        }

        /// <summary>
        /// Inserts an item to the <see cref="System.Collection.IList"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="System.Collection.IList"/>.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="System.Collection.IList"/>.</exception>
        /// <exception cref="System.ArgumentException"><paramref name="item"/> is of a type that is not assignable to the <see cref="System.Collection.IList"/>.</exception>
        void IList.Insert(int index, object item)
        {
            if (item != null && !(item is JsonValue))
            {
                throw new ArgumentException("The value must be of type JsonValue.", "value");
            }

            this.Insert(index, item as JsonValue);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="System.Collection.IList"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="System.Collection.IList"/>.</param>
        /// <exception cref="System.ArgumentException"><paramref name="item"/> is of a type that is not assignable to the <see cref="System.Collection.IList"/>.</exception>
        void IList.Remove(object item)
        {
            if (item != null && !(item is JsonValue))
            {
                throw new ArgumentException("The value must be of type JsonValue.", "value");
            }

            this.Remove(item as JsonValue);
        }

        /// <summary>
        /// Copies the elements of the <see cref="System.Collection.ICollection"/> to an <see cref="System.Array"/>, starting at a particular <see cref="System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="System.Collection.ICollection"/>.
        /// The <see cref="System.Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than <c>0</c>.</exception>
        /// <exception cref="System.ArgumentException"><paramref name="array"/> is multidimensional.</exception>
        /// <exception cref="System.ArgumentException"><paramref name="array"/> does not have zero-based indexing.</exception>
        /// <exception cref="System.ArgumentException">The number of elements in the source <see cref="System.Collection.ICollection"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        /// <exception cref="System.ArgumentException">The type of the source <see cref="System.Collection.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            if (!(array is JsonValue[]))
            {
                throw new ArgumentException("The array must be of type array of JsonValue.", "array");
            }

            this.CopyTo(array as JsonValue[], arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator"/> that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new JsonArray.Enumerator(this);
        }

        /// <summary>
        /// Writes the string representation of the <see cref="NiklasKarl.Json.JsonArray"/> to the <see cref="System.Text.StringBuilder"/> using the specified indent.
        /// The first line is not affected by the indent.
        /// </summary>
        /// <param name="destination">The <see cref="System.Text.StringBuilder"/> to write to.</param>
        /// <param name="indent">
        /// The indent to use when writing the string.
        /// A indent smaller than zero means the string should be as short as possible and no indent should be used.
        /// </param>
        internal override void ToString(StringBuilder destination, int indent)
        {
            if (this.count == 0)
            {
                destination.Append("[]");
            }
            else if (indent < 0)
            {
                destination.Append('[');

                for (int i = 0; i < this.count; i++)
                {
                    this.values[i].ToString(destination, -1);

                    if (i < this.count - 1)
                    {
                        destination.Append(',');
                    }
                }

                destination.Append(']');
            }
            else
            {
                destination.Append('[');
                destination.AppendLine();

                for (int i = 0; i < this.count; i++)
                {
                    destination.Append('\t', indent + 1);
                    this.values[i].ToString(destination, indent + 1);

                    if (i < this.count - 1)
                    {
                        destination.Append(',');
                    }

                    destination.AppendLine();
                }

                destination.Append('\t', indent);
                destination.Append(']');
            }
        }

        /// <summary>
        /// Ensures that the values buffer can hold at least <paramref name="capacity"/> elements.
        /// </summary>
        /// <param name="capacity">The minimum number of elements to hold.</param>
        private void EnsureCapacity(int capacity)
        {
            JsonValue[] tmp = new JsonValue[capacity];
            for (int i = 0; i < this.count; i++)
            {
                tmp[i] = this.values[i];
            }

            this.values = tmp;
        }

        /// <summary>
        /// Enumerates the elements of a <see cref="NiklasKarl.Json.JsonArray"/>.
        /// </summary>
        public struct Enumerator : IEnumerator<JsonValue>, IEnumerator, IDisposable
        {
            /// <summary>
            /// The <see cref="NiklasKarl.Json.JsonArray"/> to enumerate.
            /// </summary>
            private JsonArray array;

            /// <summary>
            /// The current position of the <see cref="NiklasKarl.Json.JsonArray.Enumerator"/> in the <see cref="NiklasKarl.Json.JsonArray"/>.
            /// </summary>
            private int index;

            /// <summary>
            /// Initializes a new instance of the <see cref="NiklasKarl.Json.JsonArray.Enumerator"/> struct.
            /// </summary>
            /// <param name="array">The array to enumerate over.</param>
            internal Enumerator(JsonArray array)
            {
                this.array = array;
                this.index = -1;
            }

            /// <summary>
            /// Gets the element at the current position of the <see cref="NiklasKarl.Json.JsonArray.Enumerator"/>.
            /// </summary>
            public JsonValue Current
            {
                get
                {
                    if (this.index < 0 || this.index == this.array.count)
                    {
                        throw new NotSupportedException();
                    }

                    return this.array[this.index];
                }
            }

            /// <summary>
            /// Gets the element the enumerator is currently pointing at.
            /// </summary>
            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            /// <summary>
            /// Advances the <see cref="NiklasKarl.Json.JsonArray.Enumerator"/> to the next element of the <see cref="NiklasKarl.Json.JsonArray"/>.
            /// </summary>
            /// <returns>
            /// <c>true</c> if the <see cref="NiklasKarl.Json.JsonArray.Enumerator"/> was successfully advanced to the next element;
            /// <c>false</c> if the <see cref="NiklasKarl.Json.JsonArray.Enumerator"/> has passed the end of the <see cref="NiklasKarl.Json.JsonArray"/>.
            /// </returns>
            public bool MoveNext()
            {
                if (this.index < this.array.count)
                {
                    this.index++;
                }

                return this.index != this.array.count;
            }

            /// <summary>
            /// Sets the <see cref="NiklasKarl.Json.JsonArray.Enumerator"/> to its initial position, which is before the first element in the <see cref="NiklasKarl.Json.JsonArray"/>.
            /// </summary>
            void IEnumerator.Reset()
            {
                this.index = -1;
            }

            /// <summary>
            /// Releases all resources used by the <see cref="NiklasKarl.Json.JsonArray.Enumerator"/>.
            /// </summary>
            void IDisposable.Dispose()
            {
            }
        }
    }
}
