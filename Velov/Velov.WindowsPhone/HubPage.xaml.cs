﻿using Velov.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Device.Location; // Provides the GeoCoordinate class.
using Windows.Devices.Geolocation; //Provides the Geocoordinate class.
using Windows.UI.Xaml.Controls.Maps;
using Velov.Assets;
using System.Threading.Tasks;
using Velov.DataModel;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using System.Text;
using Windows.UI.Popups;


// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace Velov
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private Geolocator geo = null;
        private CoreDispatcher _cd;
        LocationIcon10m _locationIcon10m;
        LocationIcon100m _locationIcon100m;

        public HubPage()
        {
            this.InitializeComponent();
            this._locationIcon100m = new LocationIcon100m();
            this._locationIcon10m = new LocationIcon10m();
            myMap.Children.Add(_locationIcon10m);
            myMap.Children.Add(_locationIcon100m);

            _cd = Window.Current.CoreWindow.Dispatcher;
            myMap.Center = new Geopoint(new BasicGeoposition()
            {
                Latitude = 45.7600,
                Longitude = 4.8400
            });
            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            List<LyonStation> stations = DoWork.DeserializeStations();
            foreach (var station in stations)
            {
                var myButton = new Button(); 

                MapControl.SetLocation(myButton, new Geopoint(new BasicGeoposition()
                {
                    Latitude = station.latitude,
                    Longitude = station.longitude
                }));

                myButton.Tapped += Helper.DisplayRibbon(station, myButton);   
                myMap.Children.Add(myButton);
            }

            if (geo == null)
            {
                geo = new Geolocator();
            }
            if (geo != null)
            {
                geo.MovementThreshold = 3.0;
           //     geo.PositionChanged += new TypedEventHandler<Geolocator, PositionChangedEventArgs>(geo_PositionChanged);
            }
        }     

        async private void geo_PositionChanged(Geolocator sender, PositionChangedEventArgs e)
        {
            await _cd.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Geoposition pos = e.Position;
                var location = new Geopoint(new BasicGeoposition()
                {
                    Latitude = pos.Coordinate.Point.Position.Latitude,
                    Longitude = pos.Coordinate.Point.Position.Longitude
                });
                if (pos.Coordinate.Accuracy <= 10)
                {
                    MapControl.SetLocation(_locationIcon10m, location);             
                }
                else if (pos.Coordinate.Accuracy <= 100)
                {
                    MapControl.SetLocation(_locationIcon100m, location);
                }
                myMap.Center = location;   
            });
        }       
    }
}