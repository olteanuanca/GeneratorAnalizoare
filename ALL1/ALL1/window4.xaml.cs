using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Dynamic;


namespace ALL1
{
    /// <summary>
    /// Interaction logic for window4.xaml
    /// </summary>
    public partial class window4 : Window
    {
        public Grammar G = Grammar.Instance();
        public window4()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CodeGenerator CG = new CodeGenerator();
            CG.AddFields();
            CG.AddEntryPoint();
            CG.InitializeVars();
            CG.AddMethods();
            CG.GenerateCSharpCode("1.txt");

            CG.CompileCSharpCode("1.txt","2.exe");
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //G.DirectorSymbols();
            G.CreateTable();

         
        }
    }
}
