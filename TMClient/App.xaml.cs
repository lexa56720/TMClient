using ApiTypes.Shared;
using System.Configuration;
using System.Data;
using System.Windows;
using TMApi;

namespace TMClient
{



    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Api Api { get; set; }

        public static Configurator Settings= new Configurator("Resources/config.cfg", true);


    }
}