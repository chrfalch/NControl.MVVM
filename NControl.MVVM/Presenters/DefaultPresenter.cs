﻿/****************************** Module Header ******************************\
Module Name:  DefaultPresenter.cs
Copyright (c) Christian Falch
All rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using System.Reflection;

namespace NControl.Mvvm
{
	/// <summary>
	/// Default presenter.
	/// </summary>
	public class DefaultPresenter: IPresenter
	{
		#region Private Members

		/// <summary>
		/// The navigation page stack.
		/// </summary>
		private Stack<NavigationElement> _navigationPageStack = new Stack<NavigationElement>();

		/// <summary>
		/// The presented card.
		/// </summary>
		private Stack<BaseCardPageView> _presentedCardStack = new Stack<BaseCardPageView>();

		/// <summary>
		/// The master detail page.
		/// </summary>
		private MasterDetailPage _masterDetailPage;

		/// <summary>
		/// The using master as navigation stack.
		/// </summary>
		private bool _usingMasterAsNavigationStack;

		#endregion

		#region IPresenter implementation

		/// <summary>
		/// Gets the main page.
		/// </summary>
		/// <returns>The main page.</returns>
		/// <param name="mainPage">Main page.</param>
		public Page SetMainPage(Page mainPage)
		{
			Application.Current.MainPage = mainPage;

			if (_masterDetailPage == null)
			{
				if (_navigationPageStack.Any())
					_navigationPageStack.Pop();

				_navigationPageStack.Push(new NavigationElement { Page = mainPage });

				// Is mainpage a navigation page?
				var navPage = mainPage as NavigationPage;
				if (navPage != null)
					navPage.Popped += NavPage_Popped;

				return _navigationPageStack.Peek().Page;
			}
			else
			{
				if(_masterDetailPage.Detail is NavigationPage)					
					(_masterDetailPage.Detail as NavigationPage).Popped += NavPage_Popped;

				if (_masterDetailPage.Master is NavigationPage)
					(_masterDetailPage.Master as NavigationPage).Popped += NavPage_Popped;
			}

			return mainPage;
		}

		/// <summary>
		/// Sets the master detail master.
		/// </summary>
		/// <param name="page">Page.</param>
		public void SetMasterDetailMaster(MasterDetailPage page, bool useMasterAsNavigationStack = false)
		{
			_masterDetailPage = page;

			if(_navigationPageStack.Any())
				_navigationPageStack.Pop();

			_usingMasterAsNavigationStack = useMasterAsNavigationStack;
			_navigationPageStack.Push(new NavigationElement{Page = useMasterAsNavigationStack ? page.Master : page.Detail});
		}

		/// <summary>
		/// Toggles the drawer.
		/// </summary>
		public void ToggleDrawer()
		{
			if (_masterDetailPage == null)
				return;

			_masterDetailPage.IsPresented = !_masterDetailPage.IsPresented;
		}

		#region Dismissing ViewModels

		/// <summary>
		/// Dismisses the view model async.
		/// </summary>
		/// <returns>The view model async.</returns>
		/// <param name="presentationMode">Presentation model.</param>
		public Task DismissViewModelAsync(PresentationMode presentationMode)
		{
			return DismissViewModelAsync (presentationMode, true);
		}

		/// <summary>
		/// Dismisses the view model async.
		/// </summary>
		/// <returns>The view model async.</returns>
		/// <param name="presentationMode">Presentation mode</param>
		/// <param name="success">If set to <c>true</c> success.</param>
		public Task DismissViewModelAsync(PresentationMode presentationMode, bool success)
		{
			if (presentationMode == PresentationMode.Default)
				return PopViewModelAsync();
			
			if (presentationMode == PresentationMode.Modal)
				return PopModalViewModelAsync(success);
			
			if (presentationMode == PresentationMode.Popup)
				return PopCardViewModelAsync();

			throw new InvalidOperationException("Could not pop presentation mode " + presentationMode);
		}

		/// <summary>
		/// Pops the view model async.
		/// </summary>
		/// <returns>The view model async.</returns>
		async Task PopViewModelAsync()
		{			                
			if (await _navigationPageStack.Peek().Page.Navigation.PopAsync() == null)
				_navigationPageStack.Pop();
		}

		#endregion

		#region Dialogs

		/// <summary>
		/// Shows the message async.
		/// </summary>
		/// <returns>The message async.</returns>
		/// <param name="title">Title.</param>
		/// <param name="message">Message.</param>
		public async Task<bool> ShowMessageAsync(string title, string message, string accept, string cancel)
		{
			var page = _navigationPageStack.Peek ().Page;
			if (cancel == null) {
				await page.DisplayAlert (title, message, accept ?? "OK");
				return true;
			}
			
			return await page.DisplayAlert (title, message, accept ?? "OK", cancel ?? "Cancel");
		}

		/// <summary>
		/// Shows the action sheet.
		/// </summary>
		/// <returns>The action sheet.</returns>
		/// <param name="title">Title.</param>
		/// <param name="cancel">Cancel.</param>
		/// <param name="destruction">Destruction.</param>
		/// <param name="buttons">Buttons.</param>
		public Task<string> ShowActionSheet(string title, string cancel, string destruction, params string[] buttons)
		{
			var page = _navigationPageStack.Peek ().Page;
			return page.DisplayActionSheet (title, cancel, destruction, buttons);
		}
		#endregion

		#region Regular Navigation

		/// <summary>
		/// Navigates to the provided view model of type
		/// </summary>
		/// <typeparam name="TViewModel">The 1st type parameter.</typeparam>
		public Task ShowViewModelAsync<TViewModel>(object parameter = null, bool animate = true)
			where TViewModel : BaseViewModel
		{
			return ShowViewModelAsync(typeof(TViewModel), parameter, animate);
		}

		/// <summary>
		/// Navigates to the provided view model of type
		/// </summary>
		/// <typeparam name="TViewModel">The 1st type parameter.</typeparam>
		public async Task ShowViewModelAsync(Type viewModelType, object parameter = null, bool animate = true)			
		{       
			if (_masterDetailPage != null && !_usingMasterAsNavigationStack)
				_masterDetailPage.IsPresented = false;

			var view = MvvmApp.Current.ViewContainer.GetViewFromViewModel(viewModelType);

			var viewModelProvider = view as IView;
			if (viewModelProvider == null)
				throw new ArgumentException ("Could not get viewmodel from view. View does not implement IView<T>.");

			viewModelProvider.GetViewModel().PresentationMode = PresentationMode.Default;

			if (parameter != null)
			{				
				var bt = viewModelType.GetTypeInfo().BaseType;
				var paramType = parameter.GetType ();
				var met = bt.GetRuntimeMethod ("InitializeAsync", new Type[]{paramType});
				if(met != null)
				{
					met.Invoke (viewModelProvider.GetViewModel(), new object[]{ parameter });
				}
			}

			// Should we present this on its own navigation stack?
			await _navigationPageStack.Peek().Page.Navigation.PushAsync (view, animate);
		}

		#endregion

		#region Modal Navigation

		/// <summary>
		/// Navigates to the provided view model of type
		/// </summary>
		/// <typeparam name="TViewModel">The 1st type parameter.</typeparam>
		public Task<NavigationPage> ShowViewModelModalAsync<TViewModel>(
			Action<bool> dismissedCallback = null, object parameter = null, bool animate = false)
			where TViewModel : BaseViewModel
		{
			return ShowViewModelModalAsync(typeof(TViewModel), dismissedCallback, parameter, animate);
		}

		/// <summary>
		/// Shows the view model modal async.
		/// </summary>
		/// <returns>The view model modal async.</returns>
		/// <param name="dismissedCallback">Dismissed callback.</param>
		/// <param name="parameter">Parameter.</param>
		/// <typeparam name="TViewModel">The 1st type parameter.</typeparam>
		public async Task<NavigationPage> ShowViewModelModalAsync(Type viewModelType,
			Action<bool> dismissedCallback = null, object parameter = null, bool animate = false)
		{       
			if (_masterDetailPage != null)
				_masterDetailPage.IsPresented = false;

			var view = MvvmApp.Current.ViewContainer.GetViewFromViewModel(viewModelType);

			var viewModelProvider = view as IView;
			if (viewModelProvider == null)
				throw new ArgumentException ("Could not get viewmodel from view. View does not implement IView<T>.");

			viewModelProvider.GetViewModel().PresentationMode = PresentationMode.Modal;

			// Create wrapper page
			var retVal = new ModalNavigationPage (view, viewModelProvider.GetViewModel() as BaseViewModel);
			retVal.Popped += NavPage_Popped;

			if (parameter != null)
			{
				var bt = viewModelType.GetTypeInfo().BaseType;
				var paramType = parameter.GetType ();
				var met = bt.GetRuntimeMethod ("InitializeAsync", new Type[]{paramType});
				if(met != null)
				{
					met.Invoke (viewModelProvider.GetViewModel(), new object[]{ parameter });
				}
			}

			await _navigationPageStack.Peek().Page.Navigation.PushModalAsync (retVal);

			_navigationPageStack.Push(new NavigationElement{
				Page = retVal, 
				DismissedAction = dismissedCallback,
			});

			return retVal;
		}

		/// <summary>
		/// Pops the active modal view 
		/// </summary>
		/// <returns>The modal view model async.</returns>
		public async Task PopModalViewModelAsync(bool success)
		{
			var poppedPage = await _navigationPageStack.Peek().Page.Navigation.PopModalAsync ();
			var navPage = poppedPage as ModalNavigationPage;

			if(navPage != null)				
			{
				// Dismiss all children
				if (navPage.CurrentPage != null)
				{
					var viewModelProvider = navPage.CurrentPage as IView;
					try
					{
						viewModelProvider.GetViewModel().ViewModelDismissed();		
					}
					catch (Exception ex)
					{
						System.Diagnostics.Debug.WriteLine("MVVM: An exception occured in your ViewModelDismissed method in your " + 
							viewModelProvider.GetType().Name + " view:\n" + ex.Message);
					}
				}
			}

			if(poppedPage == _navigationPageStack.Peek().Page)
			{
				var tempNavigationElement = _navigationPageStack.Peek ();
				_navigationPageStack.Pop ();
				if (tempNavigationElement.DismissedAction != null)
					tempNavigationElement.DismissedAction (success);
			}
		}

		#endregion

		#region Card Navigation

		/// <summary>
		/// Shows the view model as popup async.
		/// </summary>
		/// <returns>The view model as popup async.</returns>
		/// <param name="parameter">Parameter.</param>
		public Task ShowViewModelAsPopupAsync<TViewModel>(object parameter)
			where TViewModel : BaseViewModel
		{
			return ShowViewModelAsPopupAsync(typeof(TViewModel), parameter);
		}

		/// <summary>
		/// Navigates to card view model async.
		/// </summary>
		/// <returns>The to card view model async.</returns>
		/// <typeparam name="TViewModel">The 1st type parameter.</typeparam>
		public async Task ShowViewModelAsPopupAsync(Type viewModelType, object parameter)			
		{
			if (_masterDetailPage != null)
				_masterDetailPage.IsPresented = false;

			var view = (BaseCardPageView)MvvmApp.Current.ViewContainer.GetViewFromViewModel(viewModelType);

			var viewModelProvider = view as IView;
			if (viewModelProvider == null)
				throw new ArgumentException ("Could not get viewmodel from view. View does not implement IView<T>.");

			viewModelProvider.GetViewModel().PresentationMode = PresentationMode.Popup;

			if (parameter != null)
			{
				var bt = viewModelType.GetTypeInfo().BaseType;
				var paramType = parameter.GetType ();
				var met = bt.GetRuntimeMethod ("InitializeAsync", new Type[]{paramType});
				if(met != null)
				{
					met.Invoke (viewModelProvider.GetViewModel(), new object[]{ parameter });
				}
			}

			_presentedCardStack.Push(view);
			await view.ShowAsync();
		}

		/// <summary>
		/// Pops the card view model async.
		/// </summary>
		/// <returns>The card view model async.</returns>
		public async Task PopCardViewModelAsync()
		{
			if (!_presentedCardStack.Any())
				return;

			var card = _presentedCardStack.Pop();
			await card.BaseCloseAsync();

		}
		#endregion

		#endregion

		#region Event Handlers

		/// <summary>
		/// Handles popping in navigation pages
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void NavPage_Popped(object sender, NavigationEventArgs e)
		{
			var viewModelProvider = e.Page as IView;
			if (viewModelProvider != null)
			{
				viewModelProvider.GetViewModel().ViewModelDismissed();
			}
		}

		#endregion
	}

	/// <summary>
	/// Navigation Element Helper 
	/// </summary>
	internal class NavigationElement
	{
		public Page Page {get;set;}
		public Action<bool> DismissedAction { get; set; }
	}
}

