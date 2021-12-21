using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace MovieHunt.Utility
{
    /// <summary>
    /// Counts running operation, maintain boolean value indicating
    /// whether any operation is running, and notifies about this indicator changes.
    /// </summary>
    public class OperationsCounter
    {
        private readonly Action<bool> _isRunningChanged;
        private readonly ISubject<int> _depthChanges;
        private readonly ISubject<int> _changesSummary;

        public OperationsCounter(Action<bool> isRunningChanged)
            : this(isRunningChanged, null)
        {
        }

        public OperationsCounter(Action<bool> isRunningChanged, OperationsCounter related)
        {
            _isRunningChanged = isRunningChanged;

            _depthChanges = new BehaviorSubject<int>(0);
            _changesSummary = new BehaviorSubject<int>(0);

            var relatedObservable = related?._changesSummary ?? Observable.Return(0);

            _depthChanges
                .Scan((s, v) => v + s)
                .CombineLatest(relatedObservable, (own, rel) => own + rel)
                .Subscribe(_changesSummary);

            _changesSummary
                .Select(v => v > 0)
                .DistinctUntilChanged()
                .Subscribe(ChangeIsRunningSafe);
        }

        /// <summary>
        /// Increments operations counter.
        /// Retuned disposable should be disposed when operation was finished.
        /// </summary>
        public IDisposable Run()
        {
            _depthChanges.OnNext(1);
            return Disposable.Create(DecrementDepth);
        }

        private void DecrementDepth()
        {
            _depthChanges.OnNext(-1);
        }

        private void ChangeIsRunningSafe(bool isBusy)
        {
            RunSuppressingExceptions(() => _isRunningChanged?.Invoke(isBusy));
        }

        private static void RunSuppressingExceptions(Action action)
        {
            try
            {
                action();
            }
            catch
            {
                // Do nothing.
            }
        }
    }
}