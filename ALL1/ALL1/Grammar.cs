using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.IO;

namespace ALL1
{//verificare alfabet la inc prog!!!


 
   public sealed class Grammar:MainWindow
    {
       public struct Table
        {
            public string terminal;
            public string nonterminal;
            public int rule;

        };

        public static Grammar instance = null;
        private static object padlock = new object();

        public string startSymb; //simbol start

        public string[] nonterminal=new string[100]; //multimea neterminalelor
        public int nontermNb;

        public string[] terminal= new string[100]; //multimea terminalelor
        public int termNb;

        public string[,] rule = new string[100, 100]; //regulile de productie
        public int rulesNb; //numarul regulilor de productie
        public int[] ruleSize=new int[25]; //dimensiunea fiecarei reguli de productie

        public const string EPS = "ξ";

        public string[,] FIRST = new string[100, 100];
        public int[] firstSize = new int[100];
        string[] done1 = new string[100];


        public string[,] FOLLOW = new string[100, 100];
        public int[] followSize = new int[100];
        string[] done2 = new string[100];


        public string[,] dirSymbols = new string[100,100];

        public Table[] t = new Table[100];
       public int tableNb;
        static int init_index=0;
        Grammar()
        {
        }

        public static Grammar Instance()
        {
        

            if (instance == null)
            {
                lock (padlock)
                    if (instance == null)
                    {
                        instance = new Grammar();
                    }
            }
                return instance;
     
        }
        public void Load_grammar()
        {

            startSymb = splitGrammar[0];
            rulesNb = Convert.ToInt32(splitGrammar[3]);

            string temp;
            temp = splitGrammar[1];
            nonterminal = splitGrammar[1].Split(' ');
            nontermNb = nonterminal.Length;
            terminal = splitGrammar[2].Split(' ');
            termNb = terminal.Length;

            string[] temp2 = new string[100];
            int row = 0;
            int col = 0;

            for (int i = 4; i < 4 + rulesNb; i++)
            {
                temp2 = splitGrammar[i].Split(' ');
                for (int z = 0; z < temp2.Length; z++)
                {
                    if (z == 1)
                        continue;
                    rule[row, col] = temp2[z];
                    col++;
                }
                ruleSize[row] = col;
                col = 0;
                row++;
            }

            //afisare matrice reguli productie
            /*for (int i = 0; i < rulesNb; i++)
                for (int j = 0; j < ruleSize[i]; j++)
                    Console.WriteLine(rule[i, j]);*/



        }


