using Api_pdc_Interfaces;
using Mongo.Migration.Documents;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api_pdc_Entities
{
    public abstract class Document : Api_pdc_Interfaces.IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedAt => Id.CreationTime;
    }
}
