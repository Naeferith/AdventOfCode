using Nae.Utils.Extensions;
using Nae.Utils.Maths;

namespace Nae.Utils.Components
{
    /// <summary>
    /// Defines a structure that wraps an <see cref="System.Array"/>
    /// (generally multi-dimensionnal) of <typeparamref name="T"/>
    /// where the goal is to navigate through from a specic point.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CursorMatrix<T>
    {
        public delegate void MatrixMovementDelegate(T? element);

        /// <summary>
        /// Defines the default movement strategy in the matrix.
        /// </summary>
        private MatrixMovingStrategy MovingStrategy { get; set; }

        /// <summary>
        /// Matrix data structure
        /// </summary>
        protected Array Array { get; } = null!;

        /// <summary>
        /// Allows for a cancellation of a current movement order.
        /// All already made movement will be kept.
        /// </summary>
        protected bool CancelMovementRequest { get; set; }

        #region Public Properties
        /// <summary>
        /// Dimension index in which any movement will proceed
        /// </summary>
        public int MovingDimension { get; set; }

        /// <summary>
        /// Current position of the cursor
        /// </summary>
        public int[] Cursor { get; }

        /// <summary>
        /// Current value under the cursor.
        /// </summary>
        public T? Current => (T?)Array.GetValue(Cursor);
        #endregion

        #region Events
        public event MatrixMovementDelegate? OnMoved;
        #endregion

        #region Constructors
        private CursorMatrix(int rank, MatrixMovingStrategy strategy)
        {
            Cursor = new int[rank];
            MovingDimension = 0;
            MovingStrategy = strategy;
        }

        public CursorMatrix(params int[] lengths)
            : this(new MatrixMovingStrategy()
            {
                BypassCrossable = false,
                Behavior = MatrixEdgeBehavior.Throw
            }, lengths)
        {
            ResetCursor();
        }

        public CursorMatrix(MatrixMovingStrategy strategy, params int[] lengths)
            : this(lengths.Length, strategy)
        {
            Array = Array.CreateInstance(typeof(T), lengths);
            Array.Initialize();
            ResetCursor();
        }

        public CursorMatrix(int[] lengths, int[] lowerBounds)
            : this(new()
            {
                BypassCrossable = false,
                Behavior = MatrixEdgeBehavior.Throw
            }, lengths, lowerBounds)
        {
            ResetCursor();
        }

        public CursorMatrix(MatrixMovingStrategy strategy, int[] lengths, int[] lowerBounds)
            : this(lengths.Length, strategy)
        {
            Array = Array.CreateInstance(typeof(T), lengths, lowerBounds);
            Array.Initialize();
            ResetCursor();
        }
        #endregion

        public void ResetCursor()
        {
            for (int i = 0; i < Cursor.Length; i++)
            {
                Cursor[i] = Array.GetLowerBound(i);
            }
        }

        public bool IsPointWithinMatrix(int[] indicies)
        {
            return Array.IsPointWithinMatrix(indicies);
        }

        public bool IsIndexInDimension(int dimension, int idx)
        {
            return idx.IsBetweenInclusive(Array.GetLowerBound(dimension), Array.GetUpperBound(dimension));
        }

        public int GetLowerBound(int dimension) => Array.GetLowerBound(dimension);

        public int GetUpperBound(int dimension) => Array.GetUpperBound(dimension);

        #region Virtual Methods
        /// <summary>
        /// Defines a logic to know if an element is crossable.
        /// </summary>
        /// <param name="element">The element to be checked</param>
        /// <returns>
        ///     <see langword="true"/> if the element is crossable,
        ///     <see langword="false"/> otherwise.
        /// </returns>
        protected virtual bool IsCrossable(T? element) => true;

        protected virtual bool OnOverEdge(bool exceedUpderBound, int remainingDistance)
        {
            Cursor[MovingDimension] = exceedUpderBound
                ? Array.GetLowerBound(MovingDimension)
                : Array.GetUpperBound(MovingDimension);

            return true;
        }
        #endregion

        /// <summary>
        /// Sets the cursor to a specific location.
        /// </summary>
        /// <param name="indices">The positions indicies</param>
        /// <exception cref="RankException">
        ///     If provided <paramref name="indices"/> does not match this matrix rank count.
        /// </exception>
        public void SetCursor(params int[] indices)
        {
            if (indices.Length != Cursor.Length)
            {
                throw new RankException(nameof(indices));
            }

            indices.CopyTo(Cursor, 0);
        }

        public void Initialize(IEnumerable<T> values, IEnumerable<int[]> indicies)
        {
            foreach (var (Value, Index) in values.Zip(indicies))
            {
                Array.SetValue(Value, Index);
            }
        }

        #region Movement Methods
        public MovementResult Forward()
        {
            return Forward(1);
        }

        public MovementResult Forward(int distance)
        {
            return Forward(distance, MovingStrategy);
        }

        public MovementResult Forward(int distance, MatrixMovingStrategy strategy)
        {
            return Move(distance, true, strategy);
        }

        public MovementResult Backward()
        {
            return Backward(1);
        }

        public MovementResult Backward(int distance)
        {
            return Backward(distance, MovingStrategy);
        }

        public MovementResult Backward(int distance, MatrixMovingStrategy strategy)
        {
            return Move(distance, false, strategy);
        }

        private MovementResult Move(int distance, bool forward, MatrixMovingStrategy strategy)
        {
            distance = forward
                ? distance
                : -distance;

            if (strategy.Behavior == MatrixEdgeBehavior.Throw
                && Cursor[MovingDimension] + distance < Array.GetLowerBound(MovingDimension)
                && Cursor[MovingDimension] + distance > Array.GetUpperBound(MovingDimension))
            {
                throw new ArgumentOutOfRangeException(nameof(distance));
            }

            var processedMovements = 0;
            var absDistance = Math.Abs(distance);
            var directionUnit = distance / absDistance;

            for (; !CancelMovementRequest && processedMovements < absDistance; processedMovements++)
            {
                var nextIdx = Cursor[MovingDimension] + directionUnit;

                if (strategy.Behavior == MatrixEdgeBehavior.Stopping
                    && !nextIdx.IsBetweenInclusive(Array.GetLowerBound(MovingDimension), Array.GetUpperBound(MovingDimension)))
                {
                    return new(processedMovements, MovementStatus.EnconteredEdge);
                }

                var oldIdx = Cursor[MovingDimension];

                if (nextIdx < Array.GetLowerBound(MovingDimension))
                {
                    if (OnOverEdge(false, absDistance - processedMovements))
                    {
                        break;
                    }
                }
                else if (nextIdx > Array.GetUpperBound(MovingDimension))
                {
                    if (OnOverEdge(true, absDistance - processedMovements))
                    {
                        break;
                    }
                }
                else
                {
                    Cursor[MovingDimension] = nextIdx;
                }

                if (!strategy.BypassCrossable && !IsCrossable(Current))
                {
                    Cursor[MovingDimension] = oldIdx;
                    return new(absDistance, MovementStatus.EnconteredWall);
                }

                OnMoved?.Invoke(Current);
            }

            return new(processedMovements, MovementStatus.Ok);
        }
        #endregion
    }

    public class MatrixMovingStrategy
    {
        public bool BypassCrossable { get; set; }

        public MatrixEdgeBehavior Behavior { get; set; }
    }

    public struct MovementResult
    {
        public MovementResult(int processedMoves, MovementStatus result)
        {
            ProcessedMoves = processedMoves;
            Result = result;
        }

        public int ProcessedMoves { get; set; }

        public MovementStatus Result { get; set; }
    }

    public enum MovementStatus
    {
        Ok,
        EnconteredWall,
        EnconteredEdge
    }

    public enum MatrixEdgeBehavior
    {
        Throw,
        Stopping,
        Cycling,
    }
}
