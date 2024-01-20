using Backend.Api.Contract;
using Backend.Api.Controllers;
using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using static System.Reflection.Metadata.BlobBuilder;

namespace Backend.Api.Services
{
    public class VacancyService
    {
        private readonly LibraryWebDbContex _libraryWebDbContex;
        public VacancyService(LibraryWebDbContex libraryWebDbContex)
        {
            _libraryWebDbContex= libraryWebDbContex;
        }
        public Guid? CreateVacancy(Guid uIdUser, string text)
        {
            var user = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.User>().SingleOrDefault(x => x.UidUser == uIdUser);
            if (user == null) return null;
            var vacancy = new DatabaseAccessLayer.Entities.Vacancy
            {
                UidVacancy = Guid.NewGuid(),
                Text = text,
                StatusOfVacancy = 0,
                User = user
            };
            _libraryWebDbContex.Add(vacancy);
            _libraryWebDbContex.SaveChanges();
            return vacancy.UidVacancy;
        }
        public List<Contract.Vacancy>? GetVacancies()
        {
            var vacancies = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Vacancy>()
                .Include(x => x.User).ToList();
            if (vacancies.Count == 0) return null; 
            return vacancies.Select(vacancy => new Contract.Vacancy
            {
                UidVacancy = vacancy.UidVacancy,
                Text = vacancy.Text,
                User = new Contract.User
                {
                    UidUser = vacancy.User.UidUser,
                    Email = vacancy.User.Email,
                    Name = vacancy.User.Name,
                    Surname = vacancy.User.Surname,
                    Otchestvo = vacancy.User.Otchestvo,
                    Login = vacancy.User.Login,
                    Address = vacancy.User.Address,
                    PhoneNumber = vacancy.User.PhoneNumber,
                    Age = vacancy.User.Age,
                    IsAdmin= vacancy.User.IsAdmin
                },
                StatusOfVacancy = vacancy.StatusOfVacancy
            }).ToList();
        }     
        public List<Contract.Vacancy>? GetVacancies(Guid uIdUser)
        {
            var vacancies = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Vacancy>()
                .Include(x => x.User).Where(x => x.User.UidUser == uIdUser).ToList();
            if (vacancies.Count == 0) return null;
            return vacancies.Select(vacancy => new Contract.Vacancy
            {
                UidVacancy = vacancy.UidVacancy,
                Text = vacancy.Text,
                User = new Contract.User
                {
                    UidUser = vacancy.User.UidUser,
                    Email = vacancy.User.Email,
                    Name = vacancy.User.Name,
                    Surname = vacancy.User.Surname,
                    Otchestvo = vacancy.User.Otchestvo,
                    Login = vacancy.User.Login,
                    Address = vacancy.User.Address,
                    PhoneNumber = vacancy.User.PhoneNumber,
                    Age = vacancy.User.Age,
                    IsAdmin = vacancy.User.IsAdmin
                },
                StatusOfVacancy = vacancy.StatusOfVacancy
            }).ToList();
        }
        public bool AcceptVacancy(Guid uIdVacancy)
        {
            var vacancy = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Vacancy>()
                .Include(x => x.User)
                .SingleOrDefault(x => x.UidVacancy == uIdVacancy);
            if (vacancy == null) return false;
            vacancy.StatusOfVacancy = 1;
            vacancy.User.IsAdmin = true;
            _libraryWebDbContex.SaveChanges();
            return true;
        }
        public bool RejectVacancy(Guid uIdVacancy)
        {
            var vacancy = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Vacancy>()
                .Include(x => x.User)
                .SingleOrDefault(x => x.UidVacancy == uIdVacancy);
            if (vacancy == null) return false;
            vacancy.StatusOfVacancy = -1;
            vacancy.User.IsAdmin = false;
            _libraryWebDbContex.SaveChanges();
            return true;
        }
        public bool DeleteVacancy(Guid uIdVacancy)
        {
            var vacancy = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Vacancy>()
                .Include(x => x.User)
                .SingleOrDefault(x => x.UidVacancy == uIdVacancy);
            if (vacancy == null) return false;
            _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Vacancy>().Remove(vacancy);
            _libraryWebDbContex.SaveChanges();
            return true;
        }
        public bool DeleteVacancy(Guid uIdVacancy, Guid uIdUser)
        {
            var vacancy = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Vacancy>()
                .Include(x => x.User)
                .SingleOrDefault(x => x.UidVacancy == uIdVacancy);
            if (vacancy == null || vacancy.User.UidUser != uIdUser) return false;
            try
            {
                _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Vacancy>().Remove(vacancy);
                _libraryWebDbContex.SaveChanges();
            }
            catch { return false; }
            return true;
        }
    }
}