        public void ModifyGrammar()
        {

            int check = 0;
            int stop = 0;

            string[] alfa=new string[50];
            string[] beta=new string[50];
            int beta_index = 0;
            string [] gama=new string[50];
            int gama_index = 0;
            int check2 = 0;

            //Left Factorisation 
            while (check != rulesNb)
            {
                for (int i = 0; i < rulesNb; i++)
                {
                    for (int j = i + 1; j <= rulesNb - 1; j++)
                    {

                        alfa = new string[50];
                        beta = new string[50];
                        beta_index = 0;
                        gama = new string[50];
                        gama_index = 0;

                        if (ruleSize[i] < ruleSize[j])
                        {
                            check2 = 0;

                            for (int k = 0; k < ruleSize[i]; k++)
                            {
                                if (rule[i, k] == rule[j, k])
                                {
                                    stop++;
                                    check2++;
                                }
                                else
                                {
                                    check2--;
                                    if (stop != check2)
                                        break;
                                }

                            }
                            //A=alfa-beta ; A=alfa-gama
                            if (stop > 1)
                            {


                                for (int z = 0; z < stop; z++)
                                {
                                    alfa[z] = rule[i, z];
                                }



                                if (rule[i, stop] != null)
                                {
                                    for (int z = stop; z < ruleSize[i]; z++)
                                    {
                                        beta[beta_index] = rule[i, z];
                                        beta_index++;
                                    }
                                }
                                else
                                {
                                    beta[beta_index] = EPS;
                                    beta_index++;
                                }


                                if (rule[j, stop] != null)
                                {
                                    for (int z = stop; z < ruleSize[j]; z++)
                                    {
                                        gama[gama_index] = rule[j, z];
                                        gama_index++;
                                    }
                                }
                                else
                                {
                                    gama[gama_index] = EPS;
                                    gama_index++;
                                }


                                if (LeftFactorisation(i, j, stop, alfa, beta, gama, stop, beta_index, gama_index) == false)
                                    Console.WriteLine("Gramatica data nu este o gramatca LL1!");
                                else
                                {
                                    Console.WriteLine($"Gramatica a fost modificata: regulile {i + 1} si {j + 1}!");

                                }
                                
                            }
                        }
                        else
                        {
                            check2 = 0;
                            for (int k = 0; k < ruleSize[j]; k++)
                            {
                                if (rule[i, k] == rule[j, k])
                                {
                                    stop++;
                                    check2++;
                                }
                                else
                                {
                                    check2--;
                                    if (check2 != stop)
                                        break;
                                }
                            }

                            //A=alfa-beta ; A=alfa-gama
                            if (stop > 1)
                            {

                                for (int z = 0; z < stop; z++)
                                {
                                    alfa[z] = rule[j, z];
                                }



                                if (rule[j, stop] != null)
                                {
                                    for (int z = stop; z < ruleSize[j]; z++)
                                    {
                                        beta[beta_index] = rule[j, z];
                                        beta_index++;
                                    }
                                }
                                else
                                {
                                    beta[beta_index] = EPS;
                                    beta_index++;
                                }



                                if (rule[i, stop] != null)
                                {
                                    for (int z = stop; z < ruleSize[i]; z++)
                                    {
                                        gama[gama_index] = rule[i, z];
                                        gama_index++;
                                    }
                                }
                                else
                                {
                                    gama[gama_index] = EPS;
                                    gama_index++;
                                }


                                if (LeftFactorisation(i, j, stop, alfa, beta, gama, stop, beta_index, gama_index) == false)
                                    Console.WriteLine("Gramatica data nu este o gramatca LL1!");
                                else
                                {
                                    Console.WriteLine($"Gramatica a fost modificata: regulile {i + 1} si {j + 1}!");
                                }


                            }

                        }
                        stop = 0;
                        alfa = null;
                        beta = null;
                        gama = null;
                    }
                }
                check++;
            }

                check = 0;
                //Left Recursion

                //A-> beta ; A-> A gama
                while (check != rulesNb)
                {
                    for (int i = 0; i < rulesNb; i++)
                    {
                        for (int j = i + 1; j <= rulesNb; j++)
                            if ((rule[i, 0] == rule[i, 1]) && (rule[j, 0] == rule[i, 0]))
                            {

                                
                                beta = new string[50];
                                beta_index = 0;
                                gama = new string[50];
                                gama_index = 0;

                                for(int k=2;k<ruleSize[i];k++)
                                {
                                    gama[gama_index] = rule[i, k];
                                    gama_index++;
                                }

                                for(int k=1;k<ruleSize[j];k++)
                                {
                                    beta[beta_index] = rule[j, k];
                                    beta_index++;
                                }

                                if (LeftRecursion(i, j,beta,gama,beta_index,gama_index) == false)
                                    Console.WriteLine("Gramatica data nu este o gramatca LL1!");
                                else
                                {
                                    Console.WriteLine($"Gramatica a fost modificata: regulile {i + 1} si {j + 1}!");
                                }
                            }
                        else if(rule[j,0]==rule[j,1] && rule[j,0]==rule[i,0])
                            {

                                beta = new string[50];
                                beta_index = 0;
                                gama = new string[50];
                                gama_index = 0;

                                for (int k = 2; k < ruleSize[j]; k++)
                                {
                                    gama[gama_index] = rule[j, k];
                                    gama_index++;
                                }

                                for (int k = 1; k < ruleSize[i]; k++)
                                {
                                    beta[beta_index] = rule[i,k];
                                    beta_index++;
                                }

                                if (LeftRecursion(j, i, beta, gama, beta_index, gama_index) == false)
                                    Console.WriteLine("Gramatica data nu este o gramatca LL1!");
                                else
                                {
                                    Console.WriteLine($"Gramatica a fost modificata: regulile {i + 1} si {j + 1}!");
                                }
                            }
                        beta = null;
                        gama = null;
                        beta_index = 0;
                        gama_index = 0;

                    }
                    check++;
                }

                //Scriem in fisier noua forma a regulilor de productie
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\Work 3\Compilatoare\out.txt"))
                {
                    for (int x = 0; x < rulesNb; x++)
                    {
                        for (int y = 0; y < ruleSize[x]; y++)
                        {
                            if (y == 1)
                            {
                                file.Write("->");
                            }
                            file.Write(rule[x, y]);
                        }
                        file.WriteLine();
                    }

                }


            
        }

