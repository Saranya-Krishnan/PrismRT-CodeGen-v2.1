using Microsoft.Practices.Prism.StoreApps;
using FormValidation.UILayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Globalization;

namespace FormValidation.UILayer.Views
{
    public sealed partial class PersonListPage : VisualStateAwarePage
    {
        private double _scrollViewerVerticalOffset;

        public PersonListPage() {
            this.InitializeComponent();
        }

        private void itemsListView_Loaded(object sender, RoutedEventArgs e) {
            // Find the ScrollViewer inside the ListView
            var scrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemsListView);

            if (scrollViewer != null) {
                scrollViewer.Visibility = Visibility.Visible;
                if (scrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible) {
                    scrollViewer.ChangeView(null, _scrollViewerVerticalOffset, null);
                }
                else {
                DependencyPropertyChangedHelper helper = new DependencyPropertyChangedHelper(scrollViewer, "ComputedVerticalScrollBarVisibility");
                    helper.PropertyChanged += ScrollBarVerticalVisibilityChanged;
                }
            }
        }

        private void ScrollBarVerticalVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e) {
            var helper = (DependencyPropertyChangedHelper)sender;

            var scrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemsListView);

            if (((Visibility)e.NewValue) == Visibility.Visible) {
                // Update the Vertical offset
                scrollViewer.ChangeView(null, _scrollViewerVerticalOffset, null);
                helper.PropertyChanged -= ScrollBarVerticalVisibilityChanged;
            };
        }

        protected override void SaveState(Dictionary<string, object> pageState) {
            if (pageState == null) return;

            base.SaveState(pageState);

            var scrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemsListView);
            if (scrollViewer != null) {
                pageState["personItemsSvVerticalOffset"] = scrollViewer.VerticalOffset;
            }
        }

        protected override void LoadState(object navigationParameter, Dictionary<string, object> pageState) {
            if (pageState == null) return;

            base.LoadState(navigationParameter, pageState);

            if (pageState.ContainsKey("personItemsSvVerticalOffset")) {
                _scrollViewerVerticalOffset = double.Parse(pageState["personItemsSvVerticalOffset"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            }
        }
    }
}
