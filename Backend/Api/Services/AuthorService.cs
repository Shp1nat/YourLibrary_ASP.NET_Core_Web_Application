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
    public class AuthorService
    {
        private readonly LibraryWebDbContex _libraryWebDbContex;
        public AuthorService(LibraryWebDbContex libraryWebDbContex)
        {
            _libraryWebDbContex= libraryWebDbContex;
        }
        public Guid? CreateAuthor(string nameOfAuthor)
        {
            var author = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Author>().SingleOrDefault(x => x.NameOfAuthor== nameOfAuthor);
            if (author != null) return null;
            author = new DatabaseAccessLayer.Entities.Author
            {
                UidAuthor = Guid.NewGuid(),
                NameOfAuthor = nameOfAuthor
            };
            _libraryWebDbContex.Add(author);
            _libraryWebDbContex.SaveChanges();
            return author.UidAuthor;       
        }
        public List<Contract.Author>? GetAuthors()
        {
            var authorsDAL = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Author>().ToList();
            List<Contract.Author> authorsCntx = new List<Contract.Author>();
            if (authorsDAL.Any())
            {
                foreach (var el in authorsDAL)
                {
                    authorsCntx.Add(new Contract.Author { UidAuthor = el.UidAuthor, NameOfAuthor = el.NameOfAuthor });
                }
                return authorsCntx;
            }
            return null;
        }     
        public bool EditAuthor(Guid uIdAuthor, string nameOfAuthor)
        {
            var author = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Author>().SingleOrDefault(x => x.UidAuthor == uIdAuthor);
            if (author == null) return false;
            author.NameOfAuthor = nameOfAuthor;
            _libraryWebDbContex.SaveChanges();
            return true;
        }
        public bool DeleteAuthor(Guid uIdAuthor)
        {
            var author = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Author>().SingleOrDefault(x => x.UidAuthor == uIdAuthor);
            if (author == null) return false;
            try
            {
                _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.Author>().Remove(author);
                _libraryWebDbContex.SaveChanges();
            }        
            catch { return false; }
            return true;
        }
    }
}