// See https://aka.ms/new-console-template for more information

namespace Aston {
    static class Program
    {
        public static void Main()
        {
            Vector2 t1 = new Vector2(0.3f);
            Vector2 t2 = new Vector2(0.5f, 1.3f);
            t1 += t2;
            Console.WriteLine(t1);
        }
    }
}