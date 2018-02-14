using DatabaseManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        CancellationTokenSource Source;
        Task t;
        public MainPage()
        {
            this.InitializeComponent();
            Debug.WriteLine(ApplicationData.Current.LocalCacheFolder.Path);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (t == null)
            {
                Source = new CancellationTokenSource();
                t = Task.Run(() =>
                {
                    while(!Source.IsCancellationRequested)
                    {
                        FooDBContext.PlaceOrder();
                        Task.Delay(100).Wait();
                    }
                });
            }
            else
            {
                Source.Cancel();
                t = null;
            }
        }
    }
}