        bool LeftFactorisation(int r1, int r2,int index,string[]alfa,string[]beta,string[]gama,int alfa_index,int beta_index,int gama_index)
        {
            //Neterminalul din partea stanga a regulii de productie
            string nt = rule[r1, 0];
            //Alegen un nou neterminal
            nt = PickNonterminal(nt);
            //Il adaugam la multimea neterminalelor
            AddNonterminal(nt);


            //Modificam regulile 
            //Pas1 A=alfa A1
             
            //A=alfa
            for (int k = 0; k < alfa_index; k++)
            {
                rule[rulesNb, k] = alfa[k];

            }

            //A=alfa A1
            rule[rulesNb, index] = nt;

            ruleSize[rulesNb] = index+1;
            rulesNb++;

            //Pas2 A1=beta

            
                rule[r1, 0] = nt;
                int j = 0;

            for (int i = 1; j <beta_index; i++)
            {
                if (beta[j] != null)
                {
                    rule[r1, i] = beta[j];
                    j++;
                }
                else
                    break;
               
            }
            for(int i=j+1;i<ruleSize[r1];i++)
            {
                rule[r1, i] = null;
            }

            ruleSize[r1] = j+1;

            //Pas3
            rule[r2, 0] = nt;
            j = 0;

            for (int i = 1; j < gama_index; i++)
            {
                if (gama[j] != null)
                {
                    rule[r2, i] = gama[j];
                    j++;
                }
                else break;
            }
            for (int i = j+1; i < ruleSize[r2]; i++)
            {
                rule[r2, i] = null;
            }

            ruleSize[r2] = j+1;



            return true;
        }

        string PickNonterminal(string nt)
        {

            nt = nt + "I";
            while (checkifEx(nt) == true)
            {
                nt = nt + "I";
            }
            return nt;

        }

        bool LeftRecursion(int r1, int r2,string[]beta,string[]gama,int beta_index,int gama_index )
        {
            //Neterminalul din partea stanga a regulii de productie
            string nt = rule[r1, 0];
            //Alegen un nou neterminal
            nt = PickNonterminal(nt);
            //Il adaugam la multimea neterminalelor
            AddNonterminal(nt);

            //Modificam regulile
            //  Pas1 

            rule[rulesNb, 0] = nt;
            rule[rulesNb, 1] = EPS;
            Array.Resize(ref ruleSize, ruleSize.Length + 1);
            ruleSize[rulesNb] = 2;
            rulesNb++;

            //Pas2-regula 1

           rule[r1, 0] = nt;
            int j = 0;
            for (int i = 1; j < gama_index; i++)
            {
                rule[r1, i] = gama[j];
                j++;
            }
            j++;
            rule[r1, j] = nt;
            j++;
            for (int i = j; i < ruleSize[r1]; i++)
            {
                rule[r1, i] = null;
            }
            ruleSize[r1] = j;

            //Pas 2. -regula 2

           
            j = 0;
            for (int i = 1; j < beta_index; i++)
            {
                rule[r2, i] = beta[j];
                j++;
            }
            j++;
            rule[r2, j] = nt;
            j++;
            for (int i = j; i < ruleSize[r2]; i++)
            {
                rule[r2, i] = null;
            }
            ruleSize[r2] = j;
            

            return true;
        }

        void AddNonterminal(string nt)
        {

            Array.Resize(ref nonterminal, nonterminal.Length + 1);
            nonterminal[nontermNb] = nt;
            nontermNb++;
        }

        public bool checkifEx(string nt)
        {
            for (int i = 0; i < nontermNb; i++)
            {
                if (nt == nonterminal[i])
                    return true;
            }
            return false;
        }


        void findFollow(string symbol, string init, int index,string[] initvect)
        {
          //  initvect = new string[100];
            for (int i = 0; i < rulesNb; i++)
            {
                for (int j =1 ; j < ruleSize[i]; j++)
                {
                    if (rule[i, j] == symbol)
                    {
                        if (isTerminal(rule[i, j + 1]) == true) //daca in pt dreapta e terminal
                            AddToFollow(rule[i, j + 1], index);

                        else if (rule[i, j + 1] == null)//daca e pe ultima poz din regula
                        {

                            if (ifInit(initvect,init_index,rule[i,0]) == false) //daca nu s-a mai trecut prin aceeasi regula o data
                            {

                                initvect[init_index] = symbol;
                                init_index++;
                                findFollow(rule[i, 0], symbol, index, initvect);
                               
                                
                            }
                            else
                                break;

                        }
                        else if (checkifEx(rule[i, j + 1]) == true)//verif daca e neterminal in dreapta
                        {
                            getFirst(rule[i, j + 1], index);
                            if (isEps(rule[i, j + 1]) == true)
                            {
                                if (isDoneFollow(rule[i, 0]) == false)
                                {
                                    findFollow(rule[i, 0],init, index,initvect);

                                }
                                else
                                   if (getFollow(rule[i, 0], i, index) == false)
                                    return;
                            }

                        }
                    }
                }
            }
            


        }

