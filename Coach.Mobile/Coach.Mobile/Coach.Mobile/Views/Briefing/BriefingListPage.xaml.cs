using System;
using System.Collections.Generic;
using Coach.Mobile.Models.Briefing;
using Coach.Mobile.ViewModels.Briefing;
using Xamarin.Forms;

namespace Coach.Mobile.Views.Briefing
{
    public partial class BriefingListPage : ContentPage
    {
        public BriefingListPage()
        {
            InitializeComponent();

            this.BriefingVM = new BriefingVM();
            this.BindingContext = this.BriefingVM;
        }

        public BriefingVM BriefingVM { get; set; }

        void OnNewBriefingClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("briefing/detail");
        }

        async void OnBriefingSelected(object sender, EventArgs e)
        {
            var selectedBriefing = ((BriefingModel)(sender as ListView).SelectedItem);
            if (selectedBriefing != null)
            {
                await Navigation.PushAsync(new BriefingDetailPage(selectedBriefing));
            }
            //Shell.Current.GoToAsync($"///briefing/detail?briefingID={selectedBriefing.SQLiteID}");
        }

        async void OnDelete(object sender, EventArgs e)
        {
            ((BriefingModel)((MenuItem)sender).BindingContext).Delete();
            await this.BriefingVM.RefreshBriefingGroups();
        }

        protected override async void OnAppearing()
        {
            await this.BriefingVM.RefreshBriefingGroups();

            this.BriefingListView.SelectedItem = null;
            base.OnAppearing();
        }
    }
}
