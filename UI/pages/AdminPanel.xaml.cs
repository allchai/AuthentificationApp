using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.classes;

namespace UI.pages
{
    public partial class AdminPanel : Page
    {
        private DataBase _db;
        private User _editedUser;
        private List<User> _usersList;
        private List<User> _currentUserList;
        public AdminPanel()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }

        public void OnLoad(object sender, EventArgs e)
        {
            //_db = new DataBase();
            //List<string> occupations = _db.GetAllOccupations();
            //cmbbxOccupation.ItemsSource = occupations;
            //List<string> occupationFilters = new List<string>(occupations);
            //occupationFilters.Add("Без фильтров");
            //cmbbxFilter.ItemsSource = occupationFilters;
            //cmbbxFilter.SelectedItem = "Без фильтров";
            //cmbbxFilter.SelectedValue = "Без фильтров";
            _db = new DataBase();
            List<Occupation> occupations = _db.GetAllOccupations();
            cmbbxFilter.ItemsSource = occupations;

            UpdateUsersList();
        }

        private void UpdateUsersList()
        {
            List<User> users = _db.GetAllUsers();
            foreach (User user in users)
                user.FullName = $"{user.Name} {user.LastName} {user.SecondName}";

            UsersListView.ItemsSource = users;
            _currentUserList = users;
            _usersList = users;
        }

        public void EditUser(object sender, MouseButtonEventArgs e)
        {
            _editedUser = ((ListViewItem)sender).Content as User;
            ShowUserEditor();
        }

        public void ShowUserEditor()
        {
            txtbxName.Text = _editedUser.Name;
            txtbxLastName.Text = _editedUser.LastName;
            txtbxSecondName.Text = _editedUser.SecondName;
            txtbxLogin.Text = _editedUser.Login;
            txtbxEmail.Text = _editedUser.Email;
            cmbbxOccupation.SelectedItem = _editedUser.Occupation;
            cmbbxOccupation.SelectedValue = _editedUser.Occupation;

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

            _editedUser.Name = txtbxName.Text;
            _editedUser.SecondName = txtbxLastName.Text;
            _editedUser.LastName = txtbxSecondName.Text;

            if (txtbxPassword.Text != string.Empty)
                _editedUser.Password = txtbxPassword.Text;

            _editedUser.Login = txtbxLogin.Text;
            //_editedUser.Occupation = Convert.ToString(cmbbxOccupation.SelectedValue);
            _editedUser.Email = txtbxEmail.Text;

            _db.Update(_editedUser);
            UpdateUsersList();
            UserEditor.Visibility = Visibility.Collapsed;
        }

        private void CancelEdit(object sender, RoutedEventArgs e)
            => UserEditor.Visibility = Visibility.Collapsed;

        private void Return(object sender, RoutedEventArgs e)
            => NavigationService.GoBack();

        private void CancelSearch(object sender, RoutedEventArgs e)
        {
            UsersListView.ItemsSource = _usersList;
            _currentUserList = _usersList;
            cmbbxFilter.SelectedValue = string.Empty;
            txtbxSearch.Text = string.Empty;
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            string searchString = txtbxSearch.Text;
            if (searchString == string.Empty)
                return;

            List<User> findedUsers = new List<User>();
            string[] searchArray = searchString.Split(' ').Select(name => name.ToLower()).ToArray();
            foreach (User user in _usersList)
            {
                string[] names = user.FullName.Split(' ').Select(name => name.ToLower()).ToArray();
                foreach (string name in names)
                    if (searchArray.Contains(name))
                    {
                        findedUsers.Add(user);
                        break;
                    }
            }

            List<string> rows = new List<string>();
            foreach (User user in findedUsers)
            {
                rows.Add("\n");
                rows.Add(user.FullName);
            }
            File.WriteAllLines("logs.txt", rows);

            _currentUserList = findedUsers;
            ApplyFilter(null, null);
        }

        private void ApplyFilter(object sender, SelectionChangedEventArgs e)
        {
            Occupation filter = (Occupation)cmbbxFilter.SelectedItem;
            if (filter == null)
                return;
            List<User> filteredUsers = new List<User>();
            foreach (User user in _currentUserList)
                if (user.Occupation.Id == filter.Id)
                    filteredUsers.Add(user);

            UsersListView.ItemsSource = filteredUsers;
        }
    }
}
