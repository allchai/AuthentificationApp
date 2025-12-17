using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.classes;
using System.Windows.Threading;

namespace UI.pages
{
    public partial class AdminPanel : Page
    {
        private UsersList _usersList;
        private User _currentEditedUser;
        private List<ErrorMessage> _errorMessages;
        private DispatcherTimer _errorMessageTimer;
        public AdminPanel()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }

        public void OnLoad(object sender, EventArgs e)
        {
            _usersList = new UsersList();
            _usersList.UpdateList();
            UsersListView.ItemsSource = _usersList.List;
            cmbbxFilter.ItemsSource = _usersList.GetAllOccupations();
            _errorMessages = new List<ErrorMessage>();

            _errorMessageTimer = new DispatcherTimer();
            _errorMessageTimer.Tick += new EventHandler(ErrorMessageTick);
            _errorMessageTimer.Interval = TimeSpan.FromSeconds(1);
        }

        public void EditUser(object sender, MouseButtonEventArgs e)
        {
            _currentEditedUser = ((ListViewItem)sender).Content as User;
            ShowUserEditor();
        }

        public void ShowUserEditor()
        {
            txtbxName.Text = _currentEditedUser.Name;
            txtbxLastName.Text = _currentEditedUser.LastName;
            txtbxSecondName.Text = _currentEditedUser.SecondName;
            txtbxLogin.Text = _currentEditedUser.Login;
            txtbxEmail.Text = _currentEditedUser.Email;
            cmbbxOccupation.SelectedItem = _currentEditedUser.Occupation;
            cmbbxOccupation.SelectedValue = _currentEditedUser.Occupation;

            UserEditor.Visibility = Visibility.Visible;
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            _currentEditedUser.Name = txtbxName.Text;
            _currentEditedUser.SecondName = txtbxLastName.Text;
            _currentEditedUser.LastName = txtbxLastName.Text;
            _currentEditedUser.Login = txtbxLogin.Text;
            _currentEditedUser.Email = txtbxEmail.Text;

            bool passValidation = Validation(_currentEditedUser);
            if (passValidation == false)
                return;

            if (txtbxPassword.Text == string.Empty)
            {
                _currentEditedUser.Password = txtbxPassword.Text;
                _usersList.UpdateUser(_currentEditedUser, true);
            }
            else
                _usersList.UpdateUser(_currentEditedUser, false);

            _currentEditedUser = null;
            _usersList.UpdateList();
            UsersListView.ItemsSource = _usersList.List;
        }

        private bool Validation(User user)
        {
            List<System.ComponentModel.DataAnnotations.ValidationResult> results = User.Validate(user);
            
            foreach (System.ComponentModel.DataAnnotations.ValidationResult result in results)
            {
                ErrorMessage message = new ErrorMessage()
                {
                    View = ErrorMessage.CreateView(result.ErrorMessage),
                    TimeRemaining = 5
                };

                _errorMessages.Add(message);
                ErrorMessagePanel.Children.Add(message.View);
            }

            if (results.Count > 0)
            {
                ErrorMessagePanel.Visibility = Visibility.Visible;
                _errorMessageTimer.Start();
            }

            return results.Count == 0 ? true : false;
        }

        private void ErrorMessageTick(object sender, EventArgs e)
        {
            List<ErrorMessage> toRemove = new List<ErrorMessage>();

            foreach (ErrorMessage message in _errorMessages)
            {
                message.TimeRemaining -= 1;

                if (message.TimeRemaining <= 0)
                    toRemove.Add(message);
            }

            foreach (ErrorMessage message in toRemove)
            {
                ErrorMessagePanel.Children.Remove(message.View);
                _errorMessages.Remove(message);
            }

            if (_errorMessages.Count == 0)
            {
                ErrorMessagePanel.Visibility = Visibility.Collapsed;
                _errorMessageTimer.Stop();
            }
        }

        private void CancelEdit(object sender, RoutedEventArgs e)
        {
            _currentEditedUser = null;
            UserEditor.Visibility = Visibility.Collapsed;
            _errorMessageTimer.Stop();
            _errorMessages = new List<ErrorMessage>();
            ErrorMessagePanel.Visibility = Visibility.Collapsed;
        }

        private void Return(object sender, RoutedEventArgs e)
            => NavigationService.GoBack();

        private void ClearFilters(object sender, RoutedEventArgs e)
        {
            cmbbxFilter.SelectedItem = null;
            txtbxSearch.Text = string.Empty;
            UsersListView.ItemsSource = _usersList.List;
        }

        private void ApplyFilters(object sender, RoutedEventArgs e)
        {
            List<User> filteredUsersList = new List<User>(_usersList.List);

            if (cmbbxFilter.SelectedItem != null)
                filteredUsersList = _usersList.ApplyFilter(filteredUsersList, (Occupation)cmbbxFilter.SelectedItem);
            if (txtbxSearch.Text != string.Empty)
                filteredUsersList = _usersList.Search(filteredUsersList, txtbxSearch.Text);

            UsersListView.ItemsSource = filteredUsersList;
        }
    }
}
