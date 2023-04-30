using LudumDare53.API.Models;
using MongoDB.Driver;

namespace LudumDare53.API.Database.Repositories;

public class PlayerRepository : Repository<Player>
{
    public PlayerRepository(IMongoClient mongoClient) : base(mongoClient)
    {
        CollectionName = "players";
    }
}