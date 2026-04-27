using System;

class Node<T>
{
    public T Data { get; set; }
    public Node<T> Next { get; set; }
    public Node<T> Prev { get; set; }

    public Node(T data)
    {
        Data = data;
        Next = null;
        Prev = null;
    }
}

class MyLinkedList<T>
{
    private Node<T> head;
    private Node<T> tail;

    public void AddFirst(T item)
    {
        Node<T> newNode = new Node<T>(item);
        if (head == null)
        {
            head = tail = newNode;
        }
        else
        {
            newNode.Next = head;
            head.Prev = newNode;
            head = newNode;
        }
    }

    public void AddLast(T item)
    {
        Node<T> newNode = new Node<T>(item);
        if (tail == null)
        {
            head = tail = newNode;
        }
        else
        {
            newNode.Prev = tail;
            tail.Next = newNode;
            tail = newNode;
        }
    }

    public bool Remove(T item)
    {
        Node<T> current = head;
        while (current != null)
        {
            if (current.Data.Equals(item))
            {
                if (current.Prev != null)
                    current.Prev.Next = current.Next;
                else
                    head = current.Next;

                if (current.Next != null)
                    current.Next.Prev = current.Prev;
                else
                    tail = current.Prev;

                return true;
            }
            current = current.Next;
        }
        return false;
    }

    public Node<T> Find(T item)
    {
        Node<T> current = head;
        while (current != null)
        {
            if (current.Data.Equals(item))
                return current;
            current = current.Next;
        }
        return null;
    }

    public void PrintAll()
    {
        Node<T> current = head;
        while (current != null)
        {
            Console.Write(current.Data + " ");
            current = current.Next;
        }
        Console.WriteLine();
    }
}

class LinkedListManager<T>
{
    private MyLinkedList<T> list = new MyLinkedList<T>();

    public void AddFirst(T item)
    {
        list.AddFirst(item);
    }

    public void AddLast(T item)
    {
        list.AddLast(item);
    }

    public bool Remove(T item)
    {
        return list.Remove(item);
    }

    public bool Find(T item)
    {
        return list.Find(item) != null;
    }

    public void PrintAll()
    {
        list.PrintAll();
    }
}

class Program
{
    static void Main()
    {
        LinkedListManager<int> manager = new LinkedListManager<int>();

        manager.AddFirst(10);
        manager.AddFirst(5);
        manager.AddLast(20);
        manager.AddLast(30);

        Console.Write("Список: ");
        manager.PrintAll();

        Console.WriteLine($"Найден 20: {manager.Find(20)}");
        Console.WriteLine($"Удален 5: {manager.Remove(5)}");

        Console.Write("После удаления: ");
        manager.PrintAll();
    }
}