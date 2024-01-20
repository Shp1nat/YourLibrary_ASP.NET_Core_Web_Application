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
    public class GenreService
    {
        private readonly LibraryWebDbContex _libraryWebDbContex;
        public GenreService(LibraryWebDbContex libraryWebDbContex)
        {
            _libraryWebDbContex= libraryWebDbContex;
        }
        public Guid? CreateGenre(string nameOfGenre)
        {
            var genre = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Genre>().SingleOrDefault(x => x.NameOfGenre== nameOfGenre);
            if (genre != null) return null;
            genre = new DatabaseAccessLayer.Entities.Genre
            {
                UidGenre = Guid.NewGuid(),
                NameOfGenre = nameOfGenre
            };
            _libraryWebDbContex.Add(genre);
            _libraryWebDbContex.SaveChanges();
            return genre.UidGenre;       
        }
        public List<Contract.Genre>? GetGenres()
        {
            var genresDAL = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Genre>().ToList();
            List<Contract.Genre> genresCntx = new List<Contract.Genre>();
            if (genresDAL.Any())
            {
                foreach (var el in genresDAL)
                {
                    genresCntx.Add(new Contract.Genre { UidGenre = el.UidGenre, NameOfGenre = el.NameOfGenre });
                }
                return genresCntx;
            }
            return null;
        }     
        public bool EditGenre(Guid uIdGenre, string nameOfGenre)
        {
            var genre = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Genre>().SingleOrDefault(x => x.UidGenre == uIdGenre);
            if (genre == null) return false;
            genre.NameOfGenre = nameOfGenre;
            _libraryWebDbContex.SaveChanges();
            return true;
        }
        public bool DeleteGenre(Guid uIdGenre)
        {
            var genre = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Genre>().SingleOrDefault(x => x.UidGenre == uIdGenre);
            if (genre == null) return false;
            try
            {
                _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Genre>().Remove(genre);
                _libraryWebDbContex.SaveChanges();
            }
            catch { return false; }
            return true;
        }
    }
}