class A
{
    public int a;
    public int b;

    public A(int a, int b)
    {
        this.a = a;
        this.b = b;
    }

    public double Srednee()
    {
        return (a + b) / 2.0;
    }

    public double Vyrazhenie()
    {
        return Math.Pow(b, 3) + Math.Sqrt(a);
    }
}
