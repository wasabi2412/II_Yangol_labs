using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB2{
    public class Jar{ // класс з конструкторами максимальної та поточної ємкостей посудин
        private int fillSize;
        private int fullSize;
        public int FullSize{
            get => this.fullSize;
            set => fullSize = value;
        }

        public int FillSize{
            get => this.fillSize;
            set=> fillSize = value;
        }

        public Jar(int fullSize, int fillSize){
            this.FullSize = fullSize;
            this.FillSize = fillSize;
        }
    }

}
