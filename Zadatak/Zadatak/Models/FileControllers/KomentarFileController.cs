using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Zadatak.Models.FileControllers
{
    public class KomentarFileController : IFileController<Komentar>
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data/Komentar.xml";
        XmlSerializer serializer = new XmlSerializer(typeof(List<Komentar>));

        public KomentarFileController()
        {
            if (!File.Exists(path))
            {
                using (var w = new StreamWriter(path, false))
                {
                    serializer.Serialize(w, new List<Komentar>());
                }
            }
        }

        public void Delete(int id)
        {
            List<Komentar> l;
            using (var r = new StreamReader(path))
            {
                l = (List<Komentar>)serializer.Deserialize(r);
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

        public Komentar Get(int id)
        {
            using (var r = new StreamReader(path))
            {
                return ((List<Komentar>)serializer.Deserialize(r)).Find(x => x.Id == id && !x.Obrisan);
            }
        }

        public List<Komentar> GetAll()
        {
            using (var r = new StreamReader(path))
            {
                return ((List<Komentar>)serializer.Deserialize(r)).FindAll(x => !x.Obrisan);
            }
        }
        public void Insert(Komentar obj)
        {
            List<Komentar> l;
            using (var r = new StreamReader(path))
            {
                l = (List<Komentar>)serializer.Deserialize(r);
            }
            obj.Id = l.Count;
            l.Add(obj);
            using (var w = new StreamWriter(path, false))
            {
                serializer.Serialize(w, l);
            }
        }

        public void Update(int id, Komentar obj)
        {
            List<Komentar> l;
            using (var r = new StreamReader(path))
            {
                l = (List<Komentar>)serializer.Deserialize(r);
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