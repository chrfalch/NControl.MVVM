﻿using System;
using System.Collections.Generic;
using NControl.XAnimation;
using Xamarin.Forms;

namespace NControl.Mvvm
{
	public abstract class BaseFluidMenuView<TViewModel> : BaseFluidContentsView<TViewModel>
		where TViewModel : BaseViewModel
	{
		ContentView _contentView;
		ContentView _headerView;
		ContentView _footerView;

		public BaseFluidMenuView()
		{
			NavigationPage.SetHasNavigationBar(this, false);
		}

		protected override View CreateContents()
		{
			BackgroundColor = Color.Transparent;


			_headerView = new ContentView { BackgroundColor = Color.Aqua };
			_contentView = new ContentView
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			_footerView = new ContentView { BackgroundColor = Color.Lime };

			SetupMenuView();

			return new VerticalStackLayout
			{
				Children = {
					_headerView,
					_contentView,
					_footerView,
				}
			};
		}

		protected abstract void SetupMenuView();

		#region Public Members

		public View MenuContent
		{
			get { return _contentView.Content; }
			set { _contentView.Content = value; }
		}

		public View Header
		{
			get { return _headerView.Content; }
			set { _headerView.Content = value; }
		}

		public View Footer
		{
			get { return _footerView.Content; }
			set { _footerView.Content = value; }
		}

		#endregion

		#region Transition

		protected override IEnumerable<XAnimationPackage> ModalTransitionIn(
			INavigationContainer container, IEnumerable<XAnimationPackage> animations)
		{
			return new[] {

				// Slide in
				new XAnimationPackage(_contentView)
					.Translate(-Width, 0)
					.Rotate(-15)
					.Set()
					.Translate(0, 0)
					.Rotate(0)
					.Animate(),

					// Set background color

			};
		}

		protected override IEnumerable<XAnimationPackage> ModalTransitionOut(
			INavigationContainer container, IEnumerable<XAnimationPackage> animations)
		{
			return new[] {
				new XAnimationPackage(_contentView)
					.Translate(-Width, 0)
					.Rotate(-15)
					.Animate(),
			};
		}
		#endregion

	}
}