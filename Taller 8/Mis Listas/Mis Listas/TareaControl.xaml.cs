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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Mis_Listas
{
    public sealed partial class TareaControl : UserControl
    {
        public int list { get; set; }
        public int task { get; set; }

        public event EventHandler<deleteTaskEventArgs> deleteTask;
        public event EventHandler<checkTaskEventArgs> checkTask;

        public TareaControl(string text = "", int list = 0, int task = 0)
        {
            this.InitializeComponent();
            if(text != "")
            {
                setText(text);
            }

            this.list = list; 
            this.task = task;
        }

        public void setText(string text)
        {
            lblTask.Text = text;
        }

        private void btnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (deleteTask != null)
            {
                deleteTaskEventArgs ea = new deleteTaskEventArgs(list, task);
                deleteTask(sender, ea);
            }
        }

        private void btnCheckTask_Click(object sender, RoutedEventArgs e)
        {
            if (checkTask != null)
            {
                checkTaskEventArgs ea = new checkTaskEventArgs(list, task);
                checkTask(sender, ea);
            }
        }
    }
}
