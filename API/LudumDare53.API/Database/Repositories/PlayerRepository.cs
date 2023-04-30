﻿using LudumDare53.API.Models;
using MongoDB.Driver;

namespace LudumDare53.API.Database.Repositories;

public class PlayerRepository : Repository<Player>
{
    public PlayerRepository(IMongoClient mongoClient) : base(mongoClient)
    {
        CollectionName = "players";
    }

    public override async Task<List<Player>> Get()
    {
        var playerList = await base.Get();
        return playerList.OrderBy(x => x.Name).ToList();
    }
}