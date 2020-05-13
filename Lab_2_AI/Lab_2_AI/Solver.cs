using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB2
{
    public class Solver
    {
        public int Goal { get; } // поле цільової ємності
        public List<State> Way { get; set; } // список вузлів на шляху до цільового стану
        public List<State> CloseList { get; set; } // список вузлів, в які не треба повертатись
        public List<State> OpenList { get; set; } // список досліджуваних вузлів
        public bool Success { get; set; } // прапорець для відслідковування успішності досягнення цілі
        public Solver(int goal){ // конструктор ініціалізації, в якому відбуваються мінімальні налаштування цієї шайтан-машини
            this.Goal = goal;
            Success = false;
            CloseList = new List<State>();
            OpenList = new List<State>();
        }

        public void RecursiveBestFirstSearch(State node){
            this.RBFS(node, 99999999999); 
            this.PrintWay();
        }

        private double RBFS(State node, double bound){//реалізація рекурсивного пошуку за першим найкращим збігом
            if (node.GCost > bound){
                return node.GCost;
            }
            if (IsGoal(node)){ // якщо ми досягли цільового стану, додаємо гілку до кінцевого шляху
                Way.Add(node);
                return 0;
            }
            if (node.Moves == null){ // якщо відсутні стани, у які можна перейти, то генеруємо їх
                node.GenerateMoves(Goal);
            }
            if (node.Moves.Count == 0){ // якщо нічого не згенерувалось, то повертаємо максимально можливу межу
                return 999999999999;
            }
            CloseList.Add(node); // додаємо поточний вузол у список пройдених
            foreach (State successor in node.Moves){ // переглядаємо кожний вузол нащадок
              // якщо константна вартість переходу у поточний вузол мешна за межу для гілки, то в цю гілку ми вже ходили і треба оновити значення по дочірніх вузлах, інакше встановити значення для дочірнього вузла як його мінімальну 
                successor.FCost = (node.GCost < node.FCost) ? Math.Max(node.FCost, successor.GCost) : successor.GCost;
                if (!node.FindIn(CloseList, successor) && !node.FindIn(OpenList, successor))
                {// перевіряємо чи є дочірній список у списку вже пройдених у цій гілці вузлів, щоб не зациклитися, та чи є він у списку наступних вузлів, оскільки вони взяли собі за моду дублюватись
                    OpenList.Add(successor);
                }
            }
            if (OpenList.Count == 0)
            { // якщо ми вже всі вузли відвідали
                return 99999999999; // не досліджуємо дану гілку
            }
            node.SortByFCost(OpenList, 0, OpenList.Count - 1); // сортуємо список доступних вузлів за зростанням ф-значеннь
            State bestnode = RemoveFirstNode(); // витягуємо зі списку найлегший вузол
            State alternativeNode; // беремо наступне значення в якості наступного мінімуму, або встановлюємо надвелику межу 
            alternativeNode = (OpenList.Count > 0) ? OpenList[0] : new State(0, 0, 0, 0, 0, 0);
            // поки не перевищили доступну межу шукаємо шлях до цільового стану
            while (bestnode.FCost <= bound && bestnode.FCost < double.PositiveInfinity)
            {
                double new_bound = Math.Min(bound, alternativeNode.FCost); // перевіряємо на легкість межу сусідньої гілки 
                bestnode.FCost = RBFS(bestnode, new_bound); // запускаємо алгоритм вже з найлегшим вузлом та найлегшою доступною межею
                OpenList.Add(bestnode); //додаємо до списку доступних вузлів найлегший в минулому вузел, щоб відсортувати список
                if (Success) // якщо мерехтить зелене світло, то запам’ятовуємо що даний вузел приведе нас дорогою успіху до цільової вершини
                {
                    Way.Add(node);
                    break;
                }
                node.SortByFCost(OpenList, 0, OpenList.Count - 1); //відсортували
                bestnode = RemoveFirstNode();
                alternativeNode = RemoveFirstNode();
            }
            return node.Moves[0].FCost; // повертаємо нову межу
        }

        public State RemoveFirstNode() // допоміжна функція для вилучення першого вузлу з списку доступних вузлів
        {
            State node;
            if (OpenList.Count > 0)
            {
                node = OpenList[0];
                OpenList.RemoveAt(0);
            }
            else
            {
                node = new State(0, 0, 0, 0, 0, 0);
            }
            return node;
        }

        public bool IsGoal(State node)
        { // допоміжна функція для виявлення досягання поставленних цілей
            int l1 = node.ToString().IndexOf("S") + node.litres5.FullSize.ToString().Length + 5;
            int l2 = node.litres5.FillSize.ToString().Length;
            bool convertedFirst = int.TryParse(node.ToString().Substring(node.ToString().IndexOf(" ") + 1, node.ToString().IndexOf(";") - node.ToString().IndexOf(" ") - 1), out int firstFill);
            bool convertedSecond = int.TryParse(node.ToString().Substring(node.ToString().IndexOf("S") + node.litres5.FullSize.ToString().Length + 5, node.litres5.FillSize.ToString().Length), out int secondFill);
            bool convertedThird = int.TryParse(node.ToString().Substring(node.ToString().LastIndexOf(" ") + 1, node.ToString().Length - node.ToString().LastIndexOf(" ") - 1), out int thirdFill);
            if (convertedFirst && convertedSecond && convertedThird && ((firstFill == Goal && secondFill == Goal) || (secondFill == Goal && thirdFill == Goal) || (firstFill == Goal && thirdFill == Goal))){
                Success = true;
                Way = new List<State>();
                return true;
            }
            return false;
        }

        public void PrintWay()
        { // допоміжна функція, для виведення шляху від початкової вершини до цільової
            for (int i = Way.Count - 1; i >= 0; i--)
            {
                Console.WriteLine($"{Way[i]} GCost: {Way[i].GCost}");
            }
        }
    }
}