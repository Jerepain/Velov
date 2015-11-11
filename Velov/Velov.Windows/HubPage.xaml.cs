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
using Velov.Common;
using Bing.Maps;
using Windows.UI.Popups;
using Velov.DataModel;
using Newtonsoft.Json;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Velov.Assets;
using Windows.Data.Xml.Dom;
using System.Net;
using System.Xml;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using Windows.UI;
using Nito.AsyncEx;

// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955
//8554d38cbccde6f2ca5db7fdd689f1b588ce7786 api jcdecaux
//GET https://api.jcdecaux.com/vls/v1/stations?contract={contract_name}&apiKey={api_key}

namespace Velov
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private string json { get; set; }
        //http://blog.jerrynixon.com/2012/06/windows-8-how-to-read-files-in-winrt.html
        //private async void ProjectFile()
        //{
        //    var _Folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
        //    _Folder = await _Folder.GetFolderAsync(@"Assets");
        //    var _File = await _Folder.GetFileAsync(@"Lyon.json");
        //    json = await Windows.Storage.FileIO.ReadTextAsync(_File);
        //}

        public HubPage()
        {
            this.InitializeComponent();

            List<LyonStation> stations = DoWork.DeserializeStations();
            foreach (var station in stations)
            {
                var myButton = new Button();
                //AsyncContext.Run(async () =>
                //{
                //    string js = "";
                //    HttpClient httpClient = new HttpClient();
                //    HttpRequestMessage msg = new HttpRequestMessage(new HttpMethod("GET"), new Uri("https://api.jcdecaux.com/vls/v1/stations/" + station.number + "?contract=Lyon&apiKey=8554d38cbccde6f2ca5db7fdd689f1b588ce7786"));
                //    msg.Content = new HttpStringContent(js);
                //    msg.Content.Headers.ContentType = new HttpMediaTypeHeaderValue("application/json");
                //    HttpResponseMessage response = await httpClient.SendRequestAsync(msg).AsTask();
                //    string s = await response.Content.ReadAsStringAsync();
                //    LyonStation lyonStation = Helper.Deserialize<LyonStation>(s);
                //    pin.Text = lyonStation.available_bikes.ToString() + "/" + (lyonStation.available_bike_stands + lyonStation.available_bikes).ToString();                    
           
                //});
                MapLayer.SetPosition(myButton, new Location(station.latitude, station.longitude));

                myButton.Tapped += Helper.DisplayRibbon(station, myButton);           
                //myButton.PointerEntered += myButton_PointerEntered;
                myButton.PointerMoved += myButton_PointerEntered;
                myButton.PointerExited += myButton_PointerExited;

                myMap.Children.Add(myButton);
            }
        }

        

        private void myButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var bouton = (Button)sender;
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Colors.Black);
            bouton.Background = myBrush;
        }

        void myButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var bouton = (Button)sender;
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Colors.Gray);
            bouton.Background = myBrush;
        }
    }
}
