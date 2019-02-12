namespace Knjizenje.Domain.Seedwork
{
	public interface IRepository<T>
	{
		IUnitOfWork UnitOfWork { get; }
	}
}
