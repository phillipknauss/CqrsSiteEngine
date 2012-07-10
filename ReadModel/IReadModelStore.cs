namespace ReadModel
{
    public interface IReadModelStore
    {
        IReadModel GetReadModel<T>();
        IReadModel GetOrCreate<T>();
        void Save(IReadModel readModel);
    }
}