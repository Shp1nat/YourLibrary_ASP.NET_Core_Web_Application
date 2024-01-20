using Backend.Api.Contract;
using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Backend.Api.Services
{
    public class UserService
    {
        private readonly LibraryWebDbContex _libraryWebDbContex;
        public UserService(LibraryWebDbContex libraryWebDbContex)
        {
            _libraryWebDbContex= libraryWebDbContex;
        }
        public Guid? Register(RegisterData registerData)
        {
            var checkUser1 = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.User>().SingleOrDefault(x => x.Login == registerData.Login);
            var checkUser2 = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.User>().SingleOrDefault(x => x.Email == registerData.Email); 
            if (checkUser1 != null || checkUser2 != null) { return null; }
            var user = new DatabaseAccessLayer.Entities.User
            {
                UidUser = Guid.NewGuid(),
                Email = registerData.Email,
                Login = registerData.Login,
                Password = GetHash(registerData.Password),
                IsAdmin = false
            };
            _libraryWebDbContex.Add(user);
            _libraryWebDbContex.SaveChanges();
            return user.UidUser;
        }
        public Guid? Login(LoginData loginData)
        {
            var hashedPassword = GetHash(loginData.Password);
            var user = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.User>().SingleOrDefault(x => x.Email == loginData.Email && x.Password == hashedPassword);
            return user?.UidUser;
        }  
        public UserUpdate? GetData(Guid uIdUser)
        {
            var user = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.User>().SingleOrDefault(x => x.UidUser == uIdUser);
            if (user == null) return null;
            return new UserUpdate
            {
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                Otchestvo = user.Otchestvo,
                Login = user.Login,
                Password = "",
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Age = user.Age
            };
        }
        public Guid? EditData(Guid uIdUser, UserUpdate userUpdate)
        {
            var user = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.User>().SingleOrDefault(x => x.UidUser == uIdUser);
            if (user == null) return null;
            if ((_libraryWebDbContex.Set<DatabaseAccessLayer.Entities.User>().SingleOrDefault(x => x.Login == userUpdate.Login) != null) && (userUpdate.Login != user.Login)
                || (_libraryWebDbContex.Set<DatabaseAccessLayer.Entities.User>().SingleOrDefault(x => x.Email == userUpdate.Email) != null) && (userUpdate.Email != user.Email))
                return null;
            user.Name = userUpdate.Name;
            user.Surname = userUpdate.Surname;
            user.Otchestvo= userUpdate.Otchestvo; 
            user.Login = userUpdate.Login;
            user.Email = userUpdate.Email;
            user.Password = GetHash(userUpdate.Password);
            user.Address = userUpdate.Address;
            user.PhoneNumber = userUpdate.PhoneNumber;
            user.Age = userUpdate.Age;
            _libraryWebDbContex.SaveChanges();
            return user.UidUser;
        }
        public bool SetAdminStatus(Guid uIdUser, bool status)
        {
            var user = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.User>().SingleOrDefault(x => x.UidUser == uIdUser);
            if (user == null) return false;
            user.IsAdmin = status;
            _libraryWebDbContex.SaveChanges();
            return true;
        }
        public bool Delete(Guid uIdUser)
        {
            var user = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.User>().SingleOrDefault(x => x.UidUser == uIdUser);
            if (user == null) return false;
            try
            {
                _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.User>().Remove(user);
                _libraryWebDbContex.SaveChanges();
            }
            catch { return false; }
            return true;
        }

        public bool isAdmin(Guid uIdUser)
        {
            var user = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.User>().SingleOrDefault(x => x.UidUser == uIdUser);
            if (user != null) return user.IsAdmin;
            else  return false; 
        }

        private string GetHash(string password)
        {
            using var sha = SHA512.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }

    }
}
