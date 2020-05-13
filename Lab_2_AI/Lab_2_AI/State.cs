using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB2{
    public class State{ // клас станів
        public Jar litres12 { get; } 
        public Jar litres5 { get; }
        public Jar litres7 { get; }

        public List<State> Moves { get; set; }
        public double FCost { get; set; } 
        public double GCost { get; set; }

        public State(int firstSize, int firstFill, int secondSize, int secondFill, int thirdSize, int thirdFill){
            litres12 = new Jar(firstSize, firstFill);
            litres5 = new Jar(secondSize, secondFill);
            litres7 = new Jar(thirdSize, thirdFill);
            if (firstSize == 0 && secondSize == 0 && thirdSize == 0){
                FCost = 999999999999;
                GCost = 999999999999;
            }
        }

        public void GenerateMoves(int goal) { //створюємо список доступних переходів за такою схемою:
                                              //1 - перевірка на можливість переливання
                                              //2 - якщо можна, переливаємо повністю з одного в інше
                                              //3 - ні, то переливаємо що є
            Moves = new List<State>();
            if (litres12.FillSize > 0 && litres5.FillSize < litres5.FullSize) { //з 12літрової в п'ятилітрову
                if (litres5.FullSize - litres5.FillSize >= litres12.FillSize) {
                    Moves.Add(new State(litres12.FullSize, 0, litres5.FullSize, litres5.FillSize + litres12.FillSize, litres7.FullSize, litres7.FillSize));
                }
                else {
                    Moves.Add(new State(litres12.FullSize, litres12.FillSize - (litres5.FullSize - litres5.FillSize), litres5.FullSize, litres5.FullSize, litres7.FullSize, litres7.FillSize));
                }
            }

            if (litres12.FillSize > 0 && litres7.FillSize < litres7.FullSize) { // з 12літрової в 7літрову
                if (litres7.FullSize - litres7.FillSize >= litres12.FillSize) {
                    Moves.Add(new State(litres12.FullSize, 0, litres5.FullSize, litres5.FillSize, litres7.FullSize, litres7.FillSize + litres12.FillSize));
                }
                else {
                    Moves.Add(new State(litres12.FullSize, litres12.FillSize - (litres7.FullSize - litres7.FillSize), litres5.FullSize, litres5.FillSize, litres7.FullSize, litres7.FullSize));
                }
            }

            if (litres5.FillSize > 0 && litres12.FillSize < litres12.FullSize){ //з п'ятилітрової в 12літрову
                if (litres12.FullSize - litres12.FillSize >= litres5.FillSize){
                    Moves.Add(new State(litres12.FullSize, litres12.FillSize + litres5.FillSize, litres5.FullSize, 0, litres7.FullSize, litres7.FillSize));
                }
                else{
                    Moves.Add(new State(litres12.FullSize, litres12.FullSize, litres5.FullSize, litres5.FillSize - (litres12.FullSize - litres12.FillSize), litres7.FullSize, litres7.FillSize));
                }
            }

            if (litres5.FillSize > 0 && litres7.FillSize < litres7.FullSize){ //з 5літрової в 7літрову
                if (litres7.FullSize - litres7.FillSize >= litres5.FillSize){
                    Moves.Add(new State(litres12.FullSize, litres12.FillSize, litres5.FullSize, 0, litres7.FullSize, litres7.FillSize + litres5.FillSize));
                }
                else{
                    Moves.Add(new State(litres12.FullSize, litres12.FillSize, litres5.FullSize, litres5.FillSize - (litres7.FullSize - litres7.FillSize), litres7.FullSize, litres7.FullSize));
                }
            }

            if (litres7.FillSize > 0 && litres12.FillSize < litres12.FullSize){ //з 7літрової в 12літрову
                if (litres12.FullSize - litres12.FillSize >= litres7.FillSize){
                    Moves.Add(new State(litres12.FullSize, litres12.FillSize + litres7.FillSize, litres5.FullSize, litres5.FillSize, litres7.FullSize, 0));
                }
                else{
                    Moves.Add(new State(litres12.FullSize, litres12.FullSize, litres5.FullSize, litres5.FillSize, litres7.FullSize, litres7.FillSize - (litres12.FullSize - litres12.FillSize)));
                }
            }

            if (litres7.FillSize > 0 && litres5.FillSize < litres5.FullSize){ //з 7літрової в 5літрову
                if (litres5.FullSize - litres5.FillSize >= litres7.FillSize){
                    Moves.Add(new State(litres12.FullSize, litres12.FillSize, litres5.FullSize, litres5.FillSize + litres7.FillSize, litres7.FullSize, 0));
                }
                else{
                    Moves.Add(new State(litres12.FullSize, litres12.FillSize, litres5.FullSize, litres5.FullSize, litres7.FullSize, litres7.FillSize - (litres5.FullSize - litres5.FillSize)));
                }
            }
            CountGCOst(goal); // обраховуємо початкову вартість переходу
        }

        private void CountGCOst(int goal){
            foreach (State move in Moves){
                if (move.litres12.FullSize < goal){ // якщо у першу банку не вміщується все необхідне, то орієнтуємось на другу банку
                    move.GCost = Math.Abs(goal - (move.litres5.FillSize + move.litres7.FillSize));
                }
                else if (move.litres5.FullSize < goal){ // і навпаки
                    move.GCost = Math.Abs(goal - (move.litres12.FillSize + move.litres7.FillSize));
                }
                else if (move.litres7.FullSize < goal){
                    move.GCost = Math.Abs(goal - (move.litres12.FillSize + move.litres5.FillSize));
                }
                else{ // а якщо в обидві банки вміщується, то орієнтуємось на суму вмісту банок, щоб в разу чого хоба, перелити з однієї в іншу і приїхали
                    move.GCost = Math.Abs(goal - (move.litres12.FillSize + move.litres5.FillSize + move.litres7.FillSize));
                }
            }
        }

        override
        public string ToString(){ //конкатинація рядків для коректного вивиду
            return string.Concat($"FJ-{litres12.FullSize}: {litres12.FillSize}; SJ-{litres5.FullSize}: {litres5.FillSize}; TJ-{litres7.FullSize}: {litres7.FillSize}");
        }

        public List<State> SortByFCost(List<State> nodes, int first, int last){ // сортує стани за ф-вартістю
            if (first < last){
                double pivot = nodes[0].FCost;
                int j = first - 1;
                State temp;
                for (int i = first; i <= last - 1; i++){
                    if (nodes[i].FCost < pivot){
                        j++;
                        temp = nodes[i];
                        nodes[i] = nodes[j];
                        nodes[j] = temp;
                    }
                }
                temp = nodes[last];
                nodes[last] = nodes[j + 1];
                nodes[j + 1] = temp;
                j++;
                SortByFCost(nodes, first, j - 1);
                SortByFCost(nodes, j + 1, last);
            }
            return nodes;
        }

        public override bool Equals(object obj) // допоміжна функція для порівняння вузлів між собою
        {
            if (obj == null){
                return false;
            }
            if (!(obj is State)){
                return false;
            }
            return (this.litres12.FullSize == ((State)obj).litres12.FullSize)
                && (this.litres5.FullSize == ((State)obj).litres5.FullSize)
                && (this.litres7.FullSize == ((State)obj).litres7.FullSize)
                && (this.litres12.FillSize == ((State)obj).litres12.FillSize)
                && (this.litres5.FillSize == ((State)obj).litres5.FillSize)
                && (this.litres7.FillSize == ((State)obj).litres7.FillSize);
        }

        public bool FindIn(List<State> nodes, State compared){ // допоміжна функція пошуку необхідного вузла у списку¬¬¬
            bool result = false;
            foreach (State node in nodes){
                if (node.Equals(compared)){
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
