using System.Collections.ObjectModel;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ALS_MOB_100
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListLogViewer : ContentPage
    {

        ObservableCollection<LogFileList> LogFileList = new ObservableCollection<LogFileList>();
        public static string SelectedFile;
        public static string SelectedFileName;
        public ListLogViewer()
        {
            InitializeComponent();
            FileList.ItemsSource = LogFileList;
            GetFileList();
        }
        void GetFileList()
        {

            var files = System.IO.Directory.GetFiles(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)));
            for (int compteur = 0; compteur < files.Length; compteur++)
            {
                string fullpath = files[compteur];
                var temp = files[compteur].Split('/');
                string fullname = temp[temp.Length - 1];
                temp = fullname.Split('.');
                string name = temp[0];
                LogFileList.Add(new LogFileList(fullpath, name));
            }

        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var n = (LogFileList)e.Item;
            SelectedFile = n.Path;
            SelectedFileName = n.FileName;
            await Navigation.PushAsync(new Logviewer(), false);

            //foreach (var dev in LogFileList)
            //{
            //    if (n.Path == dev.Path)
            //    {
            //        SelectedFile = n.Path;
            //        SelectedFileName = n.FileName;
            //        await Navigation.PushAsync(new Logviewer(), false);
            //    }
            //}
        }


    }
}