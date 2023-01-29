using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Zadatak.Models.FileControllers
{
    public class CentarFileController : IFileController<FitnesCentar>
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data/FitnesCentar.xml";
        XmlSerializer serializer = new XmlSerializer(typeof(List<FitnesCentar>));

        public CentarFileController()
        {
            if (!File.Exists(path)) 
            {
                using (var w = new StreamWriter(path, false))
                {
                    serializer.Serialize(w, new List<FitnesCentar>());
                }
            }
        }

        public void Delete(int id)
        {
            List<FitnesCentar> l;
            using (var r = new StreamReader(path))
            {
                l = (List<FitnesCentar>)serializer.Deserialize(r);
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

        public FitnesCentar Get(int id)
        {
            using (var r = new StreamReader(path))
            {
                return ((List<FitnesCentar>)serializer.Deserialize(r)).Find(x => x.Id == id && !x.Obrisan);
            }
        }

        public List<FitnesCentar> GetAll()
        {
            using (var r = new StreamReader(path))
            {
                return ((List<FitnesCentar>)serializer.Deserialize(r)).FindAll(x => !x.Obrisan);
            }
        }

        public void Insert(FitnesCentar obj)
        {
            List<FitnesCentar> l;
            using (var r = new StreamReader(path))
            {
                l = (List<FitnesCentar>)serializer.Deserialize(r);
            }
            obj.Id = l.Count;
            l.Add(obj);
            using (var w = new StreamWriter(path, false))
            {
                serializer.Serialize(w, l);
            }
        }

        public void Update(int id, FitnesCentar obj)
        {
            List<FitnesCentar> l;
            using (var r = new StreamReader(path))
            {
                l = (List<FitnesCentar>)serializer.Deserialize(r);
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