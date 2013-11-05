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
    public class PersonDetailPageViewModel : ViewModel
    {
        private IPersonRepository _personRepository;
        private readonly INavigationService _navService;
        private readonly IEventAggregator _eventAggregator;
        private Person _person;
        private bool _loadingData;
        private int _numRowsUpdated;
        private CrudResult _crudResult;
        private string _errorMessage;
        private string _errorMessageTitle;

        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand NewPersonCommand { get; private set; }
        public DelegateCommand UpdatePersonCommand { get; private set; }
        public DelegateCommand DeletePersonCommand { get; private set; }
        public Action<object> TextBoxLostFocusAction { get; private set; }

        public PersonDetailPageViewModel(IPersonRepository personRepository, INavigationService navService, IEventAggregator eventAggregator) {
            _personRepository = personRepository;
            _navService = navService;
            _eventAggregator = eventAggregator;
            GoBackCommand = new DelegateCommand(
                () => _navService.GoBack(),
                () => _navService.CanGoBack());
            NewPersonCommand = new DelegateCommand(OnNewPerson, CanNewPerson);
            UpdatePersonCommand = new DelegateCommand(OnUpdatePerson, CanUpdatePerson);
            DeletePersonCommand = new DelegateCommand(OnDeletePerson, CanDeletePerson);
            TextBoxLostFocusAction = OnTextBoxLostFocusAction;
        }

        [RestorableState]
        public Person SelectedPerson {
            get {
                return _person;
            }
            private set {
                SetProperty(ref _person, value);
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
                AddEntityStateValue("personErrorsCollection", SelectedPerson.GetAllErrors(), viewModelState);
            }
        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState) {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            if (viewModelState != null) {
                if (navigationMode == NavigationMode.Refresh) {
                    var personErrorsCollection = RetrieveEntityStateValue<IDictionary<string, ReadOnlyCollection<string>>>("personErrorsCollection", viewModelState);
                    if (personErrorsCollection != null) {
                        SelectedPerson.SetAllErrors(personErrorsCollection);
                    }
                }
            }

            // Note: Each time app selects from main page (PersonListPage) detail page (PersonDetailPage) is recreated.
            // Meaning that constructor is run and SelectedPerson is null.
            // If SuspendAndTerminate (e.g. debug mode) SelectedPerson is saved to SessionState (because of [RestorableState] attribute).
            // Therefore, if SelectedPerson has been saved, use it instead of doing GetPersonAsync.
            if (SelectedPerson == null) {
                string errorMessage = string.Empty;
                int personId = (int)navigationParameter;

                if (personId == 0) {
                    SelectedPerson = new Person();
                    SelectedPerson.ValidateProperties();
                    RunAllCanExecute();
                }
                else {
                    try {
                        LoadingData = true;
                        CrudResult = await _personRepository.GetPersonAsync(personId);
                        SelectedPerson = JsonConvert.DeserializeObject<List<Person>>(CrudResult.Content.ToString()).FirstOrDefault<Person>();
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

        // Enable New button when there is no Selected Person
        private bool CanNewPerson() {
            return true;
        }

        // Disable Update button when there are Errors
        private bool CanUpdatePerson() {
			if (SelectedPerson != null) {
	            return SelectedPerson.Errors.Errors.Count == 0;
			}
			else {
				return false;
			}
        }

        // Enable Delete button when there is a Selected Person
        private bool CanDeletePerson() {
			if (SelectedPerson != null) {
	            return SelectedPerson.Id != 0;
			}
			else {
				return false;
			}            
        }

        // When New button is pressed
        private void OnNewPerson() {
            SelectedPerson = new Person();
            SelectedPerson.ValidateProperties();
            RunAllCanExecute();
        }

        // When Update button is pressed
        private async void OnUpdatePerson() {
            string errorMessage = string.Empty;
            bool isCreating = false;

            SelectedPerson.ValidateProperties();
            var updateErrors = SelectedPerson.GetAllErrors().Values.SelectMany(pc => pc).ToList();

            if (updateErrors.Count == 0) {
                try {
                    LoadingData = true;
                    if (SelectedPerson.Id == 0) {
                        isCreating = true;
                        CrudResult = await _personRepository.CreatePersonAsync(SelectedPerson);
                        SelectedPerson = JsonConvert.DeserializeObject<List<Person>>(CrudResult.Content.ToString()).FirstOrDefault<Person>();
                    }
                    else {
                        CrudResult = await _personRepository.UpdatePersonAsync(SelectedPerson);
                    }
                }
                catch (ModelValidationException mvex) {
                    // there were server-side validation errors
                    DisplayPersonErrorMessages(mvex.ValidationResult);
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
        private async void OnDeletePerson() {
            var messageDialog = new MessageDialog("Delete this Person?", "Delete confirmation");
            messageDialog.Commands.Add(new UICommand("Cancel", (command) =>
            {
            }));

            messageDialog.Commands.Add(new UICommand("Delete", async (command) =>
            {
                try {
                    LoadingData = true;
                    CrudResult = await _personRepository.DeletePersonAsync(SelectedPerson.Id);
                    _eventAggregator.GetEvent<PersonDeletedEvent>().Publish(SelectedPerson);
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
        private void DisplayPersonErrorMessages(ModelValidationResult modelValidationResult) {
            var personUpdateErrors = new Dictionary<string, ReadOnlyCollection<string>>();

            // Property keys format: address.{Propertyname}
            foreach (var propkey in modelValidationResult.ModelState.Keys) {
                string propertyName = propkey.Substring(propkey.IndexOf('.') + 1); // strip off order. prefix

                // 'modelValidationResults.ModelState[propkey]' is the collection of string error messages
                // for the property. Later on in UILayer, FirstErrorConverter will display the one of the collection.
                // 'propertyName' will only occur once for each property in the Model so a new ReadOnlyCollection
                // can be created on each pass of the foreach loop.
                personUpdateErrors.Add(propertyName, new ReadOnlyCollection<string>(modelValidationResult.ModelState[propkey]));
            }

            if (personUpdateErrors.Count > 0) {
                SelectedPerson.Errors.SetAllErrors(personUpdateErrors);
            }
        }

        // When a TextBox loses focus, RunAllCanExecute().
        private void OnTextBoxLostFocusAction(object parameter) {
            RunAllCanExecute();
        }

        // Run 'CanExecute' for all buttons
        private void RunAllCanExecute() {
            NewPersonCommand.RaiseCanExecuteChanged();
            UpdatePersonCommand.RaiseCanExecuteChanged();
            DeletePersonCommand.RaiseCanExecuteChanged();           
        }
    }
}
