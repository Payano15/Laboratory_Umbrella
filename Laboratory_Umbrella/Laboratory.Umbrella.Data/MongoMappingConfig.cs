using Laboratory.Umbrella.Dominio.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;

namespace Laboratory.Umbrella.Data;

public class MongoMappingConfig
{
    public static void RegisterMappings()
    {
        BsonSerializer.RegisterSerializer(new DateTimeSerializer(DateTimeKind.Local));
        BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
        BsonSerializer.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
        BsonSerializer.RegisterSerializer(typeof(Dictionary<string, string>), new DictionaryInterfaceImplementerSerializer<Dictionary<string, string>>(DictionaryRepresentation.ArrayOfDocuments));
        BsonSerializer.RegisterSerializer(typeof(Dictionary<int, string>), new DictionaryInterfaceImplementerSerializer<Dictionary<int, string>>(DictionaryRepresentation.ArrayOfDocuments));
        BsonSerializer.RegisterSerializer(typeof(Dictionary<ObjectId, string>), new DictionaryInterfaceImplementerSerializer<Dictionary<ObjectId, string>>(DictionaryRepresentation.ArrayOfDocuments));
        BsonSerializer.RegisterSerializer(typeof(Dictionary<ObjectId, int>), new DictionaryInterfaceImplementerSerializer<Dictionary<ObjectId, int>>(DictionaryRepresentation.ArrayOfDocuments));
        BsonSerializer.RegisterSerializer(typeof(Dictionary<ObjectId, decimal>), new DictionaryInterfaceImplementerSerializer<Dictionary<ObjectId, decimal>>(DictionaryRepresentation.ArrayOfDocuments));
        BsonSerializer.RegisterSerializer(typeof(Dictionary<int, decimal>), new DictionaryInterfaceImplementerSerializer<Dictionary<int, decimal>>(DictionaryRepresentation.ArrayOfDocuments));
        BsonSerializer.RegisterSerializer(typeof(Dictionary<string, decimal>), new DictionaryInterfaceImplementerSerializer<Dictionary<string, decimal>>(DictionaryRepresentation.ArrayOfDocuments));

    }

    private static void Register<T>() where T : IEntity
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(T))) return;

        BsonClassMap.RegisterClassMap<T>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(c => c.Id)
              .SetIdGenerator(StringObjectIdGenerator.Instance)
              .SetSerializer(new StringSerializer(BsonType.ObjectId));

            foreach (var memberMap in cm.AllMemberMaps)
            {
                if (memberMap.MemberType == typeof(DateTime) ||
                    memberMap.MemberType == typeof(DateTime?))
                {
                    memberMap.SetSerializer(new DateTimeSerializer(DateTimeKind.Local));
                }

                if (memberMap.MemberType == typeof(decimal))
                {
                    memberMap.SetSerializer(new DecimalSerializer(BsonType.Decimal128));
                }
                else if (memberMap.MemberType == typeof(decimal?))
                {
                    memberMap.SetSerializer(new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
                }
            }
        });
    }
}
