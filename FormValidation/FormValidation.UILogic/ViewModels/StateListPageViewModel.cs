using FormValidation.UILogic.Models;
using FormValidation.UILogic.Repositories;
using FormValidation.UILogic.Services;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Windows.UI.Popups;
using Newtonsoft.Json;

namespace FormValidation.UILogic.ViewModels
{
    public class StateListPageViewModel : ViewModel
    {
        private IStateRepository _stateRepository;
        private readonly INavigationService _navService;
        private readonly IEventAggregator _eventAggregator; 
        private IReadOnlyCollection<State> _stateList;
        private bool _loadingData;
        private string _errorMessage;
        private string _errorMessageTitle;
 
        public StateListPageViewModel(IStateRepository stateRepository, INavigationService navService, IEventAggregator eventAggregator) {
            _stateRepository = stateRepository;
            _navService = navService;
            _eventAggregator = eventAggregator;
            NavCommand = new DelegateCommand<State>(OnNavCommand);
            StateDetailNavCommand = new DelegateCommand(() => _navService.Navigate("StateDetail", 0));
       }

        public DelegateCommand<State> NavCommand { get; set; }
        public DelegateCommand StateDetailNavCommand { get; set; }

        public IReadOnlyCollection<State> StateList { 
            get {
                return _stateList;
            }
            private set {
                SetProperty(ref _stateList, value);
            }
        }

        public bool LoadingData {
            get { return _loadingData; }
            private set { SetProperty(ref _loadingData, value); }
        }

        public string ErrorMessage {
            get { return _errorMessage; }
            private set { SetProperty(ref _errorMessage, value); }
        }

        public string ErrorMessageTitle {
            get { return _errorMessageTitle; }
            private set { SetProperty(ref _errorMessageTitle, value); }
        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState) {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            ErrorMessageTitle = string.Empty;
            ErrorMessage = string.Empty;

            try {
                LoadingData = true;
                CrudResult crudResult = await _stateRepository.GetStatesAsync();
                StateList = JsonConvert.DeserializeObject<List<State>>(crudResult.Content.ToString());
            }
            catch (HttpRequestException ex) {
                ErrorMessageTitle = ErrorMessagesHelper.GetAllAsyncFailedError;
                ErrorMessage = string.Format("{0}{1}", Environment.NewLine, ex.Message);
            }
            catch (Exception ex) {
                ErrorMessageTitle = ErrorMessagesHelper.ExceptionError;
                ErrorMessage = string.Format("{0}{1}", Environment.NewLine, ex.Message);
            }
            finally {
                LoadingData = false;
            }
            if (ErrorMessage != null && ErrorMessage != string.Empty) {
                MessageDialog messageDialog = new MessageDialog(ErrorMessage, ErrorMessageTitle);
                await messageDialog.ShowAsync();
            }
        }

        private void OnNavCommand(State state) {
            _navService.Navigate("StateDetail", state.Id);
        }


    }
}
