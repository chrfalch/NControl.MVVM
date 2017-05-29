﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace NControl.XAnimation
{
    public abstract class XTransformationContainer : XElementContainer
    {
        #region Private Members

        /// <summary>
        /// List of animation information
        /// </summary>
        protected readonly List<XTransform> _animationInfos = new List<XTransform>();

        
        #endregion

        public XTransformationContainer(params VisualElement[] elements) : base(elements)
        {
        }

        #region Transformation Container Members

		/// <summary>
        /// Adds a new transform to the container
        /// </summary>
		public XTransform Add()
		{
			XTransform retVal = null;
			if (_animationInfos.Any())
				retVal = new XTransform(_animationInfos.Last(), true);
			else
				retVal = new XTransform();

			_animationInfos.Add(retVal);

			return retVal;
		}

        /// <summary>
        /// Adds a new transform to the container
        /// </summary>
		public XTransformationContainer Add(Action<XTransform> transformSetup)
		{
			if (transformSetup == null)
				throw new ArgumentException(nameof(transformSetup));

			XTransform retVal = null;
			if (_animationInfos.Any())
				retVal = new XTransform(_animationInfos.Last(), true);
			else
				retVal = new XTransform();

			transformSetup(retVal);

			_animationInfos.Add(retVal);

			return this;
        }

		/// <summary>
		/// Add the specified transform.
		/// </summary>
		public XTransform Set()
		{
			XTransform retVal = null;
			if (_animationInfos.Any())
				retVal = new XTransform(_animationInfos.Last(), true);
			else
				retVal = new XTransform();

			retVal.SetOnlyTransform(true);

			_animationInfos.Add(retVal);

			return retVal;
		}

		/// <summary>
		/// Add the specified transform.
		/// </summary>
		public XTransformationContainer Set(Action<XTransform> transformSetup)
		{
			if (transformSetup == null)
				throw new ArgumentException(nameof(transformSetup));

			XTransform retVal = null;
			if (_animationInfos.Any())
				retVal = new XTransform(_animationInfos.Last(), true);
			else
				retVal = new XTransform();

			retVal.SetOnlyTransform(true);
			transformSetup(retVal);

			_animationInfos.Add(retVal);

			return this;
		}

		/// <summary>
		/// Adds a reset transformation
		/// </summary>
		public XTransformationContainer Reset()
		{
			_animationInfos.Add(new XTransform(null, false));

			return this;
		}
		        
        #endregion
    }
}