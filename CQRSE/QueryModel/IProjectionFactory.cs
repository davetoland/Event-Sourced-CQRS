
namespace HelloCQRS.QueryModel
{
    public interface IProjectionFactory
    {
        Projection CreateProjection();
    }
}