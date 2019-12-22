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

    /// <summary>
    /// Provides features for transaction handling.
    /// </summary>
    internal class Transaction
    {
        private readonly SemaphoreSlim semaphore;
        private readonly ConcurrentStack<Action> actions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        internal Transaction()
        {
            this.semaphore = new SemaphoreSlim(1);
            this.actions = new ConcurrentStack<Action>();
        }

        /// <summary>
        /// Gets a value indicating whether the transaction is running.
        /// </summary>
        internal bool Running { get; private set; }

        /// <summary>
        /// Gets Undo operations for Rollback.
        /// </summary>
        internal ConcurrentStack<Action> Actions => this.actions;

        /// <summary>
        /// Begins an IO transaction.
        /// </summary>
        internal void BeginTransaction()
        {
            this.semaphore.Wait();
            if (this.Running)
            {
                this.semaphore.Release();
                throw new InvalidOperationException("Transaction has already been begun!");
            }
            else
            {
                this.Running = true;
                this.semaphore.Release();
            }
        }

        /// <summary>
        /// Commits all changes.
        /// </summary>
        internal void Commit()
        {
            this.semaphore.Wait();
            if (this.Actions == null)
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
            this.semaphore.Wait();
            if (this.Actions == null)
            {
                this.semaphore.Release();
                throw new InvalidOperationException("Transaction has to be began first!");
            }

            while (!this.Actions.IsEmpty)
            {
                if (this.Actions.TryPop(out Action action))
                {
                    action?.Invoke();
                }
            }

            this.End();
            this.semaphore.Release();
        }

        private void End()
        {
            this.Running = false;
            this.Actions.Clear();
        }
    }
}
