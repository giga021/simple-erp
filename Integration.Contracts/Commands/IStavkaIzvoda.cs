namespace Integration.Contracts.Commands
{
	public interface IStavkaIzvoda
	{
		int SifraPlacanja { get; }
		decimal Duguje { get; }
		decimal Potrazuje { get; }
	}
}
