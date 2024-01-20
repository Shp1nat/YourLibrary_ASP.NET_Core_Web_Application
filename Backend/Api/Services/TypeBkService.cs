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
    public class TypeBkService
    {
        private readonly LibraryWebDbContex _libraryWebDbContex;
        public TypeBkService(LibraryWebDbContex libraryWebDbContex)
        {
            _libraryWebDbContex= libraryWebDbContex;
        }
        public Guid? CreateTypeBk(string nameOfTypeBk)
        {
            var TypeBk = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.TypeBk>().SingleOrDefault(x => x.NameOfTypeBk== nameOfTypeBk);
            if (TypeBk != null) return null;
            TypeBk = new DatabaseAccessLayer.Entities.TypeBk
            {
                UidTypeBk = Guid.NewGuid(),
                NameOfTypeBk = nameOfTypeBk
            };
            _libraryWebDbContex.Add(TypeBk);
            _libraryWebDbContex.SaveChanges();
            return TypeBk.UidTypeBk;       
        }
        public List<Contract.TypeBk>? GetTypesBk()
        {
            var typesBkDAL = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.TypeBk>().ToList();
            List<Contract.TypeBk> typesBkCntx = new List<Contract.TypeBk>();
            if (typesBkDAL.Any())
            {
                foreach (var el in typesBkDAL)
                {
                    typesBkCntx.Add(new Contract.TypeBk { UidTypeBk = el.UidTypeBk, NameOfTypeBk = el.NameOfTypeBk });
                }
                return typesBkCntx;
            }
            return null;
        }     
        public bool EditTypeBk(Guid uIdTypeBk, string nameOfTypeBk)
        {
            var TypeBk = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.TypeBk>().SingleOrDefault(x => x.UidTypeBk == uIdTypeBk);
            if (TypeBk == null) return false;
            TypeBk.NameOfTypeBk = nameOfTypeBk;
            _libraryWebDbContex.SaveChanges();
            return true;
        }
        public bool DeleteTypeBk(Guid uIdTypeBk)
        {
            var TypeBk = _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.TypeBk>().SingleOrDefault(x => x.UidTypeBk == uIdTypeBk);
            if (TypeBk == null) return false;
            try
            {
                _libraryWebDbContex.Set<DatabaseAccessLayer.Entities.TypeBk>().Remove(TypeBk);
                _libraryWebDbContex.SaveChanges();
            }
            catch { return false; }
            return true;
        }
    }
}