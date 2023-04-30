namespace LudumDare53.API.Models;

public class Player : Resource
{
    public string Name { get; set; }
    public int DaysCompleted { get; set; }
    public int TotalMoneyEarned { get; set; }
    public int DeliveriesMade { get; set; }

    public Player() { }

    public Player(PlayerDto playerDto)
    {
        Name = playerDto.Name;
        DaysCompleted = playerDto.DaysCompleted;
        TotalMoneyEarned = playerDto.TotalMoneyEarned;
        DeliveriesMade = playerDto.DeliveriesMade;
    }
}