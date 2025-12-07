using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.classes;

namespace UI.pages
{
    public partial class AdminPanel : Page
    {
        private UsersList _usersList;
        private User _currentEditedUser;
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
            if (txtbxName.Text == string.Empty ||
                txtbxLastName.Text == string.Empty ||
                txtbxEmail.Text == string.Empty ||
                txtbxLogin.Text == string.Empty)
            {
                txtblEnterError.Visibility = Visibility.Visible;
                return;
            }

            _currentEditedUser.Name = txtbxName.Text;
            _currentEditedUser.SecondName = txtbxLastName.Text;
            _currentEditedUser.LastName = txtbxLastName.Text;
            _currentEditedUser.Login = txtbxLogin.Text;
            _currentEditedUser.Email = txtbxEmail.Text;

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

        private void CancelEdit(object sender, RoutedEventArgs e)
        {
            _currentEditedUser = null;
            UserEditor.Visibility = Visibility.Collapsed;
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
