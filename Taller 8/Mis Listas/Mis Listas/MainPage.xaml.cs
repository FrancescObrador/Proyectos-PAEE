using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core.Preview;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static System.Net.Mime.MediaTypeNames;
using Windows.UI.Popups;
using Windows.ApplicationModel.Resources;
using Mis_Listas.Models;
using Windows.Storage;
using Windows.ApplicationModel;
using Newtonsoft.Json;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Mis_Listas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ResourceLoader textos;
        private List<cList> lists;

        public MainPage()
        {
            this.InitializeComponent();

            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += this.OnCloseRequest;

            this.textos = new Windows.ApplicationModel.Resources.ResourceLoader();
            ApplicationView appView = ApplicationView.GetForCurrentView();
            appView.Title = textos.GetString("noListas");

            ApplicationView.PreferredLaunchViewSize = new Size(600, 750);
            ApplicationView.PreferredLaunchWindowingMode =
            ApplicationViewWindowingMode.PreferredLaunchViewSize;

            lists = new List<cList>();
            loadData();

        }

        private async void OnCloseRequest(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            e.Handled = true;
            ContentDialog msgBox = new ContentDialog
            {
                Title = textos.GetString("txtAtencion"),
                Content = textos.GetString("txtSeguro"),
                PrimaryButtonText = textos.GetString("txtSi"),
                CloseButtonText = textos.GetString("txtNo")
            };

            var res = await msgBox.ShowAsync();
            if(res == ContentDialogResult.Primary)
            {
                saveData();
                Windows.UI.Xaml.Application.Current.Exit();
            }
        }

        async private void loadData()
        {
            StorageFolder dataDir = await Package.Current.InstalledLocation.GetFolderAsync(@"Assets\Data");
            StorageFile dataFile = await dataDir.GetFileAsync("data.json");
            string cadena = await Windows.Storage.FileIO.ReadTextAsync(dataFile);
            lists = JsonConvert.DeserializeObject<List<cList>>(cadena);

            if(lists.Count >= 1)
            {
                slctlist.Items.Clear();
                slctlist.ItemsSource = lists;
                slctlist.DisplayMemberPath = "name";
                slctlist.SelectedIndex = 0;
                ApplicationView.GetForCurrentView().Title = lists[0].name;
                LoadTasks(0);
            }
        }

        private void LoadTasks(int indice)
        {
            cList list = lists.ElementAt(indice);
            pnlList.Children.Clear();
            if (list.tasks.Count > 0 && list.tasks.Where(t => t.finished = false).Count() > 0)
            {
                lblXdef.Visibility = Visibility.Collapsed;
                int numTask = 0;
                foreach (cTask task in list.tasks)
                {
                    if (!task.finished)
                    {
                        TareaControl auxControl = new TareaControl(task.text, slctlist.SelectedIndex, numTask);
                        auxControl.deleteTask += new EventHandler<deleteTaskEventArgs>(deleteTask);
                        auxControl.checkTask += new EventHandler<checkTaskEventArgs>(checkTask);
                        pnlList.Children.Add(auxControl);
                        numTask++;
                    }
                }
            }
            else
            {
                lblXdef.Visibility = Visibility.Visible;
            }
        }

        private void slctlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb.Items.Count > 0 && typeof(cList) == cb.Items[cb.SelectedIndex].GetType()) 
            {
                ApplicationView.GetForCurrentView().Title = lists[cb.SelectedIndex].name;
                LoadTasks(cb.SelectedIndex);
            }
        }

        private void btnNuevaLista_Click(object sender, RoutedEventArgs e)
        {
            tituloNuevalista.Text = textos.GetString("tituloNuevaLista");
            lblNuevalista.Text = textos.GetString("lblNuevaLista");
            txt_btnAdd_newList.Text = textos.GetString("btnAdd");
            txt_btnCancel_newList.Text = textos.GetString("btnCancel");
            txt_newList_name.Text = "";
            txt_newList_name.PlaceholderText = textos.GetString("ttpNuevaLista");
            txt_newList_name.PlaceholderForeground = new SolidColorBrush(Colors.LightGray);
        }

        private void btnAdd_newList_Click(object sender, RoutedEventArgs e)
        {
            if(txt_newList_name.Text != "")
            {
                flyAddList.Hide();
                cList lista = new cList();
                lista.name = txt_newList_name.Text;
                lista.color = "";
                lista.date = DateTime.Now;
                lista.visible = true;
                lists.Add(lista);
                slctlist.ItemsSource = lists;
                slctlist.DisplayMemberPath = "name";
                slctlist.UpdateLayout();
            }
            else
            {
                txt_newList_name.PlaceholderText = textos.GetString("errNuevaLista");
                txt_newList_name.PlaceholderForeground = new SolidColorBrush(Colors.Red);
            }
        }

        private void btnCancel_newList_Click(object sender, RoutedEventArgs e)
        {
            flyAddList.Hide();
        }

        private async void btnBorralista_Click(object sender, RoutedEventArgs e)
        {
            cList lista = lists.ElementAt(slctlist.SelectedIndex);
            ContentDialog msgBox = new ContentDialog
            {
                Title = textos.GetString("txtAtencion"),
                Content = String.Format(textos.GetString("txtSeguroBorrar"), lista.name),
                PrimaryButtonText = textos.GetString("txtSi"),
                CloseButtonText = textos.GetString("txtNo")
            };

            var res = await msgBox.ShowAsync();
            if(res == ContentDialogResult.Primary)
            {
                if(lists.Count > 1)
                {
                    lists.RemoveAt(slctlist.SelectedIndex);
                    slctlist.ItemsSource = lists;
                    slctlist.DisplayMemberPath = "name";
                    slctlist.UpdateLayout();
                    slctlist.SelectedIndex = 0;
                }
            }
        }

        private void btnNuevaTarea_Click(object sender, RoutedEventArgs e)
        {
            tituloNuevaTarea.Text = textos.GetString("tituloNuevaTarea");
            lblNuevaTarea.Text = textos.GetString("lblNuevaTarea");
            txt_btnAdd_newTask.Text = textos.GetString("btnAdd");
            txt_btnCancel_newTask.Text = textos.GetString("btnCancel");

            txt_newTask_name.Text = "";
            txt_newTask_name.PlaceholderText = textos.GetString("ttpNuevaTarea");
            txt_newTask_name.PlaceholderForeground = new SolidColorBrush(Colors.LightGray);
        }

        private void btnAdd_newTask_Click(object sender, RoutedEventArgs e)
        {
            if(txt_newTask_name.Text != "")
            {
                flyAddTask.Hide();
                cTask tarea = new cTask();
                tarea.text = txt_newTask_name.Text;
                tarea.date = DateTime.Now;
                tarea.visible = true;
                tarea.expired = null;
                tarea.finished = false;
                lists[slctlist.SelectedIndex].tasks.Add(tarea);
                LoadTasks(slctlist.SelectedIndex);
            }
            else
            {
                txt_newTask_name.PlaceholderText = textos.GetString("errNuevaTarea");
                txt_newTask_name.PlaceholderForeground = new SolidColorBrush(Colors.Red);
            }
        }

        private void btnCancel_newTask_Click(object sender, RoutedEventArgs e)
        {
            flyAddTask.Hide();
        }

        async private void deleteTask(object sender, deleteTaskEventArgs e)
        {
            cTask tarea = lists.ElementAt(e.list).tasks.ElementAt(e.task);
            ContentDialog msgBox = new ContentDialog
            {
                Title = textos.GetString("txtAtention"),
                Content = String.Format(textos.GetString("txtSeguroBorrarTarea"), tarea.text),
                PrimaryButtonText = textos.GetString("txtSi"),
                CloseButtonText = textos.GetString("txtNo")
            };

            var res = await msgBox.ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                lists.ElementAt(e.list).tasks.RemoveAt(e.task);
                LoadTasks(e.list);
            }
        }

        private void checkTask(object sender, checkTaskEventArgs e)
        {
            lists.ElementAt(e.list).tasks.ElementAt(e.task).finished = true;
            LoadTasks(e.list);
        }

        async private void saveData()
        {
            StorageFolder appDir = ApplicationData.Current.LocalFolder;
            StorageFile tmpFile = await appDir.CreateFileAsync("data.json", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(tmpFile, JsonConvert.SerializeObject(lists));
            StorageFolder dataDir = await Package.Current.InstalledLocation.GetFolderAsync(@"Assets\Data");
            await tmpFile.MoveAsync(dataDir, "data.json", NameCollisionOption.ReplaceExisting);
        }
    }
}
