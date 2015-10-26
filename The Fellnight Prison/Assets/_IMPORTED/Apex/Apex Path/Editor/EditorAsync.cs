/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.Editor
{
    using System;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Utility for executing actions asynchronously in the editor
    /// </summary>
    public static class EditorAsync
    {
        /// <summary>
        /// Executes the specified task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="callback">The callback.</param>
        public static void Execute(Action task, Action callback)
        {
            new EditorAsyncTask(task, callback).Execute();
        }

        /// <summary>
        /// Executes the specified task.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="task">The task.</param>
        /// <param name="callback">The callback.</param>
        public static void Execute<TResult>(Func<TResult> task, Action<TResult> callback)
        {
            new EditorAsyncTask<TResult>(task, callback).Execute();
        }

        private class EditorAsyncTask
        {
            private Action _task;
            private Action _callback;

            //The async result cannot be used as a reference for completion since isCompleted will be true before EndInvoke is called.
            private bool _completed;
            private Exception _error;

            internal EditorAsyncTask(Action task, Action callback)
            {
                _task = task;
                _callback = callback;
            }

            internal void Execute()
            {
                EditorApplication.update += Poll;

                _task.BeginInvoke(Complete, null);
            }

            private void Complete(IAsyncResult res)
            {
                try
                {
                    _task.EndInvoke(res);
                }
                catch (Exception e)
                {
                    _error = e;
                }

                _completed = true;
            }

            private void Poll()
            {
                if (_completed)
                {
                    EditorApplication.update -= Poll;

                    if (_error != null)
                    {
                        throw _error;
                    }

                    _callback();
                }
            }
        }

        private class EditorAsyncTask<TResult>
        {
            private Func<TResult> _task;
            private Action<TResult> _callback;

            //The async result cannot be used as a reference for completion since isCompleted will be true before EndInvoke is called.
            private bool _completed;
            private TResult _result;
            private Exception _error;

            internal EditorAsyncTask(Func<TResult> task, Action<TResult> callback)
            {
                _task = task;
                _callback = callback;
            }

            internal void Execute()
            {
                EditorApplication.update += Poll;

                _task.BeginInvoke(Complete, null);
            }

            private void Complete(IAsyncResult res)
            {
                try
                {
                    _result = _task.EndInvoke(res);
                }
                catch (Exception e)
                {
                    _error = e;
                }

                _completed = true;
            }

            private void Poll()
            {
                if (_completed)
                {
                    EditorApplication.update -= Poll;

                    if (_error != null)
                    {
                        throw _error;
                    }

                    _callback(_result);
                }
            }
        }
    }
}
