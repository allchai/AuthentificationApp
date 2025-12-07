using HasherLib;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using UI.model;

namespace UI.classes
{
    internal class UsersList
    {
        private List<User> _list;
        private UserEntity _db;

        public UsersList()
        {
            _db = UserEntity.GetContext();
        }

        public void UpdateList()
        {
            _list = _db.Employees.Select(employee => new User()
            {
                Id = employee.ID,
                Name = employee.Name,
                SecondName = employee.SecondName,
                LastName = employee.LastName,
                Login = employee.Login,
                Password = employee.Password,
                Occupation = new Occupation()
                {
                    Id = employee.Occupations.ID,
                    Name = employee.Occupations.Name
                },
                Email = employee.Email
            }).ToList();
            foreach (User user in _list)
                user.FullName = $"{user.Name} {user.LastName} {user.SecondName}";
        }

        public List<Occupation> GetAllOccupations()
            => _db.Occupations.Select(occupation => new Occupation()
            {
                Id = occupation.ID,
                Name = occupation.Name
            }).ToList();

        public List<User> Search(List<User> usersList, string entry)
        {
            string[] entryArray = entry.Split(' ').Select(name => name.ToLower()).ToArray();

            List<User> findedUsers = new List<User>();
            foreach (User user in usersList)
            {
                int entries = 0;
                string[] names = user.FullName.Split(' ').Select(name => name.ToLower()).ToArray();
                foreach (string name in entryArray)
                    if (names.Contains(name))
                        entries++;

                if (entries == entryArray.Length)
                    findedUsers.Add(user);
            }

            return findedUsers;
        }

        public List<User> ApplyFilter(List<User> list, Occupation filter)
             => list.Where(user => user.Occupation.Id == filter.Id).ToList();
            
        

        public void UpdateUser(User user, bool updatedPassword)
        {
            var userDb = _db.Employees.First(employee => employee.ID == user.Id);

            userDb.Name = user.Name;
            userDb.LastName = user.LastName;
            userDb.SecondName = user.SecondName;
            userDb.OccupationID = user.Occupation.Id;
            userDb.Email = user.Email;
            userDb.Login = user.Login;
            if (updatedPassword)
                userDb.Password = Hasher.Hash(user.Password);

            _db.Employees.AddOrUpdate(userDb);
            _db.SaveChanges();
        }

        public IEnumerable<User> List => _list;
    }
}
