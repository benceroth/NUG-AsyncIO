// <copyright file="Transaction.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AsyncIO.Core
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Provides features for transaction handling.
    /// </summary>
    internal class Transaction
    {
        private readonly ILogger logger;
        private readonly SemaphoreSlim semaphore;
        private readonly ConcurrentStack<Action> actions;

        private bool running;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="logger">Injectable logger.</param>
        internal Transaction(ILogger logger)
        {
            this.logger = logger;
            this.semaphore = new SemaphoreSlim(1);
            this.actions = new ConcurrentStack<Action>();
        }

        /// <summary>
        /// Begins an IO transaction.
        /// </summary>
        internal void BeginTransaction()
        {
            this.logger?.LogInformation($"#{this.GetHashCode()}: Begin transaction");
            this.semaphore.Wait();
            if (this.running)
            {
                this.semaphore.Release();
                throw new InvalidOperationException("Transaction has already been begun!");
            }
            else
            {
                this.running = true;
                this.semaphore.Release();
            }
        }

        /// <summary>
        /// Commits all changes.
        /// </summary>
        internal void Commit()
        {
            this.logger?.LogInformation($"#{this.GetHashCode()}: Commit transaction");
            this.semaphore.Wait();
            if (this.actions == null)
            {
                this.semaphore.Release();
                throw new InvalidOperationException("Transaction has to be began first!");
            }

            this.End();
            this.semaphore.Release();
        }

        /// <summary>
        /// Rollbacks all changes.
        /// </summary>
        internal void Rollback()
        {
            this.logger?.LogInformation($"#{this.GetHashCode()}: Rollback transaction");
            this.semaphore.Wait();
            if (this.actions == null)
            {
                this.semaphore.Release();
                throw new InvalidOperationException("Transaction has to be began first!");
            }

            while (!this.actions.IsEmpty)
            {
                if (this.actions.TryPop(out Action action))
                {
                    action?.Invoke();
                }
            }

            this.End();
            this.semaphore.Release();
        }

        /// <summary>
        /// Pushes an undo operation to rollback stack.
        /// </summary>
        /// <param name="action">Undo operation.</param>
        /// <param name="caller">Caller operation name.</param>
        internal void PushUndoAction(Action action, string caller)
        {
            if (this.running)
            {
                this.logger?.LogDebug($"#{this.GetHashCode()}: Added undo operation for {caller}");
                this.actions.Push(action);
            }
            else
            {
                this.logger?.LogDebug($"#{this.GetHashCode()}: Running {caller} operation without transaction.");
            }
        }

        private void End()
        {
            this.running = false;
            this.actions.Clear();
        }
    }
}
