using FormValidation.UILogic.Events;
using FormValidation.UILogic.Models;
using FormValidation.UILogic.Repositories;
using FormValidation.UILogic.Services;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace FormValidation.UILogic.ViewModels
{
    public class StateDetailPageViewModel : ViewModel
    {
        private IStateRepository _stateRepository;
        private readonly INavigationService _navService;
        private readonly IEventAggregator _eventAggregator;
        private State _state;
        private bool _loadingData;
        private int _numRowsUpdated;
        private CrudResult _crudResult;
        private string _errorMessage;
        private string _errorMessageTitle;

        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand NewStateCommand { get; private set; }
        public DelegateCommand UpdateStateCommand { get; private set; }
        public DelegateCommand DeleteStateCommand { get; private set; }
        public Action<object> TextBoxLostFocusAction { get; private set; }

        public StateDetailPageViewModel(IStateRepository stateRepository, INavigationService navService, IEventAggregator eventAggregator) {
            _stateRepository = stateRepository;
            _navService = navService;
            _eventAggregator = eventAggregator;
            GoBackCommand = new DelegateCommand(
                () => _navService.GoBack(),
                () => _navService.CanGoBack());
            NewStateCommand = new DelegateCommand(OnNewState, CanNewState);
            UpdateStateCommand = new DelegateCommand(OnUpdateState, CanUpdateState);
            DeleteStateCommand = new DelegateCommand(OnDeleteState, CanDeleteState);
            TextBoxLostFocusAction = OnTextBoxLostFocusAction;
        }

        [RestorableState]
        public State SelectedState {
            get {
                return _state;
            }
            private set {
                SetProperty(ref _state, value);
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

        public int NumRowsUpdated {
            get { return _numRowsUpdated; }
            private set { SetProperty(ref _numRowsUpdated, value); }
        }

        public CrudResult CrudResult {
            get { return _crudResult; }
            private set { SetProperty(ref _crudResult, value); }
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending) {
            base.OnNavigatedFrom(viewModelState, suspending);

            if (viewModelState != null) {
                AddEntityStateValue("stateErrorsCollection", SelectedState.GetAllErrors(), viewModelState);
            }
        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState) {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            if (viewModelState != null) {
                if (navigationMode == NavigationMode.Refresh) {
                    var stateErrorsCollection = RetrieveEntityStateValue<IDictionary<string, ReadOnlyCollection<string>>>("stateErrorsCollection", viewModelState);
                    if (stateErrorsCollection != null) {
                        SelectedState.SetAllErrors(stateErrorsCollection);
                    }
                }
            }

            // Note: Each time app selects from main page (StateListPage) detail page (StateDetailPage) is recreated.
            // Meaning that constructor is run and SelectedState is null.
            // If SuspendAndTerminate (e.g. debug mode) SelectedState is saved to SessionState (because of [RestorableState] attribute).
            // Therefore, if SelectedState has been saved, use it instead of doing GetStateAsync.
            if (SelectedState == null) {
                string errorMessage = string.Empty;
                int stateId = (int)navigationParameter;

                if (stateId == 0) {
                    SelectedState = new State();
                    SelectedState.ValidateProperties();
                    RunAllCanExecute();
                }
                else {
                    try {
                        LoadingData = true;
                        CrudResult = await _stateRepository.GetStateAsync(stateId);
                        SelectedState = JsonConvert.DeserializeObject<List<State>>(CrudResult.Content.ToString()).FirstOrDefault<State>();
                    }
                    catch (HttpRequestException ex) {
                        ErrorMessageTitle = ErrorMessagesHelper.HttpRequestExceptionError;
                        //TODO: Log stack trace to database here
                        ErrorMessage = string.Format("{0}", ex.Message);
                    }
                    finally {
                        LoadingData = false;
                    }
                    if (ErrorMessage != null && ErrorMessage != string.Empty) {
                        MessageDialog messageDialog = new MessageDialog(ErrorMessage, ErrorMessageTitle);
                        await messageDialog.ShowAsync();
                        _navService.GoBack();
                    }
                }
            }

            RunAllCanExecute();
        }

        // Enable New button when there is no Selected State
        private bool CanNewState() {
            return true;
        }

        // Disable Update button when there are Errors
        private bool CanUpdateState() {
			if (SelectedState != null) {
	            return SelectedState.Errors.Errors.Count == 0;
			}
			else {
				return false;
			}
        }

        // Enable Delete button when there is a Selected State
        private bool CanDeleteState() {
			if (SelectedState != null) {
	            return SelectedState.Id != 0;
			}
			else {
				return false;
			}            
        }

        // When New button is pressed
        private void OnNewState() {
            SelectedState = new State();
            SelectedState.ValidateProperties();
            RunAllCanExecute();
        }

        // When Update button is pressed
        private async void OnUpdateState() {
            string errorMessage = string.Empty;
            bool isCreating = false;

            SelectedState.ValidateProperties();
            var updateErrors = SelectedState.GetAllErrors().Values.SelectMany(pc => pc).ToList();

            if (updateErrors.Count == 0) {
                try {
                    LoadingData = true;
                    if (SelectedState.Id == 0) {
                        isCreating = true;
                        CrudResult = await _stateRepository.CreateStateAsync(SelectedState);
                        SelectedState = JsonConvert.DeserializeObject<List<State>>(CrudResult.Content.ToString()).FirstOrDefault<State>();
                    }
                    else {
                        CrudResult = await _stateRepository.UpdateStateAsync(SelectedState);
                    }
                }
                catch (ModelValidationException mvex) {
                    // there were server-side validation errors
                    DisplayStateErrorMessages(mvex.ValidationResult);
                }
                catch (HttpRequestException ex) {
                    ErrorMessageTitle = isCreating ? ErrorMessagesHelper.CreateAsyncFailedError : ErrorMessagesHelper.UpdateAsyncFailedError;
                    ErrorMessage = ex.Message;
                }
                finally {
                    LoadingData = false;
                    RunAllCanExecute();
                }

                if (ErrorMessage != null && ErrorMessage != string.Empty) {
                    MessageDialog messageDialog = new MessageDialog(ErrorMessage, ErrorMessageTitle);
                    await messageDialog.ShowAsync();
                    _navService.GoBack();
                }
            }
            else {
                RunAllCanExecute();
            }
        }

        // When Delete button is pressed
        private async void OnDeleteState() {
            var messageDialog = new MessageDialog("Delete this State?", "Delete confirmation");
            messageDialog.Commands.Add(new UICommand("Cancel", (command) =>
            {
            }));

            messageDialog.Commands.Add(new UICommand("Delete", async (command) =>
            {
                try {
                    LoadingData = true;
                    CrudResult = await _stateRepository.DeleteStateAsync(SelectedState.Id);
                    _eventAggregator.GetEvent<StateDeletedEvent>().Publish(SelectedState);
                }
                catch (HttpRequestException ex) {
                    ErrorMessageTitle = ErrorMessagesHelper.DeleteAsyncFailedError;
                    ErrorMessage = ex.Message;
                }
                finally {
                    LoadingData = false;
                    RunAllCanExecute();
                    _navService.GoBack();
                }
            }));

            messageDialog.DefaultCommandIndex = 0;
            await messageDialog.ShowAsync();

            if (ErrorMessage != null && ErrorMessage != string.Empty) {
                messageDialog = new MessageDialog(ErrorMessage, ErrorMessageTitle);
                await messageDialog.ShowAsync();
                _navService.GoBack();
            }
        }

        // If any Model rules broken, set SelectedCommonDataType Errors collection 
        // which are data bound to UI error textblocks.
        private void DisplayStateErrorMessages(ModelValidationResult modelValidationResult) {
            var stateUpdateErrors = new Dictionary<string, ReadOnlyCollection<string>>();

            // Property keys format: address.{Propertyname}
            foreach (var propkey in modelValidationResult.ModelState.Keys) {
                string propertyName = propkey.Substring(propkey.IndexOf('.') + 1); // strip off order. prefix

                // 'modelValidationResults.ModelState[propkey]' is the collection of string error messages
                // for the property. Later on in UILayer, FirstErrorConverter will display the one of the collection.
                // 'propertyName' will only occur once for each property in the Model so a new ReadOnlyCollection
                // can be created on each pass of the foreach loop.
                stateUpdateErrors.Add(propertyName, new ReadOnlyCollection<string>(modelValidationResult.ModelState[propkey]));
            }

            if (stateUpdateErrors.Count > 0) {
                SelectedState.Errors.SetAllErrors(stateUpdateErrors);
            }
        }

        // When a TextBox loses focus, RunAllCanExecute().
        private void OnTextBoxLostFocusAction(object parameter) {
            RunAllCanExecute();
        }

        // Run 'CanExecute' for all buttons
        private void RunAllCanExecute() {
            NewStateCommand.RaiseCanExecuteChanged();
            UpdateStateCommand.RaiseCanExecuteChanged();
            DeleteStateCommand.RaiseCanExecuteChanged();           
        }
    }
}
