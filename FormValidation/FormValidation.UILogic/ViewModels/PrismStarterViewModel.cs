using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;

namespace FormValidation.UILogic.ViewModels
{
    public class FormValidationViewModel : ViewModel
    {
        private INavigationService _navigationService;

        public FormValidationViewModel(INavigationService navigationService) {
            _navigationService = navigationService;
        }
    }
}
