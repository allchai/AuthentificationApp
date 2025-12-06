using HasherLib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.model;

namespace UI.classes
{
    public class DataBase
    {
        private UserEntity _db;

        public DataBase() { _db = UserEntity.GetContext(); }

        public User FindUser(string login)
        {
            var users = _db.Employees;
            var DbUser = users.FirstOrDefault(currentUser => currentUser.Login == login);
            if (DbUser == null)
                return null;

            User user = new User()
            {
                Name = DbUser.Name,
                LastName = DbUser.LastName,
                SecondName = DbUser.SecondName,
                Login = DbUser.Login,
                Password = DbUser.Password,
                Occupation = new Occupation()
                {
                    Id = DbUser.Occupations.ID,
                    Name = DbUser.Occupations.Name
                },
                Email = DbUser.Email
            };
            return user;
        }

        public bool TryAuthorise(User user, string password)
            => user.Password == Hasher.Hash(password) ? true : false;

        public List<User> GetAllUsers()
        {
            var DbUsers = _db.Employees;
            List<User> users = DbUsers.Select(user => new User()
            {
                Id = user.ID,
                Name = user.Name,
                LastName = user.LastName,
                SecondName = user.SecondName,
                Login = user.Login,
                Password = user.Password,
                Occupation = new Occupation()
                {
                    Id = user.Occupations.ID,
                    Name = user.Occupations.Name
                },
                Email = user.Email
            }).ToList();

            return users;
        }

        public List<Occupation> GetAllOccupations()
            => _db.Occupations.Select(occupation => new Occupation()
            {
                Id = occupation.ID,
                Name = occupation.Name,
            }).ToList();

        public void Update(User user)
        {
            Employees userDb = _db.Employees.FirstOrDefault(currentUser => currentUser.ID == user.Id);
            if (userDb != null)
            {
                //Occupations occupation = _db.Occupations.FirstOrDefault(currentOcc => currentOcc.Name == user.Occupation.Name);
                //if (occupation != null)
                //{
                //    userDb.OccupationID = occupation.ID;
                //}
                userDb.OccupationID = user.Occupation.Id;
                userDb.Name = user.Name;
                userDb.LastName = user.LastName;
                userDb.SecondName = user.SecondName;
                userDb.Login = user.Login;
                userDb.Password = Hasher.Hash(user.Password);
                userDb.Email = user.Email;

                _db.Employees.AddOrUpdate(userDb);
                _db.SaveChanges();
            }
        }
    }
}
