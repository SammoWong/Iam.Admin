using MongoDB.Bson;

namespace Iam.Data.MongoDB.Document
{
    public interface IDocument
    {
        ObjectId Id { get; set; }
    }
}
