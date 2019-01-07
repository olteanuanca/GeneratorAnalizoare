using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace ALL1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static string[] splitGrammar = new string[100];

        public string[] GETsplitGrammar
        {
            get
            {
                return splitGrammar;
            }
        }
       
        public MainWindow()
        {

            InitializeComponent();
           
            
        }

        private void button_upload_Click(object sender, RoutedEventArgs e)
        {
            

            Duration time = new Duration(TimeSpan.FromSeconds(5));
            DoubleAnimation progressbar_animation = new DoubleAnimation(200.0, time);
            progressbar_upload.BeginAnimation(ProgressBar.ValueProperty, progressbar_animation);


            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".txt";
            ofd.Filter = "txt files(*.txt) | *.txt";

            //extragerea componentelor gramaticii din fisierul de intrare
            if (ofd.ShowDialog() == true)
            {

                string filename = ofd.FileName;
                textbox_upload.Text = filename;
                textbox_content.Text = File.ReadAllText(filename);

              splitGrammar = textbox_content.Text.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);
                
               
            }

            
        }

        private void button_nextwindow2_Click(object sender, RoutedEventArgs e)
        {
            
            window2 w2 = new window2();
            App.Current.Windows[0].Close();
            w2.ShowDialog();
           
        }

       
    }
}
