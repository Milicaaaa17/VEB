using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Zadatak.Models.FileControllers
{
    public class KorisnikFileController : IFileController<Korisnik>
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data/Korisnik.xml";
        XmlSerializer serializer = new XmlSerializer(typeof(List<Korisnik>));

        public KorisnikFileController()
        {
            if (!File.Exists(path))
            {
                using (var w = new StreamWriter(path, false))
                {
                    serializer.Serialize(w, new List<Korisnik>());
                }
            }
        }

        public void Delete(int id)
        {
            List<Korisnik> l;
            using (var r = new StreamReader(path))
            {
                l = (List<Korisnik>)serializer.Deserialize(r);
            }

            int f = l.FindIndex(x => x.Id == id);
            if (f != -1)
            {
                l[f].Obrisan = true;
                using (var w = new StreamWriter(path, false))
                {
                    serializer.Serialize(w, l);
                }
            }
        }

        public Korisnik Get(int id)
        {
            using (var r = new StreamReader(path))
            {
                return ((List<Korisnik>)serializer.Deserialize(r)).Find(x => x.Id == id && !x.Obrisan);
            }
        }

        public List<Korisnik> GetAll()
        {
            using (var r = new StreamReader(path))
            {
                return ((List<Korisnik>)serializer.Deserialize(r)).FindAll(x => !x.Obrisan);
            }
        }

        public void Insert(Korisnik obj)
        {
            List<Korisnik> l;
            using (var r = new StreamReader(path))
            {
                l = (List<Korisnik>)serializer.Deserialize(r);
            }
            obj.Id = l.Count;
            l.Add(obj);
            using (var w = new StreamWriter(path, false))
            {
                serializer.Serialize(w, l);
            }
        }

        public void Update(int id, Korisnik obj)
        {
            List<Korisnik> l;
            using (var r = new StreamReader(path))
            {
                l = (List<Korisnik>)serializer.Deserialize(r);
            }

            int f = l.FindIndex(x => x.Id == id);
            if (f != -1)
            {
                l[f] = obj;
                using (var w = new StreamWriter(path, false))
                {
                    serializer.Serialize(w, l);
                }
            }
        }
    }
}