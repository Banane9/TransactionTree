using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionTree
{
    /// <summary>
    /// Contains expansion methods for the <see cref="ITransaction{T}"/> interface.
    /// </summary>
    public static class Transaction
    {
        /// <summary>
        /// Determines whether the transaction is the last one in its chain.
        /// </summary>
        /// <typeparam name="T">The type that the transaction can be applied to.</typeparam>
        /// <param name="transaction">The transaction to check.</param>
        /// <returns>Whether the transaction is the last one in its chain.</returns>
        public static bool IsEnd<T>(this ITransaction<T> transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction), "Transaction can't be null!");

            return transaction.Next.Count == 0;
        }

        /// <summary>
        /// Determines whether the transaction is the first one in its chain.
        /// </summary>
        /// <typeparam name="T">The type that the transaction can be applied to.</typeparam>
        /// <param name="transaction">The transaction to check.</param>
        /// <returns>Whether the transaction is the first one in its chain.</returns>
        public static bool IsStart<T>(this ITransaction<T> transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction), "Transaction can't be null!");

            return transaction.Previous == null;
        }

        /// <summary>
        /// Moves this transaction and all the ones after it behind the other transaction.
        /// </summary>
        /// <typeparam name="T">The type that the transaction can be applied to.</typeparam>
        /// <param name="transaction">The first transaction to move.</param>
        /// <param name="otherTransaction">The transaction to move it behind.</param>
        public static void MoveAllBehind<T>(this ITransaction<T> transaction, ITransaction<T> otherTransaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction), "Transaction can't be null!");

            transaction.Previous?.Next.Remove(transaction);
            transaction.Previous = otherTransaction;

            otherTransaction?.Next.Add(transaction);
        }

        /// <summary>
        /// Moves this transaction out of the chain and behind the other transaction.
        /// <para/>
        /// The ones previously behind this transaction will then be behind the one before it.
        /// </summary>
        /// <typeparam name="T">The type that the transaction can be applied to.</typeparam>
        /// <param name="transaction">The transaction to move.</param>
        /// <param name="otherTransaction">The transaction to move it behind.</param>
        public static void MoveBehind<T>(this ITransaction<T> transaction, ITransaction<T> otherTransaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction), "Transaction can't be null!");

            transaction.Previous?.Next.Remove(transaction);
            foreach (var trans in transaction.Next)
            {
                transaction.Previous?.Next.Add(trans);
                trans.Previous = transaction.Previous;
            }

            transaction.Previous = otherTransaction;

            otherTransaction?.Next.Add(transaction);
        }

        /// <summary>
        /// Applies this transaction and all the ones after it to the given state, returning all the possible end states.
        /// </summary>
        /// <typeparam name="T">The type that the transaction can be applied to.</typeparam>
        /// <param name="transaction">The transaction to start from.</param>
        /// <param name="state">The starting state.</param>
        /// <returns>All the possible end states.</returns>
        public static IEnumerable<T> ApplyChain<T>(this ITransaction<T> transaction, T state)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction), "Transaction can't be null!");

            var newState = transaction.Apply(state);

            if (transaction.Next.Count > 0)
            {
                foreach (var nextTrans in transaction.Next)
                {
                    foreach (var endState in nextTrans.ApplyChain(newState))
                        yield return endState;
                }

                yield break;
            }

            yield return newState;
        }
    }
}