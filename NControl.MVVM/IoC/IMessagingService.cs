﻿using System;

namespace NControl.MVVM
{
	/// <summary>
	/// I messaging center.
	/// </summary>
	public interface IMessagingService
	{
		/// <summary>
		/// Publishes a message the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="message">Message.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		void Publish<TMessageType>(TMessageType message) where TMessageType: class;

		/// <summary>
		/// Subscribe the specified subscriber and message.
		/// </summary>
		/// <param name="subscriber">Subscriber.</param>
		/// <param name="message">Message.</param>
		/// <typeparam name="TMessageType">The 1st type parameter.</typeparam>
		void Subscribe<TMessageType>(object subscriber, Action<TMessageType> message) where TMessageType: class;

		/// <summary>
		/// Unsubscribe the specified subscriber.
		/// </summary>
		/// <param name="subscriber">Subscriber.</param>
		/// <typeparam name="TMessageType">The 1st type parameter.</typeparam>
		void Unsubscribe<TMessageType>(object subscriber) where TMessageType: class;
	}
}