        bool ifInit(string [] initvect,int init_index,string symbol)
        {
            for(int i=0;i<init_index;i++)
            {
                if (symbol == initvect[i] /*&& init_index>=rulesNb*/)
                    return true;
            }


            return false;
        }
        bool getFollow(string symbol, int index, int i)
        {
            //daca a mai fost adaugat o data iesim
           
            for (int k = 0; k < followSize[i]; k++)
            {
                if (FOLLOW[i, k] == symbol)
                    return false;
            }

            while (FOLLOW[index, followSize[index]] != null)
            {
                FOLLOW[i, followSize[i]] = FOLLOW[index, followSize[index]];
                followSize[i]++;
                followSize[index]++;
                
            }
            return true;

        }
        void getFirst(string symbol, int index)
        { 
            for (int i = 0; i <rulesNb; i++)
                if (done1[i] == symbol)
                {
                    for (int j = 0; j < firstSize[i]; j++)
                    {
                        AddToFollow(FIRST[i, j], index);

                    }
                }
        }
        bool isDoneFirst(string symbol,int index)
        {
            for (int i = 0; i < done1.Length; i++)
                if (index == i && symbol==done1[i])
                    return true;
            return false;
        }

        bool isDoneFollow(string symbol)
        {
            for (int i = 0; i < done2.Length; i++)
                if (symbol == done2[i])
                    return true;
            return false;
        }
        //index=indexul din vectorul first
        void findFirst(string symbol, int index)
        {

            for ( int i = 0; i < rulesNb; i++)
            {
                if (symbol == rule[i, 0])
                {
                    if (rule[i, 1] != EPS)
                    {
                        if (isTerminal(rule[i, 1]) == true || rule[i, 1] == EPS)
                        {
                            if ((rule[index, 0] == symbol && index < i && FIRST[index, 0] != null) || (rule[index, 0] == rule[i, 0] && index > i && FIRST[i, 0] != null))
                            {
                                continue;
                            }
                            else
                                AddToFirst(rule[i, 1], index);
                        }
                        else if ((rule[index, 0] == symbol && index < i && FIRST[index, 0] != null) || (rule[index, 0] == rule[i, 0] && index > i && FIRST[i, 0] != null))
                            continue;
                        else
                            findFirst(rule[i, 1], index);
                    }
                    
                }

            }



        }

        bool isEps(string symbol)
        {
            for (int i = 0; i < rulesNb; i++)
            {
                if (symbol == rule[i, 0] && rule[i, 1] == EPS)
                    return true;
            }


            return false;
        }
       public bool isTerminal(string term)
        {
            for (int i = 0; i < termNb; i++)
                if (term == terminal[i])
                    return true;

            return false;

        }
        void AddToFirst(string symbol, int i)
        {
            FIRST[i, firstSize[i]] = symbol;
            firstSize[i]++;
        }

        void AddToFollow(string symbol, int i)
        {
            for (int j = 0; j < followSize[i]; j++)
                if (symbol == FOLLOW[i, j])
                    return;

            FOLLOW[i, followSize[i]] = symbol;
            followSize[i]++;
        }

      public  bool isLL1()
        {
            for (int i = 0; i < rulesNb; i++)
                for (int j = i + 1; j < rulesNb; j++)
                {
                    if (rule[i, 0] == rule[j, 0])
                    {
                        for (int x = 0; x < firstSize[i]; x++)
                            for (int y = 0; y < firstSize[j]; j++)
                                for (int z = 0; z < followSize[i]; z++)
                                    for (int w = 0; w < followSize[j]; w++)
                                    {
                                        if ((FIRST[i, x] == FIRST[j, y] && FIRST[i, x] != null && FIRST[j, y] != null) ||
                                           (FIRST[i, x] == FOLLOW[i, z] && FIRST[i, x] != null && FOLLOW[i, z] != null) ||
                                           (FIRST[i, x] == FOLLOW[j, w] && FIRST[i, x] != null && FOLLOW[j, w] != null) ||
                                            (FIRST[j, y] == FOLLOW[i, z] && FIRST[j, y] != null && FOLLOW[i, z] != null) ||
                                            (FIRST[j, y] == FOLLOW[j, w] && FIRST[j, y] != null && FOLLOW[j, w] != null) ||
                                            (FOLLOW[i, z] == FOLLOW[j, w] && FOLLOW[i, z] != null && FOLLOW[j, w] != null))

                                        {
                                            return false;
                                        }
                                    }



                    }
                }
            return true;
        }

