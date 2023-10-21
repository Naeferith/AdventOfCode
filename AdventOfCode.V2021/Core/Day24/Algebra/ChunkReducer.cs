using AdventOfCode.V2021.Core.Day24.Command;
using System.Linq.Expressions;

namespace AdventOfCode.V2021.Core.Day24.Algebra
{
    internal class ChunkReducer
    {
        public Predicate<AluData> ChunckPredicate(IEnumerable<ICommand> commands)
        {
            Expression<Predicate<AluData>> expression = d => d.Z == 0;

            return expression.Compile();
        }
    }

    internal class ChunkAluData
    {
        public Expression<Func<Alu, int>> DelegateW { get; set; }
        public Expression<Func<Alu, int>> DelegateX { get; set; }
        public Expression<Func<Alu, int>> DelegateY { get; set; }
        public Expression<Func<Alu, int>> DelegateZ { get; set; }
    }
}
