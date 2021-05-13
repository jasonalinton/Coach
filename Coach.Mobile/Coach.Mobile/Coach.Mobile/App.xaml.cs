using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Coach.Mobile.Services;
using Coach.Mobile.Views;
using Coach.Mobile.Data.Repository;
using Coach.Mobile.Models.Helper;
using Coach.Mobile.Models.InventoryItem;

namespace Coach.Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        public static CoachRepository  Repository { get; } = CoachRepository.Instance;

        protected override async void OnStart()
        {
            await InventoryItemModel.InitializeModels();
            await TypeModel.InitializeModels();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
