namespace Nae.Utils.Components
{
    /// <summary>
    /// Defines a structure that wraps an <see cref="Array"/>
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
        private readonly Array _array = null!;

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
        private int[] Cursor { get; }

        /// <summary>
        /// Current value under the cursor.
        /// </summary>
        public T? Current => (T?)_array.GetValue(Cursor);
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
        }

        public CursorMatrix(MatrixMovingStrategy strategy, params int[] lengths)
            : this(lengths.Length, strategy)
        {
            _array = Array.CreateInstance(typeof(T), lengths);
            _array.Initialize();
        }

        public CursorMatrix(int[] lengths, int[] lowerBounds)
            : this(new()
            {
                BypassCrossable = false,
                Behavior = MatrixEdgeBehavior.Throw
            }, lengths, lowerBounds)
        {
        }

        public CursorMatrix(MatrixMovingStrategy strategy, int[] lengths, int[] lowerBounds)
            : this(lengths.Length, strategy)
        {
            _array = Array.CreateInstance(typeof(T), lengths, lowerBounds);
            _array.Initialize();
        }
        #endregion

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
                ? _array.GetLowerBound(MovingDimension)
                : _array.GetUpperBound(MovingDimension);

            return true;
        }
        #endregion

        /// <summary>
        /// Sets the cursor to a specific location.
        /// </summary>
        /// <param name="indices">The positions indicies</param>
        /// <returns>The element under the cursor</returns>
        /// <exception cref="RankException">
        ///     If provided <paramref name="indices"/> does not match this matrix rank count.
        /// </exception>
        public T? SetCursor(params int[] indices)
        {
            if (indices.Length != Cursor.Length)
            {
                throw new RankException(nameof(indices));
            }

            indices.CopyTo(Cursor, 0);

            return Current;
        }

        #region Movement Methods
        public int Forward(int distance)
        {
            return Forward(distance, MovingStrategy);
        }

        public int Forward(int distance, MatrixMovingStrategy strategy)
        {
            return Move(distance, true, strategy);
        }

        public int Backward(int distance)
        {
            return Backward(distance, MovingStrategy);
        }

        public int Backward(int distance, MatrixMovingStrategy strategy)
        {
            return Move(distance, false, strategy);
        }

        private int Move(int distance, bool forward, MatrixMovingStrategy strategy)
        {
            distance = forward
                ? distance
                : -distance;

            if (strategy.Behavior == MatrixEdgeBehavior.Throw
                && Cursor[MovingDimension] + distance < _array.GetLowerBound(MovingDimension)
                && Cursor[MovingDimension] + distance > _array.GetUpperBound(MovingDimension))
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
                    && nextIdx < _array.GetLowerBound(MovingDimension)
                    && nextIdx > _array.GetUpperBound(MovingDimension))
                {
                    break;
                }

                var oldIdx = Cursor[MovingDimension];

                if (nextIdx < _array.GetLowerBound(MovingDimension))
                {
                    if (OnOverEdge(false, absDistance - processedMovements))
                    {
                        break;
                    }
                }
                else if (nextIdx > _array.GetUpperBound(MovingDimension))
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

                if (!IsCrossable(Current))
                {
                    Cursor[MovingDimension] = oldIdx;
                    break;
                }

                OnMoved?.Invoke(Current);
            }

            return processedMovements;
        }
        #endregion
    }

    public class MatrixMovingStrategy
    {
        public bool BypassCrossable { get; set; }

        public MatrixEdgeBehavior Behavior { get; set; }
    }

    public enum MatrixEdgeBehavior
    {
        Throw,
        Stopping,
        Cycling,
    }
}
