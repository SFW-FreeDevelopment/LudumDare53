using LudumDare53.API.Models.Request;

namespace LudumDare53.API.Models;

public class Player : Resource
{
    public string Name { get; set; }
    public int DaysCompleted { get; set; }
    public int TotalMoneyEarned { get; set; }
    public int DeliveriesMade { get; set; }
    
    public Player() { }

    public Player(PlayerCreateRequest playerCreateRequest)
    {
        Id = playerCreateRequest.Id;
        Name = playerCreateRequest.Name;
    }
}