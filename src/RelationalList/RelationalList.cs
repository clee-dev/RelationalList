using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RelationalList
{
    public static class LINQExtension
    {
        public static RelationalList<T1, T2> ToRelationalList<T1, T2>(this IEnumerable<T1> x, IEnumerable<T2> y)
        {
            RelationalList<T1, T2> ret = new RelationalList<T1, T2>();
            ret.X = x.ToList();
            ret.Y = y.ToList();
            return ret;
        }

        public static RelationalList<T1, T2> ToRelationalList<T1, T2>(this IEnumerable<T1> x, Func<T1, T2> selector)
        {
            RelationalList<T1, T2> ret = new RelationalList<T1, T2>();
            ret.X = x.ToList();
            ret.Y = (from s in x select selector(s)).ToList();
            return ret;
        }

        public static bool Any<T1, T2>(this RelationalList<T1, T2> list) => list.X.Any();

        public static bool Any<T1, T2>(this RelationalList<T1, T2> list, Func<T1, T2, Boolean> selector)
        {
            foreach (RelationalListPair<T1, T2> rp in list)
            {
                if (selector(rp.X, rp.Y)) return true;
            }
            return false;
        }
    }

    [Serializable]
    public class RelationalListPair<T1, T2>
    {
        public T1 X { get; set; }
        public T2 Y { get; set; }

        public RelationalListPair(T1 x, T2 y)
        {
            X = x;
            Y = y;
        }
    }
    public class RelationalListEnumerator<T1, T2> : IEnumerator
    {
        private RelationalList<T1, T2> RList;
        private int index;

        public RelationalListEnumerator(RelationalList<T1, T2> rl)
        {
            RList = rl;
            index = -1;
        }

        public object Current => new RelationalListPair<T1, T2>(RList.X[index], RList.Y[index]);

        public bool MoveNext()
        {
            index++;
            if (index >= RList.Count)
                return false;
            else
                return true;
        }

        public void Reset()
        {
            index = -1;
        }
    }

    [Serializable]
    public class RelationalList<T1, T2> : IEnumerable
    {
        public List<T1> X = new List<T1>();
        public List<T2> Y = new List<T2>();

        public int Count => X.Count;
        public int Capacity => X.Capacity + Y.Capacity;

        public RelationalList()
        {
            if (typeof(T1) == typeof(T2))
                throw new Exception("RelationalList requires 2 different types");
        }

        public RelationalList(IEnumerable<T1> x, IEnumerable<T2> y) : this()
        {
            X = x.ToList();
            Y = y.ToList();
        }

        public T2 this[T1 i]
        {
            //RelationList<int, string> X = new RelationalList<int, string>();
            //int n = 5;
            //string s = "test";
            //  //X[n] == "test"
            //X[n] = "test2";
            //  //X[n] == "test2"

            get
            {
                int x = X.IndexOf(i);
                return Y[x];
            }
            set
            {
                int x = X.IndexOf(i);
                Y[x] = (T2)value;
            }
        }

        public T1 this[T2 i]
        {
            //RelationList<int, string> X = new RelationalList<int, string>();
            //int n = 13;
            //string s = "yes";
            //  //X[s] == 13
            //X[s] = 14;
            //  //X[s] == 14

            get
            {
                int x = Y.IndexOf(i);
                return X[x];
            }
            set
            {
                int x = Y.IndexOf(i);
                X[x] = (T1)value;
            }
        }

        public RelationalListPair<T1, T2> ElementAt(int index) => new RelationalListPair<T1, T2>(X[index], Y[index]);

        public RelationalListPair<T1, T2> First() => ElementAt(0);

        public RelationalListPair<T1, T2> Last() => ElementAt(Count - 1);

        public RelationalListPair<T1, T2> FirstOrDefault() => Count > 0 ? First() : null;

        public RelationalListPair<T1, T2> LastOrDefault() => Count > 0 ? Last() : null;

        public void Add(T1 x, T2 y)
        {
            for (int i = 0; i < Count; i++)
                if (X[i].Equals(x) && Y[i].Equals(y)) throw new Exception("Can't add duplicate RelationalListPair.");

            X.Add(x);
            Y.Add(y);
        }

        public void Add(RelationalListPair<T1, T2> item)
        {
            Add(item.X, item.Y);
        }

        public void Remove(T1 x, T2 y)
        {
            for (int i = 0; i < Count; i++)
            {
                if (X[i].Equals(x) && Y[i].Equals(y))
                {
                    RemoveAt(i);
                    return;
                }
            }
            throw new Exception("RelationalListPair not found.");
        }

        public void Remove(RelationalListPair<T1, T2> item)
        {
            Remove(item.X, item.Y);
        }

        public void RemoveAt(int index)
        {
            if (index >= Count || index < 0) throw new IndexOutOfRangeException("Index cannot be less than 0 or greater than RelationalList.Count - 1.");
            X.RemoveAt(index);
            Y.RemoveAt(index);
        }

        public void Clear()
        {
            X.Clear();
            Y.Clear();
        }

        public int IndexOfX(T1 item) => X.IndexOf(item);

        public int IndexOfY(T2 item) => Y.IndexOf(item);

        public int IndexOf(T1 x, T2 y)
        {
            for (int i = 0; i < Count; i++)
                if (X[i].Equals(x) && Y[i].Equals(y)) return i;
            return -1;
        }

        public int IndexOf(RelationalListPair<T1, T2> item) => IndexOf(item.X, item.Y);

        public void Insert(T1 x, T2 y, int index)
        {
            for (int i = 0; i < Count; i++)
                if (X[i].Equals(x) && Y[i].Equals(y)) throw new Exception("Can't add duplicate RelationalListPair.");

            X.Insert(index, x);
            Y.Insert(index, y);
        }

        public void Insert(RelationalListPair<T1, T2> item, int index)
        {
            Insert(item.X, item.Y, index);
        }

        public bool Contains(T1 x, T2 y)
        {
            if (!X.Contains(x) || !Y.Contains(y)) return false;
            for (int i = 0; i < Count; i++)
                if (X[i].Equals(x) && Y[i].Equals(y)) return true;
            return false;
        }

        public bool Contains(RelationalListPair<T1, T2> item) => Contains(item.X, item.Y);

        public bool ContainsX(T1 item) => X.Contains(item);

        public bool ContainsY(T2 item) => Y.Contains(item);

        /// <summary>
        /// Exception thrown and new items removed if duplicates were added
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(RelationalList<T1, T2> items)
        {
            X = X.Concat(items.X).ToList();
            Y = Y.Concat(items.Y).ToList();

            if (DuplicatesFound())
            {
                RemoveRange(items);
                throw new Exception("Duplicate RelationalListPairs found after AddRange()");
            }
        }

        /// <summary>
        /// Exception thrown and new items removed if duplicates were added
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddRange(IEnumerable<T1> x, IEnumerable<T2> y)
        {
            if (x.Count() != y.Count())
                throw new Exception("Enumerables are not equal in length");

            AddRange(x.ToRelationalList(y));
        }

        /// <summary>
        /// Exception thrown and new items removed if duplicates were added
        /// </summary>
        /// <param name="index"></param>
        /// <param name="items"></param>
        public void InsertRange(int index, RelationalList<T1, T2> items)
        {
            X.InsertRange(index, items.X);
            Y.InsertRange(index, items.Y);

            if (DuplicatesFound())
            {
                RemoveRange(items);
                throw new Exception("Cannot insert duplicate elements");
            }
        }

        /// <summary>
        /// Exception thrown and new items removed if duplicates were added
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void InsertRange(int index, IEnumerable<T1> x, IEnumerable<T2> y)
        {
            if (x.Count() != y.Count())
                throw new Exception("Enumerables are not equal in length");

            InsertRange(index, x.ToRelationalList(y));
        }

        public int LastIndexOfX(T1 x)
        {
            int index = -1;
            for (int i = 0; i < Count; i++)
            {
                if (X[i].Equals(x)) index = i;
            }
            return index;
        }

        public int LastIndexOfY(T2 y)
        {
            int index = -1;
            for (int i = 0; i < Y.Count; i++)
            {
                if (Y[i].Equals(y)) index = i;
            }
            return index;
        }

        public int LastIndexOf(T1 x, T2 y)
        {
            int index = -1;
            for (int i = 0; i < Count; i++)
            {
                if (X[i].Equals(x) && Y[i].Equals(y)) index = i;
            }
            return index;
        }

        public int LastIndexOf(RelationalListPair<T1, T2> item) => LastIndexOf(item.X, item.Y);

        public void CopyTo(object[,] array)
        {
            if (array.GetLength(0) < Count || array.GetLength(1) < 2)
                throw new IndexOutOfRangeException("The target array is not large enough");

            for (int i = 0; i < Count; i++)
            {
                array[i, 0] = X[i];
                array[i, 1] = Y[i];
            }
        }

        public object[,] ToArray()
        {
            object[,] arr = new object[Count, 2];
            CopyTo(arr);
            return arr;
        }

        public List<RelationalListPair<T1, T2>> ToList()
        {
            List<RelationalListPair<T1, T2>> l = new List<RelationalListPair<T1, T2>>();
            for (int i = 0; i < Count; i++)
                l.Add(ElementAt(i));
            return l;
        }

        public void RemoveRange(int index, int count)
        {
            for (int i = 0; i < count; i++)
            {
                RemoveAt(index + i);
            }
        }

        public void RemoveRange(RelationalList<T1, T2> items)
        {
            foreach (RelationalListPair<T1, T2> p in items)
            {
                if (Contains(p))
                    Remove(p);
            }
        }

        public void Reverse()
        {
            RelationalList<T1, T2> ret = Reversed();
            X = ret.X;
            Y = ret.Y;
        }

        public RelationalList<T1, T2> Reversed()
        {
            RelationalList<T1, T2> reversed = new RelationalList<T1, T2>(X, Y);
            reversed.X.Reverse();
            reversed.Y.Reverse();
            return reversed;
        }

        public RelationalList<T1, T2> FromDictionary(Dictionary<T1, T2> d)
        {
            RelationalList<T1, T2> ret = new RelationalList<T1, T2>();
            foreach (KeyValuePair<T1, T2> kvp in d)
            {
                ret.Add(kvp.Key, kvp.Value);
            }
            return ret;
        }

        public RelationalList<TX, TY> ConvertTo<TX, TY>() where TX : T1 where TY : T2
        {
            RelationalList<TX, TY> rl = new RelationalList<TX, TY>();
            for (int i = 0; i < Count; i++)
            {
                rl.Add(new RelationalListPair<TX, TY>((TX)X[i], (TY)Y[i]));
            }
            return rl;
        }

        private bool DuplicatesFound()
        {
            for (int i = 1; i < Count; i++)
            {
                for (int x = 0; x < i; x++)
                {
                    if (X[x].Equals(X[i]) && Y[x].Equals(Y[i])) return true;
                }
            }
            return false;
        }

        public IEnumerator GetEnumerator() => new RelationalListEnumerator<T1, T2>(this);
    }
}