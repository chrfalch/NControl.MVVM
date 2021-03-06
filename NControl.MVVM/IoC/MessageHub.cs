﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NControl.Mvvm
{
	/// <summary>
	/// Messaging service.
	/// </summary>
	public class MessageHub: IMessageHub
	{
		#region Private Members

		/// <summary>
		/// The subscriber lock.
		/// </summary>
		private readonly object _subscriberLock = new object();

		/// <summary>
		/// The subscribers.
		/// </summary>
		private readonly Dictionary<Type, List<Subscriber>> _subscribers = new Dictionary<Type, List<Subscriber>>();

		#endregion

		#region IMessagingService implementation

		/// <summary>
		/// Publishes a message the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="message">Message.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		/// <typeparam name="TMessageType">The 1st type parameter.</typeparam>
		public void Publish<TMessageType>(TMessageType message) where TMessageType : class
		{
			if (!_subscribers.ContainsKey(typeof(TMessageType)))
				return;

			var list = _subscribers[typeof(TMessageType)].ToArray();
			foreach (var subscriber in list)
			{
				if (subscriber.IsAlive)
				{
					if (subscriber is Subscriber<object>)
						(subscriber as Subscriber<object>).Action(message);
					else
						(subscriber as Subscriber<TMessageType>).Action(message);
				}
			}
		}

		/// <summary>
		/// Subscribe the specified messageType, subscriber and message.
		/// </summary>
		/// <param name="messageType">Message type.</param>
		/// <param name="subscriber">Subscriber.</param>
		/// <param name="callback">Message.</param>
		public void Subscribe(Type messageType, object subscriber, Action<object> callback)
		{
			RemoveDeadReferences();

			if (!_subscribers.ContainsKey(messageType))
				lock (_subscriberLock)
					_subscribers.Add(messageType, new List<Subscriber>());

			var list = _subscribers[messageType];
			if (list.Any(m => m.TheSubscriber == subscriber))
				return;

			list.Add(new Subscriber<object>(subscriber, callback));
		}

		/// <summary>
		/// Subscribe the specified subscriber and message.
		/// </summary>
		/// <param name="subscriber">Subscriber.</param>
		/// <typeparam name="TMessageType">The 1st type parameter.</typeparam>
		public void Subscribe<TMessageType>(object subscriber, Action<TMessageType> callback) where TMessageType : class
		{
			RemoveDeadReferences();

			if (!_subscribers.ContainsKey(typeof(TMessageType)))
				lock(_subscriberLock)
					_subscribers.Add(typeof(TMessageType), new List<Subscriber>());

			var list = _subscribers[typeof(TMessageType)];
			if (list.Any(m => m.TheSubscriber == subscriber))
				return;

			list.Add(new Subscriber<TMessageType>(subscriber, callback));
		}

		/// <summary>
		/// Unsubscribe the specified messageType and subscriber.
		/// </summary>
		/// <param name="messageType">Message type.</param>
		/// <param name="subscriber">Subscriber.</param>
		public void Unsubscribe(Type messageType, object subscriber)
		{
			if (!_subscribers.ContainsKey(messageType))
				return;

			var list = _subscribers[messageType];
			var subscriberObj = list.FirstOrDefault(sub => sub.TheSubscriber == subscriber);
			if (subscriberObj != null)
				lock (_subscriberLock)
					list.Remove(subscriberObj);
		}

		/// <summary>
		/// Unsubscribe the specified subscriber.
		/// </summary>
		/// <param name="subscriber">Subscriber.</param>
		/// <typeparam name="TMessageType">The 1st type parameter.</typeparam>
		public void Unsubscribe<TMessageType>(object subscriber) where TMessageType : class
		{
			if (!_subscribers.ContainsKey(typeof(TMessageType)))
				return;

			var list = _subscribers[typeof(TMessageType)];
			var subscriberObj = list.FirstOrDefault(sub => sub.TheSubscriber == subscriber);
			if(subscriberObj != null)
				lock(_subscriberLock)
					list.Remove(subscriberObj);
		}

		#endregion


		#region Private Members

		/// <summary>
		/// Removes the dead references.
		/// </summary>
		private void RemoveDeadReferences()
		{
			var keysToRemove = new List<Type>();

			foreach (var key in _subscribers.Keys)
			{
				var listOfSubscribers = _subscribers[key];
				var listOfSubscribersToRemove = new List<Subscriber>();
				foreach (var item in listOfSubscribers)
				{
					if (!item.IsAlive)
						listOfSubscribersToRemove.Add(item);                    
				}

				foreach (var subscriber in listOfSubscribersToRemove)
					listOfSubscribers.Remove(subscriber);

				if (!listOfSubscribers.Any())
					keysToRemove.Add(key);
			}

			if(keysToRemove.Any())
				//lock(_subscriberLock)
				foreach (var key in keysToRemove)
					_subscribers.Remove(key);
		}
		#endregion
	}

	/// <summary>
	/// Subscriber.
	/// </summary>
	internal class Subscriber
	{
		#region Private Members

		/// <summary>
		/// The subscriber.
		/// </summary>
		private readonly WeakReference _subscriberReference;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="Sin4U.Data.Services.Subscriber"/> class.
		/// </summary>
		/// <param name="subscriber">Subscriber.</param>
		public Subscriber(object subscriber)
		{
			_subscriberReference = new WeakReference(subscriber);
		}

		/// <summary>
		/// Gets or sets the subscriber.
		/// </summary>
		/// <value>The subscriber.</value>
		public object TheSubscriber 
		{
			get{ return _subscriberReference.Target; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is alive.
		/// </summary>
		/// <value><c>true</c> if this instance is alive; otherwise, <c>false</c>.</value>
		public bool IsAlive { get { return _subscriberReference.IsAlive; } } 
	}

	/// <summary>
	/// Subscriber.
	/// </summary>
	internal class Subscriber<TMessageType>: Subscriber where TMessageType : class
	{
		/// <summary>
		/// Gets or sets the action.
		/// </summary>
		/// <value>The action.</value>
		public Action<TMessageType> Action {get; private set;}

		/// <summary>
		/// Initializes a new instance of the <see cref="Sin4U.Data.Services.Subscriber"/> class.
		/// </summary>
		/// <param name="subscriber">Subscriber.</param>
		public Subscriber(object subscriber, Action<TMessageType> action): base(subscriber)
		{            
			Action = action;
		}
	}
}

