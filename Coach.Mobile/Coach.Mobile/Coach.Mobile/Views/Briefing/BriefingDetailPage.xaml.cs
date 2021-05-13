using Coach.Mobile.Models.Briefing;
using Coach.Mobile.Models.Helper;
using Coach.Mobile.ViewModels.Briefing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Coach.Mobile.Views.Briefing
{
    public partial class BriefingDetailPage : ContentPage
    {
        ViewTypes viewType;

        public BriefingDetailPage()
        {
            InitializeComponent();

            this.VM = new BriefingDetailPageVM();
            this.ViewType = ViewTypes.Edit;
            this.BindingContext = this.VM;
        }

        public BriefingDetailPage(BriefingModel briefingModel)
        {
            InitializeComponent();

            this.VM = new BriefingDetailPageVM(briefingModel);
            this.ViewType = ViewTypes.View;
            this.BindingContext = this.VM;
        }

        public BriefingDetailPageVM VM { get; set; }

        ViewTypes ViewType
        {
            get { return viewType; }
            set
            {
                if (value == ViewTypes.View)
                {
                    this.VM.ToolbarButtonText = "Edit";
                    this.EditorStackLayout.IsVisible = false;
                    this.ViewerStackLayout.IsVisible = true;
                }
                else if (value == ViewTypes.Edit)
                {
                    this.VM.ToolbarButtonText = "Save";
                    this.ViewerStackLayout.IsVisible = false;
                    this.EditorStackLayout.IsVisible = true;
                }
                this.viewType = value;
            }
        }

        enum ViewTypes
        {
            View,
            Edit
        }

        async void OnToolbarButtonClicked(object sender, EventArgs e)
        {
            if (this.ViewType == ViewTypes.View) // If "Edit" button is clicked
            {
                this.ViewType = ViewTypes.Edit;
            }
            else if (this.ViewType == ViewTypes.Edit) // If "Save button is clicked
            {
                this.ViewType = ViewTypes.View;

                this.VM.BriefingModel = this.VM.BriefingEditModel.Clone();
                await this.VM.BriefingModel.Save();
            }
        }
    }
}