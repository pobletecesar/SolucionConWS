using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using BLOG.Entidades;
using System.Xml.Serialization;
using System.IO;
using BLOG.Negocio;

namespace Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SvcPost" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SvcPost.svc or SvcPost.svc.cs at the Solution Explorer and start debugging.
    public class SvcPost : ISvcPost
    {
        public bool Crear(string xmlPost)
        {
            XmlSerializer xmlSerial = new XmlSerializer(typeof(BLOG.Entidades.Post));
            StringReader xmlReader = new StringReader(xmlPost);
            BLOG.Entidades.Post oPost = (BLOG.Entidades.Post)xmlSerial.Deserialize(xmlReader);
            BLOG.Negocio.Post nPost = new BLOG.Negocio.Post();

            if(nPost.Crear(oPost))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(string xmlPost)
        {
            XmlSerializer xmlSerial = new XmlSerializer(typeof(BLOG.Entidades.Post));
            StringReader xmlReader = new StringReader(xmlPost);
            BLOG.Entidades.Post oPost = (BLOG.Entidades.Post)xmlSerial.Deserialize(xmlReader);
            BLOG.Negocio.Post nPost = new BLOG.Negocio.Post();

            if (nPost.Eliminar(oPost))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ListarTodo()
        {
            List<BLOG.Entidades.Post> lPost = new List<BLOG.Entidades.Post>();
            BLOG.Negocio.Post nPost = new BLOG.Negocio.Post();
            XmlSerializer xmlSerial = new XmlSerializer(typeof(List<BLOG.Entidades.Post>));
            StringWriter xmlWriter = new StringWriter();
            lPost = nPost.ListarTodo();
            xmlSerial.Serialize(xmlWriter, lPost);
            return xmlWriter.ToString();
        }

        public bool Modificar(string xmlPost)
        {
            XmlSerializer xmlSerial = new XmlSerializer(typeof(BLOG.Entidades.Post));
            StringReader xmlReader = new StringReader(xmlPost);
            BLOG.Entidades.Post oPost = (BLOG.Entidades.Post)xmlSerial.Deserialize(xmlReader);
            BLOG.Negocio.Post nPost = new BLOG.Negocio.Post();

            if (nPost.Modificar(oPost))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
