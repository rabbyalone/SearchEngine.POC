namespace SoftSkill.SearchEngine.POC.Services.Services
{
    public interface ISearchableModel<T>
    {
        IEnumerable<T> Search(string query);
    }
}
