using HoytSys.DataStructures.Graph;

namespace A19.Database.Diff
{
    public interface IBuildDependencyGraph
    {
        /// <summary>
        ///     Used to build the graph to figure out the commit order.
        /// </summary>
        /// <param name="graph">The uniform graph object.</param>
        void BuildDependencyGraph(UniformMatrixGraph graph);
    }
}