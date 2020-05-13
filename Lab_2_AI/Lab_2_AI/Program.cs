using System;
using System.Collections.Generic;
using System.IO;

namespace LAB2{
    class Program{
        static void Main(string[] args)
        {
            State init = new State(12, 12, 5, 0, 7, 0); // створюємо діжку та пусті банки для переливань
            Solver solv = new Solver(6); // задаємо необхідну заповненність
            solv.RecursiveBestFirstSearch(init); // запускаємо алгоритм
            Console.ReadKey();
        }
    }
}
