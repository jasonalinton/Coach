using System;
using System.Collections.Generic;
using Coach.Mobile.Views.Briefing;
using Xamarin.Forms;

namespace Coach.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("briefing/detail", typeof(BriefingDetailPage));
        }
    }
}
