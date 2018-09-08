namespace HelloCQRS.QueryModel
{
    public class Observer
    {
        protected Projection Projection;

        public Observer(IProjectionFactory factory)
        {
            Projection = factory.CreateProjection();
        }
    }
}