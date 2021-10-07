using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnQueue<T> where T : IComparable<T> {
    public T[] items;
    public int Count { get { return count; } }
    public int Size { get { return items.Length; } }
    public T currentItemTurn { get { return items[items.Length - 1]; } }
    public List<T> toList => ToList();
    int count = 0;

    /// <summary>
    /// Creates a TurnQueue object with the provided size
    /// </summary>
    /// <param name="itemCount"></param>
    public TurnQueue (int itemCount) {
        items = new T[itemCount];
    }

    /// <summary>
    /// Creates a TurnQueue from a list of objects, using its count as the Queues size.
    /// </summary>
    /// <param name="items"></param>
    public TurnQueue(List<T> items){
        int count = items.Count;
        this.items = new T[count];
        int i = 0;
        foreach (var item in items){
            this.items[0] = item;
            i++;
        }
    }

        /// <summary>
    /// Creates a TurnQueue from an array of objects, using its count as the Queues size.
    /// </summary>
    /// <param name="items"></param>
    public TurnQueue(T[] items){
        int count = items.Length;
        this.items = new T[count];
        int i = 0;
        foreach (var item in items){
            this.items[0] = item;
            i++;
        }
    }

    /// <summary>
    /// Creates a TurnQueue object with the default size, 10
    /// </summary>
    /// <param name="itemCount"></param>
    public TurnQueue () {
        items = new T[10];
    }

    /// <summary>
    /// Adds a new item to the TurnQueue. Cannot add more than the initial size of the TurnQueue.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Push (T item) {
        if (count == Size) return false;
        items[count] = item;
        count++;
        return true;
    }

    /// <summary>
    /// Only pop if the item is no longer needed. could be used in a game where you drop items, 
    /// or where you have enemies that have death anims and stuff
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T Pop (int index) {
        T[] newItems = new T[items.Length];
        T toReturn = items[index];
        for (int i = index; i < count; i++) {
            if (i >= index) {
                newItems[i - 1] = items[i];
                continue;
            }
            newItems[i] = items[i];
        }

        return toReturn;
    }

    /// <summary>
    /// Puts item at the end of the list at the begining again.
    /// </summary>
    public void NextTurn () {
        T item = items[items.Length - 1];
        for (int i = items.Length - 1; i > 0; i--) {
            items[i] = items[i - 1];
        }
        items[0] = item;
    }

    /// <summary>
    /// Swaps the positions the Queue of two items in the list.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="otherItem"></param>
    public void SwapTurnOrder (T item, T otherItem) {
        int firstIndex = GetIndex (item), secondIndex = GetIndex (otherItem);
        items[firstIndex] = otherItem;
        items[secondIndex] = item;
    }

    /// <summary>
    /// Moves the item up in the Queue by the provided amount.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    public void PushUp (T item, int amount) {
        int index = GetIndex (item);
        int newIndex = index + amount;
        if (newIndex > items.Length) return;
        
        for (int i = index; i < count; i++) {
            if (i == newIndex) {
                items[i] = item;
                break;
            }

            items[i] = items[i + 1];
        }
    }

    /// <summary>
    /// Moves the item down in the Queue by the provided amount.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    public void PushDown (T item, int amount) {
        int index = GetIndex (item);
        int newIndex = index - amount;
        if (newIndex < 0) return;

        for (int i = index; i > 0; i--) {
            if (i == newIndex) {
                items[i] = item;
                break;
            }
            items[i] = items[i - 1];
        }
    }

    /// <summary>
    /// Returns the index of the passed in item, if it's in the Queue.
    /// Otherwise returns -1.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int GetIndex (T item) {
        int index = -1;
        for (int i = 0; i < count; i++) {
            if (items[i].Equals(item)) {
                index = i;
                break;
            }
        }
        return index;
    }

    /// <summary>
    /// Creates a larger queue with a newly provided size.
    /// Just in case you actually for some reason need a larger queue. 
    /// Meant to reduce cpu/memory usage.
    /// </summary>
    /// <param name="newSize"></param>
    /// <returns></returns>
    public TurnQueue<T> largerQueue (int newSize) {
        if (Size < newSize) return null;
        TurnQueue<T> newQueue = new TurnQueue<T> (newSize);
        foreach (T item in items) {
            newQueue.Push (item);
        }
        return newQueue;
    }

    /// <summary>
    /// Sorts items by their implemented CompareTo methods
    /// </summary>
    public void Sort () {
        T temp;
        for (int i = 0; i < items.Length; i++) {
            for (int y = 0; y < items.Length; y++) {
                if (items[i].CompareTo (items[y]) == 1) {
                    temp = items[i];
                    items[i] = items[y];
                    items[y] = temp;
                }
            }
        }
    }

    /// <summary>
    /// Reverses the turn order.
    /// </summary>
    public void Reverse () {
        T[] reversedItems = new T[Size];
        for (int i = 0; i < count; i++) {
            reversedItems[i] = items[count - i];
        }
        items = reversedItems;
    }

    /// <summary>
    /// Converts the TurnQueue into a list.
    /// </summary>
    /// <returns></returns>
    public List<T> ToList(){
        List<T> list = new List<T>(Size);
        foreach (var item in items){   
            list.Add(item);
        }
        return list;
    }

    /// <summary>
    /// Converts the TurnQueue into an array.
    /// </summary>
    /// <returns></returns>
    public T[] ToArray(){
        //Have to do it like this or it manipulate the original array.
        T[] arr = new T[Size];
        int i = 0;
        foreach (var item in items){   
            arr[i] = item;
            i++;
        }
        return arr;
    } 

    /// <summary>
    /// Indexer. lets you access this like it was a normal array.
    /// </summary>
    /// <value></value>
    public T this[int index]{
        get => items[index];
        set => items[index] = value;
    }
}