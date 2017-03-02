﻿using System;
using Xamarin.Forms;

namespace NControl.Mvvm.Fluid
{
	public class FluidToolbarControl: ContentView
	{
		public FluidToolbarControl(View innerControl)
		{
			WidthRequest = MvvmApp.Current.Sizes.Get(FluidConfig.DefaultToolbarItemWidth);
			Content = innerControl;
		}
	}
}
