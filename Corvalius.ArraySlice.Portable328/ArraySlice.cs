﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

#if !NET35
using Corvalius.ArraySlice;
using System.ComponentModel;
using System.Diagnostics.Contracts;
#endif

namespace System
{
    public sealed class ArraySlice<T> : IList<T>
#if !NET35
, IHideObjectMembers
#endif
    {

#if !NET35
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif        
        public readonly int Offset;

#if !NET35
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif  
        public readonly T[] Array;

        public readonly int Length;

        public ArraySlice(int size) : this (new T[size])
        {}

        public ArraySlice(T[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array");
#if !NET35
            Contract.EndContractBlock();
#endif  
        
            Array = array;
            Offset = 0;
            Length = array.Length;
        }

        public ArraySlice(T[] array, int offset, int count)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "The argument cannot be negative."); 
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "The argument cannot be negative.");
            if (array.Length - offset < count)
                throw new ArgumentException("The length of the slice cannot be less than 0.");
#if !NET35
            Contract.EndContractBlock();
#endif  

            Array = array;
            Offset = offset;
            Length = count;
        }

        public ArraySlice(ArraySlice<T> slice) 
            : this ( slice.Array, slice.Offset, slice.Length)
        {}

        public ArraySlice(ArraySlice<T> slice, int offset, int count)
            : this(slice.Array, slice.Offset + offset, count)
        {
            if (slice.Length - offset < count)
                throw new ArgumentException("Slices created from other slices must be contained in the source slice. The length of the slice cannot be less than 0.");
#if !NET35
            Contract.EndContractBlock();
#endif  
        }

        public override int GetHashCode()
        {
            return null == Array
                        ? 0
                        : Array.GetHashCode() ^ Offset ^ Length;
        }

        public override bool Equals(Object obj)
        {
            if (obj is ArraySlice<T>)
                return Equals((ArraySlice<T>)obj);
            else
                return false;
        }

        public bool Equals(ArraySlice<T> obj)
        {
            return obj.Array == Array && obj.Offset == Offset && obj.Length == Length;
        }

        public static bool operator ==(ArraySlice<T> a, ArraySlice<T> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ArraySlice<T> a, ArraySlice<T> b)
        {
            return !(a == b);
        }

        public T this[int index]
        {
            get
            {
#if !NET35
                Contract.Requires(index >= 0);
                Contract.Requires(index < this.Length);
                Contract.Requires(this.Offset + index < this.Array.Length);
                Contract.EndContractBlock();
#endif  


                return Array[Offset + index];
            }
            set
            {
#if !NET35
                Contract.Requires(index >= 0);
                Contract.Requires(index < this.Length);
                Contract.Requires(this.Offset + index < this.Array.Length);
                Contract.EndContractBlock();
#endif  

                Array[Offset + index] = value;
            }
        }

        public static implicit operator ArraySlice<T>(T[] src)
        {
            return new ArraySlice<T>(src);
        }

        public static implicit operator T[](ArraySlice<T> src)
        {
            if (src.Count == src.Array.Length && src.Offset == 0)
                return src.Array;
            else
                throw new InvalidCastException("Casting implicitely an ArraySlice<T> into T[] is not possible when the slice does not contains the entire array.");
        }

        public int IndexOf(T item)
        {
            int end = Offset + Length;

            var array = this.Array;
            for (int i = Offset; i < end; i++)
            {
                if (array[i].Equals(item))
                    return i - Offset;
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            int end = Offset + Length;

            var array = this.Array;
            for (int i = Offset; i < end; i++)
            {
                if (array[i].Equals(item))
                    return true;
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            int end = Offset + Length;

            var src = this.Array;
            for (int i = Offset; i < end; i++)
                array[arrayIndex++] = src[i];
        }

        public int Count
        {
            get { return this.Length; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            int end = Offset + Length;

            var src = this.Array;
            for (int i = Offset; i < end; i++)
                yield return src[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
