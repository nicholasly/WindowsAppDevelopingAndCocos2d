using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Animal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public delegate string ComingEvevtHander(object sender, RoutedEventArgs e);

        public class Cat
        {
            public string Speak(Object sender, RoutedEventArgs e)
            {
                string temp = "Cat: I am a cat.";
                return temp;
            }
        }

        public class Dog
        {
            public string Speak(Object sender, RoutedEventArgs e)
            {
                string temp = "Dog: I am a Dog.";
                return temp;
            }
        }

        public class Pig
        {
            public string Speak(Object sender, RoutedEventArgs e)
            {
                string temp = "Pig: I am a pig.";
                return temp;
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            string temp = this.Input.Text;
            switch(temp)
            {
                case "cat":
                    this.Output.Text += "Cat: I am a cat.\n";
                    break;
                case "dog":
                    this.Output.Text += "Dog: I am a dog.\n";
                    break;
                case "pig":
                    this.Output.Text += "Pig: I am a pig.\n";
                    break;
            }
            this.Input.Text = "";
        }

        private void Speak_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            int random = rnd.Next(1, 4);
            switch(random)
            {
                case 1:
                    Pig pig = new Pig();
                    string t1 = pig.Speak(sender, e);
                    this.Output.Text += t1 + "\n";
                    break;
                case 2:
                    Cat cat = new Cat();
                    string t2 = cat.Speak(sender, e);
                    this.Output.Text += t2 + "\n";
                    break;
                case 3:
                    Dog dog = new Dog();
                    string t3 = dog.Speak(sender, e);
                    this.Output.Text += t3 + "\n";
                    break;
            }
           
        }

        private void Output_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
    }
}
