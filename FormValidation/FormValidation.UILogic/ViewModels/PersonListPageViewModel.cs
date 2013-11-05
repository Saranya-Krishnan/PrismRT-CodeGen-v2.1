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
    public class PersonListPageViewModel : ViewModel
    {
        private IPersonRepository _personRepository;
        private readonly INavigationService _navService;
        private readonly IEventAggregator _eventAggregator; 
        private IReadOnlyCollection<Person> _personList;
        private bool _loadingData;
        private string _errorMessage;
        private string _errorMessageTitle;
 
        public PersonListPageViewModel(IPersonRepository personRepository, INavigationService navService, IEventAggregator eventAggregator) {
            _personRepository = personRepository;
            _navService = navService;
            _eventAggregator = eventAggregator;
            NavCommand = new DelegateCommand<Person>(OnNavCommand);
            PersonDetailNavCommand = new DelegateCommand(() => _navService.Navigate("PersonDetail", 0));
       }

        public DelegateCommand<Person> NavCommand { get; set; }
        public DelegateCommand PersonDetailNavCommand { get; set; }

        public IReadOnlyCollection<Person> PersonList { 
            get {
                return _personList;
            }
            private set {
                SetProperty(ref _personList, value);
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
                CrudResult crudResult = await _personRepository.GetPeopleAsync();
                PersonList = JsonConvert.DeserializeObject<List<Person>>(crudResult.Content.ToString());
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

        private void OnNavCommand(Person person) {
            _navService.Navigate("PersonDetail", person.Id);
        }


    }
}
