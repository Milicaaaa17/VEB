using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Zadatak.Models.FileControllers
{
    public class TreningFileController : IFileController<GrupniTrening>
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data/GrupniTrening.xml";
        XmlSerializer serializer = new XmlSerializer(typeof(List<GrupniTrening>));
        public TreningFileController()
        {
            if (!File.Exists(path))
            {
                using (var w = new StreamWriter(path, false))
                {
                    serializer.Serialize(w, new List<GrupniTrening>());
                }
            }
        }
        public void Delete(int id)
        {
            List<GrupniTrening> l;
            using (var r = new StreamReader(path))
            {
                l = (List<GrupniTrening>)serializer.Deserialize(r);
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

        public GrupniTrening Get(int id)
        {
            using (var r = new StreamReader(path))
            {
                return ((List<GrupniTrening>)serializer.Deserialize(r)).Find(x => x.Id == id && !x.Obrisan);
            }
        }

        public List<GrupniTrening> GetAll()
        {
            using (var r = new StreamReader(path))
            {
                return ((List<GrupniTrening>)serializer.Deserialize(r)).FindAll(x => !x.Obrisan);
            }
        }

        public void Insert(GrupniTrening obj)
        {
            List<GrupniTrening> l;
            using (var r = new StreamReader(path))
            {
                l = (List<GrupniTrening>)serializer.Deserialize(r);
            }
            obj.Id = l.Count;
            l.Add(obj);
            using (var w = new StreamWriter(path, false))
            {
                serializer.Serialize(w, l);
            }
        }

        public void Update(int id, GrupniTrening obj)
        {
            List<GrupniTrening> l;
            using (var r = new StreamReader(path))
            {
                l = (List<GrupniTrening>)serializer.Deserialize(r);
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