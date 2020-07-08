using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISvcPost" in both code and config file together.
    [ServiceContract]
    public interface ISvcPost
    {

        [OperationContract]
        bool Crear(string xmlPost);

        [OperationContract]
        bool Modificar(string xmlPost);

        [OperationContract]
        bool Eliminar(string xmlPost);

        [OperationContract]
        string ListarTodo();

        // TODO: Add your service operations here
    }


}
