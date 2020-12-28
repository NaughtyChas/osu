// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;

namespace osu.Game.Screens.OnlinePlay
{
    /// <summary>
    /// Utility class to track ongoing online operations' progress.
    /// Can be used to disable interactivity while waiting for a response from online sources.
    /// </summary>
    public class OngoingOperationTracker
    {
        /// <summary>
        /// Whether there is an online operation in progress.
        /// </summary>
        public IBindable<bool> InProgress => inProgress;

        private readonly Bindable<bool> inProgress = new BindableBool();

        private LeasedBindable<bool> leasedInProgress;

        /// <summary>
        /// Begins tracking a new online operation.
        /// </summary>
        /// <exception cref="InvalidOperationException">An operation has already been started.</exception>
        public void BeginOperation()
        {
            if (leasedInProgress != null)
                throw new InvalidOperationException("Cannot begin operation while another is in progress.");

            leasedInProgress = inProgress.BeginLease(true);
            leasedInProgress.Value = true;
        }

        /// <summary>
        /// Ends tracking an online operation.
        /// Does nothing if an operation has not been begun yet.
        /// </summary>
        public void EndOperation()
        {
            leasedInProgress?.Return();
            leasedInProgress = null;
        }
    }
}
