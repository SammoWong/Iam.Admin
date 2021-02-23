using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Data.MongoDB.Document
{
    public abstract class Document : IDocument
    {
        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        public ObjectId Id { get; set; }
    }
}