       public void FIRSTandFOLLOW()
        {
            string[] initvect;

            for (int i = 0; i < rulesNb; i++)
            {
                if (rule[i, 1] != EPS)
                {
                    findFirst(rule[i, 0], i);
                    done1[i] = rule[i, 0];
                }
            }


            for (int j = 0; j < rulesNb; j++)
            {
                if (rule[j, 1] == EPS)
                {
                    
                    if (isDoneFollow(rule[j, 0]) == false)
                    {
                        initvect = new string[rulesNb];
                        init_index = 0;
                        findFollow(rule[j, 0], rule[j, 0], j,initvect);
                        done2[j] = rule[j, 0];
                        initvect = null;

                        FOLLOW[j, followSize[j]] = "$";
                        followSize[j]++;

                    }
                }

            }

           
            done2[0] = startSymb;

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\Work 3\Compilatoare\FF.txt"))
            {
                file.WriteLine("        FIRST set:");
                for (int x = 0; x < rulesNb; x++)
                {
                    if (FIRST[x, 0] != null)
                   { 
                    file.Write(rule[x, 0]);
                    file.Write("={");

                    for (int y = 0; y < firstSize[x]; y++)
                    {
                        if (FIRST[x, y] != null)
                        {
                            file.Write(FIRST[x, y]);
                            if (y != firstSize[x] - 1)
                                file.Write(",");
                        }
                    }
                    file.Write("}");
                    file.WriteLine();
                }
                }


                file.WriteLine("            FOLLOW set:");
                for (int x = 0; x < rulesNb; x++)
                {
                    if (FOLLOW[x, 0] != null)
                    {
                        file.Write(rule[x, 0]);
                        file.Write("={");

                        for (int y = 0; y < followSize[x]; y++)
                        {
                            if (FOLLOW[x, y] != null)
                            {
                                file.Write(FOLLOW[x, y]);
                                if (y != followSize[x] - 1)
                                    file.Write(",");
                            }
                        }
                        file.Write("}");
                        file.WriteLine();
                    }
                }

            }

         
        }

    

        public void CreateTable()
        {
       
            int k = 0;

            for (int i=0;i<rulesNb;i++)
                for(int j=0;j<firstSize[i];j++)
            {
               if(FIRST[i,j]!=null)
                    {
                        t[k].nonterminal = rule[i, 0];
                        t[k].terminal = FIRST[i, j];
                        t[k].rule = i;
                        k++;
                    }
            }

            for (int i = 0; i < rulesNb; i++)
            {
                for (int j = 0; j < followSize[i]; j++)
                {
                    if (FOLLOW[i, j] != null)
                    {
                        t[k].nonterminal = rule[i, 0];
                        t[k].terminal = FOLLOW[i, j];
                        t[k].rule = i;
                        k++;

                    }
                }
              
            }
            tableNb = k;
            Console.WriteLine(t);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\Work 3\Compilatoare\table_terminals.txt"))
            {
                for (int x = 0; x < tableNb; x++)
                {
                    file.Write(t[x].terminal);
                    if(x!=tableNb-1)
                    file.Write(" ");
                }

                
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\Work 3\Compilatoare\table_nonterminals.txt"))
            {
                for (int x = 0; x < tableNb; x++)
                {
                    file.Write(t[x].nonterminal);
                    if (x != tableNb - 1)
                        file.Write(" ");
                }


            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\Work 3\Compilatoare\table_rule.txt"))
            {
                for (int x = 0; x < tableNb; x++)
                {
                    file.Write(t[x].rule);
                    if (x != tableNb - 1)
                        file.Write(" ");
                }


            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\Work 3\Compilatoare\terminals_vector.txt"))
            {
                for (int x = 0; x < termNb; x++)
                {
                    file.Write(terminal[x]);
                    if (x != tableNb - 1)
                        file.Write(" ");
                }


            }
        }
    }


}
